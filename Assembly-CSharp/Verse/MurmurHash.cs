using System;

namespace Verse
{
	// Token: 0x02000089 RID: 137
	public static class MurmurHash
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x00089578 File Offset: 0x00087778
		public static int GetInt(uint seed, uint input)
		{
			uint num = input * 3432918353U;
			num = (num << 15 | num >> 17);
			num *= 461845907U;
			uint num2 = seed ^ num;
			num2 = (num2 << 13 | num2 >> 19);
			num2 = num2 * 5U + 3864292196U;
			num2 ^= 2834544218U;
			num2 ^= num2 >> 16;
			num2 *= 2246822507U;
			num2 ^= num2 >> 13;
			num2 *= 3266489909U;
			return (int)(num2 ^ num2 >> 16);
		}

		// Token: 0x0400024B RID: 587
		private const uint Const1 = 3432918353U;

		// Token: 0x0400024C RID: 588
		private const uint Const2 = 461845907U;

		// Token: 0x0400024D RID: 589
		private const uint Const3 = 3864292196U;

		// Token: 0x0400024E RID: 590
		private const uint Const4Mix = 2246822507U;

		// Token: 0x0400024F RID: 591
		private const uint Const5Mix = 3266489909U;

		// Token: 0x04000250 RID: 592
		private const uint Const6StreamPosition = 2834544218U;
	}
}
