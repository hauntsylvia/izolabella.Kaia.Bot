﻿namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Self
{
    public class InventoryItemViewRaw : KaiaPathEmbedRefreshable
    {
        public InventoryItemViewRaw(KaiaInventoryItem Item) : base(Strings.EmbedStrings.FakePaths.Inventory, Item.DisplayName)
        {
            this.Item = Item;
        }

        public KaiaInventoryItem Item { get; }

        protected override Task ClientRefreshAsync()
        {
            this.WithField(this.Item.DisplayString, this.Item.Description);
            if (this.Item.ReceivedAt.HasValue)
            {
                this.WithField("received at", $"`{this.Item.ReceivedAt.Value.ToShortDateString()}`");
            }
            this.WithField("unique identifier", $"`{this.Item.Id.ToString(CultureInfo.InvariantCulture)}`");
            return Task.CompletedTask;
        }
    }
}