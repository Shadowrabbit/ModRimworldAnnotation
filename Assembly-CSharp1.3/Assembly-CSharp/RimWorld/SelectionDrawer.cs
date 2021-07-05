using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135A RID: 4954
	[StaticConstructorOnStartup]
	public static class SelectionDrawer
	{
		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x06007809 RID: 30729 RVA: 0x002A4E62 File Offset: 0x002A3062
		public static Dictionary<object, float> SelectTimes
		{
			get
			{
				return SelectionDrawer.selectTimes;
			}
		}

		// Token: 0x0600780A RID: 30730 RVA: 0x002A4E69 File Offset: 0x002A3069
		public static void Notify_Selected(object t)
		{
			SelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x0600780B RID: 30731 RVA: 0x002A4E7B File Offset: 0x002A307B
		public static void Clear()
		{
			SelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x0600780C RID: 30732 RVA: 0x002A4E88 File Offset: 0x002A3088
		public static void DrawSelectionOverlays()
		{
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				SelectionDrawer.DrawSelectionBracketFor(obj);
				Thing thing = obj as Thing;
				if (thing != null)
				{
					thing.DrawExtraSelectionOverlays();
				}
			}
		}

		// Token: 0x0600780D RID: 30733 RVA: 0x002A4EEC File Offset: 0x002A30EC
		private static void DrawSelectionBracketFor(object obj)
		{
			Zone zone = obj as Zone;
			if (zone != null)
			{
				GenDraw.DrawFieldEdges(zone.Cells);
			}
			Thing thing = obj as Thing;
			if (thing != null)
			{
				CellRect? customRectForSelector = thing.CustomRectForSelector;
				if (customRectForSelector != null)
				{
					SelectionDrawerUtility.CalculateSelectionBracketPositionsWorld<object>(SelectionDrawer.bracketLocs, thing, customRectForSelector.Value.CenterVector3, new Vector2((float)customRectForSelector.Value.Width, (float)customRectForSelector.Value.Height), SelectionDrawer.selectTimes, Vector2.one, 1f);
				}
				else
				{
					SelectionDrawerUtility.CalculateSelectionBracketPositionsWorld<object>(SelectionDrawer.bracketLocs, thing, thing.DrawPos, thing.RotatedSize.ToVector2(), SelectionDrawer.selectTimes, Vector2.one, 1f);
				}
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					Quaternion rotation = Quaternion.AngleAxis((float)num, Vector3.up);
					Graphics.DrawMesh(MeshPool.plane10, SelectionDrawer.bracketLocs[i], rotation, SelectionDrawer.SelectionBracketMat, 0);
					num -= 90;
				}
			}
		}

		// Token: 0x040042C6 RID: 17094
		private static Dictionary<object, float> selectTimes = new Dictionary<object, float>();

		// Token: 0x040042C7 RID: 17095
		private static readonly Material SelectionBracketMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay);

		// Token: 0x040042C8 RID: 17096
		private static Vector3[] bracketLocs = new Vector3[4];
	}
}
