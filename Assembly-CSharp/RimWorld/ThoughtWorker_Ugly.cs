using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9F RID: 3743
	public class ThoughtWorker_Ugly : ThoughtWorker
	{
		// Token: 0x0600538D RID: 21389 RVA: 0x001C107C File Offset: 0x001BF27C
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			float statValue = other.GetStatValue(StatDefOf.PawnBeauty, true);
			if (statValue <= -2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (statValue <= -1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}
