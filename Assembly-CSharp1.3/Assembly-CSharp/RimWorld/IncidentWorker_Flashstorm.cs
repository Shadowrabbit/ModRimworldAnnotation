using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C06 RID: 3078
	public class IncidentWorker_Flashstorm : IncidentWorker
	{
		// Token: 0x06004868 RID: 18536 RVA: 0x0017F020 File Offset: 0x0017D220
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return !((Map)parms.target).gameConditionManager.ConditionIsActive(GameConditionDefOf.Flashstorm);
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x0017F040 File Offset: 0x0017D240
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition_Flashstorm gameCondition_Flashstorm = (GameCondition_Flashstorm)GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm, duration);
			map.gameConditionManager.RegisterCondition(gameCondition_Flashstorm);
			base.SendStandardLetter(this.def.letterLabel, GameConditionDefOf.Flashstorm.letterText, this.def.letterDef, parms, new TargetInfo(gameCondition_Flashstorm.centerLocation.ToIntVec3, map, false), Array.Empty<NamedArgument>());
			if (map.weatherManager.curWeather.rainRate > 0.1f)
			{
				map.weatherDecider.StartNextWeather();
			}
			return true;
		}
	}
}
