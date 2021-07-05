using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000148 RID: 328
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x06000886 RID: 2182 RVA: 0x0000CC03 File Offset: 0x0000AE03
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x040006B3 RID: 1715
		public bool isGameUniversal;

		// Token: 0x040006B4 RID: 1716
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x040006B5 RID: 1717
		public bool selfConflicting = true;
	}
}
