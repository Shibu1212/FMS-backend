using FormManagementSystem.DTOs.Registration;
using FormManagementSystem.Extensions;
using FormManagementSystem.Models;
using FormManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FormManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _service;
    private readonly IUserService _userService;

    public RegistrationController(
    IRegistrationService service,
    IUserService userService)
    {
        _service = service;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
    RegistrationRequestDto dto)
    {
        var request = await _service.CreateAsync(dto);

        var response = await _service.GetByIdAsync(request.Id);

        return CreatedAtAction(
            nameof(GetById),
            new { id = request.Id },
            response);
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Get(
        [FromQuery] RegistrationStatus? status)
    {
        var requests = await _service.GetAsync(status);

        return Ok(requests);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetById(int id)
    {
        var request = await _service.GetByIdAsync(id);

        if (request == null)
            return NotFound();

        return Ok(request);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        RegistrationStatusDto dto)
    {
        var publicId = User.GetPublicId();

        if (publicId is not { } currentPublicId)
        {
            return Unauthorized();
        }

        var adminUserId = await _userService.GetUserIdByPublicIdAsync(
            currentPublicId);

        if (adminUserId is not { } currentAdminUserId)
        {
            return Unauthorized();
        }

        var result = await _service.UpdateStatusAsync(
            id,
            dto,
            currentAdminUserId);

        if (!result)
            return NotFound();

        return NoContent();
    }
}