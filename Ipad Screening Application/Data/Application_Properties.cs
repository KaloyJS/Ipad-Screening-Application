using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ipad_Screening_Application
{
    public class Port_Status
    {
        public string status = "Empty";
        public string pid;
        public string vid;
        public string port;
    }
    public class Application_Properties
    {
        /// <summary>
        /// Keeps track of USBEvent
        /// </summary>
        public USBEvent USBEventType { get; set; }

        

        // Device vid and pid
        public string DeviceVID;
        public string DevicePID;

        public List<string> Apple_ID_List = new List<string>();
        public List<string> Android_ID_List = new List<string>();
        public int NumberOfConnectedDevice;

        // Connected device port
        public int Connected_device_port;

        // Disconnected device port
        public int Disconnected_device_port;

        public Port_Status[] Ports = new Port_Status[4];


        public string deviceId;
        public Application_Properties()
        {          
                                  
            

            for (int i = 0; i < Ports.Length; i++)
            {
                Ports[i] = new Port_Status();
            }

            

        }
    }
}
