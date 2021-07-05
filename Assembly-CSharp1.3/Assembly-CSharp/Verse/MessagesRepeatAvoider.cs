using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200041C RID: 1052
	public static class MessagesRepeatAvoider
	{
		// Token: 0x06001FA7 RID: 8103 RVA: 0x000C49B6 File Offset: 0x000C2BB6
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x000C49C4 File Offset: 0x000C2BC4
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

		// Token: 0x04001337 RID: 4919
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();
	}
}
