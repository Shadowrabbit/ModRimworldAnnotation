using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B6 RID: 438
	public sealed class TerrainGrid : IExposable
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x000427A4 File Offset: 0x000409A4
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3600, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00042818 File Offset: 0x00040A18
		public TerrainGrid(Map map)
		{
			this.map = map;
			this.ResetGrids();
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0004282D File Offset: 0x00040A2D
		public void ResetGrids()
		{
			this.topGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.underGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00042865 File Offset: 0x00040A65
		public TerrainDef TerrainAt(int ind)
		{
			return this.topGrid[ind];
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0004286F File Offset: 0x00040A6F
		public TerrainDef TerrainAt(IntVec3 c)
		{
			return this.topGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00042889 File Offset: 0x00040A89
		public TerrainDef UnderTerrainAt(int ind)
		{
			return this.underGrid[ind];
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00042893 File Offset: 0x00040A93
		public TerrainDef UnderTerrainAt(IntVec3 c)
		{
			return this.underGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000428B0 File Offset: 0x00040AB0
		public void SetTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (newTerr == null)
			{
				Log.Error("Tried to set terrain at " + c + " to null.");
				return;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
				if (designation != null)
				{
					designation.Delete();
				}
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (newTerr.layerable)
			{
				if (this.underGrid[num] == null)
				{
					if (this.topGrid[num].passability != Traversability.Impassable)
					{
						this.underGrid[num] = this.topGrid[num];
					}
					else
					{
						this.underGrid[num] = TerrainDefOf.Sand;
					}
				}
			}
			else
			{
				this.underGrid[num] = null;
			}
			this.topGrid[num] = newTerr;
			this.DoTerrainChangedEffects(c);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00042974 File Offset: 0x00040B74
		public void SetUnderTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to set terrain out of bounds at " + c);
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			this.underGrid[num] = newTerr;
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000429C0 File Offset: 0x00040BC0
		public void RemoveTopLayer(IntVec3 c, bool doLeavings = true)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (doLeavings)
			{
				GenLeaving.DoLeavingsFor(this.topGrid[num], c, this.map);
			}
			if (this.underGrid[num] != null)
			{
				this.topGrid[num] = this.underGrid[num];
				this.underGrid[num] = null;
				this.DoTerrainChangedEffects(c);
			}
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00042A20 File Offset: 0x00040C20
		public bool CanRemoveTopLayerAt(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			return this.topGrid[num].Removable && this.underGrid[num] != null;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00042A5C File Offset: 0x00040C5C
		private void DoTerrainChangedEffects(IntVec3 c)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Terrain, true, false);
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				if (thingList[i].def.category == ThingCategory.Plant && this.map.fertilityGrid.FertilityAt(c) < thingList[i].def.plant.fertilityMin)
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
				else if (thingList[i].def.category == ThingCategory.Filth && !FilthMaker.TerrainAcceptsFilth(this.TerrainAt(c), thingList[i].def, FilthSourceFlags.None))
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
				else if ((thingList[i].def.IsBlueprint || thingList[i].def.IsFrame) && !GenConstruct.CanBuildOnTerrain(thingList[i].def.entityDefToBuild, thingList[i].Position, this.map, thingList[i].Rotation, null, ((IConstructible)thingList[i]).EntityToBuildStuff()))
				{
					thingList[i].Destroy(DestroyMode.Cancel);
				}
			}
			this.map.pathing.RecalculatePerceivedPathCostAt(c);
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
			this.map.fertilityGrid.Drawer.SetDirty();
			Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c);
			if (regionAt_NoRebuild_InvalidAllowed != null && regionAt_NoRebuild_InvalidAllowed.Room != null)
			{
				regionAt_NoRebuild_InvalidAllowed.Room.Notify_TerrainChanged();
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00042C0D File Offset: 0x00040E0D
		public void ExposeData()
		{
			this.ExposeTerrainGrid(this.topGrid, "topGrid", TerrainDefOf.Soil);
			this.ExposeTerrainGrid(this.underGrid, "underGrid", null);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00042C38 File Offset: 0x00040E38
		public void Notify_TerrainBurned(IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(this.map);
			this.Notify_TerrainDestroyed(c);
			if (terrain.burnedDef != null)
			{
				this.SetTerrain(c, terrain.burnedDef);
			}
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00042C70 File Offset: 0x00040E70
		public void Notify_TerrainDestroyed(IntVec3 c)
		{
			if (!this.CanRemoveTopLayerAt(c))
			{
				return;
			}
			TerrainDef terrainDef = this.TerrainAt(c);
			this.RemoveTopLayer(c, false);
			if (terrainDef.destroyBuildingsOnDestroyed)
			{
				Building firstBuilding = c.GetFirstBuilding(this.map);
				if (firstBuilding != null)
				{
					firstBuilding.Kill(null, null);
				}
			}
			if (terrainDef.destroyEffectWater != null && this.TerrainAt(c) != null && this.TerrainAt(c).IsWater)
			{
				Effecter effecter = terrainDef.destroyEffectWater.Spawn();
				effecter.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false));
				effecter.Cleanup();
			}
			else if (terrainDef.destroyEffect != null)
			{
				Effecter effecter2 = terrainDef.destroyEffect.Spawn();
				effecter2.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false));
				effecter2.Cleanup();
			}
			ThingUtility.CheckAutoRebuildTerrainOnDestroyed(terrainDef, c, this.map);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00042D58 File Offset: 0x00040F58
		private void ExposeTerrainGrid(TerrainDef[] grid, string label, TerrainDef fallbackTerrain)
		{
			Dictionary<ushort, TerrainDef> terrainDefsByShortHash = new Dictionary<ushort, TerrainDef>();
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				terrainDefsByShortHash.Add(terrainDef.shortHash, terrainDef);
			}
			Func<IntVec3, ushort> shortReader = delegate(IntVec3 c)
			{
				TerrainDef terrainDef2 = grid[this.map.cellIndices.CellToIndex(c)];
				if (terrainDef2 == null)
				{
					return 0;
				}
				return terrainDef2.shortHash;
			};
			Action<IntVec3, ushort> shortWriter = delegate(IntVec3 c, ushort val)
			{
				TerrainDef terrainDef2 = terrainDefsByShortHash.TryGetValue(val, null);
				if (terrainDef2 == null && val != 0)
				{
					TerrainDef terrainDef3 = BackCompatibility.BackCompatibleTerrainWithShortHash(val);
					if (terrainDef3 == null)
					{
						Log.Error(string.Concat(new object[]
						{
							"Did not find terrain def with short hash ",
							val,
							" for cell ",
							c,
							"."
						}));
						terrainDef3 = TerrainDefOf.Soil;
					}
					terrainDef2 = terrainDef3;
					terrainDefsByShortHash.Add(val, terrainDef3);
				}
				if (terrainDef2 == null && fallbackTerrain != null)
				{
					Log.ErrorOnce("Replacing missing terrain with " + fallbackTerrain, Gen.HashCombine<ushort>(8388383, fallbackTerrain.shortHash));
					terrainDef2 = fallbackTerrain;
				}
				grid[this.map.cellIndices.CellToIndex(c)] = terrainDef2;
			};
			MapExposeUtility.ExposeUshort(this.map, shortReader, shortWriter, label);
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00042E00 File Offset: 0x00041000
		public string DebugStringAt(IntVec3 c)
		{
			if (c.InBounds(this.map))
			{
				TerrainDef terrain = c.GetTerrain(this.map);
				TerrainDef terrainDef = this.underGrid[this.map.cellIndices.CellToIndex(c)];
				return "top: " + ((terrain != null) ? terrain.defName : "null") + ", under=" + ((terrainDef != null) ? terrainDef.defName : "null");
			}
			return "out of bounds";
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00042E76 File Offset: 0x00041076
		public void TerrainGridUpdate()
		{
			if (Find.PlaySettings.showTerrainAffordanceOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0001A4C7 File Offset: 0x000186C7
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00042E9C File Offset: 0x0004109C
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 c = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			TerrainAffordanceDef terrainAffordanceDef;
			return !c.Filled(this.map) && !c.Fogged(this.map) && this.TryGetAffordanceDefToDraw(this.TerrainAt(index), out terrainAffordanceDef);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00042EF0 File Offset: 0x000410F0
		private bool TryGetAffordanceDefToDraw(TerrainDef terrainDef, out TerrainAffordanceDef affordance)
		{
			if (terrainDef == null || terrainDef.affordances.NullOrEmpty<TerrainAffordanceDef>())
			{
				affordance = null;
				return true;
			}
			TerrainAffordanceDef terrainAffordanceDef = null;
			int num = int.MinValue;
			foreach (TerrainAffordanceDef terrainAffordanceDef2 in terrainDef.affordances)
			{
				if (terrainAffordanceDef2.visualizeOnAffordanceOverlay)
				{
					if (num < terrainAffordanceDef2.order)
					{
						num = terrainAffordanceDef2.order;
						terrainAffordanceDef = terrainAffordanceDef2;
					}
				}
				else if (terrainAffordanceDef2.blockAffordanceOverlay)
				{
					affordance = null;
					return false;
				}
			}
			affordance = terrainAffordanceDef;
			return true;
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00042F8C File Offset: 0x0004118C
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			TerrainAffordanceDef terrainAffordanceDef;
			this.TryGetAffordanceDefToDraw(this.TerrainAt(index), out terrainAffordanceDef);
			if (terrainAffordanceDef != null)
			{
				return terrainAffordanceDef.affordanceOverlayColor;
			}
			return TerrainGrid.NoAffordanceColor;
		}

		// Token: 0x04000A09 RID: 2569
		private Map map;

		// Token: 0x04000A0A RID: 2570
		public TerrainDef[] topGrid;

		// Token: 0x04000A0B RID: 2571
		private TerrainDef[] underGrid;

		// Token: 0x04000A0C RID: 2572
		private CellBoolDrawer drawerInt;

		// Token: 0x04000A0D RID: 2573
		private static readonly Color NoAffordanceColor = Color.red;
	}
}
