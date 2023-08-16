using WSOA.Shared.Entity;

namespace WSOA.Server.Business.Interface
{
    public interface IMailService
    {
        void SendMailAccountCreation(LinkAccountCreation link);
    }
}
