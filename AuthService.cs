using System.Collections.Generic;
using System.Linq;
using My_budget.Models;

namespace My_budget;

public class AuthService
{
    private List<Users> UsersList;

    public AuthService(List<Users> usersList)
    {
        this.UsersList = usersList;
    }

    public Users Authenticate(string email, string password)
    {
        return UsersList.FirstOrDefault(u => u.email == email && u.password == password);
    }
}