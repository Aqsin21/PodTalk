namespace PodTalk.DataContext.Entities
{
    public class Episode
    {
        public int Id { get; set; }

        public  required string Title { get; set; } 

        public  required  string Description { get; set; }

        public required string CoverImageUrl { get; set; }

        public required string AudioUrl { get; set; }

        public int LikeCount { get; set; } 

        public int DownloadCount { get; set; }

        public int CommentCount { get; set; }

        public int PlayCount { get; set; }

       

        public int Duration { get; set; }

        

        public int TopicId { get; set; }
        public Topic? Topic { get; set; }


        public int SpeakerId { get; set; }
        public Speaker? Speaker { get; set; }
    }
}
