using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200020E RID: 526
	public sealed class RoomTempTracker
	{
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x00054E19 File Offset: 0x00053019
		private Map Map
		{
			get
			{
				return this.room.Map;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x00054E26 File Offset: 0x00053026
		private float ThinRoofCoverage
		{
			get
			{
				return 1f - (this.thickRoofCoverage + this.noRoofCoverage);
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x00054E3B File Offset: 0x0005303B
		public List<IntVec3> EqualizeCellsForReading
		{
			get
			{
				return this.equalizeCells;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x00054E43 File Offset: 0x00053043
		// (set) Token: 0x06000F09 RID: 3849 RVA: 0x00054E4B File Offset: 0x0005304B
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

		// Token: 0x06000F0A RID: 3850 RVA: 0x00054E63 File Offset: 0x00053063
		public RoomTempTracker(Room room, Map map)
		{
			this.room = room;
			this.Temperature = map.mapTemperature.OutdoorTemp;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00054E8E File Offset: 0x0005308E
		public void RoofChanged()
		{
			this.RegenerateEqualizationData();
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00054E96 File Offset: 0x00053096
		public void RoomChanged()
		{
			if (this.Map != null)
			{
				this.Map.autoBuildRoofAreaSetter.ResolveQueuedGenerateRoofs();
			}
			this.RegenerateEqualizationData();
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00054EB8 File Offset: 0x000530B8
		private void RegenerateEqualizationData()
		{
			this.thickRoofCoverage = 0f;
			this.noRoofCoverage = 0f;
			this.equalizeCells.Clear();
			if (this.room.DistrictCount == 0)
			{
				return;
			}
			Map map = this.Map;
			if (!this.room.UsesOutdoorTemperature)
			{
				int num = 0;
				foreach (IntVec3 c in this.room.Cells)
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
				foreach (IntVec3 a in this.room.Cells)
				{
					int i = 0;
					while (i < 4)
					{
						IntVec3 intVec = a + GenAdj.CardinalDirections[i];
						IntVec3 intVec2 = a + GenAdj.CardinalDirections[i] * 2;
						if (!intVec.InBounds(map))
						{
							goto IL_1D8;
						}
						Region region = intVec.GetRegion(map, RegionType.Set_Passable);
						if (region == null)
						{
							goto IL_1D8;
						}
						if (region.type == RegionType.Portal)
						{
							bool flag = false;
							for (int j = 0; j < region.links.Count; j++)
							{
								Region regionA = region.links[j].RegionA;
								Region regionB = region.links[j].RegionB;
								if (regionA.Room != this.room && !regionA.IsDoorway)
								{
									flag = true;
									break;
								}
								if (regionB.Room != this.room && !regionB.IsDoorway)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								goto IL_1D8;
							}
						}
						IL_23C:
						i++;
						continue;
						IL_1D8:
						if (!intVec2.InBounds(map) || intVec2.GetRoom(map) == this.room)
						{
							goto IL_23C;
						}
						bool flag2 = false;
						for (int k = 0; k < 4; k++)
						{
							if ((intVec2 + GenAdj.CardinalDirections[k]).GetRoom(map) == this.room)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							this.equalizeCells.Add(intVec2);
							goto IL_23C;
						}
						goto IL_23C;
					}
				}
				this.equalizeCells.Shuffle<IntVec3>();
			}
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00055168 File Offset: 0x00053368
		public void EqualizeTemperature()
		{
			if (this.room.UsesOutdoorTemperature)
			{
				this.Temperature = this.Map.mapTemperature.OutdoorTemp;
				return;
			}
			if (this.room.IsDoorway)
			{
				bool flag = true;
				IntVec3 a = this.room.Districts[0].Cells.First<IntVec3>();
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = a + GenAdj.CardinalDirections[i];
					if (intVec.InBounds(this.Map))
					{
						Room room = intVec.GetRoom(this.Map);
						if (room != null && !room.IsDoorway)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					this.room.Temperature += this.WallEqualizationTempChangePerInterval();
				}
				return;
			}
			float num = this.ThinRoofEqualizationTempChangePerInterval();
			float num2 = this.NoRoofEqualizationTempChangePerInterval();
			float num3 = this.WallEqualizationTempChangePerInterval();
			float num4 = this.DeepEqualizationTempChangePerInterval();
			this.Temperature += num + num2 + num3 + num4;
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0005526C File Offset: 0x0005346C
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
			return num / (float)num2 * (float)this.equalizeCells.Count * 120f * 0.00017f / (float)this.room.CellCount;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00055354 File Offset: 0x00053554
		private float TempDiffFromOutdoorsAdjusted()
		{
			float num = this.Map.mapTemperature.OutdoorTemp - this.temperatureInt;
			if (Mathf.Abs(num) < 100f)
			{
				return num;
			}
			return Mathf.Sign(num) * 100f + 5f * (num - Mathf.Sign(num) * 100f);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x000553A9 File Offset: 0x000535A9
		private float ThinRoofEqualizationTempChangePerInterval()
		{
			if (this.ThinRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.ThinRoofCoverage * 5E-05f * 120f;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x000553D7 File Offset: 0x000535D7
		private float NoRoofEqualizationTempChangePerInterval()
		{
			if (this.noRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.noRoofCoverage * 0.0007f * 120f;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00055408 File Offset: 0x00053608
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

		// Token: 0x06000F14 RID: 3860 RVA: 0x00055458 File Offset: 0x00053658
		public void DebugDraw()
		{
			foreach (IntVec3 c in this.equalizeCells)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x000554B0 File Offset: 0x000536B0
		internal string DebugString()
		{
			if (this.room.UsesOutdoorTemperature)
			{
				return "uses outdoor temperature";
			}
			if (Time.frameCount > RoomTempTracker.debugGetFrame + 120)
			{
				RoomTempTracker.debugWallEq = 0f;
				for (int i = 0; i < 40; i++)
				{
					RoomTempTracker.debugWallEq += this.WallEqualizationTempChangePerInterval();
				}
				RoomTempTracker.debugWallEq /= 40f;
				RoomTempTracker.debugGetFrame = Time.frameCount;
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
				RoomTempTracker.debugWallEq.ToStringTemperatureOffset("F3"),
				"\n  thin roof equalization: ",
				this.ThinRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  no roof equalization: ",
				this.NoRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  deep equalization: ",
				this.DeepEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n\n  temp diff from outdoors, adjusted: ",
				this.TempDiffFromOutdoorsAdjusted().ToStringTemperatureOffset("F3"),
				"\n  tempChange e=20 targ= 200C: ",
				GenTemperature.ControlTemperatureTempChange(this.room.Cells.First<IntVec3>(), this.room.Map, 20f, 200f),
				"\n  tempChange e=20 targ=-200C: ",
				GenTemperature.ControlTemperatureTempChange(this.room.Cells.First<IntVec3>(), this.room.Map, 20f, -200f),
				"\n  equalize interval ticks: ",
				120,
				"\n  equalize cells count:",
				this.equalizeCells.Count
			});
		}

		// Token: 0x04000BF8 RID: 3064
		private Room room;

		// Token: 0x04000BF9 RID: 3065
		private float temperatureInt;

		// Token: 0x04000BFA RID: 3066
		private List<IntVec3> equalizeCells = new List<IntVec3>();

		// Token: 0x04000BFB RID: 3067
		private float noRoofCoverage;

		// Token: 0x04000BFC RID: 3068
		private float thickRoofCoverage;

		// Token: 0x04000BFD RID: 3069
		public const float FractionWallEqualizeCells = 0.2f;

		// Token: 0x04000BFE RID: 3070
		public const float WallEqualizeFactor = 0.00017f;

		// Token: 0x04000BFF RID: 3071
		public const float EqualizationPowerOfFilledCells = 0.5f;

		// Token: 0x04000C00 RID: 3072
		private int cycleIndex;

		// Token: 0x04000C01 RID: 3073
		private const float ThinRoofEqualizeRate = 5E-05f;

		// Token: 0x04000C02 RID: 3074
		private const float NoRoofEqualizeRate = 0.0007f;

		// Token: 0x04000C03 RID: 3075
		private const float DeepEqualizeFractionPerTick = 5E-05f;

		// Token: 0x04000C04 RID: 3076
		private static int debugGetFrame = -999;

		// Token: 0x04000C05 RID: 3077
		private static float debugWallEq;
	}
}
