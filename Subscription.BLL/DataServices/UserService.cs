using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Subscription.BLL.DataServices.Interfaces;
using Subscription.BLL.DTOs;
using Subscription.Common;
using Subscription.DAL.Entities.Model;

namespace Subscription.BLL.DataServices
{
    public class UserService : Service<User>, IUserService
    {
        public UserService(IRepositoryAsync<User> repository) : base(repository)
        {
            _repository = repository;
        }
        public User ValidateUser(string email, string password)
        {
            return _repository.Query(u => u.Email.ToLower() ==  email.ToLower() && u.Password == password && !u.IsDeleted).Select().FirstOrDefault();

        }
        public User CheckUserIsDeleted(string email, string password)
        {
            return _repository.Query(u => u.Email.ToLower() == email.ToLower() && u.Password == password).Select().FirstOrDefault();

        }
        public bool CheckEmailDuplicated(string email)
        {
            return _repository.Queryable().Any(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted &&  u.Role == Enums.RoleType.Client);
        }
        public bool CheckPhoneDuplicated(string phone)
        {
            return _repository.Queryable().Any(u => u.Phone1 == phone.ToLower() && !u.IsDeleted && u.Role == Enums.RoleType.Client);
        }
        public PagedResultsDto GetAllUsers(int page, int pageSize)
        { 
            //var query = Queryable().Where(x => x.IsActive); 
            var query = Queryable().Where(x=>x.Role== Enums.RoleType.Client).OrderBy(x => x.UserId); 
            PagedResultsDto results = new PagedResultsDto();
            results.TotalCount = query.Select(x => x).Count();
            
            results.Data = Mapper.Map<List<User>, List<UserDto>>(query.OrderBy(x => x.UserId).Skip((page - 1) * pageSize)
                .Take(pageSize).ToList());

            return results;
        }

    }
}