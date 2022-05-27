using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    internal class InformationCommand : IKaiaCommand
    {
        public string ForeverId => CommandForeverIds.BotDevelopmentInformation;

        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public bool GuildsOnly => throw new NotImplementedException();

        public List<IzolabellaCommandParameter> Parameters => throw new NotImplementedException();

        public List<IIzolabellaCommandConstraint> Constraints => throw new NotImplementedException();

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            throw new NotImplementedException();
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            throw new NotImplementedException();
        }

        public Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            throw new NotImplementedException();
        }
    }
}
