using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200133A RID: 4922
	public class ArchitectCategoryTab
	{
		// Token: 0x170014DA RID: 5338
		// (get) Token: 0x0600771A RID: 30490 RVA: 0x0029D301 File Offset: 0x0029B501
		public bool AnySearchMatches
		{
			get
			{
				return this.anySearchMatches;
			}
		}

		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x0600771B RID: 30491 RVA: 0x0029D309 File Offset: 0x0029B509
		public Designator UniqueSearchMatch
		{
			get
			{
				return this.uniqueSearchMatch;
			}
		}

		// Token: 0x170014DC RID: 5340
		// (get) Token: 0x0600771C RID: 30492 RVA: 0x0029D311 File Offset: 0x0029B511
		public static Rect InfoRect
		{
			get
			{
				return new Rect(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f, 200f, 270f);
			}
		}

		// Token: 0x0600771D RID: 30493 RVA: 0x0029D34B File Offset: 0x0029B54B
		public ArchitectCategoryTab(DesignationCategoryDef def, QuickSearchFilter quickSearchFilter)
		{
			this.def = def;
			this.quickSearchFilter = quickSearchFilter;
			this.shouldLowLightGizmoFunc = new Func<Gizmo, bool>(this.ShouldLowLightGizmo);
			this.shouldHighLightGizmoFunc = new Func<Gizmo, bool>(this.ShouldHighLightGizmo);
		}

		// Token: 0x0600771E RID: 30494 RVA: 0x0029D388 File Offset: 0x0029B588
		public void DesignationTabOnGUI(Designator forceActivatedCommand)
		{
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f);
			}
			Func<Gizmo, bool> customActivatorFunc = (forceActivatedCommand == null) ? null : new Func<Gizmo, bool>((Gizmo cmd) => cmd == forceActivatedCommand);
			float startX = 210f;
			Gizmo selectedDesignator;
			GizmoGridDrawer.DrawGizmoGrid(this.def.ResolvedAllowedDesignators, startX, out selectedDesignator, customActivatorFunc, this.shouldHighLightGizmoFunc, this.shouldLowLightGizmoFunc);
			if (selectedDesignator == null && Find.DesignatorManager.SelectedDesignator != null)
			{
				selectedDesignator = Find.DesignatorManager.SelectedDesignator;
			}
			this.DoInfoBox(ArchitectCategoryTab.InfoRect, (Designator)selectedDesignator);
		}

		// Token: 0x0600771F RID: 30495 RVA: 0x0029D450 File Offset: 0x0029B650
		private bool ShouldLowLightGizmo(Gizmo gizmo)
		{
			Command command = gizmo as Command;
			return command != null && (this.quickSearchFilter.Active && !this.Matches(command));
		}

		// Token: 0x06007720 RID: 30496 RVA: 0x0029D484 File Offset: 0x0029B684
		private bool ShouldHighLightGizmo(Gizmo gizmo)
		{
			Command command = gizmo as Command;
			return command != null && (this.quickSearchFilter.Active && this.Matches(command));
		}

		// Token: 0x06007721 RID: 30497 RVA: 0x0029D4B6 File Offset: 0x0029B6B6
		private bool Matches(Command c)
		{
			return this.quickSearchFilter.Matches(c.Label);
		}

		// Token: 0x06007722 RID: 30498 RVA: 0x0029D4CC File Offset: 0x0029B6CC
		protected void DoInfoBox(Rect infoRect, Designator designator)
		{
			Find.WindowStack.ImmediateWindow(32520, infoRect, WindowLayer.GameUI, delegate
			{
				if (designator != null)
				{
					Rect position = infoRect.AtZero().ContractedBy(7f);
					GUI.BeginGroup(position);
					Rect rect = new Rect(0f, 0f, position.width - designator.PanelReadoutTitleExtraRightMargin, 999f);
					Text.Font = GameFont.Small;
					Widgets.Label(rect, designator.LabelCap);
					float num = Mathf.Max(24f, Text.CalcHeight(designator.LabelCap, rect.width));
					designator.DrawPanelReadout(ref num, position.width);
					Rect rect2 = new Rect(0f, num, position.width, position.height - num);
					string text = designator.Desc;
					GenText.SetTextSizeToFit(text, rect2);
					text = text.TruncateHeight(rect2.width, rect2.height, null);
					Widgets.Label(rect2, text);
					GUI.EndGroup();
				}
			}, true, false, 1f, null);
		}

		// Token: 0x06007723 RID: 30499 RVA: 0x0029D518 File Offset: 0x0029B718
		public void CacheSearchState()
		{
			this.anySearchMatches = true;
			this.uniqueSearchMatch = null;
			if (!this.quickSearchFilter.Active)
			{
				return;
			}
			int num = 0;
			Designator designator = null;
			foreach (Designator designator2 in this.def.ResolvedAllowedDesignators)
			{
				if (this.Matches(designator2))
				{
					num++;
					designator = designator2;
					if (num > 1)
					{
						return;
					}
				}
			}
			if (num == 0)
			{
				this.anySearchMatches = false;
				return;
			}
			if (num == 1)
			{
				this.uniqueSearchMatch = designator;
			}
		}

		// Token: 0x04004232 RID: 16946
		public readonly DesignationCategoryDef def;

		// Token: 0x04004233 RID: 16947
		private readonly QuickSearchFilter quickSearchFilter;

		// Token: 0x04004234 RID: 16948
		private bool anySearchMatches;

		// Token: 0x04004235 RID: 16949
		private Designator uniqueSearchMatch;

		// Token: 0x04004236 RID: 16950
		private readonly Func<Gizmo, bool> shouldLowLightGizmoFunc;

		// Token: 0x04004237 RID: 16951
		private readonly Func<Gizmo, bool> shouldHighLightGizmoFunc;

		// Token: 0x04004238 RID: 16952
		public const float InfoRectHeight = 270f;
	}
}
