namespace StudyLINQ_ch11
{
    internal class Review
    {
        public User User { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }

        public string toString()
        {
            return string.Format("user:{0}, rating:{1}, comments:{2}", User, Rating, Comments);
        }
    }
}