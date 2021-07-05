using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001033 RID: 4147
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		// Token: 0x060061F2 RID: 25074 RVA: 0x00214719 File Offset: 0x00212919
		public Tale_DoublePawnAndTrader()
		{
		}

		// Token: 0x060061F3 RID: 25075 RVA: 0x002147AB File Offset: 0x002129AB
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x060061F4 RID: 25076 RVA: 0x002147C1 File Offset: 0x002129C1
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x060061F5 RID: 25077 RVA: 0x002147E1 File Offset: 0x002129E1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x060061F6 RID: 25078 RVA: 0x002147FE File Offset: 0x002129FE
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

		// Token: 0x060061F7 RID: 25079 RVA: 0x00214815 File Offset: 0x00212A15
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x040037D1 RID: 14289
		public TaleData_Trader traderData;
	}
}
