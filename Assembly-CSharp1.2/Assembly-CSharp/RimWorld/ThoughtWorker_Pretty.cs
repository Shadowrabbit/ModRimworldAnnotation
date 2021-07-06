using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9E RID: 3742
	public class ThoughtWorker_Pretty : ThoughtWorker
	{
		// Token: 0x0600538B RID: 21387 RVA: 0x001C0FF4 File Offset: 0x001BF1F4
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (RelationsUtility.IsDisfigured(other))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			float statValue = other.GetStatValue(StatDefOf.PawnBeauty, true);
			if (statValue >= 2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (statValue >= 1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}
