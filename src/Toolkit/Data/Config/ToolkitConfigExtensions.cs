using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mttechne.Toolkit.Data.Config;

public static class ToolkitConfigExtensions
{
    public static PropertyBuilder<TProperty> AddDefaultVarChar<TProperty>(this PropertyBuilder<TProperty> propertyBuilder,
        int tamanho, bool isRequired = true)
    {
        return propertyBuilder
            .HasMaxLength(tamanho)
            .IsRequired(isRequired);
    }

    public static ReferenceCollectionBuilder<TRelatedEntity, TEntity> AddForeingKey<TEntity, TRelatedEntity>(
        this EntityTypeBuilder<TEntity> entity,
        Expression<Func<TEntity, TRelatedEntity>> navigationExpression,
        Expression<Func<TEntity, object>> foreignKeyExpression,
        bool isRequired = true)
        where TEntity : BaseEntity
        where TRelatedEntity : class
    {
        return entity.HasOne(navigationExpression)
             .WithMany()
             .HasForeignKey(foreignKeyExpression)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired(isRequired);
    }
}