using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001366 RID: 4966
	public class MainTabWindow_Architect : MainTabWindow
	{
		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x0600786F RID: 30831 RVA: 0x002A741B File Offset: 0x002A561B
		public float WinHeight
		{
			get
			{
				if (this.desPanelsCached == null)
				{
					this.CacheDesPanels();
				}
				return (float)Mathf.CeilToInt((float)this.desPanelsCached.Count / 2f) * 32f + 28f;
			}
		}

		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x06007870 RID: 30832 RVA: 0x002A744F File Offset: 0x002A564F
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(200f, this.WinHeight);
			}
		}

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x06007871 RID: 30833 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06007872 RID: 30834 RVA: 0x002A7461 File Offset: 0x002A5661
		public MainTabWindow_Architect()
		{
			this.closeOnAccept = false;
			this.quickSearchWidget.inactiveTextColor = MainTabWindow_Architect.InactiveSearchColor;
			this.CacheDesPanels();
		}

		// Token: 0x06007873 RID: 30835 RVA: 0x002A7491 File Offset: 0x002A5691
		public override void PreOpen()
		{
			base.PreOpen();
			this.quickSearchWidget.Reset();
			this.quickSearchWidget.Unfocus();
			this.CacheSearchState();
			this.forceActivatedCommand = null;
		}

		// Token: 0x06007874 RID: 30836 RVA: 0x002A74BC File Offset: 0x002A56BC
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			ArchitectCategoryTab architectCategoryTab = this.OpenTab();
			if (architectCategoryTab != null && architectCategoryTab.def.showPowerGrid)
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
			}
		}

		// Token: 0x06007875 RID: 30837 RVA: 0x002A74EB File Offset: 0x002A56EB
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			ArchitectCategoryTab architectCategoryTab = this.OpenTab();
			if (architectCategoryTab != null)
			{
				architectCategoryTab.DesignationTabOnGUI(this.forceActivatedCommand);
			}
			this.forceActivatedCommand = null;
		}

		// Token: 0x06007876 RID: 30838 RVA: 0x002A7514 File Offset: 0x002A5714
		private ArchitectCategoryTab OpenTab()
		{
			if (!this.quickSearchWidget.filter.Active || this.userForcedSelectionDuringSearch)
			{
				return this.selectedDesPanel;
			}
			if (this.selectedDesPanel != null && this.selectedDesPanel.AnySearchMatches)
			{
				return this.selectedDesPanel;
			}
			foreach (ArchitectCategoryTab architectCategoryTab in this.desPanelsCached)
			{
				if (architectCategoryTab.AnySearchMatches)
				{
					return architectCategoryTab;
				}
			}
			return null;
		}

		// Token: 0x06007877 RID: 30839 RVA: 0x002A75AC File Offset: 0x002A57AC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			float num = inRect.width / 2f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			ArchitectCategoryTab architectCategoryTab = this.OpenTab();
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				if (this.quickSearchWidget.filter.Active && architectCategoryTab != null && architectCategoryTab.UniqueSearchMatch != null)
				{
					this.forceActivatedCommand = architectCategoryTab.UniqueSearchMatch;
				}
				else
				{
					this.Close(true);
				}
				Event.current.Use();
			}
			for (int i = 0; i < this.desPanelsCached.Count; i++)
			{
				Rect rect = new Rect(num2 * num, num3 * 32f, num, 32f);
				float height = rect.height;
				rect.height = height + 1f;
				if (num2 == 0f)
				{
					rect.width += 1f;
				}
				ArchitectCategoryTab architectCategoryTab2 = this.desPanelsCached[i];
				Color? labelColor = architectCategoryTab2.AnySearchMatches ? null : new Color?(MainTabWindow_Architect.NoMatchColor);
				string label = architectCategoryTab2.def.LabelCap;
				if (Widgets.ButtonTextSubtle(rect, label, 0f, 8f, SoundDefOf.Mouseover_Category, new Vector2(-1f, -1f), labelColor, this.quickSearchWidget.filter.Active && architectCategoryTab == architectCategoryTab2))
				{
					this.ClickedCategory(architectCategoryTab2);
				}
				if (this.selectedDesPanel != architectCategoryTab2)
				{
					UIHighlighter.HighlightOpportunity(rect, architectCategoryTab2.def.cachedHighlightClosedTag);
				}
				num4 = Mathf.Max(num4, rect.yMax);
				num2 += 1f;
				if (num2 > 1f)
				{
					num2 = 0f;
					num3 += 1f;
				}
			}
			Rect rect2 = new Rect(0f, num4 + 1f, inRect.width, 24f);
			this.quickSearchWidget.OnGUI(rect2, new Action(this.CacheSearchState));
		}

		// Token: 0x06007878 RID: 30840 RVA: 0x002A77B0 File Offset: 0x002A59B0
		private void CacheDesPanels()
		{
			this.desPanelsCached = new List<ArchitectCategoryTab>();
			foreach (DesignationCategoryDef def in from dc in DefDatabase<DesignationCategoryDef>.AllDefs
			orderby dc.order descending
			select dc)
			{
				this.desPanelsCached.Add(new ArchitectCategoryTab(def, this.quickSearchWidget.filter));
			}
		}

		// Token: 0x06007879 RID: 30841 RVA: 0x002A7840 File Offset: 0x002A5A40
		private void CacheSearchState()
		{
			if (this.desPanelsCached == null)
			{
				this.CacheDesPanels();
			}
			bool flag = false;
			foreach (ArchitectCategoryTab architectCategoryTab in this.desPanelsCached)
			{
				architectCategoryTab.CacheSearchState();
				flag |= architectCategoryTab.AnySearchMatches;
			}
			this.quickSearchWidget.noResultsMatched = !flag;
			if (!this.quickSearchWidget.filter.Active)
			{
				this.userForcedSelectionDuringSearch = false;
			}
		}

		// Token: 0x0600787A RID: 30842 RVA: 0x002A78D4 File Offset: 0x002A5AD4
		protected void ClickedCategory(ArchitectCategoryTab Pan)
		{
			if (this.selectedDesPanel == Pan && !this.quickSearchWidget.filter.Active)
			{
				this.selectedDesPanel = null;
			}
			else
			{
				this.selectedDesPanel = Pan;
			}
			if (this.quickSearchWidget.filter.Active)
			{
				this.userForcedSelectionDuringSearch = true;
			}
			SoundDefOf.ArchitectCategorySelect.PlayOneShotOnCamera(null);
		}

		// Token: 0x0600787B RID: 30843 RVA: 0x002A7930 File Offset: 0x002A5B30
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			this.quickSearchWidget.Unfocus();
		}

		// Token: 0x040042E7 RID: 17127
		private List<ArchitectCategoryTab> desPanelsCached;

		// Token: 0x040042E8 RID: 17128
		public ArchitectCategoryTab selectedDesPanel;

		// Token: 0x040042E9 RID: 17129
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x040042EA RID: 17130
		private bool userForcedSelectionDuringSearch;

		// Token: 0x040042EB RID: 17131
		private Designator forceActivatedCommand;

		// Token: 0x040042EC RID: 17132
		public const float WinWidth = 200f;

		// Token: 0x040042ED RID: 17133
		private const float ButHeight = 32f;

		// Token: 0x040042EE RID: 17134
		private static readonly Color InactiveSearchColor = Color.white.ToTransparent(0.4f);

		// Token: 0x040042EF RID: 17135
		private static readonly Color NoMatchColor = Color.grey;
	}
}
