using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006BD RID: 1725
	public class JobDriver_EmptyThingContainer : JobDriver
	{
		// Token: 0x06003016 RID: 12310 RVA: 0x0011CE80 File Offset: 0x0011B080
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x0011CED3 File Offset: 0x0011B0D3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, this.job.GetTarget(TargetIndex.A).Thing.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_General.WaitWhileExtractingContents(TargetIndex.A, TargetIndex.B, 120);
			yield return Toils_General.Do(delegate
			{
				this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompThingContainer>().innerContainer.TryDropAll(this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, true);
			});
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true, false);
			yield break;
		}

		// Token: 0x04001D29 RID: 7465
		private const TargetIndex ContainerInd = TargetIndex.A;

		// Token: 0x04001D2A RID: 7466
		private const TargetIndex ContentsInd = TargetIndex.B;

		// Token: 0x04001D2B RID: 7467
		private const TargetIndex StoreCellInd = TargetIndex.C;

		// Token: 0x04001D2C RID: 7468
		private const int OpenTicks = 120;
	}
}
