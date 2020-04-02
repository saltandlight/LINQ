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
    public class Subject
    {
        [Column(IsPrimaryKey =true, Name ="ID")]
        public Guid SubjectId { get; set; }
        [Column]
        public String Description { get; set; }
        [Column]
        public String Name { get; set; }

        [Association(OtherKey ="SubjectId")]
        public EntitySet<Book> Books { get; set; }
    }
}
