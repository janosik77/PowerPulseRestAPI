namespace PowerPulseRestAPI.DTO.StockDto.Responses
{
    public class LowStockNoteListResponseDto
    {
        public List<LowStockNoteDto> Notes { get; set; } = new();
        public int Count { get; set; }
    }
}
