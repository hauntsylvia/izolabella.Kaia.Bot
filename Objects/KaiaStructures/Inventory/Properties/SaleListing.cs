using izolabella.Storage.Objects.Structures;
using izolabella.Util;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Exceptions;

namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Properties
{
    public class SaleListing : IDataStoreEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaleListing"/> class.
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Lister">If null, it is assumed the seller is Kaia.</param>
        /// <param name="CostPerItem"></param>
        /// <exception cref="KaiaSaleListingInvalidException">Thrown when the item can not be sold, usually due to
        /// either Kaia not being able to sell the item, or users not being able to sell the item.</exception>
        public SaleListing(List<KaiaInventoryItem> Items, KaiaUser? Lister, double CostPerItem, ulong? Id = null, bool IsListed = false, ulong? ListerId = null)
        {
            this.Items = Items;
            this.Id = Id ?? IdGenerator.CreateNewId(); 
            this.ListerId = ListerId ?? Lister?.Id;
            this.costPerItem = this.Lister != null || this.ListerId != null ? CostPerItem : Items.First().MarketCost;
            this.IsListed = IsListed;
            // implies an actual user is attempting to sell the item when they can't
            if ((this.ListerId == null && Items.Any(I => !I.KaiaDisplaysThisOnTheStore)) || (this.ListerId != null && Items.Any(I => !I.UsersCanSellThis)))
            {
                throw new KaiaSaleListingInvalidException(Lister);
            }
        }

        public List<KaiaInventoryItem> Items { get; private set; }

        public ulong? ListerId { get; }

        [JsonIgnore]
        public KaiaUser? Lister => this.ListerId.HasValue ? new(this.ListerId.Value) : null;

        public ulong Id { get; }

        private double costPerItem;

        public double CostPerItem
        {
            get => Math.Round(this.costPerItem, 2);
            set => this.costPerItem = value;
        }

        public bool IsListed { get; private set; }

        public async Task StartSellingAsync()
        {
            if(!(await DataStores.SaleListingsStore.ReadAllAsync<SaleListing>()).Any(SaleListing => SaleListing.ListerId == this.ListerId))
            {
                this.IsListed = true;
                KaiaUser? Lister = this.Lister;
                List<KaiaInventoryItem> ActualItems = new();
                if (Lister != null && this.ListerId != null)
                {
                    // we put lister as its own variable bc otherwise the getter just creates
                    // a new kaiauser object. this means that each time I type "this.Lister" a new
                    // instance is used.

                    // this can be changed by just changing it to a field, or otherwise only assigning
                    // it a value on construction. but no point.
                    foreach (KaiaInventoryItem Item in this.Items)
                    {
                        if (await Lister.Settings.Inventory.RemoveItemOfIdAsync(Item))
                        {
                            ActualItems.Add(Item);
                        }
                    }
                    this.Items = ActualItems;
                    await Lister.SaveAsync();
                    await DataStores.SaleListingsStore.SaveAsync(this);
                }
            }
        }

        public async Task UserBoughtAsync(KaiaUser UserBuying)
        {
            if(this.Items.Count > 0)
            {
                if(UserBuying.Settings.Inventory.Petals >= this.CostPerItem)
                {
                    KaiaUser? Lister = this.Lister;
                    KaiaInventoryItem Item = this.Items.First();
                    await UserBuying.Settings.Inventory.AddItemsToInventoryAndSaveAsync(UserBuying, Item);
                    Item.ReceivedAt = DateTime.UtcNow;
                    UserBuying.Settings.Inventory.Petals -= this.CostPerItem;

                    if (Lister != null && this.ListerId != null)
                    {
                        Lister.Settings.Inventory.Petals += this.CostPerItem;
                        await Lister.SaveAsync();
                        this.Items.Remove(Item);
                        await DataStores.SaleListingsStore.SaveAsync(this);
                    }
                    else
                    {
                        foreach (KaiaInventoryItem I in this.Items)
                        {
                            I.Id = IdGenerator.CreateNewId();
                            await I.OnKaiaStoreRefresh();
                        }
                    }
                }
            }
            if(this.Items.Count <= 0)
            {
                await DataStores.SaleListingsStore.DeleteAsync(this.Id);
            }
        }

        public async Task StopSellingAsync()
        {
            this.IsListed = false;
            KaiaUser? U = this.Lister;
            if (U != null)
            {
                await U.Settings.Inventory.AddItemsToInventoryAndSaveAsync(U, this.Items.ToArray());
            }
            await DataStores.SaleListingsStore.DeleteAsync(this.Id);
        }
    }
}
