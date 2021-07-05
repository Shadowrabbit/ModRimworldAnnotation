using System;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x020015ED RID: 5613
	public class SymbolResolver_Settlement : SymbolResolver
	{
		// Token: 0x060083B9 RID: 33721 RVA: 0x002F1334 File Offset: 0x002EF534
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			Faction faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			int num = 0;
			if (rp.edgeDefenseWidth != null)
			{
				num = rp.edgeDefenseWidth.Value;
			}
			else if (rp.rect.Width >= 20 && rp.rect.Height >= 20 && (faction.def.techLevel >= TechLevel.Industrial || Rand.Bool))
			{
				num = (Rand.Bool ? 2 : 4);
			}
			float num2 = (float)rp.rect.Area / 144f * 0.17f;
			BaseGen.globalSettings.minEmptyNodes = ((num2 < 1f) ? 0 : GenMath.RoundRandom(num2));
			ResolveParams resolveParams = rp;
			resolveParams.thingSetMakerDef = (resolveParams.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile);
			resolveParams.lootMarketValue = new float?(resolveParams.lootMarketValue ?? 1800f);
			BaseGen.symbolStack.Push("lootScatter", resolveParams, null);
			if (!(rp.settlementDontGeneratePawns ?? false))
			{
				Lord lord;
				if ((lord = rp.singlePawnLord) == null)
				{
					lord = LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, rp.rect.CenterCell, rp.attackWhenPlayerBecameEnemy ?? false), map, null);
				}
				Lord lord2 = lord;
				rp.settlementLord = lord2;
				TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rp.rect;
				resolveParams2.faction = faction;
				resolveParams2.singlePawnLord = lord2;
				resolveParams2.pawnGroupKindDef = (rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Settlement);
				resolveParams2.singlePawnSpawnCellExtraPredicate = (rp.singlePawnSpawnCellExtraPredicate ?? ((IntVec3 x) => map.reachability.CanReachMapEdge(x, traverseParms)));
				if (resolveParams2.pawnGroupMakerParams == null)
				{
					resolveParams2.pawnGroupMakerParams = new PawnGroupMakerParms();
					resolveParams2.pawnGroupMakerParams.tile = map.Tile;
					resolveParams2.pawnGroupMakerParams.faction = faction;
					resolveParams2.pawnGroupMakerParams.points = (rp.settlementPawnGroupPoints ?? SymbolResolver_Settlement.DefaultPawnsPoints.RandomInRange);
					resolveParams2.pawnGroupMakerParams.inhabitants = true;
					resolveParams2.pawnGroupMakerParams.seed = rp.settlementPawnGroupSeed;
				}
				rp.bedCount = new int?(PawnGroupMakerUtility.GeneratePawnKindsExample(SymbolResolver_PawnGroup.GetGroupMakerParms(map, resolveParams2)).Count<PawnKindDef>());
				BaseGen.symbolStack.Push("pawnGroup", resolveParams2, null);
			}
			if (SymbolResolver_TerrorBuildings.FactionShouldHaveTerrorBuildings(rp.faction))
			{
				BaseGen.symbolStack.Push("terrorBuildings", rp, null);
			}
			BaseGen.symbolStack.Push("outdoorLighting", rp, null);
			if (faction.def.techLevel >= TechLevel.Industrial)
			{
				int num3 = Rand.Chance(0.75f) ? GenMath.RoundRandom((float)rp.rect.Area / 400f) : 0;
				for (int i = 0; i < num3; i++)
				{
					ResolveParams resolveParams3 = rp;
					resolveParams3.faction = faction;
					BaseGen.symbolStack.Push("firefoamPopper", resolveParams3, null);
				}
			}
			if (num > 0)
			{
				ResolveParams resolveParams4 = rp;
				resolveParams4.faction = faction;
				resolveParams4.edgeDefenseWidth = new int?(num);
				resolveParams4.edgeThingMustReachMapEdge = new bool?(rp.edgeThingMustReachMapEdge ?? true);
				BaseGen.symbolStack.Push("edgeDefense", resolveParams4, null);
			}
			ResolveParams resolveParams5 = rp;
			resolveParams5.rect = rp.rect.ContractedBy(num);
			resolveParams5.faction = faction;
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", resolveParams5, null);
			ResolveParams resolveParams6 = rp;
			resolveParams6.rect = rp.rect.ContractedBy(num);
			resolveParams6.faction = faction;
			resolveParams6.floorOnlyIfTerrainSupports = new bool?(rp.floorOnlyIfTerrainSupports ?? true);
			BaseGen.symbolStack.Push("basePart_outdoors", resolveParams6, null);
			ResolveParams resolveParams7 = rp;
			resolveParams7.floorDef = TerrainDefOf.Bridge;
			resolveParams7.floorOnlyIfTerrainSupports = new bool?(rp.floorOnlyIfTerrainSupports ?? true);
			resolveParams7.allowBridgeOnAnyImpassableTerrain = new bool?(rp.allowBridgeOnAnyImpassableTerrain ?? true);
			BaseGen.symbolStack.Push("floor", resolveParams7, null);
		}

		// Token: 0x04005232 RID: 21042
		public static readonly FloatRange DefaultPawnsPoints = new FloatRange(1150f, 1600f);

		// Token: 0x04005233 RID: 21043
		public const float DefaultLootMarketValue = 1800f;
	}
}
