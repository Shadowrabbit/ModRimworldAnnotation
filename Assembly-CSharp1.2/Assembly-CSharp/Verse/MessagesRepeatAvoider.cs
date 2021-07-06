using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000753 RID: 1875
	public static class MessagesRepeatAvoider
	{
		// Token: 0x06002F47 RID: 12103 RVA: 0x000250C3 File Offset: 0x000232C3
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x0013A918 File Offset: 0x00138B18
		public static bool MessageShowAllowed(string tag, float minSecondsSinceLastShow)
		{
			float num;
			if (!MessagesRepeatAvoider.lastShowTimes.TryGetValue(tag, out num))
			{
				num = -99999f;
			}
			bool flag = RealTime.LastRealTime > num + minSecondsSinceLastShow;
			if (flag)
			{
				MessagesRepeatAvoider.lastShowTimes[tag] = RealTime.LastRealTime;
			}
			return flag;
		}

		// Token: 0x04002013 RID: 8211
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();
	}
}
