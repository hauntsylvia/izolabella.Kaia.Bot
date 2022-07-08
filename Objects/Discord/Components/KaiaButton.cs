using izolabella.Util;

namespace Kaia.Bot.Objects.Discord.Components
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
            Context.Reference.ButtonExecuted += this.ButtonExecutedAsync;
            this.CustomId = $"{this.Label}-{IdGenerator.CreateNewId().ToString(CultureInfo.InvariantCulture)}";
            this.Referrer = new(Context.UserContext.User.Id);
            this.Context = Context;
            this.EnabledUntil = DateTime.UtcNow.AddHours(1);
            this.AnyoneCanPress = AnyoneCanPress;
        }

        public KaiaButton(CommandContext Context, DateTime EnabledUntil, string Label, IEmote? Emote, bool Disabled = true, bool AnyoneCanPress = false) : base(Label,
                                                              null,
                                                              ButtonStyle.Secondary,
                                                              null,
                                                              Emote,
                                                              Disabled)
        {
            Context.Reference.ButtonExecuted += this.ButtonExecutedAsync;
            this.CustomId = $"{this.Label}-{IdGenerator.CreateNewId().ToString(CultureInfo.InvariantCulture)}";
            this.Referrer = new(Context.UserContext.User.Id);
            this.Context = Context;
            this.EnabledUntil = EnabledUntil;
            this.AnyoneCanPress = AnyoneCanPress;
            this.ControlLoop();
        }

        public string Id => this.CustomId;

        public KaiaUser Referrer { get; }

        public CommandContext Context { get; }

        public DateTime EnabledUntil { get; }

        public bool AnyoneCanPress { get; }

        public delegate Task ButtonExecHandler(SocketMessageComponent Arg, KaiaUser UserWhoPressed);

        public event ButtonExecHandler? OnButtonPush;

        private async void ControlLoop()
        {
            await Task.Delay(this.EnabledUntil - DateTime.UtcNow);
            this.Context.Reference.ButtonExecuted -= this.ButtonExecutedAsync;
            this.Dispose();
        }

        private Task ButtonExecutedAsync(SocketMessageComponent Arg)
        {
            if(Arg.IsValidToken && this.Id == Arg.Data.CustomId && (Arg.User.Id == this.Referrer.Id || this.AnyoneCanPress))
            {
                this.OnButtonPush?.Invoke(Arg, new(Arg.User.Id));
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
            this.OnButtonPush = null;
        }
    }
}
