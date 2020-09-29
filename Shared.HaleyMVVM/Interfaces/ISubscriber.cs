using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.MVVM.Interfaces
{
    public interface ISubscriber
    {
        string id { get; set; }
        void sendMessage(params object[] args);
    }
}
