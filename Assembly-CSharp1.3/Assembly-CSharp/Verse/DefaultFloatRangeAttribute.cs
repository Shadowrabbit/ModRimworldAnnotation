using System;

namespace Verse
{
	// Token: 0x020003D5 RID: 981
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x06001DDC RID: 7644 RVA: 0x000BADB0 File Offset: 0x000B8FB0
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
