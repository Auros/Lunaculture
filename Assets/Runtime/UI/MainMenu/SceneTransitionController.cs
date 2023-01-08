using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lunaculture
{
    public class SceneTransitionController : MonoBehaviour
    {
        public void GoToMenu() => TransitionSceneAsync(0).Forget();

        public void GoToGame() => TransitionSceneAsync(1).Forget();

        private async UniTask TransitionSceneAsync(int buildIndex)
        {
            await SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
        }
    }
}
