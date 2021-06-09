using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200094F RID: 2383
	public static class SoundSlotManager
	{
		// Token: 0x06003A77 RID: 14967 RVA: 0x00169F20 File Offset: 0x00168120
		public static bool CanPlayNow(string slotName)
		{
			if (slotName == "")
			{
				return true;
			}
			float num = 0f;
			return !SoundSlotManager.allowedPlayTimes.TryGetValue(slotName, out num) || Time.realtimeSinceStartup >= SoundSlotManager.allowedPlayTimes[slotName];
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x00169F68 File Offset: 0x00168168
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

		// Token: 0x04002882 RID: 10370
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();
	}
}
