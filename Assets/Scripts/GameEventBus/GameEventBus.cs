using System;
using System.Collections.Generic;

namespace WAS.EventBus {
    /// <summary>
    /// Simple typed event bus using the observer pattern.
    /// Allows systems to subscribe to and publish events by type.
    /// </summary>
    public static class GameEventBus {
        private static readonly Dictionary<Type, Delegate> _eventTable =
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
        /// Remove all listeners from the bus.
        /// </summary>
        public static void Clear() {
            _eventTable.Clear();
        }
    } 
}