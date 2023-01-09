using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.Machines.Miner
{
    public class MinerCollectionController : InteractionController
    {
        [SerializeField]
        private InventoryService _inventoryService = null!;
        
        protected override void OnGridObjectClicked(GridObject gridObject)
        {
            if (gridObject.Type is not GridObjectType.Miner)
                return;

            var minerGridObject = (gridObject as MinerGridObject)!;
            var controller = minerGridObject.Controller;

            var item = controller.GetItem();
            var count = controller.GetItemCount();
            _inventoryService.AddItem(item, count);
            controller.Clear();
        }

        protected override void OnHoveredGridObjectChange(GridObject? gridObject)
        {
            // unused atm
        }
    }
}