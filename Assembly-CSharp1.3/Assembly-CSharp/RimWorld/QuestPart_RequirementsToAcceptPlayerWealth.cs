using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B9E RID: 2974
	public class QuestPart_RequirementsToAcceptPlayerWealth : QuestPart_RequirementsToAccept
	{
		// Token: 0x06004577 RID: 17783 RVA: 0x00170670 File Offset: 0x0016E870
		public override AcceptanceReport CanAccept()
		{
			if (WealthUtility.PlayerWealth < this.requiredPlayerWealth)
			{
				return new AcceptanceReport("QuestRequiredPlayerWealth".Translate(this.requiredPlayerWealth.ToStringMoney(null)));
			}
			return true;
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x001706AB File Offset: 0x0016E8AB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.requiredPlayerWealth, "requiredPlayerWealth", 0f, false);
		}

		// Token: 0x04002A4F RID: 10831
		public float requiredPlayerWealth = -1f;
	}
}
