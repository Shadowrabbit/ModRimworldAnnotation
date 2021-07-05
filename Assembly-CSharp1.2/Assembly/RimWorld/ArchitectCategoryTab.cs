using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AEA RID: 6890
	public class ArchitectCategoryTab
	{
		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x060097C3 RID: 38851 RVA: 0x00065170 File Offset: 0x00063370
		public static Rect InfoRect
		{
			get
			{
				return new Rect(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f, 200f, 270f);
			}
		}

		// Token: 0x060097C4 RID: 38852 RVA: 0x000651AA File Offset: 0x000633AA
		public ArchitectCategoryTab(DesignationCategoryDef def)
		{
			this.def = def;
		}

		// Token: 0x060097C5 RID: 38853 RVA: 0x002C8C68 File Offset: 0x002C6E68
		public void DesignationTabOnGUI()
		{
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f);
			}
			float startX = 210f;
			Gizmo selectedDesignator;
			GizmoGridDrawer.DrawGizmoGrid(this.def.ResolvedAllowedDesignators.Cast<Gizmo>(), startX, out selectedDesignator);
			if (selectedDesignator == null && Find.DesignatorManager.SelectedDesignator != null)
			{
				selectedDesignator = Find.DesignatorManager.SelectedDesignator;
			}
			this.DoInfoBox(ArchitectCategoryTab.InfoRect, (Designator)selectedDesignator);
		}

		// Token: 0x060097C6 RID: 38854 RVA: 0x002C8D04 File Offset: 0x002C6F04
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
			}, true, false, 1f);
		}

		// Token: 0x040060F8 RID: 24824
		public DesignationCategoryDef def;

		// Token: 0x040060F9 RID: 24825
		public const float InfoRectHeight = 270f;
	}
}
