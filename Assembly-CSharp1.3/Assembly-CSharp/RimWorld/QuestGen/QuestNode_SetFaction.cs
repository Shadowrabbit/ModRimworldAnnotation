using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E2 RID: 5858
	public class QuestNode_SetFaction : QuestNode
	{
		// Token: 0x06008764 RID: 34660 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008765 RID: 34661 RVA: 0x0030712C File Offset: 0x0030532C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_SetFaction questPart_SetFaction = new QuestPart_SetFaction();
			questPart_SetFaction.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetFaction.faction = this.faction.GetValue(slate);
			questPart_SetFaction.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_SetFaction);
		}

		// Token: 0x04005578 RID: 21880
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005579 RID: 21881
		public SlateRef<Faction> faction;

		// Token: 0x0400557A RID: 21882
		public SlateRef<IEnumerable<Thing>> things;
	}
}
