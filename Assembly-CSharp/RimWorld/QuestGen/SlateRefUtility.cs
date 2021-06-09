using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02002005 RID: 8197
	public static class SlateRefUtility
	{
		// Token: 0x0600ADA6 RID: 44454 RVA: 0x00328A90 File Offset: 0x00326C90
		public static bool CheckSingleVariableSyntax(string str, Slate slate, out object obj, out bool exists)
		{
			if (str.NullOrEmpty())
			{
				obj = null;
				exists = false;
				return false;
			}
			if (str[0] != '$')
			{
				obj = null;
				exists = false;
				return false;
			}
			for (int i = 1; i < str.Length; i++)
			{
				if (!char.IsLetterOrDigit(str[i]) && str[i] != '_' && str[i] != '/')
				{
					obj = null;
					exists = false;
					return false;
				}
			}
			if (slate != null)
			{
				exists = slate.TryGet<object>(str.Substring(1), out obj, false);
			}
			else
			{
				exists = false;
				obj = null;
			}
			return true;
		}

		// Token: 0x0600ADA7 RID: 44455 RVA: 0x00328B1C File Offset: 0x00326D1C
		public static bool MustTranslate(string slateRef, FieldInfo fi)
		{
			if (slateRef.NullOrEmpty())
			{
				return false;
			}
			if (slateRef.Trim().Length == 0)
			{
				return false;
			}
			object obj;
			bool flag;
			if (SlateRefUtility.CheckSingleVariableSyntax(slateRef, null, out obj, out flag))
			{
				return false;
			}
			bool flag2 = false;
			for (int i = 0; i < slateRef.Length; i++)
			{
				if (char.IsLetter(slateRef[i]))
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				return false;
			}
			if (slateRef.Length >= 3 && slateRef[0] == '$' && slateRef[1] == '(' && slateRef[slateRef.Length - 1] == ')')
			{
				return false;
			}
			if (fi.DeclaringType.IsGenericType && fi.DeclaringType.GetGenericTypeDefinition() == typeof(SlateRef<>))
			{
				Type type = fi.DeclaringType.GetGenericArguments()[0];
				if (type.IsGenericType)
				{
					Type genericTypeDefinition = type.GetGenericTypeDefinition();
					if (genericTypeDefinition == typeof(IEnumerable<>) || genericTypeDefinition == typeof(IList<>) || genericTypeDefinition == typeof(List<>))
					{
						type = type.GetGenericArguments()[0];
					}
				}
				if (type != typeof(string) && type != typeof(object) && type != typeof(RulePack))
				{
					return false;
				}
				if (type == typeof(object) && (!slateRef.Contains(" ") || (ConvertHelper.IsXml(slateRef) && !slateRef.Contains("<rulesStrings>"))))
				{
					return false;
				}
			}
			return true;
		}
	}
}
