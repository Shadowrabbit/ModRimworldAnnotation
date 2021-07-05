using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001808 RID: 6152
	public static class FloatMenuMakerWorld
	{
		// Token: 0x06008FF3 RID: 36851 RVA: 0x00338BE4 File Offset: 0x00336DE4
		public static bool TryMakeFloatMenu(Caravan caravan)
		{
			if (!caravan.IsPlayerControlled)
			{
				return false;
			}
			Vector2 mousePositionOnUI = UI.MousePositionOnUI;
			List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(mousePositionOnUI, caravan);
			if (list.Count == 0)
			{
				return false;
			}
			FloatMenuWorld window = new FloatMenuWorld(list, caravan.LabelCap, mousePositionOnUI);
			Find.WindowStack.Add(window);
			return true;
		}

		// Token: 0x06008FF4 RID: 36852 RVA: 0x00338C30 File Offset: 0x00336E30
		public static List<FloatMenuOption> ChoicesAtFor(Vector2 mousePos, Caravan caravan)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<WorldObject> list2 = GenWorldUI.WorldObjectsUnderMouse(mousePos);
			for (int i = 0; i < list2.Count; i++)
			{
				list.AddRange(list2[i].GetFloatMenuOptions(caravan));
			}
			return list;
		}

		// Token: 0x06008FF5 RID: 36853 RVA: 0x00338C70 File Offset: 0x00336E70
		public static List<FloatMenuOption> ChoicesAtFor(int tile, Caravan caravan)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile)
				{
					list.AddRange(allWorldObjects[i].GetFloatMenuOptions(caravan));
				}
			}
			return list;
		}
	}
}
