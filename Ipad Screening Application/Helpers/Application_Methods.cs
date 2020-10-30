using System.Windows;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Net;
using System.Text;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Security.RightsManagement;
using System.Net.NetworkInformation;

namespace Ipad_Screening_Application
{
    class Application_Methods
    {
        #region Helper methods

        /// <summary>
        /// Sets the Headers Dynamically
        /// </summary>
        /// <param name="obj">Device obj</param>
        /// <param name="path">Path of Icon</param>
        /// <param name="msg">Header messge</param>
        public static void SetHeaders(Device obj, string path, string msg)
        {
            obj.Header = msg;
            obj.StatusIcon = path;
            obj.Status = msg;
        }

        /// <summary>
        /// Prompts an error message and updates Port Headers of designated port
        /// </summary>
        /// <param name="device"></param>
        /// <param name="msg"></param>
        public static void ShowError(Device device, string msg)
        {            
            device.Status = msg;
            device.Header = "Warning!";
            device.StatusIcon = PortStatusIconPath.WarningIcon;
            //MessageBox.Show(msg);
        }

        public static void ShowSaveError(Device device, string msg)
        {
            device.Status = msg;
            device.Header = "Warning!";
            device.StatusIcon = PortStatusIconPath.WarningIcon;
            //MessageBox.Show(msg);
        }

        /// <summary>
        /// Get port depending on currentName 
        /// </summary>
        /// <param name="currentName">current name of element</param>
        /// <returns></returns>
        public static int GetPort(string currentName) 
        {
            int port = 5;
            if (currentName.Contains("1"))
            {
                port = 0;
            }
            else if (currentName.Contains("2"))
            {
                port = 1;
            }
            else if (currentName.Contains("3"))
            {
                port = 2;
            }

            return port;
        }

        public static void ShowConnectedDevice(List<string> connectedDevicesUDID)
        {
            string output = "";
            for (int i = 0; i < connectedDevicesUDID.Count; i++)
            {
                if (i != 0)
                    output += "\n";

                output += connectedDevicesUDID[i];



            }
            MessageBox.Show(output);
        }

        /// <summary>
        /// Capitalize first letter of string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static string GetIPAddress()
        {
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            string ip = addr[1].ToString();

            return ip;
        }

        /// <summary>
        /// Get Ip address of client
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress2()
        {
            
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            
            IPHostEntry ipHostEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] address = ipHostEntry.AddressList;
            //sb.Append("The Local IP Address: " + address[4].ToString());
            //if (address.Length == 2)
            //{
            //    return address[1].ToString();
            //}
            //else
            //{
            //    return address[1].ToString();
            //}

            return address[1].ToString();
        }

        /// <summary>
        /// mapping usb ports in site 96
        /// </summary>
        /// <param name="currentPort"></param>
        /// <returns></returns>

        public static int MapUSBPort(string currentPort)
        {
            int usbPort = Int16.Parse(currentPort);
            int result = 3;
            switch (usbPort)
            {
                case 1:
                    result = 0;
                    break;
                case 2:
                    result = 1;
                    break;
                case 3:
                    result = 2;
                    break;                
                default:
                    result = 3;
                    break;
            }

            return result;

        }

