using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001651 RID: 5713
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		// Token: 0x06007C5D RID: 31837 RVA: 0x00053783 File Offset: 0x00051983
		public Tale_DoublePawnAndTrader()
		{
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x00053853 File Offset: 0x00051A53
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06007C5F RID: 31839 RVA: 0x00053869 File Offset: 0x00051A69
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06007C60 RID: 31840 RVA: 0x00053889 File Offset: 0x00051A89
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x06007C61 RID: 31841 RVA: 0x000538A6 File Offset: 0x00051AA6
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule rule in base.SpecialTextGenerationRules())
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.traderData.GetRules("TRADER"))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007C62 RID: 31842 RVA: 0x000538B6 File Offset: 0x00051AB6
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x04005164 RID: 20836
		public TaleData_Trader traderData;
	}
}
