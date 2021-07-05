using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011D8 RID: 4568
	public class IncidentWorker_PsychicSoothe : IncidentWorker_PsychicEmanation
	{
		// Token: 0x06006423 RID: 25635 RVA: 0x00044B55 File Offset: 0x00042D55
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (base.TryExecuteWorker(parms))
			{
				SoundDefOf.PsychicSootheGlobal.PlayOneShotOnCamera((Map)parms.target);
				return true;
			}
			return false;
		}

		// Token: 0x06006424 RID: 25636 RVA: 0x001F1C0C File Offset: 0x001EFE0C
		protected override void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points)
		{
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicSoothe, duration);
			gameCondition_PsychicEmanation.gender = gender;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			base.SendStandardLetter(gameCondition_PsychicEmanation.LabelCap, gameCondition_PsychicEmanation.LetterText, gameCondition_PsychicEmanation.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
		}
	}
}
