using System;
using gRaFFit.Agar.Models.SharedHelpers;
using gRaFFit.Agar.Utils.Signals;

namespace gRaFFit.Agar.Models.Timers {
    /// <summary>
    /// Таймер
    /// </summary>
    public class Timer {

        #region Public Fields

        /// <summary>
        /// Сигнал о финише таймера
        /// </summary>
        public Signal<Timer> SignalOnFinished;

        #endregion

        #region Private Fields

        /// <summary>
        /// Время, которое осталось до срабатывания таймера
        /// </summary>
        private float _timeLeft;

        /// <summary>
        /// Время, через которое произойдет "тик" таймера
        /// </summary>
        private float _tickTime;

        /// <summary>
        /// Промежуток, через который повторяются "тики" таймера, пока он работает
        /// </summary>
        private float _totalTickTime;

        /// <summary>
        /// Коллбек на финиширование таймера
        /// </summary>
        private Action _onFinishedAction;

        /// <summary>
        /// Действие по тику таймера, float - количество времени, которое осталось до срабатывания таймера
        /// </summary>
        private Action<float> _onTickAction;

        #endregion

        #region Public Methods

        /// <summary>
        /// Конструктор таймера
        /// </summary>
        /// <param name="timeOut">Время, через которое финиширует таймер</param>
        /// <param name="onFinishedAction">Действие, которое вызовется по финишу таймера</param>
        /// <param name="tickTime">Промежуток, через который повторяются "тики" таймера, пока он работает</param>
        /// <param name="onTick">Действие по тику таймера, float - количество времени, которое осталось до срабатывания таймера</param>
        public Timer(float timeOut, Action onFinishedAction, float tickTime = Constants.MAGIC_UNDEFINED_NUMBER,
            Action<float> onTick = null) {
            SignalOnFinished = new Signal<Timer>();
            _timeLeft = timeOut;
            _onFinishedAction = onFinishedAction;
            _tickTime = _totalTickTime = tickTime;
            _onTickAction = onTick;

            // первый тик делаем сразу, для того что бы обновить привязанные к таймеру вьюшки 
            if (_onTickAction != null) {
                _onTickAction(_timeLeft);
            }
        }

        /// <summary>
        /// Вызывается менеджером времени с указанием туда Time.deltaTime, отсчитывает пройденное время у таймера, и
        /// вызывает нужные коллбэки, если для них наступило время
        /// </summary>
        /// <param name="deltaTime">Время, которое прошло после предыдущего Update</param>
        public void Update(float deltaTime) {
            _timeLeft -= deltaTime;
            if (_onTickAction != null) {
                _tickTime -= deltaTime;
            }

            if (_tickTime <= 0) {
                if (_onTickAction != null) {
                    _onTickAction(_timeLeft);
                }

                _tickTime = _totalTickTime;
            }

            if (_timeLeft <= 0) {
                _timeLeft = 0;
                _onFinishedAction();

                if (SignalOnFinished != null) {
                    SignalOnFinished.Dispatch(this);
                }
            }
        }

        /// <summary>
        /// Останавливает таймер, не вызывая коллбэк финиша
        /// </summary>
        public void Stop() {
            if (SignalOnFinished != null) {
                SignalOnFinished.Dispatch(this);
            }
        }

        /// <summary>
        /// Очищает таймер (не освобождать таймер напрямую, этим обязан заниматься <see cref="TimerManager"/>, после вызова метода <see cref="Stop"/>)
        /// </summary>
        public void Dispose() {
            if (SignalOnFinished != null) {
                SignalOnFinished.RemoveAllListeners();
                SignalOnFinished = null;
            }

            _onFinishedAction = null;
            _onTickAction = null;
        }

        #endregion
    }
}