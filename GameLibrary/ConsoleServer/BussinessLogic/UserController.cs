using ConsoleServer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.BussinessLogic
{
    public class UserController
    {
        private List<User> users;

        public UserController()
        {
            users = new List<User>();
        }

        public void TryAddUser(string name)
        {
            User newUser = new User { Name = name };
            if (!users.Contains(newUser))
                users.Add(newUser);
        }

        public void BuyGame(string username, string gameName)
        {
            throw new NotImplementedException();
        }
    }
}
