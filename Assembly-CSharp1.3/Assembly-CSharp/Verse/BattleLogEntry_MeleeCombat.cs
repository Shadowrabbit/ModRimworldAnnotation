using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200012B RID: 299
	public class BattleLogEntry_MeleeCombat : LogEntry_DamageResult
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0002516D File Offset: 0x0002336D
		private string InitiatorName
		{
			get
			{
				if (this.initiator == null)
				{
					return "null";
				}
				return this.initiator.LabelShort;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00025188 File Offset: 0x00023388
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

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x000251A3 File Offset: 0x000233A3
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x000251AB File Offset: 0x000233AB
		public RulePackDef RuleDef
		{
			get
			{
				return this.ruleDef;
			}
			set
			{
				this.ruleDef = value;
				base.ResetCache();
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00024955 File Offset: 0x00022B55
		public BattleLogEntry_MeleeCombat() : base(null)
		{
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x000251BC File Offset: 0x000233BC
		public BattleLogEntry_MeleeCombat(RulePackDef ruleDef, bool alwaysShowInCompact, Pawn initiator, Thing recipient, ImplementOwnerTypeDef implementType, string toolLabel, ThingDef ownerEquipmentDef = null, HediffDef ownerHediffDef = null, LogEntryDef def = null) : base(def)
		{
			this.ruleDef = ruleDef;
			this.alwaysShowInCompact = alwaysShowInCompact;
			this.initiator = initiator;
			this.implementType = implementType;
			this.ownerEquipmentDef = ownerEquipmentDef;
			this.ownerHediffDef = ownerHediffDef;
			this.toolLabel = toolLabel;
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			if (ownerEquipmentDef != null && ownerHediffDef != null)
			{
				Log.ErrorOnce(string.Format("Combat log owned by both equipment {0} and hediff {1}, may produce unexpected results", ownerEquipmentDef.label, ownerHediffDef.label), 96474669);
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00025259 File Offset: 0x00023459
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipientPawn;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0002526F File Offset: 0x0002346F
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			yield break;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00025280 File Offset: 0x00023480
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.initiator && this.recipientPawn != null && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiator));
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000252D0 File Offset: 0x000234D0
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator && this.recipientPawn != null)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn);
				return;
			}
			if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator);
				return;
			}
			if (this.recipientPawn != null)
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00025328 File Offset: 0x00023528
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				return this.def.iconMissTex;
			}
			if (this.deflected)
			{
				return this.def.iconMissTex;
			}
			if (pov == null || pov == this.recipientPawn)
			{
				return this.def.iconDamagedTex;
			}
			if (pov == this.initiator)
			{
				return this.def.iconDamagedFromInstigatorTex;
			}
			return this.def.iconDamagedTex;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0002539A File Offset: 0x0002359A
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x000253B8 File Offset: 0x000235B8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants, true, true));
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			result.Includes.Add(this.ruleDef);
			if (!this.toolLabel.NullOrEmpty())
			{
				result.Rules.Add(new Rule_String("TOOL_label", this.toolLabel));
				result.Rules.Add(new Rule_String("TOOL_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.toolLabel, false, false)));
				result.Rules.Add(new Rule_String("TOOL_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.toolLabel, false, false)));
				result.Constants["TOOL_gender"] = LanguageDatabase.activeLanguage.ResolveGender(this.toolLabel, null).ToString();
			}
			if (this.implementType != null && !this.implementType.implementOwnerRuleName.NullOrEmpty())
			{
				if (this.ownerEquipmentDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerEquipmentDef));
				}
				else if (this.ownerHediffDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerHediffDef));
				}
			}
			if (this.initiator != null && this.initiator.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiator.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
			}
			if (this.implementType != null && !this.implementType.implementOwnerTypeValue.NullOrEmpty())
			{
				result.Constants["IMPLEMENTOWNER_type"] = this.implementType.implementOwnerTypeValue;
			}
			return result;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00025633 File Offset: 0x00023833
		public override bool ShowInCompactView()
		{
			return this.alwaysShowInCompact || Rand.ChanceSeeded(BattleLogEntry_MeleeCombat.DisplayChanceOnMiss, this.logID);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00025650 File Offset: 0x00023850
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
			Scribe_Values.Look<bool>(ref this.alwaysShowInCompact, "alwaysShowInCompact", false, false);
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ImplementOwnerTypeDef>(ref this.implementType, "implementType");
			Scribe_Defs.Look<ThingDef>(ref this.ownerEquipmentDef, "ownerDef");
			Scribe_Values.Look<string>(ref this.toolLabel, "toolLabel", null, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000256EF File Offset: 0x000238EF
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.ruleDef.defName,
				": ",
				this.InitiatorName,
				"->",
				this.RecipientName
			});
		}

		// Token: 0x0400079F RID: 1951
		private RulePackDef ruleDef;

		// Token: 0x040007A0 RID: 1952
		private Pawn initiator;

		// Token: 0x040007A1 RID: 1953
		private Pawn recipientPawn;

		// Token: 0x040007A2 RID: 1954
		private ThingDef recipientThing;

		// Token: 0x040007A3 RID: 1955
		private ImplementOwnerTypeDef implementType;

		// Token: 0x040007A4 RID: 1956
		private ThingDef ownerEquipmentDef;

		// Token: 0x040007A5 RID: 1957
		private HediffDef ownerHediffDef;

		// Token: 0x040007A6 RID: 1958
		private string toolLabel;

		// Token: 0x040007A7 RID: 1959
		public bool alwaysShowInCompact;

		// Token: 0x040007A8 RID: 1960
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.5f;
	}
}
