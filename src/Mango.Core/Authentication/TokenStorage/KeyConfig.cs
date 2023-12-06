using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Authentication.TokenStorage
{
    internal static class KeyConfig
    {
        public static string GetTokenKey(string key)
        {
            return $"JwtToken:{key}";
        }
    }
}
