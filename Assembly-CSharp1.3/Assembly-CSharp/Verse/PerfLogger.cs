using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x020004D7 RID: 1239
	public static class PerfLogger
	{
		// Token: 0x06002574 RID: 9588 RVA: 0x000E9AD1 File Offset: 0x000E7CD1
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000E9AED File Offset: 0x000E7CED
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog != null) ? PerfLogger.currentLog.ToString() : "");
			PerfLogger.Reset();
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000E9B14 File Offset: 0x000E7D14
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

		// Token: 0x06002577 RID: 9591 RVA: 0x000E9BA6 File Offset: 0x000E7DA6
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000E9BB4 File Offset: 0x000E7DB4
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000E9BC2 File Offset: 0x000E7DC2
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}

		// Token: 0x04001773 RID: 6003
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x04001774 RID: 6004
		private static long start;

		// Token: 0x04001775 RID: 6005
		private static long current;

		// Token: 0x04001776 RID: 6006
		private static int indent;
	}
}
