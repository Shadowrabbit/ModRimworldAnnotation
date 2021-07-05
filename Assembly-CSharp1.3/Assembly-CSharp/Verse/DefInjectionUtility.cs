using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld.QuestGen;

namespace Verse
{
	// Token: 0x02000150 RID: 336
	public static class DefInjectionUtility
	{
		// Token: 0x06000961 RID: 2401 RVA: 0x00030820 File Offset: 0x0002EA20
		public static void ForEachPossibleDefInjection(Type defType, DefInjectionUtility.PossibleDefInjectionTraverser action, ModMetaData onlyFromMod = null)
		{
			foreach (Def def in GenDefDatabase.GetAllDefsInDatabaseForDef(defType))
			{
				if (onlyFromMod == null || (def.modContentPack != null && !(def.modContentPack.PackageId != onlyFromMod.PackageId)))
				{
					DefInjectionUtility.ForEachPossibleDefInjectionInDef(def, action);
				}
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00030890 File Offset: 0x0002EA90
		private static void ForEachPossibleDefInjectionInDef(Def def, DefInjectionUtility.PossibleDefInjectionTraverser action)
		{
			HashSet<object> visited = new HashSet<object>();
			DefInjectionUtility.ForEachPossibleDefInjectionInDefRecursive(def, def.defName, def.defName, visited, true, def, action);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x000308BC File Offset: 0x0002EABC
		private static void ForEachPossibleDefInjectionInDefRecursive(object obj, string curNormalizedPath, string curSuggestedPath, HashSet<object> visited, bool translationAllowed, Def def, DefInjectionUtility.PossibleDefInjectionTraverser action)
		{
			if (obj == null)
			{
				return;
			}
			if (!obj.GetType().IsValueType && visited.Contains(obj))
			{
				return;
			}
			visited.Add(obj);
			foreach (FieldInfo fieldInfo in DefInjectionUtility.FieldsInDeterministicOrder(obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)))
			{
				object value = fieldInfo.GetValue(obj);
				bool flag = translationAllowed && !fieldInfo.HasAttribute<NoTranslateAttribute>() && !fieldInfo.HasAttribute<UnsavedAttribute>();
				if (!(value is Def))
				{
					if (typeof(string).IsAssignableFrom(fieldInfo.FieldType))
					{
						string currentValue = (string)value;
						string text = curNormalizedPath + "." + fieldInfo.Name;
						string suggestedPath = curSuggestedPath + "." + fieldInfo.Name;
						string text2;
						if (TKeySystem.TrySuggestTKeyPath(text, out text2, null))
						{
							suggestedPath = text2;
						}
						action(suggestedPath, text, false, currentValue, null, flag, false, fieldInfo, def);
					}
					else if (value is IEnumerable<string>)
					{
						IEnumerable<string> currentValueCollection = (IEnumerable<string>)value;
						bool flag2 = fieldInfo.HasAttribute<TranslationCanChangeCountAttribute>();
						string text3 = curNormalizedPath + "." + fieldInfo.Name;
						string suggestedPath2 = curSuggestedPath + "." + fieldInfo.Name;
						string text4;
						if (TKeySystem.TrySuggestTKeyPath(text3, out text4, null))
						{
							suggestedPath2 = text4;
						}
						action(suggestedPath2, text3, true, null, currentValueCollection, flag, flag && flag2, fieldInfo, def);
					}
					else
					{
						if (value is IEnumerable)
						{
							IEnumerable enumerable = (IEnumerable)value;
							int num = 0;
							using (IEnumerator enumerator2 = enumerable.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj2 = enumerator2.Current;
									if (obj2 != null && !(obj2 is Def) && GenTypes.IsCustomType(obj2.GetType()))
									{
										string text5 = TranslationHandleUtility.GetBestHandleWithIndexForListElement(enumerable, obj2);
										if (text5.NullOrEmpty())
										{
											text5 = num.ToString();
										}
										string curNormalizedPath2 = string.Concat(new object[]
										{
											curNormalizedPath,
											".",
											fieldInfo.Name,
											".",
											num
										});
										string curSuggestedPath2 = string.Concat(new string[]
										{
											curSuggestedPath,
											".",
											fieldInfo.Name,
											".",
											text5
										});
										DefInjectionUtility.ForEachPossibleDefInjectionInDefRecursive(obj2, curNormalizedPath2, curSuggestedPath2, visited, flag, def, action);
									}
									num++;
								}
								continue;
							}
						}
						if (value != null && GenTypes.IsCustomType(value.GetType()))
						{
							string curNormalizedPath3 = curNormalizedPath + "." + fieldInfo.Name;
							string curSuggestedPath3 = curSuggestedPath + "." + fieldInfo.Name;
							DefInjectionUtility.ForEachPossibleDefInjectionInDefRecursive(value, curNormalizedPath3, curSuggestedPath3, visited, flag, def, action);
						}
					}
				}
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00030BB0 File Offset: 0x0002EDB0
		public static bool ShouldCheckMissingInjection(string str, FieldInfo fi, Def def)
		{
			if (def.generated)
			{
				return false;
			}
			if (str.NullOrEmpty())
			{
				return false;
			}
			if (fi.HasAttribute<NoTranslateAttribute>() || fi.HasAttribute<UnsavedAttribute>() || fi.HasAttribute<MayTranslateAttribute>())
			{
				return false;
			}
			if (fi.HasAttribute<MustTranslate_SlateRefAttribute>())
			{
				return SlateRefUtility.MustTranslate(str, fi);
			}
			return fi.HasAttribute<MustTranslateAttribute>() || str.Contains(' ');
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00030C10 File Offset: 0x0002EE10
		private static IEnumerable<FieldInfo> FieldsInDeterministicOrder(IEnumerable<FieldInfo> fields)
		{
			return from x in fields
			orderby x.HasAttribute<UnsavedAttribute>() || x.HasAttribute<NoTranslateAttribute>(), x.Name == "label" descending, x.Name == "description" descending, x.Name
			select x;
		}

		// Token: 0x0200192C RID: 6444
		// (Invoke) Token: 0x0600976F RID: 38767
		public delegate void PossibleDefInjectionTraverser(string suggestedPath, string normalizedPath, bool isCollection, string currentValue, IEnumerable<string> currentValueCollection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def);
	}
}
