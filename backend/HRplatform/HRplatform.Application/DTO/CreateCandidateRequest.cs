namespace HRplatform.Application.DTO
{
    public class CreateCandidateRequest
    {
        public string FullName { get; set; } = "";
        public DateOnly? DateOfBirth { get; set; }
        public string Email { get; set; } = "";
        public string ContactNum { get; set; } = "";
    }
}
