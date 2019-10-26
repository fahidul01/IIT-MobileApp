using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Web.Infrastructure.DBModel
{
    public class StudentDBContext : IdentityDbContext<DBUser, IdentityRole, string>
    {
        internal DbSet<Batch> Batches { get; set; }
        internal DbSet<Course> Courses { get; set; }
        internal DbSet<Semester> Semesters { get; set; }
        internal DbSet<Post> Posts { get; set; }

        public StudentDBContext(DbContextOptions<StudentDBContext> opt) : base(opt)
        {
            this.Database.EnsureCreated();
        }
    }
}
