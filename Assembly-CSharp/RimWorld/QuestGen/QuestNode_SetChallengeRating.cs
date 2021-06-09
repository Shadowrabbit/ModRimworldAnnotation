using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FB9 RID: 8121
	public class QuestNode_SetChallengeRating : QuestNode
	{
		// Token: 0x0600AC6D RID: 44141 RVA: 0x00070951 File Offset: 0x0006EB51
		protected override void RunInt()
		{
			QuestGen.quest.challengeRating = this.challengeRating.GetValue(QuestGen.slate);
		}

		// Token: 0x0600AC6E RID: 44142 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x040075E3 RID: 30179
		public SlateRef<int> challengeRating;
	}
}
