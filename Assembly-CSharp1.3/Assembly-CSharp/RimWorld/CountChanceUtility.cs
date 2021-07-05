using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121E RID: 4638
	public static class CountChanceUtility
	{
		// Token: 0x06006F50 RID: 28496 RVA: 0x00252698 File Offset: 0x00250898
		public static int RandomCount(List<CountChance> chances)
		{
			float value = Rand.Value;
			float num = 0f;
			for (int i = 0; i < chances.Count; i++)
			{
				num += chances[i].chance;
				if (value < num)
				{
					if (num > 1f)
					{
						Log.Error("CountChances error: Total chance is " + num + " but it should not be above 1.");
					}
					return chances[i].count;
				}
			}
			return 0;
		}
	}
}
