using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using RimWorld;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200014D RID: 333
	public static class BackstoryTranslationUtility
	{
		// Token: 0x06000950 RID: 2384 RVA: 0x0002E8FA File Offset: 0x0002CAFA
		private static IEnumerable<XElement> BackstoryTranslationElements(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folders, List<string> loadErrors)
		{
			Dictionary<ModContentPack, HashSet<string>> alreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();
			foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in folders)
			{
				if (!alreadyLoadedFiles.ContainsKey(tuple.Item2))
				{
					alreadyLoadedFiles[tuple.Item2] = new HashSet<string>();
				}
				VirtualFile file = tuple.Item1.GetFile("Backstories/Backstories.xml");
				if (file.Exists)
				{
					if (!file.FullPath.StartsWith(tuple.Item3))
					{
						Log.Error("Failed to get a relative path for a file: " + file.FullPath + ", located in " + tuple.Item3);
					}
					else
					{
						string item = file.FullPath.Substring(tuple.Item3.Length);
						if (!alreadyLoadedFiles[tuple.Item2].Contains(item))
						{
							alreadyLoadedFiles[tuple.Item2].Add(item);
							XDocument xdocument;
							try
							{
								xdocument = file.LoadAsXDocument();
							}
							catch (Exception ex)
							{
								if (loadErrors != null)
								{
									loadErrors.Add(string.Concat(new object[]
									{
										"Exception loading backstory translation data from file ",
										file,
										": ",
										ex
									}));
								}
								yield break;
							}
							foreach (XElement xelement in xdocument.Root.Elements())
							{
								yield return xelement;
							}
							IEnumerator<XElement> enumerator2 = null;
						}
					}
				}
			}
			IEnumerator<Tuple<VirtualDirectory, ModContentPack, string>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002E914 File Offset: 0x0002CB14
		public static void LoadAndInjectBackstoryData(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folderPaths, List<string> loadErrors)
		{
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(folderPaths, loadErrors))
			{
				string text = "[unknown]";
				try
				{
					text = xelement.Name.ToString();
					string text2 = BackstoryTranslationUtility.GetText(xelement, "title");
					string text3 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
					string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
					string text5 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
					string text6 = BackstoryTranslationUtility.GetText(xelement, "desc");
					Backstory backstory;
					if (!BackstoryDatabase.TryGetWithIdentifier(text, out backstory, false))
					{
						throw new Exception("Backstory not found matching identifier " + text);
					}
					if (text2 == backstory.title && text3 == backstory.titleFemale && text4 == backstory.titleShort && text5 == backstory.titleShortFemale && text6 == backstory.baseDesc)
					{
						throw new Exception("Backstory translation exactly matches default data: " + text);
					}
					if (text2 != null)
					{
						backstory.SetTitle(text2, backstory.titleFemale);
						backstory.titleTranslated = true;
					}
					if (text3 != null)
					{
						backstory.SetTitle(backstory.title, text3);
						backstory.titleFemaleTranslated = true;
					}
					if (text4 != null)
					{
						backstory.SetTitleShort(text4, backstory.titleShortFemale);
						backstory.titleShortTranslated = true;
					}
					if (text5 != null)
					{
						backstory.SetTitleShort(backstory.titleShort, text5);
						backstory.titleShortFemaleTranslated = true;
					}
					if (text6 != null)
					{
						backstory.baseDesc = text6;
						backstory.descTranslated = true;
					}
				}
				catch (Exception ex)
				{
					loadErrors.Add(string.Concat(new string[]
					{
						"Couldn't load backstory ",
						text,
						": ",
						ex.Message,
						"\nFull XML text:\n\n",
						xelement.ToString()
					}));
				}
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002EB1C File Offset: 0x0002CD1C
		public static List<string> MissingBackstoryTranslations(LoadedLanguage lang)
		{
			List<KeyValuePair<string, Backstory>> list = BackstoryDatabase.allBackstories.ToList<KeyValuePair<string, Backstory>>();
			List<string> list2 = new List<string>();
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(lang.AllDirectories, null))
			{
				try
				{
					string text = xelement.Name.ToString();
					string modifiedIdentifier = BackstoryDatabase.GetIdentifierClosestMatch(text, false);
					bool flag = list.Any((KeyValuePair<string, Backstory> x) => x.Key == modifiedIdentifier);
					KeyValuePair<string, Backstory> backstory = list.Find((KeyValuePair<string, Backstory> x) => x.Key == modifiedIdentifier);
					if (flag)
					{
						list.RemoveAt(list.FindIndex((KeyValuePair<string, Backstory> x) => x.Key == backstory.Key));
						string text2 = BackstoryTranslationUtility.GetText(xelement, "title");
						string text3 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
						string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
						string text5 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
						string text6 = BackstoryTranslationUtility.GetText(xelement, "desc");
						if (text2.NullOrEmpty())
						{
							list2.Add(text + ".title missing");
						}
						if (flag && !backstory.Value.titleFemale.NullOrEmpty() && text3.NullOrEmpty())
						{
							list2.Add(text + ".titleFemale missing");
						}
						if (text4.NullOrEmpty())
						{
							list2.Add(text + ".titleShort missing");
						}
						if (flag && !backstory.Value.titleShortFemale.NullOrEmpty() && text5.NullOrEmpty())
						{
							list2.Add(text + ".titleShortFemale missing");
						}
						if (text6.NullOrEmpty())
						{
							list2.Add(text + ".desc missing");
						}
					}
					else
					{
						list2.Add("Translation doesn't correspond to any backstory: " + text);
					}
				}
				catch (Exception ex)
				{
					list2.Add(string.Concat(new object[]
					{
						"Exception reading ",
						xelement.Name,
						": ",
						ex.Message
					}));
				}
			}
			foreach (KeyValuePair<string, Backstory> keyValuePair in list)
			{
				list2.Add("Missing backstory: " + keyValuePair.Key);
			}
			return list2;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002EDC0 File Offset: 0x0002CFC0
		public static List<string> BackstoryTranslationsMatchingEnglish(LoadedLanguage lang)
		{
			List<string> list = new List<string>();
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(lang.AllDirectories, null))
			{
				try
				{
					string text = xelement.Name.ToString();
					Backstory backstory;
					if (BackstoryDatabase.allBackstories.TryGetValue(BackstoryDatabase.GetIdentifierClosestMatch(text, true), out backstory))
					{
						string text2 = BackstoryTranslationUtility.GetText(xelement, "title");
						string text3 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
						string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
						string text5 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
						string text6 = BackstoryTranslationUtility.GetText(xelement, "desc");
						if (!text2.NullOrEmpty() && text2 == backstory.untranslatedTitle)
						{
							list.Add(text + ".title '" + text2.Replace("\n", "\\n") + "'");
						}
						if (!text3.NullOrEmpty() && text3 == backstory.untranslatedTitleFemale)
						{
							list.Add(text + ".titleFemale '" + text3.Replace("\n", "\\n") + "'");
						}
						if (!text4.NullOrEmpty() && text4 == backstory.untranslatedTitleShort)
						{
							list.Add(text + ".titleShort '" + text4.Replace("\n", "\\n") + "'");
						}
						if (!text5.NullOrEmpty() && text5 == backstory.untranslatedTitleShortFemale)
						{
							list.Add(text + ".titleShortFemale '" + text5.Replace("\n", "\\n") + "'");
						}
						if (!text6.NullOrEmpty() && text6 == backstory.untranslatedDesc)
						{
							list.Add(text + ".desc '" + text6.Replace("\n", "\\n") + "'");
						}
					}
				}
				catch (Exception ex)
				{
					list.Add(string.Concat(new object[]
					{
						"Exception reading ",
						xelement.Name,
						": ",
						ex.Message
					}));
				}
			}
			return list;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002F020 File Offset: 0x0002D220
		private static string GetText(XElement backstory, string fieldName)
		{
			XElement xelement = backstory.Element(fieldName);
			if (xelement == null || xelement.Value == "TODO")
			{
				return null;
			}
			return xelement.Value.Replace("\\n", "\n");
		}

		// Token: 0x04000854 RID: 2132
		public const string BackstoriesFolder = "Backstories";

		// Token: 0x04000855 RID: 2133
		public const string BackstoriesFileName = "Backstories.xml";
	}
}
