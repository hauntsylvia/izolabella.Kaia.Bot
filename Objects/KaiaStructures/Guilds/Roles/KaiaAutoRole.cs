using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles
{
    public class KaiaAutoRole(ulong ListerId, ulong RoleId, bool Enforce, ulong? Id = null)
    {

        /// <summary>
        /// The id of the user that created this reaction role.
        /// </summary>
        public ulong ListerId { get; } = ListerId;

        /// <summary>
        /// The id of the role to give or remove.
        /// </summary>
        public ulong RoleId { get; } = RoleId;

        /// <summary>
        /// If true, this indicates that Kaia should check whether the user has reacted or not on startup and 
        /// grant the role (or remove it) accordingly, instead of only relying on listeners.
        /// </summary>
        public bool Enforce { get; } = Enforce;

        /// <summary>
        /// The unique id of this instance.
        /// </summary>
        public ulong Id { get; } = Id ?? IdGenerator.CreateNewId();

        public Task<IRole?> GetRoleAsync(SocketGuild Guild)
        {
            return Task.FromResult<IRole?>(Guild.GetRole(this.RoleId));
        }
    }
}