using System;

namespace Verse
{
	// Token: 0x020006D1 RID: 1745
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugOutputAttribute : Attribute
	{
		// Token: 0x06002CD0 RID: 11472 RVA: 0x000237D1 File Offset: 0x000219D1
		public DebugOutputAttribute()
		{
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000237E4 File Offset: 0x000219E4
		public DebugOutputAttribute(bool onlyWhenPlaying)
		{
			this.onlyWhenPlaying = onlyWhenPlaying;
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000237FE File Offset: 0x000219FE
		public DebugOutputAttribute(string category, bool onlyWhenPlaying = false) : this(onlyWhenPlaying)
		{
			this.category = category;
		}

		// Token: 0x04001E5B RID: 7771
		public string name;

		// Token: 0x04001E5C RID: 7772
		public string category = "General";

		// Token: 0x04001E5D RID: 7773
		public bool onlyWhenPlaying;
	}
}
