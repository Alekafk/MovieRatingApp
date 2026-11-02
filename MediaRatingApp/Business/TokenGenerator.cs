using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaRatingApp.Business
{
    public class TokenGenerator
    {
        public static string GenerateToken() {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            //const string symbols = "!@#$%^&*~";
            var random = new Random();
            int length = 50;
            var token = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int a = random.Next(chars.Length);
                token.Append(chars[a]);
                token.Append("*");
            }
            return token.ToString();
        }
    }
}
