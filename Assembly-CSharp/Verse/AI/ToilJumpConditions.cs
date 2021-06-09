using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020009D7 RID: 2519
	public static class ToilJumpConditions
	{
		// Token: 0x06003CE8 RID: 15592 RVA: 0x00173A88 File Offset: 0x00171C88
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

		// Token: 0x06003CE9 RID: 15593 RVA: 0x00173AD0 File Offset: 0x00171CD0
		public static Toil JumpIfDespawnedOrNull(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned;
			}, jumpToil);
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x00173B0C File Offset: 0x00171D0C
		public static Toil JumpIfDespawnedOrNullOrForbidden(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned || thing.IsForbidden(toil.actor);
			}, jumpToil);
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x00173B48 File Offset: 0x00171D48
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
