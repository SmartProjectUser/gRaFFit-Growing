using System.Collections.Generic;
using UnityEngine;

namespace gRaFFit.Agar.Views.Pool {
    /// <summary>
    /// Служба пуллов
    /// </summary>
    public partial class PoolService {

        #region Singleton

        private PoolService() {
        }

        private static PoolService _instance;

        public static PoolService Instance {
            get { return _instance ?? (_instance = new PoolService()); }
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// Текущие пуллы
        /// </summary>
        private Dictionary<string, GameObjectsPool> _currentPools = new Dictionary<string, GameObjectsPool>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Инициализация нового пулла с объектами
        /// </summary>
        /// <param name="poolKey">Ключ пуллу для этих объектов</param>
        /// <param name="prefab">Экземпляр объекта, с которого будут инстанциироваться копии</param>
        /// <param name="containerForInstances">Контейнер для инстансов</param>
        /// <param name="instances">Сколько инстансов создать предварительно</param>
        public void InitPoolWithNewObject(string poolKey, GameObjectPoolable prefab,
            Transform containerForInstances = null,
            int instances = 0) {

            if (prefab == null) {
                Debug.LogError(
                    $"PoolService.InitPoolWithNewObject: Error! You just tried to init {poolKey} pool with null object! Please check is it prefab exists on panel with container");
            } else {
                // если вдруг пулл с таким ключем уже есть
                if (_currentPools.ContainsKey(poolKey)) {
                    // и если префаб не совпадает - выводим ошибку, что пулл уже занят
                    if (prefab != _currentPools[poolKey].PrefabToInstantiate) {
                        Debug.LogError(
                            $"PoolService.InitPoolWithNewObject: Can't instantiate pool: pool with \"{poolKey}\" key is already instantiated with other object!");
                    }

                    // если же префаб тот же - не трогаем пулл, оставляем все как есть, ведь пул уже был проинициализирован ранее
                } else {
                    // если такого пула нет = создаем его
                    var newPool = new GameObjectsPool();
                    newPool.InitPool(prefab, containerForInstances, instances);
                    _currentPools.Add(poolKey, newPool);
                }
            }
        }

        /// <summary>
        /// Создает или берет из пулла объект по его типу
        /// </summary>
        /// <param name="poolKey">Ключ к пуллу</param>
        /// <param name="container">Если указать - объект будет проинстанциирован именно в этот контейнер</param>
        /// <returns>Новый объект</returns>
        public GameObjectPoolable PopObject(string poolKey, Transform container = null) {
            return GetPool(poolKey).PopObject(container);
        }

        /// <summary>
        /// Полностью очищает конкретный пул
        /// </summary>
        /// <param name="poolKey">Ключ пула</param>
        /// <param name="removePoolKey">Удалять ли ключ пула из сервиса полностью</param>
        public void ReleasePool(string poolKey, bool removePoolKey = true) {
            if (!string.IsNullOrEmpty(poolKey)) {
                var pool = GetPool(poolKey);
                if (pool != null) {
                    pool.DisposeInstances();
                    pool.DestroyFreeInstances();
                }

                if (removePoolKey) {
                    _currentPools.Remove(poolKey);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Получение пулла по типу. Вернет null, если такого пулла нет
        /// </summary>
        /// <param name="poolKey">Ключ к пуллу</param>
        /// <returns>Пулл, который интанциирует этот тип, или null, если получить пулл не удалось</returns>
        private GameObjectsPool GetPool(string poolKey) {
            GameObjectsPool pool = null;

            if (_currentPools.ContainsKey(poolKey)) {
                pool = _currentPools[poolKey];
            }

            return pool;
        }

        #endregion
    }
}