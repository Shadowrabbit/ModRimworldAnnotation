using System;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001C5 RID: 453
	public class BattleLogEntry_ItemUsed : BattleLogEntry_Event
	{
		// Token: 0x06000B86 RID: 2950 RVA: 0x0000EBBB File Offset: 0x0000CDBB
		public BattleLogEntry_ItemUsed()
		{
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0000EF33 File Offset: 0x0000D133
		public BattleLogEntry_ItemUsed(Pawn caster, Thing target, ThingDef itemUsed, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.itemUsed = itemUsed;
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0000EF46 File Offset: 0x0000D146
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.itemUsed, "itemUsed");
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x000A0ED8 File Offset: 0x0009F0D8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ITEM", this.itemUsed));
			return result;
		}

		// Token: 0x04000A32 RID: 2610
		public ThingDef itemUsed;
	}
}
