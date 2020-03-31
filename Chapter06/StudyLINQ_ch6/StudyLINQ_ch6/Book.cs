using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StudyLINQ_ch6
{
    [Table]
    public class Book
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public Guid BookId { get; set; }
        [Column]
        public string Isbn { get; set; }
        [Column(CanBeNull=true)]
        public string Notes { get; set; }
        [Column]
        public Int32 PageCount { get; set; }
        [Column]
        public Decimal Price { get; set; }
        [Column(CanBeNull = true)]
        public string Summary { get; set; }
        [Column(Name = "PubDate")]
        public DateTime PublicatoinDate { get; set; }

        [Column]
        public string Title { get; set; }
        [Column(Name = "Subject")]
        public Guid SubjectId { get; set; }
        [Column(Name = "Publisher")]
        public Guid PublisherId { get; set; }
    }
}
