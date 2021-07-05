using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	// Token: 0x020004D8 RID: 1240
	public static class PerfTest
	{
		// Token: 0x0600257B RID: 9595 RVA: 0x000E9BE4 File Offset: 0x000E7DE4
		public static string TestStandardMilliseconds()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			List<int> list = new List<int>();
			for (int i = 0; i < 100000; i++)
			{
				list.Add(i);
			}
			List<double> list2 = new List<double>();
			while (list.Count > 0)
			{
				int num = list[0];
				list.RemoveAt(0);
				list2.Add(Math.Sqrt((double)num));
			}
			double num2 = 0.0;
			for (int j = 0; j < list2.Count; j++)
			{
				num2 += list2[j];
			}
			stopwatch.Stop();
			return string.Concat(new object[]
			{
				"Elapsed: ",
				stopwatch.Elapsed,
				"\nMilliseconds: ",
				stopwatch.ElapsedMilliseconds,
				"\nSum: ",
				num2
			});
		}
	}
}
