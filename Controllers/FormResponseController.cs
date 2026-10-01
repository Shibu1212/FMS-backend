using FormManagementSystem.DTOs.FormResponse;
using FormManagementSystem.DTOs.Common;
using FormManagementSystem.Extensions;
using FormManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FormResponseController : ControllerBase
{
    private readonly IFormResponseService _formResponseService;
    private readonly IUserService _userService;

    public FormResponseController(
        IFormResponseService formResponseService,
        IUserService userService)
    {
        _formResponseService = formResponseService;
        _userService = userService;
    }

    private async Task<int?> GetCurrentUserIdAsync()
    {
        var publicId = User.GetPublicId();

        if (publicId is not { } currentPublicId)
        {
            return null;
        }

        return await _userService.GetUserIdByPublicIdAsync(
            currentPublicId);
    }

    [HttpPost]
    [Authorize(Roles = "FORM_VIEWER")]
    public async Task<IActionResult> Create(
        [FromBody] CreateFormResponseRequestDto request)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        try
        {
            var response =
                await _formResponseService.CreateAsync(
                    request,
                    currentUserId);

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("my")]
    [Authorize(Roles = "FORM_VIEWER")]
    public async Task<IActionResult> GetMyResponses(
    [FromQuery] PaginationRequestDto request)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var responses =
            await _formResponseService.GetByUserIdAsync(
                currentUserId,
                request);

        return Ok(responses);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "ADMIN,FORM_VIEWER")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var response =
            await _formResponseService.GetByIdAsync(id);

        if (response == null)
        {
            return NotFound(new
            {
                message = "Form response not found."
            });
        }

        var isAdmin = User.IsInRole("ADMIN");

        if (!isAdmin &&
            response.SubmittedBy != currentUserId)
        {
            return Forbid();
        }

        return Ok(response);
    }

    [HttpGet("form/{formId:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetByFormId(
    int formId,
    [FromQuery] PaginationRequestDto request)
    {
        var responses =
            await _formResponseService.GetByFormIdAsync(
                formId,
                request);

        return Ok(responses);
    }
}