using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003D6 RID: 982
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		// Token: 0x06001DDD RID: 7645 RVA: 0x000BADC4 File Offset: 0x000B8FC4
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x000BADD0 File Offset: 0x000B8FD0
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
