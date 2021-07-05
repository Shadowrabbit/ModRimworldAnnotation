using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8F RID: 2959
	public class QuestPart_PlayerWealth : QuestPartActivable
	{
		// Token: 0x06004534 RID: 17716 RVA: 0x0016EE62 File Offset: 0x0016D062
		public override void QuestPartTick()
		{
			if (Find.TickManager.TicksGame % 60 == 0 && WealthUtility.PlayerWealth >= this.playerWealth)
			{
				base.Complete();
			}
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x0016EE86 File Offset: 0x0016D086
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.playerWealth, "playerWealth", 0f, false);
		}

		// Token: 0x04002A13 RID: 10771
		public float playerWealth = 100000f;

		// Token: 0x04002A14 RID: 10772
		public const int CheckInterval = 60;
	}
}
