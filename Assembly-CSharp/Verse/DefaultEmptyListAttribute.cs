using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006E6 RID: 1766
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		// Token: 0x06002D13 RID: 11539 RVA: 0x00023A05 File Offset: 0x00021C05
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x00131DBC File Offset: 0x0012FFBC
		public override bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType().GetGenericTypeDefinition() != typeof(List<>))
			{
				return false;
			}
			Type[] genericArguments = obj.GetType().GetGenericArguments();
			return genericArguments.Length == 1 && !(genericArguments[0] != (Type)this.value) && (int)obj.GetType().GetProperty("Count").GetValue(obj, null) == 0;
		}
	}
}
