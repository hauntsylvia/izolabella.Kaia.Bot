using Discord;
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
    internal class MyLibrary : ICCBCommand
    {
        public string ForeverId => CommandForeverIds.MyLibrary;

        public string Name => "My-Library";

        public string Description => "View my collection of books.";

        public List<IzolabellaCommandParameter> Parameters { get; } = new()
        {
            Arguments.SomeoneOtherThanMeUser,
        };

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            return Task.CompletedTask;
        }

        public Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Args)
        {
            IzolabellaCommandArgument? UserArg = Args.FirstOrDefault(A => A.Name.ToLower() == Arguments.SomeoneOtherThanMeUser.Name.ToLower());
            IUser U = UserArg != null && UserArg.Value is IUser DU ? DU : Context.UserContext.User;
            return Task.CompletedTask;
        }
    }
}
