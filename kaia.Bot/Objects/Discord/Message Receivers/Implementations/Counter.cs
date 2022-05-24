﻿using Discord;
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
using Kaia.Bot.Objects.CCB_Structures.Users;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Properties;

namespace Kaia.Bot.Objects.Discord.Message_Receivers.Implementations
{
    internal class Counter : IMessageReceiver
    {
        public string Name => "Counter";

        public Task<bool> CheckMessageValidityAsync(CCBUser Author, SocketMessage Message)
        {
            return Task.FromResult(Message.Author is SocketGuildUser SUser && new CCBGuild(SUser.Guild.Id).Settings.CountingChannelId == Message.Channel.Id);
        }

        public async Task RunAsync(CCBUser Author, SocketMessage Message)
        {
            string? Split = Message.Content.Split(' ').FirstOrDefault();
            if (Split != null && decimal.TryParse(Split, out decimal Num))
            {
                if(Message.Author is SocketGuildUser SUser)
                {
                    CCBGuild G = new(SUser.Guild.Id);
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

                        bool RareCheck = new Random().Next(0, 100) == 50;
                        await Message.AddReactionAsync(RareCheck ? Emotes.Counting.CheckRare : Emotes.Counting.Check);
                        if(RareCheck)
                        {
                            Author.Settings.Inventory.Petals += new Random().Next(10, 100) + (decimal)new Random().NextDouble();
                        }
                    }
                    else if(NotSameUserAsLastTime)
                    {
                        LastSuccessfulNumber = 0;
                        await Message.AddReactionAsync(Emotes.Counting.Invalid);
                        await Message.Channel.SendMessageAsync(Strings.Responses.GetRandomCountingFailText(), messageReference: Ref);
                    }
                    else
                    {
                        await Message.AddReactionAsync(Emotes.Counting.ThumbDown);
                        await Message.Channel.SendMessageAsync(Strings.Responses.SameUserTriedCountingTwiceInARow + $" - the next number is `{LastSuccessfulNumber + 1}`.", messageReference: Ref);
                    }
                    G.Settings.LastSuccessfulNumber = LastSuccessfulNumber;
                    G.Settings.HighestCountEver = HighestGuildNumberCounted;
                    G.Settings.LastUserWhoCounted = Message.Author.Id;
                    G.Settings = G.Settings;
                    Author.Settings.HighestCountEver = UserHighestCounted;
                    Author.Settings.NumbersCounted = UserNumbersCounted;
                    await Author.SaveAsync();
                }
            }
        }

        public Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }
    }
}
