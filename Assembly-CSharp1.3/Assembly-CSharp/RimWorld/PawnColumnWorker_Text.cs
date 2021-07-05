using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200139F RID: 5023
	public abstract class PawnColumnWorker_Text : PawnColumnWorker
	{
		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x06007A3C RID: 31292 RVA: 0x002B20D3 File Offset: 0x002B02D3
		protected virtual int Width
		{
			get
			{
				return this.def.width;
			}
		}

		// Token: 0x06007A3D RID: 31293 RVA: 0x002AF436 File Offset: 0x002AD636
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06007A3E RID: 31294 RVA: 0x002B20E0 File Offset: 0x002B02E0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, Mathf.Min(rect.height, 30f));
			string textFor = this.GetTextFor(pawn);
			if (textFor != null)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Text.WordWrap = false;
				Widgets.Label(rect2, textFor);
				Text.WordWrap = true;
				Text.Anchor = TextAnchor.UpperLeft;
				if (Mouse.IsOver(rect2))
				{
					string tip = this.GetTip(pawn);
					if (!tip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, tip);
					}
				}
			}
		}

		// Token: 0x06007A3F RID: 31295 RVA: 0x002B216E File Offset: 0x002B036E
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06007A40 RID: 31296 RVA: 0x002B2182 File Offset: 0x002B0382
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnColumnWorker_Text.comparer.Compare(this.GetTextFor(a), this.GetTextFor(b));
		}

		// Token: 0x06007A41 RID: 31297
		protected abstract string GetTextFor(Pawn pawn);

		// Token: 0x06007A42 RID: 31298 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x04004397 RID: 17303
		private static NumericStringComparer comparer = new NumericStringComparer();
	}
}
