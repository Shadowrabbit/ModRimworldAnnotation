using System;

namespace Verse
{
	// Token: 0x02000045 RID: 69
	public static class MurmurHash
	{
		// Token: 0x0600038A RID: 906 RVA: 0x00013274 File Offset: 0x00011474
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

		// Token: 0x040000E5 RID: 229
		private const uint Const1 = 3432918353U;

		// Token: 0x040000E6 RID: 230
		private const uint Const2 = 461845907U;

		// Token: 0x040000E7 RID: 231
		private const uint Const3 = 3864292196U;

		// Token: 0x040000E8 RID: 232
		private const uint Const4Mix = 2246822507U;

		// Token: 0x040000E9 RID: 233
		private const uint Const5Mix = 3266489909U;

		// Token: 0x040000EA RID: 234
		private const uint Const6StreamPosition = 2834544218U;
	}
}
