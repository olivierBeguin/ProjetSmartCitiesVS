using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.Model
{
    public class Service
    {
        public long Id { get; set; }
        public string NameService { get; set; }
        public string DescriptionService { get; set; }
        public DateTime DatePublicationService { get; set; }
        public CategoryService Category { get; set; }

    }
}
