using UnityEngine;

namespace gRaFFit.Agar.Utils {
    /// <summary>
    /// Кешированная главная камера (т.к. постоянное обращение к Camera.main не оптимально)
    /// </summary>
    public class CachedMainCamera {
        #region Singleton

        private CachedMainCamera() {
        }

        private static CachedMainCamera _instance;

        public static CachedMainCamera Instance {
            get { return _instance ?? (_instance = new CachedMainCamera()); }
        }

        #endregion

        #region Public Fields

        /// <summary>
        /// Кешированная камера из Camera.main
        /// </summary>
        public Camera Camera {
            get {
                if (_cachedMainCamera == null) {
                    _cachedMainCamera = Camera.main;
                }

                return _cachedMainCamera;
            }
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// Объект для кеширования камеры. Не использовать! Вместо этого пользоваться <see cref="Camera"/>
        /// </summary>
        private Camera _cachedMainCamera;

        #endregion
    }
}