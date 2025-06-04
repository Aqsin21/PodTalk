namespace PodTalk.Areas.Admin.Data
{
    public class TopicEditViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int EpisodeCount { get; set; }
        public string? ExistingImageUrl { get; set; }
        public IFormFile? NewImageFile { get; set; }


    }
}



