using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F5 RID: 757
	public sealed class RoomGroupTempTracker
	{
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x00013E45 File Offset: 0x00012045
		private Map Map
		{
			get
			{
				return this.roomGroup.Map;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x00013E52 File Offset: 0x00012052
		private float ThinRoofCoverage
		{
			get
			{
				return 1f - (this.thickRoofCoverage + this.noRoofCoverage);
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x00013E67 File Offset: 0x00012067
		public List<IntVec3> EqualizeCellsForReading
		{
			get
			{
				return this.equalizeCells;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x00013E6F File Offset: 0x0001206F
		// (set) Token: 0x06001374 RID: 4980 RVA: 0x00013E77 File Offset: 0x00012077
		public float Temperature
		{
			get
			{
				return this.temperatureInt;
			}
			set
			{
				this.temperatureInt = Mathf.Clamp(value, -273.15f, 1000f);
			}
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00013E8F File Offset: 0x0001208F
		public RoomGroupTempTracker(RoomGroup roomGroup, Map map)
		{
			this.roomGroup = roomGroup;
			this.Temperature = map.mapTemperature.OutdoorTemp;
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00013EBA File Offset: 0x000120BA
		public void RoofChanged()
		{
			this.RegenerateEqualizationData();
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00013EC2 File Offset: 0x000120C2
		public void RoomChanged()
		{
			if (this.Map != null)
			{
				this.Map.autoBuildRoofAreaSetter.ResolveQueuedGenerateRoofs();
			}
			this.RegenerateEqualizationData();
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x000C9FF8 File Offset: 0x000C81F8
		private void RegenerateEqualizationData()
		{
			this.thickRoofCoverage = 0f;
			this.noRoofCoverage = 0f;
			this.equalizeCells.Clear();
			if (this.roomGroup.RoomCount == 0)
			{
				return;
			}
			Map map = this.Map;
			if (!this.roomGroup.UsesOutdoorTemperature)
			{
				int num = 0;
				foreach (IntVec3 c in this.roomGroup.Cells)
				{
					RoofDef roof = c.GetRoof(map);
					if (roof == null)
					{
						this.noRoofCoverage += 1f;
					}
					else if (roof.isThickRoof)
					{
						this.thickRoofCoverage += 1f;
					}
					num++;
				}
				this.thickRoofCoverage /= (float)num;
				this.noRoofCoverage /= (float)num;
				foreach (IntVec3 a in this.roomGroup.Cells)
				{
					int i = 0;
					while (i < 4)
					{
						IntVec3 intVec = a + GenAdj.CardinalDirections[i];
						IntVec3 intVec2 = a + GenAdj.CardinalDirections[i] * 2;
						if (!intVec.InBounds(map))
						{
							goto IL_1E4;
						}
						Region region = intVec.GetRegion(map, RegionType.Set_Passable);
						if (region == null)
						{
							goto IL_1E4;
						}
						if (region.type == RegionType.Portal)
						{
							bool flag = false;
							for (int j = 0; j < region.links.Count; j++)
							{
								Region regionA = region.links[j].RegionA;
								Region regionB = region.links[j].RegionB;
								if (regionA.Room.Group != this.roomGroup && !regionA.IsDoorway)
								{
									flag = true;
									break;
								}
								if (regionB.Room.Group != this.roomGroup && !regionB.IsDoorway)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								goto IL_1E4;
							}
						}
						IL_248:
						i++;
						continue;
						IL_1E4:
						if (!intVec2.InBounds(map) || intVec2.GetRoomGroup(map) == this.roomGroup)
						{
							goto IL_248;
						}
						bool flag2 = false;
						for (int k = 0; k < 4; k++)
						{
							if ((intVec2 + GenAdj.CardinalDirections[k]).GetRoomGroup(map) == this.roomGroup)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							this.equalizeCells.Add(intVec2);
							goto IL_248;
						}
						goto IL_248;
					}
				}
				this.equalizeCells.Shuffle<IntVec3>();
			}
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x000CA2B4 File Offset: 0x000C84B4
		public void EqualizeTemperature()
		{
			if (this.roomGroup.UsesOutdoorTemperature)
			{
				this.Temperature = this.Map.mapTemperature.OutdoorTemp;
				return;
			}
			if (this.roomGroup.RoomCount != 0 && this.roomGroup.Rooms[0].RegionType == RegionType.Portal)
			{
				bool flag = true;
				IntVec3 a = this.roomGroup.Rooms[0].Cells.First<IntVec3>();
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = a + GenAdj.CardinalDirections[i];
					if (intVec.InBounds(this.Map))
					{
						RoomGroup roomGroup = intVec.GetRoomGroup(this.Map);
						if (roomGroup != null && (roomGroup.RoomCount != 1 || roomGroup.Rooms[0].RegionType != RegionType.Portal))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					this.roomGroup.Temperature += this.WallEqualizationTempChangePerInterval();
				}
				return;
			}
			float num = this.ThinRoofEqualizationTempChangePerInterval();
			float num2 = this.NoRoofEqualizationTempChangePerInterval();
			float num3 = this.WallEqualizationTempChangePerInterval();
			float num4 = this.DeepEqualizationTempChangePerInterval();
			this.Temperature += num + num2 + num3 + num4;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x000CA3EC File Offset: 0x000C85EC
		private float WallEqualizationTempChangePerInterval()
		{
			if (this.equalizeCells.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = Mathf.CeilToInt((float)this.equalizeCells.Count * 0.2f);
			for (int i = 0; i < num2; i++)
			{
				this.cycleIndex++;
				int index = this.cycleIndex % this.equalizeCells.Count;
				float num3;
				if (GenTemperature.TryGetDirectAirTemperatureForCell(this.equalizeCells[index], this.Map, out num3))
				{
					num += num3 - this.Temperature;
				}
				else
				{
					num += Mathf.Lerp(this.Temperature, this.Map.mapTemperature.OutdoorTemp, 0.5f) - this.Temperature;
				}
			}
			return num / (float)num2 * (float)this.equalizeCells.Count * 120f * 0.00017f / (float)this.roomGroup.CellCount;
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x000CA4D4 File Offset: 0x000C86D4
		private float TempDiffFromOutdoorsAdjusted()
		{
			float num = this.Map.mapTemperature.OutdoorTemp - this.temperatureInt;
			if (Mathf.Abs(num) < 100f)
			{
				return num;
			}
			return Mathf.Sign(num) * 100f + 5f * (num - Mathf.Sign(num) * 100f);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00013EE2 File Offset: 0x000120E2
		private float ThinRoofEqualizationTempChangePerInterval()
		{
			if (this.ThinRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.ThinRoofCoverage * 5E-05f * 120f;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00013F10 File Offset: 0x00012110
		private float NoRoofEqualizationTempChangePerInterval()
		{
			if (this.noRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.noRoofCoverage * 0.0007f * 120f;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x000CA52C File Offset: 0x000C872C
		private float DeepEqualizationTempChangePerInterval()
		{
			if (this.thickRoofCoverage < 0.001f)
			{
				return 0f;
			}
			float num = 15f - this.temperatureInt;
			if (num > 0f)
			{
				return 0f;
			}
			return num * this.thickRoofCoverage * 5E-05f * 120f;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000CA57C File Offset: 0x000C877C
		public void DebugDraw()
		{
			foreach (IntVec3 c in this.equalizeCells)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000CA5D4 File Offset: 0x000C87D4
		internal string DebugString()
		{
			if (this.roomGroup.UsesOutdoorTemperature)
			{
				return "uses outdoor temperature";
			}
			if (Time.frameCount > RoomGroupTempTracker.debugGetFrame + 120)
			{
				RoomGroupTempTracker.debugWallEq = 0f;
				for (int i = 0; i < 40; i++)
				{
					RoomGroupTempTracker.debugWallEq += this.WallEqualizationTempChangePerInterval();
				}
				RoomGroupTempTracker.debugWallEq /= 40f;
				RoomGroupTempTracker.debugGetFrame = Time.frameCount;
			}
			return string.Concat(new object[]
			{
				"  thick roof coverage: ",
				this.thickRoofCoverage.ToStringPercent("F0"),
				"\n  thin roof coverage: ",
				this.ThinRoofCoverage.ToStringPercent("F0"),
				"\n  no roof coverage: ",
				this.noRoofCoverage.ToStringPercent("F0"),
				"\n\n  wall equalization: ",
				RoomGroupTempTracker.debugWallEq.ToStringTemperatureOffset("F3"),
				"\n  thin roof equalization: ",
				this.ThinRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  no roof equalization: ",
				this.NoRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  deep equalization: ",
				this.DeepEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n\n  temp diff from outdoors, adjusted: ",
				this.TempDiffFromOutdoorsAdjusted().ToStringTemperatureOffset("F3"),
				"\n  tempChange e=20 targ= 200C: ",
				GenTemperature.ControlTemperatureTempChange(this.roomGroup.Cells.First<IntVec3>(), this.roomGroup.Map, 20f, 200f),
				"\n  tempChange e=20 targ=-200C: ",
				GenTemperature.ControlTemperatureTempChange(this.roomGroup.Cells.First<IntVec3>(), this.roomGroup.Map, 20f, -200f),
				"\n  equalize interval ticks: ",
				120,
				"\n  equalize cells count:",
				this.equalizeCells.Count
			});
		}

		// Token: 0x04000F68 RID: 3944
		private RoomGroup roomGroup;

		// Token: 0x04000F69 RID: 3945
		private float temperatureInt;

		// Token: 0x04000F6A RID: 3946
		private List<IntVec3> equalizeCells = new List<IntVec3>();

		// Token: 0x04000F6B RID: 3947
		private float noRoofCoverage;

		// Token: 0x04000F6C RID: 3948
		private float thickRoofCoverage;

		// Token: 0x04000F6D RID: 3949
		public const float FractionWallEqualizeCells = 0.2f;

		// Token: 0x04000F6E RID: 3950
		public const float WallEqualizeFactor = 0.00017f;

		// Token: 0x04000F6F RID: 3951
		public const float EqualizationPowerOfFilledCells = 0.5f;

		// Token: 0x04000F70 RID: 3952
		private int cycleIndex;

		// Token: 0x04000F71 RID: 3953
		private const float ThinRoofEqualizeRate = 5E-05f;

		// Token: 0x04000F72 RID: 3954
		private const float NoRoofEqualizeRate = 0.0007f;

		// Token: 0x04000F73 RID: 3955
		private const float DeepEqualizeFractionPerTick = 5E-05f;

		// Token: 0x04000F74 RID: 3956
		private static int debugGetFrame = -999;

		// Token: 0x04000F75 RID: 3957
		private static float debugWallEq;
	}
}
