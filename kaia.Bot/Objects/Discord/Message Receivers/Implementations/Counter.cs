using Discord;
using Discord.WebSocket;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Discord.Events.Interfaces;
using Kaia.Bot.Objects.Exceptions;
using Kaia.Bot.Objects.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Message_Receivers.Implementations
{
    internal class Counter : IMessageReceiver
    {
        public string Name => "Counter";

        public async Task<bool> CheckMessageValidityAsync(CCBUser Author, SocketMessage Message)
        {
            return Message.Author is SocketGuildUser SUser && (await CCBGuild.GetOrCreateAsync(SUser.Guild.Id)).Settings.CountingChannelId == Message.Channel.Id;
        }

        public async Task RunAsync(CCBUser Author, SocketMessage Message)
        {
            string? Split = Message.Content.Split(' ').FirstOrDefault();
            if (Split != null && decimal.TryParse(Split, out decimal Num))
            {
                if(Message.Author is SocketGuildUser SUser)
                {
                    CCBGuild G = await CCBGuild.GetOrCreateAsync(SUser.Guild.Id);
                    MessageReference Ref = new(Message.Id, Message.Channel.Id, SUser.Guild.Id);
                    ulong LastSuccessfulNumber = G.Settings.LastSuccessfulNumber ?? 0;
                    ulong HighestGuildNumberCounted = G.Settings.HighestCountEver ?? 0;
                    ulong UserHighestCounted = Author.Settings.HighestCountEver ?? 0;
                    ulong UserNumbersCounted = Author.Settings.NumbersCounted ?? 0;
                    bool NotSameUserAsLastTime = G.Settings.LastUserWhoCounted == null || SUser.Id != G.Settings.LastUserWhoCounted || LastSuccessfulNumber == 0;
                    if (Num - 1 == (G.Settings.LastSuccessfulNumber ?? 0) && NotSameUserAsLastTime)
                    {
                        LastSuccessfulNumber++;
                        HighestGuildNumberCounted = HighestGuildNumberCounted > LastSuccessfulNumber ? HighestGuildNumberCounted : LastSuccessfulNumber;
                        UserHighestCounted = UserHighestCounted > LastSuccessfulNumber ? UserHighestCounted : LastSuccessfulNumber;
                        UserNumbersCounted++;
                        
                        await Message.AddReactionAsync(new Random().Next(0, 100) == 50 ? Emotes.CheckRare : Emotes.Check);
                    }
                    else if(NotSameUserAsLastTime)
                    {
                        LastSuccessfulNumber = 0;
                        await Message.AddReactionAsync(Emotes.Invalid);
                        await Message.Channel.SendMessageAsync(Strings.Responses.GetRandomCountingFailText(), messageReference: Ref);
                    }
                    else
                    {
                        await Message.AddReactionAsync(Emotes.ThumbDown);
                        await Message.Channel.SendMessageAsync(Strings.Responses.SameUserTriedCountingTwiceInARow + $" - the next number is `{LastSuccessfulNumber + 1}`.", messageReference: Ref);
                    }
                    await G.ChangeGuildSettings(new(G.Settings.CountingChannelId, LastSuccessfulNumber, Message.Author.Id, HighestGuildNumberCounted));
                    await Author.ChangeUserSettings(new(UserHighestCounted, UserNumbersCounted));
                }
            }
        }

        public Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }
    }
}
