using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A35 RID: 6709
	public static class FactionUIUtility
	{
		// Token: 0x06009405 RID: 37893 RVA: 0x002AC2D8 File Offset: 0x002AA4D8
		public static void DoWindowContents_NewTemp(Rect fillRect, ref Vector2 scrollPosition, ref float scrollViewHeight, Faction scrollToFaction = null)
		{
			Rect position = new Rect(0f, 0f, fillRect.width, fillRect.height);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(position.width - 120f, 0f, 120f, 24f), "Dev: Show all", ref FactionUIUtility.showAll, false, null, null, false);
			}
			else
			{
				FactionUIUtility.showAll = false;
			}
			Rect outRect = new Rect(0f, 50f, position.width, position.height - 50f);
			Rect rect = new Rect(0f, 0f, position.width - 16f, scrollViewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, rect, true);
			float num = 0f;
			foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
			{
				if ((!faction.IsPlayer && !faction.Hidden) || FactionUIUtility.showAll)
				{
					GUI.color = new Color(1f, 1f, 1f, 0.2f);
					Widgets.DrawLineHorizontal(0f, num, rect.width);
					GUI.color = Color.white;
					if (faction == scrollToFaction)
					{
						scrollPosition.y = num;
					}
					num += FactionUIUtility.DrawFactionRow(faction, num, rect);
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				scrollViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x06009406 RID: 37894 RVA: 0x000630F5 File Offset: 0x000612F5
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static void DoWindowContents(Rect fillRect, ref Vector2 scrollPosition, ref float scrollViewHeight)
		{
			FactionUIUtility.DoWindowContents_NewTemp(fillRect, ref scrollPosition, ref scrollViewHeight, null);
		}

		// Token: 0x06009407 RID: 37895 RVA: 0x002AC474 File Offset: 0x002AA674
		private static float DrawFactionRow(Faction faction, float rowY, Rect fillRect)
		{
			float num = fillRect.width - 250f - 40f - 90f - 16f - 120f;
			Faction[] array = (from f in Find.FactionManager.AllFactionsInViewOrder
			where f != faction && f.HostileTo(faction) && ((!f.IsPlayer && !f.Hidden) || FactionUIUtility.showAll)
			select f).ToArray<Faction>();
			Rect rect = new Rect(90f, rowY, 250f, 80f);
			Rect r = new Rect(24f, rowY + 4f, 42f, 42f);
			float num2 = 62f;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			FactionUIUtility.DrawFactionIconWithTooltip(r, faction);
			string label = faction.Name.CapitalizeFirst() + "\n" + faction.def.LabelCap + "\n" + ((faction.leader != null) ? (faction.LeaderTitle.CapitalizeFirst() + ": " + faction.leader.Name.ToStringFull) : "");
			Widgets.Label(rect, label);
			Rect rect2 = new Rect(rect.xMax, rowY, 40f, 80f);
			Widgets.InfoCardButton(rect2.x, rect2.y, faction);
			Rect rect3 = new Rect(rect2.xMax, rowY, 90f, 80f);
			if (!faction.IsPlayer)
			{
				string text = faction.HasGoodwill ? (faction.PlayerGoodwill.ToStringWithSign() + "\n") : "";
				text += faction.PlayerRelationKind.GetLabel();
				if (faction.defeated)
				{
					text += "\n(" + "DefeatedLower".Translate() + ")";
				}
				GUI.color = faction.PlayerRelationKind.GetColor();
				Widgets.Label(rect3, text);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect3))
				{
					TaggedString taggedString = faction.HasGoodwill ? "CurrentGoodwillTip".Translate() : "CurrentRelationTip".Translate();
					if (faction.HasGoodwill && faction.def.permanentEnemy)
					{
						taggedString += "\n\n" + "CurrentGoodwillTip_PermanentEnemy".Translate();
					}
					else if (faction.HasGoodwill)
					{
						taggedString += "\n\n";
						switch (faction.PlayerRelationKind)
						{
						case FactionRelationKind.Hostile:
							taggedString += "CurrentGoodwillTip_Hostile".Translate(0.ToString("F0"));
							break;
						case FactionRelationKind.Neutral:
							taggedString += "CurrentGoodwillTip_Neutral".Translate(-75.ToString("F0"), 75.ToString("F0"));
							break;
						case FactionRelationKind.Ally:
							taggedString += "CurrentGoodwillTip_Ally".Translate(0.ToString("F0"));
							break;
						}
						if (faction.def.goodwillDailyGain > 0f || faction.def.goodwillDailyFall > 0f)
						{
							float num3 = faction.def.goodwillDailyGain * 60f;
							float num4 = faction.def.goodwillDailyFall * 60f;
							taggedString += "\n\n" + "CurrentGoodwillTip_NaturalGoodwill".Translate(faction.def.naturalColonyGoodwill.min.ToString("F0"), faction.def.naturalColonyGoodwill.max.ToString("F0"));
							if (faction.def.naturalColonyGoodwill.min > -100)
							{
								taggedString += " " + "CurrentGoodwillTip_NaturalGoodwillRise".Translate(faction.def.naturalColonyGoodwill.min.ToString("F0"), num3.ToString("F0"));
							}
							if (faction.def.naturalColonyGoodwill.max < 100)
							{
								taggedString += " " + "CurrentGoodwillTip_NaturalGoodwillFall".Translate(faction.def.naturalColonyGoodwill.max.ToString("F0"), num4.ToString("F0"));
							}
						}
					}
					TooltipHandler.TipRegion(rect3, taggedString);
				}
				if (Mouse.IsOver(rect3))
				{
					GUI.DrawTexture(rect3, TexUI.HighlightTex);
				}
			}
			float num5 = rect3.xMax;
			string text2 = "EnemyOf".Translate();
			Vector2 vector = Text.CalcSize(text2);
			Rect rect4 = new Rect(num5, rowY + 4f, vector.x + 10f, 42f);
			num5 += rect4.width;
			Widgets.Label(rect4, text2);
			for (int i = 0; i < array.Length; i++)
			{
				if (num5 >= rect3.xMax + num)
				{
					num5 = rect3.xMax + rect4.width;
					rowY += vector.y + 5f;
					num2 += vector.y + 5f;
				}
				FactionUIUtility.DrawFactionIconWithTooltip(new Rect(num5, rowY + 4f, vector.y, vector.y), array[i]);
				num5 += vector.y + 5f;
			}
			return Mathf.Max(80f, num2);
		}

		// Token: 0x06009408 RID: 37896 RVA: 0x002ACAB4 File Offset: 0x002AACB4
		public static void DrawFactionIconWithTooltip(Rect r, Faction faction)
		{
			GUI.color = faction.Color;
			GUI.DrawTexture(r, faction.def.FactionIcon);
			GUI.color = Color.white;
			if (Mouse.IsOver(r))
			{
				TipSignal tip = new TipSignal(() => faction.Name + "\n\n" + faction.def.description, faction.loadID ^ 1938473043);
				TooltipHandler.TipRegion(r, tip);
				Widgets.DrawHighlight(r);
			}
		}

		// Token: 0x06009409 RID: 37897 RVA: 0x002ACB38 File Offset: 0x002AAD38
		public static void DrawRelatedFactionInfo(Rect rect, Faction faction, ref float curY)
		{
			Text.Anchor = TextAnchor.LowerRight;
			curY += 10f;
			FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
			string text = faction.Name.CapitalizeFirst() + "\n" + "goodwill".Translate().CapitalizeFirst() + ": " + faction.PlayerGoodwill.ToStringWithSign();
			GUI.color = Color.gray;
			Rect rect2 = new Rect(rect.x, curY, rect.width, Text.CalcHeight(text, rect.width));
			Widgets.Label(rect2, text);
			curY += rect2.height;
			GUI.color = playerRelationKind.GetColor();
			Rect rect3 = new Rect(rect2.x, curY - 7f, rect2.width, 25f);
			Widgets.Label(rect3, playerRelationKind.GetLabel());
			curY += rect3.height;
			GUI.color = Color.white;
			GenUI.ResetLabelAlign();
		}

		// Token: 0x04005DEE RID: 24046
		private static bool showAll;

		// Token: 0x04005DEF RID: 24047
		private const float FactionIconRectSize = 42f;

		// Token: 0x04005DF0 RID: 24048
		private const float FactionIconRectGapX = 24f;

		// Token: 0x04005DF1 RID: 24049
		private const float FactionIconRectGapY = 4f;

		// Token: 0x04005DF2 RID: 24050
		private const float RowMinHeight = 80f;

		// Token: 0x04005DF3 RID: 24051
		private const float LabelRowHeight = 50f;

		// Token: 0x04005DF4 RID: 24052
		private const float NameLeftMargin = 15f;

		// Token: 0x04005DF5 RID: 24053
		private const float FactionIconSpacing = 5f;
	}
}
