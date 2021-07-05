using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001CD RID: 461
	public sealed class MapDrawer
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00047B0C File Offset: 0x00045D0C
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

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00047B68 File Offset: 0x00045D68
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

		// Token: 0x06000D4C RID: 3404 RVA: 0x00047BF1 File Offset: 0x00045DF1
		public MapDrawer(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00047C00 File Offset: 0x00045E00
		public void MapMeshDirty(IntVec3 loc, MapMeshFlag dirtyFlags)
		{
			bool regenAdjacentCells = (dirtyFlags & (MapMeshFlag.FogOfWar | MapMeshFlag.Buildings)) > MapMeshFlag.None;
			bool regenAdjacentSections = (dirtyFlags & MapMeshFlag.GroundGlow) > MapMeshFlag.None;
			this.MapMeshDirty(loc, dirtyFlags, regenAdjacentCells, regenAdjacentSections);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00047C28 File Offset: 0x00045E28
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

		// Token: 0x06000D4F RID: 3407 RVA: 0x00047D44 File Offset: 0x00045F44
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

		// Token: 0x06000D50 RID: 3408 RVA: 0x00047E0C File Offset: 0x0004600C
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

		// Token: 0x06000D51 RID: 3409 RVA: 0x00047E60 File Offset: 0x00046060
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

		// Token: 0x06000D52 RID: 3410 RVA: 0x00047F08 File Offset: 0x00046108
		private IntVec2 SectionCoordsAt(IntVec3 loc)
		{
			return new IntVec2(Mathf.FloorToInt((float)(loc.x / 17)), Mathf.FloorToInt((float)(loc.z / 17)));
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00047F30 File Offset: 0x00046130
		public Section SectionAt(IntVec3 loc)
		{
			IntVec2 intVec = this.SectionCoordsAt(loc);
			return this.sections[intVec.x, intVec.z];
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00047F5C File Offset: 0x0004615C
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

		// Token: 0x06000D55 RID: 3413 RVA: 0x00048000 File Offset: 0x00046200
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

		// Token: 0x06000D56 RID: 3414 RVA: 0x00048054 File Offset: 0x00046254
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

		// Token: 0x04000AFD RID: 2813
		private Map map;

		// Token: 0x04000AFE RID: 2814
		private Section[,] sections;
	}
}
