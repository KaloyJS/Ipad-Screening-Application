using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using USBClassLibrary;
using MessageBox = System.Windows.MessageBox;
using Newtonsoft.Json;
using System.Linq;
using System.ComponentModel.Design;

namespace Ipad_Screening_Application
{
    class USBClassLibrary_Methods
    {
       

        /// <summary>
        /// Determines Which Port is the connected device
        /// </summary>
        /// <param name="App"></param>
        /// <param name="portConnectionViewModel"></param>
        public static Nullable<int> DeviceConnected(Application_Properties App, PortConnectionViewModel portConnectionViewModel)
        {
            //declaring an instance of USBClass
            USBClassLibrary.USBClass USBPort = new USBClass();


            //an instance List<T> of DeviceProperties if you want to read the properties of your devices
            List<USBClassLibrary.USBClass.DeviceProperties> ListOfUSBDeviceProperties;

            ListOfUSBDeviceProperties = new List<USBClassLibrary.USBClass.DeviceProperties>();

            Nullable<UInt32> MI = null;
            bool bGetSerialPort = false;

            if (USBClass.GetUSBDevice(uint.Parse(App.DeviceVID, System.Globalization.NumberStyles.AllowHexSpecifier), uint.Parse(App.DevicePID, System.Globalization.NumberStyles.AllowHexSpecifier), ref ListOfUSBDeviceProperties, bGetSerialPort, MI))
            {
                // Loop through devices found
                for (int i = 0; i < ListOfUSBDeviceProperties.Count; i++)
                {
                    if (!string.IsNullOrEmpty(ListOfUSBDeviceProperties[i].DeviceLocation))
                    {
                        string Manufacturer = ListOfUSBDeviceProperties[i].DeviceManufacturer;                       
                        
                        string currentPort = ListOfUSBDeviceProperties[i].DeviceLocation.Substring(ListOfUSBDeviceProperties[i].DeviceLocation.IndexOf('#') + 4, 1);
                        //MessageBox.Show(currentPort);
                        // In site 96 they dont use usb hub, so we need to map out the ports they use 3 = 2, 6 = 2, 5= 1
                        
                        int numPort = Application_Methods.MapUSBPort(currentPort);                        

                        if (numPort < 3)
                        {
                            if (App.Ports[numPort].status == "Empty")
                            {
                                App.Connected_device_port = numPort;
                                portConnectionViewModel.device[numPort].Manufacturer = Manufacturer;
                                App.Ports[numPort].status = "Connected"; // Set as Connected
                                App.Ports[numPort].pid = App.DevicePID;  // Record PID for device
                                App.Ports[numPort].vid = App.DeviceVID;  // Record VID for device
                                App.Ports[numPort].port = numPort.ToString(); // Set port number
                                break;
                            }
                        }
                        else
                        {
                            return null;
                        }


                    }
                }
            }

            return App.Connected_device_port;
        }

        /// <summary>
        /// Determine which port has a device disconnected
        /// </summary>
        /// <param name="App"></param>
        /// <param name="portConnectionViewModel"></param>
        public static Nullable<int> DeviceDisconnected(Application_Properties App, PortConnectionViewModel portConnectionViewModel)
        {
            //declaring an instance of USBClass
            USBClassLibrary.USBClass USBPort = new USBClass();


            //an instance List<T> of DeviceProperties if you want to read the properties of your devices
            List<USBClassLibrary.USBClass.DeviceProperties> ListOfUSBDeviceProperties;

            ListOfUSBDeviceProperties = new List<USBClassLibrary.USBClass.DeviceProperties>();

            Nullable<UInt32> MI = null;
            bool bGetSerialPort = false;

           


            if (USBClass.GetUSBDevice(uint.Parse(App.DeviceVID, System.Globalization.NumberStyles.AllowHexSpecifier), uint.Parse(App.DevicePID, System.Globalization.NumberStyles.AllowHexSpecifier), ref ListOfUSBDeviceProperties, bGetSerialPort, MI))
            {
                // If USB DEVICE Detected with pid and vid  it means there is one more device connected with the same
                // So loop though found device, save their ports and compare to ports array and the difference is the disconnected port
                // Creates an array of Connected Devices 
                // Get all ports with the same vid and pid
                Port_Status[] samePidVid = App.Ports.Where(p => p.pid == App.DevicePID && p.vid == App.DeviceVID).ToArray();
                // Empty all ports in array
                for (int i = 0; i < samePidVid.Length; i++)
                {
                    samePidVid[i].status = "Empty";
                }                

                List<string> connectedPorts = new List<string>();
                // Loop through found devices
                for (int i = 0; i < ListOfUSBDeviceProperties.Count; i++)
                {
                    if (!string.IsNullOrEmpty(ListOfUSBDeviceProperties[i].DeviceLocation))
                    {
                        string currentPort = ListOfUSBDeviceProperties[i].DeviceLocation.Substring(ListOfUSBDeviceProperties[i].DeviceLocation.IndexOf('#') + 4, 1);
                        int numPort = Application_Methods.MapUSBPort(currentPort);
                        connectedPorts.Add(numPort.ToString());
                        // Loop through samePidVid array and check if port is current port 
                        for (int j = 0; j < samePidVid.Length; j++)
                        {
                            if (samePidVid[j].port == numPort.ToString())
                            {
                                samePidVid[j].status = "Connected";
                            }
                        }
                    }                  

                }
                // Getting the disconnected port                
                var disconnectedPort = samePidVid.SingleOrDefault(p => p.status == "Empty");
                if (disconnectedPort != null)
                {
                    App.Disconnected_device_port = Int16.Parse(disconnectedPort.port);
                    App.Ports[App.Disconnected_device_port].status = "Empty"; // set Empty for disconnected device port
                    App.Ports[App.Disconnected_device_port].pid = "";  // empty PID for port
                    App.Ports[App.Disconnected_device_port].vid = "";  // empty VID for empty
                }
                else
                {
                    return null;
                }


            }
            else
            {
                // If USB DEVICE with pid and vid not found, look through the ports array for that pid and vid and that is the disconnected port
                // Getting the disconnected port                
                var disconnectedPort = App.Ports.SingleOrDefault(p => p.pid == App.DevicePID && p.vid == App.DeviceVID);
                if (disconnectedPort != null)
                {
                    App.Disconnected_device_port = Int16.Parse(disconnectedPort.port);
                    App.Ports[App.Disconnected_device_port].status = "Empty"; // set Empty for disconnected device port
                    App.Ports[App.Disconnected_device_port].pid = "";  // empty PID for port
                    App.Ports[App.Disconnected_device_port].vid = "";  // empty VID for empty
                }
                else
                {
                    return null;
                }

            }

            return App.Disconnected_device_port;
        }    
    
    
    }
}
