using System;

namespace Verse.AI
{
	// Token: 0x020005BA RID: 1466
	public static class JobFailReason
	{
		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002AC8 RID: 10952 RVA: 0x00100CE7 File Offset: 0x000FEEE7
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x00100CEE File Offset: 0x000FEEEE
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002ACA RID: 10954 RVA: 0x00100CF8 File Offset: 0x000FEEF8
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x00100CFF File Offset: 0x000FEEFF
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x00100D0D File Offset: 0x000FEF0D
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
		}

		// Token: 0x04001A50 RID: 6736
		private static string lastReason;

		// Token: 0x04001A51 RID: 6737
		private static string lastCustomJobString;
	}
}
