using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200021F RID: 543
	public class TemperatureSaveLoad
	{
		// Token: 0x06000F81 RID: 3969 RVA: 0x000581A0 File Offset: 0x000563A0
		public TemperatureSaveLoad(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000581B0 File Offset: 0x000563B0
		public void DoExposeWork()
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				int num = Mathf.RoundToInt(this.map.mapTemperature.OutdoorTemp);
				ushort num2 = this.TempFloatToShort((float)num);
				ushort[] tempGrid = new ushort[this.map.cellIndices.NumGridCells];
				for (int i = 0; i < this.map.cellIndices.NumGridCells; i++)
				{
					tempGrid[i] = num2;
				}
				foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (region.Room != null)
					{
						ushort num3 = this.TempFloatToShort(region.Room.Temperature);
						foreach (IntVec3 c2 in region.Cells)
						{
							tempGrid[this.map.cellIndices.CellToIndex(c2)] = num3;
						}
					}
				}
				arr = MapSerializeUtility.SerializeUshort(this.map, (IntVec3 c) => tempGrid[this.map.cellIndices.CellToIndex(c)]);
			}
			DataExposeUtility.ByteArray(ref arr, "temperatures");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.tempGrid = new ushort[this.map.cellIndices.NumGridCells];
				MapSerializeUtility.LoadUshort(arr, this.map, delegate(IntVec3 c, ushort val)
				{
					this.tempGrid[this.map.cellIndices.CellToIndex(c)] = val;
				});
			}
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00058358 File Offset: 0x00056558
		public void ApplyLoadedDataToRegions()
		{
			if (this.tempGrid != null)
			{
				CellIndices cellIndices = this.map.cellIndices;
				foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (region.Room != null)
					{
						region.Room.Temperature = this.TempShortToFloat(this.tempGrid[cellIndices.CellToIndex(region.Cells.First<IntVec3>())]);
					}
				}
				this.tempGrid = null;
			}
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x000583F4 File Offset: 0x000565F4
		private ushort TempFloatToShort(float temp)
		{
			temp = Mathf.Clamp(temp, -273.15f, 1000f);
			temp *= 16f;
			return (ushort)((int)temp + 32768);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0005841A File Offset: 0x0005661A
		private float TempShortToFloat(ushort temp)
		{
			return ((float)temp - 32768f) / 16f;
		}

		// Token: 0x04000C2C RID: 3116
		private Map map;

		// Token: 0x04000C2D RID: 3117
		private ushort[] tempGrid;
	}
}
