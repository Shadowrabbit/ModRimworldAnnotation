using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D6 RID: 4566
	public abstract class IncidentWorker_PsychicEmanation : IncidentWorker
	{
		// Token: 0x0600641C RID: 25628 RVA: 0x001F1AB0 File Offset: 0x001EFCB0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe) && map.listerThings.ThingsOfDef(ThingDefOf.PsychicDronerShipPart).Count <= 0 && map.mapPawns.FreeColonistsCount != 0;
		}

		// Token: 0x0600641D RID: 25629 RVA: 0x001F1B1C File Offset: 0x001EFD1C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(parms, map, Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f), map.mapPawns.FreeColonists.RandomElement<Pawn>().gender, parms.points);
			return true;
		}

		// Token: 0x0600641E RID: 25630
		protected abstract void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points);
	}
}
