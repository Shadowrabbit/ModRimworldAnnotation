using System;

namespace Verse.AI
{
	// Token: 0x020005B7 RID: 1463
	public static class Toils_ReserveAttackTarget
	{
		// Token: 0x06002AC0 RID: 10944 RVA: 0x001009FC File Offset: 0x000FEBFC
		public static Toil TryReserve(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				IAttackTarget attackTarget = actor.CurJob.GetTarget(ind).Thing as IAttackTarget;
				if (attackTarget != null)
				{
					actor.Map.attackTargetReservationManager.Reserve(actor, toil.actor.CurJob, attackTarget);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}
	}
}
