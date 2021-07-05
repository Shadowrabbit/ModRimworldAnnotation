using System;

namespace Verse
{
	// Token: 0x020003D1 RID: 977
	[AttributeUsage(AttributeTargets.Field)]
	public class TranslationHandleAttribute : Attribute
	{
		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x000BAD3B File Offset: 0x000B8F3B
		// (set) Token: 0x06001DD6 RID: 7638 RVA: 0x000BAD43 File Offset: 0x000B8F43
		public int Priority { get; set; }
	}
}
