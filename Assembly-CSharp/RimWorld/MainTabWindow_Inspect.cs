using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3F RID: 6975
	[StaticConstructorOnStartup]
	public class MainTabWindow_Inspect : MainTabWindow, IInspectPane
	{
		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x0600998E RID: 39310 RVA: 0x00066558 File Offset: 0x00064758
		// (set) Token: 0x0600998F RID: 39311 RVA: 0x00066560 File Offset: 0x00064760
		public Type OpenTabType
		{
			get
			{
				return this.openTabType;
			}
			set
			{
				this.openTabType = value;
			}
		}

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x06009990 RID: 39312 RVA: 0x00066569 File Offset: 0x00064769
		// (set) Token: 0x06009991 RID: 39313 RVA: 0x00066571 File Offset: 0x00064771
		public float RecentHeight
		{
			get
			{
				return this.recentHeight;
			}
			set
			{
				this.recentHeight = value;
			}
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x06009992 RID: 39314 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x06009993 RID: 39315 RVA: 0x0006657A File Offset: 0x0006477A
		public override Vector2 RequestedTabSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x1700183D RID: 6205
		// (get) Token: 0x06009994 RID: 39316 RVA: 0x00066582 File Offset: 0x00064782
		private List<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x06009995 RID: 39317 RVA: 0x000653FD File Offset: 0x000635FD
		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x06009996 RID: 39318 RVA: 0x00061C86 File Offset: 0x0005FE86
		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x06009997 RID: 39319 RVA: 0x0006658E File Offset: 0x0006478E
		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x06009998 RID: 39320 RVA: 0x0006659A File Offset: 0x0006479A
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - 165f - 35f;
			}
		}

		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x06009999 RID: 39321 RVA: 0x000665AE File Offset: 0x000647AE
		public bool AnythingSelected
		{
			get
			{
				return this.NumSelected > 0;
			}
		}

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x0600999A RID: 39322 RVA: 0x000665B9 File Offset: 0x000647B9
		public Gizmo LastMouseoverGizmo
		{
			get
			{
				return this.lastMouseOverGizmo;
			}
		}

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x0600999B RID: 39323 RVA: 0x000665C1 File Offset: 0x000647C1
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.NumSelected == 1 && (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell));
			}
		}

		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x0600999C RID: 39324 RVA: 0x000665F0 File Offset: 0x000647F0
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.NumSelected == 1;
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x0600999D RID: 39325 RVA: 0x002D1FE0 File Offset: 0x002D01E0
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (this.NumSelected == 1)
				{
					if (this.SelThing != null && this.SelThing.def.inspectorTabsResolved != null)
					{
						return this.SelThing.GetInspectTabs();
					}
					if (this.SelZone != null)
					{
						return this.SelZone.GetInspectTabs();
					}
				}
				return null;
			}
		}

		// Token: 0x0600999E RID: 39326 RVA: 0x000665FB File Offset: 0x000647FB
		public MainTabWindow_Inspect()
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.soundClose = SoundDefOf.TabClose;
		}

		// Token: 0x0600999F RID: 39327 RVA: 0x0006661C File Offset: 0x0006481C
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
			if (this.AnythingSelected && Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, this.PaneTopY);
			}
		}

		// Token: 0x060099A0 RID: 39328 RVA: 0x00066658 File Offset: 0x00064858
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			InspectPaneUtility.InspectPaneOnGUI(inRect, this);
		}

		// Token: 0x060099A1 RID: 39329 RVA: 0x00066668 File Offset: 0x00064868
		public string GetLabel(Rect rect)
		{
			return InspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
		}

		// Token: 0x060099A2 RID: 39330 RVA: 0x00066676 File Offset: 0x00064876
		public void DrawInspectGizmos()
		{
			InspectGizmoGrid.DrawInspectGizmoGridFor(this.Selected, out this.mouseoverGizmo);
		}

		// Token: 0x060099A3 RID: 39331 RVA: 0x00066689 File Offset: 0x00064889
		public void DoPaneContents(Rect rect)
		{
			InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect);
		}

		// Token: 0x060099A4 RID: 39332 RVA: 0x002D2034 File Offset: 0x002D0234
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			if (this.NumSelected == 1)
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing != null)
				{
					Widgets.InfoCardButton(rect.width - 48f, 0f, Find.Selector.SingleSelectedThing);
					lineEndWidth += 24f;
					Pawn pawn = singleSelectedThing as Pawn;
					if (pawn != null && pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse)
					{
						HostilityResponseModeUtility.DrawResponseButton(new Rect(rect.width - 72f, 0f, 24f, 24f), pawn, false);
						lineEndWidth += 24f;
					}
				}
			}
		}

		// Token: 0x060099A5 RID: 39333 RVA: 0x002D20D8 File Offset: 0x002D02D8
		public void SelectNextInCell()
		{
			if (this.NumSelected != 1)
			{
				return;
			}
			Selector selector = Find.Selector;
			if (selector.SelectedZone == null || selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell))
			{
				if (selector.SelectedZone == null)
				{
					MainTabWindow_Inspect.lastSelectCell = selector.SingleSelectedThing.Position;
				}
				Map map;
				if (selector.SingleSelectedThing != null)
				{
					map = selector.SingleSelectedThing.Map;
				}
				else
				{
					map = selector.SelectedZone.Map;
				}
				selector.SelectNextAt(MainTabWindow_Inspect.lastSelectCell, map);
			}
		}

		// Token: 0x060099A6 RID: 39334 RVA: 0x000666A0 File Offset: 0x000648A0
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
			this.lastMouseOverGizmo = this.mouseoverGizmo;
			if (this.mouseoverGizmo != null)
			{
				this.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x060099A7 RID: 39335 RVA: 0x000666CD File Offset: 0x000648CD
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x060099A8 RID: 39336 RVA: 0x000666CD File Offset: 0x000648CD
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x04006224 RID: 25124
		private Type openTabType;

		// Token: 0x04006225 RID: 25125
		private float recentHeight;

		// Token: 0x04006226 RID: 25126
		private static IntVec3 lastSelectCell;

		// Token: 0x04006227 RID: 25127
		private Gizmo mouseoverGizmo;

		// Token: 0x04006228 RID: 25128
		private Gizmo lastMouseOverGizmo;
	}
}
