using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F4 RID: 6644
	public class Dialog_ManageAreas : Window
	{
		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x060092F4 RID: 37620 RVA: 0x000626B7 File Offset: 0x000608B7
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(450f, 400f);
			}
		}

		// Token: 0x060092F5 RID: 37621 RVA: 0x000626C8 File Offset: 0x000608C8
		public Dialog_ManageAreas(Map map)
		{
			this.map = map;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060092F6 RID: 37622 RVA: 0x002A5488 File Offset: 0x002A3688
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

		// Token: 0x060092F7 RID: 37623 RVA: 0x002A5560 File Offset: 0x002A3760
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
			widgetRow.Label(area.Label, width);
			if (widgetRow.ButtonText("Rename".Translate(), null, true, true))
			{
				Find.WindowStack.Add(new Dialog_RenameArea(area));
			}
			if (widgetRow.ButtonText("InvertArea".Translate(), null, true, true))
			{
				area.Invert();
			}
			if (widgetRow.ButtonIcon(TexButton.DeleteX, null, new Color?(GenUI.SubtleMouseoverColor), true))
			{
				area.Delete();
			}
			GUI.EndGroup();
		}

		// Token: 0x060092F8 RID: 37624 RVA: 0x002A56B4 File Offset: 0x002A38B4
		public static void DoNameInputRect(Rect rect, ref string name, int maxLength)
		{
			string text = Widgets.TextField(rect, name);
			if (text.Length <= maxLength && Dialog_ManageAreas.validNameRegex.IsMatch(text))
			{
				name = text;
			}
		}

		// Token: 0x04005D19 RID: 23833
		private Map map;

		// Token: 0x04005D1A RID: 23834
		private static Regex validNameRegex = new Regex("^[\\p{L}0-9 '\\-]*$");
	}
}
