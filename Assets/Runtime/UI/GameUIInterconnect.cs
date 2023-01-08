using Lunaculture.GameTime;
using Lunaculture.Objectives;
using Lunaculture.Player.Currency;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.UI
{
    /*
     * OK OK. Let me explain myself:
     * 
     * Instead of having to search through and fulfill *every* MonoBehaviour dependency in the game UI,
     * I am using this interconnect so that all game-specific references can be filled in one component.
     * 
     * Don't like it? We should've used Dependency Injection!
     */
    public class GameUIInterconnect : MonoBehaviour
    {
        [field: SerializeField]
        public TimeController TimeController { get; private set; } = null!;

        [field: SerializeField]
        public InventoryService InventoryService { get; private set; } = null!;

        [field: SerializeField]
        public PauseController PauseController { get; private set; } = null!;

        [field: SerializeField]
        public CurrencyService CurrencyService { get; private set; } = null!;

        [field: SerializeField]
        public ObjectiveService ObjectiveService{ get; private set; } = null!;
    }
}
