using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000848 RID: 2120
	public class WorkGiver_HelpGatheringItemsForCaravan : WorkGiver
	{
		// Token: 0x0600381B RID: 14363 RVA: 0x0013BF90 File Offset: 0x0013A190
		public override Job NonScanJob(Pawn pawn)
		{
			List<Lord> lords = pawn.Map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lords[i].LordJob as LordJob_FormAndSendCaravan;
				if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.GatheringItemsNow)
				{
					Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lords[i]);
					if (thing != null && this.AnyReachableCarrierOrColonist(pawn, lords[i]))
					{
						Job job = JobMaker.MakeJob(JobDefOf.PrepareCaravan_GatherItems, thing);
						job.lord = lords[i];
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x0013C01C File Offset: 0x0013A21C
		private bool AnyReachableCarrierOrColonist(Pawn forPawn, Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(lord.ownedPawns[i], forPawn, false) && !lord.ownedPawns[i].IsForbidden(forPawn) && forPawn.CanReach(lord.ownedPawns[i], PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
