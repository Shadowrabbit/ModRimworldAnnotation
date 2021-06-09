using System;

namespace Verse
{
	// Token: 0x020006E4 RID: 1764
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x06002D10 RID: 11536 RVA: 0x000239BD File Offset: 0x00021BBD
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000239CC File Offset: 0x00021BCC
		public virtual bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return this.value == null;
			}
			return this.value != null && this.value.Equals(obj);
		}

		// Token: 0x04001E83 RID: 7811
		public object value;
	}
}
