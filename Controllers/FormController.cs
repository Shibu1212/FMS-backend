using FormManagementSystem.DTOs.Forms;
using FormManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormController : ControllerBase
{
    private readonly IFormService _formService;

    public FormController(IFormService formService)
    {
        _formService = formService;
    }

    // =========================================================
    // FORM CREATOR
    // =========================================================

    [HttpPost]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> Create(
        [FromBody] CreateFormRequestDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest(new
            {
                message = "Form name is required."
            });
        }

        var form = await _formService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = form.Id },
            form);
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> GetAll()
    {
        var forms = await _formService.GetAllAsync();

        return Ok(forms);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> GetById(int id)
    {
        var form = await _formService.GetByIdAsync(id);

        if (form == null)
        {
            return NotFound(new
            {
                message = "Form not found."
            });
        }

        return Ok(form);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateFormRequestDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest(new
            {
                message = "Form name is required."
            });
        }

        try
        {
            var updated = await _formService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Form not found."
                });
            }

            return Ok(new
            {
                message = "Form updated successfully."
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

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _formService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Form not found."
            });
        }

        return Ok(new
        {
            message = "Form deleted successfully."
        });
    }

    // =========================================================
    // FORM VIEWER
    // =========================================================

    [HttpGet("published")]
    [Authorize(Roles = "FORM_VIEWER")]
    public async Task<IActionResult> GetPublished()
    {
        var forms = await _formService.GetPublishedAsync();

        return Ok(forms);
    }

    [HttpGet("published/{id:int}")]
    [Authorize(Roles = "FORM_VIEWER")]
    public async Task<IActionResult> GetPublishedById(int id)
    {
        var form = await _formService.GetPublishedByIdAsync(id);

        if (form == null)
        {
            return NotFound(new
            {
                message = "Published form not found."
            });
        }

        return Ok(form);
    }
}