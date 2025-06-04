using PodTalk.DataContext.Entities;

namespace PodTalk.Areas.Admin.Data
{
    public class SpeakerEditViewModel
    {
        public int Id { get; set; }

        
        public string UserName { get; set; } = null!;

        public string? ExistingCoverImageUrl { get; set; }

        public IFormFile? NewCoverImageFile { get; set; }
    }
}
