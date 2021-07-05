using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005AE RID: 1454
	public static class ToilJumpConditions
	{
		// Token: 0x06002A8E RID: 10894 RVA: 0x000FFC18 File Offset: 0x000FDE18
		public static Toil JumpIf(this Toil toil, Func<bool> jumpCondition, Toil jumpToil)
		{
			toil.AddPreTickAction(delegate
			{
				if (jumpCondition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
					return;
				}
			});
			return toil;
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000FFC60 File Offset: 0x000FDE60
		public static Toil JumpIfDespawnedOrNull(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned;
			}, jumpToil);
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000FFC9C File Offset: 0x000FDE9C
		public static Toil JumpIfDespawnedOrNullOrForbidden(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned || thing.IsForbidden(toil.actor);
			}, jumpToil);
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x000FFCD8 File Offset: 0x000FDED8
		public static Toil JumpIfOutsideHomeArea(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return !toil.actor.Map.areaManager.Home[thing.Position];
			}, jumpToil);
		}
	}
}
