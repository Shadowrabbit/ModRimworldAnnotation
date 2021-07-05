using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000B17 RID: 2839
	public static class PassAllQuestPartUtility
	{
		// Token: 0x060042C5 RID: 17093 RVA: 0x0016550C File Offset: 0x0016370C
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
