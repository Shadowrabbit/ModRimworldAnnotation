using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001729 RID: 5929
	public static class SlateRefUtility
	{
		// Token: 0x060088BC RID: 35004 RVA: 0x003122E8 File Offset: 0x003104E8
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

		// Token: 0x060088BD RID: 35005 RVA: 0x00312374 File Offset: 0x00310574
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
