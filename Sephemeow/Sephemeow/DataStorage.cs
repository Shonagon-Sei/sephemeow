using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Sephemeow
{
    public static class DataStorage

    {

        public static void SaveFileUser(IEnumerable<UserAccount> accounts, string filePath)

        {

            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);

            File.WriteAllText(filePath, json);

        }

        public static void SaveFileGuild(IEnumerable<GuildAccount> accounts, string filePath)

        {

            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);

            File.WriteAllText(filePath, json);

        }

        public static IEnumerable<UserAccount> LoadUserAccounts(string filePath)

        {

            if (!File.Exists(filePath)) return null;

            string json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<List<UserAccount>>(json);

        }

        public static IEnumerable<GuildAccount> LoadGuildAccounts(string filePath)

        {

            if (!File.Exists(filePath)) return null;

            string json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<List<GuildAccount>>(json);

        }

        public static bool SaveExists(string filePath)
        {

            return File.Exists(filePath);

        }

    }
}
