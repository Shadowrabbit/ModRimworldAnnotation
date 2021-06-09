using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Steamworks;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000081 RID: 129
	public class ModSummaryWindow
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0000A369 File Offset: 0x00008569
		private static bool AnyMods
		{
			get
			{
				return ModLister.AllInstalledMods.Any((ModMetaData m) => !m.Official && m.Active);
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00088C4C File Offset: 0x00086E4C
		public static void DrawWindow(Vector2 offset, bool useWindowStack)
		{
			Rect rect = new Rect(offset.x, offset.y, ModSummaryWindow.WindowSize.x, ModSummaryWindow.GetEffectiveSize().y);
			if (useWindowStack)
			{
				Find.WindowStack.ImmediateWindow(62893996, rect, WindowLayer.Super, delegate
				{
					ModSummaryWindow.DrawContents(rect.AtZero());
				}, true, false, 1f);
				return;
			}
			Widgets.DrawShadowAround(rect);
			Widgets.DrawWindowBackground(rect);
			ModSummaryWindow.DrawContents(rect);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00088CD8 File Offset: 0x00086ED8
		private static void DrawContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			float num = 0f;
			float num2 = 17f;
			float itemListInnerMargin = 8f;
			float num3 = num2 + 4f;
			Rect rect2 = new Rect(rect.x + num2, rect.y, rect.width - num2 * 2f, 0f);
			Rect rect3 = rect;
			rect3.x += num3;
			rect3.y += 10f;
			Widgets.Label(rect3, "OfficialContent".Translate());
			num += 10f + Text.LineHeight + 4f;
			Rect rect4 = rect2;
			rect4.y += num;
			rect4.height = 94f;
			Widgets.DrawBoxSolid(rect4, ModSummaryWindow.ModInfoListBackground);
			num += 104f;
			List<GenUI.AnonymousStackElement> list = new List<GenUI.AnonymousStackElement>();
			Text.Anchor = TextAnchor.MiddleLeft;
			for (int i = 0; i < ModLister.AllExpansions.Count; i++)
			{
				ExpansionDef exp = ModLister.AllExpansions[i];
				list.Add(new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect r)
					{
						bool flag = exp.Status == ExpansionStatus.Active;
						Widgets.DrawBoxSolid(r, flag ? ModSummaryWindow.ModInfoListItemBackground : ModSummaryWindow.ModInfoListItemBackgroundDisabled);
						Widgets.DrawHighlightIfMouseover(r);
						if (!exp.isCore && !exp.StoreURL.NullOrEmpty() && Widgets.ButtonInvisible(r, true))
						{
							SteamUtility.OpenUrl(exp.StoreURL);
						}
						GUI.color = (flag ? Color.white : ModSummaryWindow.DisabledIconTint);
						Material material = flag ? null : TexUI.GrayscaleGUI;
						Rect rect8 = new Rect(r.x + itemListInnerMargin, r.y + 2f, 32f, 32f);
						float num4 = 42f;
						GenUI.DrawTextureWithMaterial(rect8, exp.Icon, material, default(Rect));
						GUI.color = (flag ? Color.white : Color.grey);
						Rect rect9 = new Rect(r.x + itemListInnerMargin + num4, r.y, r.width - num4, r.height);
						if (exp.Status != ExpansionStatus.Active)
						{
							TaggedString t = ((exp.Status == ExpansionStatus.Installed) ? "DisabledLower" : "ContentNotInstalled").Translate().ToLower();
							Widgets.Label(rect9, exp.label + " (" + t + ")");
						}
						else
						{
							Widgets.Label(rect9, exp.label);
						}
						GUI.color = Color.white;
						if (Mouse.IsOver(r))
						{
							string description = string.Concat(new string[]
							{
								exp.label,
								"\n",
								exp.StatusDescription,
								"\n\n",
								exp.description.StripTags()
							});
							TipSignal tip = new TipSignal(() => description, exp.GetHashCode() * 37);
							TooltipHandler.TipRegion(r, tip);
						}
					}
				});
			}
			GenUI.DrawElementStackVertical<GenUI.AnonymousStackElement>(new Rect(rect4.x + itemListInnerMargin, rect4.y + itemListInnerMargin, rect4.width - itemListInnerMargin * 2f, 94f), ModSummaryWindow.ListElementSize.y, list, delegate(Rect r, GenUI.AnonymousStackElement obj)
			{
				obj.drawer(r);
			}, (GenUI.AnonymousStackElement obj) => ModSummaryWindow.ListElementSize.x, 6f);
			list.Clear();
			Rect rect5 = rect;
			rect5.x += num3;
			rect5.y += num;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect5, "Mods".Translate());
			num += Text.LineHeight + 4f;
			Rect rect6 = rect2;
			rect6.y += num;
			rect6.height = (ModSummaryWindow.AnyMods ? 224f : 40f);
			Widgets.DrawBoxSolid(rect6, ModSummaryWindow.ModInfoListBackground);
			if (ModSummaryWindow.AnyMods)
			{
				Text.Anchor = TextAnchor.MiddleLeft;
				using (IEnumerator<ModMetaData> enumerator = (from m in ModLister.AllInstalledMods
				where !m.Official && m.Active
				select m).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ModMetaData mod = enumerator.Current;
						list.Add(new GenUI.AnonymousStackElement
						{
							drawer = delegate(Rect r)
							{
								Widgets.DrawBoxSolid(r, mod.VersionCompatible ? ModSummaryWindow.ModInfoListItemBackground : ModSummaryWindow.ModInfoListItemBackgroundIncompatible);
								Widgets.DrawHighlightIfMouseover(r);
								if (mod.OnSteamWorkshop && mod.GetPublishedFileId() != PublishedFileId_t.Invalid && Widgets.ButtonInvisible(r, true))
								{
									SteamUtility.OpenWorkshopPage(mod.GetPublishedFileId());
								}
								Rect rect8 = new Rect(r.x + itemListInnerMargin, r.y, r.width, r.height);
								string label = mod.Name.Truncate(rect8.width - itemListInnerMargin - 4f, null);
								Widgets.Label(rect8, label);
								if (Mouse.IsOver(r))
								{
									string description = mod.Name + "\n\n" + mod.Description.StripTags();
									if (!mod.VersionCompatible)
									{
										description = description + "\n\n" + "ModNotMadeForThisVersionShort".Translate().RawText.Colorize(Color.yellow);
									}
									TipSignal tip = new TipSignal(() => description, mod.GetHashCode() * 37);
									TooltipHandler.TipRegion(r, tip);
								}
								GUI.color = Color.white;
							}
						});
					}
				}
				Widgets.BeginScrollView(rect6, ref ModSummaryWindow.modListScrollPos, new Rect(0f, 0f, rect6.width - 16f, ModSummaryWindow.modListLastHeight + itemListInnerMargin * 2f), true);
				ModSummaryWindow.modListLastHeight = GenUI.DrawElementStack<GenUI.AnonymousStackElement>(new Rect(itemListInnerMargin, itemListInnerMargin, rect6.width - itemListInnerMargin * 2f, 99999f), ModSummaryWindow.ListElementSize.y, list, delegate(Rect r, GenUI.AnonymousStackElement obj)
				{
					obj.drawer(r);
				}, (GenUI.AnonymousStackElement obj) => ModSummaryWindow.ListElementSize.x, 6f, 5f, true).height;
				Widgets.EndScrollView();
			}
			else
			{
				Text.Anchor = TextAnchor.UpperLeft;
				Rect rect7 = rect6;
				rect7.x += itemListInnerMargin;
				rect7.y += itemListInnerMargin;
				GUI.color = Color.gray;
				Widgets.Label(rect7, "None".Translate());
				GUI.color = Color.white;
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000A394 File Offset: 0x00008594
		public static Vector2 GetEffectiveSize()
		{
			return new Vector2(ModSummaryWindow.WindowSize.x, ModSummaryWindow.AnyMods ? ModSummaryWindow.WindowSize.y : 226f);
		}

		// Token: 0x0400022E RID: 558
		private static Vector2 modListScrollPos;

		// Token: 0x0400022F RID: 559
		private static float modListLastHeight;

		// Token: 0x04000230 RID: 560
		private static readonly Vector2 WindowSize = new Vector2(776f, 410f);

		// Token: 0x04000231 RID: 561
		private static readonly Vector2 ListElementSize = new Vector2(238f, 36f);

		// Token: 0x04000232 RID: 562
		private const float WindowHeightCollapsed = 226f;

		// Token: 0x04000233 RID: 563
		private const float ExpansionListHeight = 94f;

		// Token: 0x04000234 RID: 564
		private const float ModListHeight = 224f;

		// Token: 0x04000235 RID: 565
		private const float ModListHeightCollapsed = 40f;

		// Token: 0x04000236 RID: 566
		private const float ListElementIconSize = 32f;

		// Token: 0x04000237 RID: 567
		private static readonly Color DisabledIconTint = new Color(0.35f, 0.35f, 0.35f);

		// Token: 0x04000238 RID: 568
		private static readonly Color ModInfoListBackground = new Color(0.13f, 0.13f, 0.13f);

		// Token: 0x04000239 RID: 569
		private static readonly Color ModInfoListItemBackground = new Color(0.32f, 0.32f, 0.32f);

		// Token: 0x0400023A RID: 570
		private static readonly Color ModInfoListItemBackgroundIncompatible = new Color(0.31f, 0.29f, 0.15f);

		// Token: 0x0400023B RID: 571
		private static readonly Color ModInfoListItemBackgroundDisabled = new Color(0.1f, 0.1f, 0.1f);
	}
}
