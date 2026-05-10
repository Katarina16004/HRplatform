using HRplatform.Application.DTO;
using HRplatform.Application.Interfaces;
using HRplatform.Application.Services;
using HRplatform.Domain;
using Moq;

namespace HRplatform.Tests;

public class CandidateServiceTests
{

    Mock<ICandidateRepository> _candidateRepo;
    CandidateService _candidateService;

    public CandidateServiceTests()
    {
        _candidateRepo = new Mock<ICandidateRepository>();
        _candidateService = new CandidateService(_candidateRepo.Object);
    }

    [SetUp]
    public void Setup()
    {
        _candidateRepo = new Mock<ICandidateRepository>();
        _candidateService = new CandidateService(_candidateRepo.Object);
    }

    // tests for most important methods
    #region AddCandidateAsyncTests
    [Test]
    public void AddCandidateAsync_MissingField()
    {
        CreateUpdateCandidateRequest req = new CreateUpdateCandidateRequest();
        req.FullName = " ";
        req.Email = "test@gmail.com";
        req.ContactNum = "123";
        req.DateOfBirth = new DateOnly(2000, 1, 1);

        Exception ex = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _candidateService.AddCandidateAsync(req);
        })!;

        Assert.That(ex.Message.Contains("Full name is required."), Is.True);
        _candidateRepo.Verify(r => r.CreateCandidateAsync(It.IsAny<Candidate>()), Times.Never);
    }

    [Test]
    public void AddCandidateAsync_DuplicateEmail()
    {
        _candidateRepo.Setup(r => r.CandidateExistsByEmailAsync("test@gmail.com")).ReturnsAsync(true);
        CreateUpdateCandidateRequest req = new CreateUpdateCandidateRequest();
        req.FullName = "Ana";
        req.Email = "test@gmail.com";
        req.ContactNum = "123";
        req.DateOfBirth = new DateOnly(2003, 1, 1);

        Exception ex = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _candidateService.AddCandidateAsync(req);
        })!;

        Assert.That(ex.Message, Is.EqualTo("Candidate with this email already exists."));
        _candidateRepo.Verify(r => r.CreateCandidateAsync(It.IsAny<Candidate>()), Times.Never);
    }

    [Test]
    public async Task AddCandidateAsync_ValidRequest()
    {
        CreateUpdateCandidateRequest req = new CreateUpdateCandidateRequest();
        req.FullName = "Ana";
        req.Email = "test@gmail.com";
        req.ContactNum = "123";
        req.DateOfBirth = new DateOnly(2003, 1, 1);

        _candidateRepo.Setup(r => r.CandidateExistsByEmailAsync("test@gmail.com")).ReturnsAsync(false);
        _candidateRepo.Setup(r => r.CreateCandidateAsync(It.IsAny<Candidate>())).ReturnsAsync("new_id");

        string id = await _candidateService.AddCandidateAsync(req);
        Assert.That(id, Is.EqualTo("new_id"));
        _candidateRepo.Verify(r => r.CandidateExistsByEmailAsync("test@gmail.com"), Times.Once);
        _candidateRepo.Verify(r => r.CreateCandidateAsync(It.IsAny<Candidate>()), Times.Once);
    }

    #endregion

    #region AddSkillToCandidateAsyncTests

    [Test]
    public void AddSkillToCandidateAsync_AlreadyHasSkill()
    {
        _candidateRepo.Setup(r => r.CandidateHasSkillAsync("cand_id", "skill_id")).ReturnsAsync(true);
        Exception ex = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _candidateService.AddSkillToCandidateAsync("cand_id", "skill_id");
        })!;

        Assert.That(ex.Message, Is.EqualTo("Candidate already has this skill."));
        _candidateRepo.Verify(r => r.AddSkillToCandidateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task AddSkillToCandidateAsync_ValidRequest()
    {
        _candidateRepo.Setup(r => r.CandidateHasSkillAsync("cand_id", "skill_id")).ReturnsAsync(false);
        _candidateRepo.Setup(r => r.AddSkillToCandidateAsync("cand_id", "skill_id")).ReturnsAsync(true);

        bool ok = await _candidateService.AddSkillToCandidateAsync("cand_id", "skill_id");

        Assert.That(ok, Is.True);
        _candidateRepo.Verify(r => r.CandidateHasSkillAsync("cand_id", "skill_id"), Times.Once);
        _candidateRepo.Verify(r => r.AddSkillToCandidateAsync("cand_id", "skill_id"), Times.Once);
    }

    #endregion

    #region DeleteSkillFromCandidateAsyncTests

    [Test]
    public void DeleteSkillFromCandidateAsync_DoesNotHaveSkill()
    {
        _candidateRepo.Setup(r => r.CandidateHasSkillAsync("cand_id", "skill_id")).ReturnsAsync(false);
        Exception ex = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _candidateService.DeleteSkillFromCandidateAsync("cand_id", "skill_id");
        })!;

        Assert.That(ex.Message, Is.EqualTo("Candidate does not have this skill."));
        _candidateRepo.Verify(r => r.RemoveSkillFromCandidateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task DeleteSkillFromCandidateAsync_ValidRequest()
    {
        _candidateRepo.Setup(r => r.CandidateHasSkillAsync("cand_id", "skill_id")).ReturnsAsync(true);
        _candidateRepo.Setup(r => r.RemoveSkillFromCandidateAsync("cand_id", "skill_id")).ReturnsAsync(true);

        bool ok = await _candidateService.DeleteSkillFromCandidateAsync("cand_id", "skill_id");

        Assert.That(ok, Is.True);
        _candidateRepo.Verify(r => r.CandidateHasSkillAsync("cand_id", "skill_id"), Times.Once);
        _candidateRepo.Verify(r => r.RemoveSkillFromCandidateAsync("cand_id", "skill_id"), Times.Once);
    }

    #endregion

    #region GetCandidatesBySkillsAndNameAsyncTests

    [Test]
    public async Task GetCandidatesBySkillsAndNameAsync_NameOnly()
    {
        Candidate candidate = new Candidate();
        candidate.Id = "id";
        candidate.FullName = "Ana";
        candidate.Email = "test@gmail.com";
        candidate.ContactNum = "123";
        candidate.Skills = new List<Skill>();

        List<Candidate> candidates = new List<Candidate>();
        candidates.Add(candidate);

        _candidateRepo.Setup(r => r.GetCandidatesBySkillsAndNameAsync(It.IsAny<string?>(), It.IsAny<List<string>?>())).ReturnsAsync(candidates);

        List<Candidate> result = await _candidateService.GetCandidatesBySkillsAndNameAsync("Ana", null);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Id, Is.EqualTo("id"));
        _candidateRepo.Verify(r => r.GetCandidatesBySkillsAndNameAsync("Ana", null), Times.Once);
    }

    [Test]
    public async Task GetCandidatesBySkillsAndNameAsync_SkillsOnly()
    {
        Candidate candidate = new Candidate();
        candidate.Id = "id";
        candidate.FullName = "Ana";
        candidate.Email = "test@gmail.com";
        candidate.ContactNum = "123";
        candidate.Skills = new List<Skill>();

        List<Candidate> candidates = new List<Candidate>();
        candidates.Add(candidate);

        List<string> skillsId = new List<string>();
        skillsId.Add("skill1");
        skillsId.Add("skill2");

        _candidateRepo.Setup(r => r.GetCandidatesBySkillsAndNameAsync(It.IsAny<string?>(), It.IsAny<List<string>?>())).ReturnsAsync(candidates);

        List<Candidate> result = await _candidateService.GetCandidatesBySkillsAndNameAsync(null, skillsId);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        _candidateRepo.Verify(r => r.GetCandidatesBySkillsAndNameAsync(null, skillsId), Times.Once);
    }

    [Test]
    public async Task GetCandidatesBySkillsAndNameAsync_NameAndSkills()
    {
        Candidate candidate = new Candidate();
        candidate.Id = "id";
        candidate.FullName = "Ana";
        candidate.Email = "test@gmail.com";
        candidate.ContactNum = "123";
        candidate.Skills = new List<Skill>();

        List<Candidate> candidates = new List<Candidate>();
        candidates.Add(candidate);

        List<string> skillsId = new List<string>();
        skillsId.Add("skill1");

        _candidateRepo.Setup(r => r.GetCandidatesBySkillsAndNameAsync(It.IsAny<string?>(), It.IsAny<List<string>?>())).ReturnsAsync(candidates);
        List<Candidate> result = await _candidateService.GetCandidatesBySkillsAndNameAsync("Ana", skillsId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        _candidateRepo.Verify(r => r.GetCandidatesBySkillsAndNameAsync("Ana", skillsId), Times.Once);
    }
    #endregion

    #region DeleteCandidateAsyncTests

    [Test]
    public void DeleteCandidateAsync_CandidateNotFound()
    {
        _candidateRepo.Setup(r => r.CandidateExistsByIdAsync("cand_id")).ReturnsAsync(false);
        Exception ex = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _candidateService.DeleteCandidateAsync("cand_id");
        })!;

        Assert.That(ex.Message, Is.EqualTo("Candidate not found."));
        _candidateRepo.Verify(r => r.DeleteCandidateAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task DeleteCandidateAsync_ValidRequest()
    {
        _candidateRepo.Setup(r => r.CandidateExistsByIdAsync("cand_id")).ReturnsAsync(true);
        _candidateRepo.Setup(r => r.DeleteCandidateAsync("cand_id")).ReturnsAsync(true);

        bool ok = await _candidateService.DeleteCandidateAsync("cand_id");
        Assert.That(ok, Is.True);
        _candidateRepo.Verify(r => r.CandidateExistsByIdAsync("cand_id"), Times.Once);
        _candidateRepo.Verify(r => r.DeleteCandidateAsync("cand_id"), Times.Once);
    }

    #endregion

}
