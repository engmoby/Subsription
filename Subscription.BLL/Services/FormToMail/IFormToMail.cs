using System.IO;

namespace Subscription.BLL.Services.FormToMail
{
    public interface IFormToMail
    {
        void SendMail(string subj, string message,string mailTo);
    }
}
