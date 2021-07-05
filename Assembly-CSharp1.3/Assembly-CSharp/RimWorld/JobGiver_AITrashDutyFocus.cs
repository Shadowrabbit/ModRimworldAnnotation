using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000786 RID: 1926
	public class JobGiver_AITrashDutyFocus : ThinkNode_JobGiver
	{
		// Token: 0x060034ED RID: 13549 RVA: 0x0012BCA0 File Offset: 0x00129EA0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			return (JobGiver_AITrashDutyFocus)base.DeepCopy(resolve);
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x0012BCB0 File Offset: 0x00129EB0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.duty == null || !pawn.mindState.duty.focus.IsValid)
			{
				return null;
			}
			LocalTargetInfo focus = pawn.mindState.duty.focus;
			if (focus.ThingDestroyed || !pawn.HostileTo(focus.Thing) || !pawn.CanReach(focus, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = TrashUtility.TrashJob(pawn, focus.Thing, false, true);
			if (job != null)
			{
				return job;
			}
			return null;
		}
	}
}
