using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000574 RID: 1396
	public static class SoundSlotManager
	{
		// Token: 0x06002921 RID: 10529 RVA: 0x000F8E50 File Offset: 0x000F7050
		public static bool CanPlayNow(string slotName)
		{
			if (slotName == "")
			{
				return true;
			}
			float num = 0f;
			return !SoundSlotManager.allowedPlayTimes.TryGetValue(slotName, out num) || Time.realtimeSinceStartup >= SoundSlotManager.allowedPlayTimes[slotName];
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x000F8E98 File Offset: 0x000F7098
		public static void Notify_Played(string slot, float duration)
		{
			if (slot == "")
			{
				return;
			}
			float a;
			if (SoundSlotManager.allowedPlayTimes.TryGetValue(slot, out a))
			{
				SoundSlotManager.allowedPlayTimes[slot] = Mathf.Max(a, Time.realtimeSinceStartup + duration);
				return;
			}
			SoundSlotManager.allowedPlayTimes[slot] = Time.realtimeSinceStartup + duration;
		}

		// Token: 0x0400197A RID: 6522
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();
	}
}
