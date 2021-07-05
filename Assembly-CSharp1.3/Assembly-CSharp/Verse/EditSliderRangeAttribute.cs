using System;

namespace Verse
{
	// Token: 0x020003D2 RID: 978
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x06001DD8 RID: 7640 RVA: 0x000BAD4C File Offset: 0x000B8F4C
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040011E9 RID: 4585
		public float min;

		// Token: 0x040011EA RID: 4586
		public float max = 1f;
	}
}
