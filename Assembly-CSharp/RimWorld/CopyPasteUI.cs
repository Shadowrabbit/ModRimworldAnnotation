using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001987 RID: 6535
	public static class CopyPasteUI
	{
		// Token: 0x0600908D RID: 37005 RVA: 0x0029A6AC File Offset: 0x002988AC
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

		// Token: 0x04005BFA RID: 23546
		public const float CopyPasteIconHeight = 24f;

		// Token: 0x04005BFB RID: 23547
		public const float CopyPasteIconWidth = 18f;

		// Token: 0x04005BFC RID: 23548
		public const float CopyPasteColumnWidth = 36f;
	}
}
