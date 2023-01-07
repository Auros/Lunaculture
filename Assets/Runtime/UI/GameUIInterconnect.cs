using Lunaculture.GameTime;
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
        public TimeController TimeController { get; private set; }
    }
}
