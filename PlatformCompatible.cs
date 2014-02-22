using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	//public delegate void Action<T1, T2>(T1 t1, T2 t2);

	//public delegate void Action<T1, T2, T3>(T1 t1, T2 t2, T3 t3);

	//public delegate void Action<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);

	//public delegate TResult Func<TResult>();

	//public delegate TResult Func<T, TResult>(T t);

	//public delegate TResult Func<T1, T2, TResult>(T1 t1, T2 t2);

	//public delegate TResult Func<T1, T2, T3, TResult>(T1 t1, T2 t2, T3 t3);

	//public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 t1, T2 t2, T3 t3, T4 t4);

#if SL||WP||MONOTOUCH||NET2||NET35
		namespace Collections.Concurrent
{
    public class ConcurrentQueue<T>
    {
        private Queue<T> m_Queue;

        private object m_SyncRoot = new object();

        public ConcurrentQueue()
        {
            m_Queue = new Queue<T>();
        }

        public ConcurrentQueue(int capacity)
        {
            m_Queue = new Queue<T>(capacity);
        }

        public ConcurrentQueue(IEnumerable<T> collection)
        {
            m_Queue = new Queue<T>(collection);
        }

        public void Enqueue(T item)
        {
            lock (m_SyncRoot)
            {
                m_Queue.Enqueue(item);
            }
        }

        public bool TryDequeue(out T item)
        {
            lock (m_SyncRoot)
            {
                if (m_Queue.Count <= 0)
                {
                    item = default(T);
                    return false;
                }

                item = m_Queue.Dequeue();
                return true;
            }
        }
    }
}

	namespace Runtime.CompilerServices
	{
		[AttributeUsage(AttributeTargets.Method)]
		public sealed class ExtensionAttribute : Attribute
		{
			public ExtensionAttribute() { }
		}
	}

	namespace Linq
{
	public static class LINQ
	{
		public static int Count<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
		{
			int count = 0;

			foreach (var item in source)
			{
				if (predicate(item))
					count++;
			}

			return count;
		}

		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource target)
		{
			foreach (var item in source)
			{
				if (item.Equals(target))
					return true;
			}

			return false;
		}

		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
		{
			foreach (var item in source)
			{
				if (predicate(item))
					return item;
			}

			return default(TSource);
		}

		public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> getter)
		{
			int sum = 0;

			foreach (var item in source)
			{
				sum += getter(item);
			}

			return sum;
		}

		public static IEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> getter)
			where TKey : IComparable<TKey>
		{
			var items = new List<TSource>();

			foreach (var i in source)
			{
				items.Add(i);
			}

			items.Sort(new DelegateComparer<TSource, TKey>(getter));

			return items;
		}

		public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
		{
			if (source is TSource[])
				return source as TSource[];

			var items = new List<TSource>();

			foreach (var i in source)
			{
				items.Add(i);
			}

			return items.ToArray();
		}
	}

	class DelegateComparer<TSource, TKey> : IComparer<TSource>
		where TKey : IComparable<TKey>
	{
		private Func<TSource, TKey> m_Getter;

		public DelegateComparer(Func<TSource, TKey> getter)
		{
			m_Getter = getter;
		}

		public int Compare(TSource x, TSource y)
		{
			return m_Getter(x).CompareTo(m_Getter(y));
		}
	}
}


#endif

#if NET2
#endif

#if SL||WP
	namespace Collections.Specialized
	{
		public class NameValueCollection : List<KeyValuePair<string, string>>
		{
			public new string this[int index]
			{
				get
				{
					return base[index].Value;
				}

				set
				{
					var oldKey = base[index].Key;
					base[index] = new KeyValuePair<string, string>(oldKey, value);
				}
			}

			public string this[string name]
			{
				get
				{
					return this.SingleOrDefault(kv => kv.Key.Equals(name)).Value;
				}
				set
				{
					for (var i = 0; i < this.Count; i++)
					{
						if (name.Equals(this[i], StringComparison.OrdinalIgnoreCase))
						{
							this.RemoveAt(i);
							break;
						}
					}

					this.Add(new KeyValuePair<string, string>(name, value));
				}
			}

			public NameValueCollection()
			{

			}

			public NameValueCollection(int capacity)
				: base(capacity)
			{

			}

			public void Add(string name, string value)
			{
				List<KeyValuePair<string, string>> list = this;
				for (int i = Count - 1; i >= 0; --i)
				{
					if (string.Equals(list[i].Key, name))
					{
						list[i] = new KeyValuePair<string, string>(name, list[i].Value + "," + value);
						return;
					}
				}

				Add(new KeyValuePair<string, string>(name, value));
			}

			public IEnumerable<string> AllKeys
			{
				get
				{
					return this.Select(pair => pair.Key);
				}
			}

		}
	}
#endif
}
