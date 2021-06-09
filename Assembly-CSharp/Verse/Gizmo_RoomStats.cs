using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C7 RID: 1223
	public class Gizmo_RoomStats : Gizmo
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001E5C RID: 7772 RVA: 0x0001AEC7 File Offset: 0x000190C7
		private Room Room
		{
			get
			{
				return Gizmo_RoomStats.GetRoomToShowStatsFor(this.building);
			}
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0001AED4 File Offset: 0x000190D4
		public Gizmo_RoomStats(Building building)
		{
			this.building = building;
			this.order = -100f;
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0001AEEE File Offset: 0x000190EE
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(300f, maxWidth);
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000FC4FC File Offset: 0x000FA6FC
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Room room = this.Room;
			if (room == null)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Widgets.DrawWindowBackground(rect);
			Text.WordWrap = false;
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero().ContractedBy(10f);
			Text.Font = GameFont.Small;
			Rect rect3 = new Rect(rect2.x, rect2.y - 2f, rect2.width, 100f);
			float stat = room.GetStat(RoomStatDefOf.Impressiveness);
			RoomStatScoreStage scoreStage = RoomStatDefOf.Impressiveness.GetScoreStage(stat);
			TaggedString str = room.Role.LabelCap + ", " + scoreStage.label + " (" + RoomStatDefOf.Impressiveness.ScoreToString(stat) + ")";
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			float num = rect2.y + Text.LineHeight + Text.SpaceBetweenLines + 7f;
			GUI.color = Gizmo_RoomStats.RoomStatsColor;
			Text.Font = GameFont.Tiny;
			List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
			int num2 = 0;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (!allDefsListForReading[i].isHidden && allDefsListForReading[i] != RoomStatDefOf.Impressiveness)
				{
					float stat2 = room.GetStat(allDefsListForReading[i]);
					RoomStatScoreStage scoreStage2 = allDefsListForReading[i].GetScoreStage(stat2);
					Rect rect4;
					if (num2 % 2 == 0)
					{
						rect4 = new Rect(rect2.x, num, rect2.width / 2f, 100f);
					}
					else
					{
						rect4 = new Rect(rect2.x + rect2.width / 2f, num, rect2.width / 2f, 100f);
					}
					string str2 = scoreStage2.label.CapitalizeFirst() + " (" + allDefsListForReading[i].ScoreToString(stat2) + ")";
					Widgets.Label(rect4, str2.Truncate(rect4.width, null));
					if (num2 % 2 == 1)
					{
						num += Text.LineHeight + Text.SpaceBetweenLines;
					}
					num2++;
				}
			}
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			GUI.EndGroup();
			Text.WordWrap = true;
			GenUI.AbsorbClicksInRect(rect);
			if (Mouse.IsOver(rect))
			{
				Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(false, true);
				Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.Super, delegate
				{
					float num3 = 12f;
					EnvironmentStatsDrawer.DoRoomInfo(room, ref num3, windowRect);
				}, true, false, 1f);
				return new GizmoResult(GizmoState.Mouseover);
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000FC7F0 File Offset: 0x000FA9F0
		public override void GizmoUpdateOnMouseover()
		{
			base.GizmoUpdateOnMouseover();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InspectRoomStats, KnowledgeAmount.FrameInteraction);
			Room room = this.Room;
			if (room != null)
			{
				room.DrawFieldEdges();
			}
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000FC820 File Offset: 0x000FAA20
		public static Room GetRoomToShowStatsFor(Building building)
		{
			if (!building.Spawned || building.Fogged())
			{
				return null;
			}
			Room room = null;
			if (building.def.passability != Traversability.Impassable)
			{
				room = building.GetRoom(RegionType.Set_Passable);
			}
			else if (building.def.hasInteractionCell)
			{
				room = building.InteractionCell.GetRoom(building.Map, RegionType.Set_Passable);
			}
			else
			{
				CellRect cellRect = building.OccupiedRect().ExpandedBy(1);
				foreach (IntVec3 intVec in cellRect)
				{
					if (cellRect.IsOnEdge(intVec))
					{
						room = intVec.GetRoom(building.Map, RegionType.Set_Passable);
						if (Gizmo_RoomStats.<GetRoomToShowStatsFor>g__IsValid|8_0(room))
						{
							break;
						}
					}
				}
			}
			if (!Gizmo_RoomStats.<GetRoomToShowStatsFor>g__IsValid|8_0(room))
			{
				return null;
			}
			return room;
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0001AF16 File Offset: 0x00019116
		[CompilerGenerated]
		internal static bool <GetRoomToShowStatsFor>g__IsValid|8_0(Room r)
		{
			return r != null && !r.Fogged && r.Role != RoomRoleDefOf.None;
		}

		// Token: 0x04001594 RID: 5524
		private Building building;

		// Token: 0x04001595 RID: 5525
		private static readonly Color RoomStatsColor = new Color(0.75f, 0.75f, 0.75f);
	}
}
