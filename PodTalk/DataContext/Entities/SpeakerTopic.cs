namespace PodTalk.DataContext.Entities
{
    public class SpeakerTopic
    {
        public int Id { get; set; }
      
       
        public int SpeakerId { get; set; }
        public Speaker? Speaker { get; set; } 
        public int TopicId { get; set; }
        public Topic? Topic { get; set; } 
      
    }
}
