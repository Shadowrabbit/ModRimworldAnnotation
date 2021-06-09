using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000AC0 RID: 2752
	public class AvoidGrid
	{
		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060040E7 RID: 16615 RVA: 0x00030817 File Offset: 0x0002EA17
		public ByteGrid Grid
		{
			get
			{
				if (this.gridDirty)
				{
					this.Regenerate();
				}
				return this.grid;
			}
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x0003082D File Offset: 0x0002EA2D
		public AvoidGrid(Map map)
		{
			this.map = map;
			this.grid = new ByteGrid(map);
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x00185884 File Offset: 0x00183A84
		public void Regenerate()
		{
			this.gridDirty = false;
			this.grid.Clear(0);
			List<Building> allBuildingsColonist = this.map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				if (allBuildingsColonist[i].def.building.ai_combatDangerous)
				{
					Building_TurretGun building_TurretGun = allBuildingsColonist[i] as Building_TurretGun;
					if (building_TurretGun != null)
					{
						this.PrintAvoidGridAroundTurret(building_TurretGun);
					}
				}
			}
			this.ExpandAvoidGridIntoEdifices();
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x0003084F File Offset: 0x0002EA4F
		public void Notify_BuildingSpawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x0003084F File Offset: 0x0002EA4F
		public void Notify_BuildingDespawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x00030872 File Offset: 0x0002EA72
		public void DebugDrawOnMap()
		{
			if (DebugViewSettings.drawAvoidGrid && Find.CurrentMap == this.map)
			{
				this.Grid.DebugDraw();
			}
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x001858FC File Offset: 0x00183AFC
		private void PrintAvoidGridAroundTurret(Building_TurretGun tur)
		{
			float range = tur.GunCompEq.PrimaryVerb.verbProps.range;
			float num = tur.GunCompEq.PrimaryVerb.verbProps.EffectiveMinRange(true);
			int num2 = GenRadial.NumCellsInRadius(range + 4f);
			for (int i = (num < 1f) ? 0 : GenRadial.NumCellsInRadius(num); i < num2; i++)
			{
				IntVec3 intVec = tur.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(tur.Map) && intVec.Walkable(tur.Map) && GenSight.LineOfSight(intVec, tur.Position, tur.Map, true, null, 0, 0))
				{
					this.IncrementAvoidGrid(intVec, 45);
				}
			}
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x001859B4 File Offset: 0x00183BB4
		private void IncrementAvoidGrid(IntVec3 c, int num)
		{
			byte b = this.grid[c];
			b = (byte)Mathf.Min(255, (int)b + num);
			this.grid[c] = b;
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x001859EC File Offset: 0x00183BEC
		private void ExpandAvoidGridIntoEdifices()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				if (this.grid[i] != 0 && this.map.edificeGrid[i] == null)
				{
					for (int j = 0; j < 8; j++)
					{
						IntVec3 c = this.map.cellIndices.IndexToCell(i) + GenAdj.AdjacentCells[j];
						if (c.InBounds(this.map) && c.GetEdifice(this.map) != null)
						{
							this.grid[c] = (byte)Mathf.Min(255, Mathf.Max((int)this.grid[c], (int)this.grid[i]));
						}
					}
				}
			}
		}

		// Token: 0x04002CDA RID: 11482
		public Map map;

		// Token: 0x04002CDB RID: 11483
		private ByteGrid grid;

		// Token: 0x04002CDC RID: 11484
		private bool gridDirty = true;
	}
}
