using izolabella.Kaia.Bot.Objects.Util;
using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles
{
    public class KaiaReactionRole(ulong ListerId, ulong MessageId, ulong ChannelId, ulong RoleId, KaiaEmote Emote, bool Enforce, ulong? Id = null)
    {

        /// <summary>
        /// The id of the user that created this reaction role.
        /// </summary>
        public ulong ListerId { get; } = ListerId;

        /// <summary>
        /// The message id to listen for reactions with.
        /// </summary>
        public ulong MessageId { get; } = MessageId;

        /// <summary>
        /// The channel the message resides.
        /// </summary>
        public ulong ChannelId { get; } = ChannelId;

        /// <summary>
        /// The id of the role to give or remove.
        /// </summary>
        public ulong RoleId { get; } = RoleId;

        /// <summary>
        /// The emote required in the reaction to give this role.
        /// </summary>
        public KaiaEmote Emote { get; } = Emote;

        /// <summary>
        /// If true, this indicates that Kaia should check whether the user has reacted or not on startup and 
        /// grant the role (or remove it) accordingly, instead of only relying on listeners.
        /// </summary>
        public bool Enforce { get; } = Enforce;

        /// <summary>
        /// The unique id of this instance.
        /// </summary>
        public ulong Id { get; } = Id ?? IdGenerator.CreateNewId();

        public async Task<IMessage?> GetMessageAsync(SocketGuild Guild)
        {
            SocketGuildChannel? C = Guild.GetChannel(this.ChannelId);
            if (C is not null and SocketTextChannel CS)
            {
                IMessage? M = await CS.GetMessageAsync(this.MessageId);
                return M;
            }
            return null;
        }

        public Task<IRole?> GetRoleAsync(SocketGuild Guild)
        {
            return Task.FromResult<IRole?>(Guild.GetRole(this.RoleId));
        }
    }
}