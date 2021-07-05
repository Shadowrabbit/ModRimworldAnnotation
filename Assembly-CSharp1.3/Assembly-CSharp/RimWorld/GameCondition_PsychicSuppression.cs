using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000BE7 RID: 3047
	public class GameCondition_PsychicSuppression : GameCondition
	{
		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x060047C0 RID: 18368 RVA: 0x0017B3A5 File Offset: 0x001795A5
		public override string LetterText
		{
			get
			{
				return base.LetterText.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x060047C1 RID: 18369 RVA: 0x0017B3CD File Offset: 0x001795CD
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x0017B3F5 File Offset: 0x001795F5
		public override void Init()
		{
			base.Init();
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x0017B3FD File Offset: 0x001795FD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x0017B418 File Offset: 0x00179618
		public static void CheckPawn(Pawn pawn, Gender targetGender)
		{
			if (pawn.RaceProps.Humanlike && pawn.gender == targetGender && !pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicSuppression, false))
			{
				pawn.health.AddHediff(HediffDefOf.PsychicSuppression, null, null, null);
			}
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x0017B470 File Offset: 0x00179670
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

		// Token: 0x060047C6 RID: 18374 RVA: 0x0017B504 File Offset: 0x00179704
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

		// Token: 0x04002C03 RID: 11267
		public Gender gender;
	}
}
