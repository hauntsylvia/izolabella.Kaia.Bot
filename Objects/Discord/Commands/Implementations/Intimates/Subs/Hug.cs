﻿using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Intimates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Intimates.Subs
{
    public class Hug : KaiaSubCommand
    {
        public override string ForeverId => CommandForeverIds.Acts.Hug;

        public override List<GuildPermission> RequiredPermissions => new Act().RequiredPermissions;

        public override string Name => "Hug";

        public override string Description => "Hug someone!";

        public override bool GuildsOnly => true;

        public override List<IzolabellaCommandParameter> Parameters => new Act().Parameters;

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? OtherUser = Arguments.FirstOrDefault(A => A.Name == "user");
            if(OtherUser != null && OtherUser.Value is IUser U)
            {

                IntimateDisplay D = new(new("hug", "hugs"), new(Context.UserContext.User.Id), new(U.Id));
                await Context.UserContext.RespondAsync(embed: D.Build());
            }
        }
    }
}
