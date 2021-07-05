using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200081E RID: 2078
	public class JobDriver_CreateAndEnterCocoon : JobDriver
	{
		// Token: 0x0600374C RID: 14156 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x001385B8 File Offset: 0x001367B8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				IntVec3 c2;
				CellFinder.TryFindRandomCellNear(this.job.GetTarget(TargetIndex.A).Cell, this.pawn.Map, 4, (IntVec3 c) => c.Standable(this.pawn.Map), out c2, -1);
				this.job.targetB = c2;
			});
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			yield return Toils_General.Wait(200, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_General.Do(delegate
			{
				GenSpawn.Spawn(ThingDefOf.DryadCocoon, this.job.targetB.Cell, this.pawn.Map, WipeMode.Vanish).TryGetComp<CompDryadCocoon>().TryAcceptPawn(this.pawn);
			});
			yield break;
		}

		// Token: 0x04001F0E RID: 7950
		private const int TicksToCreateCocoon = 200;
	}
}
