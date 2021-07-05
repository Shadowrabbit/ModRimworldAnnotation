using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C04 RID: 3076
	public class IncidentWorker_DiseaseAnimal : IncidentWorker_Disease
	{
		// Token: 0x0600485D RID: 18525 RVA: 0x0017EBC8 File Offset: 0x0017CDC8
		protected override IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map != null)
			{
				return from p in map.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.HostFaction == null && !p.RaceProps.Humanlike
				select p;
			}
			return from p in ((Caravan)target).PawnsListForReading
			where !p.RaceProps.Humanlike
			select p;
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x0017EC44 File Offset: 0x0017CE44
		protected override IEnumerable<Pawn> ActualVictims(IncidentParms parms)
		{
			Pawn[] potentialVictims = base.PotentialVictims(parms.target).ToArray<Pawn>();
			IEnumerable<ThingDef> source = (from v in potentialVictims
			select v.def).Distinct<ThingDef>();
			ThingDef targetRace = source.RandomElementByWeightWithFallback((ThingDef race) => (from v in potentialVictims
			where v.def == race
			select v.BodySize).Sum(), null);
			IEnumerable<Pawn> source2 = from v in potentialVictims
			where v.def == targetRace
			select v;
			int num = source2.Count<Pawn>();
			IntRange intRange = new IntRange(Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.min), Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.max));
			int num2 = intRange.RandomInRange;
			num2 = Mathf.Clamp(num2, 1, this.def.diseaseMaxVictims);
			return source2.InRandomOrder(null).Take(num2);
		}
	}
}
