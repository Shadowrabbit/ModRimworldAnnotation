using System;

namespace Verse
{
	// Token: 0x02000391 RID: 913
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TweakValue : Attribute
	{
		// Token: 0x06001AD6 RID: 6870 RVA: 0x0009BCE8 File Offset: 0x00099EE8
		public TweakValue(string category, float min = 0f, float max = 100f)
		{
			this.category = category;
			this.min = min;
			this.max = max;
		}

		// Token: 0x04001178 RID: 4472
		public string category;

		// Token: 0x04001179 RID: 4473
		public float min;

		// Token: 0x0400117A RID: 4474
		public float max;
	}
}
