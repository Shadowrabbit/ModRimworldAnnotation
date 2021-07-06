using System;

namespace Verse.AI
{
	// Token: 0x02000A0C RID: 2572
	public static class JobFailReason
	{
		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003D7A RID: 15738 RVA: 0x0002E4F0 File Offset: 0x0002C6F0
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06003D7B RID: 15739 RVA: 0x0002E4F7 File Offset: 0x0002C6F7
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06003D7C RID: 15740 RVA: 0x0002E501 File Offset: 0x0002C701
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x0002E508 File Offset: 0x0002C708
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x0002E516 File Offset: 0x0002C716
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
		}

		// Token: 0x04002AAC RID: 10924
		private static string lastReason;

		// Token: 0x04002AAD RID: 10925
		private static string lastCustomJobString;
	}
}
