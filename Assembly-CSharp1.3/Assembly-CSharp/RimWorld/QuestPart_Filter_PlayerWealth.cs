using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3C RID: 2876
	public class QuestPart_Filter_PlayerWealth : QuestPart_Filter
	{
		// Token: 0x0600434B RID: 17227 RVA: 0x00167217 File Offset: 0x00165417
		protected override bool Pass(SignalArgs args)
		{
			return WealthUtility.PlayerWealth >= this.minPlayerWealth;
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x00167229 File Offset: 0x00165429
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.minPlayerWealth, "minPlayerWealth", 0f, false);
		}

		// Token: 0x040028EB RID: 10475
		public float minPlayerWealth;
	}
}
