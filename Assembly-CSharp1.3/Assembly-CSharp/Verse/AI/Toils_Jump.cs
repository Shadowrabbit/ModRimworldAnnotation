using System;

namespace Verse.AI
{
	// Token: 0x020005B4 RID: 1460
	public static class Toils_Jump
	{
		// Token: 0x06002AAF RID: 10927 RVA: 0x00100494 File Offset: 0x000FE694
		public static Toil Jump(Toil jumpTarget)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
			};
			return toil;
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x001004D8 File Offset: 0x000FE6D8
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

		// Token: 0x06002AB1 RID: 10929 RVA: 0x00100524 File Offset: 0x000FE724
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

		// Token: 0x06002AB2 RID: 10930 RVA: 0x00100570 File Offset: 0x000FE770
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

		// Token: 0x06002AB3 RID: 10931 RVA: 0x001005BC File Offset: 0x000FE7BC
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

		// Token: 0x06002AB4 RID: 10932 RVA: 0x00100608 File Offset: 0x000FE808
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

		// Token: 0x06002AB5 RID: 10933 RVA: 0x00100654 File Offset: 0x000FE854
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

		// Token: 0x06002AB6 RID: 10934 RVA: 0x001006A0 File Offset: 0x000FE8A0
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
