using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Verse
{
	// Token: 0x02000371 RID: 881
	public static class TKeySystem
	{
		// Token: 0x0600163B RID: 5691 RVA: 0x00015C13 File Offset: 0x00013E13
		public static void Clear()
		{
			TKeySystem.keys.Clear();
			TKeySystem.tKeyToNormalizedTranslationKey.Clear();
			TKeySystem.translationKeyToTKey.Clear();
			TKeySystem.loadErrors.Clear();
			TKeySystem.treatAsList.Clear();
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000D57DC File Offset: 0x000D39DC
		public static void Parse(XmlDocument document)
		{
			foreach (object obj in document.ChildNodes[0].ChildNodes)
			{
				TKeySystem.ParseDefNode((XmlNode)obj);
			}
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00015C47 File Offset: 0x00013E47
		public static void MarkTreatAsList(XmlNode node)
		{
			TKeySystem.treatAsList.Add(node);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x000D5840 File Offset: 0x000D3A40
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

		// Token: 0x0600163F RID: 5695 RVA: 0x00015C55 File Offset: 0x00013E55
		public static bool TryGetNormalizedPath(string tKeyPath, out string normalizedPath)
		{
			return TKeySystem.TryFindShortestReplacementPath(tKeyPath, delegate(string path, out string result)
			{
				return TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(path, out result);
			}, out normalizedPath);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000D5A0C File Offset: 0x000D3C0C
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

		// Token: 0x06001641 RID: 5697 RVA: 0x000D5A8C File Offset: 0x000D3C8C
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

		// Token: 0x06001642 RID: 5698 RVA: 0x000D5ACC File Offset: 0x000D3CCC
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

		// Token: 0x06001643 RID: 5699 RVA: 0x000D5BA4 File Offset: 0x000D3DA4
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

		// Token: 0x06001645 RID: 5701 RVA: 0x000D5C54 File Offset: 0x000D3E54
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

		// Token: 0x06001646 RID: 5702 RVA: 0x000D5CC4 File Offset: 0x000D3EC4
		[CompilerGenerated]
		internal static void <ParseDefNode>g__CrawlNodesRecursive|17_3(XmlNode n, ref TKeySystem.<>c__DisplayClass17_0 A_1)
		{
			TKeySystem.<ParseDefNode>g__ProcessNode|17_2(n, ref A_1);
			foreach (object obj in n.ChildNodes)
			{
				TKeySystem.<ParseDefNode>g__CrawlNodesRecursive|17_3((XmlNode)obj, ref A_1);
			}
		}

		// Token: 0x0400110B RID: 4363
		private static List<TKeySystem.TKeyRef> keys = new List<TKeySystem.TKeyRef>();

		// Token: 0x0400110C RID: 4364
		public static List<string> loadErrors = new List<string>();

		// Token: 0x0400110D RID: 4365
		private static Dictionary<string, string> tKeyToNormalizedTranslationKey = new Dictionary<string, string>();

		// Token: 0x0400110E RID: 4366
		private static Dictionary<string, string> translationKeyToTKey = new Dictionary<string, string>();

		// Token: 0x0400110F RID: 4367
		private static HashSet<XmlNode> treatAsList = new HashSet<XmlNode>();

		// Token: 0x04001110 RID: 4368
		public const string AttributeName = "TKey";

		// Token: 0x02000372 RID: 882
		private struct TKeyRef
		{
			// Token: 0x04001111 RID: 4369
			public string defName;

			// Token: 0x04001112 RID: 4370
			public string defTypeName;

			// Token: 0x04001113 RID: 4371
			public XmlNode defRootNode;

			// Token: 0x04001114 RID: 4372
			public XmlNode node;

			// Token: 0x04001115 RID: 4373
			public string tKey;

			// Token: 0x04001116 RID: 4374
			public string tKeyPath;
		}

		// Token: 0x02000373 RID: 883
		private struct PossibleDefInjection
		{
			// Token: 0x04001117 RID: 4375
			public string normalizedPath;

			// Token: 0x04001118 RID: 4376
			public string path;
		}

		// Token: 0x02000374 RID: 884
		// (Invoke) Token: 0x06001648 RID: 5704
		private delegate bool PathMatcher(string path, out string match);
	}
}
