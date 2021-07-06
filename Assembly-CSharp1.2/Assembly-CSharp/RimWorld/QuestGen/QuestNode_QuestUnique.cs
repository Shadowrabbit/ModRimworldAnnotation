using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FAF RID: 8111
	public class QuestNode_QuestUnique : QuestNode
	{
		// Token: 0x0600AC4C RID: 44108 RVA: 0x000708DD File Offset: 0x0006EADD
		public static string GetProcessedTag(string tag, Faction faction)
		{
			if (faction == null)
			{
				return tag;
			}
			return tag + "_" + faction.Name;
		}

		// Token: 0x0600AC4D RID: 44109 RVA: 0x000708F5 File Offset: 0x0006EAF5
		private string GetProcessedTag(Slate slate)
		{
			return QuestNode_QuestUnique.GetProcessedTag(this.tag.GetValue(slate), this.faction.GetValue(slate));
		}

		// Token: 0x0600AC4E RID: 44110 RVA: 0x00321D54 File Offset: 0x0031FF54
		protected override void RunInt()
		{
			string processedTag = this.GetProcessedTag(QuestGen.slate);
			QuestUtility.AddQuestTag(ref QuestGen.quest.tags, processedTag);
			if (this.storeProcessedTagAs.GetValue(QuestGen.slate) != null)
			{
				QuestGen.slate.Set<string>(this.storeProcessedTagAs.GetValue(QuestGen.slate), processedTag, false);
			}
		}

		// Token: 0x0600AC4F RID: 44111 RVA: 0x00321DAC File Offset: 0x0031FFAC
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

		// Token: 0x040075C9 RID: 30153
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x040075CA RID: 30154
		public SlateRef<Faction> faction;

		// Token: 0x040075CB RID: 30155
		[NoTranslate]
		public SlateRef<string> storeProcessedTagAs;
	}
}
