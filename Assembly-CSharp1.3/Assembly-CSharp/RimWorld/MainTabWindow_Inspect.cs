using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136B RID: 4971
	[StaticConstructorOnStartup]
	public class MainTabWindow_Inspect : MainTabWindow, IInspectPane
	{
		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x06007891 RID: 30865 RVA: 0x002A8A47 File Offset: 0x002A6C47
		// (set) Token: 0x06007892 RID: 30866 RVA: 0x002A8A4F File Offset: 0x002A6C4F
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

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x06007893 RID: 30867 RVA: 0x002A8A58 File Offset: 0x002A6C58
		// (set) Token: 0x06007894 RID: 30868 RVA: 0x002A8A60 File Offset: 0x002A6C60
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

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x06007895 RID: 30869 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x06007896 RID: 30870 RVA: 0x002A8A69 File Offset: 0x002A6C69
		public override Vector2 RequestedTabSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x06007897 RID: 30871 RVA: 0x002A8A71 File Offset: 0x002A6C71
		private List<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x06007898 RID: 30872 RVA: 0x0029D820 File Offset: 0x0029BA20
		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x06007899 RID: 30873 RVA: 0x00266976 File Offset: 0x00264B76
		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x0600789A RID: 30874 RVA: 0x002A8A7D File Offset: 0x002A6C7D
		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x0600789B RID: 30875 RVA: 0x002A8A89 File Offset: 0x002A6C89
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - 165f - 35f;
			}
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x0600789C RID: 30876 RVA: 0x002A8A9D File Offset: 0x002A6C9D
		public bool AnythingSelected
		{
			get
			{
				return this.NumSelected > 0;
			}
		}

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x0600789D RID: 30877 RVA: 0x002A8AA8 File Offset: 0x002A6CA8
		public Gizmo LastMouseoverGizmo
		{
			get
			{
				return this.lastMouseOverGizmo;
			}
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x0600789E RID: 30878 RVA: 0x002A8AB0 File Offset: 0x002A6CB0
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.NumSelected == 1 && (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell));
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x0600789F RID: 30879 RVA: 0x002A8ADF File Offset: 0x002A6CDF
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.NumSelected == 1;
			}
		}

		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x060078A0 RID: 30880 RVA: 0x002A8AEC File Offset: 0x002A6CEC
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

		// Token: 0x060078A1 RID: 30881 RVA: 0x002A8B3D File Offset: 0x002A6D3D
		public MainTabWindow_Inspect()
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.soundClose = SoundDefOf.TabClose;
		}

		// Token: 0x060078A2 RID: 30882 RVA: 0x002A8B5E File Offset: 0x002A6D5E
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
			if (this.AnythingSelected && Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, this.PaneTopY);
			}
		}

		// Token: 0x060078A3 RID: 30883 RVA: 0x002A8B9A File Offset: 0x002A6D9A
		public override void DoWindowContents(Rect inRect)
		{
			InspectPaneUtility.InspectPaneOnGUI(inRect, this);
			if (this.lastSelectedThing != this.SelThing)
			{
				this.SetInitialSizeAndPosition();
				this.lastSelectedThing = this.SelThing;
			}
		}

		// Token: 0x060078A4 RID: 30884 RVA: 0x002A8BC3 File Offset: 0x002A6DC3
		public string GetLabel(Rect rect)
		{
			return InspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
		}

		// Token: 0x060078A5 RID: 30885 RVA: 0x002A8BD1 File Offset: 0x002A6DD1
		public void DrawInspectGizmos()
		{
			InspectGizmoGrid.DrawInspectGizmoGridFor(this.Selected, out this.mouseoverGizmo);
		}

		// Token: 0x060078A6 RID: 30886 RVA: 0x002A8BE4 File Offset: 0x002A6DE4
		public void DoPaneContents(Rect rect)
		{
			InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect);
		}

		// Token: 0x060078A7 RID: 30887 RVA: 0x002A8BFC File Offset: 0x002A6DFC
		public void DoLabelIcons(string label)
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn != null && pawn.guilt != null && pawn.guilt.IsGuilty)
			{
				Text.Font = GameFont.Medium;
				float x = Text.CalcSize(label).x;
				Text.Font = GameFont.Small;
				Rect rect = new Rect(x + 4f, 3f, 26f, 26f);
				GUI.DrawTexture(rect, TexUI.GuiltyTex);
				TooltipHandler.TipRegion(rect, () => pawn.guilt.Tip, 6321223);
			}
		}

		// Token: 0x060078A8 RID: 30888 RVA: 0x002A8CA0 File Offset: 0x002A6EA0
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

		// Token: 0x060078A9 RID: 30889 RVA: 0x002A8D44 File Offset: 0x002A6F44
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

		// Token: 0x060078AA RID: 30890 RVA: 0x002A8DC1 File Offset: 0x002A6FC1
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

		// Token: 0x060078AB RID: 30891 RVA: 0x002A8DEE File Offset: 0x002A6FEE
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x060078AC RID: 30892 RVA: 0x002A8DEE File Offset: 0x002A6FEE
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x0400430B RID: 17163
		private Type openTabType;

		// Token: 0x0400430C RID: 17164
		private float recentHeight;

		// Token: 0x0400430D RID: 17165
		private static IntVec3 lastSelectCell;

		// Token: 0x0400430E RID: 17166
		private Gizmo mouseoverGizmo;

		// Token: 0x0400430F RID: 17167
		private Gizmo lastMouseOverGizmo;

		// Token: 0x04004310 RID: 17168
		private Thing lastSelectedThing;
	}
}
