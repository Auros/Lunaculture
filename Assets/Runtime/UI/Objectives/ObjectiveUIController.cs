using ElRaccoone.Tweens.Core;
using ElRaccoone.Tweens;
using Lunaculture.Objectives;
using Lunaculture.UI.GameTime;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Lunaculture.UI.Objectives
{
    public class ObjectiveUIController : MonoBehaviour
    {
        [SerializeField] private ToastNotificationController toastNotificationController = null!;
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;
        [SerializeField] private SimpleFillController progressFill = null!;
        [SerializeField] private TextMeshProUGUI objectiveText = null!;
        [SerializeField] private Color noCompletionColor = Color.red;
        [SerializeField] private Color closeToCompletionColor = Color.yellow;
        [SerializeField] private Color completionColor = Color.green;

        private ObjectiveService objectiveService = null!;
        private Tween<float> active = null!;

        private void Start()
        {
            objectiveService = gameUIInterconnect.ObjectiveService;

            objectiveService.OnObjectiveProgress += ObjectiveService_OnObjectiveProgress;
            objectiveService.OnObjectiveComplete += ObjectiveService_OnObjectiveComplete;
            objectiveService.OnObjectiveFail += ObjectiveService_OnObjectiveFail;

            progressFill.Fill = 0;
            objectiveText.text = $"Sell {objectiveService.SellTarget}cr";
        }

        private void ObjectiveService_OnObjectiveComplete()
        {
            // TODO: Tween
            progressFill.Fill = 0;
            objectiveText.text = $"Sell {objectiveService.SellTarget}cr";

            toastNotificationController.SummonToast("Weekly Objective complete.");
        }
        private void ObjectiveService_OnObjectiveFail()
        {
            toastNotificationController.SummonToast("Weekly Objective failed.", ToastNotificationController.ToastType.Fail);
        }

        private void ObjectiveService_OnObjectiveProgress(float obj)
        {
            active.AsNull()?.Cancel();
            active = gameObject
                .TweenValueFloat(obj, 0.5f, val => progressFill.Fill = val)
                .SetFrom(progressFill.Fill)
                .SetOnComplete(() => active = null)
                .SetUseUnscaledTime(true)
                .SetEase(EaseType.QuartOut);

            progressFill.Color = obj == 1
                ? completionColor
                : Color.Lerp(noCompletionColor, closeToCompletionColor, obj);
        }

        private void OnDestroy()
        {
            objectiveService.OnObjectiveProgress -= ObjectiveService_OnObjectiveProgress;
            objectiveService.OnObjectiveComplete -= ObjectiveService_OnObjectiveComplete;
            objectiveService.OnObjectiveFail -= ObjectiveService_OnObjectiveFail;
        }
    }
}
