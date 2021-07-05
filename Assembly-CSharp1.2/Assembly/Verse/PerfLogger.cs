using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x02000882 RID: 2178
	public static class PerfLogger
	{
		// Token: 0x06003606 RID: 13830 RVA: 0x00029DE5 File Offset: 0x00027FE5
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x00029E01 File Offset: 0x00028001
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog != null) ? PerfLogger.currentLog.ToString() : "", false);
			PerfLogger.Reset();
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x0015B60C File Offset: 0x0015980C
		public static void Record(string label)
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (PerfLogger.currentLog == null)
			{
				PerfLogger.currentLog = new StringBuilder();
			}
			PerfLogger.currentLog.AppendLine(string.Format("{0}: {3}{1} ({2})", new object[]
			{
				(timestamp - PerfLogger.start) * 1000L / Stopwatch.Frequency,
				label,
				(timestamp - PerfLogger.current) * 1000L / Stopwatch.Frequency,
				new string(' ', PerfLogger.indent * 2)
			}));
			PerfLogger.current = timestamp;
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x00029E26 File Offset: 0x00028026
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x00029E34 File Offset: 0x00028034
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x00029E42 File Offset: 0x00028042
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}

		// Token: 0x040025AF RID: 9647
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x040025B0 RID: 9648
		private static long start;

		// Token: 0x040025B1 RID: 9649
		private static long current;

		// Token: 0x040025B2 RID: 9650
		private static int indent;
	}
}
