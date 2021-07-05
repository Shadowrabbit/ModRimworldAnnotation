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
	// Token: 0x0200160D RID: 5645
	public static class QuestGenUtility
	{
		// Token: 0x06008432 RID: 33842 RVA: 0x002F597C File Offset: 0x002F3B7C
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

		// Token: 0x06008433 RID: 33843 RVA: 0x002F5A5C File Offset: 0x002F3C5C
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

		// Token: 0x06008434 RID: 33844 RVA: 0x002F5ACD File Offset: 0x002F3CCD
		public static string QuestTagSignal(string questTag, string signal)
		{
			return questTag + "." + signal;
		}

		// Token: 0x06008435 RID: 33845 RVA: 0x002F5ADC File Offset: 0x002F3CDC
		public static void RunInner(Action inner, QuestPartActivable outerQuestPart)
		{
			string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
			outerQuestPart.outSignalsCompleted.Add(text);
			QuestGenUtility.RunInner(inner, text);
		}

		// Token: 0x06008436 RID: 33846 RVA: 0x002F5B08 File Offset: 0x002F3D08
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

		// Token: 0x06008437 RID: 33847 RVA: 0x002F5B5C File Offset: 0x002F3D5C
		public static void RunInnerNode(QuestNode node, QuestPartActivable outerQuestPart)
		{
			string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
			outerQuestPart.outSignalsCompleted.Add(text);
			QuestGenUtility.RunInnerNode(node, text);
		}

		// Token: 0x06008438 RID: 33848 RVA: 0x002F5B88 File Offset: 0x002F3D88
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

		// Token: 0x06008439 RID: 33849 RVA: 0x002F5BDC File Offset: 0x002F3DDC
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

		// Token: 0x0600843A RID: 33850 RVA: 0x002F5CF0 File Offset: 0x002F3EF0
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

		// Token: 0x0600843B RID: 33851 RVA: 0x002F5DE0 File Offset: 0x002F3FE0
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
				req.Rules.AddRange(GrammarUtility.RulesForFaction(absoluteName, faction, req.Constants, true));
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
					req.Rules.AddRange(GrammarUtility.RulesForFaction(absoluteName + "_faction", pawn.Faction, req.Constants, true));
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
								goto IL_382;
							}
						}
						IEnumerable<WorldObject> source;
						if ((source = (obj as IEnumerable<WorldObject>)) != null)
						{
							req.Rules.Add(new Rule_String(absoluteName, source.Select(new Func<WorldObject, string>(QuestGenUtility.<>c.<>9.<AddSlateVar>g__PossiblyWithTags|13_4)).ToCommaList(true, false)));
						}
						else
						{
							List<string> list = new List<string>();
							foreach (object obj2 in ((IEnumerable)obj))
							{
								if (obj2 != null)
								{
									list.Add(obj2.ToString());
								}
							}
							req.Rules.Add(new Rule_String(absoluteName, list.ToCommaList(true, false)));
						}
					}
					IL_382:
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
						goto IL_57C;
					}
				}
				Precept precept;
				if (obj is Ideo)
				{
					req.Rules.AddRange(GrammarUtility.RulesForIdeo(absoluteName, (Ideo)obj));
				}
				else if ((precept = (obj as Precept)) != null)
				{
					req.Rules.AddRange(GrammarUtility.RulesForPrecept(absoluteName, precept));
				}
				else
				{
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
			}
			IL_57C:
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

		// Token: 0x0600843C RID: 33852 RVA: 0x002F647C File Offset: 0x002F467C
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

		// Token: 0x0600843D RID: 33853 RVA: 0x002F64EB File Offset: 0x002F46EB
		public static string ResolveLocalText(RulePack localRules, string localRootKeyword = "root")
		{
			return QuestGenUtility.ResolveLocalText((localRules != null) ? localRules.Rules : null, null, localRootKeyword, true);
		}

		// Token: 0x0600843E RID: 33854 RVA: 0x002F6504 File Offset: 0x002F4704
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

		// Token: 0x0600843F RID: 33855 RVA: 0x002F6554 File Offset: 0x002F4754
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

		// Token: 0x06008440 RID: 33856 RVA: 0x002F65E4 File Offset: 0x002F47E4
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

		// Token: 0x06008441 RID: 33857 RVA: 0x002F6674 File Offset: 0x002F4874
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

		// Token: 0x06008442 RID: 33858 RVA: 0x002F670C File Offset: 0x002F490C
		public static LookTargets ToLookTargets(SlateRef<IEnumerable<object>> objects, Slate slate)
		{
			if (objects.GetValue(slate) == null || !objects.GetValue(slate).Any<object>())
			{
				return LookTargets.Invalid;
			}
			return QuestGenUtility.ToLookTargets(objects.GetValue(slate));
		}

		// Token: 0x06008443 RID: 33859 RVA: 0x002F673C File Offset: 0x002F493C
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

		// Token: 0x06008444 RID: 33860 RVA: 0x002F67F8 File Offset: 0x002F49F8
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

		// Token: 0x06008445 RID: 33861 RVA: 0x002F6830 File Offset: 0x002F4A30
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

		// Token: 0x06008446 RID: 33862 RVA: 0x002F6894 File Offset: 0x002F4A94
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

		// Token: 0x06008447 RID: 33863 RVA: 0x002F68FC File Offset: 0x002F4AFC
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

		// Token: 0x06008448 RID: 33864 RVA: 0x002F692C File Offset: 0x002F4B2C
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

		// Token: 0x06008449 RID: 33865 RVA: 0x002F6964 File Offset: 0x002F4B64
		public static bool IsInList(Slate slate, string name, object obj)
		{
			List<object> list;
			return slate.TryGet<List<object>>(name, out list, false) && list != null && list.Contains(obj);
		}

		// Token: 0x0600844A RID: 33866 RVA: 0x002F698C File Offset: 0x002F4B8C
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

		// Token: 0x0600844B RID: 33867 RVA: 0x002F6AC8 File Offset: 0x002F4CC8
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

		// Token: 0x0600844C RID: 33868 RVA: 0x002F6AFC File Offset: 0x002F4CFC
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

		// Token: 0x0600844D RID: 33869 RVA: 0x002F6B6C File Offset: 0x002F4D6C
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

		// Token: 0x0600844E RID: 33870 RVA: 0x002F6CCB File Offset: 0x002F4ECB
		public static void RunAdjustPointsForDistantFight()
		{
			QuestScriptDefOf.Util_AdjustPointsForDistantFight.root.Run();
		}

		// Token: 0x0600844F RID: 33871 RVA: 0x002F6CDC File Offset: 0x002F4EDC
		public static void TestRunAdjustPointsForDistantFight(Slate slate)
		{
			QuestScriptDefOf.Util_AdjustPointsForDistantFight.root.TestRun(slate);
		}

		// Token: 0x04005268 RID: 21096
		public const string OuterNodeCompletedSignal = "OuterNodeCompleted";

		// Token: 0x04005269 RID: 21097
		private static HashSet<string> tmpAddedSlateVars = new HashSet<string>();

		// Token: 0x0400526A RID: 21098
		private static StringBuilder tmpSymbol = new StringBuilder();

		// Token: 0x0400526B RID: 21099
		private static StringBuilder tmpVarAbsoluteName = new StringBuilder();

		// Token: 0x0400526C RID: 21100
		private static List<string> tmpPathParts = new List<string>();

		// Token: 0x0400526D RID: 21101
		private static StringBuilder tmpSb = new StringBuilder();
	}
}
