using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000024 RID: 36
	public static class GenAttribute
	{
		// Token: 0x060001DC RID: 476 RVA: 0x0000997C File Offset: 0x00007B7C
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T t;
			return memberInfo.TryGetAttribute(out t);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009994 File Offset: 0x00007B94
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

		// Token: 0x060001DE RID: 478 RVA: 0x000099F0 File Offset: 0x00007BF0
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result = default(T);
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
