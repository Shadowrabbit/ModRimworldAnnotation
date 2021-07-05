using System;

namespace Verse.AI
{
	// Token: 0x020009F7 RID: 2551
	public static class Toils_Jump
	{
		// Token: 0x06003D46 RID: 15686 RVA: 0x00174F0C File Offset: 0x0017310C
		public static Toil Jump(Toil jumpTarget)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
			};
			return toil;
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00174F50 File Offset: 0x00173150
		public static Toil JumpIf(Toil jumpTarget, Func<bool> condition)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (condition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
				}
			};
			return toil;
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x00174F9C File Offset: 0x0017319C
		public static Toil JumpIfTargetDespawnedOrNull(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing == null || !thing.Spawned)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x00174FE8 File Offset: 0x001731E8
		public static Toil JumpIfTargetInvalid(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (!toil.actor.jobs.curJob.GetTarget(ind).IsValid)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00175034 File Offset: 0x00173234
		public static Toil JumpIfTargetNotHittable(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (curJob.verbToUse == null || !curJob.verbToUse.IsStillUsableBy(actor) || !curJob.verbToUse.CanHitTarget(target))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00175080 File Offset: 0x00173280
		public static Toil JumpIfTargetDowned(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = actor.jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.Downed)
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x001750CC File Offset: 0x001732CC
		public static Toil JumpIfHaveTargetInQueue(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (!actor.jobs.curJob.GetTargetQueue(ind).NullOrEmpty<LocalTargetInfo>())
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x00175118 File Offset: 0x00173318
		public static Toil JumpIfCannotTouch(TargetIndex ind, PathEndMode peMode, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(ind);
				if (!actor.CanReachImmediate(target, peMode))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}
	}
}
