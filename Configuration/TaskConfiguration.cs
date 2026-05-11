using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm.Backend.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<Entities.Task>
    {
        public void Configure(EntityTypeBuilder<Entities.Task> builder)
        {
            builder.ToTable("task");
            builder.HasKey(b => b.Id); // Example: configure primary key

            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Title).HasColumnName("title");
            builder.Property(x => x.Description).HasColumnName("description");
            builder.Property(x => x.AssignedTo).HasColumnName("assigned_to");
            builder.Property(x => x.AssignedToCompany).HasColumnName("assigned_to_company");
            builder.Property(x => x.ExecutionDate).HasColumnName("execution_date");
            builder.Property(x => x.Location).HasColumnName("location");
            builder.Property(x => x.Priority).HasColumnName("priority");
            builder.Property(x => x.State).HasColumnName("state");
            builder.Property(x => x.Type).HasColumnName("type");
        }
    }
}