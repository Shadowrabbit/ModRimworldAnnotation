using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001811 RID: 6161
	public class WITab_Caravan_Needs : WITab
	{
		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x0600904B RID: 36939 RVA: 0x0033BD3B File Offset: 0x00339F3B
		private float SpecificNeedsTabWidth
		{
			get
			{
				if (this.specificNeedsTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return NeedsCardUtility.GetSize(this.specificNeedsTabForPawn).x;
			}
		}

		// Token: 0x0600904C RID: 36940 RVA: 0x0033BD60 File Offset: 0x00339F60
		public WITab_Caravan_Needs()
		{
			this.labelKey = "TabCaravanNeeds";
		}

		// Token: 0x0600904D RID: 36941 RVA: 0x0033BD73 File Offset: 0x00339F73
		protected override void FillTab()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			CaravanNeedsTabUtility.DoRows(this.size, base.SelCaravan.PawnsListForReading, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight, ref this.specificNeedsTabForPawn, this.doNeeds);
		}

		// Token: 0x0600904E RID: 36942 RVA: 0x0033BDB0 File Offset: 0x00339FB0
		protected override void UpdateSize()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			base.UpdateSize();
			this.size = CaravanNeedsTabUtility.GetSize(base.SelCaravan.PawnsListForReading, this.PaneTopY, true);
			if (this.size.x + this.SpecificNeedsTabWidth > (float)UI.screenWidth)
			{
				this.doNeeds = false;
				this.size = CaravanNeedsTabUtility.GetSize(base.SelCaravan.PawnsListForReading, this.PaneTopY, false);
			}
			else
			{
				this.doNeeds = true;
			}
			this.size.y = Mathf.Max(this.size.y, NeedsCardUtility.FullSize.y);
		}

		// Token: 0x0600904F RID: 36943 RVA: 0x0033BE54 File Offset: 0x0033A054
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificNeedsTabForPawn = this.specificNeedsTabForPawn;
			if (localSpecificNeedsTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificNeedsTabWidth = this.SpecificNeedsTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificNeedsTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificNeedsTabForPawn.DestroyedOrNull())
					{
						return;
					}
					NeedsCardUtility.DoNeedsMoodAndThoughts(rect.AtZero(), localSpecificNeedsTabForPawn, ref this.thoughtScrollPosition);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificNeedsTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f, null);
			}
		}

		// Token: 0x06009050 RID: 36944 RVA: 0x0033BEFA File Offset: 0x0033A0FA
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificNeedsTabForPawn = null;
		}

		// Token: 0x06009051 RID: 36945 RVA: 0x0033BF09 File Offset: 0x0033A109
		private void EnsureSpecificNeedsTabForPawnValid()
		{
			if (this.specificNeedsTabForPawn != null && (this.specificNeedsTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificNeedsTabForPawn)))
			{
				this.specificNeedsTabForPawn = null;
			}
		}

		// Token: 0x04005AC5 RID: 23237
		private Vector2 scrollPosition;

		// Token: 0x04005AC6 RID: 23238
		private float scrollViewHeight;

		// Token: 0x04005AC7 RID: 23239
		private Pawn specificNeedsTabForPawn;

		// Token: 0x04005AC8 RID: 23240
		private Vector2 thoughtScrollPosition;

		// Token: 0x04005AC9 RID: 23241
		private bool doNeeds;
	}
}
