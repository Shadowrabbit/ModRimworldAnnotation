using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B21 RID: 6945
	[StaticConstructorOnStartup]
	public static class SelectionDrawer
	{
		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x060098C1 RID: 39105 RVA: 0x00065D64 File Offset: 0x00063F64
		public static Dictionary<object, float> SelectTimes
		{
			get
			{
				return SelectionDrawer.selectTimes;
			}
		}

		// Token: 0x060098C2 RID: 39106 RVA: 0x00065D6B File Offset: 0x00063F6B
		public static void Notify_Selected(object t)
		{
			SelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x060098C3 RID: 39107 RVA: 0x00065D7D File Offset: 0x00063F7D
		public static void Clear()
		{
			SelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x060098C4 RID: 39108 RVA: 0x002CE5BC File Offset: 0x002CC7BC
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

		// Token: 0x060098C5 RID: 39109 RVA: 0x002CE620 File Offset: 0x002CC820
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

		// Token: 0x040061A7 RID: 24999
		private static Dictionary<object, float> selectTimes = new Dictionary<object, float>();

		// Token: 0x040061A8 RID: 25000
		private static readonly Material SelectionBracketMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay);

		// Token: 0x040061A9 RID: 25001
		private static Vector3[] bracketLocs = new Vector3[4];
	}
}
