using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AF3 RID: 6899
	[StaticConstructorOnStartup]
	public static class InspectPaneUtility
	{
		// Token: 0x060097F4 RID: 38900 RVA: 0x000652CD File Offset: 0x000634CD
		public static void Reset()
		{
			InspectPaneUtility.truncatedLabelsCached.Clear();
		}

		// Token: 0x060097F5 RID: 38901 RVA: 0x002C9774 File Offset: 0x002C7974
		public static float PaneWidthFor(IInspectPane pane)
		{
			if (pane == null)
			{
				return 432f;
			}
			InspectPaneUtility.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.visible = 0;
			if (pane.CurTabs != null)
			{
				IList list = pane.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						InspectPaneUtility.<PaneWidthFor>g__Process|9_0((InspectTabBase)list[i], ref CS$<>8__locals1);
					}
				}
				else
				{
					foreach (InspectTabBase tab in pane.CurTabs)
					{
						InspectPaneUtility.<PaneWidthFor>g__Process|9_0(tab, ref CS$<>8__locals1);
					}
				}
			}
			return 72f * (float)Mathf.Max(6, CS$<>8__locals1.visible);
		}

		// Token: 0x060097F6 RID: 38902 RVA: 0x000652D9 File Offset: 0x000634D9
		public static Vector2 PaneSizeFor(IInspectPane pane)
		{
			return new Vector2(InspectPaneUtility.PaneWidthFor(pane), 165f);
		}

		// Token: 0x060097F7 RID: 38903 RVA: 0x002C9824 File Offset: 0x002C7A24
		public static bool CanInspectTogether(object A, object B)
		{
			Thing thing = A as Thing;
			Thing thing2 = B as Thing;
			return thing != null && thing2 != null && thing.def.category != ThingCategory.Pawn && thing.def == thing2.def;
		}

		// Token: 0x060097F8 RID: 38904 RVA: 0x002C9868 File Offset: 0x002C7A68
		public static string AdjustedLabelFor(List<object> selected, Rect rect)
		{
			Zone zone;
			string text;
			if ((zone = (selected[0] as Zone)) != null)
			{
				if (selected.Count == 1)
				{
					text = zone.label;
				}
				else
				{
					string baseLabel = zone.BaseLabel;
					bool flag = true;
					for (int i = 1; i < selected.Count; i++)
					{
						Zone zone2;
						if ((zone2 = (selected[i] as Zone)) != null && zone2.BaseLabel != baseLabel)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						text = ((!zone.BaseLabel.NullOrEmpty()) ? zone.BaseLabel : "Zone".Translate());
					}
					else
					{
						text = "VariousLabel".Translate();
					}
					text = text + " x" + selected.Count;
				}
			}
			else
			{
				InspectPaneUtility.selectedThings.Clear();
				for (int j = 0; j < selected.Count; j++)
				{
					Thing outerThing;
					if ((outerThing = (selected[j] as Thing)) != null)
					{
						InspectPaneUtility.selectedThings.Add(outerThing.GetInnerIfMinified());
					}
				}
				if (InspectPaneUtility.selectedThings.Count == 1)
				{
					text = InspectPaneUtility.selectedThings[0].LabelCap;
				}
				else if (InspectPaneUtility.selectedThings.Count > 1)
				{
					string label = InspectPaneUtility.selectedThings[0].def.label;
					bool flag2 = true;
					for (int k = 1; k < InspectPaneUtility.selectedThings.Count; k++)
					{
						if (InspectPaneUtility.selectedThings[k].def.label != label)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						text = InspectPaneUtility.selectedThings[0].def.LabelCap;
					}
					else
					{
						text = "VariousLabel".Translate();
					}
					int num = 0;
					for (int l = 0; l < InspectPaneUtility.selectedThings.Count; l++)
					{
						num += InspectPaneUtility.selectedThings[l].stackCount;
					}
					text = text + " x" + num.ToStringCached();
				}
				else
				{
					text = "?";
				}
				InspectPaneUtility.selectedThings.Clear();
			}
			Text.Font = GameFont.Medium;
			return text.Truncate(rect.width, InspectPaneUtility.truncatedLabelsCached);
		}

		// Token: 0x060097F9 RID: 38905 RVA: 0x000652EB File Offset: 0x000634EB
		public static void ExtraOnGUI(IInspectPane pane)
		{
			if (pane.AnythingSelected)
			{
				if (KeyBindingDefOf.SelectNextInCell.KeyDownEvent)
				{
					pane.SelectNextInCell();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					pane.DrawInspectGizmos();
				}
				InspectPaneUtility.DoTabs(pane);
			}
		}

		// Token: 0x060097FA RID: 38906 RVA: 0x002C9AA8 File Offset: 0x002C7CA8
		public static void UpdateTabs(IInspectPane pane)
		{
			InspectPaneUtility.<>c__DisplayClass15_0 CS$<>8__locals1;
			CS$<>8__locals1.pane = pane;
			CS$<>8__locals1.tabUpdated = false;
			if (CS$<>8__locals1.pane.CurTabs != null)
			{
				IList list = CS$<>8__locals1.pane.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						InspectPaneUtility.<UpdateTabs>g__Update|15_0((InspectTabBase)list[i], ref CS$<>8__locals1);
					}
				}
				else
				{
					foreach (InspectTabBase tab in CS$<>8__locals1.pane.CurTabs)
					{
						InspectPaneUtility.<UpdateTabs>g__Update|15_0(tab, ref CS$<>8__locals1);
					}
				}
			}
			if (!CS$<>8__locals1.tabUpdated)
			{
				CS$<>8__locals1.pane.CloseOpenTab();
			}
		}

		// Token: 0x060097FB RID: 38907 RVA: 0x002C9B64 File Offset: 0x002C7D64
		public static void InspectPaneOnGUI(Rect inRect, IInspectPane pane)
		{
			pane.RecentHeight = 165f;
			if (pane.AnythingSelected)
			{
				try
				{
					Rect rect = inRect.ContractedBy(12f);
					rect.yMin -= 4f;
					rect.yMax += 6f;
					GUI.BeginGroup(rect);
					float num = 0f;
					if (pane.ShouldShowSelectNextInCellButton)
					{
						Rect rect2 = new Rect(rect.width - 24f, 0f, 24f, 24f);
						MouseoverSounds.DoRegion(rect2);
						if (Widgets.ButtonImage(rect2, TexButton.SelectOverlappingNext, true))
						{
							pane.SelectNextInCell();
						}
						num += 24f;
						TooltipHandler.TipRegionByKey(rect2, "SelectNextInSquareTip", KeyBindingDefOf.SelectNextInCell.MainKeyLabel);
					}
					pane.DoInspectPaneButtons(rect, ref num);
					Rect rect3 = new Rect(0f, 0f, rect.width - num, 50f);
					string label = pane.GetLabel(rect3);
					rect3.width += 300f;
					Text.Font = GameFont.Medium;
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect3, label);
					if (pane.ShouldShowPaneContents)
					{
						Rect rect4 = rect.AtZero();
						rect4.yMin += 26f;
						pane.DoPaneContents(rect4);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception doing inspect pane: " + ex.ToString(), false);
				}
				finally
				{
					GUI.EndGroup();
				}
			}
		}

		// Token: 0x060097FC RID: 38908 RVA: 0x002C9D08 File Offset: 0x002C7F08
		private static void DoTabs(IInspectPane pane)
		{
			InspectPaneUtility.<>c__DisplayClass17_0 CS$<>8__locals1;
			CS$<>8__locals1.pane = pane;
			try
			{
				InspectPaneUtility.<>c__DisplayClass17_1 CS$<>8__locals2;
				CS$<>8__locals2.tabsTopY = CS$<>8__locals1.pane.PaneTopY - 30f;
				CS$<>8__locals2.curTabX = InspectPaneUtility.PaneWidthFor(CS$<>8__locals1.pane) - 72f;
				CS$<>8__locals2.leftEdge = 0f;
				CS$<>8__locals2.drewOpen = false;
				if (CS$<>8__locals1.pane.CurTabs != null)
				{
					IList list = CS$<>8__locals1.pane.CurTabs as IList;
					if (list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							InspectPaneUtility.<DoTabs>g__Do|17_0((InspectTabBase)list[i], ref CS$<>8__locals1, ref CS$<>8__locals2);
						}
					}
					else
					{
						foreach (InspectTabBase tab in CS$<>8__locals1.pane.CurTabs)
						{
							InspectPaneUtility.<DoTabs>g__Do|17_0(tab, ref CS$<>8__locals1, ref CS$<>8__locals2);
						}
					}
				}
				if (CS$<>8__locals2.drewOpen)
				{
					GUI.DrawTexture(new Rect(0f, CS$<>8__locals2.tabsTopY, CS$<>8__locals2.leftEdge, 30f), InspectPaneUtility.InspectTabButtonFillTex);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 742783, false);
			}
		}

		// Token: 0x060097FD RID: 38909 RVA: 0x0006531B File Offset: 0x0006351B
		private static bool IsOpen(InspectTabBase tab, IInspectPane pane)
		{
			return tab.GetType() == pane.OpenTabType;
		}

		// Token: 0x060097FE RID: 38910 RVA: 0x002C9E44 File Offset: 0x002C8044
		private static void ToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (InspectPaneUtility.IsOpen(tab, pane) || (tab == null && pane.OpenTabType == null))
			{
				pane.OpenTabType = null;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				return;
			}
			tab.OnOpen();
			pane.OpenTabType = tab.GetType();
			SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
		}

		// Token: 0x060097FF RID: 38911 RVA: 0x002C9E9C File Offset: 0x002C809C
		public static InspectTabBase OpenTab(Type inspectTabType)
		{
			InspectPaneUtility.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1.inspectTabType = inspectTabType;
			MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			CS$<>8__locals1.tab = null;
			if (mainTabWindow_Inspect.CurTabs != null)
			{
				IList list = mainTabWindow_Inspect.CurTabs as IList;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (InspectPaneUtility.<OpenTab>g__Find|20_0((InspectTabBase)list[i], ref CS$<>8__locals1))
						{
							break;
						}
					}
				}
				else
				{
					using (IEnumerator<InspectTabBase> enumerator = mainTabWindow_Inspect.CurTabs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (InspectPaneUtility.<OpenTab>g__Find|20_0(enumerator.Current, ref CS$<>8__locals1))
							{
								break;
							}
						}
					}
				}
			}
			if (CS$<>8__locals1.tab != null)
			{
				if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, true);
				}
				if (!InspectPaneUtility.IsOpen(CS$<>8__locals1.tab, mainTabWindow_Inspect))
				{
					InspectPaneUtility.ToggleTab(CS$<>8__locals1.tab, mainTabWindow_Inspect);
				}
			}
			return CS$<>8__locals1.tab;
		}

		// Token: 0x06009800 RID: 38912 RVA: 0x0006532E File Offset: 0x0006352E
		private static void InterfaceToggleTab(InspectTabBase tab, IInspectPane pane)
		{
			if (TutorSystem.TutorialMode && !InspectPaneUtility.IsOpen(tab, pane) && !TutorSystem.AllowAction("ITab-" + tab.tutorTag + "-Open"))
			{
				return;
			}
			InspectPaneUtility.ToggleTab(tab, pane);
		}

		// Token: 0x06009802 RID: 38914 RVA: 0x002C9F9C File Offset: 0x002C819C
		[CompilerGenerated]
		internal static void <PaneWidthFor>g__Process|9_0(InspectTabBase tab, ref InspectPaneUtility.<>c__DisplayClass9_0 A_1)
		{
			if (tab.IsVisible)
			{
				int visible = A_1.visible;
				A_1.visible = visible + 1;
			}
		}

		// Token: 0x06009803 RID: 38915 RVA: 0x000653A2 File Offset: 0x000635A2
		[CompilerGenerated]
		internal static void <UpdateTabs>g__Update|15_0(InspectTabBase tab, ref InspectPaneUtility.<>c__DisplayClass15_0 A_1)
		{
			if (!tab.IsVisible)
			{
				return;
			}
			if (tab.GetType() == A_1.pane.OpenTabType)
			{
				tab.TabUpdate();
				A_1.tabUpdated = true;
			}
		}

		// Token: 0x06009804 RID: 38916 RVA: 0x002C9FC4 File Offset: 0x002C81C4
		[CompilerGenerated]
		internal static void <DoTabs>g__Do|17_0(InspectTabBase tab, ref InspectPaneUtility.<>c__DisplayClass17_0 A_1, ref InspectPaneUtility.<>c__DisplayClass17_1 A_2)
		{
			if (!tab.IsVisible)
			{
				return;
			}
			Rect rect = new Rect(A_2.curTabX, A_2.tabsTopY, 72f, 30f);
			A_2.leftEdge = A_2.curTabX;
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(rect, tab.labelKey.Translate(), true, true, true))
			{
				InspectPaneUtility.InterfaceToggleTab(tab, A_1.pane);
			}
			bool flag = tab.GetType() == A_1.pane.OpenTabType;
			if (!flag && !tab.TutorHighlightTagClosed.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, tab.TutorHighlightTagClosed);
			}
			if (flag)
			{
				tab.DoTabGUI();
				A_1.pane.RecentHeight = 700f;
				A_2.drewOpen = true;
			}
			A_2.curTabX -= 72f;
		}

		// Token: 0x06009805 RID: 38917 RVA: 0x000653D2 File Offset: 0x000635D2
		[CompilerGenerated]
		internal static bool <OpenTab>g__Find|20_0(InspectTabBase t, ref InspectPaneUtility.<>c__DisplayClass20_0 A_1)
		{
			if (A_1.inspectTabType.IsAssignableFrom(t.GetType()))
			{
				A_1.tab = t;
				return true;
			}
			return false;
		}

		// Token: 0x0400610D RID: 24845
		private static Dictionary<string, string> truncatedLabelsCached = new Dictionary<string, string>();

		// Token: 0x0400610E RID: 24846
		public const float TabWidth = 72f;

		// Token: 0x0400610F RID: 24847
		public const float TabHeight = 30f;

		// Token: 0x04006110 RID: 24848
		private static readonly Texture2D InspectTabButtonFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.07450981f, 0.08627451f, 0.105882354f, 1f));

		// Token: 0x04006111 RID: 24849
		public const float CornerButtonsSize = 24f;

		// Token: 0x04006112 RID: 24850
		public const float PaneInnerMargin = 12f;

		// Token: 0x04006113 RID: 24851
		public const float PaneHeight = 165f;

		// Token: 0x04006114 RID: 24852
		private const int TabMinimum = 6;

		// Token: 0x04006115 RID: 24853
		private static List<Thing> selectedThings = new List<Thing>();
	}
}
