using System.ComponentModel.DataAnnotations.Schema;

namespace PodTalk.DataContext.Entities
{
    public class Topic
    {
        public int Id { get; set; }


        public int EpisodeCount { get; set; }
        public required string Name { get; set; }

        public  string? ImageUrl { get; set; } 

        public List<Episode> Episodes { get; set; } = [];

        public List<SpeakerTopic> SpeakerTopics { get; set; } = [];

        [NotMapped]
        public IFormFile? ImageFile { get; set; }




    }
}
