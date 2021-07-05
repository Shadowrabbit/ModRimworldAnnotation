using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B8 RID: 1208
	public static class GenGrid
	{
		// Token: 0x060024F2 RID: 9458 RVA: 0x000E62F6 File Offset: 0x000E44F6
		public static bool InNoBuildEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 10);
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000E6301 File Offset: 0x000E4501
		public static bool InNoZoneEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 5);
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000E630C File Offset: 0x000E450C
		public static bool CloseToEdge(this IntVec3 c, Map map, int edgeDist)
		{
			IntVec3 size = map.Size;
			return c.x < edgeDist || c.z < edgeDist || c.x >= size.x - edgeDist || c.z >= size.z - edgeDist;
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000E6358 File Offset: 0x000E4558
		public static bool OnEdge(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return c.x == 0 || c.x == size.x - 1 || c.z == 0 || c.z == size.z - 1;
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000E63A0 File Offset: 0x000E45A0
		public static bool OnEdge(this IntVec3 c, Map map, Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				return c.z == 0;
			}
			if (dir == Rot4.South)
			{
				return c.z == map.Size.z - 1;
			}
			if (dir == Rot4.West)
			{
				return c.x == 0;
			}
			if (dir == Rot4.East)
			{
				return c.x == map.Size.x - 1;
			}
			Log.ErrorOnce("Invalid edge direction", 55370769);
			return false;
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000E6434 File Offset: 0x000E4634
		public static bool InBounds(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return (ulong)c.x < (ulong)((long)size.x) && (ulong)c.z < (ulong)((long)size.z);
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000E646C File Offset: 0x000E466C
		public static bool InBounds(this Vector3 v, Map map)
		{
			IntVec3 size = map.Size;
			return v.x >= 0f && v.z >= 0f && v.x < (float)size.x && v.z < (float)size.z;
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000E64BA File Offset: 0x000E46BA
		public static bool WalkableByNormal(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c);
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000E64D2 File Offset: 0x000E46D2
		public static bool WalkableByFenceBlocked(this IntVec3 c, Map map)
		{
			return map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000E64EA File Offset: 0x000E46EA
		public static bool WalkableBy(this IntVec3 c, Map map, Pawn pawn)
		{
			return map.pathing.For(pawn).pathGrid.Walkable(c);
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000E6503 File Offset: 0x000E4703
		public static bool WalkableByAny(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c) || map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000E6535 File Offset: 0x000E4735
		public static bool Walkable(this IntVec3 c, Map map)
		{
			return map.pathing.Normal.pathGrid.Walkable(c) && map.pathing.FenceBlocked.pathGrid.Walkable(c);
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000E6568 File Offset: 0x000E4768
		public static bool Standable(this IntVec3 c, Map map)
		{
			if (!c.Walkable(map))
			{
				return false;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000E65B4 File Offset: 0x000E47B4
		public static bool Impassable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability == Traversability.Impassable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x000E65F6 File Offset: 0x000E47F6
		public static bool SupportsStructureType(this IntVec3 c, Map map, TerrainAffordanceDef surfaceType)
		{
			return c.GetTerrain(map).affordances.Contains(surfaceType);
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000E660C File Offset: 0x000E480C
		public static bool CanBeSeenOver(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000E663C File Offset: 0x000E483C
		public static bool CanBeSeenOverFast(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x000E6660 File Offset: 0x000E4860
		public static bool CanBeSeenOver(this Building b)
		{
			if (b.def.Fillage == FillCategory.Full)
			{
				Building_Door building_Door = b as Building_Door;
				return building_Door != null && building_Door.Open;
			}
			return true;
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x000E6694 File Offset: 0x000E4894
		public static SurfaceType GetSurfaceType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return SurfaceType.None;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.surfaceType != SurfaceType.None)
				{
					return thingList[i].def.surfaceType;
				}
			}
			return SurfaceType.None;
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x000E66EB File Offset: 0x000E48EB
		public static bool HasEatSurface(this IntVec3 c, Map map)
		{
			return c.GetSurfaceType(map) == SurfaceType.Eat;
		}

		// Token: 0x04001716 RID: 5910
		public const int NoBuildEdgeWidth = 10;

		// Token: 0x04001717 RID: 5911
		public const int NoZoneEdgeWidth = 5;
	}
}
