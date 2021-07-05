using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x02001598 RID: 5528
	public struct ResolveParams
	{
		// Token: 0x06008296 RID: 33430 RVA: 0x002E5A21 File Offset: 0x002E3C21
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x06008297 RID: 33431 RVA: 0x002E5A31 File Offset: 0x002E3C31
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x06008298 RID: 33432 RVA: 0x002E5A3F File Offset: 0x002E3C3F
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x06008299 RID: 33433 RVA: 0x002E5A4E File Offset: 0x002E3C4E
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x0600829A RID: 33434 RVA: 0x002E5A5C File Offset: 0x002E3C5C
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
				", combatPoints=",
				(this.threatPoints != null) ? this.threatPoints.ToString() : "null",
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
				", singleThingInnerThings=",
				(this.singleThingInnerThings != null) ? this.singleThingInnerThings.ToString() : "null",
				", singlePawnToSpawn=",
				(this.singlePawnToSpawn != null) ? this.singlePawnToSpawn.ToString() : "null",
				", singlePawnKindDef=",
				(this.singlePawnKindDef != null) ? this.singlePawnKindDef.ToString() : "null",
				", disableSinglePawn=",
				(this.disableSinglePawn != null) ? this.disableSinglePawn.ToString() : "null",
				", singlePawnLord=",
				(this.singlePawnLord != null) ? this.singlePawnLord.ToString() : "null",
				", settlementLord=",
				(this.settlementLord != null) ? this.settlementLord.ToString() : "null",
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
				", ancientCryptosleepCasketOpenSignalTag=",
				(this.ancientCryptosleepCasketOpenSignalTag != null) ? this.ancientCryptosleepCasketOpenSignalTag.ToString() : "null",
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
				", fixedCulativedPlantGrowth=",
				(this.fixedCulativedPlantGrowth != null) ? this.fixedCulativedPlantGrowth.ToString() : "null",
				", fillWithThingsPadding=",
				(this.fillWithThingsPadding != null) ? this.fillWithThingsPadding.ToString() : "null",
				", settlementPawnGroupPoints=",
				(this.settlementPawnGroupPoints != null) ? this.settlementPawnGroupPoints.ToString() : "null",
				", settlementPawnGroupSeed=",
				(this.settlementPawnGroupSeed != null) ? this.settlementPawnGroupSeed.ToString() : "null",
				", settlementDontGeneratePawns=",
				(this.settlementDontGeneratePawns != null) ? this.settlementDontGeneratePawns.ToString() : "null",
				", attackWhenPlayerBecameEnemy=",
				(this.attackWhenPlayerBecameEnemy != null) ? this.attackWhenPlayerBecameEnemy.ToString() : "null",
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
				(this.allowedMonumentThings != null) ? this.allowedMonumentThings.ToString() : "null",
				", bedCount=",
				(this.bedCount != null) ? this.bedCount.ToString() : string.Concat(new string[]
				{
					"null, workSitePoints=",
					(this.workSitePoints != 0f) ? this.workSitePoints.ToString() : "null",
					", lootConcereteContents=",
					(this.lootConcereteContents != null) ? this.lootConcereteContents.ToString() : "null",
					", lootMarketValue=",
					(this.lootMarketValue != null) ? this.lootMarketValue.ToString() : "null"
				}),
				", extraDoorEdge=",
				(this.extraDoorEdge != null) ? this.extraDoorEdge.ToString() : "null",
				", minLengthAfterSplit=",
				(this.minLengthAfterSplit != null) ? this.minLengthAfterSplit.ToString() : "null"
			});
		}

		// Token: 0x0400514E RID: 20814
		public CellRect rect;

		// Token: 0x0400514F RID: 20815
		public Faction faction;

		// Token: 0x04005150 RID: 20816
		public float? threatPoints;

		// Token: 0x04005151 RID: 20817
		private Dictionary<string, object> custom;

		// Token: 0x04005152 RID: 20818
		public PawnGroupMakerParms pawnGroupMakerParams;

		// Token: 0x04005153 RID: 20819
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x04005154 RID: 20820
		public RoofDef roofDef;

		// Token: 0x04005155 RID: 20821
		public bool? noRoof;

		// Token: 0x04005156 RID: 20822
		public bool? addRoomCenterToRootsToUnfog;

		// Token: 0x04005157 RID: 20823
		public Thing singleThingToSpawn;

		// Token: 0x04005158 RID: 20824
		public ThingDef singleThingDef;

		// Token: 0x04005159 RID: 20825
		public ThingDef singleThingStuff;

		// Token: 0x0400515A RID: 20826
		public int? singleThingStackCount;

		// Token: 0x0400515B RID: 20827
		public bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit;

		// Token: 0x0400515C RID: 20828
		public bool? spawnBridgeIfTerrainCantSupportThing;

		// Token: 0x0400515D RID: 20829
		public bool? spawnOutside;

		// Token: 0x0400515E RID: 20830
		public List<Thing> singleThingInnerThings;

		// Token: 0x0400515F RID: 20831
		public Pawn singlePawnToSpawn;

		// Token: 0x04005160 RID: 20832
		public PawnKindDef singlePawnKindDef;

		// Token: 0x04005161 RID: 20833
		public bool? disableSinglePawn;

		// Token: 0x04005162 RID: 20834
		public Lord singlePawnLord;

		// Token: 0x04005163 RID: 20835
		public Lord settlementLord;

		// Token: 0x04005164 RID: 20836
		public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

		// Token: 0x04005165 RID: 20837
		public PawnGenerationRequest? singlePawnGenerationRequest;

		// Token: 0x04005166 RID: 20838
		public Action<Thing> postThingSpawn;

		// Token: 0x04005167 RID: 20839
		public Action<Thing> postThingGenerate;

		// Token: 0x04005168 RID: 20840
		public int? mechanoidsCount;

		// Token: 0x04005169 RID: 20841
		public int? hivesCount;

		// Token: 0x0400516A RID: 20842
		public bool? disableHives;

		// Token: 0x0400516B RID: 20843
		public Rot4? thingRot;

		// Token: 0x0400516C RID: 20844
		public ThingDef wallStuff;

		// Token: 0x0400516D RID: 20845
		public float? chanceToSkipWallBlock;

		// Token: 0x0400516E RID: 20846
		public ThingDef wallThingDef;

		// Token: 0x0400516F RID: 20847
		public TerrainDef floorDef;

		// Token: 0x04005170 RID: 20848
		public float? chanceToSkipFloor;

		// Token: 0x04005171 RID: 20849
		public ThingDef filthDef;

		// Token: 0x04005172 RID: 20850
		public FloatRange? filthDensity;

		// Token: 0x04005173 RID: 20851
		public bool? floorOnlyIfTerrainSupports;

		// Token: 0x04005174 RID: 20852
		public bool? allowBridgeOnAnyImpassableTerrain;

		// Token: 0x04005175 RID: 20853
		public bool? clearEdificeOnly;

		// Token: 0x04005176 RID: 20854
		public bool? clearFillageOnly;

		// Token: 0x04005177 RID: 20855
		public bool? clearRoof;

		// Token: 0x04005178 RID: 20856
		public int? ancientCryptosleepCasketGroupID;

		// Token: 0x04005179 RID: 20857
		public PodContentsType? podContentsType;

		// Token: 0x0400517A RID: 20858
		public string ancientCryptosleepCasketOpenSignalTag;

		// Token: 0x0400517B RID: 20859
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x0400517C RID: 20860
		public ThingSetMakerParams? thingSetMakerParams;

		// Token: 0x0400517D RID: 20861
		public IList<Thing> stockpileConcreteContents;

		// Token: 0x0400517E RID: 20862
		public float? stockpileMarketValue;

		// Token: 0x0400517F RID: 20863
		public int? innerStockpileSize;

		// Token: 0x04005180 RID: 20864
		public int? edgeDefenseWidth;

		// Token: 0x04005181 RID: 20865
		public int? edgeDefenseTurretsCount;

		// Token: 0x04005182 RID: 20866
		public int? edgeDefenseMortarsCount;

		// Token: 0x04005183 RID: 20867
		public int? edgeDefenseGuardsCount;

		// Token: 0x04005184 RID: 20868
		public ThingDef mortarDef;

		// Token: 0x04005185 RID: 20869
		public TerrainDef pathwayFloorDef;

		// Token: 0x04005186 RID: 20870
		public ThingDef cultivatedPlantDef;

		// Token: 0x04005187 RID: 20871
		public float? fixedCulativedPlantGrowth;

		// Token: 0x04005188 RID: 20872
		public int? fillWithThingsPadding;

		// Token: 0x04005189 RID: 20873
		public float? settlementPawnGroupPoints;

		// Token: 0x0400518A RID: 20874
		public int? settlementPawnGroupSeed;

		// Token: 0x0400518B RID: 20875
		public bool? settlementDontGeneratePawns;

		// Token: 0x0400518C RID: 20876
		public bool? attackWhenPlayerBecameEnemy;

		// Token: 0x0400518D RID: 20877
		public bool? streetHorizontal;

		// Token: 0x0400518E RID: 20878
		public bool? edgeThingAvoidOtherEdgeThings;

		// Token: 0x0400518F RID: 20879
		public bool? edgeThingMustReachMapEdge;

		// Token: 0x04005190 RID: 20880
		public bool? allowPlacementOffEdge;

		// Token: 0x04005191 RID: 20881
		public Rot4? thrustAxis;

		// Token: 0x04005192 RID: 20882
		public FloatRange? hpPercentRange;

		// Token: 0x04005193 RID: 20883
		public Thing conditionCauser;

		// Token: 0x04005194 RID: 20884
		public bool? makeWarningLetter;

		// Token: 0x04005195 RID: 20885
		public ThingFilter allowedMonumentThings;

		// Token: 0x04005196 RID: 20886
		public bool? allowGeneratingThronerooms;

		// Token: 0x04005197 RID: 20887
		public int? bedCount;

		// Token: 0x04005198 RID: 20888
		public float workSitePoints;

		// Token: 0x04005199 RID: 20889
		public IList<Thing> lootConcereteContents;

		// Token: 0x0400519A RID: 20890
		public float? lootMarketValue;

		// Token: 0x0400519B RID: 20891
		public Rot4? extraDoorEdge;

		// Token: 0x0400519C RID: 20892
		public string sleepingInsectsWakeupSignalTag;

		// Token: 0x0400519D RID: 20893
		public string sleepingMechanoidsWakeupSignalTag;

		// Token: 0x0400519E RID: 20894
		public string rectTriggerSignalTag;

		// Token: 0x0400519F RID: 20895
		public bool? destroyIfUnfogged;

		// Token: 0x040051A0 RID: 20896
		public string messageSignalTag;

		// Token: 0x040051A1 RID: 20897
		public string message;

		// Token: 0x040051A2 RID: 20898
		public MessageTypeDef messageType;

		// Token: 0x040051A3 RID: 20899
		public LookTargets lookTargets;

		// Token: 0x040051A4 RID: 20900
		public bool? spawnAnywhereIfNoGoodCell;

		// Token: 0x040051A5 RID: 20901
		public bool? ignoreRoofedRequirement;

		// Token: 0x040051A6 RID: 20902
		public IntVec3? overrideLoc;

		// Token: 0x040051A7 RID: 20903
		public bool? sendStandardLetter;

		// Token: 0x040051A8 RID: 20904
		public string infestationSignalTag;

		// Token: 0x040051A9 RID: 20905
		public float? insectsPoints;

		// Token: 0x040051AA RID: 20906
		public string ambushSignalTag;

		// Token: 0x040051AB RID: 20907
		public SignalActionAmbushType? ambushType;

		// Token: 0x040051AC RID: 20908
		public IntVec3? spawnNear;

		// Token: 0x040051AD RID: 20909
		public CellRect? spawnAround;

		// Token: 0x040051AE RID: 20910
		public bool? spawnPawnsOnEdge;

		// Token: 0x040051AF RID: 20911
		public float? ambushPoints;

		// Token: 0x040051B0 RID: 20912
		public int? cornerRadius;

		// Token: 0x040051B1 RID: 20913
		public SoundDef sound;

		// Token: 0x040051B2 RID: 20914
		public string soundOneShotActionSignalTag;

		// Token: 0x040051B3 RID: 20915
		public string unfoggedSignalTag;

		// Token: 0x040051B4 RID: 20916
		public Thing triggerContainerEmptiedThing;

		// Token: 0x040051B5 RID: 20917
		public string triggerContainerEmptiedSignalTag;

		// Token: 0x040051B6 RID: 20918
		public Building_Door openDoorActionDoor;

		// Token: 0x040051B7 RID: 20919
		public string openDoorActionSignalTag;

		// Token: 0x040051B8 RID: 20920
		public string triggerSecuritySignal;

		// Token: 0x040051B9 RID: 20921
		public Thing relicThing;

		// Token: 0x040051BA RID: 20922
		public float? exteriorThreatPoints;

		// Token: 0x040051BB RID: 20923
		public float? interiorThreatPoints;

		// Token: 0x040051BC RID: 20924
		public SitePart sitePart;

		// Token: 0x040051BD RID: 20925
		public int? minLengthAfterSplit;

		// Token: 0x040051BE RID: 20926
		public bool? sendWokenUpMessage;

		// Token: 0x040051BF RID: 20927
		public ComplexSketch ancientComplexSketch;

		// Token: 0x040051C0 RID: 20928
		public bool? ignoreDoorways;

		// Token: 0x040051C1 RID: 20929
		public int? minorBuildingCount;

		// Token: 0x040051C2 RID: 20930
		public int? minorBuildingRadialDistance;

		// Token: 0x040051C3 RID: 20931
		public PawnKindDef desiccatedCorpsePawnKind;

		// Token: 0x040051C4 RID: 20932
		public IntRange? desiccatedCorpseRandomAgeRange;

		// Token: 0x040051C5 RID: 20933
		public FloatRange? desiccatedCorpseDensityRange;
	}
}
