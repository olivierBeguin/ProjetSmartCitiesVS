using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.Model
{
    class AllCategory : CategoryService
    {
        public IEnumerable<CategoryService> AllCategoryService { get; set; }
    }
}
