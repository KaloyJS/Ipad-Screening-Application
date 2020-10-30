using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipad_Screening_Application
{
    public class SubmitButton : BaseViewModel
    {
        public bool Status { set; get; }

        public string Content { set; get; }

        public SubmitButton()
        {
            this.Status = false;
            this.Content = "Click to Save";
        }


        public void Reset()
        {
            this.Status = false;
            this.Content = "Click to Save";
        }
    }
}
