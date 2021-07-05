using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000412 RID: 1042
	public class Listing_ScenEdit : Listing_Standard
	{
		// Token: 0x06001F33 RID: 7987 RVA: 0x000C279C File Offset: 0x000C099C
		public Listing_ScenEdit(Scenario scen)
		{
			this.scen = scen;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x000C27AC File Offset: 0x000C09AC
		public Rect GetScenPartRect(ScenPart part, float height)
		{
			string label = part.Label;
			Rect rect = base.GetRect(height);
			Widgets.DrawBoxSolid(rect, new Color(1f, 1f, 1f, 0.08f));
			WidgetRow widgetRow = new WidgetRow(rect.x, rect.y, UIDirection.RightThenDown, 72f, 0f);
			if (part.def.PlayerAddRemovable && widgetRow.ButtonIcon(TexButton.DeleteX, null, new Color?(GenUI.SubtleMouseoverColor), null, null, true))
			{
				this.scen.RemovePart(part);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (this.scen.CanReorder(part, ReorderDirection.Up) && widgetRow.ButtonIcon(TexButton.ReorderUp, null, null, null, null, true))
			{
				this.scen.Reorder(part, ReorderDirection.Up);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			if (this.scen.CanReorder(part, ReorderDirection.Down) && widgetRow.ButtonIcon(TexButton.ReorderDown, null, null, null, null, true))
			{
				this.scen.Reorder(part, ReorderDirection.Down);
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
			}
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect2 = rect.LeftPart(0.5f).Rounded();
			rect2.xMax -= 4f;
			Widgets.Label(rect2, label);
			Text.Anchor = TextAnchor.UpperLeft;
			base.Gap(4f);
			return rect.RightPart(0.5f).Rounded();
		}

		// Token: 0x04001304 RID: 4868
		private Scenario scen;
	}
}
