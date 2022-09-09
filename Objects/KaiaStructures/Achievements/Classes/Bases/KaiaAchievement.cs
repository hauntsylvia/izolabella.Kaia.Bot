﻿using izolabella.Kaia.Bot.Objects.Constants.Enums;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Storage.Objects.Structures;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;

public abstract class KaiaAchievement : IDataStoreEntity
{
    public KaiaAchievement(ulong Id, string Title, string DescriptionWhenAchieved, string DescriptionWhenNotAchieved, AchievementCategory Category, KaiaEmote DisplayEmote, params KaiaUserReward[] Rewards)
    {
        this.Id = Id;
        this.Title = Title;
        this.DescriptionWhenAchieved = DescriptionWhenAchieved;
        this.DescriptionWhenNotAchieved = DescriptionWhenNotAchieved;
        this.Category = Category;
        this.Rewards = Rewards;
        this.DisplayEmote = DisplayEmote;
    }

    public ulong Id { get; }

    public string Title { get; }

    [JsonProperty("WhenAchieved")]
    private string DescriptionWhenAchieved { get; }

    [JsonProperty("WhenNotAchieved")]
    private string DescriptionWhenNotAchieved { get; }

    public AchievementCategory Category { get; }

    public KaiaUserReward[] Rewards { get; }

    public DateTime InitializedAt { get; } = DateTime.UtcNow;

    public KaiaEmote DisplayEmote { get; }

    public abstract Task<bool> CanAwardAsync(KaiaUser U, CommandContext? Context);

    public async Task<bool> UserAlreadyOwns(KaiaUser U)
    {
        return (await U.AchievementProcessor.GetUserAchievementsAsync()).Any(X => X.Id == this.Id);
    }

    public async Task<string> GetDescriptionAsync(KaiaUser U)
    {
        return await this.UserAlreadyOwns(U) ? this.DescriptionWhenAchieved : this.DescriptionWhenNotAchieved;
    }
}