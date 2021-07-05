using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000933 RID: 2355
	public class ThinkNode_TraitBehaviors : ThinkNode
	{
		// Token: 0x06003C9F RID: 15519 RVA: 0x0014FD74 File Offset: 0x0014DF74
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			List<Trait> allTraits = pawn.story.traits.allTraits;
			for (int i = 0; i < allTraits.Count; i++)
			{
				ThinkTreeDef thinkTree = allTraits[i].CurrentData.thinkTree;
				if (thinkTree != null)
				{
					return thinkTree.thinkRoot.TryIssueJobPackage(pawn, jobParams);
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
