using UnityEngine;

namespace gRaFFit.Agar.Controllers.InputSystem {
    /// <summary>
    /// Контроллер ввода для тачскрина
    /// </summary>
    public class TouchInputController : InputController {
        #region Overrides

        /// <summary>
        /// Проверка ввода
        /// </summary>
        protected override void CheckInput() {
            if (Input.touchCount > 0) {
                var exitLoop = false;
                var touches = Input.touches;
                var touchesCount = touches.Length;

                for (int i = 0; i < touchesCount; i++) {
                    var currTouch = touches[i];

                    switch (currTouch.phase) {
                        case TouchPhase.Began:
                            HandleTouchStart(currTouch.position);
                            exitLoop = true;
                            break;
                        case TouchPhase.Moved:
                        case TouchPhase.Stationary:
                            HandleTouch(currTouch.position);
                            exitLoop = true;
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            HandleTouchEnd(currTouch.position);
                            exitLoop = true;
                            break;
                    }

                    if (exitLoop)
                        return;
                }
            }
        }

        public override Vector2 GetTouchPosition() {
            return Input.touches[0].position;
        }
        
        public override bool IsTouchDown() {
            var touches = Input.touches;
            return touches.Length > 0 &&
                   touches[0].phase == TouchPhase.Began;
        }

        public override bool IsTouch() {
            var touches = Input.touches;
            return touches.Length > 0 &&
                   (
                       touches[0].phase == TouchPhase.Moved ||
                       touches[0].phase == TouchPhase.Stationary
                   );
        }

        public override bool IsTouchUp() {
            var touches = Input.touches;
            return touches.Length > 0 &&
                   (
                       touches[0].phase == TouchPhase.Ended ||
                       touches[0].phase == TouchPhase.Canceled
                   );
        }

        #endregion
    }
}