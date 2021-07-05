using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200017F RID: 383
	public static class AreaUtility
	{
		// Token: 0x06000AF5 RID: 2805 RVA: 0x0003B64C File Offset: 0x0003984C
		public static void MakeAllowedAreaListFloatMenu(Action<Area> selAction, bool addNullAreaOption, bool addManageOption, Map map)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (addNullAreaOption)
			{
				list.Add(new FloatMenuOption("NoAreaAllowed".Translate(), delegate()
				{
					selAction(null);
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0));
			}
			foreach (Area localArea2 in from a in map.areaManager.AllAreas
			where a.AssignableAsAllowed()
			select a)
			{
				Area localArea = localArea2;
				FloatMenuOption item = new FloatMenuOption(localArea.Label, delegate()
				{
					selAction(localArea);
				}, MenuOptionPriority.Default, delegate(Rect _)
				{
					localArea.MarkForDraw();
				}, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			if (addManageOption)
			{
				list.Add(new FloatMenuOption("ManageAreas".Translate(), delegate()
				{
					Find.WindowStack.Add(new Dialog_ManageAreas(map));
				}, MenuOptionPriority.Low, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0003B7AC File Offset: 0x000399AC
		public static string AreaAllowedLabel(Pawn pawn)
		{
			if (pawn.playerSettings != null)
			{
				return AreaUtility.AreaAllowedLabel_Area(pawn.playerSettings.EffectiveAreaRestriction);
			}
			return AreaUtility.AreaAllowedLabel_Area(null);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0003B7CD File Offset: 0x000399CD
		public static string AreaAllowedLabel_Area(Area area)
		{
			if (area != null)
			{
				return area.Label;
			}
			return "NoAreaAllowed".Translate();
		}
	}
}
