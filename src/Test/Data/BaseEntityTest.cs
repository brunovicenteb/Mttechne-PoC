using Moq;
using Mttechne.Toolkit.Data;

namespace Mttechne.Test.Data;
public class BaseEntityTest
{
    private BaseEntity GetBaseEntity(int? id = null)
    {
        return new Mock<BaseEntity>(id).Object;
    }

    [Theory]
    [InlineData(0)]
    [InlineData(25)]
    [InlineData(43)]
    public void CreatorTest(int id)
    {
        var newBaseEntity = GetBaseEntity(id);
        Assert.Equal(id, newBaseEntity.Id);
    }

    [Fact]
    public void GetValidatorsTest()
    {
        var newBaseEntity = GetBaseEntity();
        var validators = newBaseEntity.GetValidators();
        Assert.NotNull(validators);
        Assert.Empty(validators);
    }
}