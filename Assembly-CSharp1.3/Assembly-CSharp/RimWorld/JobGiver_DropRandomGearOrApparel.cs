using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F3 RID: 2035
	public class JobGiver_DropRandomGearOrApparel : ThinkNode_JobGiver
	{
		// Token: 0x0600367F RID: 13951 RVA: 0x00135010 File Offset: 0x00133210
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
