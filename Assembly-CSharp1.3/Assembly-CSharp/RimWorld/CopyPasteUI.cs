using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001299 RID: 4761
	public static class CopyPasteUI
	{
		// Token: 0x060071CA RID: 29130 RVA: 0x00260C6C File Offset: 0x0025EE6C
		public static void DoCopyPasteButtons(Rect rect, Action copyAction, Action pasteAction)
		{
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height / 2f - 12f), 18f, 24f);
			if (Widgets.ButtonImage(rect2, TexButton.Copy, true))
			{
				copyAction();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect2, "Copy");
			if (pasteAction != null)
			{
				Rect rect3 = rect2;
				rect3.x = rect2.xMax;
				if (Widgets.ButtonImage(rect3, TexButton.Paste, true))
				{
					pasteAction();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect3, "Paste");
			}
		}

		// Token: 0x04003EB0 RID: 16048
		public const float CopyPasteIconHeight = 24f;

		// Token: 0x04003EB1 RID: 16049
		public const float CopyPasteIconWidth = 18f;

		// Token: 0x04003EB2 RID: 16050
		public const float CopyPasteColumnWidth = 36f;
	}
}
