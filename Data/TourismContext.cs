﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourismAPI.Models;

namespace TourismAPI.Data
{
    public class TourismContext : IdentityDbContext<User, IdentityRole,String>
    {
      public DbSet<User> User { get; set; }
      public TourismContext(DbContextOptions<TourismContext> options): base(options) 
      {
        
      }
    }
}
