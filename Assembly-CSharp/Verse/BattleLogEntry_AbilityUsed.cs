using System;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001BE RID: 446
	public class BattleLogEntry_AbilityUsed : BattleLogEntry_Event
	{
		// Token: 0x06000B47 RID: 2887 RVA: 0x0000EBBB File Offset: 0x0000CDBB
		public BattleLogEntry_AbilityUsed()
		{
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0000EBC3 File Offset: 0x0000CDC3
		public BattleLogEntry_AbilityUsed(Pawn caster, Thing target, AbilityDef ability, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.abilityUsed = ability;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0000EBD6 File Offset: 0x0000CDD6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<AbilityDef>(ref this.abilityUsed, "abilityUsed");
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x000A0710 File Offset: 0x0009E910
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ABILITY", this.abilityUsed));
			if (this.subjectPawn == null && this.subjectThing == null)
			{
				result.Rules.Add(new Rule_String("SUBJECT_definite", "AreaLower".Translate()));
			}
			return result;
		}

		// Token: 0x04000A16 RID: 2582
		public AbilityDef abilityUsed;
	}
}
