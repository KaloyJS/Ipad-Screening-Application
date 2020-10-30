using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ipad_Screening_Application
{
    /// <summary>
    /// Methods for Getting Device Properties for Apple Devices
    /// </summary>
    class Apple_Methods
    {
        /// <summary>
        /// Gets and Calculates the connected device UDID which is needed to get the device property on apple devices
        /// </summary>
        /// <param name="NumberOfConnectedDevice">Number of connected device</param>
        /// <param name="connectedDeviceUDID">String array of Connected Device UDID recorded</param>
        /// <returns></returns>
        public static string GetUdid(List<string> connectedDeviceUDID) {
            string udid;
            if (connectedDeviceUDID.Count == 0)
            {
                udid = ExecuteCommandSync.AppleCommand("idevice_id -l");
            }
            else
            {
                udid = ExecuteCommandSync.AppleCommand("idevice_id -l");
                foreach (string item in connectedDeviceUDID)
                {
                    udid = udid.Replace(item, "");
                }
            }
            connectedDeviceUDID.Add(udid.Trim()); // Add udid to list of udid connected
            return udid.Trim();
        }

        /// <summary>
        /// Gets IMEI from ideviceinfo command
        /// </summary>
        /// <param name="UDID">UDID of device</param>
        /// <returns></returns>
        public static string GetIMEI(string UDID) 
        {
            string res = ExecuteCommandSync.AppleCommand($"ideviceinfo -u {UDID} -k InternationalMobileEquipmentIdentity");
            return res.Trim();
        }

        /// <summary>
        /// Pair device
        /// </summary>
        /// <param name="UDID">UDID of device</param>
        /// <returns></returns>
        public static string PairDevice(string UDID)
        {
            string res = ExecuteCommandSync.AppleCommand($"idevicepair -u {UDID} pair");
            return res.Trim();
        }

        /// <summary>
        /// Gets SerialNumber from ideviceinfo command
        /// </summary>
        /// <param name="UDID">UDID of device</param>
        /// <returns></returns>
        public static string GetSerialNumber(string UDID)
        {
            string res = ExecuteCommandSync.AppleCommand($"ideviceinfo -u {UDID} -k SerialNumber");
            return res.Trim();
        }

        /// <summary>
        /// Gets ModelNumber from ideviceinfo command
        /// </summary>
        /// <param name="UDID">UDID of device</param>
        /// <returns></returns>
        public static string GetModelNumber(string UDID)
        {
            string res = ExecuteCommandSync.AppleCommand($"ideviceinfo -u {UDID} -k ModelNumber");
            return res.Trim();
        }

        /// <summary>
        /// Gets SoftwareVersion from ideviceinfo command
        /// </summary>
        /// <param name="UDID">UDID of device</param>
        /// <returns></returns>
        public static string GetSoftwareVersion(string UDID)
        {
            string res = ExecuteCommandSync.AppleCommand($"ideviceinfo -u {UDID} -k ProductVersion");
            return res.Trim();
        }

        /// <summary>
        /// Calls to an php endpoint and it returns Codepro of product 
        /// </summary>
        /// <param name="Model">ModelNumber property of device</param>
        /// <returns></returns>
        public static string GetCodePro(string Model) { 
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "get_apple-codepro", "1", "model", Model);
        }

        /// <summary>
        /// Calls to an php endpoint and it returns jobnumber from data_wip_new
        /// </summary>
        /// <param name="Model">ModelNumber property of device</param>
        /// <returns></returns>
        public static string GetJobnumber(string IMEI)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "get_apple-jobnumber", "1", "imei", IMEI);
        }

        /// <summary>
        /// Calls to an php endpoint and it returns ModelName of product 
        /// </summary>
        /// <param name="Model">ModelNumber property of device</param>
        /// <returns></returns>
        public static string GetModelName(string Model) {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "get_apple-modelName", "1", "model", Model);
        }

        /// <summary>
        /// Calls to an php endpoint and it returns color of product 
        /// </summary>
        /// <param name="Model">ModelNumber property of device</param>
        /// <returns></returns>
        public static string GetColor(string Model)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "get_apple-color", "1", "model", Model);
        }

        /// <summary>
        /// Calls to an php endpoint and it returns capacity of product 
        /// </summary>
        /// <param name="Model">ModelNumber property of device</param>
        /// <returns></returns>
        public static string GetCapacity(string Model)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            return CallToPHP.GetPost(endPoint, "get_apple-capacity", "1", "model", Model);
        }

        /// <summary>
        /// Checks if Apple device is FMIP Locked
        /// </summary>
        /// <param name="UDID">Device UDID</param>
        /// <returns></returns>
        //public static string IsFMIPLocked(string IMEI) 
        //{
        //    //string res = ExecuteCommandSync.AppleCommand($"ideviceinfo -u {UDID} -q com.apple.fmip -k IsAssociated");
        //    //return res;

        //    string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
        //    return CallToPHP.GetPost(endPoint, "checkFMIP", "1", "imei", IMEI);

        //}

        public static string IsFMIPLocked(string UDID)
        {
            string res = ExecuteCommandSync.AppleCommand($"ideviceinfo -u {UDID} -q com.apple.fmip -k IsAssociated");
            if (res.Trim() == "true")
            {
                return "Yes";
            }
            else
            {
                return "No";
            }

            //string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            //return CallToPHP.GetPost(endPoint, "checkFMIP", "1", "imei", IMEI);

        }

        public static string IsMDM(string UDID)
        {
            string res = ExecuteCommandSync.AppleCommand($"ideviceinfo -u {UDID} -q com.apple.mobile.chaperone -k DeviceIsChaperoned");
            if (res.Trim() == "true")
            {
                return "Yes";
            }
            else
            {
                return "No";
            }

        }

        public static string BatteryQuery(string UDID)
        {
            string res = ExecuteCommandSync.AppleCommand($"idevicediagnostics -u {UDID} diagnostics GasGauge");
            return res;
        }


        public Boolean IsReadyToBeSaved(string Codepro)
        {
            if (!string.IsNullOrEmpty(Codepro) && Codepro != "Not found")
                return true;
            else
                return false;
        }


        public static string PushData(Device device)
        {
            string endPoint = "https://portal-ca.sbe-ltd.ca/SBE_Applications/actions.php";
            string data = JsonConvert.SerializeObject(device);
            return CallToPHP.GetPost(endPoint, "apple-save_data", "1", "data", data);

        }

    }
}
