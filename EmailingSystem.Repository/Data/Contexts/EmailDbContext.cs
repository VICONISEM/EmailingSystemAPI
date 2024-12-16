using EmailingSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Repository.Data.Contexts
{
    public class EmailDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DraftConversations> DraftConversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Signature> Signatures { get; set; }
        public DbSet<UserConversationStatus> UserConversationStatuses { get; set; }
        public DbSet<DraftAttachments> DraftAttachments { get; set; }



    }
}
