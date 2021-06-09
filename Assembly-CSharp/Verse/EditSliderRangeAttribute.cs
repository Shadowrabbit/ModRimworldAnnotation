using System;

namespace Verse
{
	// Token: 0x020006E2 RID: 1762
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x06002D0E RID: 11534 RVA: 0x0002398D File Offset: 0x00021B8D
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x04001E80 RID: 7808
		public float min;

		// Token: 0x04001E81 RID: 7809
		public float max = 1f;
	}
}
