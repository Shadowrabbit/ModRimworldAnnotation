using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001ED7 RID: 7895
	public static class QuestGenUtility
	{
		// Token: 0x0600A963 RID: 43363 RVA: 0x00317870 File Offset: 0x00315A70
		public static string HardcodedSignalWithQuestID(string signal)
		{
			if (!QuestGen.Working)
			{
				return signal;
			}
			if (signal.NullOrEmpty())
			{
				return null;
			}
			if (signal.StartsWith("Quest") && signal.IndexOf('.') >= 0)
			{
				return signal;
			}
			if (signal.IndexOf('.') >= 0)
			{
				int num = signal.IndexOf('.');
				string text = signal.Substring(0, num);
				string str = signal.Substring(num + 1);
				if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
				{
					text = QuestGen.slate.CurrentPrefix + "/" + text;
				}
				text = QuestGenUtility.NormalizeVarPath(text);
				QuestGen.AddSlateQuestTagToAddWhenFinished(text);
				return QuestGen.GenerateNewSignal(text + "." + str, false);
			}
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				signal = QuestGen.slate.CurrentPrefix + "/" + signal;
			}
			signal = QuestGenUtility.NormalizeVarPath(signal);
			return QuestGen.GenerateNewSignal(signal, false);
		}

		// Token: 0x0600A964 RID: 43364 RVA: 0x00317950 File Offset: 0x00315B50
		public static string HardcodedTargetQuestTagWithQuestID(string questTag)
		{
			if (!QuestGen.Working)
			{
				return questTag;
			}
			if (questTag.NullOrEmpty())
			{
				return null;
			}
			if (questTag.StartsWith("Quest") && questTag.IndexOf('.') >= 0)
			{
				return null;
			}
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				questTag = QuestGen.slate.CurrentPrefix + "/" + questTag;
			}
			questTag = QuestGenUtility.NormalizeVarPath(questTag);
			return QuestGen.GenerateNewTargetQuestTag(questTag, false);
		}

		// Token: 0x0600A965 RID: 43365 RVA: 0x0006F4A1 File Offset: 0x0006D6A1
		public static string QuestTagSignal(string questTag, string signal)
		{
			return questTag + "." + signal;
		}

		// Token: 0x0600A966 RID: 43366 RVA: 0x003179C4 File Offset: 0x00315BC4
		public static void RunInner(Action inner, QuestPartActivable outerQuestPart)
		{
			string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
			outerQuestPart.outSignalsCompleted.Add(text);
			QuestGenUtility.RunInner(inner, text);
		}

		// Token: 0x0600A967 RID: 43367 RVA: 0x003179F0 File Offset: 0x00315BF0
		public static void RunInner(Action inner, string innerNodeInSignal)
		{
			Slate.VarRestoreInfo restoreInfo = QuestGen.slate.GetRestoreInfo("inSignal");
			QuestGen.slate.Set<string>("inSignal", innerNodeInSignal, false);
			try
			{
				inner();
			}
			finally
			{
				QuestGen.slate.Restore(restoreInfo);
			}
		}

		// Token: 0x0600A968 RID: 43368 RVA: 0x00317A44 File Offset: 0x00315C44
		public static void RunInnerNode(QuestNode node, QuestPartActivable outerQuestPart)
		{
			string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
			outerQuestPart.outSignalsCompleted.Add(text);
			QuestGenUtility.RunInnerNode(node, text);
		}

		// Token: 0x0600A969 RID: 43369 RVA: 0x00317A70 File Offset: 0x00315C70
		public static void RunInnerNode(QuestNode node, string innerNodeInSignal)
		{
			Slate.VarRestoreInfo restoreInfo = QuestGen.slate.GetRestoreInfo("inSignal");
			QuestGen.slate.Set<string>("inSignal", innerNodeInSignal, false);
			try
			{
				node.Run();
			}
			finally
			{
				QuestGen.slate.Restore(restoreInfo);
			}
		}

		// Token: 0x0600A96A RID: 43370 RVA: 0x00317AC4 File Offset: 0x00315CC4
		public static void AddSlateVars(ref GrammarRequest req)
		{
			QuestGenUtility.tmpAddedSlateVars.Clear();
			List<Rule> rules = req.Rules;
			for (int i = 0; i < rules.Count; i++)
			{
				Rule_String rule_String = rules[i] as Rule_String;
				if (rule_String != null)
				{
					string text = rule_String.Generate();
					if (text != null)
					{
						bool flag = false;
						QuestGenUtility.tmpSymbol.Clear();
						for (int j = 0; j < text.Length; j++)
						{
							if (text[j] == '[')
							{
								flag = true;
							}
							else if (text[j] == ']')
							{
								QuestGenUtility.AddSlateVar(ref req, QuestGenUtility.tmpSymbol.ToString(), QuestGenUtility.tmpAddedSlateVars);
								QuestGenUtility.tmpSymbol.Clear();
								flag = false;
							}
							else if (flag)
							{
								QuestGenUtility.tmpSymbol.Append(text[j]);
							}
						}
					}
					if (rule_String.constantConstraints != null)
					{
						for (int k = 0; k < rule_String.constantConstraints.Count; k++)
						{
							string key = rule_String.constantConstraints[k].key;
							QuestGenUtility.AddSlateVar(ref req, key, QuestGenUtility.tmpAddedSlateVars);
						}
					}
				}
			}
		}

		// Token: 0x0600A96B RID: 43371 RVA: 0x00317BD8 File Offset: 0x00315DD8
		private static void AddSlateVar(ref GrammarRequest req, string absoluteName, HashSet<string> added)
		{
			if (absoluteName == null)
			{
				return;
			}
			QuestGenUtility.tmpVarAbsoluteName.Clear();
			QuestGenUtility.tmpVarAbsoluteName.Append(absoluteName);
			while (QuestGenUtility.tmpVarAbsoluteName.Length > 0)
			{
				string text = QuestGenUtility.tmpVarAbsoluteName.ToString();
				if (added.Contains(text))
				{
					break;
				}
				object obj;
				if (QuestGen.slate.TryGet<object>(text, out obj, true))
				{
					QuestGenUtility.AddSlateVar(ref req, text, obj);
					added.Add(text);
					return;
				}
				if (char.IsNumber(QuestGenUtility.tmpVarAbsoluteName[QuestGenUtility.tmpVarAbsoluteName.Length - 1]))
				{
					while (char.IsNumber(QuestGenUtility.tmpVarAbsoluteName[QuestGenUtility.tmpVarAbsoluteName.Length - 1]))
					{
						StringBuilder stringBuilder = QuestGenUtility.tmpVarAbsoluteName;
						int length = stringBuilder.Length;
						stringBuilder.Length = length - 1;
					}
				}
				else
				{
					int num = text.LastIndexOf('_');
					if (num < 0)
					{
						break;
					}
					int num2 = text.LastIndexOf('/');
					if (num < num2)
					{
						break;
					}
					QuestGenUtility.tmpVarAbsoluteName.Length = num;
				}
			}
		}

		// Token: 0x0600A96C RID: 43372 RVA: 0x00317CC8 File Offset: 0x00315EC8
		private static void AddSlateVar(ref GrammarRequest req, string absoluteName, object obj)
		{
			if (obj == null)
			{
				return;
			}
			if (obj is BodyPartRecord)
			{
				req.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord(absoluteName, (BodyPartRecord)obj));
			}
			else if (obj is Def)
			{
				req.Rules.AddRange(GrammarUtility.RulesForDef(absoluteName, (Def)obj));
			}
			else if (obj is Faction)
			{
				Faction faction = (Faction)obj;
				req.Rules.AddRange(GrammarUtility.RulesForFaction(absoluteName, faction, true));
				if (faction.leader != null)
				{
					req.Rules.AddRange(GrammarUtility.RulesForPawn(absoluteName + "_leader", faction.leader, req.Constants, true, true));
				}
			}
			else if (obj is Pawn)
			{
				Pawn pawn = (Pawn)obj;
				req.Rules.AddRange(GrammarUtility.RulesForPawn(absoluteName, pawn, req.Constants, true, true));
				if (pawn.Faction != null)
				{
					req.Rules.AddRange(GrammarUtility.RulesForFaction(absoluteName + "_faction", pawn.Faction, true));
				}
			}
			else if (obj is WorldObject)
			{
				req.Rules.AddRange(GrammarUtility.RulesForWorldObject(absoluteName, (WorldObject)obj, true));
			}
			else if (obj is Map)
			{
				req.Rules.AddRange(GrammarUtility.RulesForWorldObject(absoluteName, ((Map)obj).Parent, true));
			}
			else if (obj is IntVec2)
			{
				req.Rules.Add(new Rule_String(absoluteName, ((IntVec2)obj).ToStringCross()));
			}
			else
			{
				if (obj is IEnumerable && !(obj is string))
				{
					if (obj is IEnumerable<Thing>)
					{
						req.Rules.Add(new Rule_String(absoluteName, GenLabel.ThingsLabel(from x in (IEnumerable<Thing>)obj
						where x != null
						select x, "  - ")));
					}
					else if (obj is IEnumerable<Pawn>)
					{
						req.Rules.Add(new Rule_String(absoluteName, GenLabel.ThingsLabel((from x in (IEnumerable<Pawn>)obj
						where x != null
						select x).Cast<Thing>(), "  - ")));
					}
					else
					{
						if (obj is IEnumerable<object> && ((IEnumerable<object>)obj).Any<object>())
						{
							if (((IEnumerable<object>)obj).All((object x) => x is Thing))
							{
								req.Rules.Add(new Rule_String(absoluteName, GenLabel.ThingsLabel((from x in (IEnumerable<object>)obj
								where x != null
								select x).Cast<Thing>(), "  - ")));
								goto IL_336;
							}
						}
						List<string> list = new List<string>();
						foreach (object obj2 in ((IEnumerable)obj))
						{
							if (obj2 != null)
							{
								list.Add(obj2.ToString());
							}
						}
						req.Rules.Add(new Rule_String(absoluteName, list.ToCommaList(true)));
					}
					IL_336:
					req.Rules.Add(new Rule_String(absoluteName + "_count", ((IEnumerable)obj).EnumerableCount().ToString()));
					int num = 0;
					using (IEnumerator enumerator = ((IEnumerable)obj).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj3 = enumerator.Current;
							QuestGenUtility.AddSlateVar(ref req, absoluteName + num, obj3);
							num++;
						}
						goto IL_4E9;
					}
				}
				req.Rules.Add(new Rule_String(absoluteName, obj.ToString()));
				if (ConvertHelper.CanConvert<int>(obj))
				{
					req.Rules.Add(new Rule_String(absoluteName + "_duration", ConvertHelper.Convert<int>(obj).ToStringTicksToPeriod(true, false, true, false).Colorize(ColoredText.DateTimeColor)));
				}
				if (ConvertHelper.CanConvert<float>(obj))
				{
					req.Rules.Add(new Rule_String(absoluteName + "_money", ConvertHelper.Convert<float>(obj).ToStringMoney(null)));
				}
				if (ConvertHelper.CanConvert<float>(obj))
				{
					req.Rules.Add(new Rule_String(absoluteName + "_percent", ConvertHelper.Convert<float>(obj).ToStringPercent()));
				}
				if (ConvertHelper.CanConvert<FloatRange>(obj))
				{
					QuestGenUtility.AddSlateVar(ref req, absoluteName + "_average", ConvertHelper.Convert<FloatRange>(obj).Average);
				}
				if (ConvertHelper.CanConvert<FloatRange>(obj))
				{
					QuestGenUtility.AddSlateVar(ref req, absoluteName + "_min", ConvertHelper.Convert<FloatRange>(obj).min);
				}
				if (ConvertHelper.CanConvert<FloatRange>(obj))
				{
					QuestGenUtility.AddSlateVar(ref req, absoluteName + "_max", ConvertHelper.Convert<FloatRange>(obj).max);
				}
			}
			IL_4E9:
			if (obj is Def)
			{
				if (!req.Constants.ContainsKey(absoluteName))
				{
					req.Constants.Add(absoluteName, ((Def)obj).defName);
				}
			}
			else if (obj is Faction)
			{
				if (!req.Constants.ContainsKey(absoluteName))
				{
					req.Constants.Add(absoluteName, ((Faction)obj).def.defName);
				}
			}
			else if ((obj.GetType().IsPrimitive || obj is string || obj.GetType().IsEnum) && !req.Constants.ContainsKey(absoluteName))
			{
				req.Constants.Add(absoluteName, obj.ToString());
			}
			if (obj is IEnumerable && !(obj is string))
			{
				string key = absoluteName + "_count";
				if (!req.Constants.ContainsKey(key))
				{
					req.Constants.Add(key, ((IEnumerable)obj).EnumerableCount().ToString());
				}
			}
		}

		// Token: 0x0600A96D RID: 43373 RVA: 0x003182D0 File Offset: 0x003164D0
		public static string ResolveLocalTextWithDescriptionRules(RulePack localRules, string localRootKeyword = "root")
		{
			List<Rule> list = new List<Rule>();
			list.AddRange(QuestGen.QuestDescriptionRulesReadOnly);
			if (localRules != null)
			{
				list.AddRange(QuestGenUtility.AppendCurrentPrefix(localRules.Rules));
			}
			string text = localRootKeyword;
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				text = QuestGen.slate.CurrentPrefix + "/" + text;
			}
			text = QuestGenUtility.NormalizeVarPath(text);
			return QuestGenUtility.ResolveAbsoluteText(list, QuestGen.QuestDescriptionConstantsReadOnly, text, false);
		}

		// Token: 0x0600A96E RID: 43374 RVA: 0x0006F4AF File Offset: 0x0006D6AF
		public static string ResolveLocalText(RulePack localRules, string localRootKeyword = "root")
		{
			return QuestGenUtility.ResolveLocalText((localRules != null) ? localRules.Rules : null, null, localRootKeyword, true);
		}

		// Token: 0x0600A96F RID: 43375 RVA: 0x00318340 File Offset: 0x00316540
		public static string ResolveLocalText(List<Rule> localRules, Dictionary<string, string> localConstants = null, string localRootKeyword = "root", bool capitalizeFirstSentence = true)
		{
			string text = localRootKeyword;
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				text = QuestGen.slate.CurrentPrefix + "/" + text;
			}
			text = QuestGenUtility.NormalizeVarPath(text);
			return QuestGenUtility.ResolveAbsoluteText(QuestGenUtility.AppendCurrentPrefix(localRules), QuestGenUtility.AppendCurrentPrefix(localConstants), text, capitalizeFirstSentence);
		}

		// Token: 0x0600A970 RID: 43376 RVA: 0x00318390 File Offset: 0x00316590
		public static string ResolveAbsoluteText(List<Rule> absoluteRules, Dictionary<string, string> absoluteConstants = null, string absoluteRootKeyword = "root", bool capitalizeFirstSentence = true)
		{
			GrammarRequest request = default(GrammarRequest);
			if (absoluteRules != null)
			{
				request.Rules.AddRange(absoluteRules);
			}
			if (absoluteConstants != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in absoluteConstants)
				{
					request.Constants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			QuestGenUtility.AddSlateVars(ref request);
			return GrammarResolver.Resolve(absoluteRootKeyword, request, null, false, null, null, null, capitalizeFirstSentence);
		}

		// Token: 0x0600A971 RID: 43377 RVA: 0x00318420 File Offset: 0x00316620
		public static List<Rule> AppendCurrentPrefix(List<Rule> rules)
		{
			if (rules == null)
			{
				return null;
			}
			List<Rule> list = new List<Rule>();
			string currentPrefix = QuestGen.slate.CurrentPrefix;
			for (int i = 0; i < rules.Count; i++)
			{
				Rule rule = rules[i].DeepCopy();
				if (!currentPrefix.NullOrEmpty())
				{
					rule.keyword = currentPrefix + "/" + rule.keyword;
				}
				rule.keyword = QuestGenUtility.NormalizeVarPath(rule.keyword);
				Rule_String rule_String = rule as Rule_String;
				if (rule_String != null)
				{
					rule_String.AppendPrefixToAllKeywords(currentPrefix);
				}
				list.Add(rule);
			}
			return list;
		}

		// Token: 0x0600A972 RID: 43378 RVA: 0x003184B0 File Offset: 0x003166B0
		public static Dictionary<string, string> AppendCurrentPrefix(Dictionary<string, string> constants)
		{
			if (constants == null)
			{
				return null;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string currentPrefix = QuestGen.slate.CurrentPrefix;
			foreach (KeyValuePair<string, string> keyValuePair in constants)
			{
				string text = keyValuePair.Key;
				if (!currentPrefix.NullOrEmpty())
				{
					text = currentPrefix + "/" + text;
				}
				text = QuestGenUtility.NormalizeVarPath(text);
				dictionary.Add(text, keyValuePair.Value);
			}
			return dictionary;
		}

		// Token: 0x0600A973 RID: 43379 RVA: 0x0006F4C5 File Offset: 0x0006D6C5
		public static LookTargets ToLookTargets(SlateRef<IEnumerable<object>> objects, Slate slate)
		{
			if (objects.GetValue(slate) == null || !objects.GetValue(slate).Any<object>())
			{
				return LookTargets.Invalid;
			}
			return QuestGenUtility.ToLookTargets(objects.GetValue(slate));
		}

		// Token: 0x0600A974 RID: 43380 RVA: 0x00318548 File Offset: 0x00316748
		public static LookTargets ToLookTargets(IEnumerable<object> objects)
		{
			if (objects == null || !objects.Any<object>())
			{
				return LookTargets.Invalid;
			}
			LookTargets lookTargets = new LookTargets();
			foreach (object obj in objects)
			{
				if (obj is Thing)
				{
					lookTargets.targets.Add((Thing)obj);
				}
				else if (obj is WorldObject)
				{
					lookTargets.targets.Add((WorldObject)obj);
				}
				else if (obj is Map)
				{
					lookTargets.targets.Add(((Map)obj).Parent);
				}
			}
			return lookTargets;
		}

		// Token: 0x0600A975 RID: 43381 RVA: 0x00318604 File Offset: 0x00316804
		public static List<Rule> MergeRules(RulePack rules, string singleRule, string root)
		{
			List<Rule> list = new List<Rule>();
			if (rules != null)
			{
				list.AddRange(rules.Rules);
			}
			if (!singleRule.NullOrEmpty())
			{
				list.Add(new Rule_String(root, singleRule));
			}
			return list;
		}

		// Token: 0x0600A976 RID: 43382 RVA: 0x0031863C File Offset: 0x0031683C
		public static ChoiceLetter MakeLetter(string labelKeyword, string textKeyword, LetterDef def, Faction relatedFaction = null, Quest quest = null)
		{
			ChoiceLetter letter = LetterMaker.MakeLetter("error", "error", def, relatedFaction, quest);
			QuestGen.AddTextRequest(labelKeyword, delegate(string x)
			{
				letter.label = x;
			}, null);
			QuestGen.AddTextRequest(textKeyword, delegate(string x)
			{
				letter.text = x;
			}, null);
			return letter;
		}

		// Token: 0x0600A977 RID: 43383 RVA: 0x003186A0 File Offset: 0x003168A0
		public static ChoiceLetter MakeLetter(string labelKeyword, string textKeyword, LetterDef def, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null)
		{
			ChoiceLetter letter = LetterMaker.MakeLetter("error", "error", def, lookTargets, relatedFaction, quest, null);
			QuestGen.AddTextRequest(labelKeyword, delegate(string x)
			{
				letter.label = x;
			}, null);
			QuestGen.AddTextRequest(textKeyword, delegate(string x)
			{
				letter.text = x;
			}, null);
			return letter;
		}

		// Token: 0x0600A978 RID: 43384 RVA: 0x00318708 File Offset: 0x00316908
		public static void AddToOrMakeList(Slate slate, string name, object obj)
		{
			List<object> list;
			if (!slate.TryGet<List<object>>(name, out list, false))
			{
				list = new List<object>();
			}
			list.Add(obj);
			slate.Set<List<object>>(name, list, false);
		}

		// Token: 0x0600A979 RID: 43385 RVA: 0x00318738 File Offset: 0x00316938
		public static void AddRangeToOrMakeList(Slate slate, string name, List<object> objs)
		{
			if (objs.NullOrEmpty<object>())
			{
				return;
			}
			List<object> list;
			if (!slate.TryGet<List<object>>(name, out list, false))
			{
				list = new List<object>();
			}
			list.AddRange(objs);
			slate.Set<List<object>>(name, list, false);
		}

		// Token: 0x0600A97A RID: 43386 RVA: 0x00318770 File Offset: 0x00316970
		public static bool IsInList(Slate slate, string name, object obj)
		{
			List<object> list;
			return slate.TryGet<List<object>>(name, out list, false) && list != null && list.Contains(obj);
		}

		// Token: 0x0600A97B RID: 43387 RVA: 0x00318798 File Offset: 0x00316998
		public static List<Slate.VarRestoreInfo> SetVarsForPrefix(List<PrefixCapturedVar> capturedVars, string prefix, Slate slate)
		{
			if (capturedVars.NullOrEmpty<PrefixCapturedVar>())
			{
				return null;
			}
			if (prefix.NullOrEmpty())
			{
				List<Slate.VarRestoreInfo> list = new List<Slate.VarRestoreInfo>();
				for (int i = 0; i < capturedVars.Count; i++)
				{
					list.Add(slate.GetRestoreInfo(capturedVars[i].name));
				}
				for (int j = 0; j < capturedVars.Count; j++)
				{
					object obj;
					if (capturedVars[j].value.TryGetValue(slate, out obj))
					{
						if (capturedVars[j].name == "inSignal" && obj is string)
						{
							obj = QuestGenUtility.HardcodedSignalWithQuestID((string)obj);
						}
						slate.Set<object>(capturedVars[j].name, obj, false);
					}
				}
				return list;
			}
			for (int k = 0; k < capturedVars.Count; k++)
			{
				object obj2;
				if (capturedVars[k].value.TryGetValue(slate, out obj2))
				{
					if (capturedVars[k].name == "inSignal" && obj2 is string)
					{
						obj2 = QuestGenUtility.HardcodedSignalWithQuestID((string)obj2);
					}
					string name = prefix + "/" + capturedVars[k].name;
					slate.Set<object>(name, obj2, false);
				}
			}
			return null;
		}

		// Token: 0x0600A97C RID: 43388 RVA: 0x003188D4 File Offset: 0x00316AD4
		public static void RestoreVarsForPrefix(List<Slate.VarRestoreInfo> varsRestoreInfo, Slate slate)
		{
			if (varsRestoreInfo.NullOrEmpty<Slate.VarRestoreInfo>())
			{
				return;
			}
			for (int i = 0; i < varsRestoreInfo.Count; i++)
			{
				slate.Restore(varsRestoreInfo[i]);
			}
		}

		// Token: 0x0600A97D RID: 43389 RVA: 0x00318908 File Offset: 0x00316B08
		public static void GetReturnedVars(List<SlateRef<string>> varNames, string prefix, Slate slate)
		{
			if (varNames.NullOrEmpty<SlateRef<string>>())
			{
				return;
			}
			if (prefix.NullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < varNames.Count; i++)
			{
				string name = prefix + "/" + varNames[i].GetValue(slate);
				object var;
				if (slate.TryGet<object>(name, out var, false))
				{
					slate.Set<object>(varNames[i].GetValue(slate), var, false);
				}
			}
		}

		// Token: 0x0600A97E RID: 43390 RVA: 0x00318978 File Offset: 0x00316B78
		public static string NormalizeVarPath(string path)
		{
			if (path.NullOrEmpty())
			{
				return path;
			}
			if (!path.Contains(".."))
			{
				return path;
			}
			QuestGenUtility.tmpSb.Length = 0;
			QuestGenUtility.tmpPathParts.Clear();
			for (int i = 0; i < path.Length; i++)
			{
				if (path[i] == '/')
				{
					QuestGenUtility.tmpPathParts.Add(QuestGenUtility.tmpSb.ToString());
					QuestGenUtility.tmpSb.Length = 0;
				}
				else
				{
					QuestGenUtility.tmpSb.Append(path[i]);
				}
			}
			if (QuestGenUtility.tmpSb.Length != 0)
			{
				QuestGenUtility.tmpPathParts.Add(QuestGenUtility.tmpSb.ToString());
			}
			for (int j = 0; j < QuestGenUtility.tmpPathParts.Count; j++)
			{
				while (j < QuestGenUtility.tmpPathParts.Count && QuestGenUtility.tmpPathParts[j] == "..")
				{
					if (j == 0)
					{
						QuestGenUtility.tmpPathParts.RemoveAt(0);
					}
					else
					{
						QuestGenUtility.tmpPathParts.RemoveAt(j);
						QuestGenUtility.tmpPathParts.RemoveAt(j - 1);
						j--;
					}
				}
			}
			QuestGenUtility.tmpSb.Length = 0;
			for (int k = 0; k < QuestGenUtility.tmpPathParts.Count; k++)
			{
				if (k != 0)
				{
					QuestGenUtility.tmpSb.Append('/');
				}
				QuestGenUtility.tmpSb.Append(QuestGenUtility.tmpPathParts[k]);
			}
			return QuestGenUtility.tmpSb.ToString();
		}

		// Token: 0x040072BF RID: 29375
		public const string OuterNodeCompletedSignal = "OuterNodeCompleted";

		// Token: 0x040072C0 RID: 29376
		private static HashSet<string> tmpAddedSlateVars = new HashSet<string>();

		// Token: 0x040072C1 RID: 29377
		private static StringBuilder tmpSymbol = new StringBuilder();

		// Token: 0x040072C2 RID: 29378
		private static StringBuilder tmpVarAbsoluteName = new StringBuilder();

		// Token: 0x040072C3 RID: 29379
		private static List<string> tmpPathParts = new List<string>();

		// Token: 0x040072C4 RID: 29380
		private static StringBuilder tmpSb = new StringBuilder();
	}
}
