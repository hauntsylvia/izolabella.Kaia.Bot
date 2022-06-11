namespace Kaia.Bot.Objects.Discord.MessageReceivers.Implementations
{
    internal class Counter : IMessageReceiver
    {
        public string Name => "Counter";

        public Task<bool> CheckMessageValidityAsync(KaiaUser Author, SocketMessage Message)
        {
            return Task.FromResult(Message.Author is SocketGuildUser SUser && new KaiaGuild(SUser.Guild.Id).Settings.CountingChannelId == Message.Channel.Id);
        }

        public async Task<MessageReceiverResult> RunAsync(KaiaUser Author, KaiaGuild? Guild, SocketMessage Message)
        {
            MessageReceiverResult Result = new(false, false);
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
                        KaiaInventoryItem? Refresh = Author.Settings.Inventory.Items.FirstOrDefault(InvI =>
                        {
                            return InvI.DisplayName == Strings.ItemStrings.CountingRefresher.Name;
                        });
                        if (Refresh != null)
                        {
                            Result.ItemToUse = Refresh;
                            await Message.AddReactionAsync(Refresh.DisplayEmote);
                            global::Discord.Rest.RestUserMessage? unused2 = await Message.Channel.SendMessageAsync(Strings.Responses.Counting.UserCountingSaved + $" - the next number is `{LastSuccessfulNumber + 1}`.", messageReference: Ref);
                        }
                        else
                        {
                            LastSuccessfulNumber = 0;
                            await Message.AddReactionAsync(Emotes.Counting.Invalid);
                            global::Discord.Rest.RestUserMessage? unused1 = await Message.Channel.SendMessageAsync(Strings.Responses.Counting.GetRandomCountingFailText(), messageReference: Ref);
                        }
                    }
                    else
                    {
                        await Message.AddReactionAsync(Emotes.Counting.ThumbDown);
                        global::Discord.Rest.RestUserMessage? unused = await Message.Channel.SendMessageAsync(Strings.Responses.Counting.SameUserTriedCountingTwiceInARow + $" - the next number is `{LastSuccessfulNumber + 1}`.", messageReference: Ref);
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

        public Task CallbackAsync(KaiaUser Author, SocketMessage Message, MessageReceiverResult CausedCallback)
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }
    }
}
