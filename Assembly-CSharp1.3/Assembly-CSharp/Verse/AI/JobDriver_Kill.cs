using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200059C RID: 1436
	public class JobDriver_Kill : JobDriver
	{
		// Token: 0x060029E7 RID: 10727 RVA: 0x000FA68B File Offset: 0x000F888B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000FD0BB File Offset: 0x000FB2BB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Succeeded);
			yield return Toils_Combat.TrySetJobToUseAttackVerb(TargetIndex.A);
			Toil gotoCastPos = Toils_Combat.GotoCastPosition(TargetIndex.A, TargetIndex.None, false, 0.95f);
			yield return gotoCastPos;
			Toil jumpIfCannotHit = Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, gotoCastPos);
			yield return jumpIfCannotHit;
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield return Toils_Jump.Jump(jumpIfCannotHit);
			yield break;
		}

		// Token: 0x04001A17 RID: 6679
		private const TargetIndex VictimInd = TargetIndex.A;
	}
}
