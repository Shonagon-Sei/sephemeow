using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;

using Discord.Interactions;
using System.Threading.Tasks;

using System.Linq;
using System.Net.Http;
using System.Collections.Immutable;
using System.Globalization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

using FuzzySearchNet;
using FuzzySharp;

using System.Xml.Linq;
using Microsoft.VisualBasic;



namespace Sephemeow
{

    public class SlashContexts : InteractionModuleBase<SocketInteractionContext>
    {
        
        public InteractionService Commands { get; set; }

        Color attack = new Color(0xD68248);
        Color magic = new Color(0x3C8EC6);
        Color defense= new Color(0xD49350);
        Color movement = new Color(0x7ADEEF);
        Color jungler = new Color(0x5AD9D0);
        Color support = new Color(0xD4B5BC);

        [SlashCommand("botinfo", "About me!")]
        public async Task aboutMe()
        {
            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl("https://media.discordapp.net/attachments/1073613418227040348/1073613509633507328/6ef07475f4f6544d9c35c21d8327dc17.webp?width=1134&height=1134");
            embed.WithDescription($":flag_us:  Sephemeow - your Arena of Valor assisstant - has come to provide everything you'll ever need :3\n\n:flag_vn: Sephemeow - trợ lý Liên Quân của bạn - đã có mặt để cung cấp tất cả những gì bạn cần :3");
            await Context.Interaction.RespondAsync(embed:embed.Build());
        }

        [SlashCommand("enchantments", "Look at all the data about an enchantment in game")]
        public async Task enchantments([Summary(description: "JP or EN Format"), Choice("Veda", 1), Choice("Lokheim", 2)] int type)
        {

        }
        [SlashCommand("talent", "Look at all the data about a talent in gameaaa")]
        public async Task another()
        {
            await Context.Interaction.DeferAsync();
            string filePath = "talent.json";

            string json = File.ReadAllText(filePath);
            IEnumerable<Talent> items = JsonConvert.DeserializeObject<List<Talent>>(json);

            List<int> matchList = new List<int>();
            var embed = new EmbedBuilder();
            foreach (var i in items)
            {
                if(i.Num.Contains("*"))
                    embed.AddField(i.Icon, i.Desc, true);
                else
                    embed.AddField(i.Icon, i.Desc);
            }


            

            
            await Context.Interaction.FollowupAsync(embed: embed.Build());
        }

        

        [SlashCommand("armory", "Look at all the data about an item in game")]
        public async Task Armory(string search)
        {
            await Context.Interaction.DeferAsync();
            string filePath = "armory.json";

            string json = File.ReadAllText(filePath);
            IEnumerable<Armory> items = JsonConvert.DeserializeObject<List<Armory>>(json);

            List<int> matchList = new List<int>();

            foreach (var i in items)
            {
                matchList.Add(Fuzz.WeightedRatio(i.NameTitle.ToLower(), search.ToLower()));
                Console.WriteLine($"{i.NameTitle} - {Fuzz.Ratio(i.NameTitle, search)}");
                //matchList.Add(LevenshteinDistance(i.NameTitle, search));
            }

            var bestmatch = items.ElementAt(matchList.IndexOf(matchList.Max()));

            //var bestmatch = items.OrderBy(s => string.Compare(s.NameTitle, search)).First();

            var embed = new EmbedBuilder();
            Console.WriteLine(bestmatch.Sec);
            if (bestmatch.Sec == "A")
            {
                embed.WithAuthor("Armory - Attack", "https://media.discordapp.net/attachments/1087004809569251338/1087005035830972478/image182.png?width=256&height=256");
                embed.WithColor(attack);
            }
            else if (bestmatch.Sec == "B")
            {
                embed.WithAuthor("Armory - Magic", "https://media.discordapp.net/attachments/1087004809569251338/1087005184271585372/image384.png?width=256&height=256");
                embed.WithColor(magic);
            }              
            else if (bestmatch.Sec == "C")
            {
                embed.WithAuthor("Armory - Defense", "https://media.discordapp.net/attachments/1087004809569251338/1087005308641099806/41281.png?width=256&height=256");
                embed.WithColor(defense);
            }
            else if (bestmatch.Sec == "D")
            {
                embed.WithAuthor("Armory - Movement", "https://media.discordapp.net/attachments/1087004809569251338/1087005396289466389/011f068e44af36e08efb5814d395f3ee583ec14517012.png?width=256&height=256");
                embed.WithColor(movement);
            }             
            else if (bestmatch.Sec == "E")
            {
                embed.WithAuthor("Armory - Jungler", "https://media.discordapp.net/attachments/1087004809569251338/1087005539352989736/64.png?width=128&height=128");
                embed.WithColor(jungler);
            }               
            else if (bestmatch.Sec == "G")
            {
                embed.WithAuthor("Armory - Support", "https://media.discordapp.net/attachments/1087004809569251338/1087005615009841192/icona78636f147efe1ac850a5495c044a9565ef2df9dd61a3.png?width=200&height=200");
                embed.WithColor(support);
            }
                

            embed.WithTitle(bestmatch.NameTitle);
            embed.AddField("<:Shop:1088471353797136404> Price", $"{bestmatch.PriceField1}<:Gold:1088472406462574733>", true);
            embed.AddField("Attributes", $"{bestmatch.AttributesEmotesField2}", true);
            embed.WithThumbnailUrl(bestmatch.IconURLThumbnail);
            if(bestmatch.PassivesField3 != "N/A" && bestmatch.PassivesField3 != "")
                embed.AddField("Passive", $"{bestmatch.PassivesField3}");
            embed.WithFooter(bestmatch.NoticeFooter);
            await Context.Interaction.FollowupAsync(embed:embed.Build());

        }
        public static int LevenshteinDistance(string source, string target)
        {
            if (String.IsNullOrEmpty(source))
            {
                if (String.IsNullOrEmpty(target)) return 0;
                return target.Length;
            }
            if (String.IsNullOrEmpty(target)) return source.Length;

            if (source.Length > target.Length)
            {
                var temp = target;
                target = source;
                source = temp;
            }

            var m = target.Length;
            var n = source.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for (var j = 1; j <= m; j++) distance[0, j] = j;

            var currentRow = 0;
            for (var i = 1; i <= n; ++i)
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                var previousRow = currentRow ^ 1;
                for (var j = 1; j <= m; j++)
                {
                    var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
                    distance[currentRow, j] = Math.Min(Math.Min(
                        distance[previousRow, j] + 1,
                        distance[currentRow, j - 1] + 1),
                        distance[previousRow, j - 1] + cost);
                }
            }
            return distance[currentRow, m];
        }
    }
    
}
