using System.Collections.Generic;
using System.Management;
using System.ServiceModel;

namespace MyWCF_WMI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetIp();
        [OperationContract]    
        string GetMachine_Directoy();
        [OperationContract]
        IList<ManagementBaseObject> GetPrinters();
    }
}
