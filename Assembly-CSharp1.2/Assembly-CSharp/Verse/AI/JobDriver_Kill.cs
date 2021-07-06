using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000984 RID: 2436
	public class JobDriver_Kill : JobDriver
	{
		// Token: 0x06003B97 RID: 15255 RVA: 0x0002DA01 File Offset: 0x0002BC01
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x0002DA24 File Offset: 0x0002BC24
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Succeeded);
			yield return Toils_Combat.TrySetJobToUseAttackVerb(TargetIndex.A);
			Toil gotoCastPos = Toils_Combat.GotoCastPosition(TargetIndex.A, false, 0.95f);
			yield return gotoCastPos;
			Toil jumpIfCannotHit = Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, gotoCastPos);
			yield return jumpIfCannotHit;
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield return Toils_Jump.Jump(jumpIfCannotHit);
			yield break;
		}

		// Token: 0x0400294B RID: 10571
		private const TargetIndex VictimInd = TargetIndex.A;
	}
}
