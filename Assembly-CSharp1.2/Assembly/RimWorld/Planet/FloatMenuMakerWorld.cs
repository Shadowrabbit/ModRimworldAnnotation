using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021C5 RID: 8645
	public static class FloatMenuMakerWorld
	{
		// Token: 0x0600B918 RID: 47384 RVA: 0x00353630 File Offset: 0x00351830
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

		// Token: 0x0600B919 RID: 47385 RVA: 0x0035367C File Offset: 0x0035187C
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

		// Token: 0x0600B91A RID: 47386 RVA: 0x003536BC File Offset: 0x003518BC
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
