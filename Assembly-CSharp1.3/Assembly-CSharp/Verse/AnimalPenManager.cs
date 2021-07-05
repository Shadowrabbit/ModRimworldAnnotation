using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000174 RID: 372
	public class AnimalPenManager
	{
		// Token: 0x06000A67 RID: 2663 RVA: 0x00038CCF File Offset: 0x00036ECF
		public AnimalPenManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00038D06 File Offset: 0x00036F06
		public void RebuildAllPens()
		{
			this.ForceRebuildPens();
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00038D0E File Offset: 0x00036F0E
		public PenMarkerState GetPenMarkerState(CompAnimalPenMarker marker)
		{
			this.RebuildIfDirty();
			return this.penMarkers[marker];
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00038D24 File Offset: 0x00036F24
		public CompAnimalPenMarker GetPenNamed(string name)
		{
			return this.penMarkers.Keys.FirstOrDefault((CompAnimalPenMarker marker) => marker.label == name);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00038D5C File Offset: 0x00036F5C
		public ThingFilter GetFixedAutoCutFilter()
		{
			if (this.cachedFixedAutoCutFilter == null)
			{
				this.cachedFixedAutoCutFilter = new ThingFilter();
				foreach (ThingDef thingDef in this.map.Biome.AllWildPlants)
				{
					if (thingDef.plant.allowAutoCut)
					{
						this.cachedFixedAutoCutFilter.SetAllow(thingDef, true);
					}
				}
				this.cachedFixedAutoCutFilter.SetAllow(ThingDefOf.BurnedTree, true);
			}
			return this.cachedFixedAutoCutFilter;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00038DF8 File Offset: 0x00036FF8
		public ThingFilter GetDefaultAutoCutFilter()
		{
			if (this.cachedDefaultAutoCutFilter == null)
			{
				this.cachedDefaultAutoCutFilter = new ThingFilter();
				ThingDef plant_Grass = ThingDefOf.Plant_Grass;
				float num = plant_Grass.ingestible.CachedNutrition / plant_Grass.plant.growDays * 0.5f;
				foreach (ThingDef thingDef in this.GetFixedAutoCutFilter().AllowedThingDefs)
				{
					if (!MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(thingDef) || (thingDef.ingestible.CachedNutrition / thingDef.plant.growDays < num && (thingDef.plant.harvestedThingDef == null || !thingDef.plant.harvestedThingDef.IsIngestible)))
					{
						this.cachedDefaultAutoCutFilter.SetAllow(thingDef, true);
					}
				}
			}
			return this.cachedDefaultAutoCutFilter;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00038ED4 File Offset: 0x000370D4
		public void Notify_BuildingSpawned(Building building)
		{
			this.dirty = true;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00038ED4 File Offset: 0x000370D4
		public void Notify_BuildingDespawned(Building building)
		{
			this.dirty = true;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00038EDD File Offset: 0x000370DD
		private void RebuildIfDirty()
		{
			if (this.dirty)
			{
				this.ForceRebuildPens();
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00038EF0 File Offset: 0x000370F0
		private void ForceRebuildPens()
		{
			this.dirty = false;
			this.penMarkers.Clear();
			foreach (Building thing in this.map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker = thing.TryGetComp<CompAnimalPenMarker>();
				this.penMarkers.Add(compAnimalPenMarker, new PenMarkerState(compAnimalPenMarker));
			}
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00038F70 File Offset: 0x00037170
		public string MakeNewAnimalPenName()
		{
			this.existingPenNames.Clear();
			this.existingPenNames.AddRange(from marker in this.penMarkers.Keys
			select marker.label);
			int num = 1;
			string text;
			for (;;)
			{
				text = "AnimalPenMarkerDefaultLabel".Translate(num);
				if (!this.existingPenNames.Contains(text))
				{
					break;
				}
				num++;
			}
			this.existingPenNames.Clear();
			return text;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00038FFC File Offset: 0x000371FC
		public void DrawPlacingMouseAttachments(BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			if (((thingDef != null) ? thingDef.CompDefFor<CompAnimalPenMarker>() : null) == null)
			{
				return;
			}
			IntVec3 intVec = UI.MouseCell();
			if (!intVec.InBounds(this.map))
			{
				return;
			}
			if (this.cached_placingPenFoodCalculator_forPosition == null || intVec != this.cached_placingPenFoodCalculator_forPosition)
			{
				this.cached_placingPenFoodCalculator.ResetAndProcessPen(intVec, this.map, true);
				this.cached_placingPenFoodCalculator_forPosition = new IntVec3?(intVec);
			}
			AnimalPenGUI.DrawPlacingMouseAttachments(intVec, this.map, this.cached_placingPenFoodCalculator);
		}

		// Token: 0x040008E0 RID: 2272
		private readonly Map map;

		// Token: 0x040008E1 RID: 2273
		private Dictionary<CompAnimalPenMarker, PenMarkerState> penMarkers = new Dictionary<CompAnimalPenMarker, PenMarkerState>();

		// Token: 0x040008E2 RID: 2274
		private bool dirty = true;

		// Token: 0x040008E3 RID: 2275
		private ThingFilter cachedDefaultAutoCutFilter;

		// Token: 0x040008E4 RID: 2276
		private ThingFilter cachedFixedAutoCutFilter;

		// Token: 0x040008E5 RID: 2277
		private HashSet<string> existingPenNames = new HashSet<string>();

		// Token: 0x040008E6 RID: 2278
		private readonly PenFoodCalculator cached_placingPenFoodCalculator = new PenFoodCalculator();

		// Token: 0x040008E7 RID: 2279
		private IntVec3? cached_placingPenFoodCalculator_forPosition;
	}
}
