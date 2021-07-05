using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008F6 RID: 2294
	public class ThinkNode_Duty : ThinkNode
	{
		// Token: 0x06003C17 RID: 15383 RVA: 0x0014ECE8 File Offset: 0x0014CEE8
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (pawn.GetLord() == null)
			{
				Log.Error(pawn + " doing ThinkNode_Duty with no Lord.");
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.duty == null)
			{
				Log.Error(pawn + " doing ThinkNode_Duty with no duty.");
				return ThinkResult.NoJob;
			}
			return this.subNodes[(int)pawn.mindState.duty.def.index].TryIssueJobPackage(pawn, jobParams);
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x0014ED60 File Offset: 0x0014CF60
		protected override void ResolveSubnodes()
		{
			foreach (DutyDef dutyDef in DefDatabase<DutyDef>.AllDefs)
			{
				dutyDef.thinkNode.ResolveSubnodesAndRecur();
				this.subNodes.Add(dutyDef.thinkNode.DeepCopy(true));
			}
		}
	}
}
