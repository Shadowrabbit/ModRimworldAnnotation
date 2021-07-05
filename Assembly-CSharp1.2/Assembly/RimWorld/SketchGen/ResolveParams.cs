using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E02 RID: 7682
	public struct ResolveParams
	{
		// Token: 0x0600A66E RID: 42606 RVA: 0x0006E196 File Offset: 0x0006C396
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x0600A66F RID: 42607 RVA: 0x0006E1A6 File Offset: 0x0006C3A6
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x0600A670 RID: 42608 RVA: 0x0006E1B4 File Offset: 0x0006C3B4
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x0600A671 RID: 42609 RVA: 0x0006E1C3 File Offset: 0x0006C3C3
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x0600A672 RID: 42610 RVA: 0x00303C74 File Offset: 0x00301E74
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

		// Token: 0x040070C2 RID: 28866
		public Sketch sketch;

		// Token: 0x040070C3 RID: 28867
		public CellRect? rect;

		// Token: 0x040070C4 RID: 28868
		public bool? allowWood;

		// Token: 0x040070C5 RID: 28869
		public float? points;

		// Token: 0x040070C6 RID: 28870
		public float? totalPoints;

		// Token: 0x040070C7 RID: 28871
		public int? symmetryOrigin;

		// Token: 0x040070C8 RID: 28872
		public bool? symmetryVertical;

		// Token: 0x040070C9 RID: 28873
		public bool? symmetryOriginIncluded;

		// Token: 0x040070CA RID: 28874
		public bool? symmetryClear;

		// Token: 0x040070CB RID: 28875
		public bool? connectedGroupsSameStuff;

		// Token: 0x040070CC RID: 28876
		public ThingDef assignRandomStuffTo;

		// Token: 0x040070CD RID: 28877
		public ThingDef cornerThing;

		// Token: 0x040070CE RID: 28878
		public bool? floorFillRoomsOnly;

		// Token: 0x040070CF RID: 28879
		public bool? singleFloorType;

		// Token: 0x040070D0 RID: 28880
		public bool? onlyStoneFloors;

		// Token: 0x040070D1 RID: 28881
		public ThingDef thingCentral;

		// Token: 0x040070D2 RID: 28882
		public ThingDef wallEdgeThing;

		// Token: 0x040070D3 RID: 28883
		public IntVec2? monumentSize;

		// Token: 0x040070D4 RID: 28884
		public bool? monumentOpen;

		// Token: 0x040070D5 RID: 28885
		public bool? allowMonumentDoors;

		// Token: 0x040070D6 RID: 28886
		public ThingFilter allowedMonumentThings;

		// Token: 0x040070D7 RID: 28887
		public Map useOnlyStonesAvailableOnMap;

		// Token: 0x040070D8 RID: 28888
		public bool? allowConcrete;

		// Token: 0x040070D9 RID: 28889
		public bool? allowFlammableWalls;

		// Token: 0x040070DA RID: 28890
		public bool? onlyBuildableByPlayer;

		// Token: 0x040070DB RID: 28891
		public bool? addFloors;

		// Token: 0x040070DC RID: 28892
		public bool? requireFloor;

		// Token: 0x040070DD RID: 28893
		public IntVec2? mechClusterSize;

		// Token: 0x040070DE RID: 28894
		public bool? mechClusterDormant;

		// Token: 0x040070DF RID: 28895
		public Map mechClusterForMap;

		// Token: 0x040070E0 RID: 28896
		public bool? forceNoConditionCauser;

		// Token: 0x040070E1 RID: 28897
		private Dictionary<string, object> custom;
	}
}
