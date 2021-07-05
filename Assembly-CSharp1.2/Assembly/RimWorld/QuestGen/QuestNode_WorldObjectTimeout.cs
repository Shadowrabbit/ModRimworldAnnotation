using System;
using RimWorld.Planet;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FCA RID: 8138
	public class QuestNode_WorldObjectTimeout : QuestNode_Delay
	{
		// Token: 0x0600ACAB RID: 44203 RVA: 0x00070AC2 File Offset: 0x0006ECC2
		protected override QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_WorldObjectTimeout
			{
				worldObject = this.worldObject.GetValue(QuestGen.slate)
			};
		}

		// Token: 0x0400762B RID: 30251
		public SlateRef<WorldObject> worldObject;
	}
}
