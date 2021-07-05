using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C13 RID: 3091
	public abstract class IncidentWorker_PsychicEmanation : IncidentWorker
	{
		// Token: 0x0600489D RID: 18589 RVA: 0x001803DC File Offset: 0x0017E5DC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe) && map.listerThings.ThingsOfDef(ThingDefOf.PsychicDronerShipPart).Count <= 0 && map.mapPawns.FreeColonistsCount != 0;
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x00180448 File Offset: 0x0017E648
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(parms, map, Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f), map.mapPawns.FreeColonists.RandomElement<Pawn>().gender, parms.points);
			return true;
		}

		// Token: 0x0600489F RID: 18591
		protected abstract void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points);
	}
}
