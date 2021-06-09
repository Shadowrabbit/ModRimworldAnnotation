using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200028E RID: 654
	public sealed class MapDrawer
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x000BCDC8 File Offset: 0x000BAFC8
		private IntVec2 SectionCount
		{
			get
			{
				return new IntVec2
				{
					x = Mathf.CeilToInt((float)this.map.Size.x / 17f),
					z = Mathf.CeilToInt((float)this.map.Size.z / 17f)
				};
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x000BCE24 File Offset: 0x000BB024
		private CellRect VisibleSections
		{
			get
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				CellRect sunShadowsViewRect = this.GetSunShadowsViewRect(currentViewRect);
				sunShadowsViewRect.ClipInsideMap(this.map);
				IntVec2 intVec = this.SectionCoordsAt(sunShadowsViewRect.BottomLeft);
				IntVec2 intVec2 = this.SectionCoordsAt(sunShadowsViewRect.TopRight);
				if (intVec2.x < intVec.x || intVec2.z < intVec.z)
				{
					return CellRect.Empty;
				}
				return CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			}
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0001279C File Offset: 0x0001099C
		public MapDrawer(Map map)
		{
			this.map = map;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x000BCEB0 File Offset: 0x000BB0B0
		public void MapMeshDirty(IntVec3 loc, MapMeshFlag dirtyFlags)
		{
			bool regenAdjacentCells = (dirtyFlags & (MapMeshFlag.FogOfWar | MapMeshFlag.Buildings)) > MapMeshFlag.None;
			bool regenAdjacentSections = (dirtyFlags & MapMeshFlag.GroundGlow) > MapMeshFlag.None;
			this.MapMeshDirty(loc, dirtyFlags, regenAdjacentCells, regenAdjacentSections);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000BCED8 File Offset: 0x000BB0D8
		public void MapMeshDirty(IntVec3 loc, MapMeshFlag dirtyFlags, bool regenAdjacentCells, bool regenAdjacentSections)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			this.SectionAt(loc).dirtyFlags |= dirtyFlags;
			if (regenAdjacentCells)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = loc + GenAdj.AdjacentCells[i];
					if (intVec.InBounds(this.map))
					{
						this.SectionAt(intVec).dirtyFlags |= dirtyFlags;
					}
				}
			}
			if (regenAdjacentSections)
			{
				IntVec2 a = this.SectionCoordsAt(loc);
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = GenAdj.AdjacentCells[j];
					IntVec2 intVec3 = a + new IntVec2(intVec2.x, intVec2.z);
					IntVec2 sectionCount = this.SectionCount;
					if (intVec3.x >= 0 && intVec3.z >= 0 && intVec3.x <= sectionCount.x - 1 && intVec3.z <= sectionCount.z - 1)
					{
						this.sections[intVec3.x, intVec3.z].dirtyFlags |= dirtyFlags;
					}
				}
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000BCFF4 File Offset: 0x000BB1F4
		public void MapMeshDrawerUpdate_First()
		{
			CellRect visibleSections = this.VisibleSections;
			bool flag = false;
			foreach (IntVec3 intVec in visibleSections)
			{
				Section sect = this.sections[intVec.x, intVec.z];
				if (this.TryUpdateSection(sect))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				for (int i = 0; i < this.SectionCount.x; i++)
				{
					for (int j = 0; j < this.SectionCount.z; j++)
					{
						if (this.TryUpdateSection(this.sections[i, j]))
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x000BD0BC File Offset: 0x000BB2BC
		private bool TryUpdateSection(Section sect)
		{
			if (sect.dirtyFlags == MapMeshFlag.None)
			{
				return false;
			}
			for (int i = 0; i < MapMeshFlagUtility.allFlags.Count; i++)
			{
				MapMeshFlag mapMeshFlag = MapMeshFlagUtility.allFlags[i];
				if ((sect.dirtyFlags & mapMeshFlag) != MapMeshFlag.None)
				{
					sect.RegenerateLayers(mapMeshFlag);
				}
			}
			sect.dirtyFlags = MapMeshFlag.None;
			return true;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x000BD110 File Offset: 0x000BB310
		public void DrawMapMesh()
		{
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			currentViewRect.minX -= 17;
			currentViewRect.minZ -= 17;
			foreach (IntVec3 intVec in this.VisibleSections)
			{
				Section section = this.sections[intVec.x, intVec.z];
				section.DrawSection(!currentViewRect.Contains(section.botLeft));
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x000127AB File Offset: 0x000109AB
		private IntVec2 SectionCoordsAt(IntVec3 loc)
		{
			return new IntVec2(Mathf.FloorToInt((float)(loc.x / 17)), Mathf.FloorToInt((float)(loc.z / 17)));
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x000BD1B8 File Offset: 0x000BB3B8
		public Section SectionAt(IntVec3 loc)
		{
			IntVec2 intVec = this.SectionCoordsAt(loc);
			return this.sections[intVec.x, intVec.z];
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000BD1E4 File Offset: 0x000BB3E4
		public void RegenerateEverythingNow()
		{
			if (this.sections == null)
			{
				this.sections = new Section[this.SectionCount.x, this.SectionCount.z];
			}
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					if (this.sections[i, j] == null)
					{
						this.sections[i, j] = new Section(new IntVec3(i, 0, j), this.map);
					}
					this.sections[i, j].RegenerateAllLayers();
				}
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000BD288 File Offset: 0x000BB488
		public void WholeMapChanged(MapMeshFlag change)
		{
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					this.sections[i, j].dirtyFlags |= change;
				}
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000BD2DC File Offset: 0x000BB4DC
		private CellRect GetSunShadowsViewRect(CellRect rect)
		{
			GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.Shadow);
			if (lightSourceInfo.vector.x < 0f)
			{
				rect.maxX -= Mathf.FloorToInt(lightSourceInfo.vector.x);
			}
			else
			{
				rect.minX -= Mathf.CeilToInt(lightSourceInfo.vector.x);
			}
			if (lightSourceInfo.vector.y < 0f)
			{
				rect.maxZ -= Mathf.FloorToInt(lightSourceInfo.vector.y);
			}
			else
			{
				rect.minZ -= Mathf.CeilToInt(lightSourceInfo.vector.y);
			}
			return rect;
		}

		// Token: 0x04000DDB RID: 3547
		private Map map;

		// Token: 0x04000DDC RID: 3548
		private Section[,] sections;
	}
}
