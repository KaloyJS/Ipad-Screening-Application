using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipad_Screening_Application
{
    public class Lock_Status : BaseViewModel
    {
        public bool Status { set; get; }

        public Lock_Status()
        {
            this.Status = false;

        }

        public void Reset()
        {
            this.Status = false;
        }

    }
}
