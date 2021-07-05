using System;

namespace Verse
{
	// Token: 0x020003C7 RID: 967
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugOutputAttribute : Attribute
	{
		// Token: 0x06001DAE RID: 7598 RVA: 0x000B9808 File Offset: 0x000B7A08
		public DebugOutputAttribute()
		{
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000B981B File Offset: 0x000B7A1B
		public DebugOutputAttribute(bool onlyWhenPlaying)
		{
			this.onlyWhenPlaying = onlyWhenPlaying;
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x000B9835 File Offset: 0x000B7A35
		public DebugOutputAttribute(string category, bool onlyWhenPlaying = false) : this(onlyWhenPlaying)
		{
			this.category = category;
		}

		// Token: 0x040011D6 RID: 4566
		public string name;

		// Token: 0x040011D7 RID: 4567
		public string category = "General";

		// Token: 0x040011D8 RID: 4568
		public bool onlyWhenPlaying;
	}
}
