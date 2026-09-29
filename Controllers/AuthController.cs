using FormManagementSystem.DTOs.Auth;
using FormManagementSystem.Extensions;
using FormManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FormManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly IUserService _userService;

    public AuthController(
            IAuthService service,
            IUserService userService)
    {
        _service = service;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto dto)
    {
        var result = await _service.LoginAsync(dto);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
    RefreshTokenRequestDto dto)
    {
        var result = await _service.RefreshAsync(dto);

        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
    RefreshTokenRequestDto dto)
    {
        var result = await _service.LogoutAsync(dto);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordRequestDto dto)
    {
        var publicId = User.GetPublicId();

        if (publicId is not { } currentPublicId)
        {
            return Unauthorized();
        }

        var userId = await _userService.GetUserIdByPublicIdAsync(
            currentPublicId);

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        await _service.ChangePasswordAsync(
            currentUserId,
            dto);

        return NoContent();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        ForgotPasswordRequestDto dto)
    {
        await _service.ForgotPasswordAsync(dto);

        return Ok(new
        {
            message = "If your email is registered, you will receive a password reset link."
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
    ResetPasswordRequestDto dto)
    {
        await _service.ResetPasswordAsync(dto);

        return Ok(new
        {
            message = "Password reset successfully."
        });
    }
}