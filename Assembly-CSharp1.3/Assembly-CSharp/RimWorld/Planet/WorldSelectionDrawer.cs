using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200181C RID: 6172
	public static class WorldSelectionDrawer
	{
		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x060090B5 RID: 37045 RVA: 0x0033EB41 File Offset: 0x0033CD41
		public static Dictionary<WorldObject, float> SelectTimes
		{
			get
			{
				return WorldSelectionDrawer.selectTimes;
			}
		}

		// Token: 0x060090B6 RID: 37046 RVA: 0x0033EB48 File Offset: 0x0033CD48
		public static void Notify_Selected(WorldObject t)
		{
			WorldSelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x060090B7 RID: 37047 RVA: 0x0033EB5A File Offset: 0x0033CD5A
		public static void Clear()
		{
			WorldSelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x060090B8 RID: 37048 RVA: 0x0033EB68 File Offset: 0x0033CD68
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

		// Token: 0x060090B9 RID: 37049 RVA: 0x0033EBA4 File Offset: 0x0033CDA4
		public static void DrawSelectionOverlays()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				selectedObjects[i].DrawExtraSelectionOverlays();
			}
		}

		// Token: 0x060090BA RID: 37050 RVA: 0x0033EBDC File Offset: 0x0033CDDC
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

		// Token: 0x04005AF8 RID: 23288
		private static Dictionary<WorldObject, float> selectTimes = new Dictionary<WorldObject, float>();

		// Token: 0x04005AF9 RID: 23289
		private const float BaseSelectedTexJump = 25f;

		// Token: 0x04005AFA RID: 23290
		private const float BaseSelectedTexScale = 0.4f;

		// Token: 0x04005AFB RID: 23291
		private const float BaseSelectionRectSize = 35f;

		// Token: 0x04005AFC RID: 23292
		private static readonly Color HiddenSelectionBracketColor = new Color(1f, 1f, 1f, 0.35f);

		// Token: 0x04005AFD RID: 23293
		private static Vector2[] bracketLocs = new Vector2[4];
	}
}
