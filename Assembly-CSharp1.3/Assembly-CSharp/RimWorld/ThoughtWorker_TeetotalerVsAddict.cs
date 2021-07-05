using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098E RID: 2446
	public class ThoughtWorker_TeetotalerVsAddict : ThoughtWorker
	{
		// Token: 0x06003DA4 RID: 15780 RVA: 0x00152C9C File Offset: 0x00150E9C
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.IsTeetotaler())
			{
				return false;
			}
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			List<Hediff> hediffs = other.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.IsAddiction)
				{
					return true;
				}
			}
			return false;
		}
	}
}
