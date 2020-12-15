using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManage
{
    class DataBridge
    {
        static DataBridge dataBridge;

        private Dictionary<string, object> keyValues = new Dictionary<string, object>(); 
        private DataBridge()
        {

        }

        public static DataBridge GetInstance()
        {
            if (dataBridge == null)
            {
                dataBridge = new DataBridge();
            }
            return dataBridge;
        }

        public Dictionary<string, object> getDictionary()
        {
            return keyValues;
        }

        public void clear()
        {
            keyValues = new Dictionary<string, object>();
        }
    }
}
