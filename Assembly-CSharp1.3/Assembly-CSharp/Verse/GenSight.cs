using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004BB RID: 1211
	public static class GenSight
	{
		// Token: 0x0600250D RID: 9485 RVA: 0x000E69DC File Offset: 0x000E4BDC
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null, int halfXOffset = 0, int halfZOffset = 0)
		{
			if (!start.InBounds(map) || !end.InBounds(map))
			{
				return false;
			}
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x > start.x) ? 1 : -1;
			int num6 = (end.z > start.z) ? 1 : -1;
			num *= 4;
			num2 *= 4;
			num += halfXOffset * 2;
			num2 += halfZOffset * 2;
			int num7 = num / 2 - num2 / 2;
			IntVec3 intVec = default(IntVec3);
			while (i > 1)
			{
				intVec.x = num3;
				intVec.z = num4;
				if (!skipFirstCell || !(intVec == start))
				{
					if (!intVec.CanBeSeenOverFast(map))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
			return true;
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000E6B28 File Offset: 0x000E4D28
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null)
		{
			if (!start.InBounds(map) || !end.InBounds(map))
			{
				return false;
			}
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x > start.x) ? 1 : -1;
			int num6 = (end.z > start.z) ? 1 : -1;
			int num7 = num - num2;
			num *= 2;
			num2 *= 2;
			IntVec3 intVec = default(IntVec3);
			while (i > 1)
			{
				intVec.x = num3;
				intVec.z = num4;
				if (endRect.Contains(intVec))
				{
					return true;
				}
				if (!startRect.Contains(intVec))
				{
					if (!intVec.CanBeSeenOverFast(map))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
			return true;
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x000E6C6C File Offset: 0x000E4E6C
		public static IEnumerable<IntVec3> PointsOnLineOfSight(IntVec3 start, IntVec3 end)
		{
			bool sideOnEqual;
			if (start.x == end.x)
			{
				sideOnEqual = (start.z < end.z);
			}
			else
			{
				sideOnEqual = (start.x < end.x);
			}
			int dx = Mathf.Abs(end.x - start.x);
			int dz = Mathf.Abs(end.z - start.z);
			int x = start.x;
			int z = start.z;
			int i = 1 + dx + dz;
			int x_inc = (end.x > start.x) ? 1 : -1;
			int z_inc = (end.z > start.z) ? 1 : -1;
			int error = dx - dz;
			dx *= 2;
			dz *= 2;
			IntVec3 c = default(IntVec3);
			while (i > 1)
			{
				c.x = x;
				c.z = z;
				yield return c;
				if (error > 0 || (error == 0 && sideOnEqual))
				{
					x += x_inc;
					error -= dz;
				}
				else
				{
					z += z_inc;
					error += dx;
				}
				int num = i - 1;
				i = num;
			}
			yield break;
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x000E6C84 File Offset: 0x000E4E84
		public static void PointsOnLineOfSight(IntVec3 start, IntVec3 end, Action<IntVec3> visitor)
		{
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x > start.x) ? 1 : -1;
			int num6 = (end.z > start.z) ? 1 : -1;
			int num7 = num - num2;
			num *= 2;
			num2 *= 2;
			IntVec3 obj = default(IntVec3);
			while (i > 1)
			{
				obj.x = num3;
				obj.z = num4;
				visitor(obj);
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x000E6D88 File Offset: 0x000E4F88
		public static bool LineOfSightToEdges(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, 0, 0))
			{
				return true;
			}
			int num = (start * 2).DistanceToSquared(end * 2);
			for (int i = 0; i < 4; i++)
			{
				if ((start * 2).DistanceToSquared(end * 2 + GenAdj.CardinalDirections[i]) <= num && GenSight.LineOfSight(start, end, map, skipFirstCell, validator, GenAdj.CardinalDirections[i].x, GenAdj.CardinalDirections[i].z))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x000E6E1C File Offset: 0x000E501C
		public static bool LineOfSightToThing(IntVec3 start, Thing t, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			if (t.def.size == IntVec2.One)
			{
				return GenSight.LineOfSight(start, t.Position, map, false, null, 0, 0);
			}
			foreach (IntVec3 end in t.OccupiedRect())
			{
				if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, 0, 0))
				{
					return true;
				}
			}
			return false;
		}
	}
}
