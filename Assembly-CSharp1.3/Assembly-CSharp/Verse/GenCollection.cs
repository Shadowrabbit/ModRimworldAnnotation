using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000025 RID: 37
	public static class GenCollection
	{
		// Token: 0x060001DF RID: 479 RVA: 0x00009A10 File Offset: 0x00007C10
		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				hashSet.Add(item);
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009A5C File Offset: 0x00007C5C
		public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dest, IDictionary<TKey, TValue> source)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
			{
				dest.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009AB4 File Offset: 0x00007CB4
		public static void SetOrAdd<K, V>(this Dictionary<K, V> dict, K key, V value)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = value;
				return;
			}
			dict.Add(key, value);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009AD0 File Offset: 0x00007CD0
		public static void AddDistinct<K, V>(this Dictionary<K, V> dict, K key, V value)
		{
			if (!dict.ContainsKey(key))
			{
				dict.Add(key, value);
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009AE4 File Offset: 0x00007CE4
		public static void Increment<K>(this Dictionary<K, int> dict, K key)
		{
			if (dict.ContainsKey(key))
			{
				int num = dict[key];
				dict[key] = num + 1;
				return;
			}
			dict[key] = 1;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009B18 File Offset: 0x00007D18
		public static bool SharesElementWith<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			IList<T> list;
			IList<T> list2;
			if ((list = (source as IList<T>)) != null && (list2 = (other as IList<T>)) != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						if (EqualityComparer<T>.Default.Equals(list[i], list2[j]))
						{
							return true;
						}
					}
				}
				return false;
			}
			return source.Any((T item) => other.Contains(item));
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009BA2 File Offset: 0x00007DA2
		public static IEnumerable<T> InRandomOrder<T>(this IEnumerable<T> source, IList<T> workingList = null)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (workingList == null)
			{
				workingList = source.ToList<T>();
			}
			else
			{
				workingList.Clear();
				foreach (T item in source)
				{
					workingList.Add(item);
				}
			}
			int countUnChosen = workingList.Count;
			int rand = 0;
			while (countUnChosen > 0)
			{
				rand = Rand.Range(0, countUnChosen);
				yield return workingList[rand];
				T value = workingList[rand];
				workingList[rand] = workingList[countUnChosen - 1];
				workingList[countUnChosen - 1] = value;
				int num = countUnChosen;
				countUnChosen = num - 1;
			}
			yield break;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009BBC File Offset: 0x00007DBC
		public static T RandomElement<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list == null)
			{
				list = source.ToList<T>();
			}
			if (list.Count == 0)
			{
				Log.Warning("Getting random element from empty collection.");
				return default(T);
			}
			return list[Rand.Range(0, list.Count)];
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009C18 File Offset: 0x00007E18
		public static T RandomElementWithFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElement(out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009C34 File Offset: 0x00007E34
		public static bool TryRandomElement<T>(this IEnumerable<T> source, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				if (list.Count == 0)
				{
					result = default(T);
					return false;
				}
			}
			else
			{
				list = source.ToList<T>();
				if (!list.Any<T>())
				{
					result = default(T);
					return false;
				}
			}
			result = list.RandomElement<T>();
			return true;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009C90 File Offset: 0x00007E90
		public static T RandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
		{
			float num = 0f;
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num2,
							" from ",
							list[i]
						}));
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					return list[0];
				}
			}
			else
			{
				int num3 = 0;
				foreach (T t in source)
				{
					num3++;
					float num4 = weightSelector(t);
					if (num4 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num4,
							" from ",
							t
						}));
						num4 = 0f;
					}
					num += num4;
				}
				if (num3 == 1 && num > 0f)
				{
					return source.First<T>();
				}
			}
			if (num <= 0f)
			{
				Log.Error("RandomElementByWeight with totalWeight=" + num + " - use TryRandomElementByWeight.");
				return default(T);
			}
			float num5 = Rand.Value * num;
			float num6 = 0f;
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					float num7 = weightSelector(list[j]);
					if (num7 > 0f)
					{
						num6 += num7;
						if (num6 >= num5)
						{
							return list[j];
						}
					}
				}
			}
			else
			{
				foreach (T t2 in source)
				{
					float num8 = weightSelector(t2);
					if (num8 > 0f)
					{
						num6 += num8;
						if (num6 >= num5)
						{
							return t2;
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009ED4 File Offset: 0x000080D4
		public static T RandomElementByWeightWithFallback<T>(this IEnumerable<T> source, Func<T, float> weightSelector, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElementByWeight(weightSelector, out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009EF0 File Offset: 0x000080F0
		public static bool TryRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
		{
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num2,
							" from ",
							list[i]
						}));
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					result = list[0];
					return true;
				}
				if (num == 0f)
				{
					result = default(T);
					return false;
				}
				num *= Rand.Value;
				for (int j = 0; j < list.Count; j++)
				{
					float num3 = weightSelector(list[j]);
					if (num3 > 0f)
					{
						num -= num3;
						if (num <= 0f)
						{
							result = list[j];
							return true;
						}
					}
				}
			}
			IEnumerator<T> enumerator = source.GetEnumerator();
			result = default(T);
			float num4 = 0f;
			while (num4 == 0f && enumerator.MoveNext())
			{
				result = enumerator.Current;
				num4 = weightSelector(result);
				if (num4 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num4,
						" from ",
						result
					}));
					num4 = 0f;
				}
			}
			if (num4 == 0f)
			{
				result = default(T);
				return false;
			}
			while (enumerator.MoveNext())
			{
				T t = enumerator.Current;
				float num5 = weightSelector(t);
				if (num5 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num5,
						" from ",
						t
					}));
					num5 = 0f;
				}
				if (Rand.Range(0f, num4 + num5) >= num4)
				{
					result = t;
				}
				num4 += num5;
			}
			return true;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A128 File Offset: 0x00008328
		public static T RandomElementByWeightWithDefault<T>(this IEnumerable<T> source, Func<T, float> weightSelector, float defaultValueWeight)
		{
			if (defaultValueWeight < 0f)
			{
				Log.Error("Negative default value weight.");
				defaultValueWeight = 0f;
			}
			float num = 0f;
			foreach (T t in source)
			{
				float num2 = weightSelector(t);
				if (num2 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num2,
						" from ",
						t
					}));
					num2 = 0f;
				}
				num += num2;
			}
			float num3 = defaultValueWeight + num;
			if (num3 <= 0f)
			{
				Log.Error("RandomElementByWeightWithDefault with totalWeight=" + num3);
				return default(T);
			}
			if (Rand.Value < defaultValueWeight / num3 || num == 0f)
			{
				return default(T);
			}
			return source.RandomElementByWeight(weightSelector);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000A22C File Offset: 0x0000842C
		public static T FirstOrFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return fallback;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000A274 File Offset: 0x00008474
		public static T FirstOrFallback<T>(this IEnumerable<T> source, Func<T, bool> predicate, T fallback = default(T))
		{
			return source.Where(predicate).FirstOrFallback(fallback);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A283 File Offset: 0x00008483
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A294 File Offset: 0x00008494
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) > 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000A340 File Offset: 0x00008540
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, TSource fallback = default(TSource))
		{
			return source.MaxByWithFallback(selector, Comparer<TKey>.Default, fallback);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000A350 File Offset: 0x00008550
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, TSource fallback = default(TSource))
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = fallback;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					result = tsource;
				}
			}
			return result;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A3F8 File Offset: 0x000085F8
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMaxBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A408 File Offset: 0x00008608
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A4BC File Offset: 0x000086BC
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000A4CC File Offset: 0x000086CC
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) < 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A578 File Offset: 0x00008778
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMinBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A588 File Offset: 0x00008788
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) < 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000A63C File Offset: 0x0000883C
		public static void SortBy<T, TSortBy>(this List<T> list, Func<T, TSortBy> selector) where TSortBy : IComparable<TSortBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				return tsortBy.CompareTo(selector(b));
			});
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000A674 File Offset: 0x00008874
		public static void SortBy<T, TSortBy, TThenBy>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				TSortBy other = selector(b);
				if (!tsortBy.Equals(other))
				{
					return tsortBy.CompareTo(other);
				}
				TThenBy tthenBy = thenBySelector(a);
				return tthenBy.CompareTo(thenBySelector(b));
			});
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A6B4 File Offset: 0x000088B4
		public static void SortBy<T, TSortBy, TThenBy, TThenBy2>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector, Func<T, TThenBy2> thenBy2Selector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>, IEquatable<TThenBy> where TThenBy2 : IComparable<TThenBy2>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				TSortBy other = selector(b);
				if (!tsortBy.Equals(other))
				{
					return tsortBy.CompareTo(other);
				}
				TThenBy tthenBy = thenBySelector(a);
				TThenBy other2 = thenBySelector(b);
				if (!tthenBy.Equals(other2))
				{
					return tthenBy.CompareTo(other2);
				}
				TThenBy2 tthenBy2 = thenBy2Selector(a);
				return tthenBy2.CompareTo(thenBy2Selector(b));
			});
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000A6F8 File Offset: 0x000088F8
		public static void SortByColor<T>(this List<T> colorDefs, Func<T, Color> getColor)
		{
			colorDefs.SortBy(delegate(T x)
			{
				float num;
				float a;
				float num2;
				Color.RGBToHSV(getColor(x), out num, out a, out num2);
				if (!Mathf.Approximately(a, 0f))
				{
					return (float)Mathf.RoundToInt(num * 100f);
				}
				return -1f;
			}, delegate(T x)
			{
				float num;
				float num2;
				float num3;
				Color.RGBToHSV(getColor(x), out num, out num2, out num3);
				return Mathf.RoundToInt(num3 * 100f);
			});
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000A730 File Offset: 0x00008930
		public static void SortByDescending<T, TSortByDescending>(this List<T> list, Func<T, TSortByDescending> selector) where TSortByDescending : IComparable<TSortByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending tsortByDescending = selector(b);
				return tsortByDescending.CompareTo(selector(a));
			});
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A768 File Offset: 0x00008968
		public static void SortByDescending<T, TSortByDescending, TThenByDescending>(this List<T> list, Func<T, TSortByDescending> selector, Func<T, TThenByDescending> thenByDescendingSelector) where TSortByDescending : IComparable<TSortByDescending>, IEquatable<TSortByDescending> where TThenByDescending : IComparable<TThenByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending other = selector(a);
				TSortByDescending other2 = selector(b);
				if (!other.Equals(other2))
				{
					return other2.CompareTo(other);
				}
				TThenByDescending tthenByDescending = thenByDescendingSelector(b);
				return tthenByDescending.CompareTo(thenByDescendingSelector(a));
			});
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A7A8 File Offset: 0x000089A8
		public static void SortStable<T>(this IList<T> list, Func<T, T, int> comparator)
		{
			if (list.Count <= 1)
			{
				return;
			}
			List<Pair<T, int>> list2;
			bool flag;
			if (GenCollection.SortStableTempList<T>.working)
			{
				list2 = new List<Pair<T, int>>();
				flag = false;
			}
			else
			{
				list2 = GenCollection.SortStableTempList<T>.list;
				GenCollection.SortStableTempList<T>.working = true;
				flag = true;
			}
			try
			{
				list2.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(new Pair<T, int>(list[i], i));
				}
				list2.Sort(delegate(Pair<T, int> lhs, Pair<T, int> rhs)
				{
					int num = comparator(lhs.First, rhs.First);
					if (num != 0)
					{
						return num;
					}
					return lhs.Second.CompareTo(rhs.Second);
				});
				list.Clear();
				for (int j = 0; j < list2.Count; j++)
				{
					list.Add(list2[j].First);
				}
				list2.Clear();
			}
			finally
			{
				if (flag)
				{
					GenCollection.SortStableTempList<T>.working = false;
				}
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A87C File Offset: 0x00008A7C
		public static IComparer<T> ThenBy<T>(this IComparer<T> first, IComparer<T> second)
		{
			return new GenCollection.ComparerChain<T>(first, second);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A885 File Offset: 0x00008A85
		public static IComparer<T> Descending<T>(this IComparer<T> cmp)
		{
			return new GenCollection.DescendingComparer<T>(cmp);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A88D File Offset: 0x00008A8D
		public static IComparer<T> CompareBy<T, TComparable>(Func<T, TComparable> selector) where TComparable : IComparable<TComparable>
		{
			return Comparer<T>.Create(delegate(T a, T b)
			{
				TComparable tcomparable = selector(a);
				return tcomparable.CompareTo(selector(b));
			});
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A8AC File Offset: 0x00008AAC
		public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			List<TKey> list = null;
			int result;
			try
			{
				foreach (KeyValuePair<TKey, TValue> obj in dictionary)
				{
					if (predicate(obj))
					{
						if (list == null)
						{
							list = SimplePool<List<TKey>>.Get();
						}
						list.Add(obj.Key);
					}
				}
				if (list != null)
				{
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						dictionary.Remove(list[i]);
						i++;
					}
					result = list.Count;
				}
				else
				{
					result = 0;
				}
			}
			finally
			{
				if (list != null)
				{
					list.Clear();
					SimplePool<List<TKey>>.Return(list);
				}
			}
			return result;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A968 File Offset: 0x00008B68
		public static void RemoveAll<T>(this List<T> list, Func<T, int, bool> predicate)
		{
			int num = 0;
			int count = list.Count;
			while (num < count && !predicate(list[num], num))
			{
				num++;
			}
			if (num >= count)
			{
				return;
			}
			int i = num + 1;
			while (i < count)
			{
				while (i < count && predicate(list[i], i))
				{
					i++;
				}
				if (i < count)
				{
					list[num++] = list[i++];
				}
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A9D9 File Offset: 0x00008BD9
		public static void RemoveLast<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A9E9 File Offset: 0x00008BE9
		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000AA07 File Offset: 0x00008C07
		public static bool Any<T>(this List<T> list, Predicate<T> predicate)
		{
			return list.FindIndex(predicate) != -1;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000AA16 File Offset: 0x00008C16
		public static bool Any<T>(this List<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000AA21 File Offset: 0x00008C21
		public static bool Any<T>(this HashSet<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000AA2C File Offset: 0x00008C2C
		public static bool Any<T>(this Stack<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000AA38 File Offset: 0x00008C38
		public static void AddRange<T>(this HashSet<T> set, List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				set.Add(list[i]);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000AA64 File Offset: 0x00008C64
		public static void AddRange<T>(this HashSet<T> set, HashSet<T> other)
		{
			foreach (T item in other)
			{
				set.Add(item);
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000AAB4 File Offset: 0x00008CB4
		public static int Count_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return 0;
			}
			ICollection collection = e as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in e)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000AB1C File Offset: 0x00008D1C
		public static object FirstOrDefault_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return null;
			}
			IList list = e as IList;
			if (list == null)
			{
				using (IEnumerator enumerator = e.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
				return null;
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000AB8C File Offset: 0x00008D8C
		public static float AverageWeighted<T>(this IEnumerable<T> list, Func<T, float> weight, Func<T, float> value)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (T arg in list)
			{
				float num3 = weight(arg);
				num += num3;
				num2 += value(arg) * num3;
			}
			return num2 / num;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000ABF8 File Offset: 0x00008DF8
		public static void ExecuteEnumerable(this IEnumerable enumerable)
		{
			foreach (object obj in enumerable)
			{
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000AC40 File Offset: 0x00008E40
		public static bool EnumerableNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return true;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count == 0;
			}
			return !enumerable.Any<T>();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000AC70 File Offset: 0x00008E70
		public static int EnumerableCount(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in enumerable)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000ACD8 File Offset: 0x00008ED8
		public static int FirstIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T arg in enumerable)
			{
				if (predicate(arg))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000AD30 File Offset: 0x00008F30
		public static V TryGetValue<T, V>(this IDictionary<T, V> dict, T key, V fallback = default(V))
		{
			V result;
			if (!dict.TryGetValue(key, out result))
			{
				result = fallback;
			}
			return result;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000AD4B File Offset: 0x00008F4B
		public static IEnumerable<Pair<T, V>> Cross<T, V>(this IEnumerable<T> lhs, IEnumerable<V> rhs)
		{
			T[] lhsv = lhs.ToArray<T>();
			V[] rhsv = rhs.ToArray<V>();
			int num;
			for (int i = 0; i < lhsv.Length; i = num)
			{
				for (int j = 0; j < rhsv.Length; j = num)
				{
					yield return new Pair<T, V>(lhsv[i], rhsv[j]);
					num = j + 1;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000AD62 File Offset: 0x00008F62
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> lhs, T rhs)
		{
			foreach (T t in lhs)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield return rhs;
			yield break;
			yield break;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000AD79 File Offset: 0x00008F79
		public static IEnumerable<TSource> ConcatIfNotNull<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (second == null)
			{
				return first;
			}
			return first.Concat(second);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000AD98 File Offset: 0x00008F98
		public static LocalTargetInfo FirstValid(this List<LocalTargetInfo> source)
		{
			if (source == null)
			{
				return LocalTargetInfo.Invalid;
			}
			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].IsValid)
				{
					return source[i];
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000ADDD File Offset: 0x00008FDD
		public static IEnumerable<T> Except<T>(this IEnumerable<T> lhs, T rhs) where T : class
		{
			foreach (T t in lhs)
			{
				if (t != rhs)
				{
					yield return t;
				}
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000ADF4 File Offset: 0x00008FF4
		public static bool ListsEqual<T>(List<T> a, List<T> b) where T : class
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < a.Count; i++)
			{
				if (!@default.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000AE6C File Offset: 0x0000906C
		public static bool DictsEqual<TKey, TValue>(Dictionary<TKey, TValue> a, Dictionary<TKey, TValue> b)
		{
			if (a.NullOrEmpty<TKey, TValue>() && b.NullOrEmpty<TKey, TValue>())
			{
				return true;
			}
			if (a.NullOrEmpty<TKey, TValue>() || b.NullOrEmpty<TKey, TValue>())
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in a)
			{
				TValue y;
				if (!b.TryGetValue(keyValuePair.Key, out y) || !@default.Equals(keyValuePair.Value, y))
				{
					return false;
				}
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair2 in b)
			{
				TValue y2;
				if (!a.TryGetValue(keyValuePair2.Key, out y2) || !@default.Equals(keyValuePair2.Value, y2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000AF78 File Offset: 0x00009178
		public static bool SetsEqual<T>(this List<T> a, List<T> b)
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!b.Contains(a[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < b.Count; j++)
			{
				if (!a.Contains(b[j]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000AFF4 File Offset: 0x000091F4
		public static IEnumerable<T> TakeRandom<T>(this List<T> list, int count)
		{
			if (list.NullOrEmpty<T>())
			{
				yield break;
			}
			int num;
			for (int i = 0; i < count; i = num)
			{
				yield return list[Rand.Range(0, list.Count)];
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000B00C File Offset: 0x0000920C
		public static void AddDistinct<T>(this List<T> list, T element) where T : class
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == element)
				{
					return;
				}
			}
			list.Add(element);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B048 File Offset: 0x00009248
		public static int Replace<T>(this IList<T> list, T replace, T with) where T : class
		{
			if (list == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == replace)
				{
					list[i] = with;
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B08E File Offset: 0x0000928E
		public static Pair<K, List<E>> ConvertIGroupingToPair<K, E>(IGrouping<K, E> g)
		{
			return new Pair<K, List<E>>(g.Key, g.ToList<E>());
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B0A4 File Offset: 0x000092A4
		public static int GetCountGreaterOrEqualInSortedList(List<int> list, int val)
		{
			int num = list.BinarySearch(val);
			if (num >= 0)
			{
				return list.Count - num;
			}
			int num2 = ~num;
			return list.Count - num2;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B0D4 File Offset: 0x000092D4
		public static void InsertIntoSortedList<T>(this List<T> list, T val, IComparer<T> cmp)
		{
			if (list.Count == 0)
			{
				list.Add(val);
				return;
			}
			int num = list.BinarySearch(val, cmp);
			if (num >= 0)
			{
				list.Insert(num, val);
				return;
			}
			list.Insert(~num, val);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000B110 File Offset: 0x00009310
		public static void RemoveBatchUnordered<T>(this List<T> list, List<int> indices)
		{
			if (indices.Count == 0)
			{
				return;
			}
			int num = list.Count - 1;
			foreach (int num2 in indices)
			{
				if (num == num2)
				{
					num--;
				}
				else
				{
					if (num <= 0)
					{
						break;
					}
					list[num2] = list[num];
					num--;
				}
			}
			for (int i = list.Count - 1; i > num; i--)
			{
				list.RemoveAt(i);
			}
		}

		// Token: 0x0200186C RID: 6252
		private static class SortStableTempList<T>
		{
			// Token: 0x04005D66 RID: 23910
			public static List<Pair<T, int>> list = new List<Pair<T, int>>();

			// Token: 0x04005D67 RID: 23911
			public static bool working;
		}

		// Token: 0x0200186D RID: 6253
		private class ComparerChain<T> : IComparer<T>
		{
			// Token: 0x06009341 RID: 37697 RVA: 0x0034C52B File Offset: 0x0034A72B
			public ComparerChain(IComparer<T> first, IComparer<T> second)
			{
				this.first = first;
				this.second = second;
			}

			// Token: 0x06009342 RID: 37698 RVA: 0x0034C544 File Offset: 0x0034A744
			public int Compare(T x, T y)
			{
				int num = this.first.Compare(x, y);
				if (num != 0)
				{
					return num;
				}
				return this.second.Compare(x, y);
			}

			// Token: 0x04005D68 RID: 23912
			private readonly IComparer<T> first;

			// Token: 0x04005D69 RID: 23913
			private readonly IComparer<T> second;
		}

		// Token: 0x0200186E RID: 6254
		private class DescendingComparer<T> : IComparer<T>
		{
			// Token: 0x06009343 RID: 37699 RVA: 0x0034C571 File Offset: 0x0034A771
			public DescendingComparer(IComparer<T> cmp)
			{
				this.cmp = cmp;
			}

			// Token: 0x06009344 RID: 37700 RVA: 0x0034C580 File Offset: 0x0034A780
			public int Compare(T x, T y)
			{
				return -this.cmp.Compare(x, y);
			}

			// Token: 0x04005D6A RID: 23914
			private readonly IComparer<T> cmp;
		}
	}
}
