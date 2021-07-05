using System;

namespace Verse
{
	// Token: 0x020003D4 RID: 980
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x06001DDA RID: 7642 RVA: 0x000BAD7C File Offset: 0x000B8F7C
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000BAD8B File Offset: 0x000B8F8B
		public virtual bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return this.value == null;
			}
			return this.value != null && this.value.Equals(obj);
		}

		// Token: 0x040011EC RID: 4588
		public object value;
	}
}
