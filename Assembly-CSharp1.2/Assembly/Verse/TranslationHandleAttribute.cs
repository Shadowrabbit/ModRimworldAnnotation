using System;

namespace Verse
{
	// Token: 0x020006E1 RID: 1761
	[AttributeUsage(AttributeTargets.Field)]
	public class TranslationHandleAttribute : Attribute
	{
		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x0002397C File Offset: 0x00021B7C
		// (set) Token: 0x06002D0C RID: 11532 RVA: 0x00023984 File Offset: 0x00021B84
		public int Priority { get; set; }
	}
}
