using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E2A RID: 7722
	public struct ResolveParams
	{
		// Token: 0x0600A709 RID: 42761 RVA: 0x0006E67A File Offset: 0x0006C87A
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x0600A70A RID: 42762 RVA: 0x0006E68A File Offset: 0x0006C88A
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x0600A70B RID: 42763 RVA: 0x0006E698 File Offset: 0x0006C898
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x0600A70C RID: 42764 RVA: 0x0006E6A7 File Offset: 0x0006C8A7
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x0600A70D RID: 42765 RVA: 0x00308B9C File Offset: 0x00306D9C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"rect=",
				this.rect,
				", faction=",
				(this.faction != null) ? this.faction.ToString() : "null",
				", custom=",
				(this.custom != null) ? this.custom.Count.ToString() : "null",
				", pawnGroupMakerParams=",
				(this.pawnGroupMakerParams != null) ? this.pawnGroupMakerParams.ToString() : "null",
				", pawnGroupKindDef=",
				(this.pawnGroupKindDef != null) ? this.pawnGroupKindDef.ToString() : "null",
				", roofDef=",
				(this.roofDef != null) ? this.roofDef.ToString() : "null",
				", noRoof=",
				(this.noRoof != null) ? this.noRoof.ToString() : "null",
				", addRoomCenterToRootsToUnfog=",
				(this.addRoomCenterToRootsToUnfog != null) ? this.addRoomCenterToRootsToUnfog.ToString() : "null",
				", singleThingToSpawn=",
				(this.singleThingToSpawn != null) ? this.singleThingToSpawn.ToString() : "null",
				", singleThingDef=",
				(this.singleThingDef != null) ? this.singleThingDef.ToString() : "null",
				", singleThingStuff=",
				(this.singleThingStuff != null) ? this.singleThingStuff.ToString() : "null",
				", singleThingStackCount=",
				(this.singleThingStackCount != null) ? this.singleThingStackCount.ToString() : "null",
				", skipSingleThingIfHasToWipeBuildingOrDoesntFit=",
				(this.skipSingleThingIfHasToWipeBuildingOrDoesntFit != null) ? this.skipSingleThingIfHasToWipeBuildingOrDoesntFit.ToString() : "null",
				", spawnBridgeIfTerrainCantSupportThing=",
				(this.spawnBridgeIfTerrainCantSupportThing != null) ? this.spawnBridgeIfTerrainCantSupportThing.ToString() : "null",
				", singlePawnToSpawn=",
				(this.singlePawnToSpawn != null) ? this.singlePawnToSpawn.ToString() : "null",
				", singlePawnKindDef=",
				(this.singlePawnKindDef != null) ? this.singlePawnKindDef.ToString() : "null",
				", disableSinglePawn=",
				(this.disableSinglePawn != null) ? this.disableSinglePawn.ToString() : "null",
				", singlePawnLord=",
				(this.singlePawnLord != null) ? this.singlePawnLord.ToString() : "null",
				", singlePawnSpawnCellExtraPredicate=",
				(this.singlePawnSpawnCellExtraPredicate != null) ? this.singlePawnSpawnCellExtraPredicate.ToString() : "null",
				", singlePawnGenerationRequest=",
				(this.singlePawnGenerationRequest != null) ? this.singlePawnGenerationRequest.ToString() : "null",
				", postThingSpawn=",
				(this.postThingSpawn != null) ? this.postThingSpawn.ToString() : "null",
				", postThingGenerate=",
				(this.postThingGenerate != null) ? this.postThingGenerate.ToString() : "null",
				", mechanoidsCount=",
				(this.mechanoidsCount != null) ? this.mechanoidsCount.ToString() : "null",
				", hivesCount=",
				(this.hivesCount != null) ? this.hivesCount.ToString() : "null",
				", disableHives=",
				(this.disableHives != null) ? this.disableHives.ToString() : "null",
				", thingRot=",
				(this.thingRot != null) ? this.thingRot.ToString() : "null",
				", wallStuff=",
				(this.wallStuff != null) ? this.wallStuff.ToString() : "null",
				", chanceToSkipWallBlock=",
				(this.chanceToSkipWallBlock != null) ? this.chanceToSkipWallBlock.ToString() : "null",
				", floorDef=",
				(this.floorDef != null) ? this.floorDef.ToString() : "null",
				", chanceToSkipFloor=",
				(this.chanceToSkipFloor != null) ? this.chanceToSkipFloor.ToString() : "null",
				", filthDef=",
				(this.filthDef != null) ? this.filthDef.ToString() : "null",
				", filthDensity=",
				(this.filthDensity != null) ? this.filthDensity.ToString() : "null",
				", floorOnlyIfTerrainSupports=",
				(this.floorOnlyIfTerrainSupports != null) ? this.floorOnlyIfTerrainSupports.ToString() : "null",
				", allowBridgeOnAnyImpassableTerrain=",
				(this.allowBridgeOnAnyImpassableTerrain != null) ? this.allowBridgeOnAnyImpassableTerrain.ToString() : "null",
				", clearEdificeOnly=",
				(this.clearEdificeOnly != null) ? this.clearEdificeOnly.ToString() : "null",
				", clearFillageOnly=",
				(this.clearFillageOnly != null) ? this.clearFillageOnly.ToString() : "null",
				", clearRoof=",
				(this.clearRoof != null) ? this.clearRoof.ToString() : "null",
				", ancientCryptosleepCasketGroupID=",
				(this.ancientCryptosleepCasketGroupID != null) ? this.ancientCryptosleepCasketGroupID.ToString() : "null",
				", podContentsType=",
				(this.podContentsType != null) ? this.podContentsType.ToString() : "null",
				", thingSetMakerDef=",
				(this.thingSetMakerDef != null) ? this.thingSetMakerDef.ToString() : "null",
				", thingSetMakerParams=",
				(this.thingSetMakerParams != null) ? this.thingSetMakerParams.ToString() : "null",
				", stockpileConcreteContents=",
				(this.stockpileConcreteContents != null) ? this.stockpileConcreteContents.Count.ToString() : "null",
				", stockpileMarketValue=",
				(this.stockpileMarketValue != null) ? this.stockpileMarketValue.ToString() : "null",
				", innerStockpileSize=",
				(this.innerStockpileSize != null) ? this.innerStockpileSize.ToString() : "null",
				", edgeDefenseWidth=",
				(this.edgeDefenseWidth != null) ? this.edgeDefenseWidth.ToString() : "null",
				", edgeDefenseTurretsCount=",
				(this.edgeDefenseTurretsCount != null) ? this.edgeDefenseTurretsCount.ToString() : "null",
				", edgeDefenseMortarsCount=",
				(this.edgeDefenseMortarsCount != null) ? this.edgeDefenseMortarsCount.ToString() : "null",
				", edgeDefenseGuardsCount=",
				(this.edgeDefenseGuardsCount != null) ? this.edgeDefenseGuardsCount.ToString() : "null",
				", mortarDef=",
				(this.mortarDef != null) ? this.mortarDef.ToString() : "null",
				", pathwayFloorDef=",
				(this.pathwayFloorDef != null) ? this.pathwayFloorDef.ToString() : "null",
				", cultivatedPlantDef=",
				(this.cultivatedPlantDef != null) ? this.cultivatedPlantDef.ToString() : "null",
				", fillWithThingsPadding=",
				(this.fillWithThingsPadding != null) ? this.fillWithThingsPadding.ToString() : "null",
				", settlementPawnGroupPoints=",
				(this.settlementPawnGroupPoints != null) ? this.settlementPawnGroupPoints.ToString() : "null",
				", settlementPawnGroupSeed=",
				(this.settlementPawnGroupSeed != null) ? this.settlementPawnGroupSeed.ToString() : "null",
				", streetHorizontal=",
				(this.streetHorizontal != null) ? this.streetHorizontal.ToString() : "null",
				", edgeThingAvoidOtherEdgeThings=",
				(this.edgeThingAvoidOtherEdgeThings != null) ? this.edgeThingAvoidOtherEdgeThings.ToString() : "null",
				", edgeThingMustReachMapEdge=",
				(this.edgeThingMustReachMapEdge != null) ? this.edgeThingMustReachMapEdge.ToString() : "null",
				", allowPlacementOffEdge=",
				(this.allowPlacementOffEdge != null) ? this.allowPlacementOffEdge.ToString() : "null",
				", thrustAxis=",
				(this.thrustAxis != null) ? this.thrustAxis.ToString() : "null",
				", makeWarningLetter=",
				(this.makeWarningLetter != null) ? this.makeWarningLetter.ToString() : "null",
				", allowedMonumentThings=",
				(this.allowedMonumentThings != null) ? this.allowedMonumentThings.ToString() : "null"
			});
		}

		// Token: 0x04007156 RID: 29014
		public CellRect rect;

		// Token: 0x04007157 RID: 29015
		public Faction faction;

		// Token: 0x04007158 RID: 29016
		private Dictionary<string, object> custom;

		// Token: 0x04007159 RID: 29017
		public PawnGroupMakerParms pawnGroupMakerParams;

		// Token: 0x0400715A RID: 29018
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x0400715B RID: 29019
		public RoofDef roofDef;

		// Token: 0x0400715C RID: 29020
		public bool? noRoof;

		// Token: 0x0400715D RID: 29021
		public bool? addRoomCenterToRootsToUnfog;

		// Token: 0x0400715E RID: 29022
		public Thing singleThingToSpawn;

		// Token: 0x0400715F RID: 29023
		public ThingDef singleThingDef;

		// Token: 0x04007160 RID: 29024
		public ThingDef singleThingStuff;

		// Token: 0x04007161 RID: 29025
		public int? singleThingStackCount;

		// Token: 0x04007162 RID: 29026
		public bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit;

		// Token: 0x04007163 RID: 29027
		public bool? spawnBridgeIfTerrainCantSupportThing;

		// Token: 0x04007164 RID: 29028
		public Pawn singlePawnToSpawn;

		// Token: 0x04007165 RID: 29029
		public PawnKindDef singlePawnKindDef;

		// Token: 0x04007166 RID: 29030
		public bool? disableSinglePawn;

		// Token: 0x04007167 RID: 29031
		public Lord singlePawnLord;

		// Token: 0x04007168 RID: 29032
		public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

		// Token: 0x04007169 RID: 29033
		public PawnGenerationRequest? singlePawnGenerationRequest;

		// Token: 0x0400716A RID: 29034
		public Action<Thing> postThingSpawn;

		// Token: 0x0400716B RID: 29035
		public Action<Thing> postThingGenerate;

		// Token: 0x0400716C RID: 29036
		public int? mechanoidsCount;

		// Token: 0x0400716D RID: 29037
		public int? hivesCount;

		// Token: 0x0400716E RID: 29038
		public bool? disableHives;

		// Token: 0x0400716F RID: 29039
		public Rot4? thingRot;

		// Token: 0x04007170 RID: 29040
		public ThingDef wallStuff;

		// Token: 0x04007171 RID: 29041
		public float? chanceToSkipWallBlock;

		// Token: 0x04007172 RID: 29042
		public TerrainDef floorDef;

		// Token: 0x04007173 RID: 29043
		public float? chanceToSkipFloor;

		// Token: 0x04007174 RID: 29044
		public ThingDef filthDef;

		// Token: 0x04007175 RID: 29045
		public FloatRange? filthDensity;

		// Token: 0x04007176 RID: 29046
		public bool? floorOnlyIfTerrainSupports;

		// Token: 0x04007177 RID: 29047
		public bool? allowBridgeOnAnyImpassableTerrain;

		// Token: 0x04007178 RID: 29048
		public bool? clearEdificeOnly;

		// Token: 0x04007179 RID: 29049
		public bool? clearFillageOnly;

		// Token: 0x0400717A RID: 29050
		public bool? clearRoof;

		// Token: 0x0400717B RID: 29051
		public int? ancientCryptosleepCasketGroupID;

		// Token: 0x0400717C RID: 29052
		public PodContentsType? podContentsType;

		// Token: 0x0400717D RID: 29053
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x0400717E RID: 29054
		public ThingSetMakerParams? thingSetMakerParams;

		// Token: 0x0400717F RID: 29055
		public IList<Thing> stockpileConcreteContents;

		// Token: 0x04007180 RID: 29056
		public float? stockpileMarketValue;

		// Token: 0x04007181 RID: 29057
		public int? innerStockpileSize;

		// Token: 0x04007182 RID: 29058
		public int? edgeDefenseWidth;

		// Token: 0x04007183 RID: 29059
		public int? edgeDefenseTurretsCount;

		// Token: 0x04007184 RID: 29060
		public int? edgeDefenseMortarsCount;

		// Token: 0x04007185 RID: 29061
		public int? edgeDefenseGuardsCount;

		// Token: 0x04007186 RID: 29062
		public ThingDef mortarDef;

		// Token: 0x04007187 RID: 29063
		public TerrainDef pathwayFloorDef;

		// Token: 0x04007188 RID: 29064
		public ThingDef cultivatedPlantDef;

		// Token: 0x04007189 RID: 29065
		public int? fillWithThingsPadding;

		// Token: 0x0400718A RID: 29066
		public float? settlementPawnGroupPoints;

		// Token: 0x0400718B RID: 29067
		public int? settlementPawnGroupSeed;

		// Token: 0x0400718C RID: 29068
		public bool? streetHorizontal;

		// Token: 0x0400718D RID: 29069
		public bool? edgeThingAvoidOtherEdgeThings;

		// Token: 0x0400718E RID: 29070
		public bool? edgeThingMustReachMapEdge;

		// Token: 0x0400718F RID: 29071
		public bool? allowPlacementOffEdge;

		// Token: 0x04007190 RID: 29072
		public Rot4? thrustAxis;

		// Token: 0x04007191 RID: 29073
		public FloatRange? hpPercentRange;

		// Token: 0x04007192 RID: 29074
		public Thing conditionCauser;

		// Token: 0x04007193 RID: 29075
		public bool? makeWarningLetter;

		// Token: 0x04007194 RID: 29076
		public ThingFilter allowedMonumentThings;
	}
}
