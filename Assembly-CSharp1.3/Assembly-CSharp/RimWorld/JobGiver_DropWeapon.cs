using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000797 RID: 1943
	public class JobGiver_DropWeapon : ThinkNode_JobGiver
	{
		// Token: 0x06003528 RID: 13608 RVA: 0x0012CD34 File Offset: 0x0012AF34
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment != null)
			{
				foreach (ThingWithComps thingWithComps in pawn.equipment.AllEquipmentListForReading)
				{
					if (thingWithComps.def.IsWeapon)
					{
						return JobMaker.MakeJob(JobDefOf.DropEquipment, thingWithComps);
					}
				}
			}
			return null;
		}
	}
}
