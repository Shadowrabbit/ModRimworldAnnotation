using System;

namespace Verse
{
	// Token: 0x020006E5 RID: 1765
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x06002D12 RID: 11538 RVA: 0x000239F1 File Offset: 0x00021BF1
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
