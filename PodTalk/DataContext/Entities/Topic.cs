namespace PodTalk.DataContext.Entities
{
    public class Topic
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string ImageUrl { get; set; } 

        public List<Episode> Episodes { get; set; } = [];

        public List<SpeakerTopic> SpeakerTopics { get; set; } = [];


    }
}
