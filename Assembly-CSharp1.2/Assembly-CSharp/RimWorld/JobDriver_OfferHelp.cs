using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BE5 RID: 3045
	public class JobDriver_OfferHelp : JobDriver
	{
		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x060047A0 RID: 18336 RVA: 0x001988F4 File Offset: 0x00196AF4
		public Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.pawn.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x000341A7 File Offset: 0x000323A7
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

		// Token: 0x04002FF0 RID: 12272
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
