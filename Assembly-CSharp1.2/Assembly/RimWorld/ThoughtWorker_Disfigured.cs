using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E94 RID: 3732
	public class ThoughtWorker_Disfigured : ThoughtWorker
	{
		// Token: 0x06005377 RID: 21367 RVA: 0x001C0BA4 File Offset: 0x001BEDA4
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || other.Dead)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!RelationsUtility.IsDisfigured(other))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			return true;
		}
	}
}
