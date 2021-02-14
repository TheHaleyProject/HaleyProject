using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Abstractions
{
    public interface ISubscriber
    {
        string id { get; set; }
        Type declaring_type { get;}
        string listener_method { get; set; }
        void sendMessage(params object[] args);
    }
}
