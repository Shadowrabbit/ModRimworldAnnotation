using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000659 RID: 1625
	public class AvoidGrid
	{
		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002DDD RID: 11741 RVA: 0x00112FAF File Offset: 0x001111AF
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

		// Token: 0x06002DDE RID: 11742 RVA: 0x00112FC5 File Offset: 0x001111C5
		public AvoidGrid(Map map)
		{
			this.map = map;
			this.grid = new ByteGrid(map);
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x00112FE8 File Offset: 0x001111E8
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

		// Token: 0x06002DE0 RID: 11744 RVA: 0x0011305F File Offset: 0x0011125F
		public void Notify_BuildingSpawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x0011305F File Offset: 0x0011125F
		public void Notify_BuildingDespawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x00113082 File Offset: 0x00111282
		public void DebugDrawOnMap()
		{
			if (DebugViewSettings.drawAvoidGrid && Find.CurrentMap == this.map)
			{
				this.Grid.DebugDraw();
			}
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x001130A4 File Offset: 0x001112A4
		private void PrintAvoidGridAroundTurret(Building_TurretGun tur)
		{
			float range = tur.GunCompEq.PrimaryVerb.verbProps.range;
			float num = tur.GunCompEq.PrimaryVerb.verbProps.EffectiveMinRange(true);
			int num2 = GenRadial.NumCellsInRadius(range + 4f);
			for (int i = (num < 1f) ? 0 : GenRadial.NumCellsInRadius(num); i < num2; i++)
			{
				IntVec3 intVec = tur.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(tur.Map) && intVec.WalkableByNormal(tur.Map) && GenSight.LineOfSight(intVec, tur.Position, tur.Map, true, null, 0, 0))
				{
					this.IncrementAvoidGrid(intVec, 45);
				}
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x0011315C File Offset: 0x0011135C
		private void IncrementAvoidGrid(IntVec3 c, int num)
		{
			byte b = this.grid[c];
			b = (byte)Mathf.Min(255, (int)b + num);
			this.grid[c] = b;
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x00113194 File Offset: 0x00111394
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

		// Token: 0x04001C30 RID: 7216
		public Map map;

		// Token: 0x04001C31 RID: 7217
		private ByteGrid grid;

		// Token: 0x04001C32 RID: 7218
		private bool gridDirty = true;
	}
}
