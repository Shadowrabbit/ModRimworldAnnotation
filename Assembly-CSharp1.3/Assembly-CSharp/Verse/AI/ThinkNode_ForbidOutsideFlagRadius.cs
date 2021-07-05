using System;

namespace Verse.AI
{
	// Token: 0x02000627 RID: 1575
	public class ThinkNode_ForbidOutsideFlagRadius : ThinkNode_Priority
	{
		// Token: 0x06002D3D RID: 11581 RVA: 0x0010F4FE File Offset: 0x0010D6FE
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ForbidOutsideFlagRadius thinkNode_ForbidOutsideFlagRadius = (ThinkNode_ForbidOutsideFlagRadius)base.DeepCopy(resolve);
			thinkNode_ForbidOutsideFlagRadius.maxDistToSquadFlag = this.maxDistToSquadFlag;
			return thinkNode_ForbidOutsideFlagRadius;
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x0010F518 File Offset: 0x0010D718
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result;
			try
			{
				if (this.maxDistToSquadFlag > 0f)
				{
					if (pawn.mindState.maxDistToSquadFlag > 0f)
					{
						Log.Error("Squad flag was not reset properly; raiders may behave strangely");
					}
					pawn.mindState.maxDistToSquadFlag = this.maxDistToSquadFlag;
				}
				result = base.TryIssueJobPackage(pawn, jobParams);
			}
			finally
			{
				pawn.mindState.maxDistToSquadFlag = -1f;
			}
			return result;
		}

		// Token: 0x04001BB4 RID: 7092
		public float maxDistToSquadFlag = -1f;
	}
}
