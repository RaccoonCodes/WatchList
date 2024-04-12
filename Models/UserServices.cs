using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace WatchList.Models
{
    public class UserServices
    {
        private readonly InfoDbContext context;

        public UserServices(InfoDbContext context) => this.context = context; 

        public int? GetID(string loginName, string password)
        {
            var user = context.UsersDBModels
                .FirstOrDefault(u => u.UserName == loginName && u.Password == password);

            return user?.UserID;

        }

        public LoginModel? AuthenticateUSer(string loginName, string password)
        {
            var userID= GetID(loginName, password);
            
            if(userID.HasValue)
            {
                return new LoginModel
                {
                    UserID = userID.Value,
                    LoginName = loginName,
                    Password = password
                };
            }
            else { return null; }
        }
    }
}
