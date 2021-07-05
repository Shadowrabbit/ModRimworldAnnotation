using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x0200002E RID: 46
	public static class GenTypes
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000D25B File Offset: 0x0000B45B
		private static IEnumerable<Assembly> AllActiveAssemblies
		{
			get
			{
				yield return Assembly.GetExecutingAssembly();
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					int num;
					for (int i = 0; i < mod.assemblies.loadedAssemblies.Count; i = num + 1)
					{
						yield return mod.assemblies.loadedAssemblies[i];
						num = i;
					}
					mod = null;
				}
				IEnumerator<ModContentPack> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000D264 File Offset: 0x0000B464
		public static IEnumerable<Type> AllTypes
		{
			get
			{
				foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
				{
					Type[] array = null;
					try
					{
						array = assembly.GetTypes();
					}
					catch (ReflectionTypeLoadException)
					{
						Log.Error("Exception getting types in assembly " + assembly.ToString());
					}
					if (array != null)
					{
						foreach (Type type in array)
						{
							yield return type;
						}
						Type[] array2 = null;
					}
				}
				IEnumerator<Assembly> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000D26D File Offset: 0x0000B46D
		public static IEnumerable<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			return from x in GenTypes.AllTypes
			where x.HasAttribute<TAttr>()
			select x;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000D298 File Offset: 0x0000B498
		public static List<Type> AllSubclasses(this Type baseType)
		{
			if (!GenTypes.cachedSubclasses.ContainsKey(baseType))
			{
				GenTypes.cachedSubclasses.Add(baseType, (from x in GenTypes.AllTypes
				where x.IsSubclassOf(baseType)
				select x).ToList<Type>());
			}
			return GenTypes.cachedSubclasses[baseType];
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000D300 File Offset: 0x0000B500
		public static List<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			if (!GenTypes.cachedSubclassesNonAbstract.ContainsKey(baseType))
			{
				GenTypes.cachedSubclassesNonAbstract.Add(baseType, (from x in GenTypes.AllTypes
				where x.IsSubclassOf(baseType) && !x.IsAbstract
				select x).ToList<Type>());
			}
			return GenTypes.cachedSubclassesNonAbstract[baseType];
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000D367 File Offset: 0x0000B567
		public static void ClearCache()
		{
			GenTypes.cachedSubclasses.Clear();
			GenTypes.cachedSubclassesNonAbstract.Clear();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000D37D File Offset: 0x0000B57D
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000D3A9 File Offset: 0x0000B5A9
		public static IEnumerable<Type> InstantiableDescendantsAndSelf(this Type baseType)
		{
			if (!baseType.IsAbstract)
			{
				yield return baseType;
			}
			foreach (Type type in baseType.AllSubclasses())
			{
				if (!type.IsAbstract)
				{
					yield return type;
				}
			}
			List<Type>.Enumerator enumerator = default(List<Type>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000D3BC File Offset: 0x0000B5BC
		public static Type GetTypeInAnyAssembly(string typeName, string namespaceIfAmbiguous = null)
		{
			GenTypes.TypeCacheKey key = new GenTypes.TypeCacheKey(typeName, namespaceIfAmbiguous);
			Type type = null;
			if (!GenTypes.typeCache.TryGetValue(key, out type))
			{
				type = GenTypes.GetTypeInAnyAssemblyInt(typeName, namespaceIfAmbiguous);
				GenTypes.typeCache.Add(key, type);
			}
			return type;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000D3F8 File Offset: 0x0000B5F8
		private static Type GetTypeInAnyAssemblyInt(string typeName, string namespaceIfAmbiguous = null)
		{
			Type typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (typeInAnyAssemblyRaw != null)
			{
				return typeInAnyAssemblyRaw;
			}
			if (!namespaceIfAmbiguous.NullOrEmpty() && GenTypes.IgnoredNamespaceNames.Contains(namespaceIfAmbiguous))
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(namespaceIfAmbiguous + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(GenTypes.IgnoredNamespaceNames[i] + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			if (GenTypes.TryGetMixedAssemblyGenericType(typeName, out typeInAnyAssemblyRaw))
			{
				return typeInAnyAssemblyRaw;
			}
			return null;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000D494 File Offset: 0x0000B694
		private static bool TryGetMixedAssemblyGenericType(string typeName, out Type type)
		{
			type = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (type == null && typeName.Contains("`"))
			{
				try
				{
					Match match = Regex.Match(typeName, "(?<MainType>.+`(?<ParamCount>[0-9]+))(?<Types>\\[.*\\])");
					if (match.Success)
					{
						int capacity = int.Parse(match.Groups["ParamCount"].Value);
						string value = match.Groups["Types"].Value;
						List<string> list = new List<string>(capacity);
						foreach (object obj in Regex.Matches(value, "\\[(?<Type>.*?)\\],?"))
						{
							Match match2 = (Match)obj;
							if (match2.Success)
							{
								list.Add(match2.Groups["Type"].Value.Trim());
							}
						}
						Type[] array = new Type[list.Count];
						for (int i = 0; i < list.Count; i++)
						{
							Type type2;
							if (!GenTypes.TryGetMixedAssemblyGenericType(list[i], out type2))
							{
								return false;
							}
							array[i] = type2;
						}
						Type type3;
						if (GenTypes.TryGetMixedAssemblyGenericType(match.Groups["MainType"].Value, out type3))
						{
							type = type3.MakeGenericType(array);
						}
					}
				}
				catch (Exception ex)
				{
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Error in TryGetMixedAssemblyGenericType with typeName=",
						typeName,
						": ",
						ex
					}), typeName.GetHashCode());
				}
			}
			return type != null;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000D65C File Offset: 0x0000B85C
		private static Type GetTypeInAnyAssemblyRaw(string typeName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(typeName);
			if (num <= 2299065237U)
			{
				if (num <= 1092586446U)
				{
					if (num <= 431052896U)
					{
						if (num != 296283782U)
						{
							if (num != 398550328U)
							{
								if (num == 431052896U)
								{
									if (typeName == "byte?")
									{
										return typeof(byte?);
									}
								}
							}
							else if (typeName == "string")
							{
								return typeof(string);
							}
						}
						else if (typeName == "char?")
						{
							return typeof(char?);
						}
					}
					else if (num != 513669818U)
					{
						if (num != 520654156U)
						{
							if (num == 1092586446U)
							{
								if (typeName == "float?")
								{
									return typeof(float?);
								}
							}
						}
						else if (typeName == "decimal")
						{
							return typeof(decimal);
						}
					}
					else if (typeName == "uint?")
					{
						return typeof(uint?);
					}
				}
				else if (num <= 1454009365U)
				{
					if (num != 1189328644U)
					{
						if (num != 1299622921U)
						{
							if (num == 1454009365U)
							{
								if (typeName == "sbyte?")
								{
									return typeof(sbyte?);
								}
							}
						}
						else if (typeName == "decimal?")
						{
							return typeof(decimal?);
						}
					}
					else if (typeName == "long?")
					{
						return typeof(long?);
					}
				}
				else if (num <= 1630192034U)
				{
					if (num != 1603400371U)
					{
						if (num == 1630192034U)
						{
							if (typeName == "ushort")
							{
								return typeof(ushort);
							}
						}
					}
					else if (typeName == "int?")
					{
						return typeof(int?);
					}
				}
				else if (num != 1683620383U)
				{
					if (num == 2299065237U)
					{
						if (typeName == "double?")
						{
							return typeof(double?);
						}
					}
				}
				else if (typeName == "byte")
				{
					return typeof(byte);
				}
			}
			else if (num <= 2823553821U)
			{
				if (num <= 2515107422U)
				{
					if (num != 2471414311U)
					{
						if (num != 2508976771U)
						{
							if (num == 2515107422U)
							{
								if (typeName == "int")
								{
									return typeof(int);
								}
							}
						}
						else if (typeName == "ulong?")
						{
							return typeof(ulong?);
						}
					}
					else if (typeName == "ushort?")
					{
						return typeof(ushort?);
					}
				}
				else if (num <= 2699759368U)
				{
					if (num != 2667225454U)
					{
						if (num == 2699759368U)
						{
							if (typeName == "double")
							{
								return typeof(double);
							}
						}
					}
					else if (typeName == "ulong")
					{
						return typeof(ulong);
					}
				}
				else if (num != 2797886853U)
				{
					if (num == 2823553821U)
					{
						if (typeName == "char")
						{
							return typeof(char);
						}
					}
				}
				else if (typeName == "float")
				{
					return typeof(float);
				}
			}
			else if (num <= 3286667814U)
			{
				if (num != 3122818005U)
				{
					if (num != 3270303571U)
					{
						if (num == 3286667814U)
						{
							if (typeName == "bool?")
							{
								return typeof(bool?);
							}
						}
					}
					else if (typeName == "long")
					{
						return typeof(long);
					}
				}
				else if (typeName == "short")
				{
					return typeof(short);
				}
			}
			else if (num <= 3415750305U)
			{
				if (num != 3365180733U)
				{
					if (num == 3415750305U)
					{
						if (typeName == "uint")
						{
							return typeof(uint);
						}
					}
				}
				else if (typeName == "bool")
				{
					return typeof(bool);
				}
			}
			else if (num != 3996115294U)
			{
				if (num == 4088464520U)
				{
					if (typeName == "sbyte")
					{
						return typeof(sbyte);
					}
				}
			}
			else if (typeName == "short?")
			{
				return typeof(short?);
			}
			foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
			{
				Type type = assembly.GetType(typeName, false, true);
				if (type != null)
				{
					return type;
				}
			}
			Type type2 = Type.GetType(typeName, false, true);
			if (type2 != null)
			{
				return type2;
			}
			return null;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		public static string GetTypeNameWithoutIgnoredNamespaces(Type type)
		{
			if (type.IsGenericType)
			{
				return type.ToString();
			}
			for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
			{
				if (type.Namespace == GenTypes.IgnoredNamespaceNames[i])
				{
					return type.Name;
				}
			}
			return type.FullName;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000DC40 File Offset: 0x0000BE40
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return @namespace == null || (!@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks"));
		}

		// Token: 0x04000071 RID: 113
		public static readonly List<string> IgnoredNamespaceNames = new List<string>
		{
			"RimWorld",
			"Verse",
			"Verse.AI",
			"Verse.Sound",
			"Verse.Grammar",
			"RimWorld.Planet",
			"RimWorld.BaseGen",
			"RimWorld.QuestGen",
			"RimWorld.SketchGen",
			"System"
		};

		// Token: 0x04000072 RID: 114
		private static Dictionary<Type, List<Type>> cachedSubclasses = new Dictionary<Type, List<Type>>();

		// Token: 0x04000073 RID: 115
		private static Dictionary<Type, List<Type>> cachedSubclassesNonAbstract = new Dictionary<Type, List<Type>>();

		// Token: 0x04000074 RID: 116
		private static Dictionary<GenTypes.TypeCacheKey, Type> typeCache = new Dictionary<GenTypes.TypeCacheKey, Type>(EqualityComparer<GenTypes.TypeCacheKey>.Default);

		// Token: 0x02001886 RID: 6278
		private struct TypeCacheKey : IEquatable<GenTypes.TypeCacheKey>
		{
			// Token: 0x060093B9 RID: 37817 RVA: 0x0034D89F File Offset: 0x0034BA9F
			public override int GetHashCode()
			{
				if (this.namespaceIfAmbiguous == null)
				{
					return this.typeName.GetHashCode();
				}
				return (17 * 31 + this.typeName.GetHashCode()) * 31 + this.namespaceIfAmbiguous.GetHashCode();
			}

			// Token: 0x060093BA RID: 37818 RVA: 0x0034D8D5 File Offset: 0x0034BAD5
			public bool Equals(GenTypes.TypeCacheKey other)
			{
				return string.Equals(this.typeName, other.typeName) && string.Equals(this.namespaceIfAmbiguous, other.namespaceIfAmbiguous);
			}

			// Token: 0x060093BB RID: 37819 RVA: 0x0034D8FD File Offset: 0x0034BAFD
			public override bool Equals(object obj)
			{
				return obj is GenTypes.TypeCacheKey && this.Equals((GenTypes.TypeCacheKey)obj);
			}

			// Token: 0x060093BC RID: 37820 RVA: 0x0034D915 File Offset: 0x0034BB15
			public TypeCacheKey(string typeName, string namespaceIfAmbigous = null)
			{
				this.typeName = typeName;
				this.namespaceIfAmbiguous = namespaceIfAmbigous;
			}

			// Token: 0x04005DE7 RID: 24039
			public string typeName;

			// Token: 0x04005DE8 RID: 24040
			public string namespaceIfAmbiguous;
		}
	}
}
