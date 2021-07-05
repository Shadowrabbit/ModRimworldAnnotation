using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200074F RID: 1871
	public class JobDriver_Feed : JobDriver
	{
		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x060033C7 RID: 13255 RVA: 0x000FE409 File Offset: 0x000FC609
		protected Thing Food
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060033C8 RID: 13256 RVA: 0x001263A4 File Offset: 0x001245A4
		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x001263BB File Offset: 0x001245BB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x001263E2 File Offset: 0x001245E2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			if (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA))
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else if (base.TargetThingA is Building_NutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.ChewIngestible(this.Deliveree, 1.5f, TargetIndex.A, TargetIndex.None).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.Deliveree, TargetIndex.A);
			yield break;
		}

		// Token: 0x04001E16 RID: 7702
		private const TargetIndex IngestableInd = TargetIndex.A;

		// Token: 0x04001E17 RID: 7703
		private const TargetIndex DelivereeInd = TargetIndex.B;

		// Token: 0x04001E18 RID: 7704
		private const float FeedDurationMultiplier = 1.5f;
	}
}
