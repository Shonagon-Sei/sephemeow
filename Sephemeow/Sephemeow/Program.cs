using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using Discord.Net;
using Discord.Rest;
using Discord.Webhook;
using Discord.Audio;
using System.Linq;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO.Enumeration;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

using Discord.API;
using Discord.Commands.Builders;
using Discord.Net.Queue;
using Discord.Net.WebSockets;
using Discord.Net.Converters;
using Discord.Net.Udp;
using Discord.Audio.Streams;
using Newtonsoft.Json;
using Fergun.Interactive;
using System.Text.RegularExpressions;
using System.ComponentModel;

using System.ComponentModel.DataAnnotations;

namespace Sephemeow
{
    public class Program
    {
        public static DiscordSocketClient Client;
        private CommandService Commands;
        public InteractionService _interactionService;

        public static IServiceProvider services;
        public static Random rnd = new Random();




        static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {


            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                //AlwaysAcknowledgeInteractions = true,
                //GatewayIntents = GatewayIntents.AllUnprivileged & GatewayIntents.GuildMembers
                GatewayIntents = GatewayIntents.All



            });

            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = Discord.Commands.RunMode.Async,
                LogLevel = LogSeverity.Debug,



            });

            _interactionService = new InteractionService(Client.Rest);

            services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(Commands)
                .AddSingleton(_interactionService)
                .AddSingleton(new InteractiveConfig { DefaultTimeout = TimeSpan.FromMinutes(5) })
                .AddSingleton<InteractiveService>()
                .BuildServiceProvider();
            Commands = new CommandService();
            Commands.AddTypeReader<SocketGuild>(new GuildTypeReader<SocketGuild>());
            Commands.AddTypeReader<Emote>(new EmoteTypeReader());
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            //Client.MessageReceived += Client_MessageReceived;

            Client.Ready += Client_Ready;

            Client.Log += Client_Log;
            Client.ButtonExecuted += MyButtonHandler;
            Client.ModalSubmitted += ModalSubmitted;

            Client.SlashCommandExecuted += async (interaction) =>
            {
                var ctx = new SocketInteractionContext(Client, interaction);
                await _interactionService.ExecuteCommandAsync(ctx, services);
            };




            string load = File.ReadAllText("Token.txt");
            
            await Client.LoginAsync(TokenType.Bot, load);
            await Client.StartAsync();



            await Task.Delay(-1);

        }
        private async Task Client_Log(LogMessage Message)
        {
            Console.WriteLine($"{DateTime.Now} at  {Message.Source}] {Message.Message}");
        }
        private async Task Client_SlashCommand(SocketSlashCommand command)
        {
            var Context = new SocketInteractionContext(Client, command);
            await _interactionService.ExecuteCommandAsync(Context, services);


        }

        private async Task Client_Ready()
        {
            //await Client.Rest.DeleteAllGlobalCommandsAsync();
            //List<ApplicationCommandProperties> applicationCommandProperties = new List<ApplicationCommandProperties>();

            await Client.SetGameAsync($"Grinding to Legendary");
            await _interactionService.RegisterCommandsGloballyAsync();

        }

        
        private async Task ModalSubmitted(SocketModal modal)
        {

        }
        private async Task MyButtonHandler(SocketMessageComponent interaction)
        {

            var id = interaction.Data.CustomId;

            
            
        }
        private async Task Client_MessageReceived(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            if (Message == null)
            {
                return;
            }
            var Context = new SocketCommandContext(Client, Message);

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot || Context.User.Id == 624874883947560991) return;

            int ArgPos = 0;

            string prefix = "-";
            if (Client.CurrentUser.Id == 1011562292535623760)
            {
                prefix = "=";
            }

            if (!(Message.HasStringPrefix("-", ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos)))
            {
                return;
            }
            var Result = await Commands.ExecuteAsync(Context, ArgPos, services);
            if (!(Context.Channel is IPrivateChannel))
            {
                if (!Result.IsSuccess)
                {
                    Console.WriteLine($"{DateTime.Now} at Commands] Something went wrong with executing a command. Text: {Context.Message.Content} | Error {Result.ErrorReason}");
                    if (Result.Error != CommandError.UnknownCommand)
                        await Context.Channel.SendMessageAsync($"Something went wrong with executing the command. `Error: {Result.ErrorReason}`");
                }
            }

        }

    }
}