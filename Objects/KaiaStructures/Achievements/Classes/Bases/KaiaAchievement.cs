using izolabella.Discord.Objects.Arguments;
using izolabella.Storage.Objects.Structures;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.KaiaStructures.Users;
using Kaia.Bot.Objects.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases
{
    public abstract class KaiaAchievement : IDataStoreEntity
    {
        public KaiaAchievement(ulong Id, string Title, string Description, AchievementCategory Category, params KaiaAchievementReward[] Rewards)
        {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;
            this.Category = Category;
            this.Rewards = Rewards;
        }

        public ulong Id { get; }
        public string Title { get; }
        public string Description { get; }
        public AchievementCategory Category { get; }
        public KaiaAchievementReward[] Rewards { get; }
        public DateTime InitializedAt { get; } = DateTime.UtcNow;
        public abstract Task<bool> CanAwardAsync(KaiaUser U, CommandContext? Context);
        public async Task<bool> UserAlreadyOwns(KaiaUser U)
        {
            return ((await U.Settings.AchievementProcessor.GetUserAchievementsAsync()).Any(X => X.Id == this.Id));
        }
    }
}