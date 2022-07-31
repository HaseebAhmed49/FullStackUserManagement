using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement_WebAPI.Data.Entities;

namespace UserManagement_WebAPI.Data
{
    public class AppDbContext:IdentityDbContext<AppUser,IdentityRole,string>
    {
        public AppDbContext(DbContextOptions options): base(options)
        {
           
        }

//        public DbSet<AppUser> users { get; set; }
    }
}

