using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using RimWorld.QuestGen;

namespace Verse
{
	// Token: 0x02000491 RID: 1169
	public static class DirectXmlToObject
	{
		// Token: 0x06001D5C RID: 7516 RVA: 0x000F49A0 File Offset: 0x000F2BA0
		public static Func<XmlNode, bool, object> GetObjectFromXmlMethod(Type type)
		{
			Func<XmlNode, bool, object> func;
			if (!DirectXmlToObject.objectFromXmlMethods.TryGetValue(type, out func))
			{
				MethodInfo method = typeof(DirectXmlToObject).GetMethod("ObjectFromXmlReflection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				DirectXmlToObject.tmpOneTypeArray[0] = type;
				func = (Func<XmlNode, bool, object>)Delegate.CreateDelegate(typeof(Func<XmlNode, bool, object>), method.MakeGenericMethod(DirectXmlToObject.tmpOneTypeArray));
				DirectXmlToObject.objectFromXmlMethods.Add(type, func);
			}
			return func;
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x0001A5C7 File Offset: 0x000187C7
		private static object ObjectFromXmlReflection<T>(XmlNode xmlRoot, bool doPostLoad)
		{
			return DirectXmlToObject.ObjectFromXml<T>(xmlRoot, doPostLoad);
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000F4A08 File Offset: 0x000F2C08
		public static T ObjectFromXml<T>(XmlNode xmlRoot, bool doPostLoad)
		{
			XmlAttribute xmlAttribute = xmlRoot.Attributes["IsNull"];
			T result;
			if (xmlAttribute != null && xmlAttribute.Value.ToUpperInvariant() == "TRUE")
			{
				result = default(T);
				return result;
			}
			MethodInfo methodInfo = DirectXmlToObject.CustomDataLoadMethodOf(typeof(T));
			if (methodInfo != null)
			{
				xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
				Type type = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
				DirectXmlToObject.currentlyInstantiatingObjectOfType.Push(type);
				T t;
				try
				{
					t = (T)((object)Activator.CreateInstance(type));
				}
				finally
				{
					DirectXmlToObject.currentlyInstantiatingObjectOfType.Pop();
				}
				try
				{
					methodInfo.Invoke(t, new object[]
					{
						xmlRoot
					});
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception in custom XML loader for ",
						typeof(T),
						". Node is:\n ",
						xmlRoot.OuterXml,
						"\n\nException is:\n ",
						ex.ToString()
					}), false);
					t = default(T);
				}
				if (doPostLoad)
				{
					DirectXmlToObject.TryDoPostLoad(t);
				}
				return t;
			}
			if (typeof(ISlateRef).IsAssignableFrom(typeof(T)))
			{
				try
				{
					return ParseHelper.FromString<T>(DirectXmlToObject.InnerTextWithReplacedNewlinesOrXML(xmlRoot));
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception parsing ",
						xmlRoot.OuterXml,
						" to type ",
						typeof(T),
						": ",
						ex2
					}), false);
				}
				return default(T);
			}
			if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.CDATA)
			{
				if (typeof(T) != typeof(string))
				{
					Log.Error("CDATA can only be used for strings. Bad xml: " + xmlRoot.OuterXml, false);
					return default(T);
				}
				return (T)((object)xmlRoot.FirstChild.Value);
			}
			else
			{
				if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.Text)
				{
					try
					{
						return ParseHelper.FromString<T>(xmlRoot.InnerText);
					}
					catch (Exception ex3)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception parsing ",
							xmlRoot.OuterXml,
							" to type ",
							typeof(T),
							": ",
							ex3
						}), false);
					}
					return default(T);
				}
				if (Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
				{
					List<T> list = DirectXmlToObject.ListFromXml<T>(xmlRoot);
					int num = 0;
					foreach (T t2 in list)
					{
						int num2 = (int)((object)t2);
						num |= num2;
					}
					return (T)((object)num);
				}
				if (typeof(T).HasGenericDefinition(typeof(List<>)))
				{
					Func<XmlNode, object> func = null;
					if (!DirectXmlToObject.listFromXmlMethods.TryGetValue(typeof(T), out func))
					{
						MethodInfo method = typeof(DirectXmlToObject).GetMethod("ListFromXmlReflection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						Type[] genericArguments = typeof(T).GetGenericArguments();
						func = (Func<XmlNode, object>)Delegate.CreateDelegate(typeof(Func<XmlNode, object>), method.MakeGenericMethod(genericArguments));
						DirectXmlToObject.listFromXmlMethods.Add(typeof(T), func);
					}
					return (T)((object)func(xmlRoot));
				}
				if (typeof(T).HasGenericDefinition(typeof(Dictionary<, >)))
				{
					Func<XmlNode, object> func2 = null;
					if (!DirectXmlToObject.dictionaryFromXmlMethods.TryGetValue(typeof(T), out func2))
					{
						MethodInfo method2 = typeof(DirectXmlToObject).GetMethod("DictionaryFromXmlReflection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						Type[] genericArguments2 = typeof(T).GetGenericArguments();
						func2 = (Func<XmlNode, object>)Delegate.CreateDelegate(typeof(Func<XmlNode, object>), method2.MakeGenericMethod(genericArguments2));
						DirectXmlToObject.dictionaryFromXmlMethods.Add(typeof(T), func2);
					}
					return (T)((object)func2(xmlRoot));
				}
				if (!xmlRoot.HasChildNodes)
				{
					if (typeof(T) == typeof(string))
					{
						return (T)((object)"");
					}
					XmlAttribute xmlAttribute2 = xmlRoot.Attributes["IsNull"];
					if (xmlAttribute2 != null && xmlAttribute2.Value.ToUpperInvariant() == "TRUE")
					{
						return default(T);
					}
					if (typeof(T).IsGenericType)
					{
						Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
						if (genericTypeDefinition == typeof(List<>) || genericTypeDefinition == typeof(HashSet<>) || genericTypeDefinition == typeof(Dictionary<, >))
						{
							return Activator.CreateInstance<T>();
						}
					}
				}
				xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
				Type type2 = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
				Type type3 = Nullable.GetUnderlyingType(type2) ?? type2;
				DirectXmlToObject.currentlyInstantiatingObjectOfType.Push(type3);
				T t3;
				try
				{
					t3 = (T)((object)Activator.CreateInstance(type3));
				}
				finally
				{
					DirectXmlToObject.currentlyInstantiatingObjectOfType.Pop();
				}
				HashSet<string> hashSet = null;
				if (xmlRoot.ChildNodes.Count > 1)
				{
					hashSet = new HashSet<string>();
				}
				for (int i = 0; i < xmlRoot.ChildNodes.Count; i++)
				{
					XmlNode xmlNode = xmlRoot.ChildNodes[i];
					if (!(xmlNode is XmlComment))
					{
						if (xmlRoot.ChildNodes.Count > 1)
						{
							if (hashSet.Contains(xmlNode.Name))
							{
								Log.Error(string.Concat(new object[]
								{
									"XML ",
									typeof(T),
									" defines the same field twice: ",
									xmlNode.Name,
									".\n\nField contents: ",
									xmlNode.InnerText,
									".\n\nWhole XML:\n\n",
									xmlRoot.OuterXml
								}), false);
							}
							else
							{
								hashSet.Add(xmlNode.Name);
							}
						}
						FieldInfo fieldInfo = null;
						DeepProfiler.Start("GetFieldInfoForType");
						try
						{
							fieldInfo = DirectXmlToObject.GetFieldInfoForType(t3.GetType(), xmlNode.Name, xmlRoot);
						}
						finally
						{
							DeepProfiler.End();
						}
						if (fieldInfo == null)
						{
							DeepProfiler.Start("Field search");
							try
							{
								DirectXmlToObject.FieldAliasCache key = new DirectXmlToObject.FieldAliasCache(t3.GetType(), xmlNode.Name);
								if (!DirectXmlToObject.fieldAliases.TryGetValue(key, out fieldInfo))
								{
									foreach (FieldInfo fieldInfo2 in t3.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
									{
										object[] customAttributes = fieldInfo2.GetCustomAttributes(typeof(LoadAliasAttribute), true);
										for (int k = 0; k < customAttributes.Length; k++)
										{
											if (((LoadAliasAttribute)customAttributes[k]).alias.EqualsIgnoreCase(xmlNode.Name))
											{
												fieldInfo = fieldInfo2;
												break;
											}
										}
										if (fieldInfo != null)
										{
											break;
										}
									}
									DirectXmlToObject.fieldAliases.Add(key, fieldInfo);
								}
							}
							finally
							{
								DeepProfiler.End();
							}
						}
						if (fieldInfo != null && fieldInfo.TryGetAttribute<UnsavedAttribute>() != null && !fieldInfo.TryGetAttribute<UnsavedAttribute>().allowLoading)
						{
							Log.Error(string.Concat(new string[]
							{
								"XML error: ",
								xmlNode.OuterXml,
								" corresponds to a field in type ",
								t3.GetType().Name,
								" which has an Unsaved attribute. Context: ",
								xmlRoot.OuterXml
							}), false);
						}
						else
						{
							if (fieldInfo == null)
							{
								DeepProfiler.Start("Field search 2");
								try
								{
									bool flag = false;
									XmlAttributeCollection attributes = xmlNode.Attributes;
									XmlAttribute xmlAttribute3 = (attributes != null) ? attributes["IgnoreIfNoMatchingField"] : null;
									if (xmlAttribute3 != null && xmlAttribute3.Value.ToUpperInvariant() == "TRUE")
									{
										flag = true;
									}
									else
									{
										object[] customAttributes = t3.GetType().GetCustomAttributes(typeof(IgnoreSavedElementAttribute), true);
										for (int j = 0; j < customAttributes.Length; j++)
										{
											if (string.Equals(((IgnoreSavedElementAttribute)customAttributes[j]).elementToIgnore, xmlNode.Name, StringComparison.OrdinalIgnoreCase))
											{
												flag = true;
												break;
											}
										}
									}
									if (flag)
									{
										goto IL_991;
									}
									Log.Error(string.Concat(new string[]
									{
										"XML error: ",
										xmlNode.OuterXml,
										" doesn't correspond to any field in type ",
										t3.GetType().Name,
										". Context: ",
										xmlRoot.OuterXml
									}), false);
									goto IL_991;
								}
								finally
								{
									DeepProfiler.End();
								}
							}
							if (typeof(Def).IsAssignableFrom(fieldInfo.FieldType))
							{
								if (xmlNode.InnerText.NullOrEmpty())
								{
									fieldInfo.SetValue(t3, null);
								}
								else
								{
									XmlAttribute xmlAttribute4 = xmlNode.Attributes["MayRequire"];
									DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(t3, fieldInfo, xmlNode.InnerText, (xmlAttribute4 != null) ? xmlAttribute4.Value.ToLower() : null, null);
								}
							}
							else
							{
								object value = null;
								try
								{
									value = DirectXmlToObject.GetObjectFromXmlMethod(fieldInfo.FieldType)(xmlNode, doPostLoad);
								}
								catch (Exception ex4)
								{
									Log.Error("Exception loading from " + xmlNode.ToString() + ": " + ex4.ToString(), false);
									goto IL_991;
								}
								if (!typeof(T).IsValueType)
								{
									fieldInfo.SetValue(t3, value);
								}
								else
								{
									object obj = t3;
									fieldInfo.SetValue(obj, value);
									t3 = (T)((object)obj);
								}
							}
						}
					}
					IL_991:;
				}
				if (doPostLoad)
				{
					DirectXmlToObject.TryDoPostLoad(t3);
				}
				return t3;
			}
			return result;
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x000F5450 File Offset: 0x000F3650
		private static Type ClassTypeOf<T>(XmlNode xmlRoot)
		{
			XmlAttribute xmlAttribute = xmlRoot.Attributes["Class"];
			if (xmlAttribute == null)
			{
				return typeof(T);
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlAttribute.Value, typeof(T).Namespace);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Could not find type named " + xmlAttribute.Value + " from node " + xmlRoot.OuterXml, false);
				return typeof(T);
			}
			return typeInAnyAssembly;
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x000F54D0 File Offset: 0x000F36D0
		private static void TryDoPostLoad(object obj)
		{
			DeepProfiler.Start("TryDoPostLoad");
			try
			{
				MethodInfo method = obj.GetType().GetMethod("PostLoad");
				if (method != null)
				{
					method.Invoke(obj, null);
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while executing PostLoad on ",
					obj.ToStringSafe<object>(),
					": ",
					ex
				}), false);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0001A5D5 File Offset: 0x000187D5
		private static object ListFromXmlReflection<T>(XmlNode listRootNode)
		{
			return DirectXmlToObject.ListFromXml<T>(listRootNode);
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000F5560 File Offset: 0x000F3760
		private static List<T> ListFromXml<T>(XmlNode listRootNode)
		{
			List<T> list = new List<T>();
			try
			{
				bool flag = typeof(Def).IsAssignableFrom(typeof(T));
				foreach (object obj in listRootNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (DirectXmlToObject.ValidateListNode(xmlNode, listRootNode, typeof(T)))
					{
						XmlAttribute xmlAttribute = xmlNode.Attributes["MayRequire"];
						if (flag)
						{
							DirectXmlCrossRefLoader.RegisterListWantsCrossRef<T>(list, xmlNode.InnerText, listRootNode.Name, (xmlAttribute != null) ? xmlAttribute.Value : null);
						}
						else
						{
							try
							{
								if (xmlAttribute == null || xmlAttribute.Value.NullOrEmpty() || ModsConfig.IsActive(xmlAttribute.Value))
								{
									list.Add(DirectXmlToObject.ObjectFromXml<T>(xmlNode, true));
								}
							}
							catch (Exception ex)
							{
								Log.Error(string.Concat(new object[]
								{
									"Exception loading list element from XML: ",
									ex,
									"\nXML:\n",
									listRootNode.OuterXml
								}), false);
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception loading list from XML: ",
					ex2,
					"\nXML:\n",
					listRootNode.OuterXml
				}), false);
			}
			return list;
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x0001A5DD File Offset: 0x000187DD
		private static object DictionaryFromXmlReflection<K, V>(XmlNode dictRootNode)
		{
			return DirectXmlToObject.DictionaryFromXml<K, V>(dictRootNode);
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x000F5700 File Offset: 0x000F3900
		private static Dictionary<K, V> DictionaryFromXml<K, V>(XmlNode dictRootNode)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			try
			{
				bool flag = typeof(Def).IsAssignableFrom(typeof(K));
				bool flag2 = typeof(Def).IsAssignableFrom(typeof(V));
				if (!flag && !flag2)
				{
					using (IEnumerator enumerator = dictRootNode.ChildNodes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							XmlNode xmlNode = (XmlNode)obj;
							if (DirectXmlToObject.ValidateListNode(xmlNode, dictRootNode, typeof(KeyValuePair<K, V>)))
							{
								K key = DirectXmlToObject.ObjectFromXml<K>(xmlNode["key"], true);
								V value = DirectXmlToObject.ObjectFromXml<V>(xmlNode["value"], true);
								dictionary.Add(key, value);
							}
						}
						goto IL_114;
					}
				}
				foreach (object obj2 in dictRootNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					if (DirectXmlToObject.ValidateListNode(xmlNode2, dictRootNode, typeof(KeyValuePair<K, V>)))
					{
						DirectXmlCrossRefLoader.RegisterDictionaryWantsCrossRef<K, V>(dictionary, xmlNode2, dictRootNode.Name);
					}
				}
				IL_114:;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Malformed dictionary XML. Node: ",
					dictRootNode.OuterXml,
					".\n\nException: ",
					ex
				}), false);
			}
			return dictionary;
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x0001A5E5 File Offset: 0x000187E5
		private static MethodInfo CustomDataLoadMethodOf(Type type)
		{
			return type.GetMethod("LoadDataFromXmlCustom", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x000F58A4 File Offset: 0x000F3AA4
		private static bool ValidateListNode(XmlNode listEntryNode, XmlNode listRootNode, Type listItemType)
		{
			if (listEntryNode is XmlComment)
			{
				return false;
			}
			if (listEntryNode is XmlText)
			{
				Log.Error("XML format error: Raw text found inside a list element. Did you mean to surround it with list item <li> tags? " + listRootNode.OuterXml, false);
				return false;
			}
			if (listEntryNode.Name != "li" && DirectXmlToObject.CustomDataLoadMethodOf(listItemType) == null)
			{
				Log.Error("XML format error: List item found with name that is not <li>, and which does not have a custom XML loader method, in " + listRootNode.OuterXml, false);
				return false;
			}
			return true;
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x000F5914 File Offset: 0x000F3B14
		private static FieldInfo GetFieldInfoForType(Type type, string token, XmlNode debugXmlNode)
		{
			Dictionary<string, FieldInfo> dictionary = DirectXmlToObject.fieldInfoLookup.TryGetValue(type, null);
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, FieldInfo>();
				DirectXmlToObject.fieldInfoLookup[type] = dictionary;
			}
			FieldInfo fieldInfo = dictionary.TryGetValue(token, null);
			if (fieldInfo == null && !dictionary.ContainsKey(token))
			{
				fieldInfo = DirectXmlToObject.SearchTypeHierarchy(type, token, BindingFlags.Default);
				if (fieldInfo == null)
				{
					fieldInfo = DirectXmlToObject.SearchTypeHierarchy(type, token, BindingFlags.IgnoreCase);
					if (fieldInfo != null && !type.HasAttribute<CaseInsensitiveXMLParsing>())
					{
						string text = string.Format("Attempt to use string {0} to refer to field {1} in type {2}; xml tags are now case-sensitive", token, fieldInfo.Name, type);
						if (debugXmlNode != null)
						{
							text = text + ". XML: " + debugXmlNode.OuterXml;
						}
						Log.Error(text, false);
					}
				}
				dictionary[token] = fieldInfo;
			}
			return fieldInfo;
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x000F59C4 File Offset: 0x000F3BC4
		private static FieldInfo SearchTypeHierarchy(Type type, string token, BindingFlags extraFlags)
		{
			FieldInfo field;
			for (;;)
			{
				field = type.GetField(token, extraFlags | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (!(field == null) || !(type.BaseType != typeof(object)))
				{
					break;
				}
				type = type.BaseType;
			}
			return field;
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x0001A5F4 File Offset: 0x000187F4
		public static string InnerTextWithReplacedNewlinesOrXML(XmlNode xmlNode)
		{
			if (xmlNode.ChildNodes.Count == 1 && xmlNode.FirstChild.NodeType == XmlNodeType.Text)
			{
				return xmlNode.InnerText.Replace("\\n", "\n");
			}
			return xmlNode.InnerXml;
		}

		// Token: 0x040014F4 RID: 5364
		public static Stack<Type> currentlyInstantiatingObjectOfType = new Stack<Type>();

		// Token: 0x040014F5 RID: 5365
		public const string DictionaryKeyName = "key";

		// Token: 0x040014F6 RID: 5366
		public const string DictionaryValueName = "value";

		// Token: 0x040014F7 RID: 5367
		public const string LoadDataFromXmlCustomMethodName = "LoadDataFromXmlCustom";

		// Token: 0x040014F8 RID: 5368
		public const string PostLoadMethodName = "PostLoad";

		// Token: 0x040014F9 RID: 5369
		public const string ObjectFromXmlMethodName = "ObjectFromXmlReflection";

		// Token: 0x040014FA RID: 5370
		public const string ListFromXmlMethodName = "ListFromXmlReflection";

		// Token: 0x040014FB RID: 5371
		public const string DictionaryFromXmlMethodName = "DictionaryFromXmlReflection";

		// Token: 0x040014FC RID: 5372
		private static Dictionary<Type, Func<XmlNode, object>> listFromXmlMethods = new Dictionary<Type, Func<XmlNode, object>>();

		// Token: 0x040014FD RID: 5373
		private static Dictionary<Type, Func<XmlNode, object>> dictionaryFromXmlMethods = new Dictionary<Type, Func<XmlNode, object>>();

		// Token: 0x040014FE RID: 5374
		private static readonly Type[] tmpOneTypeArray = new Type[1];

		// Token: 0x040014FF RID: 5375
		private static readonly Dictionary<Type, Func<XmlNode, bool, object>> objectFromXmlMethods = new Dictionary<Type, Func<XmlNode, bool, object>>();

		// Token: 0x04001500 RID: 5376
		private static Dictionary<DirectXmlToObject.FieldAliasCache, FieldInfo> fieldAliases = new Dictionary<DirectXmlToObject.FieldAliasCache, FieldInfo>(EqualityComparer<DirectXmlToObject.FieldAliasCache>.Default);

		// Token: 0x04001501 RID: 5377
		private static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoLookup = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x02000492 RID: 1170
		private struct FieldAliasCache : IEquatable<DirectXmlToObject.FieldAliasCache>
		{
			// Token: 0x06001D6B RID: 7531 RVA: 0x0001A62E File Offset: 0x0001882E
			public FieldAliasCache(Type type, string fieldName)
			{
				this.type = type;
				this.fieldName = fieldName.ToLower();
			}

			// Token: 0x06001D6C RID: 7532 RVA: 0x0001A643 File Offset: 0x00018843
			public bool Equals(DirectXmlToObject.FieldAliasCache other)
			{
				return this.type == other.type && string.Equals(this.fieldName, other.fieldName);
			}

			// Token: 0x04001502 RID: 5378
			public Type type;

			// Token: 0x04001503 RID: 5379
			public string fieldName;
		}
	}
}
