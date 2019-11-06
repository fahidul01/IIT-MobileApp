using System.Threading.Tasks;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Web.Infrastructure.DBModel
{
    public class StudentDBContext : IdentityDbContext<DBUser, IdentityRole, string>
    {
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<DBFile> DBFiles { get; internal set; }

        public StudentDBContext(DbContextOptions<StudentDBContext> opt) : base(opt)
        {
            this.Database.EnsureCreated();
        }
    }
}
