using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using RimWorld;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000228 RID: 552
	public static class TranslationFilesCleaner
	{
		// Token: 0x06000E21 RID: 3617 RVA: 0x000B0B20 File Offset: 0x000AED20
		public static void CleanupTranslationFiles()
		{
			LoadedLanguage curLang = LanguageDatabase.activeLanguage;
			LoadedLanguage english = LanguageDatabase.defaultLanguage;
			if (curLang == english)
			{
				return;
			}
			IEnumerable<ModMetaData> activeModsInLoadOrder = ModsConfig.ActiveModsInLoadOrder;
			if (activeModsInLoadOrder.Any((ModMetaData x) => x.IsCoreMod))
			{
				if (!activeModsInLoadOrder.Any((ModMetaData x) => !x.Official))
				{
					if (LanguageDatabase.activeLanguage.AllDirectories.Any((Tuple<VirtualDirectory, ModContentPack, string> x) => x.Item1 is TarDirectory))
					{
						Messages.Message("MessageUnpackBeforeCleaningTranslationFiles".Translate(), MessageTypeDefOf.RejectInput, false);
						return;
					}
					LongEventHandler.QueueLongEvent(delegate()
					{
						if (curLang.anyKeyedReplacementsXmlParseError || curLang.anyDefInjectionsXmlParseError)
						{
							string value = curLang.lastKeyedReplacementsXmlParseErrorInFile ?? curLang.lastDefInjectionsXmlParseErrorInFile;
							Messages.Message("MessageCantCleanupTranslationFilesBeucaseOfXmlError".Translate(value), MessageTypeDefOf.RejectInput, false);
							return;
						}
						english.LoadData();
						curLang.LoadData();
						Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("ConfirmCleanupTranslationFiles".Translate(curLang.FriendlyNameNative), delegate
						{
							LongEventHandler.QueueLongEvent(new Action(TranslationFilesCleaner.DoCleanupTranslationFiles), "CleaningTranslationFiles".Translate(), true, null, true);
						}, true, null);
						dialog_MessageBox.buttonAText = "ConfirmCleanupTranslationFiles_Confirm".Translate();
						Find.WindowStack.Add(dialog_MessageBox);
					}, null, false, null, true);
					return;
				}
			}
			Messages.Message("MessageDisableModsBeforeCleaningTranslationFiles".Translate(), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x000B0C28 File Offset: 0x000AEE28
		private static void DoCleanupTranslationFiles()
		{
			if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
			{
				return;
			}
			try
			{
				try
				{
					TranslationFilesCleaner.CleanupKeyedTranslations();
				}
				catch (Exception arg)
				{
					Log.Error("Could not cleanup keyed translations: " + arg, false);
				}
				try
				{
					TranslationFilesCleaner.CleanupDefInjections();
				}
				catch (Exception arg2)
				{
					Log.Error("Could not cleanup def-injections: " + arg2, false);
				}
				try
				{
					TranslationFilesCleaner.CleanupBackstories();
				}
				catch (Exception arg3)
				{
					Log.Error("Could not cleanup backstories: " + arg3, false);
				}
				string value = string.Join("\n", (from x in ModsConfig.ActiveModsInLoadOrder
				select TranslationFilesCleaner.GetLanguageFolderPath(LanguageDatabase.activeLanguage, x.RootDir.FullName)).ToArray<string>());
				Messages.Message("MessageTranslationFilesCleanupDone".Translate(value), MessageTypeDefOf.TaskCompletion, false);
			}
			catch (Exception arg4)
			{
				Log.Error("Could not cleanup translation files: " + arg4, false);
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x000B0D3C File Offset: 0x000AEF3C
		private static void CleanupKeyedTranslations()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage english = LanguageDatabase.defaultLanguage;
			List<LoadedLanguage.KeyedReplacement> list = (from x in activeLanguage.keyedReplacements
			where !x.Value.isPlaceholder && !english.HaveTextForKey(x.Key, false)
			select x.Value).ToList<LoadedLanguage.KeyedReplacement>();
			HashSet<LoadedLanguage.KeyedReplacement> writtenUnusedKeyedTranslations = new HashSet<LoadedLanguage.KeyedReplacement>();
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				string languageFolderPath = TranslationFilesCleaner.GetLanguageFolderPath(activeLanguage, modMetaData.RootDir.FullName);
				string text = Path.Combine(languageFolderPath, "CodeLinked");
				string text2 = Path.Combine(languageFolderPath, "Keyed");
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				if (directoryInfo.Exists)
				{
					if (!Directory.Exists(text2))
					{
						Directory.Move(text, text2);
						Thread.Sleep(1000);
						directoryInfo = new DirectoryInfo(text2);
					}
				}
				else
				{
					directoryInfo = new DirectoryInfo(text2);
				}
				DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(TranslationFilesCleaner.GetLanguageFolderPath(english, modMetaData.RootDir.FullName), "Keyed"));
				if (!directoryInfo2.Exists)
				{
					if (modMetaData.IsCoreMod)
					{
						Log.Error("English keyed translations folder doesn't exist.", false);
					}
					if (!directoryInfo.Exists)
					{
						continue;
					}
				}
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
				{
					try
					{
						fileInfo.Delete();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not delete ",
							fileInfo.Name,
							": ",
							ex
						}), false);
					}
				}
				foreach (FileInfo fileInfo2 in directoryInfo2.GetFiles("*.xml", SearchOption.AllDirectories))
				{
					try
					{
						string path = new Uri(directoryInfo2.FullName + Path.DirectorySeparatorChar.ToString()).MakeRelativeUri(new Uri(fileInfo2.FullName)).ToString();
						string text3 = Path.Combine(directoryInfo.FullName, path);
						Directory.CreateDirectory(Path.GetDirectoryName(text3));
						fileInfo2.CopyTo(text3);
					}
					catch (Exception ex2)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not copy ",
							fileInfo2.Name,
							": ",
							ex2
						}), false);
					}
				}
				foreach (FileInfo fileInfo3 in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
				{
					try
					{
						XDocument xdocument = XDocument.Load(fileInfo3.FullName, LoadOptions.PreserveWhitespace);
						XElement xelement = xdocument.DescendantNodes().OfType<XElement>().FirstOrDefault<XElement>();
						if (xelement != null)
						{
							try
							{
								foreach (XNode xnode in xelement.DescendantNodes().ToArray<XNode>())
								{
									XElement xelement2 = xnode as XElement;
									if (xelement2 != null)
									{
										foreach (XNode xnode2 in xelement2.DescendantNodes().ToArray<XNode>())
										{
											try
											{
												XText xtext = xnode2 as XText;
												if (xtext != null && !xtext.Value.NullOrEmpty())
												{
													string comment = " EN: " + xtext.Value + " ";
													xnode.AddBeforeSelf(new XComment(TranslationFilesCleaner.SanitizeXComment(comment)));
													xnode.AddBeforeSelf(Environment.NewLine);
													xnode.AddBeforeSelf("  ");
												}
											}
											catch (Exception ex3)
											{
												Log.Error(string.Concat(new object[]
												{
													"Could not add comment node in ",
													fileInfo3.Name,
													": ",
													ex3
												}), false);
											}
											xnode2.Remove();
										}
										try
										{
											TaggedString taggedString;
											if (activeLanguage.TryGetTextFromKey(xelement2.Name.ToString(), out taggedString))
											{
												if (!taggedString.NullOrEmpty())
												{
													xelement2.Add(new XText(taggedString.Replace("\n", "\\n").RawText));
												}
											}
											else
											{
												xelement2.Add(new XText("TODO"));
											}
										}
										catch (Exception ex4)
										{
											Log.Error(string.Concat(new object[]
											{
												"Could not add existing translation or placeholder in ",
												fileInfo3.Name,
												": ",
												ex4
											}), false);
										}
									}
								}
								bool flag = false;
								foreach (LoadedLanguage.KeyedReplacement keyedReplacement in list)
								{
									if (new Uri(fileInfo3.FullName).Equals(new Uri(keyedReplacement.fileSourceFullPath)))
									{
										if (!flag)
										{
											xelement.Add("  ");
											xelement.Add(new XComment(" UNUSED "));
											xelement.Add(Environment.NewLine);
											flag = true;
										}
										XElement xelement3 = new XElement(keyedReplacement.key);
										if (keyedReplacement.isPlaceholder)
										{
											xelement3.Add(new XText("TODO"));
										}
										else if (!keyedReplacement.value.NullOrEmpty())
										{
											xelement3.Add(new XText(keyedReplacement.value.Replace("\n", "\\n")));
										}
										xelement.Add("  ");
										xelement.Add(xelement3);
										xelement.Add(Environment.NewLine);
										writtenUnusedKeyedTranslations.Add(keyedReplacement);
									}
								}
								if (flag)
								{
									xelement.Add(Environment.NewLine);
								}
							}
							finally
							{
								TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(xdocument.Root, fileInfo3.FullName);
							}
						}
					}
					catch (Exception ex5)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not process ",
							fileInfo3.Name,
							": ",
							ex5
						}), false);
					}
				}
			}
			IEnumerable<LoadedLanguage.KeyedReplacement> source = list;
			Func<LoadedLanguage.KeyedReplacement, bool> <>9__2;
			Func<LoadedLanguage.KeyedReplacement, bool> predicate;
			if ((predicate = <>9__2) == null)
			{
				predicate = (<>9__2 = ((LoadedLanguage.KeyedReplacement x) => !writtenUnusedKeyedTranslations.Contains(x)));
			}
			foreach (IGrouping<string, LoadedLanguage.KeyedReplacement> grouping in from x in source.Where(predicate)
			group x by x.fileSourceFullPath)
			{
				try
				{
					if (File.Exists(grouping.Key))
					{
						Log.Error("Could not save unused keyed translations to " + grouping.Key + " because this file already exists.", false);
					}
					else
					{
						object[] array3 = new object[1];
						int num = 0;
						XName name = "LanguageData";
						object[] array4 = new object[4];
						array4[0] = new XComment("NEWLINE");
						array4[1] = new XComment(" UNUSED ");
						array4[2] = grouping.Select(delegate(LoadedLanguage.KeyedReplacement x)
						{
							string text4 = x.isPlaceholder ? "TODO" : x.value;
							return new XElement(x.key, new XText(text4.NullOrEmpty() ? "" : text4.Replace("\n", "\\n")));
						});
						array4[3] = new XComment("NEWLINE");
						array3[num] = new XElement(name, array4);
						TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(new XDocument(array3), grouping.Key);
					}
				}
				catch (Exception ex6)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not save unused keyed translations to ",
						grouping.Key,
						": ",
						ex6
					}), false);
				}
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x000B1588 File Offset: 0x000AF788
		private static void CleanupDefInjections()
		{
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				string languageFolderPath = TranslationFilesCleaner.GetLanguageFolderPath(LanguageDatabase.activeLanguage, modMetaData.RootDir.FullName);
				string text = Path.Combine(languageFolderPath, "DefLinked");
				string text2 = Path.Combine(languageFolderPath, "DefInjected");
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				if (directoryInfo.Exists)
				{
					if (!Directory.Exists(text2))
					{
						Directory.Move(text, text2);
						Thread.Sleep(1000);
						directoryInfo = new DirectoryInfo(text2);
					}
				}
				else
				{
					directoryInfo = new DirectoryInfo(text2);
				}
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
				{
					try
					{
						fileInfo.Delete();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not delete ",
							fileInfo.Name,
							": ",
							ex
						}), false);
					}
				}
				foreach (Type type in GenDefDatabase.AllDefTypesWithDatabases())
				{
					try
					{
						TranslationFilesCleaner.CleanupDefInjectionsForDefType(type, directoryInfo.FullName, modMetaData);
					}
					catch (Exception ex2)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not process def-injections for type ",
							type.Name,
							": ",
							ex2
						}), false);
					}
				}
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x000B1778 File Offset: 0x000AF978
		private static void CleanupDefInjectionsForDefType(Type defType, string defInjectionsFolderPath, ModMetaData mod)
		{
			List<KeyValuePair<string, DefInjectionPackage.DefInjection>> list = (from x in (from x in LanguageDatabase.activeLanguage.defInjections
			where x.defType == defType
			select x).SelectMany((DefInjectionPackage x) => x.injections)
			where !x.Value.isPlaceholder && x.Value.ModifiesDefFromModOrNullCore(mod, defType)
			select x).ToList<KeyValuePair<string, DefInjectionPackage.DefInjection>>();
			Dictionary<string, DefInjectionPackage.DefInjection> dictionary = new Dictionary<string, DefInjectionPackage.DefInjection>();
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in list)
			{
				if (!dictionary.ContainsKey(keyValuePair.Value.normalizedPath))
				{
					dictionary.Add(keyValuePair.Value.normalizedPath, keyValuePair.Value);
				}
			}
			List<TranslationFilesCleaner.PossibleDefInjection> possibleDefInjections = new List<TranslationFilesCleaner.PossibleDefInjection>();
			DefInjectionUtility.ForEachPossibleDefInjection(defType, delegate(string suggestedPath, string normalizedPath, bool isCollection, string str, IEnumerable<string> collection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
			{
				if (translationAllowed)
				{
					TranslationFilesCleaner.PossibleDefInjection possibleDefInjection2 = new TranslationFilesCleaner.PossibleDefInjection();
					possibleDefInjection2.suggestedPath = suggestedPath;
					possibleDefInjection2.normalizedPath = normalizedPath;
					possibleDefInjection2.isCollection = isCollection;
					possibleDefInjection2.fullListTranslationAllowed = fullListTranslationAllowed;
					possibleDefInjection2.curValue = str;
					possibleDefInjection2.curValueCollection = collection;
					possibleDefInjection2.fieldInfo = fieldInfo;
					possibleDefInjection2.def = def;
					possibleDefInjections.Add(possibleDefInjection2);
				}
			}, mod);
			if (!possibleDefInjections.Any<TranslationFilesCleaner.PossibleDefInjection>() && !list.Any<KeyValuePair<string, DefInjectionPackage.DefInjection>>())
			{
				return;
			}
			List<KeyValuePair<string, DefInjectionPackage.DefInjection>> source = (from x in list
			where !x.Value.injected
			select x).ToList<KeyValuePair<string, DefInjectionPackage.DefInjection>>();
			using (IEnumerator<string> enumerator2 = (from x in possibleDefInjections
			select TranslationFilesCleaner.GetSourceFile(x.def)).Concat(from x in source
			select x.Value.fileSource).Distinct<string>().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string fileName = enumerator2.Current;
					try
					{
						XDocument xdocument = new XDocument();
						bool flag = false;
						try
						{
							XElement xelement = new XElement("LanguageData");
							xdocument.Add(xelement);
							xelement.Add(new XComment("NEWLINE"));
							List<TranslationFilesCleaner.PossibleDefInjection> source2 = (from x in possibleDefInjections
							where TranslationFilesCleaner.GetSourceFile(x.def) == fileName
							select x).ToList<TranslationFilesCleaner.PossibleDefInjection>();
							List<KeyValuePair<string, DefInjectionPackage.DefInjection>> source3 = (from x in source
							where x.Value.fileSource == fileName
							select x).ToList<KeyValuePair<string, DefInjectionPackage.DefInjection>>();
							using (IEnumerator<string> enumerator3 = (from x in (from x in source2
							select x.def.defName).Concat(from x in source3
							select x.Value.DefName).Distinct<string>()
							orderby x
							select x).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									string defName = enumerator3.Current;
									try
									{
										IEnumerable<TranslationFilesCleaner.PossibleDefInjection> enumerable = from x in source2
										where x.def.defName == defName
										select x;
										IEnumerable<KeyValuePair<string, DefInjectionPackage.DefInjection>> enumerable2 = from x in source3
										where x.Value.DefName == defName
										select x;
										if (enumerable.Any<TranslationFilesCleaner.PossibleDefInjection>())
										{
											bool flag2 = false;
											foreach (TranslationFilesCleaner.PossibleDefInjection possibleDefInjection in enumerable)
											{
												if (possibleDefInjection.isCollection)
												{
													IEnumerable<string> englishList = TranslationFilesCleaner.GetEnglishList(possibleDefInjection.normalizedPath, possibleDefInjection.curValueCollection, dictionary);
													bool flag3 = false;
													if (englishList != null)
													{
														int num = 0;
														foreach (string text in englishList)
														{
															if (dictionary.ContainsKey(possibleDefInjection.normalizedPath + "." + num))
															{
																flag3 = true;
																break;
															}
															num++;
														}
													}
													if (flag3 || !possibleDefInjection.fullListTranslationAllowed)
													{
														if (englishList == null)
														{
															continue;
														}
														int num2 = -1;
														using (IEnumerator<string> enumerator5 = englishList.GetEnumerator())
														{
															while (enumerator5.MoveNext())
															{
																string text2 = enumerator5.Current;
																num2++;
																string text3 = possibleDefInjection.normalizedPath + "." + num2;
																string suggestedPath2 = possibleDefInjection.suggestedPath + "." + num2;
																string text4;
																if (TKeySystem.TrySuggestTKeyPath(text3, out text4, null))
																{
																	suggestedPath2 = text4;
																}
																DefInjectionPackage.DefInjection defInjection;
																if (!dictionary.TryGetValue(text3, out defInjection))
																{
																	defInjection = null;
																}
																if (defInjection != null || DefInjectionUtility.ShouldCheckMissingInjection(text2, possibleDefInjection.fieldInfo, possibleDefInjection.def))
																{
																	flag2 = true;
																	flag = true;
																	try
																	{
																		if (!text2.NullOrEmpty())
																		{
																			xelement.Add(new XComment(TranslationFilesCleaner.SanitizeXComment(" EN: " + text2.Replace("\n", "\\n") + " ")));
																		}
																	}
																	catch (Exception ex)
																	{
																		Log.Error(string.Concat(new object[]
																		{
																			"Could not add comment node in ",
																			fileName,
																			": ",
																			ex
																		}), false);
																	}
																	xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(suggestedPath2, defInjection));
																}
															}
															continue;
														}
													}
													bool flag4 = false;
													if (englishList != null)
													{
														using (IEnumerator<string> enumerator5 = englishList.GetEnumerator())
														{
															while (enumerator5.MoveNext())
															{
																if (DefInjectionUtility.ShouldCheckMissingInjection(enumerator5.Current, possibleDefInjection.fieldInfo, possibleDefInjection.def))
																{
																	flag4 = true;
																	break;
																}
															}
														}
													}
													DefInjectionPackage.DefInjection defInjection2;
													if (!dictionary.TryGetValue(possibleDefInjection.normalizedPath, out defInjection2))
													{
														defInjection2 = null;
													}
													if (defInjection2 != null || flag4)
													{
														flag2 = true;
														flag = true;
														try
														{
															string text5 = TranslationFilesCleaner.ListToLiNodesString(englishList);
															if (!text5.NullOrEmpty())
															{
																xelement.Add(new XComment(TranslationFilesCleaner.SanitizeXComment(" EN:\n" + text5.Indented("    ") + "\n  ")));
															}
														}
														catch (Exception ex2)
														{
															Log.Error(string.Concat(new object[]
															{
																"Could not add comment node in ",
																fileName,
																": ",
																ex2
															}), false);
														}
														xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(possibleDefInjection.suggestedPath, defInjection2));
													}
												}
												else
												{
													DefInjectionPackage.DefInjection defInjection3;
													if (!dictionary.TryGetValue(possibleDefInjection.normalizedPath, out defInjection3))
													{
														defInjection3 = null;
													}
													string text6 = (defInjection3 != null && defInjection3.injected) ? defInjection3.replacedString : possibleDefInjection.curValue;
													if (defInjection3 != null || DefInjectionUtility.ShouldCheckMissingInjection(text6, possibleDefInjection.fieldInfo, possibleDefInjection.def))
													{
														flag2 = true;
														flag = true;
														try
														{
															if (!text6.NullOrEmpty())
															{
																xelement.Add(new XComment(TranslationFilesCleaner.SanitizeXComment(" EN: " + text6.Replace("\n", "\\n") + " ")));
															}
														}
														catch (Exception ex3)
														{
															Log.Error(string.Concat(new object[]
															{
																"Could not add comment node in ",
																fileName,
																": ",
																ex3
															}), false);
														}
														xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(possibleDefInjection.suggestedPath, defInjection3));
													}
												}
											}
											if (flag2)
											{
												xelement.Add(new XComment("NEWLINE"));
											}
										}
										if (enumerable2.Any<KeyValuePair<string, DefInjectionPackage.DefInjection>>())
										{
											flag = true;
											xelement.Add(new XComment(" UNUSED "));
											foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair2 in enumerable2)
											{
												xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(keyValuePair2.Value.path, keyValuePair2.Value));
											}
											xelement.Add(new XComment("NEWLINE"));
										}
									}
									catch (Exception ex4)
									{
										Log.Error(string.Concat(new object[]
										{
											"Could not process def-injections for def ",
											defName,
											": ",
											ex4
										}), false);
									}
								}
							}
						}
						finally
						{
							if (flag)
							{
								string text7 = Path.Combine(defInjectionsFolderPath, defType.Name);
								Directory.CreateDirectory(text7);
								TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(xdocument, Path.Combine(text7, fileName));
							}
						}
					}
					catch (Exception ex5)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not process def-injections for file ",
							fileName,
							": ",
							ex5
						}), false);
					}
				}
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x000B2104 File Offset: 0x000B0304
		private static void CleanupBackstories()
		{
			string text = Path.Combine(TranslationFilesCleaner.GetActiveLanguageCoreModFolderPath(), "Backstories");
			Directory.CreateDirectory(text);
			string path = Path.Combine(text, "Backstories.xml");
			File.Delete(path);
			XDocument xdocument = new XDocument();
			try
			{
				XElement xelement = new XElement("BackstoryTranslations");
				xdocument.Add(xelement);
				xelement.Add(new XComment("NEWLINE"));
				foreach (KeyValuePair<string, Backstory> keyValuePair in from x in BackstoryDatabase.allBackstories
				orderby x.Key
				select x)
				{
					try
					{
						XElement xelement2 = new XElement(keyValuePair.Key);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "title", keyValuePair.Value.title, keyValuePair.Value.untranslatedTitle, keyValuePair.Value.titleTranslated);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "titleFemale", keyValuePair.Value.titleFemale, keyValuePair.Value.untranslatedTitleFemale, keyValuePair.Value.titleFemaleTranslated);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "titleShort", keyValuePair.Value.titleShort, keyValuePair.Value.untranslatedTitleShort, keyValuePair.Value.titleShortTranslated);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "titleShortFemale", keyValuePair.Value.titleShortFemale, keyValuePair.Value.untranslatedTitleShortFemale, keyValuePair.Value.titleShortFemaleTranslated);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "desc", keyValuePair.Value.baseDesc, keyValuePair.Value.untranslatedDesc, keyValuePair.Value.descTranslated);
						xelement.Add(xelement2);
						xelement.Add(new XComment("NEWLINE"));
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not process backstory ",
							keyValuePair.Key,
							": ",
							ex
						}), false);
					}
				}
			}
			finally
			{
				TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(xdocument, path);
			}
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x000B2360 File Offset: 0x000B0560
		private static void AddBackstoryFieldElement(XElement addTo, string fieldName, string currentValue, string untranslatedValue, bool wasTranslated)
		{
			if (wasTranslated || !untranslatedValue.NullOrEmpty())
			{
				if (!untranslatedValue.NullOrEmpty())
				{
					addTo.Add(new XComment(TranslationFilesCleaner.SanitizeXComment(" EN: " + untranslatedValue.Replace("\n", "\\n") + " ")));
				}
				string text = wasTranslated ? currentValue : "TODO";
				addTo.Add(new XElement(fieldName, text.NullOrEmpty() ? "" : text.Replace("\n", "\\n")));
			}
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x000B23F0 File Offset: 0x000B05F0
		private static string GetActiveLanguageCoreModFolderPath()
		{
			ModContentPack modContentPack = LoadedModManager.RunningMods.FirstOrDefault((ModContentPack x) => x.IsCoreMod);
			return TranslationFilesCleaner.GetLanguageFolderPath(LanguageDatabase.activeLanguage, modContentPack.RootDir);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00010A25 File Offset: 0x0000EC25
		public static string GetLanguageFolderPath(LoadedLanguage language, string modRootDir)
		{
			return Path.Combine(Path.Combine(modRootDir, "Languages"), language.folderName);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00010A3D File Offset: 0x0000EC3D
		private static void SaveXMLDocumentWithProcessedNewlineTags(XNode doc, string path)
		{
			File.WriteAllText(path, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" + doc.ToString().Replace("<!--NEWLINE-->", "").Replace("&gt;", ">"), Encoding.UTF8);
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x000B2438 File Offset: 0x000B0638
		private static string ListToLiNodesString(IEnumerable<string> list)
		{
			if (list == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in list)
			{
				stringBuilder.Append("<li>");
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(text.Replace("\n", "\\n"));
				}
				stringBuilder.Append("</li>");
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x000B24D4 File Offset: 0x000B06D4
		private static XElement ListToXElement(IEnumerable<string> list, string name, List<Pair<int, string>> comments)
		{
			XElement xelement = new XElement(name);
			if (list != null)
			{
				int num = 0;
				foreach (string text in list)
				{
					if (comments != null)
					{
						for (int i = 0; i < comments.Count; i++)
						{
							if (comments[i].First == num)
							{
								xelement.Add(new XComment(comments[i].Second));
							}
						}
					}
					XElement xelement2 = new XElement("li");
					if (!text.NullOrEmpty())
					{
						xelement2.Add(new XText(text.Replace("\n", "\\n")));
					}
					xelement.Add(xelement2);
					num++;
				}
				if (comments != null)
				{
					for (int j = 0; j < comments.Count; j++)
					{
						if (comments[j].First == num)
						{
							xelement.Add(new XComment(comments[j].Second));
						}
					}
				}
			}
			return xelement;
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00010A78 File Offset: 0x0000EC78
		private static string AppendXmlExtensionIfNotAlready(string fileName)
		{
			if (!fileName.ToLower().EndsWith(".xml"))
			{
				return fileName + ".xml";
			}
			return fileName;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00010A99 File Offset: 0x0000EC99
		private static string GetSourceFile(Def def)
		{
			if (!def.fileName.NullOrEmpty())
			{
				return TranslationFilesCleaner.AppendXmlExtensionIfNotAlready(def.fileName);
			}
			return "Unknown.xml";
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x000B2604 File Offset: 0x000B0804
		private static string TryRemoveLastIndexSymbol(string str)
		{
			int num = str.LastIndexOf('.');
			if (num >= 0)
			{
				if (str.Substring(num + 1).All((char x) => char.IsNumber(x)))
				{
					return str.Substring(0, num);
				}
			}
			return str;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x000B2658 File Offset: 0x000B0858
		private static IEnumerable<string> GetEnglishList(string normalizedPath, IEnumerable<string> curValue, Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath)
		{
			DefInjectionPackage.DefInjection defInjection;
			if (injectionsByNormalizedPath.TryGetValue(normalizedPath, out defInjection) && defInjection.injected)
			{
				return defInjection.replacedList;
			}
			if (curValue == null)
			{
				return null;
			}
			List<string> list = curValue.ToList<string>();
			for (int i = 0; i < list.Count; i++)
			{
				string key = normalizedPath + "." + i;
				DefInjectionPackage.DefInjection defInjection2;
				if (injectionsByNormalizedPath.TryGetValue(key, out defInjection2) && defInjection2.injected)
				{
					list[i] = defInjection2.replacedString;
				}
			}
			return list;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x000B26D4 File Offset: 0x000B08D4
		private static XElement GetDefInjectableFieldNode(string suggestedPath, DefInjectionPackage.DefInjection existingInjection)
		{
			if (existingInjection == null || existingInjection.isPlaceholder)
			{
				return new XElement(suggestedPath, new XText("TODO"));
			}
			if (existingInjection.IsFullListInjection)
			{
				return TranslationFilesCleaner.ListToXElement(existingInjection.fullListInjection, suggestedPath, existingInjection.fullListInjectionComments);
			}
			XElement xelement;
			if (!existingInjection.injection.NullOrEmpty())
			{
				if (existingInjection.suggestedPath.EndsWith(".slateRef") && ConvertHelper.IsXml(existingInjection.injection))
				{
					try
					{
						return XElement.Parse(string.Concat(new string[]
						{
							"<",
							suggestedPath,
							">",
							existingInjection.injection,
							"</",
							suggestedPath,
							">"
						}));
					}
					catch (Exception ex)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Could not parse XML: ",
							existingInjection.injection,
							". Exception: ",
							ex
						}), false);
						xelement = new XElement(suggestedPath);
						xelement.Add(existingInjection.injection);
						return xelement;
					}
				}
				xelement = new XElement(suggestedPath);
				xelement.Add(new XText(existingInjection.injection.Replace("\n", "\\n")));
			}
			else
			{
				xelement = new XElement(suggestedPath);
			}
			return xelement;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000B2830 File Offset: 0x000B0A30
		private static string SanitizeXComment(string comment)
		{
			while (comment.Contains("-----"))
			{
				comment = comment.Replace("-----", "- - -");
			}
			while (comment.Contains("--"))
			{
				comment = comment.Replace("--", "- -");
			}
			return comment;
		}

		// Token: 0x04000BAE RID: 2990
		private const string NewlineTag = "NEWLINE";

		// Token: 0x04000BAF RID: 2991
		private const string NewlineTagFull = "<!--NEWLINE-->";

		// Token: 0x02000229 RID: 553
		private class PossibleDefInjection
		{
			// Token: 0x04000BB0 RID: 2992
			public string suggestedPath;

			// Token: 0x04000BB1 RID: 2993
			public string normalizedPath;

			// Token: 0x04000BB2 RID: 2994
			public bool isCollection;

			// Token: 0x04000BB3 RID: 2995
			public bool fullListTranslationAllowed;

			// Token: 0x04000BB4 RID: 2996
			public string curValue;

			// Token: 0x04000BB5 RID: 2997
			public IEnumerable<string> curValueCollection;

			// Token: 0x04000BB6 RID: 2998
			public FieldInfo fieldInfo;

			// Token: 0x04000BB7 RID: 2999
			public Def def;
		}
	}
}
