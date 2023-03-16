using Mttechne.Toolkit.Data;
using Mttechne.Toolkit.Exceptions;

namespace Mttechne.Test.Data;
public class LifeTimeEntityTest
{
    #region NewLifeTimeEntityTest 
    private class NewLifeTimeEntityTest : LifeTimeEntity
    {
        public NewLifeTimeEntityTest() :
            base()
        {
        }
        public NewLifeTimeEntityTest(DateTime createAt, DateTime? updateAt, DateTime? deleteAd)
            : base(createAt, updateAt, deleteAd)
        {
        }
    }
    #endregion

    [Fact]
    public void NewTimeCycleEntityTest_CreateWithoutParamsTest()
    {
        var newBaseEntity = new NewLifeTimeEntityTest();
        Assert.Equal(0, newBaseEntity.Id);
        Assert.Null(newBaseEntity.CreatedAt);
    }

    [Fact]
    public void NewTimeCycleEntityTest_CreateWithParamsTest()
    {
        DateTime now = DateTime.Now;
        var newTimeCycleEntity = new NewLifeTimeEntityTest(now, null, null);
        Assert.Equal(now, newTimeCycleEntity.CreatedAt.Value);
        Assert.False(newTimeCycleEntity.UpdatedAt.HasValue);
        Assert.False(newTimeCycleEntity.DeletedAt.HasValue);
    }

    [Fact]
    public void NewTimeCycleEntityTest_IsActive()
    {
        DateTime now = DateTime.Now;
        var entity = new NewLifeTimeEntityTest(now, null, null);
        Assert.True(entity.IsActive);

        entity = new NewLifeTimeEntityTest(now, null, DateTime.UtcNow);
        Assert.False(entity.IsActive);
    }

    [Fact]
    public void NewTimeCycleEntityTest_Delete()
    {
        DateTime now = DateTime.Now;
        var entity = new NewLifeTimeEntityTest(now, null, null);
        Assert.True(entity.IsActive);
        Assert.True(entity.Delete());
        Assert.False(entity.IsActive);
    }

    [Fact]
    public void NewTimeCycleEntityTest_DeleteAlreadyDeleted()
    {
        var now = DateTime.Now;
        var entity = new NewLifeTimeEntityTest(now, null, DateTime.UtcNow);
        var exception = Assert.Throws<DomainRuleException>(() => entity.Delete());
        Assert.Equal("Entity already deleted.", exception.Message);

        entity = new NewLifeTimeEntityTest(now, null, null);
        entity.Delete();
        exception = Assert.Throws<DomainRuleException>(() => entity.Delete());
        Assert.Equal("Entity already deleted.", exception.Message);
    }

    [Fact]
    public void NewTimeCycleEntityTest_Update()
    {
        var entity = new NewLifeTimeEntityTest(DateTime.UtcNow, null, null);
        Assert.False(entity.UpdatedAt.HasValue);

        entity.Update();
        Assert.True(entity.UpdatedAt.HasValue);

        var moment = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1));
        TimeSpan sp = entity.UpdatedAt.Value.Date.Subtract(moment);
        Assert.NotEqual(moment, entity.UpdatedAt.Value);

        entity.Update(moment);
        Assert.Equal(moment, entity.UpdatedAt.Value);
    }
}