using FormManagementSystem.DTOs.Forms;
using FormManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormFieldController : ControllerBase
{
    private readonly IFormFieldService _formFieldService;

    public FormFieldController(
        IFormFieldService formFieldService)
    {
        _formFieldService = formFieldService;
    }

    // =========================================================
    // GET ALL FIELDS FOR A FORM
    // =========================================================

    [HttpGet("form/{formId:int}")]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> GetByFormId(int formId)
    {
        var fields =
            await _formFieldService.GetByFormIdAsync(formId);

        return Ok(fields);
    }

    // =========================================================
    // GET FIELD BY ID
    // =========================================================

    [HttpGet("{id:int}")]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> GetById(int id)
    {
        var field =
            await _formFieldService.GetByIdAsync(id);

        if (field == null)
        {
            return NotFound(new
            {
                message = "Form field not found."
            });
        }

        return Ok(field);
    }

    // =========================================================
    // CREATE FIELD
    // =========================================================

    [HttpPost("form/{formId:int}")]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> Create(
        int formId,
        [FromBody] CreateFormFieldRequestDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Label))
        {
            return BadRequest(new
            {
                message = "Field label is required."
            });
        }

        try
        {
            var field =
                await _formFieldService.CreateAsync(
                    formId,
                    dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = field.Id },
                field);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
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

    // =========================================================
    // UPDATE FIELD
    // =========================================================

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateFormFieldRequestDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Label))
        {
            return BadRequest(new
            {
                message = "Field label is required."
            });
        }

        try
        {
            var updated =
                await _formFieldService.UpdateAsync(
                    id,
                    dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Form field not found."
                });
            }

            return Ok(new
            {
                message = "Form field updated successfully."
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

    // =========================================================
    // DELETE FIELD
    // =========================================================

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN,FORM_CREATOR")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _formFieldService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Form field not found."
            });
        }

        return Ok(new
        {
            message = "Form field deleted successfully."
        });
    }
}