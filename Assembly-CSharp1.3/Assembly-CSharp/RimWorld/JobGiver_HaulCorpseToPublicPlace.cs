using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F5 RID: 2037
	public class JobGiver_HaulCorpseToPublicPlace : ThinkNode_JobGiver
	{
		// Token: 0x06003686 RID: 13958 RVA: 0x00135284 File Offset: 0x00133484
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_CorpseObsession mentalState_CorpseObsession = pawn.MentalState as MentalState_CorpseObsession;
			if (mentalState_CorpseObsession == null || mentalState_CorpseObsession.corpse == null || mentalState_CorpseObsession.alreadyHauledCorpse)
			{
				return null;
			}
			Corpse corpse = mentalState_CorpseObsession.corpse;
			Building_Grave building_Grave = mentalState_CorpseObsession.corpse.ParentHolder as Building_Grave;
			if (building_Grave != null)
			{
				if (!pawn.CanReserveAndReach(building_Grave, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
				{
					return null;
				}
			}
			else if (!pawn.CanReserveAndReach(corpse, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulCorpseToPublicPlace, corpse, building_Grave);
			job.count = 1;
			return job;
		}
	}
}
