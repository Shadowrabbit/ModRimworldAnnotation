using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E1 RID: 5857
	public class QuestNode_SetChallengeRating : QuestNode
	{
		// Token: 0x06008761 RID: 34657 RVA: 0x0030710E File Offset: 0x0030530E
		protected override void RunInt()
		{
			QuestGen.quest.challengeRating = this.challengeRating.GetValue(QuestGen.slate);
		}

		// Token: 0x06008762 RID: 34658 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04005577 RID: 21879
		public SlateRef<int> challengeRating;
	}
}
