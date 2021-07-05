using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Verse
{
	// Token: 0x02000256 RID: 598
	public static class TKeySystem
	{
		// Token: 0x06001102 RID: 4354 RVA: 0x00060419 File Offset: 0x0005E619
		public static void Clear()
		{
			TKeySystem.keys.Clear();
			TKeySystem.tKeyToNormalizedTranslationKey.Clear();
			TKeySystem.translationKeyToTKey.Clear();
			TKeySystem.loadErrors.Clear();
			TKeySystem.treatAsList.Clear();
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00060450 File Offset: 0x0005E650
		public static void Parse(XmlDocument document)
		{
			foreach (object obj in document.ChildNodes[0].ChildNodes)
			{
				TKeySystem.ParseDefNode((XmlNode)obj);
			}
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000604B4 File Offset: 0x0005E6B4
		public static void MarkTreatAsList(XmlNode node)
		{
			TKeySystem.treatAsList.Add(node);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000604C4 File Offset: 0x0005E6C4
		public static void BuildMappings()
		{
			Dictionary<string, string> tmpTranslationKeyToTKey = new Dictionary<string, string>();
			foreach (TKeySystem.TKeyRef tkeyRef in TKeySystem.keys)
			{
				string normalizedTranslationKey = TKeySystem.GetNormalizedTranslationKey(tkeyRef);
				string text;
				if (TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(tkeyRef.tKeyPath, out text))
				{
					TKeySystem.loadErrors.Add(string.Concat(new string[]
					{
						"Duplicate TKey: ",
						tkeyRef.tKeyPath,
						" -> NEW=",
						normalizedTranslationKey,
						" | OLD",
						text,
						" - Ignoring old"
					}));
				}
				else
				{
					TKeySystem.tKeyToNormalizedTranslationKey.Add(tkeyRef.tKeyPath, normalizedTranslationKey);
					tmpTranslationKeyToTKey.Add(normalizedTranslationKey, tkeyRef.tKeyPath);
				}
			}
			DefInjectionUtility.PossibleDefInjectionTraverser <>9__1;
			foreach (string typeName in (from k in TKeySystem.keys
			select k.defTypeName).Distinct<string>())
			{
				Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(typeName, null);
				DefInjectionUtility.PossibleDefInjectionTraverser action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(string suggestedPath, string normalizedPath, bool isCollection, string currentValue, IEnumerable<string> currentValueCollection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
					{
						string text2;
						string value;
						if (translationAllowed && !TKeySystem.TryGetNormalizedPath(suggestedPath, out text2) && TKeySystem.TrySuggestTKeyPath(normalizedPath, out value, tmpTranslationKeyToTKey))
						{
							tmpTranslationKeyToTKey.Add(suggestedPath, value);
						}
					});
				}
				DefInjectionUtility.ForEachPossibleDefInjection(typeInAnyAssembly, action, null);
			}
			foreach (KeyValuePair<string, string> keyValuePair in tmpTranslationKeyToTKey)
			{
				TKeySystem.translationKeyToTKey.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00060690 File Offset: 0x0005E890
		public static bool TryGetNormalizedPath(string tKeyPath, out string normalizedPath)
		{
			return TKeySystem.TryFindShortestReplacementPath(tKeyPath, delegate(string path, out string result)
			{
				return TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(path, out result);
			}, out normalizedPath);
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x000606B8 File Offset: 0x0005E8B8
		private static bool TryFindShortestReplacementPath(string path, TKeySystem.PathMatcher matcher, out string result)
		{
			if (matcher(path, out result))
			{
				return true;
			}
			int num = 100;
			int num2 = path.Length - 1;
			for (;;)
			{
				if (num2 > 0 && path[num2] != '.')
				{
					num2--;
				}
				else
				{
					if (path[num2] == '.')
					{
						string path2 = path.Substring(0, num2);
						if (matcher(path2, out result))
						{
							break;
						}
					}
					num2--;
					num--;
					if (num2 <= 0 || num <= 0)
					{
						goto IL_6D;
					}
				}
			}
			result += path.Substring(num2);
			return true;
			IL_6D:
			result = null;
			return false;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00060738 File Offset: 0x0005E938
		public static bool TrySuggestTKeyPath(string translationPath, out string tKeyPath, Dictionary<string, string> lookup = null)
		{
			if (lookup == null)
			{
				lookup = TKeySystem.translationKeyToTKey;
			}
			return TKeySystem.TryFindShortestReplacementPath(translationPath, delegate(string path, out string result)
			{
				return lookup.TryGetValue(path, out result);
			}, out tKeyPath);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00060778 File Offset: 0x0005E978
		private static string GetNormalizedTranslationKey(TKeySystem.TKeyRef tKeyRef)
		{
			string text = "";
			XmlNode currentNode;
			for (currentNode = tKeyRef.node; currentNode != tKeyRef.defRootNode; currentNode = currentNode.ParentNode)
			{
				if (currentNode.Name == "li" || TKeySystem.treatAsList.Contains(currentNode.ParentNode))
				{
					text = "." + currentNode.ParentNode.ChildNodes.Cast<XmlNode>().FirstIndexOf((XmlNode n) => n == currentNode) + text;
				}
				else
				{
					text = "." + currentNode.Name + text;
				}
			}
			return tKeyRef.defName + text;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00060850 File Offset: 0x0005EA50
		private static void ParseDefNode(XmlNode node)
		{
			string text = (from XmlNode n in node.ChildNodes
			where n.Name == "defName"
			select n.InnerText).FirstOrDefault<string>();
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			TKeySystem.<>c__DisplayClass17_0 CS$<>8__locals1;
			CS$<>8__locals1.tKeyRefTemplate = default(TKeySystem.TKeyRef);
			CS$<>8__locals1.tKeyRefTemplate.defName = text;
			CS$<>8__locals1.tKeyRefTemplate.defTypeName = node.Name;
			CS$<>8__locals1.tKeyRefTemplate.defRootNode = node;
			TKeySystem.<ParseDefNode>g__CrawlNodesRecursive|17_3(node, ref CS$<>8__locals1);
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00060934 File Offset: 0x0005EB34
		[CompilerGenerated]
		internal static void <ParseDefNode>g__ProcessNode|17_2(XmlNode n, ref TKeySystem.<>c__DisplayClass17_0 A_1)
		{
			XmlAttribute xmlAttribute;
			if (n.Attributes != null && (xmlAttribute = n.Attributes["TKey"]) != null)
			{
				TKeySystem.TKeyRef tKeyRefTemplate = A_1.tKeyRefTemplate;
				tKeyRefTemplate.tKey = xmlAttribute.Value;
				tKeyRefTemplate.node = n;
				tKeyRefTemplate.tKeyPath = tKeyRefTemplate.defName + "." + tKeyRefTemplate.tKey;
				TKeySystem.keys.Add(tKeyRefTemplate);
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x000609A4 File Offset: 0x0005EBA4
		[CompilerGenerated]
		internal static void <ParseDefNode>g__CrawlNodesRecursive|17_3(XmlNode n, ref TKeySystem.<>c__DisplayClass17_0 A_1)
		{
			TKeySystem.<ParseDefNode>g__ProcessNode|17_2(n, ref A_1);
			foreach (object obj in n.ChildNodes)
			{
				TKeySystem.<ParseDefNode>g__CrawlNodesRecursive|17_3((XmlNode)obj, ref A_1);
			}
		}

		// Token: 0x04000CEB RID: 3307
		private static List<TKeySystem.TKeyRef> keys = new List<TKeySystem.TKeyRef>();

		// Token: 0x04000CEC RID: 3308
		public static List<string> loadErrors = new List<string>();

		// Token: 0x04000CED RID: 3309
		private static Dictionary<string, string> tKeyToNormalizedTranslationKey = new Dictionary<string, string>();

		// Token: 0x04000CEE RID: 3310
		private static Dictionary<string, string> translationKeyToTKey = new Dictionary<string, string>();

		// Token: 0x04000CEF RID: 3311
		private static HashSet<XmlNode> treatAsList = new HashSet<XmlNode>();

		// Token: 0x04000CF0 RID: 3312
		public const string AttributeName = "TKey";

		// Token: 0x020019CC RID: 6604
		private struct TKeyRef
		{
			// Token: 0x040062FC RID: 25340
			public string defName;

			// Token: 0x040062FD RID: 25341
			public string defTypeName;

			// Token: 0x040062FE RID: 25342
			public XmlNode defRootNode;

			// Token: 0x040062FF RID: 25343
			public XmlNode node;

			// Token: 0x04006300 RID: 25344
			public string tKey;

			// Token: 0x04006301 RID: 25345
			public string tKeyPath;
		}

		// Token: 0x020019CD RID: 6605
		private struct PossibleDefInjection
		{
			// Token: 0x04006302 RID: 25346
			public string normalizedPath;

			// Token: 0x04006303 RID: 25347
			public string path;
		}

		// Token: 0x020019CE RID: 6606
		// (Invoke) Token: 0x06009A2A RID: 39466
		private delegate bool PathMatcher(string path, out string match);
	}
}
