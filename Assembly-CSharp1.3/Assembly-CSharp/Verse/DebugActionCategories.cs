using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003BF RID: 959
	public static class DebugActionCategories
	{
		// Token: 0x06001D8F RID: 7567 RVA: 0x000B8BB0 File Offset: 0x000B6DB0
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

		// Token: 0x06001D90 RID: 7568 RVA: 0x000B8C8C File Offset: 0x000B6E8C
		public static int GetOrderFor(string category)
		{
			int result;
			if (DebugActionCategories.categoryOrders.TryGetValue(category, out result))
			{
				return result;
			}
			return int.MaxValue;
		}

		// Token: 0x040011BC RID: 4540
		public const string Incidents = "Incidents";

		// Token: 0x040011BD RID: 4541
		public const string Quests = "Quests";

		// Token: 0x040011BE RID: 4542
		public const string QuestsOld = "Quests (old)";

		// Token: 0x040011BF RID: 4543
		public const string Translation = "Translation";

		// Token: 0x040011C0 RID: 4544
		public const string General = "General";

		// Token: 0x040011C1 RID: 4545
		public const string Pawns = "Pawns";

		// Token: 0x040011C2 RID: 4546
		public const string Spawning = "Spawning";

		// Token: 0x040011C3 RID: 4547
		public const string MapManagement = "Map management";

		// Token: 0x040011C4 RID: 4548
		public const string Autotests = "Autotests";

		// Token: 0x040011C5 RID: 4549
		public const string Mods = "Mods";

		// Token: 0x040011C6 RID: 4550
		public static readonly Dictionary<string, int> categoryOrders = new Dictionary<string, int>();
	}
}
