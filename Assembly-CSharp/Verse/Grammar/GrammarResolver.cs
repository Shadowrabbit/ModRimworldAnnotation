using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse.Grammar
{
	// Token: 0x020008F7 RID: 2295
	public static class GrammarResolver
	{
		// Token: 0x06003909 RID: 14601 RVA: 0x001645F0 File Offset: 0x001627F0
		public static string Resolve(string rootKeyword, GrammarRequest request, string debugLabel = null, bool forceLog = false, string untranslatedRootKeyword = null, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
			{
				return GrammarResolver.ResolveUnsafe(rootKeyword, request, debugLabel, forceLog, false, extraTags, outTags, capitalizeFirstSentence);
			}
			bool flag;
			string text;
			Exception ex;
			try
			{
				text = GrammarResolver.ResolveUnsafe(rootKeyword, request, out flag, debugLabel, forceLog, false, extraTags, outTags, capitalizeFirstSentence);
				ex = null;
			}
			catch (Exception ex2)
			{
				flag = false;
				text = "";
				ex = ex2;
			}
			if (flag)
			{
				return text;
			}
			string text2 = "Failed to resolve text. Trying again with English.";
			if (ex != null)
			{
				text2 = text2 + " Exception: " + ex;
			}
			Log.ErrorOnce(text2, text.GetHashCode(), false);
			if (outTags != null)
			{
				outTags.Clear();
			}
			return GrammarResolver.ResolveUnsafe(untranslatedRootKeyword ?? rootKeyword, request, out flag, debugLabel, forceLog, true, extraTags, outTags, capitalizeFirstSentence);
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x00164698 File Offset: 0x00162898
		public static string ResolveUnsafe(string rootKeyword, GrammarRequest request, string debugLabel = null, bool forceLog = false, bool useUntranslatedRules = false, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			bool flag;
			return GrammarResolver.ResolveUnsafe(rootKeyword, request, out flag, debugLabel, forceLog, useUntranslatedRules, extraTags, outTags, capitalizeFirstSentence);
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x001646B8 File Offset: 0x001628B8
		public static string ResolveUnsafe(string rootKeyword, GrammarRequest request, out bool success, string debugLabel = null, bool forceLog = false, bool useUntranslatedRules = false, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			bool flag = forceLog || DebugViewSettings.logGrammarResolution;
			GrammarResolver.rules.Clear();
			GrammarResolver.rulePool.Clear();
			if (flag)
			{
				GrammarResolver.logSbTrace = new StringBuilder();
				GrammarResolver.logSbMid = new StringBuilder();
				GrammarResolver.logSbRules = new StringBuilder();
			}
			List<Rule> rulesAllowNull = request.RulesAllowNull;
			if (rulesAllowNull != null)
			{
				if (flag)
				{
					GrammarResolver.logSbRules.AppendLine("CUSTOM RULES");
				}
				for (int i = 0; i < rulesAllowNull.Count; i++)
				{
					GrammarResolver.AddRule(rulesAllowNull[i]);
					if (flag)
					{
						GrammarResolver.logSbRules.AppendLine("■" + rulesAllowNull[i].ToString());
					}
				}
				if (flag)
				{
					GrammarResolver.logSbRules.AppendLine();
				}
			}
			List<RulePackDef> includesAllowNull = request.IncludesAllowNull;
			if (includesAllowNull != null)
			{
				HashSet<RulePackDef> hashSet = new HashSet<RulePackDef>();
				List<RulePackDef> list = new List<RulePackDef>(includesAllowNull);
				if (flag)
				{
					GrammarResolver.logSbMid.AppendLine("INCLUDES");
				}
				while (list.Count > 0)
				{
					RulePackDef rulePackDef = list[list.Count - 1];
					list.RemoveLast<RulePackDef>();
					if (!hashSet.Contains(rulePackDef))
					{
						if (flag)
						{
							GrammarResolver.logSbMid.AppendLine(string.Format("{0}", rulePackDef.defName));
						}
						hashSet.Add(rulePackDef);
						List<Rule> list2 = useUntranslatedRules ? rulePackDef.UntranslatedRulesImmediate : rulePackDef.RulesImmediate;
						if (list2 != null)
						{
							foreach (Rule rule in list2)
							{
								GrammarResolver.AddRule(rule);
							}
						}
						if (!rulePackDef.include.NullOrEmpty<RulePackDef>())
						{
							list.AddRange(rulePackDef.include);
						}
					}
				}
			}
			List<RulePack> includesBareAllowNull = request.IncludesBareAllowNull;
			if (includesBareAllowNull != null)
			{
				if (flag)
				{
					GrammarResolver.logSbMid.AppendLine();
					GrammarResolver.logSbMid.AppendLine("BARE INCLUDES");
				}
				for (int j = 0; j < includesBareAllowNull.Count; j++)
				{
					List<Rule> list3 = useUntranslatedRules ? includesBareAllowNull[j].UntranslatedRules : includesBareAllowNull[j].Rules;
					for (int k = 0; k < list3.Count; k++)
					{
						GrammarResolver.AddRule(list3[k]);
						if (flag)
						{
							GrammarResolver.logSbMid.AppendLine("  " + list3[k].ToString());
						}
					}
				}
			}
			if (flag && !extraTags.NullOrEmpty<string>())
			{
				GrammarResolver.logSbMid.AppendLine();
				GrammarResolver.logSbMid.AppendLine("EXTRA TAGS");
				for (int l = 0; l < extraTags.Count; l++)
				{
					GrammarResolver.logSbMid.AppendLine("  " + extraTags[l]);
				}
			}
			List<Rule> list4 = useUntranslatedRules ? RulePackDefOf.GlobalUtility.UntranslatedRulesPlusIncludes : RulePackDefOf.GlobalUtility.RulesPlusIncludes;
			for (int m = 0; m < list4.Count; m++)
			{
				GrammarResolver.AddRule(list4[m]);
			}
			GrammarResolver.loopCount = 0;
			Dictionary<string, string> constantsAllowNull = request.ConstantsAllowNull;
			if (flag && constantsAllowNull != null)
			{
				GrammarResolver.logSbMid.AppendLine("CONSTANTS");
				foreach (KeyValuePair<string, string> keyValuePair in constantsAllowNull)
				{
					GrammarResolver.logSbMid.AppendLine(keyValuePair.Key.PadRight(38) + " " + keyValuePair.Value);
				}
			}
			if (flag)
			{
				GrammarResolver.logSbTrace.Append("GRAMMAR RESOLUTION TRACE");
			}
			string text = "err";
			bool flag2 = false;
			List<string> list5 = new List<string>();
			if (GrammarResolver.TryResolveRecursive(new GrammarResolver.RuleEntry(new Rule_String("", "[" + rootKeyword + "]")), 0, constantsAllowNull, out text, flag, extraTags, list5))
			{
				if (outTags != null)
				{
					outTags.Clear();
					outTags.AddRange(list5);
				}
			}
			else
			{
				flag2 = true;
				if (request.Rules.NullOrEmpty<Rule>())
				{
					text = "ERR";
				}
				else
				{
					text = "ERR: " + request.Rules[0].Generate();
				}
				if (flag)
				{
					GrammarResolver.logSbTrace.Insert(0, "Grammar unresolvable. Root '" + rootKeyword + "'\n\n");
				}
				else
				{
					GrammarResolver.ResolveUnsafe(rootKeyword, request, debugLabel, true, useUntranslatedRules, extraTags, null, true);
				}
			}
			text = GenText.CapitalizeSentences(Find.ActiveLanguageWorker.PostProcessed(text), capitalizeFirstSentence);
			text = GrammarResolver.Spaces.Replace(text, (Match match) => match.Groups[1].Value);
			text = text.Trim();
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(GrammarResolver.logSbTrace.ToString().TrimEndNewlines());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append(GrammarResolver.logSbMid.ToString().TrimEndNewlines());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append(GrammarResolver.logSbRules.ToString().TrimEndNewlines());
				if (flag2)
				{
					if (DebugViewSettings.logGrammarResolution)
					{
						Log.Error(stringBuilder.ToString().Trim() + "\n", false);
					}
					else
					{
						Log.ErrorOnce(stringBuilder.ToString().Trim() + "\n", stringBuilder.ToString().Trim().GetHashCode(), false);
					}
				}
				else
				{
					Log.Message(stringBuilder.ToString().Trim() + "\n", false);
				}
				GrammarResolver.logSbTrace = null;
				GrammarResolver.logSbMid = null;
				GrammarResolver.logSbRules = null;
			}
			success = !flag2;
			return text;
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x00164C74 File Offset: 0x00162E74
		private static void AddRule(Rule rule)
		{
			List<GrammarResolver.RuleEntry> list = null;
			if (!GrammarResolver.rules.TryGetValue(rule.keyword, out list))
			{
				list = GrammarResolver.rulePool.Get();
				list.Clear();
				GrammarResolver.rules[rule.keyword] = list;
			}
			list.Add(new GrammarResolver.RuleEntry(rule));
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x00164CC8 File Offset: 0x00162EC8
		private static bool TryResolveRecursive(GrammarResolver.RuleEntry entry, int depth, Dictionary<string, string> constants, out string output, bool log, List<string> extraTags, List<string> resolvedTags)
		{
			string text = "";
			for (int i = 0; i < depth; i++)
			{
				text += "  ";
			}
			if (log && depth > 0)
			{
				GrammarResolver.logSbTrace.AppendLine();
				GrammarResolver.logSbTrace.Append(depth.ToStringCached().PadRight(3));
				GrammarResolver.logSbTrace.Append(text + entry);
			}
			text += "     ";
			GrammarResolver.loopCount++;
			if (GrammarResolver.loopCount > 1000)
			{
				Log.Error("Hit loops limit resolving grammar.", false);
				output = "HIT_LOOPS_LIMIT";
				if (log)
				{
					GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Hit loops limit");
				}
				return false;
			}
			if (depth > 50)
			{
				Log.Error("Grammar recurred too deep while resolving keyword (>" + 50 + " deep)", false);
				output = "DEPTH_LIMIT_REACHED";
				if (log)
				{
					GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Depth limit reached");
				}
				return false;
			}
			string text2 = entry.rule.Generate();
			bool flag = false;
			int num = -1;
			for (int j = 0; j < text2.Length; j++)
			{
				char c = text2[j];
				if (c == '[')
				{
					num = j;
				}
				if (c == ']')
				{
					if (num == -1)
					{
						Log.Error("Could not resolve rule because of mismatched brackets: " + text2, false);
						output = "MISMATCHED_BRACKETS";
						if (log)
						{
							GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Mismatched brackets");
						}
						flag = true;
					}
					else
					{
						string text3 = text2.Substring(num + 1, j - num - 1);
						GrammarResolver.RuleEntry ruleEntry;
						List<string> list;
						string str;
						for (;;)
						{
							ruleEntry = GrammarResolver.RandomPossiblyResolvableEntry(text3, constants, extraTags, resolvedTags);
							if (ruleEntry == null)
							{
								break;
							}
							ruleEntry.uses++;
							list = resolvedTags.ToList<string>();
							if (GrammarResolver.TryResolveRecursive(ruleEntry, depth + 1, constants, out str, log, extraTags, list))
							{
								goto Block_14;
							}
							ruleEntry.MarkKnownUnresolvable();
						}
						entry.MarkKnownUnresolvable();
						output = "CANNOT_RESOLVE_SUBSYMBOL:" + text3;
						if (log)
						{
							GrammarResolver.logSbTrace.Append("\n" + text + text3 + " → UNRESOLVABLE");
						}
						flag = true;
						goto IL_271;
						Block_14:
						text2 = text2.Substring(0, num) + str + text2.Substring(j + 1);
						j = num;
						resolvedTags.Clear();
						resolvedTags.AddRange(list);
						if (!ruleEntry.rule.tag.NullOrEmpty() && !resolvedTags.Contains(ruleEntry.rule.tag))
						{
							resolvedTags.Add(ruleEntry.rule.tag);
						}
					}
				}
				IL_271:;
			}
			output = text2;
			return !flag;
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x00164F60 File Offset: 0x00163160
		private static GrammarResolver.RuleEntry RandomPossiblyResolvableEntry(string keyword, Dictionary<string, string> constants, List<string> extraTags, List<string> resolvedTags)
		{
			List<GrammarResolver.RuleEntry> list = GrammarResolver.rules.TryGetValue(keyword, null);
			if (list == null)
			{
				return null;
			}
			float maxPriority = float.MinValue;
			for (int i = 0; i < list.Count; i++)
			{
				GrammarResolver.RuleEntry ruleEntry = list[i];
				if (!ruleEntry.knownUnresolvable && ruleEntry.ValidateConstantConstraints(constants) && ruleEntry.ValidateRequiredTag(extraTags, resolvedTags) && ruleEntry.SelectionWeight != 0f)
				{
					maxPriority = Mathf.Max(maxPriority, ruleEntry.Priority);
				}
			}
			return list.RandomElementByWeightWithFallback(delegate(GrammarResolver.RuleEntry rule)
			{
				if (rule.knownUnresolvable || !rule.ValidateConstantConstraints(constants) || !rule.ValidateRequiredTag(extraTags, resolvedTags) || rule.Priority != maxPriority)
				{
					return 0f;
				}
				return rule.SelectionWeight;
			}, null);
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0002C20B File Offset: 0x0002A40B
		public static bool ContainsSpecialChars(string str)
		{
			return str.IndexOfAny(GrammarResolver.SpecialChars) >= 0;
		}

		// Token: 0x04002743 RID: 10051
		private static SimpleLinearPool<List<GrammarResolver.RuleEntry>> rulePool = new SimpleLinearPool<List<GrammarResolver.RuleEntry>>();

		// Token: 0x04002744 RID: 10052
		private static Dictionary<string, List<GrammarResolver.RuleEntry>> rules = new Dictionary<string, List<GrammarResolver.RuleEntry>>();

		// Token: 0x04002745 RID: 10053
		private static int loopCount;

		// Token: 0x04002746 RID: 10054
		private static StringBuilder logSbTrace;

		// Token: 0x04002747 RID: 10055
		private static StringBuilder logSbMid;

		// Token: 0x04002748 RID: 10056
		private static StringBuilder logSbRules;

		// Token: 0x04002749 RID: 10057
		private const int DepthLimit = 50;

		// Token: 0x0400274A RID: 10058
		private const int LoopsLimit = 1000;

		// Token: 0x0400274B RID: 10059
		private static Regex Spaces = new Regex(" +([,.])");

		// Token: 0x0400274C RID: 10060
		public const char SymbolStartChar = '[';

		// Token: 0x0400274D RID: 10061
		public const char SymbolEndChar = ']';

		// Token: 0x0400274E RID: 10062
		private static readonly char[] SpecialChars = new char[]
		{
			'[',
			']',
			'{',
			'}'
		};

		// Token: 0x020008F8 RID: 2296
		private class RuleEntry
		{
			// Token: 0x1700090D RID: 2317
			// (get) Token: 0x06003911 RID: 14609 RVA: 0x0002C259 File Offset: 0x0002A459
			public float SelectionWeight
			{
				get
				{
					return this.rule.BaseSelectionWeight * 100000f / (float)((this.uses + 1) * 1000);
				}
			}

			// Token: 0x1700090E RID: 2318
			// (get) Token: 0x06003912 RID: 14610 RVA: 0x0002C27C File Offset: 0x0002A47C
			public float Priority
			{
				get
				{
					return this.rule.Priority;
				}
			}

			// Token: 0x06003913 RID: 14611 RVA: 0x0002C289 File Offset: 0x0002A489
			public RuleEntry(Rule rule)
			{
				this.rule = rule;
				this.knownUnresolvable = false;
			}

			// Token: 0x06003914 RID: 14612 RVA: 0x0002C29F File Offset: 0x0002A49F
			public void MarkKnownUnresolvable()
			{
				this.knownUnresolvable = true;
			}

			// Token: 0x06003915 RID: 14613 RVA: 0x00165020 File Offset: 0x00163220
			public bool ValidateConstantConstraints(Dictionary<string, string> constraints)
			{
				if (!this.constantConstraintsChecked)
				{
					this.constantConstraintsValid = true;
					if (this.rule.constantConstraints != null)
					{
						for (int i = 0; i < this.rule.constantConstraints.Count; i++)
						{
							Rule.ConstantConstraint constantConstraint = this.rule.constantConstraints[i];
							string text = (constraints != null) ? constraints.TryGetValue(constantConstraint.key, "") : "";
							float num = 0f;
							float num2 = 0f;
							bool flag = !text.NullOrEmpty() && !constantConstraint.value.NullOrEmpty() && float.TryParse(text, out num) && float.TryParse(constantConstraint.value, out num2);
							bool flag2;
							switch (constantConstraint.type)
							{
							case Rule.ConstantConstraint.Type.Equal:
								flag2 = text.EqualsIgnoreCase(constantConstraint.value);
								break;
							case Rule.ConstantConstraint.Type.NotEqual:
								flag2 = !text.EqualsIgnoreCase(constantConstraint.value);
								break;
							case Rule.ConstantConstraint.Type.Less:
								flag2 = (flag && num < num2);
								break;
							case Rule.ConstantConstraint.Type.Greater:
								flag2 = (flag && num > num2);
								break;
							case Rule.ConstantConstraint.Type.LessOrEqual:
								flag2 = (flag && num <= num2);
								break;
							case Rule.ConstantConstraint.Type.GreaterOrEqual:
								flag2 = (flag && num >= num2);
								break;
							default:
								Log.Error("Unknown ConstantConstraint type: " + constantConstraint.type, false);
								flag2 = false;
								break;
							}
							if (!flag2)
							{
								this.constantConstraintsValid = false;
								break;
							}
						}
					}
					this.constantConstraintsChecked = true;
				}
				return this.constantConstraintsValid;
			}

			// Token: 0x06003916 RID: 14614 RVA: 0x0002C2A8 File Offset: 0x0002A4A8
			public bool ValidateRequiredTag(List<string> extraTags, List<string> resolvedTags)
			{
				return this.rule.requiredTag.NullOrEmpty() || (extraTags != null && extraTags.Contains(this.rule.requiredTag)) || resolvedTags.Contains(this.rule.requiredTag);
			}

			// Token: 0x06003917 RID: 14615 RVA: 0x0002C2E7 File Offset: 0x0002A4E7
			public override string ToString()
			{
				return this.rule.ToString();
			}

			// Token: 0x0400274F RID: 10063
			public Rule rule;

			// Token: 0x04002750 RID: 10064
			public bool knownUnresolvable;

			// Token: 0x04002751 RID: 10065
			public bool constantConstraintsChecked;

			// Token: 0x04002752 RID: 10066
			public bool constantConstraintsValid;

			// Token: 0x04002753 RID: 10067
			public int uses;
		}
	}
}
