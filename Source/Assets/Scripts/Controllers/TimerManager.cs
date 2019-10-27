using System;
using System.Collections.Generic;
using gRaFFit.Agar.Models.SharedHelpers;
using UnityEngine;

namespace gRaFFit.Agar.Models.Timers {
    /// <summary>
    /// Менеджер таймеров
    /// </summary>
    public class TimerManager : MonoBehaviour {
        #region MonoSingleton

        private TimerManager() {
        }

        private void Awake() {
            if (Instance != null && Instance != this) {
                Debug.LogError($"Multiple instances of {GetType()} on scene found, fix this!!!");
                Destroy(Instance);
            }
        }

        public static TimerManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<TimerManager>();
                }

                return _instance;
            }
        }

        private static TimerManager _instance;

        #endregion

        #region Private Fields

        /// <summary>
        /// Текущие работающие таймеры
        /// </summary>
        private List<Timer> _currentTimers = new List<Timer>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Создает и запускает таймер с заданными параметрами
        /// </summary>
        /// <param name="timeOut">Время, через которое сработает таймер</param>
        /// <param name="onFinished">Коллбэк на финиш таймера</param>
        /// <param name="tickTime">Промежуток, времени, через которое на протяжении работы таймера будут срабатывать тики</param>
        /// <param name="onTick">Коллбэк на тик таймера</param>
        /// <returns>Созданный объект таймера</returns>
        public Timer SetTimeout(float timeOut, Action onFinished, float tickTime = Constants.MAGIC_UNDEFINED_NUMBER,
            Action<float> onTick = null) {
            var newTimer = new Timer(timeOut, onFinished, tickTime, onTick);
            newTimer.SignalOnFinished.AddOnce(OnTimerFinished);
            _currentTimers.Add(newTimer);
            return newTimer;
        }


        /// <summary>
        /// Возвращает форматированную строку времени время из секунд
        /// </summary>
        /// <param name="time">Время в секундах</param>
        /// <returns>Форматированное время в виде строки</returns>
        public static string GetFormattedTimeFromSeconds_M_S(float time) {
            var timeSpan = TimeSpan.FromSeconds(time < 0 ? 0 : time);
            return string.Format("{0:D2}:{1:D2}", (int) timeSpan.TotalMinutes, timeSpan.Seconds);
        }

        #endregion

        #region Juggler

        /// <summary>
        /// Вызывается джаглером каждый фрейм
        /// </summary>
        public void Update() {
            // Таймеры испльзуются в некоторых корутинах, из-за чего количество таймеров может измениться, поэтому
            // нужен всегда актуальный Count
            for (int i = 0; i < _currentTimers.Count; i++) {
                _currentTimers[i].Update(Time.deltaTime);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Вызывается когда какой-либо таймер финиширует, удаляет таймер из списка всех таймеров
        /// </summary>
        /// <param name="timer">Таймер, который финишировал</param>
        private void OnTimerFinished(Timer timer) {
            _currentTimers.Remove(timer);
            timer.Dispose();
        }

        #endregion
    }
}