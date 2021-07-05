using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099B RID: 2459
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		// Token: 0x06003DBE RID: 15806 RVA: 0x00153358 File Offset: 0x00151558
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (Find.TaleManager.GetLatestTale(this.def.taleDef, other) == null)
			{
				return false;
			}
			return true;
		}
	}
}
