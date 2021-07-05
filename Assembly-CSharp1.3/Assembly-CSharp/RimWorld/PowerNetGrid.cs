using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD8 RID: 3288
	public class PowerNetGrid
	{
		// Token: 0x06004CBA RID: 19642 RVA: 0x001995B0 File Offset: 0x001977B0
		public PowerNetGrid(Map map)
		{
			this.map = map;
			this.netGrid = new PowerNet[map.cellIndices.NumGridCells];
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x001995E0 File Offset: 0x001977E0
		public PowerNet TransmittedPowerNetAt(IntVec3 c)
		{
			return this.netGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x001995FC File Offset: 0x001977FC
		public void Notify_PowerNetCreated(PowerNet newNet)
		{
			if (this.powerNetCells.ContainsKey(newNet))
			{
				Log.Warning("Net " + newNet + " is already registered in PowerNetGrid.");
				this.powerNetCells.Remove(newNet);
			}
			List<IntVec3> list = new List<IntVec3>();
			this.powerNetCells.Add(newNet, list);
			for (int i = 0; i < newNet.transmitters.Count; i++)
			{
				CellRect cellRect = newNet.transmitters[i].parent.OccupiedRect();
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					for (int k = cellRect.minX; k <= cellRect.maxX; k++)
					{
						int num = this.map.cellIndices.CellToIndex(k, j);
						if (this.netGrid[num] != null)
						{
							Log.Warning(string.Concat(new object[]
							{
								"Two power nets on the same cell (",
								k,
								", ",
								j,
								"). First transmitters: ",
								newNet.transmitters[0].parent.LabelCap,
								" and ",
								this.netGrid[num].transmitters.NullOrEmpty<CompPower>() ? "[none]" : this.netGrid[num].transmitters[0].parent.LabelCap,
								"."
							}));
						}
						this.netGrid[num] = newNet;
						list.Add(new IntVec3(k, 0, j));
					}
				}
			}
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x0019979C File Offset: 0x0019799C
		public void Notify_PowerNetDeleted(PowerNet deadNet)
		{
			List<IntVec3> list;
			if (!this.powerNetCells.TryGetValue(deadNet, out list))
			{
				Log.Warning("Net " + deadNet + " does not exist in PowerNetGrid's dictionary.");
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				int num = this.map.cellIndices.CellToIndex(list[i]);
				if (this.netGrid[num] == deadNet)
				{
					this.netGrid[num] = null;
				}
				else
				{
					Log.Warning("Multiple nets on the same cell " + list[i] + ". This is probably a result of an earlier error.");
				}
			}
			this.powerNetCells.Remove(deadNet);
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x0019983C File Offset: 0x00197A3C
		public void DrawDebugPowerNetGrid()
		{
			if (!DebugViewSettings.drawPowerNetGrid)
			{
				return;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (this.map != Find.CurrentMap)
			{
				return;
			}
			Rand.PushState();
			foreach (IntVec3 c in Find.CameraDriver.CurrentViewRect.ClipInsideMap(this.map))
			{
				PowerNet powerNet = this.netGrid[this.map.cellIndices.CellToIndex(c)];
				if (powerNet != null)
				{
					Rand.Seed = powerNet.GetHashCode();
					CellRenderer.RenderCell(c, Rand.Value);
				}
			}
			Rand.PopState();
		}

		// Token: 0x04002E76 RID: 11894
		private Map map;

		// Token: 0x04002E77 RID: 11895
		private PowerNet[] netGrid;

		// Token: 0x04002E78 RID: 11896
		private Dictionary<PowerNet, List<IntVec3>> powerNetCells = new Dictionary<PowerNet, List<IntVec3>>();
	}
}
