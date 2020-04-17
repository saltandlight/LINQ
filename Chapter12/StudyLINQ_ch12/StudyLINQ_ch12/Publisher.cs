using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch12
{
    public class Publisher
    {
        public string Logo { get; set; }
        public string Name { get; set; }
        private string WebSite { get; set; }

        public string toString()
        {
            return string.Format("Logo:{0}, Name:{1}, WebSite:{2} ", this.Logo, this.Name, this.WebSite);
        }
    }
}
