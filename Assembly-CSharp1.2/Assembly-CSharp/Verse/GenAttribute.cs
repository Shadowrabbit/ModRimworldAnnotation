using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000037 RID: 55
	public static class GenAttribute
	{
		// Token: 0x0600027A RID: 634 RVA: 0x00080144 File Offset: 0x0007E344
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T t;
			return memberInfo.TryGetAttribute(out t);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0008015C File Offset: 0x0007E35C
		public static bool TryGetAttribute<T>(this MemberInfo memberInfo, out T customAttribute) where T : Attribute
		{
			object[] customAttributes = memberInfo.GetCustomAttributes(typeof(T), true);
			if (customAttributes.Length == 0)
			{
				customAttribute = default(T);
				return false;
			}
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] is T)
				{
					customAttribute = (T)((object)customAttributes[i]);
					return true;
				}
			}
			customAttribute = default(T);
			return false;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000801B8 File Offset: 0x0007E3B8
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result = default(T);
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
