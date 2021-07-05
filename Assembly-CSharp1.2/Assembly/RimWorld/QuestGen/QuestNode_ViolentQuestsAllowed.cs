using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F74 RID: 8052
	public class QuestNode_ViolentQuestsAllowed : QuestNode
	{
		// Token: 0x0600AB93 RID: 43923 RVA: 0x0031F8B8 File Offset: 0x0031DAB8
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate, (QuestNode n) => n.TestRun(slate));
		}

		// Token: 0x0600AB94 RID: 43924 RVA: 0x00070630 File Offset: 0x0006E830
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate, delegate(QuestNode n)
			{
				n.Run();
				return true;
			});
		}

		// Token: 0x0600AB95 RID: 43925 RVA: 0x0031F8EC File Offset: 0x0031DAEC
		private bool DoWork(Slate slate, Func<QuestNode, bool> func)
		{
			bool allowViolentQuests = Find.Storyteller.difficultyValues.allowViolentQuests;
			slate.Set<bool>("allowViolentQuests", allowViolentQuests, false);
			if (allowViolentQuests)
			{
				if (this.node != null)
				{
					return func(this.node);
				}
			}
			else if (this.elseNode != null)
			{
				return func(this.elseNode);
			}
			return true;
		}

		// Token: 0x040074EC RID: 29932
		public QuestNode node;

		// Token: 0x040074ED RID: 29933
		public QuestNode elseNode;
	}
}
