using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Ipad_Screening_Application
{

    public class Device : BaseViewModel
    {
        #region Public members
        /// <summary>
        /// Class for device properties
        /// </summary>
        ///        

        public string OEM { set; get; }


        // Apple Devices Properties
        public string UDID { set; get; }

        public string Pair_Status { set; get; } // for apple device only

        public string Model { set; get; }

        public string Manufacturer { set; get; }

        // Universal Properties
        public string Header { set; get; }

        public int Port { set; get; }

        public string IMEI { set; get; }

        public string Serial_Number { set; get; }

        public string Software_Version { set; get; }
        public string Color { set; get; }

        public string Capacity { set; get; }

        public string WorkStation { set; get; }

        public string Status { set; get; }

        /// <summary>
        ///  Where Status of each port is housed
        /// </summary>
        public string StatusIcon { get; set; }

        public string FMIP { get; set; }

        public string Battery_Design_Capacity { get; set; }
        public string Battery_Full_Charge_Capacity { get; set; }
        public string Battery_Cycle_Count { get; set; }

        public string Battery_Health { get; set; }
        public string Charger_Presence { get; set; }
        public string Cable_Presence { get; set; }
        public string Cosmetic_Condition { get; set; }
        public string Power_Status { get; set; }
        public string Box_Status { get; set; }
        public string Dep_MDM_Status { get; set; }
        public string Box { get; set; }
        public string Position { get; set; }

        public string Jobnumber { get; set; }



        public Device(int Port) 
        {
            this.Header = "Connect Device";
            this.Port = Port;
            this.WorkStation = System.Environment.MachineName;            
            this.StatusIcon = PortStatusIconPath.DisconnectedIcon;

        }

        /// <summary>
        /// Resets the class properties
        /// </summary>
        public void Reset() {
            this.Header = "Connect Device";
            //this.StatusIcon = PortStatusIconPath.DisconnectedIcon;
            // Reset everything except Port, WorkStation and UserName
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; ++i)
            {
                if (properties[i].Name != "Port" && properties[i].Name != "WorkStation" && properties[i].Name != "UserName" && properties[i].Name != "Header" && properties[i].Name != "StatusIcon" && properties[i].Name != "IP")
                {
                    properties[i].SetValue(this, null);
                }
                
            }
            this.StatusIcon = PortStatusIconPath.DisconnectedIcon; // Reset status to disconnect

        }

        

        

       




        #endregion
    }
}
