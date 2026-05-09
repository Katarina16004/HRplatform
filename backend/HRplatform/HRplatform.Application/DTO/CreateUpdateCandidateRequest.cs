namespace HRplatform.Application.DTO
{
    public class CreateUpdateCandidateRequest
    {
        public string FullName { get; set; } = "";
        public DateOnly? DateOfBirth { get; set; }
        public string Email { get; set; } = "";
        public string ContactNum { get; set; } = "";
    }
}
