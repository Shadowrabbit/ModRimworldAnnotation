using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200078E RID: 1934
	public class JobGiver_ReturnedCaravan_PenAnimals : JobGiver_PrepareCaravan_RopePawns
	{
		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x0600350C RID: 13580 RVA: 0x0012C4AA File Offset: 0x0012A6AA
		protected override JobDef RopeJobDef
		{
			get
			{
				return JobDefOf.ReturnedCaravan_PenAnimals;
			}
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool AnimalNeedsGathering(Pawn pawn, Pawn animal)
		{
			return false;
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x0012C4B1 File Offset: 0x0012A6B1
		protected override void DecorateJob(Job job)
		{
			base.DecorateJob(job);
			job.ropeToUnenclosedPens = true;
		}
	}
}
