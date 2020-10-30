using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipad_Screening_Application.ViewModels
{
    /// <summary>
    /// Class that binds to the property textboxes and determines if they will be read only or editable
    /// </summary>
    public class Plist_Status : BaseViewModel
    {
        public bool Status { set; get; }

        public Plist_Status()
        {
            this.Status = true;
            
        }

        public void Reset()
        {
            this.Status = true;
        }
    }
}
