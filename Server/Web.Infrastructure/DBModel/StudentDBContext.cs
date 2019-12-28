﻿using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Student.Infrastructure.DBModel
{
    public class StudentDBContext : IdentityDbContext<DBUser, IdentityRole, string>
    {
        public DbSet<Batch> Batches { get; internal set; }
        public DbSet<Course> Courses { get; internal set; }
        public DbSet<Semester> Semesters { get; internal set; }
        public DbSet<Notice> Notices { get; internal set; }
        public DbSet<DBFile> DBFiles { get; internal set; }
        public DbSet<StudentCourse> StudentCourses { get; internal set; }
        public DbSet<Lesson> Lessons { get; internal set; }
        public DbSet<ToDoItem> ToDoItems { get; internal set; }

        public StudentDBContext(DbContextOptions<StudentDBContext> opt) : base(opt)
        {
        }

        public StudentDBContext()
        {

        }
    }
}
