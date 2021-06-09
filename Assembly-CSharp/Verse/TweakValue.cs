using System;

namespace Verse
{
	// Token: 0x0200053B RID: 1339
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TweakValue : Attribute
	{
		// Token: 0x0600226B RID: 8811 RVA: 0x0001D906 File Offset: 0x0001BB06
		public TweakValue(string category, float min = 0f, float max = 100f)
		{
			this.category = category;
			this.min = min;
			this.max = max;
		}

		// Token: 0x04001756 RID: 5974
		public string category;

		// Token: 0x04001757 RID: 5975
		public float min;

		// Token: 0x04001758 RID: 5976
		public float max;
	}
}
