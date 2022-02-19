using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace svetotelegraf
{
    public interface ISignal
    {
        Task flash(int len, bool sound);
        void SendNotifcatiom();
        void RemoveNotification();
    }
}
