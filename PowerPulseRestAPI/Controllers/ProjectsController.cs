using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.ProjectDto.Requests;
using PowerPulseRestAPI.Services.ProjectS;
using System.Security.Claims;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProjectDto dto,
            CancellationToken cancellationToken)
        {
            var created = await _projectService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken)
        {
            var result = await _projectService.GetListAsync(cancellationToken);
            return Ok(result);
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(
            long id,
            CancellationToken cancellationToken)
        {
            var result = await _projectService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] UpdateProjectDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _projectService.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("select-options")]
        public async Task<IActionResult> GetSelectOptions(CancellationToken ct)
        {
            var result = await _projectService.GetSelectOptionsAsync(ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("tasks/employee")]
        public async Task<IActionResult> GetTaskListByEmployeeId(
                       CancellationToken cancellationToken)
        {
            var employeeIdClaim = User.FindFirst("employeeId")?.Value;
            if (!long.TryParse(employeeIdClaim, out var loggedEmployeeId))
                return Forbid();

            var result = await _projectService.GetTaskListByEmployeeIdAsync(loggedEmployeeId, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("tasks/{taskId:long}")]
        public async Task<IActionResult> GetTaskById(
                       long taskId,
                       CancellationToken cancellationToken)
        {
            var result = await _projectService.GetTaskByIdAsync(taskId, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            await _projectService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpPost("{projectId:long}/notes")]
        public async Task<IActionResult> CreateNote(
            long projectId,
            [FromBody] CreateProjectNoteDto dto,
            CancellationToken ct)
        {
            if (dto.ProjectId != projectId)
                return BadRequest("ProjectId in route and body must match.");

            var result = await _projectService.CreateNoteAsync(dto, ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpPut("notes/{noteId:long}")]
        public async Task<IActionResult> UpdateNote(
            long noteId,
            [FromBody] UpdateProjectNoteDto dto,
            CancellationToken ct)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = User.FindFirst("employeeId")?.Value;

            if (!long.TryParse(employeeIdClaim, out var loggedEmployeeId))
                return Forbid();

            var result = await _projectService.UpdateNoteAsync(
                noteId,
                dto,
                loggedEmployeeId,
                role,
                ct);

            return Ok(result);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpDelete("notes/{noteId:long}")]
        public async Task<IActionResult> DeleteNote(
            long noteId,
            CancellationToken ct)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeIdClaim = User.FindFirst("employeeId")?.Value;

            if (!long.TryParse(employeeIdClaim, out var loggedEmployeeId))
                return Forbid();

            await _projectService.DeleteNoteAsync(
                noteId,
                loggedEmployeeId,
                role,
                ct);

            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{projectId:long}/tasks")]
        public async Task<IActionResult> CreateTask(
            long projectId,
            [FromQuery] long createdByEmployeeId,
            [FromBody] CreateProjectTaskDto dto,
            CancellationToken ct)
        {
            var result = await _projectService.CreateTaskAsync(projectId, createdByEmployeeId, dto, ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("tasks/{taskId:long}")]
        public async Task<IActionResult> UpdateTask(
            long taskId,
            [FromBody] UpdateProjectTaskDto dto,
            CancellationToken ct)
        {
            var result = await _projectService.UpdateTaskAsync(taskId, dto, ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("tasks/{taskId:long}")]
        public async Task<IActionResult> DeleteTask(
            long taskId,
            CancellationToken ct)
        {
            await _projectService.DeleteTaskAsync(taskId, ct);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{projectId:long}/attachments")]
        public async Task<IActionResult> CreateAttachment(
            long projectId,
            [FromBody] CreateProjectAttachmentDto dto,
            CancellationToken ct)
        {
            if (dto.ProjectId != projectId)
                return BadRequest("ProjectId in route and body must match.");

            var result = await _projectService.CreateAttachmentAsync(dto, ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("attachments/{attachmentId:long}")]
        public async Task<IActionResult> UpdateAttachment(
            long attachmentId,
            [FromBody] UpdateProjectAttachmentDto dto,
            CancellationToken ct)
        {
            var result = await _projectService.UpdateAttachmentAsync(attachmentId, dto, ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("attachments/{attachmentId:long}")]
        public async Task<IActionResult> DeleteAttachment(
            long attachmentId,
            CancellationToken ct)
        {
            await _projectService.DeleteAttachmentAsync(attachmentId, ct);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{projectId:long}/accesses")]
        public async Task<IActionResult> CreateAccess(
            long projectId,
            [FromBody] CreateProjectAccessDto dto,
            CancellationToken ct)
        {
            if (dto.ProjectId != projectId)
                return BadRequest("ProjectId in route and body must match.");

            var result = await _projectService.CreateAccessAsync(dto, ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("accesses/project-select-options")]
        public async Task<IActionResult> GetProjectAccessesSelectOptionsList(CancellationToken ct)
        {
            var employeeIdClaim = User.FindFirst("employeeId")?.Value;
            if (!long.TryParse(employeeIdClaim, out var loggedEmployeeId))
            {
                return Forbid();
            }

            var result = await _projectService.GetProjectAccessSelectOptionsListAsync(loggedEmployeeId, ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("accesses/{accessId:long}")]
        public async Task<IActionResult> UpdateAccess(
            long accessId,
            [FromBody] UpdateProjectAccessDto dto,
            CancellationToken ct)
        {
            var result = await _projectService.UpdateAccessAsync(accessId, dto, ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("accesses/{accessId:long}")]
        public async Task<IActionResult> DeleteAccess(
            long accessId,
            CancellationToken ct)
        {
            await _projectService.DeleteAccessAsync(accessId, ct);
            return NoContent();
        }

        [Authorize(Roles = "USER")]
        [HttpPatch("tasks/{id:long}/status")]
        public async Task<IActionResult> UpdateTaskStatus(
            long id,
            [FromBody] UpdateTaskStatusDto dto,
            CancellationToken cancellationToken)
        {
            var employeeIdClaim = User.FindFirst("employeeId")?.Value;

            if (!long.TryParse(employeeIdClaim, out var loggedEmployeeId))
            {
                return Forbid();
            }

            var updated = await _projectService.UpdateStatusAsync(
                id,
                loggedEmployeeId,
                dto,
                cancellationToken);

            return Ok(updated);
        }
    }
}