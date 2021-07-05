using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000821 RID: 2081
	public class JobDriver_ReturnToGauranlenTree : JobDriver
	{
		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06003756 RID: 14166 RVA: 0x00138850 File Offset: 0x00136A50
		private CompTreeConnection TreeComp
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x00138867 File Offset: 0x00136A67
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.TreeComp.ShouldReturnToTree(this.pawn));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false).WithEffect(EffecterDefOf.GauranlenLeaves, TargetIndex.A, null).PlaySustainerOrSound(SoundDefOf.Interact_Sow, 1f);
			yield return Toils_General.Do(delegate
			{
				this.TreeComp.RemoveDryad(this.pawn);
				this.pawn.DeSpawn(DestroyMode.Vanish);
				Find.WorldPawns.PassToWorld(this.pawn, PawnDiscardDecideMode.Discard);
			});
			yield break;
		}

		// Token: 0x04001F10 RID: 7952
		private const int WaitTicks = 180;
	}
}
