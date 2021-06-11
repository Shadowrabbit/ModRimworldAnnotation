using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E3F RID: 3647
	public class ThinkNode_Duty : ThinkNode
	{
		// Token: 0x060052AB RID: 21163 RVA: 0x001BF624 File Offset: 0x001BD824
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (pawn.GetLord() == null)
			{
				Log.Error(pawn + " doing ThinkNode_Duty with no Lord.", false);
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.duty == null)
			{
				Log.Error(pawn + " doing ThinkNode_Duty with no duty.", false);
				return ThinkResult.NoJob;
			}
			return subNodes[pawn.mindState.duty.def.index].TryIssueJobPackage(pawn, jobParams);
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x001BF69C File Offset: 0x001BD89C
		protected override void ResolveSubnodes()
		{
			foreach (DutyDef dutyDef in DefDatabase<DutyDef>.AllDefs)
			{
				//处理每一个职责定义的节点和子节点
				dutyDef.thinkNode.ResolveSubnodesAndRecur();
				//将所有职责中的节点设置为当前节点的子节点
				this.subNodes.Add(dutyDef.thinkNode.DeepCopy());
			}
		}
	}
}
