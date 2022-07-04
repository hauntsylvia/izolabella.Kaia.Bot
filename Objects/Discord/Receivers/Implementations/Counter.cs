﻿using Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Implementations;

namespace Kaia.Bot.Objects.Discord.Receivers.Implementations
{
    public class Counter : Receiver
    {
        public override string Name => "Counter";

        public override Task<bool> CheckMessageValidityAsync(KaiaUser Author, SocketMessage Message)
        {
            return Task.FromResult(Message.Author is SocketGuildUser SUser && new KaiaGuild(SUser.Guild.Id).Settings.CountingChannelId == Message.Channel.Id);
        }

        public override async Task<ReceiverResult> OnMessageAsync(KaiaUser Author, KaiaGuild? Guild, SocketMessage Message)
        {
            ReceiverResult Result = new();
            string? Split = Message.Content.Split(' ').FirstOrDefault();
            if (Split != null && double.TryParse(Split, out double Num))
            {
                if (Guild != null)
                {
                    MessageReference Ref = new(Message.Id, Message.Channel.Id, Guild.Id);
                    ulong LastSuccessfulNumber = Guild.Settings.LastSuccessfulNumber;
                    ulong HighestGuildNumberCounted = Guild.Settings.HighestCountEver ?? 0;
                    ulong UserHighestCounted = Author.Settings.HighestCountEver ?? 0;
                    ulong UserNumbersCounted = Author.Settings.NumbersCounted ?? 0;
                    bool NotSameUserAsLastTime = Guild.Settings.LastUserWhoCounted == null || Author.Id != Guild.Settings.LastUserWhoCounted || LastSuccessfulNumber == 0;
                    if (Num - 1 == Guild.Settings.LastSuccessfulNumber && NotSameUserAsLastTime)
                    {
                        LastSuccessfulNumber++;
                        HighestGuildNumberCounted = HighestGuildNumberCounted > LastSuccessfulNumber ? HighestGuildNumberCounted : LastSuccessfulNumber;
                        UserHighestCounted = UserHighestCounted > LastSuccessfulNumber ? UserHighestCounted : LastSuccessfulNumber;
                        UserNumbersCounted++;

                        bool RareCheck = new Random().Next(100) < 2;
                        await Message.AddReactionAsync(RareCheck ? Emotes.Counting.CheckRare : Emotes.Counting.Check);
                        if (RareCheck)
                        {
                            Author.Settings.Inventory.Petals += new Random().Next(10, 100) + (double)new Random().NextDouble();
                        }
                    }
                    else if (NotSameUserAsLastTime)
                    {
                        KaiaInventoryItem? Refresh = await Author.Settings.Inventory.GetItemOfDisplayName(new CountingRefresher());
                        if (Refresh != null)
                        {
                            Result.ItemToUse = Refresh;
                            await Message.AddReactionAsync(Refresh.DisplayEmote);
                            _ = await Message.Channel.SendMessageAsync(Strings.Responses.Counting.UserCountingSaved + $" - the next number is `{LastSuccessfulNumber + 1}`.", messageReference: Ref);
                        }
                        else
                        {
                            LastSuccessfulNumber = 0;
                            await Message.AddReactionAsync(Emotes.Counting.Invalid);
                            _ = await Message.Channel.SendMessageAsync(Strings.Responses.Counting.GetRandomCountingFailText(), messageReference: Ref);
                        }
                    }
                    else
                    {
                        await Message.AddReactionAsync(Emotes.Counting.ThumbDown);
                        _ = await Message.Channel.SendMessageAsync(Strings.Responses.Counting.SameUserTriedCountingTwiceInARow + $" - the next number is `{LastSuccessfulNumber + 1}`.", messageReference: Ref);
                    }
                    Guild.Settings.LastSuccessfulNumber = LastSuccessfulNumber;
                    Guild.Settings.HighestCountEver = HighestGuildNumberCounted;
                    Guild.Settings.LastUserWhoCounted = Message.Author.Id;
                    Author.Settings.HighestCountEver = UserHighestCounted;
                    Author.Settings.NumbersCounted = UserNumbersCounted;
                    await Guild.SaveAsync();
                    await Author.SaveAsync();
                }
            }
            return Result;
        }

        public override Task CallbackAsync(KaiaUser Author, SocketMessage Message, ReceiverResult CausedCallback)
        {
            return Task.CompletedTask;
        }

        public override Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }
    }
}