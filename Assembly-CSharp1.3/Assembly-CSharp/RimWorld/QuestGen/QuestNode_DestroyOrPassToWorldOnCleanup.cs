using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B9 RID: 5817
	public class QuestNode_DestroyOrPassToWorldOnCleanup : QuestNode
	{
		// Token: 0x060086DE RID: 34526 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086DF RID: 34527 RVA: 0x00304CE4 File Offset: 0x00302EE4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_DestroyThingsOrPassToWorldOnCleanup questPart_DestroyThingsOrPassToWorldOnCleanup = new QuestPart_DestroyThingsOrPassToWorldOnCleanup();
			questPart_DestroyThingsOrPassToWorldOnCleanup.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_DestroyThingsOrPassToWorldOnCleanup);
		}

		// Token: 0x040054AC RID: 21676
		public SlateRef<IEnumerable<Thing>> things;
	}
}
