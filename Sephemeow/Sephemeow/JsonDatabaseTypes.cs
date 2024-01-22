using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sephemeow
{
    /*
    "Sec": "A",
    "Col": 1,
    "Num": "01",
    "Icon (Preview)": "",
    "Icon URL [Thumbnail]": "https://media.discordapp.net/attachments/1086304598370816031/1086308446376296529/image56.png?width=200&height=200",
    "Name [Title]": "Gloves",
    "Price [Field 1]*": 300,
    "Attributes (+ Emotes) [Field 2]*": "<:CritRate:1088333019326189618> + 8% Critical Chance",
    "Passives [Field 3]": "N/A",
    "Tag": "GL",
    "Label": "A101 - [GL] Gloves",
    "Notice [Footer]": "",
    "": "",
    "__1": "* Enable Inline"
    */

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Armory
    {
        public string Sec { get; set; }
        public int Col { get; set; }
        public object Num { get; set; }

        [JsonProperty("Icon (Preview)")]
        public string IconPreview { get; set; }

        [JsonProperty("Icon URL [Thumbnail]")]
        public string IconURLThumbnail { get; set; }

        [JsonProperty("Name [Title]")]
        public string NameTitle { get; set; }

        [JsonProperty("Price [Field 1]*")]
        public object PriceField1 { get; set; }

        [JsonProperty("Attributes (+ Emotes) [Field 2]*")]
        public string AttributesEmotesField2 { get; set; }

        [JsonProperty("Passives [Field 3]")]
        public string PassivesField3 { get; set; }
        public string Tag { get; set; }
        public string Label { get; set; }

        [JsonProperty("Notice [Footer]")]
        public string NoticeFooter { get; set; }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Talent
    {
        public string Num { get; set; }
        public string Icon { get; set; }
        public string Desc { get; set; }
    }



    public class UserAccount
    {
        public ulong id { get; set; }
        public List<(string, string, string)> decks { get; set; }
        public ulong xp { get; set; }
        public ulong level { get; set; }
        public bool premium { get; set; }
        public Dictionary<string, string> favorites { get; set; }
    }
    public class GuildAccount
    {
        public ulong id { get; set; }
        public bool levelAlerts { get; set; }
        public List<(string, ulong, int, int, int)> ranked { get; set; }
        public ulong gameNumber { get; set; }
        public List<(ulong, ulong, ulong, bool)> queuedRecord { get; set; }

        //Swiss Tour
        public List<SocketUser> tourPlayers { get; set; }

    }
}
