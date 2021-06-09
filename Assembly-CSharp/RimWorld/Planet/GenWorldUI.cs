using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021C6 RID: 8646
	public static class GenWorldUI
	{
		// Token: 0x17001B81 RID: 7041
		// (get) Token: 0x0600B91B RID: 47387 RVA: 0x00077E5F File Offset: 0x0007605F
		public static float CaravanDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.averageTileSize;
			}
		}

		// Token: 0x17001B82 RID: 7042
		// (get) Token: 0x0600B91C RID: 47388 RVA: 0x00077E71 File Offset: 0x00076071
		private static float CaravanWideClickRadius
		{
			get
			{
				return 0.75f * Find.WorldGrid.averageTileSize;
			}
		}

		// Token: 0x17001B83 RID: 7043
		// (get) Token: 0x0600B91D RID: 47389 RVA: 0x00077E5F File Offset: 0x0007605F
		private static float DynamicallyDrawnObjectDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.averageTileSize;
			}
		}

		// Token: 0x0600B91E RID: 47390 RVA: 0x00353710 File Offset: 0x00351910
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

		// Token: 0x0600B91F RID: 47391 RVA: 0x003539C4 File Offset: 0x00351BC4
		public static Vector2 WorldToUIPosition(Vector3 worldLoc)
		{
			Vector3 vector = Find.WorldCamera.WorldToScreenPoint(worldLoc) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x0600B920 RID: 47392 RVA: 0x00353A00 File Offset: 0x00351C00
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

		// Token: 0x04007E75 RID: 32373
		private static List<Caravan> clickedCaravans = new List<Caravan>();

		// Token: 0x04007E76 RID: 32374
		private static List<WorldObject> clickedDynamicallyDrawnObjects = new List<WorldObject>();
	}
}
