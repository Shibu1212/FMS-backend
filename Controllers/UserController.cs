using FormManagementSystem.DTOs.Users;
using FormManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPatch("{userId:int}/role")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> ChangeRole(int userId, [FromBody] ChangeUserRoleRequestDto dto)
    {
        if (dto == null || dto.RoleId <= 0)
        {
            return BadRequest(new { message = "Invalid role ID." });
        }

        var result = await _userService.ChangeUserRoleAsync(userId, dto.RoleId);

        return result switch
        {
            ChangeRoleResult.Success => Ok(new { message = "User role updated successfully." }),
            ChangeRoleResult.UserNotFound => NotFound(new { message = "User not found." }),
            ChangeRoleResult.RoleNotFound => NotFound(new { message = "Role not found." }),
            ChangeRoleResult.InvalidRequest => BadRequest(new { message = "Invalid role ID." }),
            _ => BadRequest(new { message = "An error occurred while updating the role." })
        };
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllAsync();

        return Ok(users);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateUser(
    [FromBody] CreateUserRequestDto dto)
    {
        if (dto == null)
        {
            return BadRequest(new
            {
                message = "Invalid request."
            });
        }

        try
        {
            await _userService.CreateByAdminAsync(dto);

            return Ok(new
            {
                message = "User created successfully. Login credentials have been sent to the user's email."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut("{userId:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateUser(
    int userId,
    [FromBody] UpdateUserRequestDto dto)
    {
        if (dto == null)
        {
            return BadRequest(new
            {
                message = "Invalid request."
            });
        }

        try
        {
            var updated = await _userService.UpdateAsync(
                userId,
                dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = "User updated successfully."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPatch("{userId:int}/status")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> ChangeStatus(
    int userId,
    [FromBody] ChangeUserStatusRequestDto dto)
    {
        if (dto == null)
        {
            return BadRequest(new
            {
                message = "Invalid request."
            });
        }

        var result = await _userService.ChangeUserStatusAsync(
            userId,
            dto.IsActive);

        return result switch
        {
            ChangeUserStatusResult.Success =>
                Ok(new
                {
                    message = dto.IsActive
                        ? "User activated successfully."
                        : "User deactivated successfully."
                }),

            ChangeUserStatusResult.UserNotFound =>
                NotFound(new
                {
                    message = "User not found."
                }),

            ChangeUserStatusResult.InvalidRequest =>
                BadRequest(new
                {
                    message = "Invalid request."
                }),

            _ => BadRequest(new
            {
                message = "An error occurred while changing user status."
            })
        };
    }

    [HttpDelete("{userId:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        try
        {
            var deleted = await _userService.DeleteAsync(userId);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = "User deleted successfully."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}
