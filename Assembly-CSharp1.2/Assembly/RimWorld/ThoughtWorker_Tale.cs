using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA6 RID: 3750
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		// Token: 0x0600539B RID: 21403 RVA: 0x001C13D8 File Offset: 0x001BF5D8
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
