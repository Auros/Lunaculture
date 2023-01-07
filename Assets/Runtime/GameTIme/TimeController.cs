using System;
using UnityEngine;

namespace Lunaculture.GameTime
{
    public class TimeController : MonoBehaviour
    {
        public event Action<DayChangeEvent> OnDayChange;

        public event Action<WeekChangeEvent> OnWeekChange;

        public float GameSpeed
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }

        public int CurrentDay { get; private set; } = 0;
        public int CurrentWeek { get; private set; } = 0;

        public float DayProgress => gameTime % 1;

        public float CurrentDayExact
        {
            get => gameTime;
            set
            {
                gameTime = value;
                CurrentDay = (int)value;
                CurrentWeek = (int)(value / daysPerWeek);
            }
        }

        public float CurrentWeekExact
        {
            get => gameTime / daysPerWeek;
            set => CurrentDayExact = value * daysPerWeek;
        }

        [Header("Day duration is in minutes")]
        [SerializeField]
        private float dayDuration = 2f;

        [SerializeField]
        private int daysPerWeek = 5;

        private float gameTime = 0;

        private void Update()
        {
            gameTime += Time.deltaTime * Time.timeScale / 60;

            var day = (int)(gameTime / dayDuration);
            if (CurrentDay != day)
            {
                CurrentDay = day;
                OnDayChange?.Invoke(new(CurrentDay));
            }

            var week = CurrentDay / daysPerWeek;
            if (CurrentWeek != week)
            {
                CurrentWeek = week;
                OnWeekChange?.Invoke(new(CurrentWeek));
            }
        }
    }
}
