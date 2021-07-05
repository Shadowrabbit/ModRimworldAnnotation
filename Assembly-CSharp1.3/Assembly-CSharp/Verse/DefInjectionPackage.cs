using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using RimWorld;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200014F RID: 335
	public class DefInjectionPackage
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x0002F066 File Offset: 0x0002D266
		public DefInjectionPackage(Type defType)
		{
			this.defType = defType;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002F096 File Offset: 0x0002D296
		private string ProcessedPath(string path)
		{
			if (path == null)
			{
				path = "";
			}
			if (!path.Contains('[') && !path.Contains(']'))
			{
				return path;
			}
			return path.Replace("]", "").Replace('[', '.');
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002F0D1 File Offset: 0x0002D2D1
		private string ProcessedTranslation(string rawTranslation)
		{
			return rawTranslation.Replace("\\n", "\n");
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002F0E4 File Offset: 0x0002D2E4
		public void AddDataFromFile(VirtualFile file, out bool xmlParseError)
		{
			xmlParseError = false;
			try
			{
				foreach (XElement xelement in file.LoadAsXDocument().Root.Elements())
				{
					if (xelement.Name == "rep")
					{
						string key = this.ProcessedPath(xelement.Elements("path").First<XElement>().Value);
						string translation = this.ProcessedTranslation(xelement.Elements("trans").First<XElement>().Value);
						this.TryAddInjection(file, key, translation);
						this.usedOldRepSyntax = true;
					}
					else
					{
						string text = this.ProcessedPath(xelement.Name.ToString());
						if (text.EndsWith(".slateRef"))
						{
							if (xelement.HasElements)
							{
								this.TryAddInjection(file, text, xelement.GetInnerXml());
							}
							else
							{
								string translation2 = this.ProcessedTranslation(xelement.Value);
								this.TryAddInjection(file, text, translation2);
							}
						}
						else if (xelement.HasElements)
						{
							List<string> list = new List<string>();
							List<Pair<int, string>> list2 = null;
							bool flag = false;
							foreach (XNode xnode in xelement.DescendantNodes())
							{
								XElement xelement2 = xnode as XElement;
								if (xelement2 != null)
								{
									if (xelement2.Name == "li")
									{
										list.Add(this.ProcessedTranslation(xelement2.Value));
									}
									else if (!flag)
									{
										this.loadErrors.Add(text + " has elements which are not 'li' (" + file.Name + ")");
										flag = true;
									}
								}
								XComment xcomment = xnode as XComment;
								if (xcomment != null)
								{
									if (list2 == null)
									{
										list2 = new List<Pair<int, string>>();
									}
									list2.Add(new Pair<int, string>(list.Count, xcomment.Value));
								}
							}
							this.TryAddFullListInjection(file, text, list, list2);
						}
						else
						{
							string translation3 = this.ProcessedTranslation(xelement.Value);
							this.TryAddInjection(file, text, translation3);
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading translation data from file ",
					file.Name,
					": ",
					ex
				}));
				xmlParseError = true;
			}
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002F388 File Offset: 0x0002D588
		private void TryAddInjection(VirtualFile file, string key, string translation)
		{
			string text = key;
			key = this.BackCompatibleKey(key);
			if (this.CheckErrors(file, key, text, false))
			{
				return;
			}
			DefInjectionPackage.DefInjection defInjection = new DefInjectionPackage.DefInjection();
			if (translation == "TODO")
			{
				defInjection.isPlaceholder = true;
				translation = "";
			}
			defInjection.path = key;
			defInjection.injection = translation;
			defInjection.fileSource = file.Name;
			defInjection.nonBackCompatiblePath = text;
			this.injections.SetOrAdd(key, defInjection);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002F3FC File Offset: 0x0002D5FC
		private void TryAddFullListInjection(VirtualFile file, string key, List<string> translation, List<Pair<int, string>> comments)
		{
			string text = key;
			key = this.BackCompatibleKey(key);
			if (this.CheckErrors(file, key, text, true))
			{
				return;
			}
			if (translation == null)
			{
				translation = new List<string>();
			}
			DefInjectionPackage.DefInjection defInjection = new DefInjectionPackage.DefInjection();
			defInjection.path = key;
			defInjection.fullListInjection = translation;
			defInjection.fullListInjectionComments = comments;
			defInjection.fileSource = file.Name;
			defInjection.nonBackCompatiblePath = text;
			this.injections.Add(key, defInjection);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002F468 File Offset: 0x0002D668
		private string BackCompatibleKey(string key)
		{
			string[] array = key.Split(new char[]
			{
				'.'
			});
			if (array.Any<string>())
			{
				array[0] = BackCompatibility.BackCompatibleDefName(this.defType, array[0], true, null);
			}
			key = string.Join(".", array);
			if (this.defType == typeof(ConceptDef) && key.Contains(".helpTexts.0"))
			{
				key = key.Replace(".helpTexts.0", ".helpText");
			}
			return key;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002F4E8 File Offset: 0x0002D6E8
		private bool CheckErrors(VirtualFile file, string key, string nonBackCompatibleKey, bool replacingFullList)
		{
			if (!key.Contains('.'))
			{
				this.loadErrors.Add(string.Concat(new string[]
				{
					"Error loading DefInjection from file ",
					file.Name,
					": Key lacks a dot: ",
					key,
					(key == nonBackCompatibleKey) ? "" : (" (auto-renamed from " + nonBackCompatibleKey + ")"),
					" (",
					file.Name,
					")"
				}));
				return true;
			}
			DefInjectionPackage.DefInjection defInjection;
			if (this.injections.TryGetValue(key, out defInjection))
			{
				string text;
				if (key != nonBackCompatibleKey)
				{
					text = " (auto-renamed from " + nonBackCompatibleKey + ")";
				}
				else if (defInjection.path != defInjection.nonBackCompatiblePath)
				{
					text = string.Concat(new string[]
					{
						" (",
						defInjection.nonBackCompatiblePath,
						" was auto-renamed to ",
						defInjection.path,
						")"
					});
				}
				else
				{
					text = "";
				}
				this.loadErrors.Add(string.Concat(new string[]
				{
					"Duplicate def-injected translation key: ",
					key,
					text,
					" (",
					file.Name,
					")"
				}));
			}
			bool flag = false;
			if (replacingFullList)
			{
				if (this.injections.Any((KeyValuePair<string, DefInjectionPackage.DefInjection> x) => !x.Value.IsFullListInjection && x.Key.StartsWith(key + ".")))
				{
					flag = true;
				}
			}
			else if (key.Contains('.') && char.IsNumber(key[key.Length - 1]))
			{
				string key2 = key.Substring(0, key.LastIndexOf('.'));
				if (this.injections.ContainsKey(key2) && this.injections[key2].IsFullListInjection)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.loadErrors.Add(string.Concat(new string[]
				{
					"Replacing the whole list and individual elements at the same time doesn't make sense. Either replace the whole list or translate individual elements by using their indexes. key=",
					key,
					(key == nonBackCompatibleKey) ? "" : (" (auto-renamed from " + nonBackCompatibleKey + ")"),
					" (",
					file.Name,
					")"
				}));
				return true;
			}
			return false;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0002F75C File Offset: 0x0002D95C
		public void InjectIntoDefs(bool errorOnDefNotFound)
		{
			this.loadSyntaxSuggestions.Clear();
			this.loadErrors.Clear();
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in this.injections)
			{
				if (!keyValuePair.Value.injected)
				{
					string normalizedPath;
					string suggestedPath;
					if (keyValuePair.Value.IsFullListInjection)
					{
						keyValuePair.Value.injected = this.SetDefFieldAtPath(this.defType, keyValuePair.Key, keyValuePair.Value.fullListInjection, typeof(List<string>), errorOnDefNotFound, keyValuePair.Value.fileSource, keyValuePair.Value.isPlaceholder, out normalizedPath, out suggestedPath, out keyValuePair.Value.replacedString, out keyValuePair.Value.replacedList);
					}
					else
					{
						keyValuePair.Value.injected = this.SetDefFieldAtPath(this.defType, keyValuePair.Key, keyValuePair.Value.injection, typeof(string), errorOnDefNotFound, keyValuePair.Value.fileSource, keyValuePair.Value.isPlaceholder, out normalizedPath, out suggestedPath, out keyValuePair.Value.replacedString, out keyValuePair.Value.replacedList);
					}
					keyValuePair.Value.normalizedPath = normalizedPath;
					keyValuePair.Value.suggestedPath = suggestedPath;
				}
			}
			GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.defType, "ClearCachedData");
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0002F8FC File Offset: 0x0002DAFC
		private bool SetDefFieldAtPath(Type defType, string path, object value, Type ensureFieldType, bool errorOnDefNotFound, string fileSource, bool isPlaceholder, out string normalizedPath, out string suggestedPath, out string replacedString, out IEnumerable<string> replacedStringsList)
		{
			replacedString = null;
			replacedStringsList = null;
			string b = path;
			string text = path;
			bool flag = TKeySystem.TryGetNormalizedPath(path, out normalizedPath);
			if (flag)
			{
				text = text + " (" + normalizedPath + ")";
				suggestedPath = path;
				path = normalizedPath;
			}
			else
			{
				normalizedPath = path;
				suggestedPath = path;
			}
			string text2 = path.Split(new char[]
			{
				'.'
			})[0];
			text2 = BackCompatibility.BackCompatibleDefName(defType, text2, true, null);
			if (GenDefDatabase.GetDefSilentFail(defType, text2, false) == null)
			{
				if (errorOnDefNotFound)
				{
					this.loadErrors.Add(string.Concat(new object[]
					{
						"Found no ",
						defType,
						" named ",
						text2,
						" to match ",
						text,
						" (",
						fileSource,
						")"
					}));
				}
				return false;
			}
			bool flag2 = false;
			int num = 0;
			List<object> list = new List<object>();
			bool result2;
			try
			{
				List<string> list2 = path.Split(new char[]
				{
					'.'
				}).ToList<string>();
				object obj = GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamedSilentFail", new object[]
				{
					list2[0]
				});
				if (obj == null)
				{
					throw new InvalidOperationException("Def named " + list2[0] + " not found.");
				}
				num++;
				string text3;
				int num2;
				DefInjectionPathPartKind defInjectionPathPartKind;
				FieldInfo field;
				int num3;
				int num4;
				for (;;)
				{
					text3 = list2[num];
					list.Add(obj);
					num2 = -1;
					if (int.TryParse(text3, out num2))
					{
						defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
					}
					else if (this.GetFieldNamed(obj.GetType(), text3) != null)
					{
						defInjectionPathPartKind = DefInjectionPathPartKind.Field;
					}
					else if (obj.GetType().GetProperty("Count") != null)
					{
						if (text3.Contains('-'))
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.ListHandleWithIndex;
							string[] array = text3.Split(new char[]
							{
								'-'
							});
							text3 = array[0];
							num2 = ParseHelper.FromString<int>(array[1]);
						}
						else
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.ListHandle;
						}
					}
					else
					{
						defInjectionPathPartKind = DefInjectionPathPartKind.Field;
					}
					if (num == list2.Count - 1)
					{
						break;
					}
					if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
					{
						field = obj.GetType().GetField(text3, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						if (field == null)
						{
							goto Block_48;
						}
						if (field.HasAttribute<NoTranslateAttribute>())
						{
							goto Block_49;
						}
						if (field.HasAttribute<UnsavedAttribute>())
						{
							goto Block_50;
						}
						if (field.HasAttribute<TranslationCanChangeCountAttribute>())
						{
							flag2 = true;
						}
						if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
						{
							obj = field.GetValue(obj);
						}
						else
						{
							object value2 = field.GetValue(obj);
							PropertyInfo property = value2.GetType().GetProperty("Item");
							if (property == null)
							{
								goto Block_53;
							}
							num3 = (int)value2.GetType().GetProperty("Count").GetValue(value2, null);
							if (num2 < 0 || num2 >= num3)
							{
								goto IL_B40;
							}
							obj = property.GetValue(value2, new object[]
							{
								num2
							});
						}
					}
					else if (defInjectionPathPartKind == DefInjectionPathPartKind.ListIndex || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
					{
						object obj2 = obj;
						PropertyInfo property2 = obj2.GetType().GetProperty("Item");
						if (property2 == null)
						{
							goto Block_57;
						}
						bool flag3;
						if (defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
						{
							num2 = TranslationHandleUtility.GetElementIndexByHandle(obj2, text3, num2);
							defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
							flag3 = true;
						}
						else
						{
							flag3 = false;
						}
						num4 = (int)obj2.GetType().GetProperty("Count").GetValue(obj2, null);
						if (num2 < 0 || num2 >= num4)
						{
							goto IL_C04;
						}
						obj = property2.GetValue(obj2, new object[]
						{
							num2
						});
						if (flag3)
						{
							string[] array2 = normalizedPath.Split(new char[]
							{
								'.'
							});
							array2[num] = num2.ToString();
							normalizedPath = string.Join(".", array2);
						}
						else if (!flag)
						{
							string bestHandleWithIndexForListElement = TranslationHandleUtility.GetBestHandleWithIndexForListElement(obj2, obj);
							if (!bestHandleWithIndexForListElement.NullOrEmpty())
							{
								string[] array3 = suggestedPath.Split(new char[]
								{
									'.'
								});
								array3[num] = bestHandleWithIndexForListElement;
								suggestedPath = string.Join(".", array3);
							}
						}
					}
					else
					{
						this.loadErrors.Add(string.Concat(new object[]
						{
							"Can't enter node ",
							text3,
							" at path ",
							text,
							", element kind is ",
							defInjectionPathPartKind,
							". (",
							fileSource,
							")"
						}));
					}
					num++;
				}
				bool flag4 = false;
				foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in this.injections)
				{
					if (!(keyValuePair.Key == b) && keyValuePair.Value.injected && keyValuePair.Value.normalizedPath == normalizedPath)
					{
						string text4 = string.Concat(new string[]
						{
							"Duplicate def-injected translation key. Both ",
							keyValuePair.Value.path,
							" and ",
							text,
							" refer to the same field (",
							suggestedPath,
							")"
						});
						if (keyValuePair.Value.path != keyValuePair.Value.nonBackCompatiblePath)
						{
							text4 = string.Concat(new string[]
							{
								text4,
								" (",
								keyValuePair.Value.nonBackCompatiblePath,
								" was auto-renamed to ",
								keyValuePair.Value.path,
								")"
							});
						}
						text4 = text4 + " (" + keyValuePair.Value.fileSource + ")";
						this.loadErrors.Add(text4);
						flag4 = true;
						break;
					}
				}
				bool result = false;
				if (!flag4)
				{
					if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
					{
						FieldInfo fieldNamed = this.GetFieldNamed(obj.GetType(), text3);
						if (fieldNamed == null)
						{
							throw new InvalidOperationException(string.Concat(new object[]
							{
								"Field ",
								text3,
								" does not exist in type ",
								obj.GetType(),
								"."
							}));
						}
						if (fieldNamed.HasAttribute<NoTranslateAttribute>())
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Translated untranslatable field ",
								fieldNamed.Name,
								" of type ",
								fieldNamed.FieldType,
								" at path ",
								text,
								". Translating this field will break the game. (",
								fileSource,
								")"
							}));
						}
						else if (fieldNamed.HasAttribute<UnsavedAttribute>())
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Translated untranslatable field (UnsavedAttribute) ",
								fieldNamed.Name,
								" of type ",
								fieldNamed.FieldType,
								" at path ",
								text,
								". Translating this field will break the game. (",
								fileSource,
								")"
							}));
						}
						else if (!isPlaceholder && fieldNamed.FieldType != ensureFieldType)
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Translated non-",
								ensureFieldType,
								" field ",
								fieldNamed.Name,
								" of type ",
								fieldNamed.FieldType,
								" at path ",
								text,
								". Expected ",
								ensureFieldType,
								". (",
								fileSource,
								")"
							}));
						}
						else if (!isPlaceholder && ensureFieldType != typeof(string) && !fieldNamed.HasAttribute<TranslationCanChangeCountAttribute>())
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Tried to translate field ",
								fieldNamed.Name,
								" of type ",
								fieldNamed.FieldType,
								" at path ",
								text,
								", but this field doesn't have [TranslationCanChangeCount] attribute so it doesn't allow this type of translation. (",
								fileSource,
								")"
							}));
						}
						else if (!isPlaceholder)
						{
							if (ensureFieldType == typeof(string))
							{
								replacedString = (string)fieldNamed.GetValue(obj);
							}
							else
							{
								replacedStringsList = (fieldNamed.GetValue(obj) as IEnumerable<string>);
							}
							fieldNamed.SetValue(obj, value);
							result = true;
						}
					}
					else if (defInjectionPathPartKind == DefInjectionPathPartKind.ListIndex || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
					{
						object obj3 = obj;
						if (obj3 == null)
						{
							throw new InvalidOperationException("Tried to use index on null list at " + text);
						}
						Type type = obj3.GetType();
						PropertyInfo property3 = type.GetProperty("Count");
						if (property3 == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Count' property).");
						}
						if (defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
						{
							num2 = TranslationHandleUtility.GetElementIndexByHandle(obj3, text3, num2);
							defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
						}
						int num5 = (int)property3.GetValue(obj3, null);
						if (num2 >= num5)
						{
							throw new InvalidOperationException(string.Concat(new object[]
							{
								"Trying to translate ",
								defType,
								".",
								text,
								" at index ",
								num2,
								" but the list only has ",
								num5,
								" entries (so max index is ",
								(num5 - 1).ToString(),
								")."
							}));
						}
						PropertyInfo property4 = type.GetProperty("Item");
						if (property4 == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
						}
						Type propertyType = property4.PropertyType;
						if (!isPlaceholder && propertyType != ensureFieldType)
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Translated non-",
								ensureFieldType,
								" list item of type ",
								propertyType,
								" at path ",
								text,
								". Expected ",
								ensureFieldType,
								". (",
								fileSource,
								")"
							}));
						}
						else if (!isPlaceholder && ensureFieldType != typeof(string) && !flag2)
						{
							this.loadErrors.Add(string.Concat(new object[]
							{
								"Tried to translate field of type ",
								propertyType,
								" at path ",
								text,
								", but this field doesn't have [TranslationCanChangeCount] attribute so it doesn't allow this type of translation. (",
								fileSource,
								")"
							}));
						}
						else if (num2 < 0 || num2 >= (int)type.GetProperty("Count").GetValue(obj3, null))
						{
							this.loadErrors.Add("Index out of bounds (max index is " + ((int)type.GetProperty("Count").GetValue(obj3, null) - 1) + ")");
						}
						else if (!isPlaceholder)
						{
							replacedString = (string)property4.GetValue(obj3, new object[]
							{
								num2
							});
							property4.SetValue(obj3, value, new object[]
							{
								num2
							});
							result = true;
						}
					}
					else
					{
						this.loadErrors.Add(string.Concat(new object[]
						{
							"Translated ",
							text3,
							" at path ",
							text,
							" but it's not a field, it's ",
							defInjectionPathPartKind,
							". (",
							fileSource,
							")"
						}));
					}
				}
				for (int i = list.Count - 1; i > 0; i--)
				{
					if (list[i].GetType().IsValueType && !list[i].GetType().IsPrimitive)
					{
						FieldInfo fieldNamed2 = this.GetFieldNamed(list[i - 1].GetType(), list2[i]);
						if (fieldNamed2 != null)
						{
							fieldNamed2.SetValue(list[i - 1], list[i]);
						}
					}
				}
				string text5;
				if (flag)
				{
					path = suggestedPath;
				}
				else if (TKeySystem.TrySuggestTKeyPath(path, out text5, null))
				{
					suggestedPath = text5;
				}
				if (path != suggestedPath)
				{
					IList<string> list3 = value as IList<string>;
					string text6;
					if (list3 != null)
					{
						text6 = list3.ToStringSafeEnumerable();
					}
					else
					{
						text6 = value.ToString();
					}
					this.loadSyntaxSuggestions.Add(string.Concat(new string[]
					{
						"Consider using ",
						suggestedPath,
						" instead of ",
						text,
						" for translation '",
						text6,
						"' (",
						fileSource,
						")"
					}));
				}
				return result;
				Block_48:
				throw new InvalidOperationException("Field or TKey " + text3 + " does not exist.");
				Block_49:
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Translated untranslatable field ",
					field.Name,
					" of type ",
					field.FieldType,
					" at path ",
					text,
					". Translating this field will break the game."
				}));
				Block_50:
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Translated untranslatable field ([Unsaved] attribute) ",
					field.Name,
					" of type ",
					field.FieldType,
					" at path ",
					text,
					". Translating this field will break the game."
				}));
				Block_53:
				throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
				IL_B40:
				throw new InvalidOperationException("Index out of bounds (max index is " + (num3 - 1) + ")");
				Block_57:
				throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
				IL_C04:
				throw new InvalidOperationException("Index out of bounds (max index is " + (num4 - 1) + ")");
			}
			catch (Exception ex)
			{
				string text7 = string.Concat(new object[]
				{
					"Couldn't inject ",
					text,
					" into ",
					defType,
					" (",
					fileSource,
					"): ",
					ex.Message
				});
				if (ex.InnerException != null)
				{
					text7 = text7 + " -> " + ex.InnerException.Message;
				}
				this.loadErrors.Add(text7);
				result2 = false;
			}
			return result2;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x000306DC File Offset: 0x0002E8DC
		private FieldInfo GetFieldNamed(Type type, string name)
		{
			FieldInfo field = type.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				FieldInfo[] fields = type.GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					object[] customAttributes = fields[i].GetCustomAttributes(typeof(LoadAliasAttribute), false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						for (int j = 0; j < customAttributes.Length; j++)
						{
							if (((LoadAliasAttribute)customAttributes[j]).alias == name)
							{
								return fields[i];
							}
						}
					}
				}
			}
			return field;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0003075C File Offset: 0x0002E95C
		public List<string> MissingInjections(List<string> outUnnecessaryDefInjections)
		{
			List<string> missingInjections = new List<string>();
			Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath = new Dictionary<string, DefInjectionPackage.DefInjection>();
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in this.injections)
			{
				if (!injectionsByNormalizedPath.ContainsKey(keyValuePair.Value.normalizedPath))
				{
					injectionsByNormalizedPath.Add(keyValuePair.Value.normalizedPath, keyValuePair.Value);
				}
			}
			DefInjectionUtility.ForEachPossibleDefInjection(this.defType, delegate(string suggestedPath, string normalizedPath, bool isCollection, string str, IEnumerable<string> collection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
			{
				DefInjectionPackage.DefInjection defInjection2;
				if (!isCollection)
				{
					bool flag = false;
					string text = null;
					DefInjectionPackage.DefInjection defInjection;
					if (injectionsByNormalizedPath.TryGetValue(normalizedPath, out defInjection) && !defInjection.IsFullListInjection)
					{
						if (!translationAllowed)
						{
							outUnnecessaryDefInjections.Add(defInjection.path + " '" + defInjection.injection.Replace("\n", "\\n") + "'");
						}
						else if (defInjection.isPlaceholder)
						{
							flag = true;
							text = defInjection.fileSource;
						}
					}
					else
					{
						flag = true;
					}
					if (flag && translationAllowed && DefInjectionUtility.ShouldCheckMissingInjection(str, fieldInfo, def))
					{
						missingInjections.Add(string.Concat(new string[]
						{
							suggestedPath,
							" '",
							str.Replace("\n", "\\n"),
							"'",
							text.NullOrEmpty() ? "" : (" (placeholder exists in " + text + ")")
						}));
						return;
					}
				}
				else if (injectionsByNormalizedPath.TryGetValue(normalizedPath, out defInjection2) && defInjection2.IsFullListInjection)
				{
					if (!translationAllowed || !fullListTranslationAllowed)
					{
						outUnnecessaryDefInjections.Add(defInjection2.path + " '" + defInjection2.fullListInjection.ToStringSafeEnumerable().Replace("\n", "\\n") + "'");
						return;
					}
					if (defInjection2.isPlaceholder && translationAllowed && !def.generated)
					{
						missingInjections.Add(suggestedPath + (defInjection2.fileSource.NullOrEmpty() ? "" : (" (placeholder exists in " + defInjection2.fileSource + ")")));
						return;
					}
				}
				else
				{
					int num = 0;
					foreach (string text2 in collection)
					{
						string key = normalizedPath + "." + num;
						string text3 = suggestedPath + "." + num;
						bool flag2 = false;
						string text4 = null;
						DefInjectionPackage.DefInjection defInjection3;
						if (injectionsByNormalizedPath.TryGetValue(key, out defInjection3) && !defInjection3.IsFullListInjection)
						{
							if (!translationAllowed)
							{
								outUnnecessaryDefInjections.Add(defInjection3.path + " '" + defInjection3.injection.Replace("\n", "\\n") + "'");
							}
							else if (defInjection3.isPlaceholder)
							{
								flag2 = true;
								text4 = defInjection3.fileSource;
							}
						}
						else
						{
							flag2 = true;
						}
						if (flag2 && translationAllowed && DefInjectionUtility.ShouldCheckMissingInjection(text2, fieldInfo, def))
						{
							DefInjectionPackage.DefInjection defInjection4;
							if (text4.NullOrEmpty() && injectionsByNormalizedPath.TryGetValue(normalizedPath, out defInjection4) && defInjection4.isPlaceholder)
							{
								text4 = defInjection4.fileSource;
							}
							missingInjections.Add(string.Concat(new string[]
							{
								text3,
								" '",
								text2.Replace("\n", "\\n"),
								"'",
								fullListTranslationAllowed ? " (hint: this list allows full-list translation by using <li> nodes)" : "",
								text4.NullOrEmpty() ? "" : (" (placeholder exists in " + text4 + ")")
							}));
						}
						num++;
					}
				}
			}, null);
			return missingInjections;
		}

		// Token: 0x0400085B RID: 2139
		public Type defType;

		// Token: 0x0400085C RID: 2140
		public Dictionary<string, DefInjectionPackage.DefInjection> injections = new Dictionary<string, DefInjectionPackage.DefInjection>();

		// Token: 0x0400085D RID: 2141
		public List<string> loadErrors = new List<string>();

		// Token: 0x0400085E RID: 2142
		public List<string> loadSyntaxSuggestions = new List<string>();

		// Token: 0x0400085F RID: 2143
		public bool usedOldRepSyntax;

		// Token: 0x04000860 RID: 2144
		public const BindingFlags FieldBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x04000861 RID: 2145
		public const string RepNodeName = "rep";

		// Token: 0x02001929 RID: 6441
		public class DefInjection
		{
			// Token: 0x1700190C RID: 6412
			// (get) Token: 0x06009766 RID: 38758 RVA: 0x0035CB18 File Offset: 0x0035AD18
			public bool IsFullListInjection
			{
				get
				{
					return this.fullListInjection != null;
				}
			}

			// Token: 0x1700190D RID: 6413
			// (get) Token: 0x06009767 RID: 38759 RVA: 0x0035CB23 File Offset: 0x0035AD23
			public string DefName
			{
				get
				{
					if (!this.path.NullOrEmpty())
					{
						return this.path.Split(new char[]
						{
							'.'
						})[0];
					}
					return "";
				}
			}

			// Token: 0x06009768 RID: 38760 RVA: 0x0035CB50 File Offset: 0x0035AD50
			public bool ModifiesDefFromModOrNullCore(ModMetaData mod, Type defType)
			{
				Def defSilentFail = GenDefDatabase.GetDefSilentFail(defType, this.DefName, true);
				if (defSilentFail == null)
				{
					return mod.IsCoreMod;
				}
				if (mod == null)
				{
					return defSilentFail.modContentPack == null;
				}
				return defSilentFail.modContentPack != null && defSilentFail.modContentPack.FolderName == mod.FolderName;
			}

			// Token: 0x0400609E RID: 24734
			public string path;

			// Token: 0x0400609F RID: 24735
			public string normalizedPath;

			// Token: 0x040060A0 RID: 24736
			public string nonBackCompatiblePath;

			// Token: 0x040060A1 RID: 24737
			public string suggestedPath;

			// Token: 0x040060A2 RID: 24738
			public string injection;

			// Token: 0x040060A3 RID: 24739
			public List<string> fullListInjection;

			// Token: 0x040060A4 RID: 24740
			public List<Pair<int, string>> fullListInjectionComments;

			// Token: 0x040060A5 RID: 24741
			public string fileSource;

			// Token: 0x040060A6 RID: 24742
			public bool injected;

			// Token: 0x040060A7 RID: 24743
			public string replacedString;

			// Token: 0x040060A8 RID: 24744
			public IEnumerable<string> replacedList;

			// Token: 0x040060A9 RID: 24745
			public bool isPlaceholder;
		}
	}
}
