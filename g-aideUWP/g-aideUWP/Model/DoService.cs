using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.Model
{
    class DoService
    {
        public long Id { get; set; }
        public DateTime DateService { get; set; }
        public UserApp UserDoService { get; set; }
        public Service ServiceDone { get; set; }
        public virtual Comment  CommentOfService { get; set; }
    }
}
