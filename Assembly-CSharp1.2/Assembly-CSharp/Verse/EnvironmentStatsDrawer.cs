using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000707 RID: 1799
	public static class EnvironmentStatsDrawer
	{
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002D94 RID: 11668 RVA: 0x001342A0 File Offset: 0x001324A0
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

		// Token: 0x06002D95 RID: 11669 RVA: 0x00023F2D File Offset: 0x0002212D
		private static bool ShouldShowWindowNow()
		{
			return (EnvironmentStatsDrawer.ShouldShowRoomStats() || EnvironmentStatsDrawer.ShouldShowBeauty()) && !Mouse.IsInputBlockedNow;
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x001342E0 File Offset: 0x001324E0
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
			Room room = UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_All);
			return room != null && room.Role != RoomRoleDefOf.None;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x00134348 File Offset: 0x00132548
		private static bool ShouldShowBeauty()
		{
			return Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_Passable) != null;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x00023F49 File Offset: 0x00022149
		public static void EnvironmentStatsOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !EnvironmentStatsDrawer.ShouldShowWindowNow())
			{
				return;
			}
			EnvironmentStatsDrawer.DrawInfoWindow();
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x0013439C File Offset: 0x0013259C
		private static void DrawInfoWindow()
		{
			Text.Font = GameFont.Small;
			Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(EnvironmentStatsDrawer.ShouldShowBeauty(), EnvironmentStatsDrawer.ShouldShowRoomStats());
			Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.Super, delegate
			{
				EnvironmentStatsDrawer.FillWindow(windowRect);
			}, true, false, 1f);
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x001343F4 File Offset: 0x001325F4
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

		// Token: 0x06002D9B RID: 11675 RVA: 0x0013451C File Offset: 0x0013271C
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
				EnvironmentStatsDrawer.DoRoomInfo(UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_All), ref CS$<>8__locals1.curY, CS$<>8__locals1.windowRect);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x0013462C File Offset: 0x0013282C
		public static void DrawRoomOverlays()
		{
			if (Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap))
			{
				GenUI.RenderMouseoverBracket();
			}
			if (EnvironmentStatsDrawer.ShouldShowWindowNow() && EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				Room room = UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_All);
				if (room != null && room.Role != RoomRoleDefOf.None)
				{
					room.DrawFieldEdges();
				}
			}
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x00134690 File Offset: 0x00132890
		public static void DoRoomInfo(Room room, ref float curY, Rect windowRect)
		{
			Rect rect = new Rect(12f, curY, windowRect.width - 24f, 100f);
			GUI.color = Color.white;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x + 10f, curY, rect.width - 10f, rect.height), EnvironmentStatsDrawer.GetRoomRoleLabel(room));
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

		// Token: 0x06002D9E RID: 11678 RVA: 0x001348EC File Offset: 0x00132AEC
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
				taggedString = room.Role.LabelCap;
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

		// Token: 0x06002DA0 RID: 11680 RVA: 0x001349D8 File Offset: 0x00132BD8
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

		// Token: 0x04001F06 RID: 7942
		private const float StatLabelColumnWidth = 100f;

		// Token: 0x04001F07 RID: 7943
		private const float StatGutterColumnWidth = 10f;

		// Token: 0x04001F08 RID: 7944
		private const float ScoreColumnWidth = 50f;

		// Token: 0x04001F09 RID: 7945
		private const float ScoreStageLabelColumnWidth = 160f;

		// Token: 0x04001F0A RID: 7946
		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		// Token: 0x04001F0B RID: 7947
		private static readonly Color UnrelatedStatColor = Color.gray;

		// Token: 0x04001F0C RID: 7948
		private const float DistFromMouse = 26f;

		// Token: 0x04001F0D RID: 7949
		public const float WindowPadding = 12f;

		// Token: 0x04001F0E RID: 7950
		private const float LineHeight = 23f;

		// Token: 0x04001F0F RID: 7951
		private const float FootnoteHeight = 23f;

		// Token: 0x04001F10 RID: 7952
		private const float TitleHeight = 30f;

		// Token: 0x04001F11 RID: 7953
		private const float SpaceBetweenLines = 2f;

		// Token: 0x04001F12 RID: 7954
		private const float SpaceBetweenColumns = 35f;
	}
}
