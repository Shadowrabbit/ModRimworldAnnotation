using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012EF RID: 4847
	public class Dialog_ManageAreas : Window
	{
		// Token: 0x17001473 RID: 5235
		// (get) Token: 0x06007470 RID: 29808 RVA: 0x002787B9 File Offset: 0x002769B9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(450f, 400f);
			}
		}

		// Token: 0x06007471 RID: 29809 RVA: 0x002787CA File Offset: 0x002769CA
		public Dialog_ManageAreas(Map map)
		{
			this.map = map;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06007472 RID: 29810 RVA: 0x002787FC File Offset: 0x002769FC
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = inRect.width;
			listing_Standard.Begin(inRect);
			List<Area> allAreas = this.map.areaManager.AllAreas;
			int i = 0;
			for (int j = 0; j < allAreas.Count; j++)
			{
				if (allAreas[j].Mutable)
				{
					Dialog_ManageAreas.DoAreaRow(listing_Standard.GetRect(24f), allAreas[j]);
					listing_Standard.Gap(6f);
					i++;
				}
			}
			if (this.map.areaManager.CanMakeNewAllowed())
			{
				while (i < 9)
				{
					listing_Standard.Gap(30f);
					i++;
				}
				if (listing_Standard.ButtonText("NewArea".Translate(), null))
				{
					Area_Allowed area_Allowed;
					this.map.areaManager.TryMakeNewAllowed(out area_Allowed);
				}
			}
			listing_Standard.End();
		}

		// Token: 0x06007473 RID: 29811 RVA: 0x002788D4 File Offset: 0x00276AD4
		private static void DoAreaRow(Rect rect, Area area)
		{
			if (Mouse.IsOver(rect))
			{
				area.MarkForDraw();
				GUI.color = area.Color;
				Widgets.DrawHighlight(rect);
				GUI.color = Color.white;
			}
			GUI.BeginGroup(rect);
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			widgetRow.Icon(area.ColorTexture, null);
			widgetRow.Gap(4f);
			float width = rect.width - widgetRow.FinalX - 4f - Text.CalcSize("Rename".Translate()).x - 16f - 4f - Text.CalcSize("InvertArea".Translate()).x - 16f - 4f - 24f;
			widgetRow.Label(area.Label, width, null, -1f);
			if (widgetRow.ButtonText("Rename".Translate(), null, true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_RenameArea(area));
			}
			if (widgetRow.ButtonText("InvertArea".Translate(), null, true, true, true, null))
			{
				area.Invert();
			}
			if (widgetRow.ButtonIcon(TexButton.DeleteX, null, new Color?(GenUI.SubtleMouseoverColor), null, null, true))
			{
				area.Delete();
			}
			GUI.EndGroup();
		}

		// Token: 0x06007474 RID: 29812 RVA: 0x00278A54 File Offset: 0x00276C54
		public static void DoNameInputRect(Rect rect, ref string name, int maxLength)
		{
			string text = Widgets.TextField(rect, name);
			if (text.Length <= maxLength && Dialog_ManageAreas.validNameRegex.IsMatch(text))
			{
				name = text;
			}
		}

		// Token: 0x04004028 RID: 16424
		private Map map;

		// Token: 0x04004029 RID: 16425
		private static Regex validNameRegex = new Regex("^[\\p{L}0-9 '\\-]*$");
	}
}
