using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000584 RID: 1412
	public class JobDriver_ExtractToInventory : JobDriver
	{
		// Token: 0x06002969 RID: 10601 RVA: 0x000FA68B File Offset: 0x000F888B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000FA6AE File Offset: 0x000F88AE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, this.job.GetTarget(TargetIndex.A).Thing.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_General.WaitWhileExtractingContents(TargetIndex.A, TargetIndex.B, 300);
			yield return Toils_General.Do(delegate
			{
				this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompThingContainer>().innerContainer.TryDropAll(this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, true);
			});
			yield return Toils_Haul.TakeToInventory(TargetIndex.B, this.job.count);
			yield break;
		}

		// Token: 0x040019A6 RID: 6566
		private const TargetIndex ContainerInd = TargetIndex.A;

		// Token: 0x040019A7 RID: 6567
		private const TargetIndex ContentsInd = TargetIndex.B;

		// Token: 0x040019A8 RID: 6568
		public const int OpenTicks = 300;
	}
}
