using Lunaculture.Objectives;
using Lunaculture.UI.GameTime;
using TMPro;
using UnityEngine;

namespace Lunaculture.UI.Objectives
{
    public class ObjectiveUIController : MonoBehaviour
    {
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;
        [SerializeField] private SimpleFillController progressFill = null!;
        [SerializeField] private TextMeshProUGUI objectiveText = null!;

        private ObjectiveService objectiveService;

        private void Start()
        {
            objectiveService = gameUIInterconnect.ObjectiveService;

            objectiveService.OnObjectiveProgress += ObjectiveService_OnObjectiveProgress;
            objectiveService.OnObjectiveComplete += ObjectiveService_OnObjectiveComplete;

            ObjectiveService_OnObjectiveComplete();
        }

        private void ObjectiveService_OnObjectiveComplete()
        {
            // TODO: Tween
            progressFill.Fill = 0;
            objectiveText.text = $"Sell {objectiveService.SellTarget}cr";
        }

        private void ObjectiveService_OnObjectiveProgress(float obj)
        {
            // TODO: Tween
            progressFill.Fill = obj;
        }

        private void OnDestroy()
        {
            objectiveService.OnObjectiveProgress -= ObjectiveService_OnObjectiveProgress;
            objectiveService.OnObjectiveComplete -= ObjectiveService_OnObjectiveComplete;
        }
    }
}
