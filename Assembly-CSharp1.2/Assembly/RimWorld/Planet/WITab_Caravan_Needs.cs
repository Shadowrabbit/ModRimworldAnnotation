using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020021DB RID: 8667
	public class WITab_Caravan_Needs : WITab
	{
		// Token: 0x17001B8F RID: 7055
		// (get) Token: 0x0600B991 RID: 47505 RVA: 0x0007822A File Offset: 0x0007642A
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

		// Token: 0x0600B992 RID: 47506 RVA: 0x0007824F File Offset: 0x0007644F
		public WITab_Caravan_Needs()
		{
			this.labelKey = "TabCaravanNeeds";
		}

		// Token: 0x0600B993 RID: 47507 RVA: 0x00078262 File Offset: 0x00076462
		protected override void FillTab()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			CaravanNeedsTabUtility.DoRows(this.size, base.SelCaravan.PawnsListForReading, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight, ref this.specificNeedsTabForPawn, this.doNeeds);
		}

		// Token: 0x0600B994 RID: 47508 RVA: 0x00356528 File Offset: 0x00354728
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

		// Token: 0x0600B995 RID: 47509 RVA: 0x003565CC File Offset: 0x003547CC
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
				}, true, false, 1f);
			}
		}

		// Token: 0x0600B996 RID: 47510 RVA: 0x0007829E File Offset: 0x0007649E
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificNeedsTabForPawn = null;
		}

		// Token: 0x0600B997 RID: 47511 RVA: 0x000782AD File Offset: 0x000764AD
		private void EnsureSpecificNeedsTabForPawnValid()
		{
			if (this.specificNeedsTabForPawn != null && (this.specificNeedsTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificNeedsTabForPawn)))
			{
				this.specificNeedsTabForPawn = null;
			}
		}

		// Token: 0x04007ECD RID: 32461
		private Vector2 scrollPosition;

		// Token: 0x04007ECE RID: 32462
		private float scrollViewHeight;

		// Token: 0x04007ECF RID: 32463
		private Pawn specificNeedsTabForPawn;

		// Token: 0x04007ED0 RID: 32464
		private Vector2 thoughtScrollPosition;

		// Token: 0x04007ED1 RID: 32465
		private bool doNeeds;
	}
}
