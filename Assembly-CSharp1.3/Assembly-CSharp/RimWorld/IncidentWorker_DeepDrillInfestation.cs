using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C00 RID: 3072
	public class IncidentWorker_DeepDrillInfestation : IncidentWorker
	{
		// Token: 0x0600484C RID: 18508 RVA: 0x0017E4BD File Offset: 0x0017C6BD
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			if (Faction.OfInsects == null)
			{
				return false;
			}
			Map map = (Map)parms.target;
			IncidentWorker_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, IncidentWorker_DeepDrillInfestation.tmpDrills);
			return IncidentWorker_DeepDrillInfestation.tmpDrills.Any<Thing>();
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x0017E4FC File Offset: 0x0017C6FC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IncidentWorker_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, IncidentWorker_DeepDrillInfestation.tmpDrills);
			Thing deepDrill;
			if (!IncidentWorker_DeepDrillInfestation.tmpDrills.TryRandomElement(out deepDrill))
			{
				return false;
			}
			IntVec3 intVec = CellFinder.FindNoWipeSpawnLocNear(deepDrill.Position, map, ThingDefOf.TunnelHiveSpawner, Rot4.North, 2, (IntVec3 x) => x.Walkable(map) && x.GetFirstThing(map, deepDrill.def) == null && x.GetFirstThingWithComp(map) == null && x.GetFirstThing(map, ThingDefOf.Hive) == null && x.GetFirstThing(map, ThingDefOf.TunnelHiveSpawner) == null);
			if (intVec == deepDrill.Position)
			{
				return false;
			}
			TunnelHiveSpawner tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null);
			tunnelHiveSpawner.spawnHive = false;
			tunnelHiveSpawner.insectsPoints = Mathf.Clamp(parms.points * Rand.Range(0.3f, 0.6f), 200f, 1000f);
			tunnelHiveSpawner.spawnedByInfestationThingComp = true;
			GenSpawn.Spawn(tunnelHiveSpawner, intVec, map, WipeMode.FullRefund);
			deepDrill.TryGetComp<CompCreatesInfestations>().Notify_CreatedInfestation();
			base.SendStandardLetter(parms, new TargetInfo(tunnelHiveSpawner.Position, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x04002C57 RID: 11351
		private static List<Thing> tmpDrills = new List<Thing>();

		// Token: 0x04002C58 RID: 11352
		private const float MinPointsFactor = 0.3f;

		// Token: 0x04002C59 RID: 11353
		private const float MaxPointsFactor = 0.6f;

		// Token: 0x04002C5A RID: 11354
		private const float MinPoints = 200f;

		// Token: 0x04002C5B RID: 11355
		private const float MaxPoints = 1000f;
	}
}
