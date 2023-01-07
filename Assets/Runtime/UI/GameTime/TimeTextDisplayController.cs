using Lunaculture.GameTime;
using TMPro;
using UnityEngine;

namespace Lunaculture.UI.GameTime
{
    public class TimeTextDisplayController : MonoBehaviour
    {
        [SerializeField] private GameUIInterconnect gameUIInterconnect;
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private TextMeshProUGUI weekText;

        private TimeController timeController;

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
