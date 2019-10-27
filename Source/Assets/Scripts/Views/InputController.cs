using gRaFFit.Agar.Utils;
using gRaFFit.Agar.Utils.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace gRaFFit.Agar.Controllers.InputSystem {
    /// <summary>
    /// Абстрактный контроллер ввода
    /// </summary>
    public abstract class InputController : MonoBehaviour {
        #region Singleton

        protected InputController() {
        }

        private static InputController _instance;

        public static InputController Instance {
            get {
                if (_instance == null) {
                    var gameObject = new GameObject("InputController");

#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
                    _instance = gameObject.AddComponent<MouseInputController>();
#elif (UNITY_ANDROID || UNITY_IOS)
                    _instance = gameObject.AddComponent<TouchInputController>();
#endif
                }

                return _instance;
            }
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// Позиция, в которой начал свайпать игрок
        /// </summary>
        private Vector2 _startSwipePosition;

        /// <summary>
        /// Необходимо для того, что бы обеспечить один свайп на одно нажатие, не более
        /// </summary>
        private bool _alreadySwipedOnce;

        /// <summary>
        /// Активен-ли сейчас контроллер?
        /// </summary>
        private bool _isActive;

        #endregion

        #region Signals

        public Signal<Vector2> SignalOnTouchStart = new Signal<Vector2>();

        public Signal<Vector2> SignalOnTouch = new Signal<Vector2>();

        public Signal<Vector2> SignalOnTouchEnd = new Signal<Vector2>();

        #endregion

        /// <summary>
        /// Вызывается джаглером каждый фрейм
        /// </summary>
        public void Update() {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                CheckInput();
            }
        }

        #region Protected Methods

        /// <summary>
        /// Проверка ввода
        /// </summary>
        protected abstract void CheckInput();
        public abstract bool IsTouchDown();
        public abstract bool IsTouch();
        public abstract bool IsTouchUp();
        
        #endregion

        #region Public Methods
        
        public abstract Vector2 GetTouchPosition();

        public Vector2 GetTouchWorldPosition() {
            return CachedMainCamera.Instance.Camera.ScreenToWorldPoint(GetTouchPosition());
        }
        
        #endregion

        #region Private Methods

        protected void HandleTouchStart(Vector3 inputPosition) {
            SignalOnTouchStart.Dispatch(inputPosition);
        }

        protected void HandleTouch(Vector3 inputPosition) {
            SignalOnTouch.Dispatch(inputPosition);
        }

        protected void HandleTouchEnd(Vector2 inputPosition) {
            SignalOnTouchEnd.Dispatch(inputPosition);
        }

        #endregion
    }
}