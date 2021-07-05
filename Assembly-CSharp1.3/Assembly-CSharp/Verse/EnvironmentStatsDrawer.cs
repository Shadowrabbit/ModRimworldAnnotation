using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EE RID: 1006
	public static class EnvironmentStatsDrawer
	{
		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001E4E RID: 7758 RVA: 0x000BD818 File Offset: 0x000BBA18
		private static int DisplayedRoomStatsCount
		{
			get
			{
				int num = 0;
				List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!allDefsListForReading[i].isHidden || DebugViewSettings.showAllRoomStats)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000BD858 File Offset: 0x000BBA58
		private static bool ShouldShowWindowNow()
		{
			return (EnvironmentStatsDrawer.ShouldShowRoomStats() || EnvironmentStatsDrawer.ShouldShowBeauty()) && !Mouse.IsInputBlockedNow;
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000BD874 File Offset: 0x000BBA74
		private static bool ShouldShowRoomStats()
		{
			if (!Find.PlaySettings.showRoomStats)
			{
				return false;
			}
			if (!UI.MouseCell().InBounds(Find.CurrentMap) || UI.MouseCell().Fogged(Find.CurrentMap))
			{
				return false;
			}
			Room room = UI.MouseCell().GetRoom(Find.CurrentMap);
			return room != null && room.Role != RoomRoleDefOf.None;
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000BD8D8 File Offset: 0x000BBAD8
		private static bool ShouldShowBeauty()
		{
			return Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && UI.MouseCell().GetRoom(Find.CurrentMap) != null;
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x000BD929 File Offset: 0x000BBB29
		public static void EnvironmentStatsOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !EnvironmentStatsDrawer.ShouldShowWindowNow())
			{
				return;
			}
			EnvironmentStatsDrawer.DrawInfoWindow();
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x000BD948 File Offset: 0x000BBB48
		private static void DrawInfoWindow()
		{
			Text.Font = GameFont.Small;
			Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(EnvironmentStatsDrawer.ShouldShowBeauty(), EnvironmentStatsDrawer.ShouldShowRoomStats());
			Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.Super, delegate
			{
				EnvironmentStatsDrawer.FillWindow(windowRect);
			}, true, false, 1f, null);
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000BD9A0 File Offset: 0x000BBBA0
		public static Rect GetWindowRect(bool shouldShowBeauty, bool shouldShowRoomStats)
		{
			Rect result = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 414f, 24f);
			int num = 0;
			if (shouldShowBeauty)
			{
				num++;
				result.height += 25f;
			}
			if (shouldShowRoomStats)
			{
				num++;
				result.height += 23f;
				result.height += (float)EnvironmentStatsDrawer.DisplayedRoomStatsCount * 25f + 23f;
			}
			result.height += 13f * (float)(num - 1);
			result.x += 26f;
			result.y += 26f;
			if (result.xMax > (float)UI.screenWidth)
			{
				result.x -= result.width + 52f;
			}
			if (result.yMax > (float)UI.screenHeight)
			{
				result.y -= result.height + 52f;
			}
			return result;
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x000BDAC8 File Offset: 0x000BBCC8
		private static void FillWindow(Rect windowRect)
		{
			EnvironmentStatsDrawer.<>c__DisplayClass21_0 CS$<>8__locals1;
			CS$<>8__locals1.windowRect = windowRect;
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InspectRoomStats, KnowledgeAmount.FrameDisplayed);
			Text.Font = GameFont.Small;
			CS$<>8__locals1.curY = 12f;
			CS$<>8__locals1.dividingLinesSeen = 0;
			if (EnvironmentStatsDrawer.ShouldShowBeauty())
			{
				EnvironmentStatsDrawer.<FillWindow>g__DrawDividingLineIfNecessary|21_0(ref CS$<>8__locals1);
				float beauty = BeautyUtility.AverageBeautyPerceptible(UI.MouseCell(), Find.CurrentMap);
				Rect rect = new Rect(22f, CS$<>8__locals1.curY, CS$<>8__locals1.windowRect.width - 24f - 10f, 100f);
				GUI.color = BeautyDrawer.BeautyColor(beauty, 40f);
				Widgets.Label(rect, "BeautyHere".Translate() + ": " + beauty.ToString("F1"));
				CS$<>8__locals1.curY += 25f;
			}
			if (EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				EnvironmentStatsDrawer.<FillWindow>g__DrawDividingLineIfNecessary|21_0(ref CS$<>8__locals1);
				EnvironmentStatsDrawer.DoRoomInfo(UI.MouseCell().GetRoom(Find.CurrentMap), ref CS$<>8__locals1.curY, CS$<>8__locals1.windowRect);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000BDBD8 File Offset: 0x000BBDD8
		public static void DrawRoomOverlays()
		{
			if (Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap))
			{
				GenUI.RenderMouseoverBracket();
			}
			if (EnvironmentStatsDrawer.ShouldShowWindowNow() && EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				Room room = UI.MouseCell().GetRoom(Find.CurrentMap);
				if (room != null && room.Role != RoomRoleDefOf.None)
				{
					room.DrawFieldEdges();
				}
			}
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000BDC3C File Offset: 0x000BBE3C
		public static void DoRoomInfo(Room room, ref float curY, Rect windowRect)
		{
			Rect rect = new Rect(12f, curY, windowRect.width - 24f, 100f);
			GUI.color = Color.white;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x + 10f, curY, rect.width - 10f, rect.height), EnvironmentStatsDrawer.GetRoomRoleLabel(room).CapitalizeFirst());
			curY += 30f;
			Text.Font = GameFont.Small;
			Text.WordWrap = false;
			int num = 0;
			bool flag = false;
			for (int i = 0; i < DefDatabase<RoomStatDef>.AllDefsListForReading.Count; i++)
			{
				RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.AllDefsListForReading[i];
				if (!roomStatDef.isHidden || DebugViewSettings.showAllRoomStats)
				{
					float stat = room.GetStat(roomStatDef);
					RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
					GUI.color = Color.white;
					Rect rect2 = new Rect(rect.x, curY, rect.width, 23f);
					if (num % 2 == 1)
					{
						Widgets.DrawLightHighlight(rect2);
					}
					Rect rect3 = new Rect(rect.x, curY, 10f, 23f);
					if (room.Role.IsStatRelated(roomStatDef))
					{
						flag = true;
						Widgets.Label(rect3, "*");
						GUI.color = EnvironmentStatsDrawer.RelatedStatColor;
					}
					else
					{
						GUI.color = EnvironmentStatsDrawer.UnrelatedStatColor;
					}
					Rect rect4 = new Rect(rect3.xMax, curY, 100f, 23f);
					Widgets.Label(rect4, roomStatDef.LabelCap);
					Rect rect5 = new Rect(rect4.xMax + 35f, curY, 50f, 23f);
					string label = roomStatDef.ScoreToString(stat);
					Widgets.Label(rect5, label);
					Widgets.Label(new Rect(rect5.xMax + 35f, curY, 160f, 23f), (scoreStage == null) ? "" : scoreStage.label.CapitalizeFirst());
					curY += 25f;
					num++;
				}
			}
			if (flag)
			{
				GUI.color = Color.grey;
				Text.Font = GameFont.Tiny;
				Widgets.Label(new Rect(rect.x, curY, rect.width, 23f), "* " + "StatRelatesToCurrentRoom".Translate());
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
			}
			Text.WordWrap = true;
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000BDE9C File Offset: 0x000BC09C
		private static string GetRoomRoleLabel(Room room)
		{
			Pawn pawn = null;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in room.Owners)
			{
				if (pawn == null)
				{
					pawn = pawn3;
				}
				else
				{
					pawn2 = pawn3;
				}
			}
			TaggedString taggedString;
			if (pawn == null)
			{
				taggedString = room.Role.PostProcessedLabelCap;
			}
			else if (pawn2 == null)
			{
				taggedString = "SomeonesRoom".Translate(pawn.LabelShort, room.Role.label, pawn.Named("PAWN"));
			}
			else
			{
				taggedString = "CouplesRoom".Translate(pawn.LabelShort, pawn2.LabelShort, room.Role.label, pawn.Named("PAWN1"), pawn2.Named("PAWN2"));
			}
			return taggedString;
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000BDFB4 File Offset: 0x000BC1B4
		[CompilerGenerated]
		internal static void <FillWindow>g__DrawDividingLineIfNecessary|21_0(ref EnvironmentStatsDrawer.<>c__DisplayClass21_0 A_0)
		{
			int dividingLinesSeen = A_0.dividingLinesSeen;
			A_0.dividingLinesSeen = dividingLinesSeen + 1;
			if (A_0.dividingLinesSeen <= 1)
			{
				return;
			}
			A_0.curY += 5f;
			GUI.color = new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(12f, A_0.curY, A_0.windowRect.width - 24f);
			GUI.color = Color.white;
			A_0.curY += 8f;
		}

		// Token: 0x04001264 RID: 4708
		private const float StatLabelColumnWidth = 100f;

		// Token: 0x04001265 RID: 4709
		private const float StatGutterColumnWidth = 10f;

		// Token: 0x04001266 RID: 4710
		private const float ScoreColumnWidth = 50f;

		// Token: 0x04001267 RID: 4711
		private const float ScoreStageLabelColumnWidth = 160f;

		// Token: 0x04001268 RID: 4712
		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		// Token: 0x04001269 RID: 4713
		private static readonly Color UnrelatedStatColor = Color.gray;

		// Token: 0x0400126A RID: 4714
		private const float DistFromMouse = 26f;

		// Token: 0x0400126B RID: 4715
		public const float WindowPadding = 12f;

		// Token: 0x0400126C RID: 4716
		private const float LineHeight = 23f;

		// Token: 0x0400126D RID: 4717
		private const float FootnoteHeight = 23f;

		// Token: 0x0400126E RID: 4718
		private const float TitleHeight = 30f;

		// Token: 0x0400126F RID: 4719
		private const float SpaceBetweenLines = 2f;

		// Token: 0x04001270 RID: 4720
		private const float SpaceBetweenColumns = 35f;
	}
}
