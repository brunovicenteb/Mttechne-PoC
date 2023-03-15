using Mttechne.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mttechne.Toolkit.Data;
using Mttechne.Toolkit.Data.Config;

namespace Mttechne.Infra.Data.Mappings
{
    public class MovementTypeMap : IEntityTypeConfiguration<MovementType>
    {
        public void Configure(EntityTypeBuilder<MovementType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).UseIdentityAlwaysColumn();
            Toolkit.Data.ToolkitConfigExtensions.AddDefaultVarChar(builder.Property(c => c.Name), tamanho: 25);
        }
    }
}