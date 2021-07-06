using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B96 RID: 7062
	public abstract class PawnColumnWorker_Text : PawnColumnWorker
	{
		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x06009B99 RID: 39833 RVA: 0x0006783B File Offset: 0x00065A3B
		protected virtual int Width
		{
			get
			{
				return this.def.width;
			}
		}

		// Token: 0x06009B9A RID: 39834 RVA: 0x00066D45 File Offset: 0x00064F45
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06009B9B RID: 39835 RVA: 0x002D9D54 File Offset: 0x002D7F54
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

		// Token: 0x06009B9C RID: 39836 RVA: 0x00067848 File Offset: 0x00065A48
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06009B9D RID: 39837 RVA: 0x0006785C File Offset: 0x00065A5C
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnColumnWorker_Text.comparer.Compare(this.GetTextFor(a), this.GetTextFor(a));
		}

		// Token: 0x06009B9E RID: 39838
		protected abstract string GetTextFor(Pawn pawn);

		// Token: 0x06009B9F RID: 39839 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0400630C RID: 25356
		private static NumericStringComparer comparer = new NumericStringComparer();
	}
}
