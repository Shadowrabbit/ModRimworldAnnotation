using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001049 RID: 4169
	public static class PassAllQuestPartUtility
	{
		// Token: 0x06005AE5 RID: 23269 RVA: 0x001D6CC8 File Offset: 0x001D4EC8
		public static bool AllReceived(List<string> inSignals, List<bool> signalsReceived)
		{
			if (inSignals.Count != signalsReceived.Count)
			{
				return false;
			}
			for (int i = 0; i < signalsReceived.Count; i++)
			{
				if (!signalsReceived[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
