using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000331 RID: 817
	public static class Scribe_Collections
	{
		// Token: 0x06001738 RID: 5944 RVA: 0x00089A9C File Offset: 0x00087C9C
		public static void Look<T>(ref List<T> list, string label, LookMode lookMode = LookMode.Undefined, params object[] ctorArgs)
		{
			Scribe_Collections.Look<T>(ref list, false, label, lookMode, ctorArgs);
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x00089AA8 File Offset: 0x00087CA8
		public static void Look<T>(ref List<T> list, bool saveDestroyedThings, string label, LookMode lookMode = LookMode.Undefined, params object[] ctorArgs)
		{
			if (lookMode == LookMode.Undefined && !Scribe_Universal.TryResolveLookMode(typeof(T), out lookMode, false, false))
			{
				Log.Error("LookList call with a list of " + typeof(T) + " must have lookMode set explicitly.");
				return;
			}
			if (Scribe.EnterNode(label))
			{
				try
				{
					if (Scribe.mode == LoadSaveMode.Saving)
					{
						if (list == null)
						{
							Scribe.saver.WriteAttribute("IsNull", "True");
							return;
						}
						using (List<T>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								T t = enumerator.Current;
								if (lookMode == LookMode.Value)
								{
									T t2 = t;
									Scribe_Values.Look<T>(ref t2, "li", default(T), true);
								}
								else if (lookMode == LookMode.LocalTargetInfo)
								{
									LocalTargetInfo localTargetInfo = (LocalTargetInfo)((object)t);
									Scribe_TargetInfo.Look(ref localTargetInfo, saveDestroyedThings, "li");
								}
								else if (lookMode == LookMode.TargetInfo)
								{
									TargetInfo targetInfo = (TargetInfo)((object)t);
									Scribe_TargetInfo.Look(ref targetInfo, saveDestroyedThings, "li");
								}
								else if (lookMode == LookMode.GlobalTargetInfo)
								{
									GlobalTargetInfo globalTargetInfo = (GlobalTargetInfo)((object)t);
									Scribe_TargetInfo.Look(ref globalTargetInfo, saveDestroyedThings, "li");
								}
								else if (lookMode == LookMode.Def)
								{
									Def def = (Def)((object)t);
									Scribe_Defs.Look<Def>(ref def, "li");
								}
								else if (lookMode == LookMode.BodyPart)
								{
									BodyPartRecord bodyPartRecord = (BodyPartRecord)((object)t);
									Scribe_BodyParts.Look(ref bodyPartRecord, "li", null);
								}
								else if (lookMode == LookMode.Deep)
								{
									T t3 = t;
									Scribe_Deep.Look<T>(ref t3, saveDestroyedThings, "li", ctorArgs);
								}
								else if (lookMode == LookMode.Reference)
								{
									ILoadReferenceable loadReferenceable = (ILoadReferenceable)((object)t);
									Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, "li", saveDestroyedThings);
								}
							}
							return;
						}
					}
					if (Scribe.mode == LoadSaveMode.LoadingVars)
					{
						XmlNode curXmlParent = Scribe.loader.curXmlParent;
						XmlAttribute xmlAttribute = curXmlParent.Attributes["IsNull"];
						if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
						{
							if (lookMode == LookMode.Reference)
							{
								Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(null, null);
							}
							list = null;
						}
						else
						{
							if (lookMode == LookMode.Value)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj = enumerator2.Current;
										T item = ScribeExtractor.ValueFromNode<T>((XmlNode)obj, default(T));
										list.Add(item);
									}
									return;
								}
							}
							if (lookMode == LookMode.Deep)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj2 = enumerator2.Current;
										T item2 = ScribeExtractor.SaveableFromNode<T>((XmlNode)obj2, ctorArgs);
										list.Add(item2);
									}
									return;
								}
							}
							if (lookMode == LookMode.Def)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj3 = enumerator2.Current;
										T item3 = ScribeExtractor.DefFromNodeUnsafe<T>((XmlNode)obj3);
										list.Add(item3);
									}
									return;
								}
							}
							if (lookMode == LookMode.BodyPart)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num = 0;
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj4 = enumerator2.Current;
										T item4 = (T)((object)ScribeExtractor.BodyPartFromNode((XmlNode)obj4, num.ToString(), null));
										list.Add(item4);
										num++;
									}
									return;
								}
							}
							if (lookMode == LookMode.LocalTargetInfo)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num2 = 0;
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj5 = enumerator2.Current;
										T item5 = (T)((object)ScribeExtractor.LocalTargetInfoFromNode((XmlNode)obj5, num2.ToString(), LocalTargetInfo.Invalid));
										list.Add(item5);
										num2++;
									}
									return;
								}
							}
							if (lookMode == LookMode.TargetInfo)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num3 = 0;
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj6 = enumerator2.Current;
										T item6 = (T)((object)ScribeExtractor.TargetInfoFromNode((XmlNode)obj6, num3.ToString(), TargetInfo.Invalid));
										list.Add(item6);
										num3++;
									}
									return;
								}
							}
							if (lookMode == LookMode.GlobalTargetInfo)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num4 = 0;
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj7 = enumerator2.Current;
										T item7 = (T)((object)ScribeExtractor.GlobalTargetInfoFromNode((XmlNode)obj7, num4.ToString(), GlobalTargetInfo.Invalid));
										list.Add(item7);
										num4++;
									}
									return;
								}
							}
							if (lookMode == LookMode.Reference)
							{
								List<string> list2 = new List<string>(curXmlParent.ChildNodes.Count);
								foreach (object obj8 in curXmlParent.ChildNodes)
								{
									XmlNode xmlNode = (XmlNode)obj8;
									list2.Add(xmlNode.InnerText);
								}
								Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(list2, "");
							}
						}
					}
					else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
					{
						if (lookMode == LookMode.Reference)
						{
							list = Scribe.loader.crossRefs.TakeResolvedRefList<T>("");
						}
						else if (lookMode == LookMode.LocalTargetInfo)
						{
							if (list != null)
							{
								for (int i = 0; i < list.Count; i++)
								{
									list[i] = (T)((object)ScribeExtractor.ResolveLocalTargetInfo((LocalTargetInfo)((object)list[i]), i.ToString()));
								}
							}
						}
						else if (lookMode == LookMode.TargetInfo)
						{
							if (list != null)
							{
								for (int j = 0; j < list.Count; j++)
								{
									list[j] = (T)((object)ScribeExtractor.ResolveTargetInfo((TargetInfo)((object)list[j]), j.ToString()));
								}
							}
						}
						else if (lookMode == LookMode.GlobalTargetInfo && list != null)
						{
							for (int k = 0; k < list.Count; k++)
							{
								list[k] = (T)((object)ScribeExtractor.ResolveGlobalTargetInfo((GlobalTargetInfo)((object)list[k]), k.ToString()));
							}
						}
					}
					return;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (lookMode == LookMode.Reference)
				{
					Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(null, label);
				}
				list = null;
			}
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x0008A2D8 File Offset: 0x000884D8
		public static void Look<K, V>(ref Dictionary<K, V> dict, string label, LookMode keyLookMode = LookMode.Undefined, LookMode valueLookMode = LookMode.Undefined)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				bool flag = keyLookMode == LookMode.Reference;
				bool flag2 = valueLookMode == LookMode.Reference;
				if (flag != flag2)
				{
					Log.Error("You need to provide working lists for the keys and values in order to be able to load such dictionary. label=" + label);
				}
			}
			List<K> list = null;
			List<V> list2 = null;
			Scribe_Collections.Look<K, V>(ref dict, label, keyLookMode, valueLookMode, ref list, ref list2);
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x0008A31C File Offset: 0x0008851C
		public static void Look<K, V>(ref Dictionary<K, V> dict, string label, LookMode keyLookMode, LookMode valueLookMode, ref List<K> keysWorkingList, ref List<V> valuesWorkingList)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					if (Scribe.mode == LoadSaveMode.Saving && dict == null)
					{
						Scribe.saver.WriteAttribute("IsNull", "True");
						return;
					}
					if (Scribe.mode == LoadSaveMode.LoadingVars)
					{
						XmlAttribute xmlAttribute = Scribe.loader.curXmlParent.Attributes["IsNull"];
						if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
						{
							dict = null;
						}
						else
						{
							dict = new Dictionary<K, V>();
						}
					}
					if (Scribe.mode == LoadSaveMode.Saving || Scribe.mode == LoadSaveMode.LoadingVars)
					{
						keysWorkingList = new List<K>();
						valuesWorkingList = new List<V>();
						if (Scribe.mode == LoadSaveMode.Saving && dict != null)
						{
							foreach (KeyValuePair<K, V> keyValuePair in dict)
							{
								keysWorkingList.Add(keyValuePair.Key);
								valuesWorkingList.Add(keyValuePair.Value);
							}
						}
					}
					if (Scribe.mode == LoadSaveMode.Saving || dict != null)
					{
						Scribe_Collections.Look<K>(ref keysWorkingList, "keys", keyLookMode, Array.Empty<object>());
						Scribe_Collections.Look<V>(ref valuesWorkingList, "values", valueLookMode, Array.Empty<object>());
					}
					if (Scribe.mode == LoadSaveMode.Saving)
					{
						if (keysWorkingList != null)
						{
							keysWorkingList.Clear();
							keysWorkingList = null;
						}
						if (valuesWorkingList != null)
						{
							valuesWorkingList.Clear();
							valuesWorkingList = null;
						}
					}
					bool flag = keyLookMode == LookMode.Reference || valueLookMode == LookMode.Reference;
					if (((flag && Scribe.mode == LoadSaveMode.ResolvingCrossRefs) || (!flag && Scribe.mode == LoadSaveMode.LoadingVars)) && dict != null)
					{
						if (keysWorkingList == null)
						{
							Log.Error("Cannot fill dictionary because there are no keys. label=" + label);
						}
						else if (valuesWorkingList == null)
						{
							Log.Error("Cannot fill dictionary because there are no values. label=" + label);
						}
						else
						{
							if (keysWorkingList.Count != valuesWorkingList.Count)
							{
								Log.Error(string.Concat(new object[]
								{
									"Keys count does not match the values count while loading a dictionary (maybe keys and values were resolved during different passes?). Some elements will be skipped. keys=",
									keysWorkingList.Count,
									", values=",
									valuesWorkingList.Count,
									", label=",
									label
								}));
							}
							int num = Math.Min(keysWorkingList.Count, valuesWorkingList.Count);
							for (int i = 0; i < num; i++)
							{
								if (keysWorkingList[i] == null)
								{
									Log.Error(string.Concat(new object[]
									{
										"Null key while loading dictionary of ",
										typeof(K),
										" and ",
										typeof(V),
										". label=",
										label
									}));
								}
								else
								{
									try
									{
										dict.Add(keysWorkingList[i], valuesWorkingList[i]);
									}
									catch (OutOfMemoryException)
									{
										throw;
									}
									catch (Exception ex)
									{
										Log.Error(string.Concat(new object[]
										{
											"Exception in LookDictionary(label=",
											label,
											"): ",
											ex
										}));
									}
								}
							}
						}
					}
					if (Scribe.mode == LoadSaveMode.PostLoadInit)
					{
						if (keysWorkingList != null)
						{
							keysWorkingList.Clear();
							keysWorkingList = null;
						}
						if (valuesWorkingList != null)
						{
							valuesWorkingList.Clear();
							valuesWorkingList = null;
						}
					}
					return;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				dict = null;
			}
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x0008A6A8 File Offset: 0x000888A8
		public static void Look<T>(ref HashSet<T> valueHashSet, string label, LookMode lookMode = LookMode.Undefined)
		{
			Scribe_Collections.Look<T>(ref valueHashSet, false, label, lookMode);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x0008A6B4 File Offset: 0x000888B4
		public static void Look<T>(ref HashSet<T> valueHashSet, bool saveDestroyedThings, string label, LookMode lookMode = LookMode.Undefined)
		{
			List<T> list = null;
			if (Scribe.mode == LoadSaveMode.Saving && valueHashSet != null)
			{
				list = new List<T>();
				foreach (T item in valueHashSet)
				{
					list.Add(item);
				}
			}
			Scribe_Collections.Look<T>(ref list, saveDestroyedThings, label, lookMode, Array.Empty<object>());
			if ((lookMode == LookMode.Reference && Scribe.mode == LoadSaveMode.ResolvingCrossRefs) || (lookMode != LookMode.Reference && Scribe.mode == LoadSaveMode.LoadingVars))
			{
				if (list == null)
				{
					valueHashSet = null;
					return;
				}
				valueHashSet = new HashSet<T>();
				for (int i = 0; i < list.Count; i++)
				{
					valueHashSet.Add(list[i]);
				}
			}
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x0008A76C File Offset: 0x0008896C
		public static void Look<T>(ref Stack<T> valueStack, string label, LookMode lookMode = LookMode.Undefined)
		{
			List<T> list = null;
			if (Scribe.mode == LoadSaveMode.Saving && valueStack != null)
			{
				list = new List<T>();
				foreach (T item in valueStack)
				{
					list.Add(item);
				}
			}
			Scribe_Collections.Look<T>(ref list, label, lookMode, Array.Empty<object>());
			if ((lookMode == LookMode.Reference && Scribe.mode == LoadSaveMode.ResolvingCrossRefs) || (lookMode != LookMode.Reference && Scribe.mode == LoadSaveMode.LoadingVars))
			{
				if (list == null)
				{
					valueStack = null;
					return;
				}
				valueStack = new Stack<T>();
				for (int i = 0; i < list.Count; i++)
				{
					valueStack.Push(list[i]);
				}
			}
		}
	}
}
