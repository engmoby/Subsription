using System.IO;

namespace Subscription.BLL.Services.ManageStorage
{
    public interface IFormToMail
    {
        void SendMail(string subj, string message,string mailTo);
    }
}
