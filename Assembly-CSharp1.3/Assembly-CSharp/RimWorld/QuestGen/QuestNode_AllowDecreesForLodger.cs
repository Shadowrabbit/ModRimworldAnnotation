using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016AE RID: 5806
	public class QuestNode_AllowDecreesForLodger : QuestNode
	{
		// Token: 0x060086BC RID: 34492 RVA: 0x0030460F File Offset: 0x0030280F
		protected override void RunInt()
		{
			QuestGen.quest.AddPart(new QuestPart_AllowDecreesForLodger
			{
				lodger = this.lodger.GetValue(QuestGen.slate)
			});
		}

		// Token: 0x060086BD RID: 34493 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04005485 RID: 21637
		public SlateRef<Pawn> lodger;
	}
}
