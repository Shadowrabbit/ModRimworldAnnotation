using System;

namespace Verse
{
	// Token: 0x020001B2 RID: 434
	[Flags]
	public enum WorkTags
	{
		// Token: 0x040009DB RID: 2523
		None = 0,
		// Token: 0x040009DC RID: 2524
		ManualDumb = 2,
		// Token: 0x040009DD RID: 2525
		ManualSkilled = 4,
		// Token: 0x040009DE RID: 2526
		Violent = 8,
		// Token: 0x040009DF RID: 2527
		Caring = 16,
		// Token: 0x040009E0 RID: 2528
		Social = 32,
		// Token: 0x040009E1 RID: 2529
		Commoner = 64,
		// Token: 0x040009E2 RID: 2530
		Intellectual = 128,
		// Token: 0x040009E3 RID: 2531
		Animals = 256,
		// Token: 0x040009E4 RID: 2532
		Artistic = 512,
		// Token: 0x040009E5 RID: 2533
		Crafting = 1024,
		// Token: 0x040009E6 RID: 2534
		Cooking = 2048,
		// Token: 0x040009E7 RID: 2535
		Firefighting = 4096,
		// Token: 0x040009E8 RID: 2536
		Cleaning = 8192,
		// Token: 0x040009E9 RID: 2537
		Hauling = 16384,
		// Token: 0x040009EA RID: 2538
		PlantWork = 32768,
		// Token: 0x040009EB RID: 2539
		Mining = 65536,
		// Token: 0x040009EC RID: 2540
		Hunting = 131072,
		// Token: 0x040009ED RID: 2541
		AllWork = 262144
	}
}
