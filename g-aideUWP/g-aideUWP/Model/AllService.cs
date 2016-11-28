using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.Model
{
    class AllService
    {
        public static IEnumerable<Service> Services { get; set; }

        public static IEnumerable<Service> GetAllStudents()
        {
            return Services = new List<Service>();
        }
    }
}
