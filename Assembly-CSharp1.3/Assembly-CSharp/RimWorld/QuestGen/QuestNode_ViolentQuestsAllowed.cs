using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A2 RID: 5794
	public class QuestNode_ViolentQuestsAllowed : QuestNode
	{
		// Token: 0x06008696 RID: 34454 RVA: 0x00303F38 File Offset: 0x00302138
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate, (QuestNode n) => n.TestRun(slate));
		}

		// Token: 0x06008697 RID: 34455 RVA: 0x00303F6A File Offset: 0x0030216A
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate, delegate(QuestNode n)
			{
				n.Run();
				return true;
			});
		}

		// Token: 0x06008698 RID: 34456 RVA: 0x00303F98 File Offset: 0x00302198
		private bool DoWork(Slate slate, Func<QuestNode, bool> func)
		{
			bool allowViolentQuests = Find.Storyteller.difficulty.allowViolentQuests;
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

		// Token: 0x0400546C RID: 21612
		public QuestNode node;

		// Token: 0x0400546D RID: 21613
		public QuestNode elseNode;
	}
}
