using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F8 RID: 4856
	public class PowerNetGrid
	{
		// Token: 0x06006963 RID: 26979 RVA: 0x00047D8A File Offset: 0x00045F8A
		public PowerNetGrid(Map map)
		{
			this.map = map;
			this.netGrid = new PowerNet[map.cellIndices.NumGridCells];
		}

		// Token: 0x06006964 RID: 26980 RVA: 0x00047DBA File Offset: 0x00045FBA
		public PowerNet TransmittedPowerNetAt(IntVec3 c)
		{
			return this.netGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06006965 RID: 26981 RVA: 0x00207574 File Offset: 0x00205774
		public void Notify_PowerNetCreated(PowerNet newNet)
		{
			if (this.powerNetCells.ContainsKey(newNet))
			{
				Log.Warning("Net " + newNet + " is already registered in PowerNetGrid.", false);
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
							}), false);
						}
						this.netGrid[num] = newNet;
						list.Add(new IntVec3(k, 0, j));
					}
				}
			}
		}

		// Token: 0x06006966 RID: 26982 RVA: 0x00207714 File Offset: 0x00205914
		public void Notify_PowerNetDeleted(PowerNet deadNet)
		{
			List<IntVec3> list;
			if (!this.powerNetCells.TryGetValue(deadNet, out list))
			{
				Log.Warning("Net " + deadNet + " does not exist in PowerNetGrid's dictionary.", false);
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
					Log.Warning("Multiple nets on the same cell " + list[i] + ". This is probably a result of an earlier error.", false);
				}
			}
			this.powerNetCells.Remove(deadNet);
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x002077B8 File Offset: 0x002059B8
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

		// Token: 0x04004634 RID: 17972
		private Map map;

		// Token: 0x04004635 RID: 17973
		private PowerNet[] netGrid;

		// Token: 0x04004636 RID: 17974
		private Dictionary<PowerNet, List<IntVec3>> powerNetCells = new Dictionary<PowerNet, List<IntVec3>>();
	}
}
