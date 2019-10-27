using System;
using UnityEngine;

namespace gRaFFit.Agar.Utils.Signals {
    /// <summary>
    /// Сигнал без параметров
    /// </summary>
    public class Signal {
        /// <summary>
        /// Делегат для подписок
        /// </summary>
        public event Action Listener = delegate { };

        /// <summary>
        /// Делегат для подписки на одно получение сигнала (после вызова список подписчиков очищается)
        /// </summary>
        public event Action OnceListener = delegate { };

        /// <summary>
        /// Подписка на сигнал
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала</param>
        public void AddListener(Action callback) {
            foreach (var existingCallback in Listener.GetInvocationList()) {
                if (existingCallback == (MulticastDelegate) callback) {
                    SignalsErrorHandler.LogErrorDuplicationListenersError(callback.Method.Name);
                }
            }

            Listener += callback;
        }

        /// <summary>
        /// Подписка на сигнал (с момента подписки сработает только один раз)
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала</param>
        public void AddOnce(Action callback) {
            OnceListener += callback;
        }

        /// <summary>
        /// Отписка от сигнала
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала, который был подписан</param>
        public void RemoveListener(Action callback) {
            Listener -= callback;
            OnceListener -= callback;
        }

        /// <summary>
        /// Отпрака сигнала всем подписчикам
        /// </summary>
        public void Dispatch() {
            Listener();
            OnceListener();
            OnceListener = delegate { };
        }

        /// <summary>
        /// Удаление всех лиснеров
        /// </summary>
        public void RemoveAllListeners() {
            Listener = delegate { };
        }
    }

    /// <summary>
    /// Сигнал с одним параметром
    /// </summary>
    public class Signal<T> {
        /// <summary>
        /// Делегат для подписок
        /// </summary>
        public event Action<T> Listener = delegate { };

        /// <summary>
        /// Делегат для подписки на одно получение сигнала (после вызова список подписчиков очищается)
        /// </summary>
        public event Action<T> OnceListener = delegate { };


        /// <summary>
        /// Подписка на сигнал
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала</param>
        public void AddListener(Action<T> callback) {
            foreach (var existingCallback in Listener.GetInvocationList()) {
                if (existingCallback == (MulticastDelegate) callback) {
                    SignalsErrorHandler.LogErrorDuplicationListenersError(callback.Method.Name);
                }
            }

            Listener += callback;
        }

        /// <summary>
        /// Подписка на сигнал (с момента подписки сработает только один раз)
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала</param>
        public void AddOnce(Action<T> callback) {
            OnceListener += callback;
        }

        /// <summary>
        /// Отписка от сигнала
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала, который был подписан</param>
        public void RemoveListener(Action<T> callback) {
            Listener -= callback;
            OnceListener -= callback;
        }

        /// <summary>
        /// Отпрака сигнала всем подписчикам
        /// </summary>
        public void Dispatch(T type1) {
            Listener(type1);
            OnceListener(type1);
            OnceListener = delegate { };
        }

        /// <summary>
        /// Удаление всех лиснеров
        /// </summary>
        public void RemoveAllListeners() {
            Listener = delegate { };
            OnceListener = delegate { };
        }
    }

    public class Signal<T, U> {
        /// <summary>
        /// Делегат для подписок
        /// </summary>
        public event Action<T, U> Listener = delegate { };

        /// <summary>
        /// Делегат для подписки на одно получение сигнала (после вызова список подписчиков очищается)
        /// </summary>
        public event Action<T, U> OnceListener = delegate { };

        /// <summary>
        /// Подписка на сигнал
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала</param>
        public void AddListener(Action<T, U> callback) {
            foreach (var existingCallback in Listener.GetInvocationList()) {
                if (existingCallback == (MulticastDelegate) callback) {
                    SignalsErrorHandler.LogErrorDuplicationListenersError(callback.Method.Name);
                }
            }

            Listener += callback;
        }

        /// <summary>
        /// Подписка на сигнал (с момента подписки сработает только один раз)
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала</param>
        public void AddOnce(Action<T, U> callback) {
            OnceListener += callback;
        }

        /// <summary>
        /// Отписка от сигнала
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала, который был подписан</param>
        public void RemoveListener(Action<T, U> callback) {
            Listener -= callback;
            OnceListener -= callback;
        }

        /// <summary>
        /// Отпрака сигнала всем подписчикам
        /// </summary>
        public void Dispatch(T type1, U type2) {
            Listener(type1, type2);
            OnceListener(type1, type2);
            OnceListener = delegate { };
        }

        /// <summary>
        /// Удаление всех лиснеров
        /// </summary>
        public void RemoveAllListeners() {
            Listener = delegate { };
            OnceListener = delegate { };
        }
    }

    public class Signal<T, U, V> {
        /// <summary>
        /// Делегат для подписок
        /// </summary>
        public event Action<T, U, V> Listener = delegate { };

        /// <summary>
        /// Делегат для подписки на одно получение сигнала (после вызова список подписчиков очищается)
        /// </summary>
        public event Action<T, U, V> OnceListener = delegate { };

        /// <summary>
        /// Подписка на сигнал
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала</param>
        public void AddListener(Action<T, U, V> callback) {
            foreach (var existingCallback in Listener.GetInvocationList()) {
                if (existingCallback == (MulticastDelegate) callback) {
                    SignalsErrorHandler.LogErrorDuplicationListenersError(callback.Method.Name);
                }
            }

            Listener += callback;
        }

        /// <summary>
        /// Подписка на сигнал (с момента подписки сработает только один раз)
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала</param>
        public void AddOnce(Action<T, U, V> callback) {
            OnceListener += callback;
        }

        /// <summary>
        /// Отписка от сигнала
        /// </summary>
        /// <param name="callback">Отклик на Dispatch() сигнала, который был подписан</param>
        public void RemoveListener(Action<T, U, V> callback) {
            Listener -= callback;
            OnceListener -= callback;
        }

        /// <summary>
        /// Отпрака сигнала всем подписчикам
        /// </summary>
        public void Dispatch(T type1, U type2, V type3) {
            Listener(type1, type2, type3);
            OnceListener(type1, type2, type3);
            OnceListener = delegate { };
        }

        /// <summary>
        /// Удаление всех лиснеров
        /// </summary>
        public void RemoveAllListeners() {
            Listener = delegate { };
            OnceListener = delegate { };
        }
    }

    /// <summary>
    /// Хендлер ошибок в сигналах
    /// </summary>
    public static class SignalsErrorHandler {
        /// <summary>
        /// Отправляет ошибку дублирующихся сигналов в лог и в кибану
        /// </summary>
        /// <param name="methodName">Имя метода</param>
        public static void LogErrorDuplicationListenersError(string methodName) {
            Debug.LogError(
                $"Signals: DUPLICATING LISTENERS DETECTED!!! IT MAY CAUSE UNEXPECTED BEHAVIOUR! Method name:{methodName}");
        }
    }
}