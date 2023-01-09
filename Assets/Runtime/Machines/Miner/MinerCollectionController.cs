using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
using Lunaculture.Player.Inventory;
using Lunaculture.UI.Tooltips;
using UnityEngine;

namespace Lunaculture.Machines.Miner
{
    public class MinerCollectionController : InteractionController
    {
        [SerializeField]
        private InventoryService _inventoryService = null!;

        [SerializeField]
        private TooltipController _tooltipController = null!;

        private bool _currentlyShowingTooltip;
        
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
            
            if (_currentlyShowingTooltip)
                OnHoveredGridObjectChange(gridObject);
        }

        protected override void OnHoveredGridObjectChange(GridObject? gridObject)
        {
            if (gridObject is not MinerGridObject miner)
            {
                _currentlyShowingTooltip = false;
                _tooltipController.CloseTooltip();
                return;
            }
            _currentlyShowingTooltip = true;
            var count = miner.Controller.GetItemCount();
            var plural = count == 1 ? string.Empty : "s";
            _tooltipController.ShowTooltip("Resource Collector", $"Currently has {count} resource{plural}. Click to harvest.");
        }
    }
}