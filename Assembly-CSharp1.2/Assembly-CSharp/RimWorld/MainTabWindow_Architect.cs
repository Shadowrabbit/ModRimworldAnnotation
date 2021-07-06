using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B37 RID: 6967
	public class MainTabWindow_Architect : MainTabWindow
	{
		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x06009967 RID: 39271 RVA: 0x00066396 File Offset: 0x00064596
		public float WinHeight
		{
			get
			{
				if (this.desPanelsCached == null)
				{
					this.CacheDesPanels();
				}
				return (float)Mathf.CeilToInt((float)this.desPanelsCached.Count / 2f) * 32f;
			}
		}

		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x06009968 RID: 39272 RVA: 0x000663C4 File Offset: 0x000645C4
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(200f, this.WinHeight);
			}
		}

		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x06009969 RID: 39273 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600996A RID: 39274 RVA: 0x000663D6 File Offset: 0x000645D6
		public MainTabWindow_Architect()
		{
			this.CacheDesPanels();
		}

		// Token: 0x0600996B RID: 39275 RVA: 0x000663E4 File Offset: 0x000645E4
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x0600996C RID: 39276 RVA: 0x000663FC File Offset: 0x000645FC
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (this.selectedDesPanel != null && this.selectedDesPanel.def.showPowerGrid)
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
			}
		}

		// Token: 0x0600996D RID: 39277 RVA: 0x00066423 File Offset: 0x00064623
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.selectedDesPanel != null)
			{
				this.selectedDesPanel.DesignationTabOnGUI();
			}
		}

		// Token: 0x0600996E RID: 39278 RVA: 0x002D0E0C File Offset: 0x002CF00C
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			Text.Font = GameFont.Small;
			float num = inRect.width / 2f;
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this.desPanelsCached.Count; i++)
			{
				Rect rect = new Rect(num2 * num, num3 * 32f, num, 32f);
				float height = rect.height;
				rect.height = height + 1f;
				if (num2 == 0f)
				{
					rect.width += 1f;
				}
				if (Widgets.ButtonTextSubtle(rect, this.desPanelsCached[i].def.LabelCap, 0f, 8f, SoundDefOf.Mouseover_Category, new Vector2(-1f, -1f)))
				{
					this.ClickedCategory(this.desPanelsCached[i]);
				}
				if (this.selectedDesPanel != this.desPanelsCached[i])
				{
					UIHighlighter.HighlightOpportunity(rect, this.desPanelsCached[i].def.cachedHighlightClosedTag);
				}
				num2 += 1f;
				if (num2 > 1f)
				{
					num2 = 0f;
					num3 += 1f;
				}
			}
		}

		// Token: 0x0600996F RID: 39279 RVA: 0x002D0F48 File Offset: 0x002CF148
		private void CacheDesPanels()
		{
			this.desPanelsCached = new List<ArchitectCategoryTab>();
			foreach (DesignationCategoryDef def in from dc in DefDatabase<DesignationCategoryDef>.AllDefs
			orderby dc.order descending
			select dc)
			{
				this.desPanelsCached.Add(new ArchitectCategoryTab(def));
			}
		}

		// Token: 0x06009970 RID: 39280 RVA: 0x0006643E File Offset: 0x0006463E
		protected void ClickedCategory(ArchitectCategoryTab Pan)
		{
			if (this.selectedDesPanel == Pan)
			{
				this.selectedDesPanel = null;
			}
			else
			{
				this.selectedDesPanel = Pan;
			}
			SoundDefOf.ArchitectCategorySelect.PlayOneShotOnCamera(null);
		}

		// Token: 0x040061FA RID: 25082
		private List<ArchitectCategoryTab> desPanelsCached;

		// Token: 0x040061FB RID: 25083
		public ArchitectCategoryTab selectedDesPanel;

		// Token: 0x040061FC RID: 25084
		public const float WinWidth = 200f;

		// Token: 0x040061FD RID: 25085
		private const float ButHeight = 32f;
	}
}
