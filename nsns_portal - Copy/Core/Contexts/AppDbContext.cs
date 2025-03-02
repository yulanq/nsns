
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using Core.Models;





namespace Core.Contexts
{
   

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Coach> Coaches { get; set; }

        
        public DbSet<Child> Children { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Core.Models.Activity> Activities { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<ParentChild> ParentChild { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ActivityNotification> ActivityNotifications { get; set; }
        public DbSet<CourseNotification> CourseNotifications { get; set; }
        public DbSet<Coach> CoacheIncomes { get; set; }
        public DbSet<CourseEnrollment> CourseEnrollments { get; set; }
        public DbSet<ActivityEnrollment> ActivityEnrollments { get; set; }
        public DbSet<PaymentPackage> PaymentPackages { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<City> Cities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Core.Models.Activity>()
            .ToTable("activities"); // Explicitly map to the table name

            modelBuilder.Entity<ActivityEnrollment>()
                .ToTable("activity_enrollments"); // Explicitly map to the table name


            modelBuilder.Entity<ActivityFeedback>()
           .ToTable("activity_feedback"); // Explicitly map to the table name

            modelBuilder.Entity<Admin>()
          .ToTable("admins"); // Explicitly map to the table name

            modelBuilder.Entity<Child>()
          .ToTable("children"); // Explicitly map to the table name

            modelBuilder.Entity<ChildBalance>()
          .ToTable("child_balance"); // Explicitly map to the table name

            modelBuilder.Entity<Coach>()
          .ToTable("coaches"); // Explicitly map to the table name

            modelBuilder.Entity<CoachIncome>()
          .ToTable("coach_income"); // Explicitly map to the table name

            modelBuilder.Entity<Course>()
          .ToTable("courses"); // Explicitly map to the table name


            modelBuilder.Entity<CourseEnrollment>()
          .ToTable("course_enrollments"); // Explicitly map to the table name

            modelBuilder.Entity<CourseNotification>()
          .ToTable("course_notifications"); // Explicitly map to the table name

            modelBuilder.Entity<Parent>()
          .ToTable("parents"); // Explicitly map to the table name

            modelBuilder.Entity<ParentChild>()
         .ToTable("parent_child"); // Explicitly map to the table name

            modelBuilder.Entity<Payment>()
         .ToTable("payments"); // Explicitly map to the table name

            modelBuilder.Entity<PaymentPackage>()
         .ToTable("payment_package"); // Explicitly map to the table name

            
            modelBuilder.Entity<Staff>()
         .ToTable("staff"); // Explicitly map to the table name

            modelBuilder.Entity<User>()
         .ToTable("users"); // Explicitly map to the table name

            modelBuilder.Entity<Specialty>()
        .ToTable("specialties"); // Explicitly map to the table name

            modelBuilder.Entity<City>()
        .ToTable("cities"); // Explicitly map to the table name
        }
    }

}
