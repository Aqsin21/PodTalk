

namespace PodTalk.Areas.Admin.Data
{
    public class EpisodeEditViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }
        public string ExistingCoverImageUrl { get; set; }

        public string ExistingAudioUrl { get; set; }
        public IFormFile? NewCoverImageUrl { get; set; }
        public int TopicId { get; set; }
        public int SpeakerId { get; set; }

    }
}
