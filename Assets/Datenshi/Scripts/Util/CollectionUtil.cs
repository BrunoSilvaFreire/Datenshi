using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Datenshi.Scripts.Util {
    public static class CollectionUtil {
        public static bool IsEmpty<T>(this IList<T> list) {
            return list.Count <= 0;
        }

        public static T GetOrPut<T>(this ICollection<T> collection, Func<T, bool> predicate, Func<T> instantiator) {
            foreach (var obj in collection) {
                if (predicate(obj)) {
                    return obj;
                }
            }

            var newObj = instantiator();
            collection.Add(newObj);
            return newObj;
        }

        public static V GetOrPut<K, V>(this IDictionary<K, V> collection, K key, Func<V> instantiator) {
            if (collection.ContainsKey(key)) {
                return collection[key];
            }

            var newObj = instantiator();
            collection[key] = newObj;
            return newObj;
        }

        public static bool HasSingle(this ICollection collection) {
            return collection.Count == 1;
        }

        public static List<T> GetAllOrPut<T>(
            this ICollection<T> collection,
            Func<T, bool> predicate,
            Func<T> instantiator) {
            var list = collection.Where(predicate).ToList();
            if (list.IsEmpty()) {
                var newObj = instantiator();
                collection.Add(newObj);
                list.Add(newObj);
            }

            return list;
        }

        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector) {
            return source.MinBy(selector, null);
        }

        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator()) {
                if (!sourceIterator.MoveNext()) {
                    throw new InvalidOperationException("Sequence contains no elements");
                }

                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext()) {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) >= 0) {
                        continue;
                    }

                    min = candidate;
                    minKey = candidateProjected;
                }

                return min;
            }
        }

        public static List<T> EmptyList<T>() {
            return Enumerable.Empty<T>().ToList();
        }

        public static E RandomElement<E>(this IList<E> list) {
            return list.RandomElement(new Random());
        }

        public static E RandomElement<E>(this IList<E> list, Random random) {
            return list[random.Next(list.Count)];
        }

        public static E FirstOrAdd<E>(this IList<E> list, Func<E, bool> selector, Func<E> creator) {
            var f = list.FirstOrDefault(selector);
            if (f == null) {
                f = creator();
            }

            return f;
        }

        public static E CreateAndAdd<E>(this IList<E> list, Func<E> creator) {
            var e = creator();
            list.Add(e);
            return e;
        }

        public static E FirstOrDefaultComparable<E, T>(this IEnumerable<E> list, T comparable)
            where E : IComparable<T> {
            return list.FirstOrDefault(e => e.CompareTo(comparable) == 0);
        }

        public static List<T> CompositeList<T>(IEnumerable<IEnumerable<T>> enumerables) {
            var list = new List<T>();
            foreach (var enumerable in enumerables) {
                list.AddRange(enumerable);
            }

            return list;
        }
    }
}