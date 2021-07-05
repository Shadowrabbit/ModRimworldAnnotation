using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013DC RID: 5084
	public static class TutorUtility
	{
		// Token: 0x06007BA1 RID: 31649 RVA: 0x002B92EC File Offset: 0x002B74EC
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

		// Token: 0x06007BA2 RID: 31650 RVA: 0x002B9344 File Offset: 0x002B7544
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

		// Token: 0x06007BA3 RID: 31651 RVA: 0x002B9460 File Offset: 0x002B7660
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

		// Token: 0x06007BA4 RID: 31652 RVA: 0x002B94C8 File Offset: 0x002B76C8
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

		// Token: 0x06007BA5 RID: 31653 RVA: 0x002B955C File Offset: 0x002B775C
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

		// Token: 0x06007BA6 RID: 31654 RVA: 0x002B95D1 File Offset: 0x002B77D1
		public static void DrawCellRectOnGUI(CellRect cellRect, string label = null)
		{
			if (label != null)
			{
				TutorUtility.DrawLabelOnGUI(cellRect.CenterVector3, label);
			}
		}

		// Token: 0x06007BA7 RID: 31655 RVA: 0x002B95E4 File Offset: 0x002B77E4
		public static void DrawCellRectUpdate(CellRect cellRect)
		{
			foreach (IntVec3 c in cellRect)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x06007BA8 RID: 31656 RVA: 0x002B9638 File Offset: 0x002B7838
		public static void DoModalDialogIfNotKnown(ConceptDef conc, params string[] input)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				TutorUtility.DoModalDialogIfNotKnownInner(conc, string.Format(conc.HelpTextAdjusted, input));
			}
		}

		// Token: 0x06007BA9 RID: 31657 RVA: 0x002B9661 File Offset: 0x002B7861
		public static void DoModalDialogWithArgsIfNotKnown(ConceptDef conc, params NamedArgument[] args)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				TutorUtility.DoModalDialogIfNotKnownInner(conc, conc.HelpTextAdjusted.Formatted(args));
			}
		}

		// Token: 0x06007BAA RID: 31658 RVA: 0x002B9684 File Offset: 0x002B7884
		public static void DoModalDialogWithArgsIfNotKnown(ConceptDef conc, string buttonAText, Action buttonAAction, string buttonBText = null, Action buttonBAction = null, params NamedArgument[] args)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				Find.WindowStack.Add(new Dialog_MessageBox(conc.HelpTextAdjusted.Formatted(args), buttonAText, buttonAAction, buttonBText, buttonBAction, null, false, null, null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(conc, KnowledgeAmount.Total);
			}
		}

		// Token: 0x06007BAB RID: 31659 RVA: 0x002B96C8 File Offset: 0x002B78C8
		private static void DoModalDialogIfNotKnownInner(ConceptDef conc, string msg)
		{
			Find.WindowStack.Add(new Dialog_MessageBox(msg, null, null, null, null, null, false, null, null));
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(conc, KnowledgeAmount.Total);
		}

		// Token: 0x06007BAC RID: 31660 RVA: 0x002B96FC File Offset: 0x002B78FC
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

		// Token: 0x06007BAD RID: 31661 RVA: 0x002B97A0 File Offset: 0x002B79A0
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
