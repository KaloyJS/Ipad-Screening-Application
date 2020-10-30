using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipad_Screening_Application
{
    class ADB_Commands
    {
        /// <summary>
        /// Method that gets device udid
        /// </summary>
        /// <returns></returns>
        public static string GetID(List<string> ID_List) 
        {
            // Get Device Id connected list
            string qry = "devices";
            string result = ExecuteCommandSync.AndroidCommand(qry);
            // Remove unneccessary strings from list
            result = result.Replace("List of devices attached", "");
            result = result.Replace("device", "");
            result = result.Replace("unauthorized", "");
            result = result.Replace("offline", "");
            // If ID List count is more than zero remove items on list from result
            if (ID_List.Count != 0)
            {
                foreach (string item in ID_List)
                {
                    result = result.Replace(item, "");
                }
            }            
            // If the result is not empty or null add to list
            if (!string.IsNullOrEmpty(result.Trim()))
            {
                ID_List.Add(result.Trim());
            }
            // return result
            return result.Trim();



        }
        /// <summary>
        /// method that processes adb devices command result to get device id
        /// </summary>
        /// <param name="res"></param>
        /// <param name="ID_List"></param>
        /// <returns></returns>
        public static string ProcessDeviceId(string res, List<string> ID_List) 
        {
            // remove unwanted characters
            res = res.Replace("List of devices attached", "");
            res = res.Replace("device", "");
            //res = res.replace("unauthorized", "");

            if (ID_List.Count != 0)
            {
                foreach (string id in ID_List)
                {
                    res = res.Replace(id, "");
                }
            }
            ID_List.Add(res.Trim());
            return res.Trim();
        }

        /// <summary>
        /// Uses cmd prompt command to get device Id from adb shell
        /// </summary>
        /// <param name="ID_List"></param>
        /// <returns></returns>
        public static string DeviceID(List<string> ID_List)
        {

            string qry = "devices";
            string result = ExecuteCommandSync.AndroidCommand(qry);
            if (result.Contains("unauthorized"))
            {
                return "";
            }
            return ProcessDeviceId(result, ID_List);            
        }

        /// <summary>
        /// command prompt adb shell command to get oem 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetOEM(string id)
        {
            string qry = $"-s {id} shell getprop ro.product.manufacturer";
            return ExecuteCommandSync.AndroidCommand(qry).Trim();
        }

        /// <summary>
        /// command prompt adb shell command to get imei
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetIMEI(string id)
        {
            string qry = "-s " + id + " shell \"service call iphonesubinfo 1 | grep -o '[0-9a-f]\\{8\\} ' | tail -n+3 | while read a; do echo -n \\\\u${a:4:4}\\\\u${a:0:4}; done\"";
            return ExecuteCommandSync.AndroidCommand(qry).Trim().TrimEnd('\0');
        }

        /// <summary>
        /// command prompt adb shell command to get model from samsung device
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Samsung_getModel(string id)
        {
            string qry = $"-s {id} shell getprop ro.product.model";
            return ExecuteCommandSync.AndroidCommand(qry).Trim();
        }

        /// <summary>
        /// command prompt adb shell command to get software version from samsung device
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Samsung_getSoftwareVersion(string id)
        {
            string qry = $"-s {id} shell getprop ro.bootloader";
            return ExecuteCommandSync.AndroidCommand(qry).Trim();
        }

        /// <summary>
        /// command prompt adb shell command to get Serial Number from samsung device
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Samsung_getSerialNumber(string id)
        {
            string qry = $"-s {id} shell getprop ro.serialno";
            return ExecuteCommandSync.AndroidCommand(qry).Trim();
        }

    }
}
