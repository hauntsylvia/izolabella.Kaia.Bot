using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.Discord.Components
{
    public class KaiaButton : ButtonBuilder, IDisposable
    {
        public KaiaButton(CommandContext Context, string Label, IEmote? Emote, bool Disabled = true, bool AnyoneCanPress = false) : base(Label,
                                                              null,
                                                              ButtonStyle.Secondary,
                                                              null,
                                                              Emote,
                                                              Disabled)
        {
            Context.Reference.ButtonExecuted += ButtonExecutedAsync;
            CustomId = $"{this.Label}-{IdGenerator.CreateNewId().ToString(CultureInfo.InvariantCulture)}";
            Referrer = new(Context.UserContext.User.Id);
            this.Context = Context;
            EnabledUntil = DateTime.UtcNow.AddHours(1);
            this.AnyoneCanPress = AnyoneCanPress;
        }

        public KaiaButton(CommandContext Context, DateTime EnabledUntil, string Label, IEmote? Emote, bool Disabled = true, bool AnyoneCanPress = false) : base(Label,
                                                              null,
                                                              ButtonStyle.Secondary,
                                                              null,
                                                              Emote,
                                                              Disabled)
        {
            Context.Reference.ButtonExecuted += ButtonExecutedAsync;
            CustomId = $"{this.Label}-{IdGenerator.CreateNewId().ToString(CultureInfo.InvariantCulture)}";
            Referrer = new(Context.UserContext.User.Id);
            this.Context = Context;
            this.EnabledUntil = EnabledUntil;
            this.AnyoneCanPress = AnyoneCanPress;
            ControlLoop();
        }

        public string Id => CustomId;

        public KaiaUser Referrer { get; }

        public CommandContext Context { get; }

        public DateTime EnabledUntil { get; }

        public bool AnyoneCanPress { get; }

        public delegate Task ButtonExecHandler(SocketMessageComponent Arg, KaiaUser UserWhoPressed);

        public event ButtonExecHandler? OnButtonPush;

        private async void ControlLoop()
        {
            await Task.Delay(EnabledUntil - DateTime.UtcNow);
            Context.Reference.ButtonExecuted -= ButtonExecutedAsync;
            Dispose();
        }

        private Task ButtonExecutedAsync(SocketMessageComponent Arg)
        {
            if (Arg.IsValidToken && Id == Arg.Data.CustomId && (Arg.User.Id == Referrer.Id || AnyoneCanPress))
            {
                OnButtonPush?.Invoke(Arg, new(Arg.User.Id));
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            /* if u call this.Context.Reference.ButtonExecuted -= this.ButtonExecutedAsync,
             * any embed that uses these buttons in their constructors will no longer work, since
             * this instance is no longer listening for button pushes.
             * 
             * DO NOT "fix" this. that is an anti-design pattern
            */
            OnButtonPush = null;
        }
    }
}
