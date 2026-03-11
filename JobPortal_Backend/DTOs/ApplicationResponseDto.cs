namespace JobPortal_Backend.DTOs
{
    public class ApplicationResponseDto
    {
        public int ApplicationId { get; set; }

        public int JobId { get; set; }

        public int UserId { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string Status { get; set; }
    }
}