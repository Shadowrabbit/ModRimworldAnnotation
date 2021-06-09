using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001C2A RID: 7210
	public static class SpectatorCellFinder
	{
		// Token: 0x06009E84 RID: 40580 RVA: 0x002E7BDC File Offset: 0x002E5DDC
		public static bool TryFindSpectatorCellFor(Pawn p, CellRect spectateRect, Map map, out IntVec3 cell, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1, List<IntVec3> extraDisallowedCells = null)
		{
			spectateRect.ClipInsideMap(map);
			if (spectateRect.Area == 0 || allowedSides == SpectateRectSide.None)
			{
				cell = IntVec3.Invalid;
				return false;
			}
			CellRect rectWithMargin = spectateRect.ExpandedBy(margin).ClipInsideMap(map);
			Predicate<Thing> <>9__1;
			Predicate<IntVec3> predicate = delegate(IntVec3 x)
			{
				if (!x.InBounds(map))
				{
					return false;
				}
				if (!x.Standable(map))
				{
					return false;
				}
				if (x.Fogged(map))
				{
					return false;
				}
				if (rectWithMargin.Contains(x))
				{
					return false;
				}
				if ((x.z <= rectWithMargin.maxZ || (allowedSides & SpectateRectSide.Up) != SpectateRectSide.Up) && (x.x <= rectWithMargin.maxX || (allowedSides & SpectateRectSide.Right) != SpectateRectSide.Right) && (x.z >= rectWithMargin.minZ || (allowedSides & SpectateRectSide.Down) != SpectateRectSide.Down) && (x.x >= rectWithMargin.minX || (allowedSides & SpectateRectSide.Left) != SpectateRectSide.Left))
				{
					return false;
				}
				IntVec3 intVec3 = spectateRect.ClosestCellTo(x);
				if ((float)intVec3.DistanceToSquared(x) > 210.25f)
				{
					return false;
				}
				if (!GenSight.LineOfSight(intVec3, x, map, true, null, 0, 0))
				{
					return false;
				}
				List<Thing> thingList = x.GetThingList(map);
				Predicate<Thing> match;
				if ((match = <>9__1) == null)
				{
					match = (<>9__1 = ((Thing y) => y is Pawn && y != p));
				}
				if (thingList.Find(match) != null)
				{
					return false;
				}
				if (p != null)
				{
					if (!p.CanReserveAndReach(x, PathEndMode.OnCell, Danger.Some, 1, -1, null, false))
					{
						return false;
					}
					Building edifice = x.GetEdifice(map);
					if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable && !p.CanReserve(edifice, 1, -1, null, false))
					{
						return false;
					}
					if (x.IsForbidden(p))
					{
						return false;
					}
					if (x.GetDangerFor(p, map) != Danger.None)
					{
						return false;
					}
				}
				if (extraDisallowedCells != null && extraDisallowedCells.Contains(x))
				{
					return false;
				}
				if (!SpectatorCellFinder.CorrectlyRotatedChairAt(x, map, spectateRect))
				{
					int num = 0;
					for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
					{
						if (SpectatorCellFinder.CorrectlyRotatedChairAt(x + GenAdj.AdjacentCells[k], map, spectateRect))
						{
							num++;
						}
					}
					if (num >= 3)
					{
						return false;
					}
					int num2 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(-1, 0, 0), map, 4, spectateRect);
					if (num2 >= 0)
					{
						int num3 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(1, 0, 0), map, 4, spectateRect);
						if (num3 >= 0 && Mathf.Abs(num2 - num3) <= 1)
						{
							return false;
						}
					}
					int num4 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(0, 0, 1), map, 4, spectateRect);
					if (num4 >= 0)
					{
						int num5 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(0, 0, -1), map, 4, spectateRect);
						if (num5 >= 0 && Mathf.Abs(num4 - num5) <= 1)
						{
							return false;
						}
					}
				}
				return true;
			};
			if (p != null && predicate(p.Position) && SpectatorCellFinder.CorrectlyRotatedChairAt(p.Position, map, spectateRect))
			{
				cell = p.Position;
				return true;
			}
			for (int i = 0; i < 1000; i++)
			{
				IntVec3 intVec = rectWithMargin.CenterCell + GenRadial.RadialPattern[i];
				if (predicate(intVec))
				{
					if (!SpectatorCellFinder.CorrectlyRotatedChairAt(intVec, map, spectateRect))
					{
						for (int j = 0; j < 90; j++)
						{
							IntVec3 intVec2 = intVec + GenRadial.RadialPattern[j];
							if (SpectatorCellFinder.CorrectlyRotatedChairAt(intVec2, map, spectateRect) && predicate(intVec2))
							{
								cell = intVec2;
								return true;
							}
						}
					}
					cell = intVec;
					return true;
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06009E85 RID: 40581 RVA: 0x0006979A File Offset: 0x0006799A
		private static bool CorrectlyRotatedChairAt(IntVec3 x, Map map, CellRect spectateRect)
		{
			return SpectatorCellFinder.GetCorrectlyRotatedChairAt(x, map, spectateRect) != null;
		}

		// Token: 0x06009E86 RID: 40582 RVA: 0x002E7D7C File Offset: 0x002E5F7C
		private static Building GetCorrectlyRotatedChairAt(IntVec3 x, Map map, CellRect spectateRect)
		{
			if (!x.InBounds(map))
			{
				return null;
			}
			Building edifice = x.GetEdifice(map);
			if (edifice == null || edifice.def.category != ThingCategory.Building || !edifice.def.building.isSittable)
			{
				return null;
			}
			if (GenGeo.AngleDifferenceBetween(edifice.Rotation.AsAngle, (spectateRect.ClosestCellTo(x) - edifice.Position).AngleFlat) > 75f)
			{
				return null;
			}
			return edifice;
		}

		// Token: 0x06009E87 RID: 40583 RVA: 0x002E7DFC File Offset: 0x002E5FFC
		private static int DistanceToClosestChair(IntVec3 from, IntVec3 step, Map map, int maxDist, CellRect spectateRect)
		{
			int num = 0;
			IntVec3 intVec = from;
			for (;;)
			{
				intVec += step;
				num++;
				if (!intVec.InBounds(map))
				{
					break;
				}
				if (SpectatorCellFinder.CorrectlyRotatedChairAt(intVec, map, spectateRect))
				{
					return num;
				}
				if (!intVec.Walkable(map))
				{
					return -1;
				}
				if (num >= maxDist)
				{
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x06009E88 RID: 40584 RVA: 0x002E7E44 File Offset: 0x002E6044
		public static void DebugFlashPotentialSpectatorCells(CellRect spectateRect, Map map, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1)
		{
			List<IntVec3> list = new List<IntVec3>();
			int num = 50;
			int num2 = 0;
			IntVec3 intVec;
			while (num2 < num && SpectatorCellFinder.TryFindSpectatorCellFor(null, spectateRect, map, out intVec, allowedSides, margin, list))
			{
				list.Add(intVec);
				float a = Mathf.Lerp(1f, 0.08f, (float)num2 / (float)num);
				Material mat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0.8f, 0f, a), false);
				map.debugDrawer.FlashCell(intVec, mat, (num2 + 1).ToString(), 50);
				num2++;
			}
			SpectateRectSide spectateRectSide = SpectatorCellFinder.FindSingleBestSide(spectateRect, map, allowedSides, margin);
			IntVec3 centerCell = spectateRect.CenterCell;
			switch (spectateRectSide)
			{
			case SpectateRectSide.Up:
				centerCell.z += spectateRect.Height / 2 + 10;
				break;
			case SpectateRectSide.Right:
				centerCell.x += spectateRect.Width / 2 + 10;
				break;
			case SpectateRectSide.Up | SpectateRectSide.Right:
				break;
			case SpectateRectSide.Down:
				centerCell.z -= spectateRect.Height / 2 + 10;
				break;
			default:
				if (spectateRectSide == SpectateRectSide.Left)
				{
					centerCell.x -= spectateRect.Width / 2 + 10;
				}
				break;
			}
			map.debugDrawer.FlashLine(spectateRect.CenterCell, centerCell, 50, SimpleColor.White);
		}

		// Token: 0x06009E89 RID: 40585 RVA: 0x000697A7 File Offset: 0x000679A7
		public static SpectateRectSide FindSingleBestSide(CellRect spectateRect, Map map, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1)
		{
			return SpectatorCellFinder.FindSingleBestSide_NewTemp(spectateRect, map, allowedSides, margin, null);
		}

		// Token: 0x06009E8A RID: 40586 RVA: 0x002E7F7C File Offset: 0x002E617C
		public static SpectateRectSide FindSingleBestSide_NewTemp(CellRect spectateRect, Map map, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1, Func<IntVec3, SpectateRectSide, int, float> scoreOffset = null)
		{
			for (int i = 0; i < SpectatorCellFinder.scorePerSide.Length; i++)
			{
				SpectatorCellFinder.scorePerSide[i] = 0f;
			}
			SpectatorCellFinder.usedCells.Clear();
			int num = 30;
			CellRect cellRect = spectateRect.ExpandedBy(margin).ClipInsideMap(map);
			int num2 = 0;
			IntVec3 intVec;
			while (num2 < num && SpectatorCellFinder.TryFindSpectatorCellFor(null, spectateRect, map, out intVec, allowedSides, margin, SpectatorCellFinder.usedCells))
			{
				SpectatorCellFinder.usedCells.Add(intVec);
				SpectateRectSide spectateRectSide = SpectateRectSide.None;
				if (intVec.z > cellRect.maxZ)
				{
					spectateRectSide |= SpectateRectSide.Up;
				}
				if (intVec.x > cellRect.maxX)
				{
					spectateRectSide |= SpectateRectSide.Right;
				}
				if (intVec.z < cellRect.minZ)
				{
					spectateRectSide |= SpectateRectSide.Down;
				}
				if (intVec.x < cellRect.minX)
				{
					spectateRectSide |= SpectateRectSide.Left;
				}
				float num3 = Mathf.Lerp(1f, 0.35f, (float)num2 / (float)num);
				float num4 = num3 + ((scoreOffset != null) ? scoreOffset(intVec, spectateRectSide, num2) : 0f);
				Building correctlyRotatedChairAt = SpectatorCellFinder.GetCorrectlyRotatedChairAt(intVec, map, spectateRect);
				foreach (SpectateRectSide spectateRectSide2 in spectateRectSide.GetAllSelectedItems<SpectateRectSide>())
				{
					if (spectateRectSide2.ValidSingleSide() && allowedSides.HasFlag(spectateRectSide2))
					{
						SpectatorCellFinder.scorePerSide[spectateRectSide2.AsInt()] += num4;
						if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == spectateRectSide2.AsRot4())
						{
							SpectatorCellFinder.scorePerSide[spectateRectSide2.AsInt()] += 1.2f * num3;
						}
					}
				}
				num2++;
			}
			float num5 = 0f;
			int num6 = -1;
			for (int j = 0; j < SpectatorCellFinder.scorePerSide.Length; j++)
			{
				if (SpectatorCellFinder.scorePerSide[j] != 0f && (num6 < 0 || SpectatorCellFinder.scorePerSide[j] > num5))
				{
					num6 = j;
					num5 = SpectatorCellFinder.scorePerSide[j];
				}
			}
			SpectatorCellFinder.usedCells.Clear();
			return num6.ToSpectatorSide();
		}

		// Token: 0x06009E8B RID: 40587 RVA: 0x000697B3 File Offset: 0x000679B3
		public static bool ValidSingleSide(this SpectateRectSide side)
		{
			switch (side)
			{
			case SpectateRectSide.Up:
				return true;
			case SpectateRectSide.Right:
				return true;
			case SpectateRectSide.Up | SpectateRectSide.Right:
				break;
			case SpectateRectSide.Down:
				return true;
			default:
				if (side == SpectateRectSide.Left)
				{
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x06009E8C RID: 40588 RVA: 0x000697DC File Offset: 0x000679DC
		public static Rot4 AsRot4(this SpectateRectSide side)
		{
			switch (side)
			{
			case SpectateRectSide.Up:
				return Rot4.North;
			case SpectateRectSide.Right:
				return Rot4.East;
			case SpectateRectSide.Up | SpectateRectSide.Right:
				break;
			case SpectateRectSide.Down:
				return Rot4.South;
			default:
				if (side == SpectateRectSide.Left)
				{
					return Rot4.West;
				}
				break;
			}
			return Rot4.Invalid;
		}

		// Token: 0x06009E8D RID: 40589 RVA: 0x00069819 File Offset: 0x00067A19
		public static int AsInt(this SpectateRectSide side)
		{
			switch (side)
			{
			case SpectateRectSide.Up:
				return 0;
			case SpectateRectSide.Right:
				return 1;
			case SpectateRectSide.Up | SpectateRectSide.Right:
				break;
			case SpectateRectSide.Down:
				return 2;
			default:
				if (side == SpectateRectSide.Left)
				{
					return 3;
				}
				break;
			}
			return 0;
		}

		// Token: 0x06009E8E RID: 40590 RVA: 0x00069842 File Offset: 0x00067A42
		public static SpectateRectSide ToSpectatorSide(this int side)
		{
			switch (side)
			{
			case 0:
				return SpectateRectSide.Up;
			case 1:
				return SpectateRectSide.Right;
			case 2:
				return SpectateRectSide.Down;
			case 3:
				return SpectateRectSide.Left;
			default:
				return SpectateRectSide.None;
			}
		}

		// Token: 0x0400651D RID: 25885
		private const float MaxDistanceToSpectateRect = 14.5f;

		// Token: 0x0400651E RID: 25886
		private static float[] scorePerSide = new float[4];

		// Token: 0x0400651F RID: 25887
		private static List<IntVec3> usedCells = new List<IntVec3>();
	}
}
