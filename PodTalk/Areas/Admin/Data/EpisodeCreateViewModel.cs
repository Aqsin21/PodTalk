using Microsoft.AspNetCore.Mvc.Rendering;

namespace PodTalk.Areas.Admin.Data
{
    public class EpisodeCreateViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public string CoverImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IFormFile AudioFile { get; set; }

        public int TopicId { get; set; }
        public int SpeakerId { get; set; }

        public List<SelectListItem>? Topics { get; set; }
        public List<SelectListItem>? Speakers { get; set; }
    }
}
