using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Self;
using izolabella.Music.Structure.Music.Songs;
using izolabella.Music.Structure.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Self;

public class LoFiStats : KaiaPathEmbedRefreshable
{
    public LoFiStats(ulong DiscordId) : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.LoFi)
    {
        this.DiscordId = DiscordId;
    }

    public ulong DiscordId { get; }

    private const int take = 4;

    private static string FormatTimeSpan(TimeSpan Format)
    {
        return $"{(Format.TotalDays >= 1 ? $"`{Format.Days}d`, " : "")}" +
               $"{(Format.TotalHours >= 1 ? $"`{Format.Hours}h`, " : "")}" +
               $"{(Format.TotalMinutes >= 1 ? $"`{Format.Minutes}min`, " : "")}" +
               $"{(Format.TotalSeconds >= 1 ? $"`{Format.Seconds}s`" : "")}";
    }

    protected override async Task ClientRefreshAsync()
    {
        LoFiUser? User = await LoFiUser.Get(this.DiscordId, DataStores.LoFiUserStore);
        if(User != null)
        {
            this.WithField("Alias", $"`{User.Profile.DisplayName}`");
            List<LoFiUserSongListened> Listens = await LoFi.Server.Structures.Constants.DataStores.GetUsersListensStore(User.Id).ReadAllAsync<LoFiUserSongListened>();
            if(Listens.Any())
            {
                TimeSpan TimeListened = TimeSpan.FromSeconds(Listens.Sum(L => L.TimeListened.TotalSeconds));
                this.WithField("Total Time Listened", FormatTimeSpan(TimeListened));
            }
            List<LoFiUserSongListened> Concat = new();
            foreach(LoFiUserSongListened Listened in Listens)
            {
                LoFiUserSongListened? AlreadyExisting = Concat.FirstOrDefault(L => L.SongId == Listened.SongId);
                if (AlreadyExisting != null)
                {
                    AlreadyExisting.IncrementTimeListened(Listened.TimeListened);
                }
                else
                {
                    Concat.Add(Listened);
                }
            }
            if(Concat.Any())
            {
                IEnumerable<LoFiUserSongListened> MostListened = Concat.OrderByDescending(L => L.TimeListened).Take(take);
                this.WithListWrittenToField($"Top `{take}` Songs", MostListened.Select(ML =>
                (izolabella.LoFi.Server.Structures.Constants.DataStores.SongStore.ReadAsync<IzolabellaSong>(ML.SongId).Result?.Name ?? "unknown song") + 
                $" - {FormatTimeSpan(ML.TimeListened)}"), "\n");
            }
        }
        else
        {
            this.WithField("No Data", $"There is no data present. Try running the `{new VerifyCommand().Name}` command.");
        }
    }
}
