using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D26 RID: 3366
	public class JobGiver_DropRandomGearOrApparel : ThinkNode_JobGiver
	{
		// Token: 0x06004D22 RID: 19746 RVA: 0x001AD3B0 File Offset: 0x001AB5B0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment != null && pawn.equipment.HasAnything())
			{
				return JobMaker.MakeJob(JobDefOf.DropEquipment, pawn.equipment.AllEquipmentListForReading.RandomElement<ThingWithComps>());
			}
			if (pawn.apparel != null && pawn.apparel.WornApparel.Any<Apparel>())
			{
				return JobMaker.MakeJob(JobDefOf.RemoveApparel, pawn.apparel.WornApparel.RandomElement<Apparel>());
			}
			return null;
		}
	}
}
