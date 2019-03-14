using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty_Server
{

    /// <summary>
    /// Klasa pomocnicza
    /// </summary>
    public class Util
    {
        public static string generateGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8);
        }
    }
}
