﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001093 RID: 4243
	public class PlaceWorker_MonumentMarker : PlaceWorker
	{
		// Token: 0x06006537 RID: 25911 RVA: 0x00222FB8 File Offset: 0x002211B8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			MonumentMarker monumentMarker = thing as MonumentMarker;
			if (monumentMarker != null)
			{
				monumentMarker.DrawGhost(center, true, rot);
			}
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x00222FDC File Offset: 0x002211DC
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			MonumentMarker monumentMarker = thing as MonumentMarker;
			if (monumentMarker != null)
			{
				CellRect rect = monumentMarker.sketch.OccupiedRect.MovedBy(loc);
				Blueprint_Install thingToIgnore2 = monumentMarker.FindMyBlueprint(rect, map);
				foreach (SketchEntity sketchEntity in monumentMarker.sketch.Entities)
				{
					CellRect cellRect = sketchEntity.OccupiedRect.MovedBy(loc);
					if (!cellRect.InBounds(map))
					{
						return false;
					}
					if (cellRect.InNoBuildEdgeArea(map))
					{
						return "TooCloseToMapEdge".Translate();
					}
					foreach (IntVec3 intVec in cellRect)
					{
						if (!sketchEntity.CanBuildOnTerrain(intVec, map))
						{
							TerrainDef terrain = intVec.GetTerrain(map);
							return "CannotPlaceMonumentOnTerrain".Translate(terrain.LabelCap);
						}
					}
				}
				PlaceWorker_MonumentMarker.tmpMonumentThings.Clear();
				foreach (SketchBuildable sketchBuildable in monumentMarker.sketch.Buildables)
				{
					Thing spawnedBlueprintOrFrame = sketchBuildable.GetSpawnedBlueprintOrFrame(loc + sketchBuildable.pos, map);
					SketchThing sketchThing;
					if (spawnedBlueprintOrFrame != null)
					{
						PlaceWorker_MonumentMarker.tmpMonumentThings.Add(spawnedBlueprintOrFrame);
					}
					else if ((sketchThing = (sketchBuildable as SketchThing)) != null)
					{
						Thing sameSpawned = sketchThing.GetSameSpawned(loc + sketchThing.pos, map);
						if (sameSpawned != null)
						{
							PlaceWorker_MonumentMarker.tmpMonumentThings.Add(sameSpawned);
						}
					}
				}
				foreach (SketchEntity sketchEntity2 in monumentMarker.sketch.Entities)
				{
					if (!sketchEntity2.IsSameSpawnedOrBlueprintOrFrame(loc + sketchEntity2.pos, map))
					{
						foreach (IntVec3 c in sketchEntity2.OccupiedRect.MovedBy(loc))
						{
							if (c.InBounds(map))
							{
								Building firstBuilding = c.GetFirstBuilding(map);
								if (firstBuilding != null && !PlaceWorker_MonumentMarker.tmpMonumentThings.Contains(firstBuilding))
								{
									PlaceWorker_MonumentMarker.tmpMonumentThings.Clear();
									return "CannotPlaceMonumentOver".Translate(firstBuilding.LabelCap);
								}
							}
						}
						SketchBuildable sketchBuildable2;
						if ((sketchBuildable2 = (sketchEntity2 as SketchBuildable)) != null)
						{
							Thing thing2 = sketchBuildable2.FirstPermanentBlockerAt(loc + sketchEntity2.pos, map);
							if (thing2 != null && !PlaceWorker_MonumentMarker.tmpMonumentThings.Contains(thing2))
							{
								PlaceWorker_MonumentMarker.tmpMonumentThings.Clear();
								return "CannotPlaceMonumentOver".Translate(thing2.LabelCap);
							}
						}
					}
				}
				foreach (SketchEntity sketchEntity3 in monumentMarker.sketch.Entities)
				{
					Building firstAdjacentBuilding = MonumentMarkerUtility.GetFirstAdjacentBuilding(sketchEntity3, loc, PlaceWorker_MonumentMarker.tmpMonumentThings, map);
					if (firstAdjacentBuilding != null)
					{
						return "MonumentAdjacentToBuilding".Translate(firstAdjacentBuilding.LabelCap);
					}
					if (sketchEntity3.IsSpawningBlockedPermanently(loc + sketchEntity3.pos, map, thingToIgnore2, false))
					{
						PlaceWorker_MonumentMarker.tmpMonumentThings.Clear();
						return "MonumentBlockedPermanently".Translate();
					}
				}
				PlaceWorker_MonumentMarker.tmpMonumentThings.Clear();
			}
			return true;
		}

		// Token: 0x040038F7 RID: 14583
		private static List<Thing> tmpMonumentThings = new List<Thing>();
	}
}
