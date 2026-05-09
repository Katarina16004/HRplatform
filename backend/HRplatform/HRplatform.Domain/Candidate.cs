namespace HRplatform.Domain
{
    public class Candidate
    {
        public string Id { get; set; } = "";  //GUID
        public string FullName { get; set; } = "";
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; } = "";
        public string ContactNum { get; set; } = "";
        public List<Skill> Skills { get; set; } = new List<Skill>();
    }
}
