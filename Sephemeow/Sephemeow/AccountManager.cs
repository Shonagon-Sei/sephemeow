using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.Webhook;
using Newtonsoft.Json;
using System.IO;

namespace Sephemeow
{
    public class GuildAccounts
    {
        private static List<GuildAccount> accounts;
        private static string accountsFile = "guild.json";

        static GuildAccounts()
        {
            if (DataStorage.SaveExists(accountsFile))
            {
                accounts = DataStorage.LoadGuildAccounts(accountsFile).ToList();
            }
            else
            {
                accounts = new List<GuildAccount>();
                SaveAccounts();
            }
        }
        public static void SaveAccounts()
        {
            DataStorage.SaveFileGuild(accounts, accountsFile);
        }
        public static GuildAccount GetAccount(SocketGuild guild)
        {
            return GetOrCreateAccount(guild.Id);
        }
        public static GuildAccount GetAccountByID(ulong id)
        {
            return GetOrCreateAccount(id);
        }
        private static GuildAccount GetOrCreateAccount(ulong id)
        {
            var result = from a in accounts
                         where a.id == id
                         select a;

            var account = result.FirstOrDefault();
            if (account == null)
                account = CreateGuildAccount(id);
            return account;
        }
        private static GuildAccount CreateGuildAccount(ulong id)
        {
            var newAccount = new GuildAccount()
            {
                id = id,
                levelAlerts = true
            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
    public class UserAccounts : ModuleBase<SocketCommandContext>
    {
        private static List<UserAccount> accounts;
        private static string accountsFile = "users.json";

        static UserAccounts()
        {
            if (DataStorage.SaveExists(accountsFile))
            {
                accounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
            }
            else
            {
                accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }
        public static void SaveAccounts()
        {
             DataStorage.SaveFileUser(accounts, accountsFile);
        }

        public static UserAccount GetAccount(SocketUser user)
        {
            return GetOrCreateAccount(user.Id);
        }

        public static UserAccount GetIDAccount(ulong user)
        {
            return GetOrCreateAccount(user);
        }

        public static int GetCount()
        {
            var accounted = accounts.Count();
            return accounted;
        }

        public static List<UserAccount> GetSeveralAccount()
        {
            return accounts;
        }

        private static UserAccount GetOrCreateAccount(ulong id)
        {
            var result = from a in accounts
                         where a.id == id
                         select a;

            var account = result.FirstOrDefault();
            if (account == null) 
                account = CreateUserAccount(id);
            return account;
        }
        private static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount()
            {
                id = id,
                decks = new List<(string, string, string)>()
            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }

    }
}
