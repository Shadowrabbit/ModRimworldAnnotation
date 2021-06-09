using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200086A RID: 2154
	public static class KeyBindingDefGenerator
	{
		// Token: 0x060035B0 RID: 13744 RVA: 0x00029A9D File Offset: 0x00027C9D
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

		// Token: 0x060035B1 RID: 13745 RVA: 0x00029AA6 File Offset: 0x00027CA6
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
