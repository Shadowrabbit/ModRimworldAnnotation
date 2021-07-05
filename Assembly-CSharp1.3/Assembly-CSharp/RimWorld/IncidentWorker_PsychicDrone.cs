using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C14 RID: 3092
	public class IncidentWorker_PsychicDrone : IncidentWorker_PsychicEmanation
	{
		// Token: 0x060048A1 RID: 18593 RVA: 0x001804A0 File Offset: 0x0017E6A0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (base.TryExecuteWorker(parms))
			{
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera((Map)parms.target);
				return true;
			}
			return false;
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x001804C4 File Offset: 0x0017E6C4
		protected override void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points)
		{
			if (points < 0f)
			{
				points = StorytellerUtility.DefaultThreatPointsNow(map);
			}
			PsychicDroneLevel level;
			if (points < 800f)
			{
				level = PsychicDroneLevel.BadLow;
			}
			else if (points < 2000f)
			{
				level = PsychicDroneLevel.BadMedium;
			}
			else
			{
				level = PsychicDroneLevel.BadHigh;
			}
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicDrone, duration);
			gameCondition_PsychicEmanation.gender = gender;
			gameCondition_PsychicEmanation.level = level;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			base.SendStandardLetter(gameCondition_PsychicEmanation.LabelCap, gameCondition_PsychicEmanation.LetterText, gameCondition_PsychicEmanation.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
		}

		// Token: 0x04002C6C RID: 11372
		private const float MaxPointsDroneLow = 800f;

		// Token: 0x04002C6D RID: 11373
		private const float MaxPointsDroneMedium = 2000f;
	}
}
