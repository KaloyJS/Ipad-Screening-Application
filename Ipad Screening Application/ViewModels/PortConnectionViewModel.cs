using Ipad_Screening_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Ipad_Screening_Application
{
    public class PortConnectionViewModel : BaseViewModel
    {
        /// <summary>
        /// Objects of the 3 ports are declared
        /// </summary>
        public Device[] device = new Device[4];

        public Device[] Device { get { return device; } }


        /// <summary>
        /// array of button object 
        /// </summary>

        public SubmitButton[] button = new SubmitButton[3];

        public SubmitButton[] Button { get { return button; } }



        /// <summary>
        /// array of Plist status
        /// </summary>

        public Plist_Status[] plist = new Plist_Status[3];

        public Plist_Status[] Plist { get { return plist; } }

        /// <summary>
        /// array of Lock Status
        /// </summary>

        public Lock_Status[] locked = new Lock_Status[3];

        public Lock_Status[] Locked { get { return locked; } }


        public PortConnectionViewModel()
        {
            // instantiate 3 devices (for ports 1 - 2), add additional of adding more ports
            for (int i = 0; i < device.Length; i++)
            {
                device[i] = new Device(i);
            }


            // instantiate button object
            for (int i = 0; i < button.Length; i++)
            {
                button[i] = new SubmitButton();
            }

            // instantiate plist status
            for (int i = 0; i < plist.Length; i++)
            {
                plist[i] = new Plist_Status();
            }

            // instantiate locked status
            for (int i = 0; i < locked.Length; i++)
            {
                locked[i] = new Lock_Status();
            }

        }
    }
}
