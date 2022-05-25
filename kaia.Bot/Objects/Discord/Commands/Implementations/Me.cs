using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Discord.Embeds.Implementations;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Constants;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Discord.WebSocket;
using Discord;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.User_Data;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class Me : ICCBCommand
    {
        public string Name => "Me";

        public string Description => "View my or another user's statistics.";

        public List<IzolabellaCommandParameter> Parameters => new()
        {
            new("User", "The user I'd like to view.", ApplicationCommandOptionType.User, false),
            new("Library", "If true, I'd like to view my library.", ApplicationCommandOptionType.Boolean, false)
        };

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new();

        public string ForeverId => CommandForeverIds.MeCommand;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? UserArg = Arguments.FirstOrDefault(A => A.Name.ToLower() == "user");
            IzolabellaCommandArgument? LibraryViewArg = Arguments.FirstOrDefault(A => A.Name.ToLower() == "library");
            bool ViewLibrary = LibraryViewArg != null && LibraryViewArg.Value is bool V && V;
            IUser U = UserArg != null && UserArg.Value is IUser DU ? DU : Context.UserContext.User;
            if(U.Id == Context.UserContext.User.Id)
            {
                await (!ViewLibrary ? new MeInventory(new(U.Id), Context, 4).StartAsync() : new MeLibrary(new(U.Id), Context, 10).StartAsync());
            }
            else
            {
                await Context.UserContext.RespondAsync(text: "", embed: new MeView(U.Username, new(U.Id)).Build());
            }
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
