using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BEC RID: 7148
	public static class TutorUtility
	{
		// Token: 0x06009D4B RID: 40267 RVA: 0x002DFFD4 File Offset: 0x002DE1D4
		public static bool BuildingOrBlueprintOrFrameCenterExists(IntVec3 c, Map map, ThingDef buildingDef)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (!(thing.Position != c))
				{
					if (thing.def == buildingDef)
					{
						return true;
					}
					if (thing.def.entityDefToBuild == buildingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06009D4C RID: 40268 RVA: 0x002E002C File Offset: 0x002DE22C
		public static CellRect FindUsableRect(int width, int height, Map map, float minFertility = 0f, bool noItems = false)
		{
			IntVec3 center = map.Center;
			float num = 1f;
			CellRect cellRect;
			for (;;)
			{
				cellRect = CellRect.CenteredOn(center + new IntVec3((int)Rand.Range(-num, num), 0, (int)Rand.Range(-num, num)), width / 2);
				cellRect.Width = width;
				cellRect.Height = height;
				cellRect = cellRect.ExpandedBy(1);
				bool flag = true;
				foreach (IntVec3 intVec in cellRect)
				{
					if (intVec.Fogged(map) || !intVec.Walkable(map) || !intVec.GetTerrain(map).affordances.Contains(TerrainAffordanceDefOf.Heavy) || intVec.GetTerrain(map).fertility < minFertility || intVec.GetZone(map) != null || TutorUtility.ContainsBlockingThing(intVec, map, noItems) || intVec.InNoBuildEdgeArea(map) || intVec.InNoZoneEdgeArea(map))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				num += 0.25f;
			}
			return cellRect.ContractedBy(1);
		}

		// Token: 0x06009D4D RID: 40269 RVA: 0x002E0148 File Offset: 0x002DE348
		private static bool ContainsBlockingThing(IntVec3 cell, Map map, bool noItems)
		{
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Building)
				{
					return true;
				}
				if (thingList[i] is Blueprint)
				{
					return true;
				}
				if (noItems && thingList[i].def.category == ThingCategory.Item)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009D4E RID: 40270 RVA: 0x002E01B0 File Offset: 0x002DE3B0
		public static void DrawLabelOnThingOnGUI(Thing t, string label)
		{
			Vector2 vector = (t.DrawPos + new Vector3(0f, 0f, 0.5f)).MapToUIPosition();
			Vector2 vector2 = Text.CalcSize(label);
			Rect rect = new Rect(vector.x - vector2.x / 2f, vector.y - vector2.y / 2f, vector2.x, vector2.y);
			GUI.DrawTexture(rect, TexUI.GrayTextBG);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06009D4F RID: 40271 RVA: 0x002E0244 File Offset: 0x002DE444
		public static void DrawLabelOnGUI(Vector3 mapPos, string label)
		{
			Vector2 vector = mapPos.MapToUIPosition();
			Vector2 vector2 = Text.CalcSize(label);
			Rect rect = new Rect(vector.x - vector2.x / 2f, vector.y - vector2.y / 2f, vector2.x, vector2.y);
			GUI.DrawTexture(rect, TexUI.GrayTextBG);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06009D50 RID: 40272 RVA: 0x00068BA8 File Offset: 0x00066DA8
		public static void DrawCellRectOnGUI(CellRect cellRect, string label = null)
		{
			if (label != null)
			{
				TutorUtility.DrawLabelOnGUI(cellRect.CenterVector3, label);
			}
		}

		// Token: 0x06009D51 RID: 40273 RVA: 0x002E02BC File Offset: 0x002DE4BC
		public static void DrawCellRectUpdate(CellRect cellRect)
		{
			foreach (IntVec3 c in cellRect)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x06009D52 RID: 40274 RVA: 0x002E0310 File Offset: 0x002DE510
		public static void DoModalDialogIfNotKnown(ConceptDef conc, params string[] input)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				TutorUtility.DoModalDialogIfNotKnownInner(conc, string.Format(conc.HelpTextAdjusted, input));
			}
		}

		// Token: 0x06009D53 RID: 40275 RVA: 0x00068BBA File Offset: 0x00066DBA
		public static void DoModalDialogWithArgsIfNotKnown(ConceptDef conc, params NamedArgument[] args)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				TutorUtility.DoModalDialogIfNotKnownInner(conc, conc.HelpTextAdjusted.Formatted(args));
			}
		}

		// Token: 0x06009D54 RID: 40276 RVA: 0x002E033C File Offset: 0x002DE53C
		public static void DoModalDialogWithArgsIfNotKnown(ConceptDef conc, string buttonAText, Action buttonAAction, string buttonBText = null, Action buttonBAction = null, params NamedArgument[] args)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				Find.WindowStack.Add(new Dialog_MessageBox(conc.HelpTextAdjusted.Formatted(args), buttonAText, buttonAAction, buttonBText, buttonBAction, null, false, null, null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(conc, KnowledgeAmount.Total);
			}
		}

		// Token: 0x06009D55 RID: 40277 RVA: 0x002E0380 File Offset: 0x002DE580
		private static void DoModalDialogIfNotKnownInner(ConceptDef conc, string msg)
		{
			Find.WindowStack.Add(new Dialog_MessageBox(msg, null, null, null, null, null, false, null, null));
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(conc, KnowledgeAmount.Total);
		}

		// Token: 0x06009D56 RID: 40278 RVA: 0x002E03B4 File Offset: 0x002DE5B4
		public static bool EventCellsMatchExactly(EventPack ep, List<IntVec3> targetCells)
		{
			if (ep.Cell.IsValid)
			{
				return targetCells.Count == 1 && ep.Cell == targetCells[0];
			}
			if (ep.Cells == null)
			{
				return false;
			}
			int num = 0;
			foreach (IntVec3 item in ep.Cells)
			{
				if (!targetCells.Contains(item))
				{
					return false;
				}
				num++;
			}
			return num == targetCells.Count;
		}

		// Token: 0x06009D57 RID: 40279 RVA: 0x002E0458 File Offset: 0x002DE658
		public static bool EventCellsAreWithin(EventPack ep, List<IntVec3> targetCells)
		{
			if (ep.Cell.IsValid)
			{
				return targetCells.Contains(ep.Cell);
			}
			return ep.Cells != null && !ep.Cells.Any((IntVec3 c) => !targetCells.Contains(c));
		}
	}
}
