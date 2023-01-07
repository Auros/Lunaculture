using Lunaculture.GameTime;
using TMPro;
using UnityEngine;

namespace Lunaculture.UI.GameTime
{
    public class TimeTextDisplayController : MonoBehaviour
    {
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;
        [SerializeField] private TextMeshProUGUI dayText = null!;
        [SerializeField] private TextMeshProUGUI weekText = null!;

        private TimeController timeController = null!;

        private void Start()
        {
            timeController = gameUIInterconnect.TimeController;
            timeController.OnDayChange += TimeController_OnDayChange;
            timeController.OnWeekChange += TimeController_OnWeekChange;
        }

        private void TimeController_OnDayChange(DayChangeEvent obj)
        {
            dayText.text = $"Day {obj.Day}";
        }

        private void TimeController_OnWeekChange(WeekChangeEvent obj)
        {
            weekText.text = $"Week {obj.Week}";
        }

        private void OnDestroy()
        {
            timeController.OnDayChange -= TimeController_OnDayChange;
            timeController.OnWeekChange -= TimeController_OnWeekChange;
        }
    }
}
