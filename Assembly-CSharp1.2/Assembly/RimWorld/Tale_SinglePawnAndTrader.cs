using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200165A RID: 5722
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		// Token: 0x06007CA4 RID: 31908 RVA: 0x00053A70 File Offset: 0x00051C70
		public Tale_SinglePawnAndTrader()
		{
		}

		// Token: 0x06007CA5 RID: 31909 RVA: 0x00053BFA File Offset: 0x00051DFA
		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06007CA6 RID: 31910 RVA: 0x00053C0F File Offset: 0x00051E0F
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06007CA7 RID: 31911 RVA: 0x00053C2F File Offset: 0x00051E2F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x06007CA8 RID: 31912 RVA: 0x00053C4C File Offset: 0x00051E4C
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

		// Token: 0x06007CA9 RID: 31913 RVA: 0x00053C5C File Offset: 0x00051E5C
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x0400517C RID: 20860
		public TaleData_Trader traderData;
	}
}
