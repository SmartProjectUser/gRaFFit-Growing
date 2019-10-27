using System;
using UnityEngine;

namespace gRaFFit.Agar.Views.Pool {
    /// <summary>
    /// Игровой объект, помещаемый в пулл
    /// </summary>
    public abstract class GameObjectPoolable : MonoBehaviour {
        #region Public Fields

        /// <summary>
        /// Ивент освобождения объекта
        /// </summary>
        public event Action<GameObjectPoolable> OnObjectDisposed;

        #endregion

        #region Public Methods

        /// <summary>
        /// Инстанциирование объекта
        /// </summary>
        public virtual void Instantiate() {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Освобождение объекта и возвращение в пул
        /// </summary>
        public virtual void Dispose() {
            gameObject.SetActive(false);

            OnObjectDisposed?.Invoke(this);
            OnObjectDisposed = null;
        }

        #endregion
    }
}