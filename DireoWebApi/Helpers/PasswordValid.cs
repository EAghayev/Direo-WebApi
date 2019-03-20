using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Helpers
{
    public class PasswordValid
    {
        public static string Message;
        public static bool Valid(string password)
        {
            if (password.Length < 8)
            {
                Message = "Password must long from 8";
                return false;
            }

            if (!password.Any(p => char.IsDigit(p)))
            {
                Message = "Password must have digit";
                return false;
            }

            if (!password.Any(p => char.IsUpper(p)))
            {
                Message = "Password must have uppercase";
                return false;
            }

            if (!password.Any(p => char.IsLower(p)))
            {
                Message = "Password must have lowercase";
                return false;
            }

            return true;
        }
    }
}
