using izolabella.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Context.Reference.Client.ButtonExecuted += this.ButtonExecutedAsync;
            this.CustomId = this.Id;
            this.Referrer = new(Context.UserContext.User.Id);
            this.Context = Context;
            this.AnyoneCanPress = AnyoneCanPress;
        }

        public string Id { get; } = IdGenerator.CreateNewId().ToString(CultureInfo.InvariantCulture);

        public KaiaUser Referrer { get; }

        public CommandContext Context { get; }

        public bool AnyoneCanPress { get; }

        public delegate Task ButtonExecHandler(SocketMessageComponent Arg, KaiaUser UserWhoPressed);

        public event ButtonExecHandler? OnButtonPush;

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
            this.Context.Reference.Client.ButtonExecuted -= this.ButtonExecutedAsync;
            this.OnButtonPush = null;
        }
    }
}
