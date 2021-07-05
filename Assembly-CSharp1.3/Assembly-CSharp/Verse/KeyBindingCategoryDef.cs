using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000D7 RID: 215
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x0001EA35 File Offset: 0x0001CC35
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x040004B7 RID: 1207
		public bool isGameUniversal;

		// Token: 0x040004B8 RID: 1208
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x040004B9 RID: 1209
		public bool selfConflicting = true;
	}
}
