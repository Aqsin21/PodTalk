namespace PodTalk.Areas.Admin.Data
{
    public class TopicCreateViewModel
    {
        public required string Name { get; set; }
        public int EpisodeCount { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
