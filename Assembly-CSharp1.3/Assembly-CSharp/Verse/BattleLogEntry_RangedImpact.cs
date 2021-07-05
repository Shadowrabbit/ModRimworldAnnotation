using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200012D RID: 301
	public class BattleLogEntry_RangedImpact : LogEntry_DamageResult
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x00025B98 File Offset: 0x00023D98
		private string InitiatorName
		{
			get
			{
				if (this.initiatorPawn != null)
				{
					return this.initiatorPawn.LabelShort;
				}
				if (this.initiatorThing != null)
				{
					return this.initiatorThing.defName;
				}
				return "null";
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00025BC7 File Offset: 0x00023DC7
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn != null)
				{
					return this.recipientPawn.LabelShort;
				}
				if (this.recipientThing != null)
				{
					return this.recipientThing.defName;
				}
				return "null";
			}
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00024955 File Offset: 0x00022B55
		public BattleLogEntry_RangedImpact() : base(null)
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00025BF8 File Offset: 0x00023DF8
		public BattleLogEntry_RangedImpact(Thing initiator, Thing recipient, Thing originalTarget, ThingDef weaponDef, ThingDef projectileDef, ThingDef coverDef) : base(null)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			if (originalTarget is Pawn)
			{
				this.originalTargetPawn = (originalTarget as Pawn);
				this.originalTargetMobile = (!this.originalTargetPawn.Downed && !this.originalTargetPawn.Dead && this.originalTargetPawn.Awake());
			}
			else if (originalTarget != null)
			{
				this.originalTargetThing = originalTarget.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.coverDef = coverDef;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00025CC1 File Offset: 0x00023EC1
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn || t == this.originalTargetPawn;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00025CE0 File Offset: 0x00023EE0
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
			if (this.originalTargetPawn != null)
			{
				yield return this.originalTargetPawn;
			}
			yield break;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00025CF0 File Offset: 0x00023EF0
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipientPawn != null && ((pov == this.initiatorPawn && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn)));
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00025D40 File Offset: 0x00023F40
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

		// Token: 0x0600082A RID: 2090 RVA: 0x00025D8F File Offset: 0x00023F8F
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				return null;
			}
			if (this.deflected)
			{
				return null;
			}
			if (pov == null || pov == this.recipientPawn)
			{
				return LogEntry.Blood;
			}
			if (pov == this.initiatorPawn)
			{
				return LogEntry.BloodTarget;
			}
			return null;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00025DCC File Offset: 0x00023FCC
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00025DE8 File Offset: 0x00023FE8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn != null || this.recipientThing != null)
			{
				result.Includes.Add(this.deflected ? RulePackDefOf.Combat_RangedDeflect : RulePackDefOf.Combat_RangedDamage);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedMiss);
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
			if (this.originalTargetPawn != this.recipientPawn || this.originalTargetThing != this.recipientThing)
			{
				if (this.originalTargetPawn != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForPawn("ORIGINALTARGET", this.originalTargetPawn, result.Constants, true, true));
					result.Constants["ORIGINALTARGET_mobile"] = this.originalTargetMobile.ToString();
				}
				else if (this.originalTargetThing != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef("ORIGINALTARGET", this.originalTargetThing));
				}
				else
				{
					result.Constants["ORIGINALTARGET_missing"] = "True";
				}
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
			result.Constants["COVER_missing"] = ((this.coverDef != null) ? "False" : "True");
			if (this.coverDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("COVER", this.coverDef));
			}
			return result;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000260B4 File Offset: 0x000242B4
		public override bool ShowInCompactView()
		{
			if (!this.deflected)
			{
				if (this.recipientPawn != null)
				{
					return true;
				}
				if (this.originalTargetThing != null && this.originalTargetThing == this.recipientThing)
				{
					return true;
				}
			}
			int num = 1;
			if (this.weaponDef != null && !this.weaponDef.Verbs.NullOrEmpty<VerbProperties>())
			{
				num = this.weaponDef.Verbs[0].burstShotCount;
			}
			return Rand.ChanceSeeded(BattleLogEntry_RangedImpact.DisplayChanceOnMiss / (float)num, this.logID);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00026134 File Offset: 0x00024334
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_References.Look<Pawn>(ref this.originalTargetPawn, "originalTargetPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.originalTargetThing, "originalTargetThing");
			Scribe_Values.Look<bool>(ref this.originalTargetMobile, "originalTargetMobile", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Defs.Look<ThingDef>(ref this.coverDef, "coverDef");
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000261EC File Offset: 0x000243EC
		public override string ToString()
		{
			return "BattleLogEntry_RangedImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x040007B1 RID: 1969
		private Pawn initiatorPawn;

		// Token: 0x040007B2 RID: 1970
		private ThingDef initiatorThing;

		// Token: 0x040007B3 RID: 1971
		private Pawn recipientPawn;

		// Token: 0x040007B4 RID: 1972
		private ThingDef recipientThing;

		// Token: 0x040007B5 RID: 1973
		private Pawn originalTargetPawn;

		// Token: 0x040007B6 RID: 1974
		private ThingDef originalTargetThing;

		// Token: 0x040007B7 RID: 1975
		private bool originalTargetMobile;

		// Token: 0x040007B8 RID: 1976
		private ThingDef weaponDef;

		// Token: 0x040007B9 RID: 1977
		private ThingDef projectileDef;

		// Token: 0x040007BA RID: 1978
		private ThingDef coverDef;

		// Token: 0x040007BB RID: 1979
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.25f;
	}
}
