using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001641 RID: 5697
	public class QuestNode_SendSignals : QuestNode
	{
		// Token: 0x06008529 RID: 34089 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600852A RID: 34090 RVA: 0x002FD3D4 File Offset: 0x002FB5D4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<string> enumerable = Enumerable.Empty<string>();
			if (this.outSignals.GetValue(slate) != null)
			{
				enumerable = enumerable.Concat(this.outSignals.GetValue(slate));
			}
			if (this.outSignalsFormattedCount.GetValue(slate) > 0)
			{
				for (int i = 0; i < this.outSignalsFormattedCount.GetValue(slate); i++)
				{
					enumerable = enumerable.Concat(Gen.YieldSingle<string>(this.outSignalsFormat.GetValue(slate).Formatted(i.Named("INDEX")).ToString()));
				}
			}
			if (enumerable.EnumerableNullOrEmpty<string>())
			{
				return;
			}
			if (enumerable.Count<string>() == 1)
			{
				QuestPart_Pass questPart_Pass = new QuestPart_Pass();
				questPart_Pass.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				questPart_Pass.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(enumerable.First<string>());
				QuestGen.quest.AddPart(questPart_Pass);
				return;
			}
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			foreach (string signal in enumerable)
			{
				questPart_PassOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			QuestGen.quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x040052F4 RID: 21236
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x040052F5 RID: 21237
		[NoTranslate]
		public SlateRef<string> outSignalsFormat;

		// Token: 0x040052F6 RID: 21238
		public SlateRef<int> outSignalsFormattedCount;
	}
}
