using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017B5 RID: 6069
	public static class CaravanTweenerUtility
	{
		// Token: 0x06008CCB RID: 36043 RVA: 0x00328E60 File Offset: 0x00327060
		public static Vector3 PatherTweenedPosRoot(Caravan caravan)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			if (!caravan.Spawned)
			{
				return worldGrid.GetTileCenter(caravan.Tile);
			}
			if (caravan.pather.Moving)
			{
				float num;
				if (!caravan.pather.IsNextTilePassable())
				{
					num = 0f;
				}
				else
				{
					num = 1f - caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal;
				}
				int tileID;
				if (caravan.pather.nextTile == caravan.Tile && caravan.pather.previousTileForDrawingIfInDoubt != -1)
				{
					tileID = caravan.pather.previousTileForDrawingIfInDoubt;
				}
				else
				{
					tileID = caravan.Tile;
				}
				return worldGrid.GetTileCenter(caravan.pather.nextTile) * num + worldGrid.GetTileCenter(tileID) * (1f - num);
			}
			return worldGrid.GetTileCenter(caravan.Tile);
		}

		// Token: 0x06008CCC RID: 36044 RVA: 0x00328F40 File Offset: 0x00327140
		public static Vector3 CaravanCollisionPosOffsetFor(Caravan caravan)
		{
			if (!caravan.Spawned)
			{
				return Vector3.zero;
			}
			bool flag = caravan.Spawned && caravan.pather.Moving;
			float d = 0.15f * Find.WorldGrid.averageTileSize;
			if (!flag || caravan.pather.nextTile == caravan.pather.Destination)
			{
				int num;
				if (flag)
				{
					num = caravan.pather.nextTile;
				}
				else
				{
					num = caravan.Tile;
				}
				int num2 = 0;
				int vertexIndex = 0;
				CaravanTweenerUtility.GetCaravansStandingAtOrAboutToStandAt(num, out num2, out vertexIndex, caravan);
				if (num2 == 0)
				{
					return Vector3.zero;
				}
				return WorldRendererUtility.ProjectOnQuadTangentialToPlanet(Find.WorldGrid.GetTileCenter(num), GenGeo.RegularPolygonVertexPosition(num2, vertexIndex) * d);
			}
			else
			{
				if (CaravanTweenerUtility.DrawPosCollides(caravan))
				{
					Rand.PushState();
					Rand.Seed = caravan.ID;
					float f = Rand.Range(0f, 360f);
					Rand.PopState();
					Vector2 point = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * d;
					return WorldRendererUtility.ProjectOnQuadTangentialToPlanet(CaravanTweenerUtility.PatherTweenedPosRoot(caravan), point);
				}
				return Vector3.zero;
			}
		}

		// Token: 0x06008CCD RID: 36045 RVA: 0x0032904C File Offset: 0x0032724C
		private static void GetCaravansStandingAtOrAboutToStandAt(int tile, out int caravansCount, out int caravansWithLowerIdCount, Caravan forCaravan)
		{
			caravansCount = 0;
			caravansWithLowerIdCount = 0;
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			int i = 0;
			while (i < caravans.Count)
			{
				Caravan caravan = caravans[i];
				if (caravan.Tile != tile)
				{
					if (caravan.pather.Moving && caravan.pather.nextTile == caravan.pather.Destination)
					{
						if (caravan.pather.Destination == tile)
						{
							goto IL_68;
						}
					}
				}
				else if (!caravan.pather.Moving)
				{
					goto IL_68;
				}
				IL_82:
				i++;
				continue;
				IL_68:
				caravansCount++;
				if (caravan.ID < forCaravan.ID)
				{
					caravansWithLowerIdCount++;
					goto IL_82;
				}
				goto IL_82;
			}
		}

		// Token: 0x06008CCE RID: 36046 RVA: 0x003290E8 File Offset: 0x003272E8
		private static bool DrawPosCollides(Caravan caravan)
		{
			Vector3 a = CaravanTweenerUtility.PatherTweenedPosRoot(caravan);
			float num = Find.WorldGrid.averageTileSize * 0.2f;
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan2 = caravans[i];
				if (caravan2 != caravan && Vector3.Distance(a, CaravanTweenerUtility.PatherTweenedPosRoot(caravan2)) < num)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400594B RID: 22859
		private const float BaseRadius = 0.15f;

		// Token: 0x0400594C RID: 22860
		private const float BaseDistToCollide = 0.2f;
	}
}
