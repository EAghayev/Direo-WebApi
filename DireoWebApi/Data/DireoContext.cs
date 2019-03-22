using DireoWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Data
{
    public class DireoContext:DbContext
    {
        public DireoContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Place> Places { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PlacesTags> PlacesTags { get; set; }
        public DbSet<PlaceFaq> PlaceFaqs { get; set; }
        public DbSet<WorkHour> WorkHours { get; set; }
        public DbSet<PlaceSliderPhotos> PlaceSliderPhotos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
