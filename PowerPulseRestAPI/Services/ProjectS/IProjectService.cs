using PowerPulseRestAPI.DTO.ProjectDto.Requests;
using PowerPulseRestAPI.DTO.ProjectDto.Responses;

namespace PowerPulseRestAPI.Services.ProjectS
{
    public interface IProjectService
    {
        Task<ProjectShortDetailsDto> CreateAsync(
            CreateProjectDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProjectListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default);

        Task<ProjectFullDetailsDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<ProjectShortDetailsDto> UpdateAsync(
            long id,
            UpdateProjectDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProjectSelectOptionDto>> GetSelectOptionsAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProjectTaskListItemDto>> GetTaskListByEmployeeIdAsync(
            long employeeId,
            CancellationToken cancellationToken = default);

        Task<ProjectTaskListItemDto> GetTaskByIdAsync(
            long taskId,
            CancellationToken cancellationToken = default);

            Task<ProjectNoteListItemDto> CreateNoteAsync(
            CreateProjectNoteDto dto, 
            CancellationToken ct = default);

        Task<ProjectNoteListItemDto> UpdateNoteAsync(
            long noteId, 
            UpdateProjectNoteDto dto,
            long loggedEmployeeId,
            string? role,
            CancellationToken ct = default);

        Task DeleteNoteAsync(
            long noteId,
            long loggedEmployeeId,
            string? role,
            CancellationToken ct = default);

        Task<ProjectTaskListItemDto> CreateTaskAsync(
            long projectId, 
            long createdByEmployeeId,
            CreateProjectTaskDto dto, 
            CancellationToken ct = default);

        Task<ProjectTaskListItemDto> UpdateTaskAsync(
            long taskId, 
            UpdateProjectTaskDto dto, 
            CancellationToken ct = default);

        Task DeleteTaskAsync(
            long taskId, 
            CancellationToken ct = default);

        Task<ProjectAttachmentListItemDto> CreateAttachmentAsync(
            CreateProjectAttachmentDto dto, 
            CancellationToken ct = default);
        Task<ProjectAttachmentListItemDto> UpdateAttachmentAsync(
            long attachmentId, 
            UpdateProjectAttachmentDto dto, 
            CancellationToken ct = default);

        Task DeleteAttachmentAsync(
            long attachmentId, 
            CancellationToken ct = default);

        Task<IReadOnlyList<ProjectSelectOptionDto>> GetProjectAccessSelectOptionsListAsync (
            long loggedEmployeeId,          
            CancellationToken ct = default);

        Task<ProjectAccessListItemDto> CreateAccessAsync(
            CreateProjectAccessDto dto, 
            CancellationToken ct = default);

        Task<ProjectAccessListItemDto> UpdateAccessAsync(
            long accessId, 
            UpdateProjectAccessDto dto, 
            CancellationToken ct = default);

        Task DeleteAccessAsync(
            long accessId, 
            CancellationToken ct = default);

        Task<ProjectTaskListItemDto> UpdateStatusAsync(
            long id,
            long loggedEmployeeId,
            UpdateTaskStatusDto dto,
            CancellationToken cancellationToken);


    }
}
