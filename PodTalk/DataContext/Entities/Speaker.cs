namespace PodTalk.DataContext.Entities
{
    public class Speaker
    {
        public int Id { get; set; }

        public required string UserName { get; set; }
        public required string CoverImageUrl { get; set; }

        public List<SpeakerTopic> SpeakerTopics { get; set; }= [];
        public List<Episode> Episodes { get; set; } = [];

    }
}
