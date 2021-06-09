using System;

namespace Verse
{
	// Token: 0x020006E3 RID: 1763
	[AttributeUsage(AttributeTargets.Field)]
	public class UnsavedAttribute : Attribute
	{
		// Token: 0x06002D0F RID: 11535 RVA: 0x000239AE File Offset: 0x00021BAE
		public UnsavedAttribute(bool allowLoading = false)
		{
			this.allowLoading = allowLoading;
		}

		// Token: 0x04001E82 RID: 7810
		public bool allowLoading;
	}
}
