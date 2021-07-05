using System;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200012A RID: 298
	public class BattleLogEntry_ItemUsed : BattleLogEntry_Event
	{
		// Token: 0x06000800 RID: 2048 RVA: 0x0002489E File Offset: 0x00022A9E
		public BattleLogEntry_ItemUsed()
		{
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002510E File Offset: 0x0002330E
		public BattleLogEntry_ItemUsed(Pawn caster, Thing target, ThingDef itemUsed, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.itemUsed = itemUsed;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00025121 File Offset: 0x00023321
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.itemUsed, "itemUsed");
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002513C File Offset: 0x0002333C
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ITEM", this.itemUsed));
			return result;
		}

		// Token: 0x0400079E RID: 1950
		public ThingDef itemUsed;
	}
}
