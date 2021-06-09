using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CA2 RID: 3234
	public class JobGiver_AITrashDutyFocus : ThinkNode_JobGiver
	{
		// Token: 0x06004B47 RID: 19271 RVA: 0x00035B51 File Offset: 0x00033D51
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			return (JobGiver_AITrashDutyFocus)base.DeepCopy(resolve);
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x001A4F24 File Offset: 0x001A3124
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.duty == null || !pawn.mindState.duty.focus.IsValid)
			{
				return null;
			}
			LocalTargetInfo focus = pawn.mindState.duty.focus;
			if (focus.ThingDestroyed || !pawn.HostileTo(focus.Thing) || !pawn.CanReach(focus, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = TrashUtility.TrashJob_NewTemp(pawn, focus.Thing, false, true);
			if (job != null)
			{
				return job;
			}
			return null;
		}
	}
}
