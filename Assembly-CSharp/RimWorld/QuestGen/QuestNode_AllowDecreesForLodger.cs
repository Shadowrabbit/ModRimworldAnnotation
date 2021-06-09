using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F82 RID: 8066
	public class QuestNode_AllowDecreesForLodger : QuestNode
	{
		// Token: 0x0600ABBE RID: 43966 RVA: 0x000706D3 File Offset: 0x0006E8D3
		protected override void RunInt()
		{
			QuestGen.quest.AddPart(new QuestPart_AllowDecreesForLodger
			{
				lodger = this.lodger.GetValue(QuestGen.slate)
			});
		}

		// Token: 0x0600ABBF RID: 43967 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04007508 RID: 29960
		public SlateRef<Pawn> lodger;
	}
}
