using System;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000126 RID: 294
	public class BattleLogEntry_AbilityUsed : BattleLogEntry_Event
	{
		// Token: 0x060007D9 RID: 2009 RVA: 0x0002489E File Offset: 0x00022A9E
		public BattleLogEntry_AbilityUsed()
		{
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000248A6 File Offset: 0x00022AA6
		public BattleLogEntry_AbilityUsed(Pawn caster, Thing target, AbilityDef ability, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.abilityUsed = ability;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x000248B9 File Offset: 0x00022AB9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<AbilityDef>(ref this.abilityUsed, "abilityUsed");
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x000248D4 File Offset: 0x00022AD4
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

		// Token: 0x0400078E RID: 1934
		public AbilityDef abilityUsed;
	}
}
