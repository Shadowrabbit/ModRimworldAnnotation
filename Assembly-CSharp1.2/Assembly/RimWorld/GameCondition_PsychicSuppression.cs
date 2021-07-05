using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001180 RID: 4480
	public class GameCondition_PsychicSuppression : GameCondition
	{
		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x060062C9 RID: 25289 RVA: 0x00043F67 File Offset: 0x00042167
		public override string LetterText
		{
			get
			{
				return base.LetterText.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x060062CA RID: 25290 RVA: 0x00043F8F File Offset: 0x0004218F
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x060062CB RID: 25291 RVA: 0x00043FB7 File Offset: 0x000421B7
		public override void Init()
		{
			base.Init();
		}

		// Token: 0x060062CC RID: 25292 RVA: 0x00043FBF File Offset: 0x000421BF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x060062CD RID: 25293 RVA: 0x001ED0C4 File Offset: 0x001EB2C4
		public static void CheckPawn(Pawn pawn, Gender targetGender)
		{
			if (pawn.RaceProps.Humanlike && pawn.gender == targetGender && !pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicSuppression, false))
			{
				pawn.health.AddHediff(HediffDefOf.PsychicSuppression, null, null, null);
			}
		}

		// Token: 0x060062CE RID: 25294 RVA: 0x001ED11C File Offset: 0x001EB31C
		public override void GameConditionTick()
		{
			foreach (Map map in base.AffectedMaps)
			{
				foreach (Pawn pawn in map.mapPawns.AllPawns)
				{
					GameCondition_PsychicSuppression.CheckPawn(pawn, this.gender);
				}
			}
		}

		// Token: 0x060062CF RID: 25295 RVA: 0x001ED1B0 File Offset: 0x001EB3B0
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			if (map.mapPawns.FreeColonistsCount > 0)
			{
				this.gender = map.mapPawns.FreeColonists.RandomElement<Pawn>().gender;
			}
			else
			{
				this.gender = Rand.Element<Gender>(Gender.Male, Gender.Female);
			}
			outExtraDescriptionRules.Add(new Rule_String("psychicSuppressorGender", this.gender.GetLabel(false)));
		}

		// Token: 0x04004220 RID: 16928
		public Gender gender;
	}
}
