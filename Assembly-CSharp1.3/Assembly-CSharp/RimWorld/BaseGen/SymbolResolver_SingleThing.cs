using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015C8 RID: 5576
	public class SymbolResolver_SingleThing : SymbolResolver
	{
		// Token: 0x06008347 RID: 33607 RVA: 0x002EC19C File Offset: 0x002EA39C
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.Spawned)
			{
				return true;
			}
			IntVec3 intVec;
			if (rp.singleThingToSpawn is Pawn)
			{
				ResolveParams rp2 = rp;
				rp2.singlePawnToSpawn = (Pawn)rp.singleThingToSpawn;
				if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp2, out intVec))
				{
					return false;
				}
			}
			return ((rp.singleThingDef == null || rp.singleThingDef.category != ThingCategory.Item) && (rp.singleThingToSpawn == null || rp.singleThingToSpawn.def.category != ThingCategory.Item)) || this.TryFindSpawnCellForItem(rp.rect, out intVec, false);
		}

		// Token: 0x06008348 RID: 33608 RVA: 0x002EC240 File Offset: 0x002EA440
		public override void Resolve(ResolveParams rp)
		{
			if (rp.singleThingToSpawn is Pawn)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = (Pawn)rp.singleThingToSpawn;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
				return;
			}
			if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.Spawned)
			{
				return;
			}
			ThingDef thingDef2;
			if (rp.singleThingToSpawn == null)
			{
				ThingDef thingDef;
				if ((thingDef = rp.singleThingDef) == null)
				{
					thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
					where x.IsWeapon || x.IsMedicine || x.IsDrug
					select x).RandomElement<ThingDef>();
				}
				thingDef2 = thingDef;
			}
			else
			{
				thingDef2 = rp.singleThingToSpawn.def;
			}
			Rot4? thingRot = rp.thingRot;
			IntVec3 intVec;
			if (thingDef2.category == ThingCategory.Item)
			{
				thingRot = new Rot4?(Rot4.North);
				if (!this.TryFindSpawnCellForItem(rp.rect, out intVec, rp.spawnOutside ?? false))
				{
					if (rp.singleThingToSpawn != null)
					{
						rp.singleThingToSpawn.Destroy(DestroyMode.Vanish);
					}
					return;
				}
			}
			else
			{
				bool flag;
				bool flag2;
				intVec = this.FindBestSpawnCellForNonItem(rp.rect, thingDef2, ref thingRot, out flag, out flag2, rp.spawnOutside ?? false);
				if ((flag || flag2) && rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit != null && rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit.Value)
				{
					return;
				}
			}
			if (thingRot == null)
			{
				Log.Error("Could not resolve rotation. Bug.");
			}
			Thing thing;
			if (rp.singleThingToSpawn == null)
			{
				ThingDef stuff;
				if (rp.singleThingStuff != null && rp.singleThingStuff.stuffProps.CanMake(thingDef2))
				{
					stuff = rp.singleThingStuff;
				}
				else if (thingDef2.building != null)
				{
					stuff = BaseGenUtility.CheapStuffFor(thingDef2, rp.faction);
				}
				else
				{
					stuff = GenStuff.RandomStuffInexpensiveFor(thingDef2, rp.faction, null);
				}
				thing = ThingMaker.MakeThing(thingDef2, stuff);
				thing.stackCount = (rp.singleThingStackCount ?? 1);
				if (thing.stackCount <= 0)
				{
					thing.stackCount = 1;
				}
				if (thing.def.CanHaveFaction && thing.Faction != rp.faction)
				{
					thing.SetFaction(rp.faction, null);
					Thing thing2 = thing;
					FactionIdeosTracker ideos = rp.faction.ideos;
					ThingStyleDef styleDef;
					if (ideos == null)
					{
						styleDef = null;
					}
					else
					{
						Ideo primaryIdeo = ideos.PrimaryIdeo;
						styleDef = ((primaryIdeo != null) ? primaryIdeo.GetStyleFor(thing.def) : null);
					}
					thing2.SetStyleDef(styleDef);
				}
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					compQuality.SetQuality(QualityUtility.GenerateQualityBaseGen(), ArtGenerationContext.Outsider);
				}
				if (rp.postThingGenerate != null)
				{
					rp.postThingGenerate(thing);
				}
			}
			else
			{
				thing = rp.singleThingToSpawn;
			}
			if (rp.spawnBridgeIfTerrainCantSupportThing == null || rp.spawnBridgeIfTerrainCantSupportThing.Value)
			{
				BaseGenUtility.CheckSpawnBridgeUnder(thing.def, intVec, thingRot.Value);
			}
			thing = GenSpawn.Spawn(thing, intVec, BaseGen.globalSettings.map, thingRot.Value, WipeMode.Vanish, false);
			if (thing != null && rp.singleThingInnerThings != null)
			{
				CompThingContainer compThingContainer = thing.TryGetComp<CompThingContainer>();
				if (compThingContainer != null)
				{
					using (List<Thing>.Enumerator enumerator = rp.singleThingInnerThings.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Thing thing3 = enumerator.Current;
							if (compThingContainer.Accepts(thing3))
							{
								compThingContainer.innerContainer.TryAdd(thing3, true);
							}
						}
						goto IL_37D;
					}
				}
				Building_Casket building_Casket;
				if ((building_Casket = (thing as Building_Casket)) != null)
				{
					ThingOwner directlyHeldThings = building_Casket.GetDirectlyHeldThings();
					foreach (Thing item in rp.singleThingInnerThings)
					{
						directlyHeldThings.TryAdd(item, true);
					}
				}
			}
			IL_37D:
			if (thing != null && thing.def.category == ThingCategory.Item)
			{
				thing.SetForbidden(true, false);
			}
			if (rp.postThingSpawn != null)
			{
				rp.postThingSpawn(thing);
			}
		}

		// Token: 0x06008349 RID: 33609 RVA: 0x002EC614 File Offset: 0x002EA814
		private bool TryFindSpawnCellForItem(CellRect rect, out IntVec3 result, bool spawnOutside = false)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
			{
				if (c.GetFirstItem(map) != null)
				{
					return false;
				}
				if (!c.Standable(map))
				{
					SurfaceType surfaceType = c.GetSurfaceType(map);
					if (surfaceType != SurfaceType.Item && surfaceType != SurfaceType.Eat)
					{
						return false;
					}
				}
				return !spawnOutside || !c.Roofed(map);
			}, out result);
		}

		// Token: 0x0600834A RID: 33610 RVA: 0x002EC654 File Offset: 0x002EA854
		private IntVec3 FindBestSpawnCellForNonItem(CellRect rect, ThingDef thingDef, ref Rot4? rot, out bool hasToWipeBuilding, out bool doesntFit, bool spawnOutside = false)
		{
			if (!thingDef.rotatable)
			{
				rot = new Rot4?(Rot4.North);
			}
			if (rot == null)
			{
				SymbolResolver_SingleThing.tmpRotations.Shuffle<Rot4>();
				for (int i = 0; i < SymbolResolver_SingleThing.tmpRotations.Length; i++)
				{
					IntVec3 result = this.FindBestSpawnCellForNonItem(rect, thingDef, SymbolResolver_SingleThing.tmpRotations[i], out hasToWipeBuilding, out doesntFit, spawnOutside);
					if (!hasToWipeBuilding && !doesntFit)
					{
						rot = new Rot4?(SymbolResolver_SingleThing.tmpRotations[i]);
						return result;
					}
				}
				for (int j = 0; j < SymbolResolver_SingleThing.tmpRotations.Length; j++)
				{
					IntVec3 result2 = this.FindBestSpawnCellForNonItem(rect, thingDef, SymbolResolver_SingleThing.tmpRotations[j], out hasToWipeBuilding, out doesntFit, spawnOutside);
					if (!doesntFit)
					{
						rot = new Rot4?(SymbolResolver_SingleThing.tmpRotations[j]);
						return result2;
					}
				}
				rot = new Rot4?(Rot4.Random);
				return this.FindBestSpawnCellForNonItem(rect, thingDef, rot.Value, out hasToWipeBuilding, out doesntFit, spawnOutside);
			}
			return this.FindBestSpawnCellForNonItem(rect, thingDef, rot.Value, out hasToWipeBuilding, out doesntFit, spawnOutside);
		}

		// Token: 0x0600834B RID: 33611 RVA: 0x002EC760 File Offset: 0x002EA960
		private IntVec3 FindBestSpawnCellForNonItem(CellRect rect, ThingDef thingDef, Rot4 rot, out bool hasToWipeBuilding, out bool doesntFit, bool spawnOutside = false)
		{
			Map map = BaseGen.globalSettings.map;
			if (thingDef.category == ThingCategory.Building)
			{
				foreach (IntVec3 intVec in rect.Cells.InRandomOrder(null))
				{
					CellRect rect2 = GenAdj.OccupiedRect(intVec, rot, thingDef.size);
					if (rect2.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(rect2, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect2) && GenConstruct.TerrainCanSupport(rect2, map, thingDef) && (!spawnOutside || !intVec.Roofed(map)) && !this.InteractionCellBlocked(thingDef, intVec, rot, map))
					{
						hasToWipeBuilding = false;
						doesntFit = false;
						return intVec;
					}
				}
				foreach (IntVec3 intVec2 in rect.Cells.InRandomOrder(null))
				{
					CellRect rect3 = GenAdj.OccupiedRect(intVec2, rot, thingDef.size);
					if (rect3.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(rect3, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect3) && (!spawnOutside || !intVec2.Roofed(map)) && !this.InteractionCellBlocked(thingDef, intVec2, rot, map))
					{
						hasToWipeBuilding = false;
						doesntFit = false;
						return intVec2;
					}
				}
			}
			foreach (IntVec3 intVec3 in rect.Cells.InRandomOrder(null))
			{
				CellRect rect4 = GenAdj.OccupiedRect(intVec3, rot, thingDef.size);
				if (rect4.FullyContainedWithin(rect) && !this.AnyNonStandableCellOrAnyBuildingInside(rect4) && (!spawnOutside || !intVec3.Roofed(map)) && !this.InteractionCellBlocked(thingDef, intVec3, rot, map))
				{
					hasToWipeBuilding = false;
					doesntFit = false;
					return intVec3;
				}
			}
			foreach (IntVec3 intVec4 in rect.Cells.InRandomOrder(null))
			{
				if (GenAdj.OccupiedRect(intVec4, rot, thingDef.size).FullyContainedWithin(rect) && (!spawnOutside || !intVec4.Roofed(map)))
				{
					hasToWipeBuilding = true;
					doesntFit = false;
					return intVec4;
				}
			}
			IntVec3 centerCell = rect.CenterCell;
			CellRect cellRect = GenAdj.OccupiedRect(centerCell, rot, thingDef.size);
			if (cellRect.minX < 0)
			{
				centerCell.x += -cellRect.minX;
			}
			if (cellRect.minZ < 0)
			{
				centerCell.z += -cellRect.minZ;
			}
			if (cellRect.maxX >= map.Size.x)
			{
				centerCell.x -= cellRect.maxX - map.Size.x + 1;
			}
			if (cellRect.maxZ >= map.Size.z)
			{
				centerCell.z -= cellRect.maxZ - map.Size.z + 1;
			}
			hasToWipeBuilding = true;
			doesntFit = true;
			return centerCell;
		}

		// Token: 0x0600834C RID: 33612 RVA: 0x002ECA80 File Offset: 0x002EAC80
		private bool AnyNonStandableCellOrAnyBuildingInside(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			foreach (IntVec3 c in rect)
			{
				if (!c.Standable(map) || c.GetEdifice(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600834D RID: 33613 RVA: 0x002ECAEC File Offset: 0x002EACEC
		private bool InteractionCellBlocked(ThingDef thingDef, IntVec3 cell, Rot4 rot, Map map)
		{
			return thingDef.hasInteractionCell && !ThingUtility.InteractionCellWhenAt(thingDef, cell, rot, map).Standable(map);
		}

		// Token: 0x04005201 RID: 20993
		private static Rot4[] tmpRotations = new Rot4[]
		{
			Rot4.North,
			Rot4.South,
			Rot4.West,
			Rot4.East
		};
	}
}
