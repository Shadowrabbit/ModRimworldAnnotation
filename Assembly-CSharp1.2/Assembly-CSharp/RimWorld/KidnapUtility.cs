using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015AA RID: 5546
	public static class KidnapUtility
	{
		// Token: 0x0600786F RID: 30831 RVA: 0x002252B8 File Offset: 0x002234B8
		public static bool IsKidnapped(this Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
