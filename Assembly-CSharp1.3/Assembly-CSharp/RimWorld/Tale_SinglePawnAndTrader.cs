using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001038 RID: 4152
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		// Token: 0x06006211 RID: 25105 RVA: 0x0021494A File Offset: 0x00212B4A
		public Tale_SinglePawnAndTrader()
		{
		}

		// Token: 0x06006212 RID: 25106 RVA: 0x00214A57 File Offset: 0x00212C57
		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06006213 RID: 25107 RVA: 0x00214A6C File Offset: 0x00212C6C
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06006214 RID: 25108 RVA: 0x00214A8C File Offset: 0x00212C8C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x06006215 RID: 25109 RVA: 0x00214AA9 File Offset: 0x00212CA9
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			foreach (Rule rule in base.SpecialTextGenerationRules(outConstants))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.traderData.GetRules("TRADER", null))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006216 RID: 25110 RVA: 0x00214AC0 File Offset: 0x00212CC0
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x040037D5 RID: 14293
		public TaleData_Trader traderData;
	}
}
