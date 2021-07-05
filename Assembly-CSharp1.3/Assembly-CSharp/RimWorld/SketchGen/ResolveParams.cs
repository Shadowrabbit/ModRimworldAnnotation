using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200157C RID: 5500
	public struct ResolveParams
	{
		// Token: 0x0600820F RID: 33295 RVA: 0x002DF3E7 File Offset: 0x002DD5E7
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x06008210 RID: 33296 RVA: 0x002DF3F7 File Offset: 0x002DD5F7
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x06008211 RID: 33297 RVA: 0x002DF405 File Offset: 0x002DD605
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x06008212 RID: 33298 RVA: 0x002DF414 File Offset: 0x002DD614
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x06008213 RID: 33299 RVA: 0x002DF424 File Offset: 0x002DD624
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"sketch=",
				(this.sketch != null) ? this.sketch.ToString() : "null",
				", rect=",
				(this.rect != null) ? this.rect.ToString() : "null",
				", allowWood=",
				(this.allowWood != null) ? this.allowWood.ToString() : "null",
				", custom=",
				(this.custom != null) ? this.custom.Count.ToString() : "null",
				", symmetryOrigin=",
				(this.symmetryOrigin != null) ? this.symmetryOrigin.ToString() : "null",
				", symmetryVertical=",
				(this.symmetryVertical != null) ? this.symmetryVertical.ToString() : "null",
				", symmetryOriginIncluded=",
				(this.symmetryOriginIncluded != null) ? this.symmetryOriginIncluded.ToString() : "null",
				", symmetryClear=",
				(this.symmetryClear != null) ? this.symmetryClear.ToString() : "null",
				", connectedGroupsSameStuff=",
				(this.connectedGroupsSameStuff != null) ? this.connectedGroupsSameStuff.ToString() : "null",
				", assignRandomStuffTo=",
				(this.assignRandomStuffTo != null) ? this.assignRandomStuffTo.ToString() : "null",
				", cornerThing=",
				(this.cornerThing != null) ? this.cornerThing.ToString() : "null",
				", floorFillRoomsOnly=",
				(this.floorFillRoomsOnly != null) ? this.floorFillRoomsOnly.ToString() : "null",
				", singleFloorType=",
				(this.singleFloorType != null) ? this.singleFloorType.ToString() : "null",
				", onlyStoneFloors=",
				(this.onlyStoneFloors != null) ? this.onlyStoneFloors.ToString() : "null",
				", thingCentral=",
				(this.thingCentral != null) ? this.thingCentral.ToString() : "null",
				", wallEdgeThing=",
				(this.wallEdgeThing != null) ? this.wallEdgeThing.ToString() : "null",
				", monumentSize=",
				(this.monumentSize != null) ? this.monumentSize.ToString() : "null",
				", monumentOpen=",
				(this.monumentOpen != null) ? this.monumentOpen.ToString() : "null",
				", allowMonumentDoors=",
				(this.allowMonumentDoors != null) ? this.allowMonumentDoors.ToString() : "null",
				", allowedMonumentThings=",
				(this.allowedMonumentThings != null) ? this.allowedMonumentThings.ToString() : "null",
				", useOnlyStonesAvailableOnMap=",
				(this.useOnlyStonesAvailableOnMap != null) ? this.useOnlyStonesAvailableOnMap.ToString() : "null",
				", allowConcrete=",
				(this.allowConcrete != null) ? this.allowConcrete.ToString() : "null",
				", allowFlammableWalls=",
				(this.allowFlammableWalls != null) ? this.allowFlammableWalls.ToString() : "null",
				", onlyBuildableByPlayer=",
				(this.onlyBuildableByPlayer != null) ? this.onlyBuildableByPlayer.ToString() : "null",
				", addFloor=",
				(this.addFloors != null) ? this.addFloors.ToString() : "null",
				", requireFloor=",
				(this.requireFloor != null) ? this.requireFloor.ToString() : "null",
				", mechClusterSize=",
				(this.mechClusterSize != null) ? this.mechClusterSize.ToString() : "null",
				", mechClusterDormant=",
				(this.mechClusterDormant != null) ? this.mechClusterDormant.ToString() : "null",
				", mechClusterForMap=",
				(this.mechClusterForMap != null) ? this.mechClusterForMap.ToString() : "null"
			});
		}

		// Token: 0x040050E4 RID: 20708
		public Sketch sketch;

		// Token: 0x040050E5 RID: 20709
		public CellRect? rect;

		// Token: 0x040050E6 RID: 20710
		public bool? allowWood;

		// Token: 0x040050E7 RID: 20711
		public float? points;

		// Token: 0x040050E8 RID: 20712
		public float? totalPoints;

		// Token: 0x040050E9 RID: 20713
		public float? chance;

		// Token: 0x040050EA RID: 20714
		public int? symmetryOrigin;

		// Token: 0x040050EB RID: 20715
		public bool? symmetryVertical;

		// Token: 0x040050EC RID: 20716
		public bool? symmetryOriginIncluded;

		// Token: 0x040050ED RID: 20717
		public bool? symmetryClear;

		// Token: 0x040050EE RID: 20718
		public bool? connectedGroupsSameStuff;

		// Token: 0x040050EF RID: 20719
		public ThingDef assignRandomStuffTo;

		// Token: 0x040050F0 RID: 20720
		public ThingDef cornerThing;

		// Token: 0x040050F1 RID: 20721
		public bool? floorFillRoomsOnly;

		// Token: 0x040050F2 RID: 20722
		public bool? singleFloorType;

		// Token: 0x040050F3 RID: 20723
		public bool? onlyStoneFloors;

		// Token: 0x040050F4 RID: 20724
		public ThingDef thingCentral;

		// Token: 0x040050F5 RID: 20725
		public ThingDef wallEdgeThing;

		// Token: 0x040050F6 RID: 20726
		public IntVec2? monumentSize;

		// Token: 0x040050F7 RID: 20727
		public bool? monumentOpen;

		// Token: 0x040050F8 RID: 20728
		public bool? allowMonumentDoors;

		// Token: 0x040050F9 RID: 20729
		public ThingFilter allowedMonumentThings;

		// Token: 0x040050FA RID: 20730
		public Map useOnlyStonesAvailableOnMap;

		// Token: 0x040050FB RID: 20731
		public bool? allowConcrete;

		// Token: 0x040050FC RID: 20732
		public bool? allowFlammableWalls;

		// Token: 0x040050FD RID: 20733
		public bool? onlyBuildableByPlayer;

		// Token: 0x040050FE RID: 20734
		public bool? addFloors;

		// Token: 0x040050FF RID: 20735
		public bool? requireFloor;

		// Token: 0x04005100 RID: 20736
		public IntVec2? mechClusterSize;

		// Token: 0x04005101 RID: 20737
		public bool? mechClusterDormant;

		// Token: 0x04005102 RID: 20738
		public Map mechClusterForMap;

		// Token: 0x04005103 RID: 20739
		public bool? forceNoConditionCauser;

		// Token: 0x04005104 RID: 20740
		public IntVec2? utilityBuildingSize;

		// Token: 0x04005105 RID: 20741
		public float? destroyChanceExp;

		// Token: 0x04005106 RID: 20742
		public IntVec2? landingPadSize;

		// Token: 0x04005107 RID: 20743
		private Dictionary<string, object> custom;
	}
}
