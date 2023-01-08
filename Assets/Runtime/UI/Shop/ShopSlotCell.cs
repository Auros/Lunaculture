using Lunaculture.UI.Inventory;

namespace Lunaculture.UI.Shop
{
    public class ShopSlotCell : SlotCell
    {
        protected override string LabelText => $"{AssignedStack?.ItemType.AsNull()?.BuyPrice ?? -1}cr";
    }
}
