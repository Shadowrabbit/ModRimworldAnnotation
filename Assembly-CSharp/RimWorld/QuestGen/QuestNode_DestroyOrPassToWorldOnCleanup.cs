using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F8C RID: 8076
	public class QuestNode_DestroyOrPassToWorldOnCleanup : QuestNode
	{
		// Token: 0x0600ABDD RID: 43997 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABDE RID: 43998 RVA: 0x003204A0 File Offset: 0x0031E6A0
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

		// Token: 0x0400752A RID: 29994
		public SlateRef<IEnumerable<Thing>> things;
	}
}
