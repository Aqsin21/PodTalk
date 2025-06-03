using Microsoft.EntityFrameworkCore;
using PodTalk.DataContext.Entities;

namespace PodTalk.DataContext
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Topic> Topics { get; set; } 

        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<Episode> Episodes { get; set; }
        
        public DbSet<SpeakerTopic> SpeakerTopics { get; set; }

    }
}
