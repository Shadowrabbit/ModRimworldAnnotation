using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000854 RID: 2132
	public static class GenThreading
	{
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06003570 RID: 13680 RVA: 0x000298C4 File Offset: 0x00027AC4
		public static int ProcessorCount
		{
			get
			{
				return Environment.ProcessorCount;
			}
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000298CB File Offset: 0x00027ACB
		private static void GetMaxDegreeOfParallelism(ref int maxDegreeOfParallelism)
		{
			if (maxDegreeOfParallelism == -1)
			{
				maxDegreeOfParallelism = GenThreading.ProcessorCount;
			}
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x001586CC File Offset: 0x001568CC
		public static List<GenThreading.Slice> SliceWork(int fromInclusive, int toExclusive, int maxBatches)
		{
			List<GenThreading.Slice> list = new List<GenThreading.Slice>(maxBatches);
			int num = toExclusive - fromInclusive;
			if (num <= 0)
			{
				return list;
			}
			int num2 = num % maxBatches;
			int num3 = Mathf.FloorToInt((float)num / (float)maxBatches);
			if (num3 > 0)
			{
				int num4 = 0;
				for (int i = 0; i < maxBatches; i++)
				{
					int num5 = num3;
					if (num2 > 0)
					{
						num5++;
						num2--;
					}
					list.Add(new GenThreading.Slice(num4, num4 + num5));
					num4 += num5;
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					list.Add(new GenThreading.Slice(j, j + 1));
				}
			}
			return list;
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x00158760 File Offset: 0x00156960
		public static List<List<T>> SliceWork<T>(List<T> list, int maxBatches)
		{
			List<List<T>> list2 = new List<List<T>>(maxBatches);
			foreach (GenThreading.Slice slice in GenThreading.SliceWork(0, list.Count, maxBatches))
			{
				List<T> list3 = new List<T>(slice.toExclusive - slice.fromInclusive);
				for (int i = slice.fromInclusive; i < slice.toExclusive; i++)
				{
					list3.Add(list[i]);
				}
				list2.Add(list3);
			}
			return list2;
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x00158800 File Offset: 0x00156A00
		public static void ParallelForEach<T>(List<T> list, Action<T> callback, int maxDegreeOfParallelism = -1)
		{
			GenThreading.GetMaxDegreeOfParallelism(ref maxDegreeOfParallelism);
			int count = list.Count;
			long tasksDone = 0L;
			AutoResetEvent taskDoneEvent = new AutoResetEvent(false);
			using (List<List<T>>.Enumerator enumerator = GenThreading.SliceWork<T>(list, maxDegreeOfParallelism).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<T> localBatch2 = enumerator.Current;
					List<T> localBatch = localBatch2;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						foreach (T obj in localBatch)
						{
							try
							{
								callback(obj);
							}
							catch (Exception arg)
							{
								Log.Error("Error in ParallelForEach(): " + arg, false);
							}
						}
						Interlocked.Add(ref tasksDone, (long)localBatch.Count);
						taskDoneEvent.Set();
					});
				}
				goto IL_8F;
			}
			IL_83:
			taskDoneEvent.WaitOne();
			IL_8F:
			if (Interlocked.Read(ref tasksDone) >= (long)count)
			{
				return;
			}
			goto IL_83;
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x001588BC File Offset: 0x00156ABC
		public static void ParallelFor(int fromInclusive, int toExclusive, Action<int> callback, int maxDegreeOfParallelism = -1)
		{
			GenThreading.GetMaxDegreeOfParallelism(ref maxDegreeOfParallelism);
			int num = toExclusive - fromInclusive;
			long tasksDone = 0L;
			AutoResetEvent taskDoneEvent = new AutoResetEvent(false);
			using (List<GenThreading.Slice>.Enumerator enumerator = GenThreading.SliceWork(fromInclusive, toExclusive, maxDegreeOfParallelism).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenThreading.Slice localBatch2 = enumerator.Current;
					GenThreading.Slice localBatch = localBatch2;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						for (int i = localBatch.fromInclusive; i < localBatch.toExclusive; i++)
						{
							try
							{
								callback(i);
							}
							catch (Exception arg)
							{
								Log.Error("Error in ParallelFor(): " + arg, false);
							}
						}
						Interlocked.Add(ref tasksDone, (long)(localBatch.toExclusive - localBatch.fromInclusive));
						taskDoneEvent.Set();
					});
				}
				goto IL_8D;
			}
			IL_81:
			taskDoneEvent.WaitOne();
			IL_8D:
			if (Interlocked.Read(ref tasksDone) >= (long)num)
			{
				return;
			}
			goto IL_81;
		}

		// Token: 0x02000855 RID: 2133
		public struct Slice
		{
			// Token: 0x06003576 RID: 13686 RVA: 0x000298D9 File Offset: 0x00027AD9
			public Slice(int fromInclusive, int toExclusive)
			{
				this.fromInclusive = fromInclusive;
				this.toExclusive = toExclusive;
			}

			// Token: 0x04002524 RID: 9508
			public readonly int fromInclusive;

			// Token: 0x04002525 RID: 9509
			public readonly int toExclusive;
		}
	}
}
