using System;
using System.Collections.Generic;

namespace WAS.EventBus{
    public static class GameEventBus {
        private static readonly Dictionary<Type, Delegate> _eventTable =
            new Dictionary<Type, Delegate>();

        private static readonly Dictionary<Type, Delegate> _queryTable =
            new Dictionary<Type, Delegate>();

        /// <summary>
        /// Subscribe to an event of type T.
        /// </summary>
        public static void Subscribe<T>(Action<T> listener) where T : struct {
            if (listener == null)
                return;

            var type = typeof(T);
            if (_eventTable.TryGetValue(type, out var current)) {
                _eventTable[type] = Delegate.Combine(current, listener);
            } else {
                _eventTable[type] = listener;
            }
        }

        /// <summary>
        /// Unsubscribe from an event of type T.
        /// </summary>
        public static void Unsubscribe<T>(Action<T> listener) where T : struct {
            if (listener == null)
                return;

            var type = typeof(T);
            if (_eventTable.TryGetValue(type, out var current)) {
                current = Delegate.Remove(current, listener);
                if (current == null) {
                    _eventTable.Remove(type);
                } else {
                    _eventTable[type] = current;
                }
            }
        }

        /// <summary>
        /// Fire an event of type T to all listeners.
        /// </summary>
        public static void Fire<T>(T eventData) where T : struct {
            var type = typeof(T);
            if (_eventTable.TryGetValue(type, out var del)) {
                (del as Action<T>)?.Invoke(eventData);
            }
        }

        /// <summary>
        /// Subscribe to a query of type T that returns TResult.
        /// </summary>
        public static void Subscribe<T, TResult>(Func<T, TResult> provider) where T : struct {
            if (provider == null)
                return;

            var type = typeof(T);
            if (_queryTable.TryGetValue(type, out var current)) {
                _queryTable[type] = Delegate.Combine(current, provider);
            } else {
                _queryTable[type] = provider;
            }
        }

        /// <summary>
        /// Unsubscribe from a query of type T.
        /// </summary>
        public static void Unsubscribe<T, TResult>(Func<T, TResult> provider) where T : struct {
            if (provider == null)
                return;

            var type = typeof(T);
            if (_queryTable.TryGetValue(type, out var current)) {
                current = Delegate.Remove(current, provider);
                if (current == null) {
                    _queryTable.Remove(type);
                } else {
                    _queryTable[type] = current;
                }
            }
        }

        /// <summary>
        /// Fire a query of type T and get the result.
        /// If multiple subscribers exist, the last one's result is returned.
        /// Returns default(TResult) if no subscribers.
        /// </summary>
        public static TResult Query<T, TResult>(T queryData) where T : struct {
            var type = typeof(T);
            if (_queryTable.TryGetValue(type, out var del)) {
                if (del is Func<T, TResult> func) {
                    return func.Invoke(queryData);
                }
            }
            return default;
        }

        /// <summary>
        /// Remove all listeners from the bus.
        /// </summary>
        public static void Clear() {
            _eventTable.Clear();
            _queryTable.Clear();
        }
    } 
}