using System;
using System.Linq;
using RimWorld.BaseGen;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC1 RID: 3265
	public class GenStep_WorkSitePawns : GenStep
	{
		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06004C00 RID: 19456 RVA: 0x001956E8 File Offset: 0x001938E8
		public override int SeedPart
		{
			get
			{
				return 237483478;
			}
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x001956F0 File Offset: 0x001938F0
		public static int GetEnemiesCount(Site site, SitePartParams parms, PawnGroupKindDef workerGroupKind)
		{
			int pawnGroupMakerSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms);
			return PawnGroupMakerUtility.GeneratePawnKindsExample(GenStep_WorkSitePawns.GroupMakerParmsWorkers(site.Tile, site.Faction, parms.threatPoints, pawnGroupMakerSeed, workerGroupKind)).Count<PawnKindDef>() + PawnGroupMakerUtility.GeneratePawnKindsExample(GenStep_WorkSitePawns.GroupMakerParmsFighters(site.Tile, site.Faction, parms.threatPoints, pawnGroupMakerSeed)).Count<PawnKindDef>();
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x0019574C File Offset: 0x0019394C
		private static PawnGroupMakerParms GroupMakerParmsWorkers(int tile, Faction faction, float points, int seed, PawnGroupKindDef workerGroupKind)
		{
			float a = points / 2f;
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = workerGroupKind;
			pawnGroupMakerParms.tile = tile;
			pawnGroupMakerParms.faction = faction;
			pawnGroupMakerParms.inhabitants = true;
			pawnGroupMakerParms.seed = new int?(seed);
			pawnGroupMakerParms.points = Mathf.Max(a, faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, pawnGroupMakerParms));
			return pawnGroupMakerParms;
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x001957B0 File Offset: 0x001939B0
		private static PawnGroupMakerParms GroupMakerParmsFighters(int tile, Faction faction, float points, int seed)
		{
			float a = points / 2f;
			PawnGroupKindDef groupKindDef = PawnGroupKindDefOf.Combat;
			if (!faction.def.pawnGroupMakers.Any((PawnGroupMaker maker) => maker.kindDef == groupKindDef))
			{
				groupKindDef = PawnGroupKindDefOf.Settlement;
			}
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = groupKindDef;
			pawnGroupMakerParms.tile = tile;
			pawnGroupMakerParms.faction = faction;
			pawnGroupMakerParms.inhabitants = true;
			pawnGroupMakerParms.generateFightersOnly = true;
			pawnGroupMakerParms.seed = new int?(seed);
			pawnGroupMakerParms.points = Mathf.Max(a, faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, pawnGroupMakerParms));
			return pawnGroupMakerParms;
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x00195858 File Offset: 0x00193A58
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect rect;
			IntVec3 baseCenter;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rect))
			{
				baseCenter = rect.CenterCell;
				Log.Error("No rect of interest set when running GenStep_WorkSitePawns!");
			}
			else
			{
				baseCenter = map.Center;
			}
			Faction faction = parms.sitePart.site.Faction;
			Lord singlePawnLord = LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, baseCenter, false), map, null);
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			resolveParams.singlePawnLord = singlePawnLord;
			resolveParams.singlePawnSpawnCellExtraPredicate = ((IntVec3 x) => map.reachability.CanReachMapEdge(x, traverseParms));
			int pawnGroupMakerSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms);
			resolveParams.pawnGroupMakerParams = GenStep_WorkSitePawns.GroupMakerParmsWorkers(map.Tile, faction, parms.sitePart.parms.threatPoints, pawnGroupMakerSeed, ((SitePartWorker_WorkSite)parms.sitePart.def.Worker).WorkerGroupKind);
			resolveParams.pawnGroupKindDef = resolveParams.pawnGroupMakerParams.groupKind;
			BaseGen.symbolStack.Push("pawnGroup", resolveParams, null);
			ResolveParams resolveParams2 = resolveParams;
			resolveParams2.pawnGroupMakerParams = GenStep_WorkSitePawns.GroupMakerParmsFighters(map.Tile, faction, parms.sitePart.parms.threatPoints, pawnGroupMakerSeed);
			resolveParams2.pawnGroupKindDef = resolveParams2.pawnGroupMakerParams.groupKind;
			BaseGen.symbolStack.Push("pawnGroup", resolveParams2, null);
			BaseGen.globalSettings.map = map;
			BaseGen.Generate();
		}
	}
}
