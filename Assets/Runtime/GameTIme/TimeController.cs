using System;
using UnityEngine;

namespace Lunaculture.GameTime
{
    public class TimeController : MonoBehaviour
    {
        public event Action<DayChangeEvent>? OnDayChange;

        public event Action<WeekChangeEvent>? OnWeekChange;

        public float GameSpeed
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }

        public int CurrentDay { get; private set; } = 0;
        public int CurrentWeek { get; private set; } = 0;

        public float DayProgress => gameTime / DayDuration % 1;

        public float CurrentDayExact
        {
            get => gameTime;
            set
            {
                gameTime = value;
                CurrentDay = (int)value;
                CurrentWeek = (int)(value / DaysPerWeek);
            }
        }

        public float CurrentWeekExact
        {
            get => gameTime / DaysPerWeek;
            set => CurrentDayExact = value * DaysPerWeek;
        }

        [field: Header("Day duration is in minutes")]
        [field: SerializeField]
        public float DayDuration { get; private set; } = 2f;

        [field: SerializeField]
        public int DaysPerWeek { get; private set; } = 5;

        private float gameTime = 0;

        private void Update()
        {
            gameTime += Time.deltaTime * Time.timeScale / 60;

            var day = (int)(gameTime / DayDuration);
            if (CurrentDay != day)
            {
                CurrentDay = day;
                OnDayChange?.Invoke(new(CurrentDay));
            }

            var week = CurrentDay / DaysPerWeek;
            if (CurrentWeek != week)
            {
                CurrentWeek = week;
                OnWeekChange?.Invoke(new(CurrentWeek));
            }
        }
    }
}
