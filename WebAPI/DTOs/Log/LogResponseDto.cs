namespace WebAPI.DTOs.Log
{
    public class LogResponseDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = null!;
        public string LogLevelTitle { get; set; } = null!;
    }
}
