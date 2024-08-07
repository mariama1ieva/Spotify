using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public interface SettingConfiguration : IEntityTypeConfiguration<Setting>
    {

        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(m => m.Value).IsRequired().HasMaxLength(50);
            builder.Property(m => m.Key).IsRequired().HasMaxLength(20);

        }
    }
}
