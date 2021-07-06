using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021F0 RID: 8688
	public static class WorldSelectionDrawer
	{
		// Token: 0x17001BB3 RID: 7091
		// (get) Token: 0x0600BA16 RID: 47638 RVA: 0x000788AD File Offset: 0x00076AAD
		public static Dictionary<WorldObject, float> SelectTimes
		{
			get
			{
				return WorldSelectionDrawer.selectTimes;
			}
		}

		// Token: 0x0600BA17 RID: 47639 RVA: 0x000788B4 File Offset: 0x00076AB4
		public static void Notify_Selected(WorldObject t)
		{
			WorldSelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x0600BA18 RID: 47640 RVA: 0x000788C6 File Offset: 0x00076AC6
		public static void Clear()
		{
			WorldSelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x0600BA19 RID: 47641 RVA: 0x00358AAC File Offset: 0x00356CAC
		public static void SelectionOverlaysOnGUI()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				WorldObject worldObject = selectedObjects[i];
				WorldSelectionDrawer.DrawSelectionBracketOnGUIFor(worldObject);
				worldObject.ExtraSelectionOverlaysOnGUI();
			}
		}

		// Token: 0x0600BA1A RID: 47642 RVA: 0x00358AE8 File Offset: 0x00356CE8
		public static void DrawSelectionOverlays()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				selectedObjects[i].DrawExtraSelectionOverlays();
			}
		}

		// Token: 0x0600BA1B RID: 47643 RVA: 0x00358B20 File Offset: 0x00356D20
		private static void DrawSelectionBracketOnGUIFor(WorldObject obj)
		{
			Vector2 vector = obj.ScreenPos();
			Rect rect = new Rect(vector.x - 17.5f, vector.y - 17.5f, 35f, 35f);
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * 0.4f, (float)SelectionDrawerUtility.SelectedTexGUI.height * 0.4f);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<WorldObject>(WorldSelectionDrawer.bracketLocs, obj, rect, WorldSelectionDrawer.selectTimes, textureSize, 25f);
			if (obj.HiddenBehindTerrainNow())
			{
				GUI.color = WorldSelectionDrawer.HiddenSelectionBracketColor;
			}
			else
			{
				GUI.color = Color.white;
			}
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(WorldSelectionDrawer.bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, 0.4f);
				num += 90;
			}
			GUI.color = Color.white;
		}

		// Token: 0x04007F15 RID: 32533
		private static Dictionary<WorldObject, float> selectTimes = new Dictionary<WorldObject, float>();

		// Token: 0x04007F16 RID: 32534
		private const float BaseSelectedTexJump = 25f;

		// Token: 0x04007F17 RID: 32535
		private const float BaseSelectedTexScale = 0.4f;

		// Token: 0x04007F18 RID: 32536
		private const float BaseSelectionRectSize = 35f;

		// Token: 0x04007F19 RID: 32537
		private static readonly Color HiddenSelectionBracketColor = new Color(1f, 1f, 1f, 0.35f);

		// Token: 0x04007F1A RID: 32538
		private static Vector2[] bracketLocs = new Vector2[4];
	}
}
