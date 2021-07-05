using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BD RID: 4541
	public class IncidentWorker_Flashstorm : IncidentWorker
	{
		// Token: 0x060063CE RID: 25550 RVA: 0x0004485D File Offset: 0x00042A5D
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return !((Map)parms.target).gameConditionManager.ConditionIsActive(GameConditionDefOf.Flashstorm);
		}

		// Token: 0x060063CF RID: 25551 RVA: 0x001F0AEC File Offset: 0x001EECEC
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
