using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.Model
{
    public class CategoryService
    {
        public long Id { get; set; }
        public string Label { get; set; }


        public override string ToString()
        {
            return Label;
        }
    }
}
