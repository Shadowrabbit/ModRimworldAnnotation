﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000499 RID: 1177
	public static class Dijkstra<T>
	{
		// Token: 0x060023E0 RID: 9184 RVA: 0x000DF6C1 File Offset: 0x000DD8C1
		public static void Run(T startingNode, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, List<KeyValuePair<T, float>> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.singleNodeList.Clear();
			Dijkstra<T>.singleNodeList.Add(startingNode);
			Dijkstra<T>.Run(Dijkstra<T>.singleNodeList, neighborsGetter, distanceGetter, outDistances, outParents);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x000DF6E8 File Offset: 0x000DD8E8
		public static void Run(IEnumerable<T> startingNodes, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, List<KeyValuePair<T, float>> outDistances, Dictionary<T, T> outParents = null)
		{
			outDistances.Clear();
			Dijkstra<T>.distances.Clear();
			Dijkstra<T>.queue.Clear();
			if (outParents != null)
			{
				outParents.Clear();
			}
			IList<T> list = startingNodes as IList<!0>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					T key = list[i];
					if (!Dijkstra<T>.distances.ContainsKey(key))
					{
						Dijkstra<T>.distances.Add(key, 0f);
						Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(key, 0f));
					}
				}
				goto IL_183;
			}
			using (IEnumerator<T> enumerator = startingNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T key2 = enumerator.Current;
					if (!Dijkstra<T>.distances.ContainsKey(key2))
					{
						Dijkstra<T>.distances.Add(key2, 0f);
						Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(key2, 0f));
					}
				}
				goto IL_183;
			}
			IL_DC:
			KeyValuePair<T, float> node = Dijkstra<T>.queue.Pop();
			float num = Dijkstra<T>.distances[node.Key];
			if (node.Value == num)
			{
				IEnumerable<T> enumerable = neighborsGetter(node.Key);
				if (enumerable != null)
				{
					IList<T> list2 = enumerable as IList<!0>;
					if (list2 != null)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							Dijkstra<T>.HandleNeighbor(list2[j], num, node, distanceGetter, outParents);
						}
					}
					else
					{
						foreach (!0 n in enumerable)
						{
							Dijkstra<T>.HandleNeighbor(n, num, node, distanceGetter, outParents);
						}
					}
				}
			}
			IL_183:
			if (Dijkstra<T>.queue.Count == 0)
			{
				foreach (KeyValuePair<T, float> item in Dijkstra<T>.distances)
				{
					outDistances.Add(item);
				}
				Dijkstra<T>.distances.Clear();
				return;
			}
			goto IL_DC;
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x000DF8F4 File Offset: 0x000DDAF4
		public static void Run(T startingNode, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, Dictionary<T, float> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.singleNodeList.Clear();
			Dijkstra<T>.singleNodeList.Add(startingNode);
			Dijkstra<T>.Run(Dijkstra<T>.singleNodeList, neighborsGetter, distanceGetter, outDistances, outParents);
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x000DF91C File Offset: 0x000DDB1C
		public static void Run(IEnumerable<T> startingNodes, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, Dictionary<T, float> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.Run(startingNodes, neighborsGetter, distanceGetter, Dijkstra<T>.tmpResult, outParents);
			outDistances.Clear();
			for (int i = 0; i < Dijkstra<T>.tmpResult.Count; i++)
			{
				outDistances.Add(Dijkstra<T>.tmpResult[i].Key, Dijkstra<T>.tmpResult[i].Value);
			}
			Dijkstra<T>.tmpResult.Clear();
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x000DF98C File Offset: 0x000DDB8C
		private static void HandleNeighbor(T n, float nodeDist, KeyValuePair<T, float> node, Func<T, T, float> distanceGetter, Dictionary<T, T> outParents)
		{
			float num = nodeDist + Mathf.Max(distanceGetter(node.Key, n), 0f);
			bool flag = false;
			float num2;
			if (Dijkstra<T>.distances.TryGetValue(n, out num2))
			{
				if (num < num2)
				{
					Dijkstra<T>.distances[n] = num;
					flag = true;
				}
			}
			else
			{
				Dijkstra<T>.distances.Add(n, num);
				flag = true;
			}
			if (flag)
			{
				Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(n, num));
				if (outParents != null)
				{
					if (outParents.ContainsKey(n))
					{
						outParents[n] = node.Key;
						return;
					}
					outParents.Add(n, node.Key);
				}
			}
		}

		// Token: 0x040016A2 RID: 5794
		private static Dictionary<T, float> distances = new Dictionary<T, float>();

		// Token: 0x040016A3 RID: 5795
		private static FastPriorityQueue<KeyValuePair<T, float>> queue = new FastPriorityQueue<KeyValuePair<T, float>>(new Dijkstra<T>.DistanceComparer());

		// Token: 0x040016A4 RID: 5796
		private static List<T> singleNodeList = new List<T>();

		// Token: 0x040016A5 RID: 5797
		private static List<KeyValuePair<T, float>> tmpResult = new List<KeyValuePair<T, float>>();

		// Token: 0x02001C9B RID: 7323
		private class DistanceComparer : IComparer<KeyValuePair<T, float>>
		{
			// Token: 0x0600A7A6 RID: 42918 RVA: 0x00384190 File Offset: 0x00382390
			public int Compare(KeyValuePair<T, float> a, KeyValuePair<T, float> b)
			{
				return a.Value.CompareTo(b.Value);
			}
		}
	}
}
