namespace StudyLINQ_ch12
{
    public class Author
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private string WebSite { get; set; }

        public string toString()
        {
            return string.Format("Name: {0} {1}, Website:{2} ", this.FirstName, this.LastName, this.WebSite);
        }
    }
}