using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D7 RID: 5847
	public class QuestNode_QuestUnique : QuestNode
	{
		// Token: 0x06008741 RID: 34625 RVA: 0x00306BAB File Offset: 0x00304DAB
		public static string GetProcessedTag(string tag, Faction faction)
		{
			if (faction == null)
			{
				return tag;
			}
			return tag + "_" + faction.Name;
		}

		// Token: 0x06008742 RID: 34626 RVA: 0x00306BC3 File Offset: 0x00304DC3
		private string GetProcessedTag(Slate slate)
		{
			return QuestNode_QuestUnique.GetProcessedTag(this.tag.GetValue(slate), this.faction.GetValue(slate));
		}

		// Token: 0x06008743 RID: 34627 RVA: 0x00306BE4 File Offset: 0x00304DE4
		protected override void RunInt()
		{
			string processedTag = this.GetProcessedTag(QuestGen.slate);
			QuestUtility.AddQuestTag(ref QuestGen.quest.tags, processedTag);
			if (this.storeProcessedTagAs.GetValue(QuestGen.slate) != null)
			{
				QuestGen.slate.Set<string>(this.storeProcessedTagAs.GetValue(QuestGen.slate), processedTag, false);
			}
		}

		// Token: 0x06008744 RID: 34628 RVA: 0x00306C3C File Offset: 0x00304E3C
		protected override bool TestRunInt(Slate slate)
		{
			string processedTag = this.GetProcessedTag(slate);
			if (this.storeProcessedTagAs.GetValue(slate) != null)
			{
				slate.Set<string>(this.storeProcessedTagAs.GetValue(slate), processedTag, false);
			}
			foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
			{
				if (quest.State == QuestState.Ongoing && quest.tags.Contains(processedTag))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400555E RID: 21854
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400555F RID: 21855
		public SlateRef<Faction> faction;

		// Token: 0x04005560 RID: 21856
		[NoTranslate]
		public SlateRef<string> storeProcessedTagAs;
	}
}
