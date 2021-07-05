using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C8 RID: 1224
	public static class KeyBindingDefGenerator
	{
		// Token: 0x06002544 RID: 9540 RVA: 0x000E8871 File Offset: 0x000E6A71
		public static IEnumerable<KeyBindingCategoryDef> ImpliedKeyBindingCategoryDefs()
		{
			List<KeyBindingCategoryDef> gameUniversalCats = (from d in DefDatabase<KeyBindingCategoryDef>.AllDefs
			where d.isGameUniversal
			select d).ToList<KeyBindingCategoryDef>();
			foreach (DesignationCategoryDef designationCategoryDef in DefDatabase<DesignationCategoryDef>.AllDefs)
			{
				KeyBindingCategoryDef keyBindingCategoryDef = new KeyBindingCategoryDef();
				keyBindingCategoryDef.defName = "Architect_" + designationCategoryDef.defName;
				keyBindingCategoryDef.label = designationCategoryDef.label + " tab";
				keyBindingCategoryDef.description = "Key bindings for the \"" + designationCategoryDef.LabelCap + "\" section of the Architect menu";
				keyBindingCategoryDef.modContentPack = designationCategoryDef.modContentPack;
				keyBindingCategoryDef.checkForConflicts.AddRange(gameUniversalCats);
				for (int i = 0; i < gameUniversalCats.Count; i++)
				{
					gameUniversalCats[i].checkForConflicts.Add(keyBindingCategoryDef);
				}
				designationCategoryDef.bindingCatDef = keyBindingCategoryDef;
				yield return keyBindingCategoryDef;
			}
			IEnumerator<DesignationCategoryDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x000E887A File Offset: 0x000E6A7A
		public static IEnumerable<KeyBindingDef> ImpliedKeyBindingDefs()
		{
			foreach (MainButtonDef mainButtonDef in from td in DefDatabase<MainButtonDef>.AllDefs
			orderby td.order
			select td)
			{
				if (mainButtonDef.defaultHotKey != KeyCode.None)
				{
					KeyBindingDef keyBindingDef = new KeyBindingDef();
					keyBindingDef.label = "Toggle " + mainButtonDef.label + " tab";
					keyBindingDef.defName = "MainTab_" + mainButtonDef.defName;
					keyBindingDef.category = KeyBindingCategoryDefOf.MainTabs;
					keyBindingDef.defaultKeyCodeA = mainButtonDef.defaultHotKey;
					keyBindingDef.modContentPack = mainButtonDef.modContentPack;
					mainButtonDef.hotKey = keyBindingDef;
					yield return keyBindingDef;
				}
			}
			IEnumerator<MainButtonDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}
