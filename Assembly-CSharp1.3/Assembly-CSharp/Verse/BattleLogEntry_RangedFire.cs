using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200012C RID: 300
	public class BattleLogEntry_RangedFire : LogEntry
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x00025738 File Offset: 0x00023938
		private string InitiatorName
		{
			get
			{
				if (this.initiatorPawn == null)
				{
					return "null";
				}
				return this.initiatorPawn.LabelShort;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00025753 File Offset: 0x00023953
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn == null)
				{
					return "null";
				}
				return this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00024AB6 File Offset: 0x00022CB6
		public BattleLogEntry_RangedFire() : base(null)
		{
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00025770 File Offset: 0x00023970
		public BattleLogEntry_RangedFire(Thing initiator, Thing target, ThingDef weaponDef, ThingDef projectileDef, bool burst) : base(null)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (target is Pawn)
			{
				this.recipientPawn = (target as Pawn);
			}
			else if (target != null)
			{
				this.recipientThing = target.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.burst = burst;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x000257E5 File Offset: 0x000239E5
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x000257FB File Offset: 0x000239FB
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			yield break;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0002580C File Offset: 0x00023A0C
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipientPawn != null && ((pov == this.initiatorPawn && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn)));
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0002585C File Offset: 0x00023A5C
		public override void ClickedFromPOV(Thing pov)
		{
			if (this.recipientPawn == null)
			{
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn);
				return;
			}
			if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000258AC File Offset: 0x00023AAC
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.initiatorPawn == null && this.initiatorThing == null)
			{
				Log.ErrorOnce("BattleLogEntry_RangedFire has a null initiator.", 60465709);
			}
			if (this.weaponDef != null && this.weaponDef.Verbs[0].rangedFireRulepack != null)
			{
				result.Includes.Add(this.weaponDef.Verbs[0].rangedFireRulepack);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedFire);
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, true, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			else
			{
				result.Constants["INITIATOR_missing"] = "True";
			}
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			else
			{
				result.Constants["RECIPIENT_missing"] = "True";
			}
			result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", this.weaponDef, this.projectileDef));
			if (this.initiatorPawn != null && this.initiatorPawn.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiatorPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			result.Constants["BURST"] = this.burst.ToString();
			return result;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00025AD3 File Offset: 0x00023CD3
		public override bool ShowInCompactView()
		{
			return Rand.ChanceSeeded(BattleLogEntry_RangedFire.DisplayChance, this.logID);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00025AE8 File Offset: 0x00023CE8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Values.Look<bool>(ref this.burst, "burst", false, false);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00025B6F File Offset: 0x00023D6F
		public override string ToString()
		{
			return "BattleLogEntry_RangedFire: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x040007A9 RID: 1961
		private Pawn initiatorPawn;

		// Token: 0x040007AA RID: 1962
		private ThingDef initiatorThing;

		// Token: 0x040007AB RID: 1963
		private Pawn recipientPawn;

		// Token: 0x040007AC RID: 1964
		private ThingDef recipientThing;

		// Token: 0x040007AD RID: 1965
		private ThingDef weaponDef;

		// Token: 0x040007AE RID: 1966
		private ThingDef projectileDef;

		// Token: 0x040007AF RID: 1967
		private bool burst;

		// Token: 0x040007B0 RID: 1968
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChance = 0.25f;
	}
}
