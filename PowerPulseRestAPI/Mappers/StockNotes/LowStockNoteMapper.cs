using PowerPulseRestAPI.Data.Models.StockModels;
using PowerPulseRestAPI.DTO.StockDto.Responses;

namespace PowerPulseRestAPI.Mappers.StockNotes
{
    public static class LowStockNoteMapper
    {
        public static LowStockNoteDto ToDto(
            this LowStockNote note,
            long currentEmployeeId,
            bool isAdmin)
        {
            var isOwner = note.CreatedByEmployeeId == currentEmployeeId;
            var canManage = isAdmin || isOwner;

            return new LowStockNoteDto
            {
                Id = note.Id,
                Priority = note.Priority,
                Note = note.Note,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt,
                CreatedByEmployeeId = note.CreatedByEmployeeId,
                CreatedByEmployeeFullName =
                    $"{note.CreatedByEmployee.Person.FirstName} {note.CreatedByEmployee.Person.LastName}",
                IsOwner = isOwner,
                CanEdit = canManage,
                CanDelete = canManage
            };
        }
    }
}
