using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Framework.OAuth
{
    public class DefaultUsers
    {
        private static readonly IDictionary<string, string> Dictionary = new Dictionary<string, string>();
        private static readonly IDictionary<string, string[]> DictionaryRole = new Dictionary<string, string[]>();

        static DefaultUsers()
        {
            Add("Admin","Admin1234");
            AddRole("Admin", new[] { Role.Admin.ToString()});
        }

        public static void Add(string key, string value)
        {
            if (!Dictionary.ContainsKey(key))
            {
                Dictionary.Add(key, value);
            }
        }

        public static void AddRole(string key, string[] value)
        {
            if (!DictionaryRole.ContainsKey(key))
            {
                DictionaryRole.Add(key,value);
            }
        }

        public static string[] GetRoles(string key)
        {
            return DictionaryRole[key];
        }

        public static bool IsDefaultUser(string userId)
        {
            if (Dictionary.ContainsKey(userId))
            {
                return true;
            }
            return false;
        }

        public static bool IsDefaultUser(string userId, string password)
        {
            string pass;
            if (!Dictionary.TryGetValue(userId,out pass))
            {
                return false;
            }
            if (pass == password)
            {
                return true;
            }
            return false;
        }
    }
}
