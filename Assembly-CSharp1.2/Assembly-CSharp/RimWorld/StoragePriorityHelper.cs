using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001696 RID: 5782
	public static class StoragePriorityHelper
	{
		// Token: 0x06007E7D RID: 32381 RVA: 0x00259EFC File Offset: 0x002580FC
		public static string Label(this StoragePriority p)
		{
			switch (p)
			{
			case StoragePriority.Unstored:
				return "StoragePriorityUnstored".Translate();
			case StoragePriority.Low:
				return "StoragePriorityLow".Translate();
			case StoragePriority.Normal:
				return "StoragePriorityNormal".Translate();
			case StoragePriority.Preferred:
				return "StoragePriorityPreferred".Translate();
			case StoragePriority.Important:
				return "StoragePriorityImportant".Translate();
			case StoragePriority.Critical:
				return "StoragePriorityCritical".Translate();
			default:
				return "Unknown";
			}
		}
	}
}
