using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000BFE RID: 3070
	public class IncidentWorker_CrashedShipPart : IncidentWorker
	{
		// Token: 0x06004842 RID: 18498 RVA: 0x0017E01C File Offset: 0x0017C21C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ((Map)parms.target).listerThings.ThingsOfDef(this.def.mechClusterBuilding).Count <= 0;
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x0017E04C File Offset: 0x0017C24C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<TargetInfo> list = new List<TargetInfo>();
			ThingDef shipPartDef = this.def.mechClusterBuilding;
			IntVec3 intVec = IncidentWorker_CrashedShipPart.FindDropPodLocation(map, (IntVec3 spot) => base.<TryExecuteWorker>g__CanPlaceAt|0(spot));
			if (intVec == IntVec3.Invalid)
			{
				return false;
			}
			float points = Mathf.Max(parms.points * 0.9f, 300f);
			List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				tile = map.Tile,
				faction = Faction.OfMechanoids,
				points = points
			}, true).ToList<Pawn>();
			Thing thing = ThingMaker.MakeThing(shipPartDef, null);
			thing.SetFaction(Faction.OfMechanoids, null);
			LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_SleepThenMechanoidsDefend(new List<Thing>
			{
				thing
			}, Faction.OfMechanoids, 28f, intVec, false, false), map, list2);
			DropPodUtility.DropThingsNear(intVec, map, list2.Cast<Thing>(), 110, false, false, true, true);
			foreach (Pawn thing2 in list2)
			{
				CompCanBeDormant compCanBeDormant = thing2.TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null)
				{
					compCanBeDormant.ToSleep();
				}
			}
			list.AddRange(from p in list2
			select new TargetInfo(p));
			GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(ThingDefOf.CrashedShipPartIncoming, thing), intVec, map, WipeMode.Vanish);
			list.Add(new TargetInfo(intVec, map, false));
			base.SendStandardLetter(parms, list, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x0017E224 File Offset: 0x0017C424
		private static IntVec3 FindDropPodLocation(Map map, Predicate<IntVec3> validator)
		{
			for (int i = 0; i < 200; i++)
			{
				IntVec3 intVec = RCellFinder.FindSiegePositionFrom(DropCellFinder.FindRaidDropCenterDistant(map, true), map, true);
				if (validator(intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x04002C51 RID: 11345
		private const float ShipPointsFactor = 0.9f;

		// Token: 0x04002C52 RID: 11346
		private const int IncidentMinimumPoints = 300;

		// Token: 0x04002C53 RID: 11347
		private const float DefendRadius = 28f;
	}
}
