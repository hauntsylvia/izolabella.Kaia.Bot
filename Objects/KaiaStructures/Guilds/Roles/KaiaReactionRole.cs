using izolabella.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.KaiaStructures.Guilds.Roles
{
    public class KaiaReactionRole
    {
        public KaiaReactionRole(ulong ListerId, ulong MessageId, ulong ChannelId, ulong RoleId, KaiaEmote Emote, bool Enforce, ulong? Id = null)
        {
            this.ListerId = ListerId;
            this.MessageId = MessageId;
            this.ChannelId = ChannelId;
            this.RoleId = RoleId;
            this.Emote = Emote;
            this.Enforce = Enforce;
            this.Id = Id ?? IdGenerator.CreateNewId();
        }

        /// <summary>
        /// The id of the user that created this reaction role.
        /// </summary>
        public ulong ListerId { get; }

        /// <summary>
        /// The message id to listen for reactions with.
        /// </summary>
        public ulong MessageId { get; }

        /// <summary>
        /// The channel the message resides.
        /// </summary>
        public ulong ChannelId { get; }

        /// <summary>
        /// The id of the role to give or remove.
        /// </summary>
        public ulong RoleId { get; }

        /// <summary>
        /// The emote required in the reaction to give this role.
        /// </summary>
        public KaiaEmote Emote { get; }

        /// <summary>
        /// If true, this indicates that Kaia should check whether the user has reacted or not on startup and 
        /// grant the role (or remove it) accordingly, instead of only relying on listeners.
        /// </summary>
        public bool Enforce { get; }

        /// <summary>
        /// The unique id of this instance.
        /// </summary>
        public ulong Id { get; }

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
