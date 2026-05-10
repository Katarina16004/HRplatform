using HRplatform.Application.DTO;
using HRplatform.Application.Interfaces;
using HRplatform.Application.Services;
using HRplatform.Domain;
using Moq;

namespace HRplatform.Tests;

[TestFixture]
public class SkillServiceTests
{
    private Mock<ISkillRepository> _skillRepo;
    private SkillService _skillService;

    public SkillServiceTests()
    {
        _skillRepo = new Mock<ISkillRepository>();
        _skillService = new SkillService(_skillRepo.Object);
    }

    [SetUp]
    public void Setup()
    {
        _skillRepo = new Mock<ISkillRepository>();
        _skillService = new SkillService(_skillRepo.Object);
    }

    #region AddSkillAsyncTests

    [Test]
    public void AddSkillAsync_SkillAlreadyExists()
    {
        CreateSkillRequest req = new CreateSkillRequest();
        req.Name = "C";

        _skillRepo.Setup(r => r.SkillExistsByNameAsync("C")).ReturnsAsync(true);
        Exception ex = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _skillService.AddSkillAsync(req);
        })!;

        Assert.That(ex.Message, Is.EqualTo("Skill already exists."));
        _skillRepo.Verify(r => r.CreateSkillAsync(It.IsAny<Skill>()), Times.Never);
    }

    [Test]
    public async Task AddSkillAsync_ValidRequest()
    {
        CreateSkillRequest req = new CreateSkillRequest();
        req.Name = "C";

        _skillRepo.Setup(r => r.SkillExistsByNameAsync("C")).ReturnsAsync(false);
        _skillRepo.Setup(r => r.CreateSkillAsync(It.IsAny<Skill>())).ReturnsAsync("new_id");

        string id = await _skillService.AddSkillAsync(req);

        Assert.That(id, Is.EqualTo("new_id"));

        _skillRepo.Verify(r => r.SkillExistsByNameAsync("C"), Times.Once);
        _skillRepo.Verify(r => r.CreateSkillAsync(It.Is<Skill>(s =>
            s != null &&
            s.Id != null && s.Id.Length > 0 &&
            s.Name == "C"
        )), Times.Once);
    }

    #endregion

    #region GetAllSkillsAsyncTests

    [Test]
    public async Task GetAllSkillsAsync_ReturnsList()
    {
        List<Skill> skills = new List<Skill>();
        skills.Add(new Skill { Id = "1", Name = "C" });
        skills.Add(new Skill { Id = "2", Name = "SQL" });

        _skillRepo.Setup(r => r.GetAllSkillsAsync()).ReturnsAsync(skills);

        List<Skill> result = await _skillService.GetAllSkillsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("C"));
        Assert.That(result[1].Name, Is.EqualTo("SQL"));

        _skillRepo.Verify(r => r.GetAllSkillsAsync(), Times.Once);
    }

    #endregion

    #region GetSkillByIdAsyncTests

    [Test]
    public void GetSkillByIdAsync_SkillNotFound()
    {
        _skillRepo.Setup(r => r.GetSkillByIdAsync("skill_id")).ReturnsAsync((Skill?)null);
        Exception ex = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _skillService.GetSkillByIdAsync("skill_id");
        })!;

        Assert.That(ex.Message, Is.EqualTo("Skill not found."));
    }

    [Test]
    public async Task GetSkillByIdAsync_ValidRequest()
    {
        Skill skill = new Skill();
        skill.Id = "skill_id";
        skill.Name = "C";

        _skillRepo.Setup(r => r.GetSkillByIdAsync("skill_id")).ReturnsAsync(skill);
        Skill? result = await _skillService.GetSkillByIdAsync("skill_id");
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo("skill_id"));
        Assert.That(result.Name, Is.EqualTo("C"));

        _skillRepo.Verify(r => r.GetSkillByIdAsync("skill_id"), Times.Once);
    }

    #endregion

    #region GetSkillByNameAsyncTests

    [Test]
    public async Task GetSkillByNameAsync_SkillDoesNotExist()
    {
        _skillRepo.Setup(r => r.SkillExistsByNameAsync("C")).ReturnsAsync(false);

        Skill? result = await _skillService.GetSkillByNameAsync("C");
        Assert.That(result, Is.Null);
        _skillRepo.Verify(r => r.SkillExistsByNameAsync("C"), Times.Once);
        _skillRepo.Verify(r => r.GetSkillByNameAsync(It.IsAny<string>()), Times.Never);
    }


    [Test]
    public async Task GetSkillByNameAsync_ValidRequest()
    {
        Skill skill = new Skill();
        skill.Id = "skill_id";
        skill.Name = "C";

        _skillRepo.Setup(r => r.SkillExistsByNameAsync("C")).ReturnsAsync(true);
        _skillRepo.Setup(r => r.GetSkillByNameAsync("C")).ReturnsAsync(skill);

        Skill? result = await _skillService.GetSkillByNameAsync("C");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("C"));

        _skillRepo.Verify(r => r.SkillExistsByNameAsync("C"), Times.Once);
        _skillRepo.Verify(r => r.GetSkillByNameAsync("C"), Times.Once);
    }

    #endregion

    #region DeleteSkillAsyncTests
    [Test]
    public void DeleteSkillAsync_SkillNotFound()
    {
        _skillRepo.Setup(r => r.GetSkillByIdAsync("skill_id")).ReturnsAsync((Skill?)null);
        Exception ex = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _skillService.DeleteSkillAsync("skill_id");
        })!;

        Assert.That(ex.Message, Is.EqualTo("Skill not found."));
        _skillRepo.Verify(r => r.DeleteSkillAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task DeleteSkillAsync_ValidRequest()
    {
        Skill skill = new Skill();
        skill.Id = "skill_id";
        skill.Name = "C";

        _skillRepo.Setup(r => r.GetSkillByIdAsync("skill_id")).ReturnsAsync(skill);
        _skillRepo.Setup(r => r.DeleteSkillAsync("skill_id")).ReturnsAsync(true);
        bool ok = await _skillService.DeleteSkillAsync("skill_id");

        Assert.That(ok, Is.True);
        _skillRepo.Verify(r => r.GetSkillByIdAsync("skill_id"), Times.Once);
        _skillRepo.Verify(r => r.DeleteSkillAsync("skill_id"), Times.Once);
    }

    #endregion
}