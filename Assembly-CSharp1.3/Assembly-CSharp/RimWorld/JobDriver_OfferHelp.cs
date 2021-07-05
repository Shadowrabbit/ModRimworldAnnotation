using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000728 RID: 1832
	public class JobDriver_OfferHelp : JobDriver
	{
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x060032E1 RID: 13025 RVA: 0x00123D58 File Offset: 0x00121F58
		public Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.pawn.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x00123D83 File Offset: 0x00121F83
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.OtherPawn.mindState.WillJoinColonyIfRescued);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.DoAtomic(delegate
			{
				this.OtherPawn.mindState.JoinColonyBecauseRescuedBy(this.pawn);
			});
			yield break;
		}

		// Token: 0x04001DE1 RID: 7649
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
