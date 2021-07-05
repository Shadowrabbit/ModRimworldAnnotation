using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000BE1 RID: 3041
	public class GameCondition_PsychicEmanation : GameCondition
	{
		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06004799 RID: 18329 RVA: 0x0017AC78 File Offset: 0x00178E78
		public override string Label
		{
			get
			{
				if (this.level == PsychicDroneLevel.GoodMedium)
				{
					return this.def.label + ": " + this.gender.GetLabel(false).CapitalizeFirst();
				}
				if (this.gender != Gender.None)
				{
					return string.Concat(new string[]
					{
						this.def.label,
						": ",
						this.level.GetLabel().CapitalizeFirst(),
						" (",
						this.gender.GetLabel(false).ToLower(),
						")"
					});
				}
				return this.def.label + ": " + this.level.GetLabel().CapitalizeFirst();
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600479A RID: 18330 RVA: 0x0017AD3C File Offset: 0x00178F3C
		public override string LetterText
		{
			get
			{
				if (this.level == PsychicDroneLevel.GoodMedium)
				{
					return this.def.letterText.Formatted(this.gender.GetLabel(false).ToLower());
				}
				return this.def.letterText.Formatted(this.gender.GetLabel(false).ToLower(), this.level.GetLabel());
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600479B RID: 18331 RVA: 0x0017ADB9 File Offset: 0x00178FB9
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x0017ADE1 File Offset: 0x00178FE1
		public override void PostMake()
		{
			base.PostMake();
			this.level = this.def.defaultDroneLevel;
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x0017ADFC File Offset: 0x00178FFC
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			if (this.def.defaultDroneLevel == PsychicDroneLevel.GoodMedium)
			{
				this.level = PsychicDroneLevel.GoodMedium;
			}
			else if (points < 800f)
			{
				this.level = PsychicDroneLevel.BadLow;
			}
			else if (points < 2000f)
			{
				this.level = PsychicDroneLevel.BadMedium;
			}
			else
			{
				this.level = PsychicDroneLevel.BadHigh;
			}
			if (map.mapPawns.FreeColonistsCount > 0)
			{
				this.gender = map.mapPawns.FreeColonists.RandomElement<Pawn>().gender;
			}
			else
			{
				this.gender = Rand.Element<Gender>(Gender.Male, Gender.Female);
			}
			outExtraDescriptionRules.Add(new Rule_String("psychicDroneLevel", this.level.GetLabel()));
			outExtraDescriptionRules.Add(new Rule_String("psychicDroneGender", this.gender.GetLabel(false)));
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x0017AEB8 File Offset: 0x001790B8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.level, "level", PsychicDroneLevel.None, false);
		}

		// Token: 0x04002BF0 RID: 11248
		public Gender gender;

		// Token: 0x04002BF1 RID: 11249
		public PsychicDroneLevel level = PsychicDroneLevel.BadMedium;

		// Token: 0x04002BF2 RID: 11250
		public const float MaxPointsDroneLow = 800f;

		// Token: 0x04002BF3 RID: 11251
		public const float MaxPointsDroneMedium = 2000f;
	}
}
