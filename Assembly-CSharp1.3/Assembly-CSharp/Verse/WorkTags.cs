using System;

namespace Verse
{
	// Token: 0x0200011F RID: 287
	[Flags]
	public enum WorkTags
	{
		// Token: 0x04000760 RID: 1888
		None = 0,
		// Token: 0x04000761 RID: 1889
		ManualDumb = 2,
		// Token: 0x04000762 RID: 1890
		ManualSkilled = 4,
		// Token: 0x04000763 RID: 1891
		Violent = 8,
		// Token: 0x04000764 RID: 1892
		Caring = 16,
		// Token: 0x04000765 RID: 1893
		Social = 32,
		// Token: 0x04000766 RID: 1894
		Commoner = 64,
		// Token: 0x04000767 RID: 1895
		Intellectual = 128,
		// Token: 0x04000768 RID: 1896
		Animals = 256,
		// Token: 0x04000769 RID: 1897
		Artistic = 512,
		// Token: 0x0400076A RID: 1898
		Crafting = 1024,
		// Token: 0x0400076B RID: 1899
		Cooking = 2048,
		// Token: 0x0400076C RID: 1900
		Firefighting = 4096,
		// Token: 0x0400076D RID: 1901
		Cleaning = 8192,
		// Token: 0x0400076E RID: 1902
		Hauling = 16384,
		// Token: 0x0400076F RID: 1903
		PlantWork = 32768,
		// Token: 0x04000770 RID: 1904
		Mining = 65536,
		// Token: 0x04000771 RID: 1905
		Hunting = 131072,
		// Token: 0x04000772 RID: 1906
		Constructing = 262144,
		// Token: 0x04000773 RID: 1907
		Shooting = 524288,
		// Token: 0x04000774 RID: 1908
		AllWork = 1048576
	}
}
