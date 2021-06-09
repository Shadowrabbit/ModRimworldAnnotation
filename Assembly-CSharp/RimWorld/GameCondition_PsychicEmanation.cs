using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001178 RID: 4472
	public class GameCondition_PsychicEmanation : GameCondition
	{
		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06006285 RID: 25221 RVA: 0x001EC374 File Offset: 0x001EA574
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

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06006286 RID: 25222 RVA: 0x001EC438 File Offset: 0x001EA638
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

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06006287 RID: 25223 RVA: 0x00043CFB File Offset: 0x00041EFB
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x06006288 RID: 25224 RVA: 0x00043D23 File Offset: 0x00041F23
		public override void PostMake()
		{
			base.PostMake();
			this.level = this.def.defaultDroneLevel;
		}

		// Token: 0x06006289 RID: 25225 RVA: 0x001EC4B8 File Offset: 0x001EA6B8
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

		// Token: 0x0600628A RID: 25226 RVA: 0x00043D3C File Offset: 0x00041F3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.level, "level", PsychicDroneLevel.None, false);
		}

		// Token: 0x04004208 RID: 16904
		public Gender gender;

		// Token: 0x04004209 RID: 16905
		public PsychicDroneLevel level = PsychicDroneLevel.BadMedium;

		// Token: 0x0400420A RID: 16906
		public const float MaxPointsDroneLow = 800f;

		// Token: 0x0400420B RID: 16907
		public const float MaxPointsDroneMedium = 2000f;
	}
}
