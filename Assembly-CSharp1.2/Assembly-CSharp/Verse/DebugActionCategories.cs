using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006C1 RID: 1729
	public static class DebugActionCategories
	{
		// Token: 0x06002C9C RID: 11420 RVA: 0x0012FBA4 File Offset: 0x0012DDA4
		static DebugActionCategories()
		{
			DebugActionCategories.categoryOrders.Add("Incidents", 100);
			DebugActionCategories.categoryOrders.Add("Quests", 200);
			DebugActionCategories.categoryOrders.Add("Quests (old)", 250);
			DebugActionCategories.categoryOrders.Add("Translation", 300);
			DebugActionCategories.categoryOrders.Add("General", 400);
			DebugActionCategories.categoryOrders.Add("Pawns", 500);
			DebugActionCategories.categoryOrders.Add("Spawning", 600);
			DebugActionCategories.categoryOrders.Add("Map management", 700);
			DebugActionCategories.categoryOrders.Add("Autotests", 800);
			DebugActionCategories.categoryOrders.Add("Mods", 900);
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x0012FC80 File Offset: 0x0012DE80
		public static int GetOrderFor(string category)
		{
			int result;
			if (DebugActionCategories.categoryOrders.TryGetValue(category, out result))
			{
				return result;
			}
			return int.MaxValue;
		}

		// Token: 0x04001E31 RID: 7729
		public const string Incidents = "Incidents";

		// Token: 0x04001E32 RID: 7730
		public const string Quests = "Quests";

		// Token: 0x04001E33 RID: 7731
		public const string QuestsOld = "Quests (old)";

		// Token: 0x04001E34 RID: 7732
		public const string Translation = "Translation";

		// Token: 0x04001E35 RID: 7733
		public const string General = "General";

		// Token: 0x04001E36 RID: 7734
		public const string Pawns = "Pawns";

		// Token: 0x04001E37 RID: 7735
		public const string Spawning = "Spawning";

		// Token: 0x04001E38 RID: 7736
		public const string MapManagement = "Map management";

		// Token: 0x04001E39 RID: 7737
		public const string Autotests = "Autotests";

		// Token: 0x04001E3A RID: 7738
		public const string Mods = "Mods";

		// Token: 0x04001E3B RID: 7739
		public static readonly Dictionary<string, int> categoryOrders = new Dictionary<string, int>();
	}
}
