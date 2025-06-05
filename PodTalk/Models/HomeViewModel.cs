using PodTalk.DataContext.Entities;

namespace PodTalk.Models
{
    public class HomeViewModel
    {
        public List<Topic> Topics { get; set; } = new List<Topic>();

        public List<Speaker> Speakers { get; set; } = new List<Speaker>();

        public List<Episode> Episodes { get; set;} = new List<Episode>();

        


    }
}
