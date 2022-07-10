using izolabella.Util;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles
{
    public class KaiaAutoRole
    {
        public KaiaAutoRole(ulong ListerId, ulong RoleId, bool Enforce, ulong? Id = null)
        {
            this.ListerId = ListerId;
            this.RoleId = RoleId;
            this.Enforce = Enforce;
            this.Id = Id ?? IdGenerator.CreateNewId();
        }

        /// <summary>
        /// The id of the user that created this reaction role.
        /// </summary>
        public ulong ListerId { get; }

        /// <summary>
        /// The id of the role to give or remove.
        /// </summary>
        public ulong RoleId { get; }

        /// <summary>
        /// If true, this indicates that Kaia should check whether the user has reacted or not on startup and 
        /// grant the role (or remove it) accordingly, instead of only relying on listeners.
        /// </summary>
        public bool Enforce { get; }

        /// <summary>
        /// The unique id of this instance.
        /// </summary>
        public ulong Id { get; }

        public Task<IRole?> GetRoleAsync(SocketGuild Guild)
        {
            return Task.FromResult<IRole?>(Guild.GetRole(this.RoleId));
        }
    }
}
