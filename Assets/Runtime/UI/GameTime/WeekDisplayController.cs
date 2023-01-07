using Lunaculture.GameTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunaculture.UI.GameTime
{
    public class WeekDisplayController : MonoBehaviour
    {
        [SerializeField] private GameUIInterconnect interconnect = null!;
        [SerializeField] private SimpleFillController simpleFillPrefab = null!;

        private TimeController timeController = null!;

        private SimpleFillController[] dayUIElements = null!;

        private void Start()
        {
            timeController = interconnect.TimeController;
        
            dayUIElements = new SimpleFillController[timeController.DaysPerWeek];

            for (var i = 0; i < timeController.DaysPerWeek; i++)
            {
                dayUIElements[i] = Instantiate(simpleFillPrefab, transform);
            }

            timeController.OnDayChange += TimeController_OnDayChange;
            timeController.OnWeekChange += TimeController_OnWeekChange;
        }

        private void Update()
        {
            var dayOfWeek = timeController.CurrentDay % timeController.DaysPerWeek;

            dayUIElements[dayOfWeek].Fill = timeController.DayProgress;
        }

        private void TimeController_OnDayChange(DayChangeEvent obj)
        {
            var previousDay = (obj.Day - 1) % timeController.DaysPerWeek;

            if (previousDay >= 0)
            {
                dayUIElements[previousDay].Fill = 1f;
            }
        }

        private void TimeController_OnWeekChange(WeekChangeEvent obj)
        {
            for (var i = 0; i < timeController.DaysPerWeek; i++)
            {
                dayUIElements[i].Fill = 0f;
            }
        }

        private void OnDestroy()
        {
            timeController.OnDayChange -= TimeController_OnDayChange;
            timeController.OnWeekChange -= TimeController_OnWeekChange;
        }
    }
}