        public static string GetXMLValue(string XML, string searchTerm)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XML);
            XmlNodeList nodes = doc.SelectNodes("root/key");
            foreach (XmlNode node in nodes)
            {
                XmlAttributeCollection nodeAtt = node.Attributes;
                if (nodeAtt["name"].Value.ToString() == searchTerm)
                {
                    XmlDocument childNode = new XmlDocument();
                    childNode.LoadXml(node.OuterXml);
                    return childNode.SelectSingleNode("key/value").InnerText;
                }
                else
                {
                    return "did not match any documents";
                }
            }
            return "No key value pair found";
        }

        public static XmlNodeList GetViaName(string test)
        {
            

            XmlDocument xmltest = new XmlDocument();
            if (!string.IsNullOrEmpty(test))
            {
                xmltest.LoadXml(test);
                
            }
            XmlNodeList elemlist = xmltest.GetElementsByTagName("integer");

            return elemlist;

        }

        public static void SetValue()
        {
            MainWindow win = new MainWindow();
            win.Power_0.SelectedValue= "Yes";
        }


        /// <summary>
        /// Gets port from element name
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static int GetPortFromElementName(string elementName)
        {
            if (elementName.Contains("0"))
            {
                return 0;
            }
            else if (elementName.Contains("1"))
            {
                return 1;
            }
            else if (elementName.Contains("2"))
            {
                return 2;
            }
            else
            {
                return 3;
            }

        }

        public static string GetMDM(string mdm)
        {
            switch (mdm.ToLower())
            {
                case "true":
                    return "Yes";
                    break;
                case "false":
                    return "No";
                    break;
                default:
                    return "";
                    break;
            }
        }

        /// <summary>
        /// Adjust Port to 1,2,3
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string AdjustPort(int port)
        {
            switch (port)
            {
                case 0:
                    return "1";
                    break;
                case 1:
                    return "2";
                    break;
                case 2:
                    return "3";
                    break;
                default:
                    return "";
                    break;
            }
        }

        public static void SaveProcess(PortConnectionViewModel portConnectionViewModel, int port)
        {
            string json = SaveDataToServer(portConnectionViewModel.device[port]);
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            if (result.Status == "Something went wrong contact data department")
            {
                // show error
                ShowError(portConnectionViewModel.device[port], "Something went wrong, contact data department");
            }
            else if (result.Status == "Device already saved")
            {
                // get jobnumber, box and position for label
                portConnectionViewModel.device[port].Jobnumber = result.Jobnumber;
                portConnectionViewModel.device[port].Box = result.Box;
                portConnectionViewModel.device[port].Position = result.Position;
                portConnectionViewModel.device[port].IMEI = result.IMEI;
                portConnectionViewModel.device[port].Model = result.Model;
                portConnectionViewModel.device[port].Capacity = result.Capacity;
                portConnectionViewModel.device[port].Color = result.Color;
                portConnectionViewModel.device[port].Serial_Number = result.Serial_Number;
                portConnectionViewModel.device[port].FMIP = result.FMIP;
                portConnectionViewModel.device[port].Dep_MDM_Status = result.Dep_MDM_Status;
                //portConnectionViewModel.device[port].Status = "Device already saved, to reprint label press button";
                //change submit button mode to print
                portConnectionViewModel.button[port].Content = "Print Label";
                // set header and icon to print mode
                SetHeaders(portConnectionViewModel.device[port], PortStatusIconPath.PrinterIcon, "Device already saved, to reprint label press button");

            }
            else
            {
                // get jobnumber, box and position for label
                portConnectionViewModel.device[port].Jobnumber = result.Jobnumber;
                portConnectionViewModel.device[port].Box = result.Box;
                portConnectionViewModel.device[port].Position = result.Position;
                //portConnectionViewModel.device[port].Status = "Device saved successful, print label";
                //change submit button mode to print
                portConnectionViewModel.button[port].Content = "Print Label";
                // set header and icon to print mode
                SetHeaders(portConnectionViewModel.device[port], PortStatusIconPath.PrinterIcon, "Device saved successful, print label");
            }
        }

        /// <summary>
        /// Gets device information from serial number then reprints label
        /// </summary>
        /// <param name="portConnectionViewModel"></param>
        /// <param name="port"></param>
        public static void ReprintProcess(PortConnectionViewModel portConnectionViewModel, int port)
        {
            string json = GetDeviceInfo(portConnectionViewModel.device[port].Serial_Number);
            //portConnectionViewModel.device[port].Status = json;
            if (!string.IsNullOrEmpty(json))
            {
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                portConnectionViewModel.device[port].Jobnumber = result.Jobnumber;
                portConnectionViewModel.device[port].Box = result.Box;
                portConnectionViewModel.device[port].Position = result.Position;
                portConnectionViewModel.device[port].IMEI = result.IMEI;
                portConnectionViewModel.device[port].Model = result.Model;
                portConnectionViewModel.device[port].Capacity = result.Capacity;
                portConnectionViewModel.device[port].Color = result.Color;
                portConnectionViewModel.device[port].Serial_Number = result.Serial_Number;
                portConnectionViewModel.device[port].FMIP = result.FMIP;
                portConnectionViewModel.device[port].Dep_MDM_Status = result.Dep_MDM_Status;

                PrintLabel(port, portConnectionViewModel);
            }
            else
            {
                SetHeaders(portConnectionViewModel.device[port], PortStatusIconPath.WarningIcon, "Error");
            }

        }


        #endregion

        #region CallToPhp methods 

        /// <summary>
        /// Call on php endpoint to get codepro
        /// </summary>
        /// <param name="modelNumber"></param>
        /// <returns></returns>
        public static string Apple_GetCodePro(string modelNumber)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "get_apple-codepro", "1", "model", modelNumber);
        }

        /// <summary>
        /// Call on php endpoint to get model
        /// </summary>
        /// <param name="modelNumber"></param>
        /// <returns></returns>
        public static string Apple_GetModel(string modelNumber)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "get_apple-modelName", "1", "model", modelNumber);
        }

        /// <summary>
        /// Call on php endpoint to get color
        /// </summary>
        /// <param name="modelNumber"></param>
        /// <returns></returns>
        public static string Apple_GetColor(string modelNumber)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "get_apple-color", "1", "model", modelNumber);
        }

        /// <summary>
        /// Call on php endpoint to get capacity
        /// </summary>
        /// <param name="modelNumber"></param>
        /// <returns></returns>
        public static string Apple_GetCapacity(string modelNumber)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "get_apple-capacity", "1", "model", modelNumber);
        }

        /// <summary>
        /// Call on php endpoint to call Samsung Api to get code ref
        /// </summary>
        /// <param name="IMEI"></param>
        /// <returns></returns>
        public static string Samsung_GetCoderef(string IMEI)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/SamsungApi2.php";
            return CallToPHP.GetPost(endPoint, "getCodeRef", "1", "imei", IMEI);
        }

        /// <summary>
        /// Samsung Get Capacity of device
        /// </summary>
        /// <param name="coderef"></param>
        /// <returns></returns>
        public static string Samsung_GetCapacity(string coderef)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "samsung-getCapacity", "1", "coderef", coderef);
        }

        /// <summary>
        /// Samsung Get Color of device
        /// </summary>
        /// <param name="coderef"></param>
        /// <returns></returns>
        public static string Samsung_GetColor(string coderef)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "samsung-getColor", "1", "coderef", coderef);
        }

        /// <summary>
        /// Samsung Get Codepro
        /// </summary>
        /// <param name="coderef"></param>
        /// <returns></returns>
        public static string Samsung_GetCodepro(string coderef)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "samsung-getCodepro", "1", "coderef", coderef);
        }

        /// <summary>
        /// Call to php endpoint to check Fmip lock
        /// </summary>
        /// <param name="IMEI"></param>
        /// <returns></returns>
        public static string Apple_CheckFMIP(string IMEI) 
        {
            
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "checkFMIP", "1", "imei", IMEI);
        }





        /// <summary>
        /// Push to json encoded data to php endpoint to save to db and upload csv 
        /// </summary>
        /// <param name="device">device object of device being saved</param>
        /// <param name="OEM"></param>
        /// <returns></returns>

        public static string SaveDataToServer(Device device)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/compugen-actions.php";
            string data = JsonConvert.SerializeObject(device);
            // switch statement to determine
            string path = "save_data";
            return CallToPHP.GetPost(endPoint, path, "1", "data", data);

        }

        public static string GetDeviceInfo(string Serial_Number)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/compugen-actions.php";           
            // switch statement to determine
            string path = "getDeviceInfo";
            return CallToPHP.GetPost(endPoint, path, "1", "sn", Serial_Number);

        }

        /// <summary>
        /// Resets the workstation ip and port to connected no
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>

        public static string Disconnect_on_server(Device device)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            string data = JsonConvert.SerializeObject(device);
            // switch statement to determine
            string path = "telus_screening-disconnect";
            return CallToPHP.GetPost(endPoint, path, "1", "data", data);
        }

        /// <summary>
        /// Resets the connected column of all ports of workstation
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>

        public static string Reset_On_Close(string workstation)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            
            // switch statement to determine
            string path = "telus_screening-close";
            return CallToPHP.GetPost(endPoint, path, "1", "workstation", workstation);
        }

        #endregion              

        #region On Connect Proccess
        public static void OnConnectPortProcess(int port, PortConnectionViewModel portConnectionViewModel, Application_Properties App)
        {
            App.NumberOfConnectedDevice++;
            // set header as connected
            portConnectionViewModel.device[port].Header = "Connected";
            // set icon to connected
            portConnectionViewModel.device[port].StatusIcon = PortStatusIconPath.ConnectedIcon;
            // set power status to yes
            portConnectionViewModel.device[port].Power_Status = "Yes";
            // Determine if Apple or Android device
            if (portConnectionViewModel.device[port].Manufacturer.Trim() == "Apple, Inc.")
            {
                // Get device UDID
                portConnectionViewModel.device[port].UDID = Apple_Methods.GetUdid(App.Apple_ID_List);
                if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].UDID.Trim()))
                {
                    portConnectionViewModel.device[port].OEM = "Apple";
                    // Pair device
                    portConnectionViewModel.device[port].Pair_Status = Apple_Methods.PairDevice(portConnectionViewModel.device[port].UDID);

                    if (!portConnectionViewModel.device[port].Pair_Status.ToLower().Contains("returned unhandled error code -38"))
                    {
                        while (portConnectionViewModel.device[port].Pair_Status.ToLower().Contains("error"))
                        {
                            portConnectionViewModel.device[port].Pair_Status = Apple_Methods.PairDevice(portConnectionViewModel.device[port].UDID);
                        }



                        //check if pair is success
                        if (portConnectionViewModel.device[port].Pair_Status.ToLower().Contains("success"))
                        {
                            portConnectionViewModel.device[port].IMEI = Apple_Methods.GetIMEI(portConnectionViewModel.device[port].UDID.Trim());
                            if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].IMEI.Trim()))
                            {
                                // get serial Number
                                portConnectionViewModel.device[port].Serial_Number = Apple_Methods.GetSerialNumber(portConnectionViewModel.device[port].UDID.Trim());
                                if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Serial_Number.Trim()))
                                {
                                    // get software Version
                                    portConnectionViewModel.device[port].Software_Version = Apple_Methods.GetSoftwareVersion(portConnectionViewModel.device[port].UDID.Trim());
                                    if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Software_Version.Trim()))
                                    {
                                        // get fmip
                                        portConnectionViewModel.device[port].FMIP = Apple_Methods.IsFMIPLocked(portConnectionViewModel.device[port].UDID.Trim());
                                        
                                        if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].FMIP.Trim()))
                                        {
                                            // check battery
                                            string batteryQuery = Apple_Methods.BatteryQuery(portConnectionViewModel.device[port].UDID.Trim());
                                            // parse xml return nodelist
                                            XmlNodeList batteryData = GetViaName(batteryQuery);
                                            // get cycle count
                                            portConnectionViewModel.device[port].Battery_Cycle_Count = batteryData[0].InnerXml;
                                            portConnectionViewModel.device[port].Battery_Design_Capacity = batteryData[1].InnerXml;
                                            portConnectionViewModel.device[port].Battery_Full_Charge_Capacity = batteryData[2].InnerXml;
                                            // check if battery data is present and then enable save button
                                            if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Battery_Cycle_Count.Trim()) && !string.IsNullOrEmpty(portConnectionViewModel.device[port].Battery_Design_Capacity.Trim()) && !string.IsNullOrEmpty(portConnectionViewModel.device[port].Battery_Full_Charge_Capacity.Trim()))
                                            {
                                                // get mdm lock                                                            
                                                portConnectionViewModel.device[port].Dep_MDM_Status = Apple_Methods.IsMDM(portConnectionViewModel.device[port].UDID.Trim());
                                                if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Dep_MDM_Status))
                                                {
                                                    string modelNumber = Apple_Methods.GetModelNumber(portConnectionViewModel.device[port].UDID);
                                                    if (!string.IsNullOrEmpty(modelNumber))
                                                    {
                                                        // get model
                                                        portConnectionViewModel.device[port].Model = Apple_GetModel(modelNumber); //Call to php server to get codepro
                                                        if (portConnectionViewModel.device[port].Model != "N/A")
                                                        {
                                                            // get color
                                                            portConnectionViewModel.device[port].Color = Apple_GetColor(modelNumber);
                                                            if (portConnectionViewModel.device[port].Color != "N/A")
                                                            {
                                                                // get capacity
                                                                portConnectionViewModel.device[port].Capacity = Apple_GetCapacity(modelNumber);
                                                                if (portConnectionViewModel.device[port].Capacity != "N/A")
                                                                {
                                                                   
                                                                    //enable the save button                                                            
                                                                    portConnectionViewModel.button[port].Status = true;
                                                                }
                                                                else
                                                                {
                                                                    ShowError(portConnectionViewModel.device[port], $"Model not mapped: {modelNumber}");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ShowError(portConnectionViewModel.device[port], $"Model not mapped: {modelNumber}");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ShowError(portConnectionViewModel.device[port], $"Model not mapped: {modelNumber}");
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    ShowError(portConnectionViewModel.device[port], "Could not see if DEP/MDM locked contact data department");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ShowError(portConnectionViewModel.device[port], "Could not see if FMIP locked contact data department");
                                        }
                                    }
                                    else
                                    {
                                        ShowError(portConnectionViewModel.device[port], "Could not get Software Version, reconnect to try again");
                                    }
                                }
                                else
                                {
                                    ShowError(portConnectionViewModel.device[port], "Could not get Serial Number, reconnect to try again");
                                }
                            }
                            else
                            {
                                ShowError(portConnectionViewModel.device[port], "Could not get IMEI, reconnect to try again");
                            }
                        }
                        else
                        {
                            ShowError(portConnectionViewModel.device[port], "Pairing failed reconnect device to try again");
                        }
                    }
                    else
                    {
                        // set model as ipad mini 4
                        portConnectionViewModel.device[port].Model = "IPAD MINI 4";
                        //set dep locked as yes
                        portConnectionViewModel.device[port].Dep_MDM_Status = "Yes";
                        // set locked as true
                        portConnectionViewModel.locked[port].Status = true;
                        // enable properties edit
                        portConnectionViewModel.plist[port].Status = false;
                        // enable submit button
                        portConnectionViewModel.button[port].Status = true;
                        ShowError(portConnectionViewModel.device[port], "Could not get device information enter properties manually");
                    }
                    
                }
                else
                {
                    ShowError(portConnectionViewModel.device[port], "Could not get device id, please disconnect and reconnect");
                }
                
            }
            



            

        }

        #endregion

        #region On Disconnect
        public static void OnDisconnectPortProcess(int port, PortConnectionViewModel portConnectionViewModel, Application_Properties App)
        {
            
            App.NumberOfConnectedDevice--; //Update Connected count by subtracting 1
            // remove ID from id list
            if (portConnectionViewModel.device[port].Manufacturer.Trim() == "Apple, Inc.")
            {
                App.Apple_ID_List.Remove(portConnectionViewModel.device[port].UDID);                
            }
             
            // Reset device obj
            portConnectionViewModel.device[port].Reset();
            portConnectionViewModel.plist[port].Reset(); // turn plist to read only 
            //// disable the disconnected submit button
            portConnectionViewModel.button[port].Reset(); // reset button
            // set locked as false
            portConnectionViewModel.locked[port].Reset();
        }
        #endregion

        #region Save data
        public static void Save_data(PortConnectionViewModel portConnectionViewModel, int port)
        {
            // check if lock status is locked if yes operator needs to manually enter properties


            // get mainwindow elements
            if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Power_Status))
            {
                if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Charger_Presence))
                {
                    if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Cable_Presence))
                    {
                        if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Charger_Presence))
                        {
                            if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Cosmetic_Condition))
                            {
                                if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Box_Status))
                                {
                                    if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Dep_MDM_Status))
                                    {

                                        if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Serial_Number))
                                        {
                                            if (portConnectionViewModel.locked[port].Status == true)
                                            {
                                                // if yes then check if properties are not empty
                                                if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].IMEI))
                                                {
                                                    if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Model))
                                                    {
                                                        if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Color))
                                                        {
                                                            if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Capacity))
                                                            {
                                                                if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].FMIP))
                                                                {
                                                                    // get fmip
                                                                    //portConnectionViewModel.device[port].FMIP = Apple_CheckFMIP(portConnectionViewModel.device[port].IMEI.Trim());

                                                                    //Continue with Save process
                                                                    SaveProcess(portConnectionViewModel, port);
                                                                }
                                                                else
                                                                {
                                                                    ShowSaveError(portConnectionViewModel.device[port], "Please enter device FMIP Lock Status, and press save again");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ShowSaveError(portConnectionViewModel.device[port], "Please enter device Capacity, and press save again");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ShowSaveError(portConnectionViewModel.device[port], "Please enter device Color, and press save again");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ShowSaveError(portConnectionViewModel.device[port], "Please enter device Model, and press save again");
                                                    }
                                                }
                                                else
                                                {
                                                    ShowSaveError(portConnectionViewModel.device[port], "Please enter device IMEI, and press save again");
                                                }
                                            }
                                            else
                                            {
                                                

                                                // continue with save process
                                                SaveProcess(portConnectionViewModel, port);
                                            }

                                        }
                                        else
                                        {
                                            ShowSaveError(portConnectionViewModel.device[port], "Please enter device Serial Number, and press save again");
                                        }
                                    }
                                    else
                                    {
                                        ShowSaveError(portConnectionViewModel.device[port], "Please enter if device is DEP/MDM locked, and press save again");
                                    }
                                }
                                else
                                {
                                    ShowSaveError(portConnectionViewModel.device[port], "Please enter if box is reusable, and press save again");
                                }
                            }
                            else
                            {
                                ShowSaveError(portConnectionViewModel.device[port], "Please enter cosmetic condition, and press save again");
                            }
                        }
                        else
                        {
                            ShowSaveError(portConnectionViewModel.device[port], "Please enter if charger is present, and press save again");
                        }
                    }
                    else
                    {
                        ShowSaveError(portConnectionViewModel.device[port], "Please enter if cable is present, and press save again");
                    }
                }
                else
                {
                    ShowSaveError(portConnectionViewModel.device[port], "Please enter if charger is present, and press save again");
                }
            }
            else
            {
                ShowError(portConnectionViewModel.device[port], "Please enter if device is powering on, and press save again");
            }
            
        }
        #endregion

        #region Print Label
        public static void PrintLabel(int port, PortConnectionViewModel portConnectionViewModel)
        {
            PrintLabel p = new PrintLabel();
            p.PrintUsingFlowDocument(portConnectionViewModel.Device[port]);
        }
        #endregion

        #region reprint function
        public static void ReprintLabel(PortConnectionViewModel portConnectionViewModel, int port)
        {
            if (!string.IsNullOrEmpty(portConnectionViewModel.device[port].Serial_Number))
            {
                ReprintProcess(portConnectionViewModel, port);
            }
            else 
            {
                ShowError(portConnectionViewModel.device[port], "Please enter device serial number and press reprint again");
            }
        }
        #endregion
    }
}
