using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using izolabella.LoFi.Server.Structures.Endpoints;
using izolabella.LoFi.Server.Structures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Others
{
    public class VerifyEmbed : KaiaPathEmbedRefreshable
    {
        public VerifyEmbed(KaiaUser UserVerifying, LoFiUser? User) : base(Strings.EmbedStrings.FakePaths.Global)
        {
            this.UserVerifying = UserVerifying;
            this.User = User;
            this.Req = new(this.UserVerifying.Id, TimeSpan.FromMinutes(30));
        }

        public KaiaUser UserVerifying { get; }

        public LoFiUser? User { get; }

        public bool IsComplete => this.User != null;

        public Uri VLink => new($"https://izolabella.dev:21621/{new Verify().Route}/{Req.Secret}");

        public LoFiUserVerificationRequest Req { get; }

        protected override async Task ClientRefreshAsync()
        {
            if(!IsComplete)
            {
                await izolabella.LoFi.Server.Structures.Constants.DataStores.VerificationRequestsStore.SaveAsync(Req);
                this.WithField("verify", $"[click here]({VLink}) to verify", true);
                this.WithField("time limit", $"expires at <t:{(int)Req.ExpiresAt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds}:T>", true);
            }
            else
            {
                LoFiUser? Complete = await izolabella.LoFi.Server.Structures.Constants.DataStores.UserStore.ReadAsync<LoFiUser>(this.UserVerifying.Id);
                if(Complete != null)
                {
                    this.WithField("verified", "verification was successful");
                    this.WithField("secret - _only u can see this embed_", $"||{Complete.Credentials.Secret}||");
                    this.WithField("what now?", "copy and paste the secret into the lofi clients to enable tracking of ur statistics, such as the amount of time u spend listening to the radio");
                }
                else
                {
                    this.WithField("?", "something went wrong");
                }
            }
        }
    }
}
