using System.Collections.Generic;
using UnityEngine;

namespace gRaFFit.Agar.Views.Pool {
    /// <summary>
    /// Служба пуллов
    /// </summary>
    public partial class PoolService {
        /// <summary>
        /// Пул объектов. Является private частью PoolService, что бы запретить использовать GameObjectsPool напрямую
        /// </summary>
        private class GameObjectsPool {

            #region Public Fields

            /// <summary>
            /// Объект, из которого будут инстанциироваться новые копии в пуле
            /// </summary>
            public GameObjectPoolable PrefabToInstantiate { get; private set; }

            #endregion

            #region Private Fields

            /// <summary>
            /// Стек свободных, неиспользуемых объектов в пуле
            /// </summary>
            private Stack<GameObjectPoolable> _currentFreeInstances = new Stack<GameObjectPoolable>();

            /// <summary>
            /// Контейнер, в который будут помещены все инстансы объектов
            /// </summary>
            private Transform _containerForInstances;

            /// <summary>
            /// Список проинстанциированных партиклов
            /// </summary>
            private List<GameObjectPoolable> _instancesList = new List<GameObjectPoolable>();

            #endregion

            #region Public Methods

            /// <summary>
            /// Инициализация пула префабом
            /// </summary>
            /// <param name="prefabToInstantiate">Префаб, экземляры которого будут помещаться в пул</param>
            /// <param name="containerForInstances">Контейнер, в котором будут размещены все новые объекты</param>
            /// <param name="instances">Количество инстансов, которые надо создать заранее</param>
            public void InitPool(GameObjectPoolable prefabToInstantiate, Transform containerForInstances,
                int instances = 0) {
                PrefabToInstantiate = prefabToInstantiate;
                _containerForInstances = containerForInstances;

                if (instances > 0) {
                    for (int i = 0; i < instances; i++) {
                        var instance = Object.Instantiate(PrefabToInstantiate, _containerForInstances);
                        instance.Dispose();
                        _currentFreeInstances.Push(instance);
                    }
                }
            }

            /// <summary>
            /// Возвращает объект из пула, либо создает новый, если все такие уже заняты
            /// </summary>
            /// <param name="container">Контейнер, в который необходимо проинстанциировать объект</param>
            public GameObjectPoolable PopObject(Transform container = null) {
                if (PrefabToInstantiate == null) {
                    Debug.LogError("GameObjectsPool.PopObject: Pool is not instantiated!!! Can't instantiate objects!");
                    return null;
                }

                GameObjectPoolable instance;

                if (_currentFreeInstances.Count > 0) {
                    instance = _currentFreeInstances.Pop();
                    if (container != null) {
                        instance.transform.SetParent(container);
                    }
                } else {
                    instance = Object.Instantiate(PrefabToInstantiate,
                        container == null ? _containerForInstances : container);
                }

                if (instance != null) {
                    instance.transform.SetAsLastSibling();
                    instance.Instantiate();
                    instance.OnObjectDisposed += OnObjectDisposed;
                    _instancesList.Add(instance);
                } else {
                    Debug.LogError(
                        $"GameObjectsPool.PopObject: Something went wrong: can't get instance! {PrefabToInstantiate.name}");
                }

                return instance;
            }

            /// <summary>
            /// Уничтожает свободные инстансы
            /// </summary>
            public void DestroyFreeInstances() {
                foreach (var instance in _currentFreeInstances) {
                    Object.Destroy(instance.gameObject);
                }

                _currentFreeInstances.Clear();
                _containerForInstances = null;
            }

            /// <summary>
            /// Освобождает все текущие инстансы
            /// </summary>
            public void DisposeInstances() {
                while (_instancesList.Count > 0) {
                    _instancesList[0].Dispose();
                }
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// Вызывается по ивенту освобождения пуллэбл объекта
            /// </summary>
            /// <param name="disposedObject">Объект, который освободился, и ждет возвращения в пул</param>
            private void OnObjectDisposed(GameObjectPoolable disposedObject) {
                _currentFreeInstances.Push(disposedObject);
                _instancesList.Remove(disposedObject);
            }

            #endregion
        }
    }
}