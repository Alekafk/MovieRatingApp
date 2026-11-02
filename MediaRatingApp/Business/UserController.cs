using MediaRatingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediaRatingApp.Models;

namespace MediaRatingApp.Business
{
    public class UserController
    {
        public static List<User> Users = new List<User>();
        public static bool UserExists(string username) => Users.Exists(u => u.Username == username);
        public static void AddUser(User user) => Users.Add(user);
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d).{8,}$");
            return regex.IsMatch(password);
        }
    }
}
