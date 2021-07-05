using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025D RID: 605
	public static class PawnCollisionTweenerUtility
	{
		// Token: 0x0600112F RID: 4399 RVA: 0x00061868 File Offset: 0x0005FA68
		public static Vector3 PawnCollisionPosOffsetFor(Pawn pawn)
		{
			if (pawn.GetPosture() != PawnPosture.Standing)
			{
				return Vector3.zero;
			}
			bool flag = pawn.Spawned && pawn.pather.MovingNow;
			if (!flag || pawn.pather.nextCell == pawn.pather.Destination.Cell)
			{
				if (!flag && pawn.Drawer.leaner.ShouldLean())
				{
					return Vector3.zero;
				}
				IntVec3 at;
				if (flag)
				{
					at = pawn.pather.nextCell;
				}
				else
				{
					at = pawn.Position;
				}
				int polygonVertices;
				int vertexIndex;
				bool flag2;
				PawnCollisionTweenerUtility.GetPawnsStandingAtOrAboutToStandAt(at, pawn.Map, out polygonVertices, out vertexIndex, out flag2, pawn);
				if (!flag2)
				{
					return Vector3.zero;
				}
				return GenGeo.RegularPolygonVertexPositionVec3(polygonVertices, vertexIndex) * 0.32f;
			}
			else
			{
				IntVec3 nextCell = pawn.pather.nextCell;
				if (PawnCollisionTweenerUtility.CanGoDirectlyToNextCell(pawn))
				{
					return Vector3.zero;
				}
				int num = pawn.thingIDNumber % 2;
				if (nextCell.x != pawn.Position.x)
				{
					if (num == 0)
					{
						return new Vector3(0f, 0f, 0.32f);
					}
					return new Vector3(0f, 0f, -0.32f);
				}
				else
				{
					if (num == 0)
					{
						return new Vector3(0.32f, 0f, 0f);
					}
					return new Vector3(-0.32f, 0f, 0f);
				}
			}
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x000619BC File Offset: 0x0005FBBC
		private static void GetPawnsStandingAtOrAboutToStandAt(IntVec3 at, Map map, out int pawnsCount, out int pawnsWithLowerIdCount, out bool forPawnFound, Pawn forPawn)
		{
			pawnsCount = 0;
			pawnsWithLowerIdCount = 0;
			forPawnFound = false;
			foreach (IntVec3 intVec in CellRect.SingleCell(at).ExpandedBy(1))
			{
				if (intVec.InBounds(map))
				{
					List<Thing> thingList = intVec.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn = thingList[i] as Pawn;
						if (pawn != null && pawn.GetPosture() == PawnPosture.Standing)
						{
							if (intVec != at)
							{
								if (!pawn.pather.MovingNow || pawn.pather.nextCell != pawn.pather.Destination.Cell)
								{
									goto IL_101;
								}
								if (pawn.pather.Destination.Cell != at)
								{
									goto IL_101;
								}
							}
							else if (pawn.pather.MovingNow)
							{
								goto IL_101;
							}
							if (pawn == forPawn)
							{
								forPawnFound = true;
							}
							pawnsCount++;
							if (pawn.thingIDNumber < forPawn.thingIDNumber)
							{
								pawnsWithLowerIdCount++;
							}
						}
						IL_101:;
					}
				}
			}
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00061B18 File Offset: 0x0005FD18
		private static bool CanGoDirectlyToNextCell(Pawn pawn)
		{
			IntVec3 nextCell = pawn.pather.nextCell;
			foreach (IntVec3 c in CellRect.FromLimits(nextCell, pawn.Position).ExpandedBy(1))
			{
				if (c.InBounds(pawn.Map))
				{
					List<Thing> thingList = c.GetThingList(pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn2 = thingList[i] as Pawn;
						if (pawn2 != null && pawn2 != pawn && pawn2.GetPosture() == PawnPosture.Standing)
						{
							if (pawn2.pather.MovingNow)
							{
								if (((pawn2.Position == nextCell && PawnCollisionTweenerUtility.WillBeFasterOnNextCell(pawn, pawn2)) || pawn2.pather.nextCell == nextCell || pawn2.Position == pawn.Position || (pawn2.pather.nextCell == pawn.Position && PawnCollisionTweenerUtility.WillBeFasterOnNextCell(pawn2, pawn))) && pawn2.thingIDNumber < pawn.thingIDNumber)
								{
									return false;
								}
							}
							else if (pawn2.Position == pawn.Position || pawn2.Position == nextCell)
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00061CB0 File Offset: 0x0005FEB0
		private static bool WillBeFasterOnNextCell(Pawn p1, Pawn p2)
		{
			if (p1.pather.nextCellCostLeft == p2.pather.nextCellCostLeft)
			{
				return p1.thingIDNumber < p2.thingIDNumber;
			}
			return p1.pather.nextCellCostLeft < p2.pather.nextCellCostLeft;
		}

		// Token: 0x04000D16 RID: 3350
		private const float Radius = 0.32f;
	}
}
