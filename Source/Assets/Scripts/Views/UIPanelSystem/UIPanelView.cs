using System.Collections;
using gRaFFit.Agar.Models.SharedHelpers;
using gRaFFit.Agar.Utils;
using gRaFFit.Agar.Utils.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace gRaFFit.Agar.Views.UIPanelSystem {
    
    /// <summary>
    /// Абстрактный класс панели
    /// </summary>
    public abstract class UIPanelView : MonoBehaviour {
        #region Layout

        /// <summary>
        /// Кнопка возвращения в предыдущее меню
        /// </summary>
        [SerializeField] private Button _backButton;

        /// <summary>
        /// Аниматор этого окна
        /// </summary>
        [SerializeField] protected Animator _animator;

        #endregion

        #region Public Fields

        /// <summary>
        /// Видима-ли в данный момент панель?
        /// </summary>
        public bool Visible { get; protected set; }

        /// <summary>
        /// Сигнал о том, что финишировала анимация сокрытия панели
        /// </summary>
        public Signal SignalOnHideAnimationFinished;

        /// <summary>
        /// Сигнал о том, что финишировала анимация сокрытия панели
        /// </summary>
        public Signal<UIPanelView> SignalOnHideAnimationStarted;

        /// <summary>
        /// Сигнал о том, что была нажата кнопка "назад"
        /// </summary>
        public Signal SignalOnBackButtonClicked;

        /// <summary>
        /// Проинициализированы-ли сигналы?
        /// </summary>
        public bool IsSignalsInited { get; private set; }

        /// <summary>
        /// Активна ли кнопка back
        /// </summary>
        public bool IsBackButtonActive;

        #endregion

        #region Public Methods

        /// <summary>
        /// Отображает панель
        /// </summary>
        public virtual void Show() {
            if (!Visible) {
                Visible = true;

                StopAllCoroutines();
                InitLocalization();

                if (_backButton != null) {
                    _backButton.gameObject.SetActive(true);
                    IsBackButtonActive = true;
                }

                gameObject.SetActive(true);

                if (_animator != null) {
                    AnimatorHelper.Instance.PlayAnimation(_animator, Constants.ANIMATION_STATE_PANEL_SHOW);
                }

                InitSignals();
                AddListeners();
            }
        }

        /// <summary>
        /// Возвращает время анимации отображения
        /// </summary>
        public float GetShowAnimationTime() {
            var result = 0f;
            if (_animator != null) {
                result = AnimatorHelper.Instance.PlayAnimation(_animator, Constants.ANIMATION_STATE_PANEL_SHOW);
            }

            return result;
        }

        /// <summary>
        /// Прячет панель
        /// </summary>
        public virtual void Hide() {
            if (Visible) {
                Visible = false;
                if (SignalOnHideAnimationStarted != null) {
                    SignalOnHideAnimationStarted.Dispatch(this);
                }

                if (_animator != null) {
                    var hideTime =
                        AnimatorHelper.Instance.PlayAnimation(_animator, Constants.ANIMATION_STATE_PANEL_HIDE);
                    StartCoroutine(HidePanelAfterTime(hideTime));
                } else {
                    OnHideAnimationFinished();
                }

                RemoveListeners();
            }
        }

        /// <summary>
        /// Иницализация локализации
        /// </summary>
        public virtual void InitLocalization() {

        }

        /// <summary>
        /// Скрывает панель без эффектов
        /// </summary>
        public void HideWithoutEffects() {
            if (Visible) {
                Visible = false;

                if (SignalOnHideAnimationStarted != null) {
                    SignalOnHideAnimationStarted.Dispatch(this);
                }

                RemoveListeners();
                OnHideAnimationFinished();
            }
        }

        /// <summary>
        /// Отклик на нажатие кнопки возврата в предыдущее меню
        /// </summary>
        public virtual void OnBackButtonClicked() {
            if (IsBackButtonActive) {
                SignalOnBackButtonClicked.Dispatch();

                _backButton.onClick.RemoveAllListeners();
                _backButton.gameObject.SetActive(false);

                Hide();
            }
        }


        /// <summary>
        /// Отписка от сигналов
        /// </summary>
        public virtual void RemoveListeners() {
            if (_backButton != null) {
                _backButton.onClick.RemoveAllListeners();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Подписка на сигналы
        /// </summary>
        protected virtual void AddListeners() {
            // для предотвращения навешивания повторных лиснеров при повторном вызове Show у уже отображенной панели
            RemoveListeners();

            if (_backButton != null) {
                _backButton.onClick.AddListener(OnBackButtonClicked);
            }
        }

        /// <summary>
        /// Инициализация сигналов
        /// </summary>
        protected virtual void InitSignals() {
            RemoveListeners();

            if (!IsSignalsInited) {
                SignalOnHideAnimationStarted = new Signal<UIPanelView>();
                SignalOnHideAnimationFinished = new Signal();
                SignalOnBackButtonClicked = new Signal();
                IsSignalsInited = true;
            }
        }

        /// <summary>
        /// Уничтожение сигналов
        /// </summary>
        protected virtual void DestroySignals() {
            RemoveListeners();

            if (IsSignalsInited) {
                SignalOnHideAnimationStarted.RemoveAllListeners();
                SignalOnHideAnimationStarted = null;

                SignalOnHideAnimationFinished.RemoveAllListeners();
                SignalOnHideAnimationFinished = null;

                SignalOnBackButtonClicked.RemoveAllListeners();
                SignalOnBackButtonClicked = null;

                IsSignalsInited = false;
            }
        }

        /// <summary>
        /// Корутина, которая прячет панель после некто
        /// </summary>
        /// <param name="hideTime"></param>
        protected IEnumerator HidePanelAfterTime(float hideTime) {
            yield return new WaitForSeconds(hideTime);
            OnHideAnimationFinished();
        }


        /// <summary>
        /// Вызывается при окончании анимации прятяния окна (когда уже можно чистить объекты)
        /// </summary>
        protected virtual void OnHideAnimationFinished() {
            gameObject.SetActive(false);

            if (IsSignalsInited) {
                SignalOnHideAnimationFinished.Dispatch();
                DestroySignals();
            }
        }

        #endregion
    }
}