using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C15 RID: 3093
	public class IncidentWorker_PsychicSoothe : IncidentWorker_PsychicEmanation
	{
		// Token: 0x060048A4 RID: 18596 RVA: 0x00180564 File Offset: 0x0017E764
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (base.TryExecuteWorker(parms))
			{
				SoundDefOf.PsychicSootheGlobal.PlayOneShotOnCamera((Map)parms.target);
				return true;
			}
			return false;
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x00180588 File Offset: 0x0017E788
		protected override void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points)
		{
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicSoothe, duration);
			gameCondition_PsychicEmanation.gender = gender;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			base.SendStandardLetter(gameCondition_PsychicEmanation.LabelCap, gameCondition_PsychicEmanation.LetterText, gameCondition_PsychicEmanation.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
		}
	}
}
