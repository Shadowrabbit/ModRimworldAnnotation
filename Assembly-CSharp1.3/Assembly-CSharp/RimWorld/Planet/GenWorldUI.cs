using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001809 RID: 6153
	public static class GenWorldUI
	{
		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x06008FF6 RID: 36854 RVA: 0x00338CC2 File Offset: 0x00336EC2
		public static float CaravanDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.averageTileSize;
			}
		}

		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x06008FF7 RID: 36855 RVA: 0x00338CD4 File Offset: 0x00336ED4
		private static float CaravanWideClickRadius
		{
			get
			{
				return 0.75f * Find.WorldGrid.averageTileSize;
			}
		}

		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x06008FF8 RID: 36856 RVA: 0x00338CC2 File Offset: 0x00336EC2
		private static float DynamicallyDrawnObjectDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.averageTileSize;
			}
		}

		// Token: 0x06008FF9 RID: 36857 RVA: 0x00338CE8 File Offset: 0x00336EE8
		public static List<WorldObject> WorldObjectsUnderMouse(Vector2 mousePos)
		{
			List<WorldObject> list = new List<WorldObject>();
			ExpandableWorldObjectsUtility.GetExpandedWorldObjectUnderMouse(mousePos, list);
			float caravanDirectClickRadius = GenWorldUI.CaravanDirectClickRadius;
			GenWorldUI.clickedCaravans.Clear();
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (caravan.DistanceToMouse(mousePos) < caravanDirectClickRadius)
				{
					GenWorldUI.clickedCaravans.Add(caravan);
				}
			}
			GenWorldUI.clickedCaravans.SortBy((Caravan x) => x.DistanceToMouse(mousePos));
			for (int j = 0; j < GenWorldUI.clickedCaravans.Count; j++)
			{
				if (!list.Contains(GenWorldUI.clickedCaravans[j]))
				{
					list.Add(GenWorldUI.clickedCaravans[j]);
				}
			}
			float dynamicallyDrawnObjectDirectClickRadius = GenWorldUI.DynamicallyDrawnObjectDirectClickRadius;
			GenWorldUI.clickedDynamicallyDrawnObjects.Clear();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int k = 0; k < allWorldObjects.Count; k++)
			{
				WorldObject worldObject = allWorldObjects[k];
				if (worldObject.def.useDynamicDrawer && worldObject.DistanceToMouse(mousePos) < dynamicallyDrawnObjectDirectClickRadius)
				{
					GenWorldUI.clickedDynamicallyDrawnObjects.Add(worldObject);
				}
			}
			GenWorldUI.clickedDynamicallyDrawnObjects.SortBy((WorldObject x) => x.DistanceToMouse(mousePos));
			for (int l = 0; l < GenWorldUI.clickedDynamicallyDrawnObjects.Count; l++)
			{
				if (!list.Contains(GenWorldUI.clickedDynamicallyDrawnObjects[l]))
				{
					list.Add(GenWorldUI.clickedDynamicallyDrawnObjects[l]);
				}
			}
			int num = GenWorld.TileAt(mousePos, false);
			List<WorldObject> allWorldObjects2 = Find.WorldObjects.AllWorldObjects;
			for (int m = 0; m < allWorldObjects2.Count; m++)
			{
				if (allWorldObjects2[m].Tile == num && !list.Contains(allWorldObjects2[m]))
				{
					list.Add(allWorldObjects2[m]);
				}
			}
			float caravanWideClickRadius = GenWorldUI.CaravanWideClickRadius;
			GenWorldUI.clickedCaravans.Clear();
			List<Caravan> caravans2 = Find.WorldObjects.Caravans;
			for (int n = 0; n < caravans2.Count; n++)
			{
				Caravan caravan2 = caravans2[n];
				if (caravan2.DistanceToMouse(mousePos) < caravanWideClickRadius)
				{
					GenWorldUI.clickedCaravans.Add(caravan2);
				}
			}
			GenWorldUI.clickedCaravans.SortBy((Caravan x) => x.DistanceToMouse(mousePos));
			for (int num2 = 0; num2 < GenWorldUI.clickedCaravans.Count; num2++)
			{
				if (!list.Contains(GenWorldUI.clickedCaravans[num2]))
				{
					list.Add(GenWorldUI.clickedCaravans[num2]);
				}
			}
			GenWorldUI.clickedCaravans.Clear();
			return list;
		}

		// Token: 0x06008FFA RID: 36858 RVA: 0x00338F9C File Offset: 0x0033719C
		public static Vector2 WorldToUIPosition(Vector3 worldLoc)
		{
			Vector3 vector = Find.WorldCamera.WorldToScreenPoint(worldLoc) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x06008FFB RID: 36859 RVA: 0x00338FD8 File Offset: 0x003371D8
		public static float CurUITileSize()
		{
			Vector3 localPosition = Find.WorldCamera.transform.localPosition;
			Quaternion rotation = Find.WorldCamera.transform.rotation;
			Find.WorldCamera.transform.localPosition = new Vector3(0f, 0f, localPosition.magnitude);
			Find.WorldCamera.transform.rotation = Quaternion.identity;
			float x = (GenWorldUI.WorldToUIPosition(new Vector3(-Find.WorldGrid.averageTileSize / 2f, 0f, 100f)) - GenWorldUI.WorldToUIPosition(new Vector3(Find.WorldGrid.averageTileSize / 2f, 0f, 100f))).x;
			Find.WorldCamera.transform.localPosition = localPosition;
			Find.WorldCamera.transform.rotation = rotation;
			return x;
		}

		// Token: 0x04005A84 RID: 23172
		private static List<Caravan> clickedCaravans = new List<Caravan>();

		// Token: 0x04005A85 RID: 23173
		private static List<WorldObject> clickedDynamicallyDrawnObjects = new List<WorldObject>();
	}
}
