using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C03 RID: 3075
	public class IncidentWorker_DiseaseHuman : IncidentWorker_Disease
	{
		// Token: 0x0600485A RID: 18522 RVA: 0x0017EAE0 File Offset: 0x0017CCE0
		protected override IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map != null)
			{
				return map.mapPawns.FreeColonistsAndPrisoners;
			}
			return from x in ((Caravan)target).PawnsListForReading
			where x.IsFreeColonist || x.IsPrisonerOfColony
			select x;
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x0017EB34 File Offset: 0x0017CD34
		protected override IEnumerable<Pawn> ActualVictims(IncidentParms parms)
		{
			int num = this.PotentialVictimCandidates(parms.target).Count<Pawn>();
			IntRange intRange = new IntRange(Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.min), Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.max));
			int num2 = intRange.RandomInRange;
			num2 = Mathf.Clamp(num2, 1, this.def.diseaseMaxVictims);
			return base.PotentialVictims(parms.target).InRandomOrder(null).Take(num2);
		}
	}
}
