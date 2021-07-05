using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001712 RID: 5906
	public class QuestNode_Filter_AnyColonistAlive : QuestNode_Filter
	{
		// Token: 0x0600885F RID: 34911 RVA: 0x003100DC File Offset: 0x0030E2DC
		protected override QuestPart_Filter MakeFilterQuestPart()
		{
			Slate slate = QuestGen.slate;
			Map map = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
			return new QuestPart_Filter_AnyColonistAlive
			{
				mapParent = map.Parent
			};
		}

		// Token: 0x04005649 RID: 22089
		public SlateRef<Map> map;
	}
}
