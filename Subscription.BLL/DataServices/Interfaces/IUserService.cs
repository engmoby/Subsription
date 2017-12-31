using Service.Pattern;
using Subscription.BLL.DTOs;
using Subscription.DAL.Entities.Model;

namespace Subscription.BLL.DataServices.Interfaces
{
    public interface IUserService:IService<User>
    {
        User ValidateUser(string email, string password);
        bool CheckEmailDuplicated(string email);
        bool CheckPhoneDuplicated(string phone);
        User CheckUserIsDeleted(string firstName, string password);
        PagedResultsDto GetAllUsers(int page, int pageSize); 
    }
}
