using System;
using RimWorld.Planet;

namespace RimWorld.QuestGen
{
	// Token: 0x020016EF RID: 5871
	public class QuestNode_WorldObjectTimeout : QuestNode_Delay
	{
		// Token: 0x0600878E RID: 34702 RVA: 0x00307C54 File Offset: 0x00305E54
		protected override QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_WorldObjectTimeout
			{
				worldObject = this.worldObject.GetValue(QuestGen.slate)
			};
		}

		// Token: 0x040055AE RID: 21934
		public SlateRef<WorldObject> worldObject;
	}
}
