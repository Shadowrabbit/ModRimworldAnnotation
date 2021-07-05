using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001556 RID: 5462
	public class PlaceWorker_WatermillGenerator : PlaceWorker
	{
		// Token: 0x0600818A RID: 33162 RVA: 0x002DCD8C File Offset: 0x002DAF8C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in CompPowerPlantWater.GroundCells(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.Heavy))
				{
					return new AcceptanceReport("TerrainCannotSupport_TerrainAffordance".Translate(checkingDef, TerrainAffordanceDefOf.Heavy));
				}
			}
			if (!this.WaterCellsPresent(loc, rot, map))
			{
				return new AcceptanceReport("MustBeOnMovingWater".Translate());
			}
			return true;
		}

		// Token: 0x0600818B RID: 33163 RVA: 0x002DCE44 File Offset: 0x002DB044
		private bool WaterCellsPresent(IntVec3 loc, Rot4 rot, Map map)
		{
			foreach (IntVec3 c in CompPowerPlantWater.WaterCells(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600818C RID: 33164 RVA: 0x002DCEB0 File Offset: 0x002DB0B0
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(CompPowerPlantWater.GroundCells(loc, rot).ToList<IntVec3>(), Color.white, null);
			Color color = this.WaterCellsPresent(loc, rot, Find.CurrentMap) ? Designator_Place.CanPlaceColor.ToOpaque() : Designator_Place.CannotPlaceColor.ToOpaque();
			GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterCells(loc, rot).ToList<IntVec3>(), color, null);
			bool flag = false;
			CellRect cellRect = CompPowerPlantWater.WaterUseRect(loc, rot);
			PlaceWorker_WatermillGenerator.waterMills.AddRange(Find.CurrentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator).Cast<Thing>());
			PlaceWorker_WatermillGenerator.waterMills.AddRange(from t in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint)
			where t.def.entityDefToBuild == ThingDefOf.WatermillGenerator
			select t);
			PlaceWorker_WatermillGenerator.waterMills.AddRange(from t in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame)
			where t.def.entityDefToBuild == ThingDefOf.WatermillGenerator
			select t);
			foreach (Thing thing2 in PlaceWorker_WatermillGenerator.waterMills)
			{
				GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterUseCells(thing2.Position, thing2.Rotation).ToList<IntVec3>(), new Color(0.2f, 0.2f, 1f), null);
				if (cellRect.Overlaps(CompPowerPlantWater.WaterUseRect(thing2.Position, thing2.Rotation)))
				{
					flag = true;
				}
			}
			PlaceWorker_WatermillGenerator.waterMills.Clear();
			Color color2 = flag ? new Color(1f, 0.6f, 0f) : Designator_Place.CanPlaceColor.ToOpaque();
			if (!flag || Time.realtimeSinceStartup % 0.4f < 0.2f)
			{
				GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterUseCells(loc, rot).ToList<IntVec3>(), color2, null);
			}
		}

		// Token: 0x0600818D RID: 33165 RVA: 0x002DD0C0 File Offset: 0x002DB2C0
		public override IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield return TerrainAffordanceDefOf.Heavy;
			yield return TerrainAffordanceDefOf.MovingFluid;
			yield break;
		}

		// Token: 0x040050AF RID: 20655
		private static List<Thing> waterMills = new List<Thing>();
	}
}
