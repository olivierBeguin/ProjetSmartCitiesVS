using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.Model
{
    class ServiceBindingModel
    {
        public string Label { get; set; }
        public string DescriptionService { get; set; }
        public DateTime DatePublicationService { get; set; }
        public bool ServiceDone { get; set; }
        public string UserNeedService { get; set; }
        public int Category { get; set; }
    }
}
