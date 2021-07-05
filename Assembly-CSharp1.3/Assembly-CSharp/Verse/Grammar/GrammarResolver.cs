using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse.Grammar
{
	// Token: 0x02000535 RID: 1333
	public static class GrammarResolver
	{
		// Token: 0x06002829 RID: 10281 RVA: 0x000F4AF4 File Offset: 0x000F2CF4
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
			Log.ErrorOnce(text2, text.GetHashCode());
			if (outTags != null)
			{
				outTags.Clear();
			}
			return GrammarResolver.ResolveUnsafe(untranslatedRootKeyword ?? rootKeyword, request, out flag, debugLabel, forceLog, true, extraTags, outTags, capitalizeFirstSentence);
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x000F4B9C File Offset: 0x000F2D9C
		public static string ResolveUnsafe(string rootKeyword, GrammarRequest request, string debugLabel = null, bool forceLog = false, bool useUntranslatedRules = false, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			bool flag;
			return GrammarResolver.ResolveUnsafe(rootKeyword, request, out flag, debugLabel, forceLog, useUntranslatedRules, extraTags, outTags, capitalizeFirstSentence);
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000F4BBC File Offset: 0x000F2DBC
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
			if (GrammarResolver.TryResolveRecursive(new GrammarResolver.RuleEntry(new Rule_String("", "[" + rootKeyword + "]")), 0, constantsAllowNull, out text, flag, extraTags, list5, request.customizer))
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
						Log.Error(stringBuilder.ToString().Trim() + "\n");
					}
					else
					{
						Log.ErrorOnce(stringBuilder.ToString().Trim() + "\n", stringBuilder.ToString().Trim().GetHashCode());
					}
				}
				else
				{
					Log.Message(stringBuilder.ToString().Trim() + "\n");
				}
				GrammarResolver.logSbTrace = null;
				GrammarResolver.logSbMid = null;
				GrammarResolver.logSbRules = null;
			}
			success = !flag2;
			return text;
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000F5178 File Offset: 0x000F3378
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

		// Token: 0x0600282D RID: 10285 RVA: 0x000F51CC File Offset: 0x000F33CC
		private static bool TryResolveRecursive(GrammarResolver.RuleEntry entry, int depth, Dictionary<string, string> constants, out string output, bool log, List<string> extraTags, List<string> resolvedTags, GrammarRequest.ICustomizer customizer)
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
				Log.Error("Hit loops limit resolving grammar.");
				output = "HIT_LOOPS_LIMIT";
				if (log)
				{
					GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Hit loops limit");
				}
				return false;
			}
			if (depth > 50)
			{
				Log.Error("Grammar recurred too deep while resolving keyword (>" + 50 + " deep)");
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
						Log.Error("Could not resolve rule because of mismatched brackets: " + text2);
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
							ruleEntry = GrammarResolver.RandomPossiblyResolvableEntry(text3, constants, extraTags, resolvedTags, customizer);
							if (ruleEntry == null)
							{
								break;
							}
							ruleEntry.uses++;
							list = resolvedTags.ToList<string>();
							if (GrammarResolver.TryResolveRecursive(ruleEntry, depth + 1, constants, out str, log, extraTags, list, customizer))
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
						goto IL_284;
						Block_14:
						text2 = text2.Substring(0, num) + str + text2.Substring(j + 1);
						j = num;
						resolvedTags.Clear();
						resolvedTags.AddRange(list);
						if (!ruleEntry.rule.tag.NullOrEmpty() && !resolvedTags.Contains(ruleEntry.rule.tag))
						{
							resolvedTags.Add(ruleEntry.rule.tag);
						}
						if (customizer != null)
						{
							customizer.Notify_RuleUsed(ruleEntry.rule);
						}
					}
				}
				IL_284:;
			}
			output = text2;
			return !flag;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000F5478 File Offset: 0x000F3678
		private static GrammarResolver.RuleEntry RandomPossiblyResolvableEntry(string keyword, Dictionary<string, string> constants, List<string> extraTags, List<string> resolvedTags, GrammarRequest.ICustomizer customizer)
		{
			GrammarResolver.<>c__DisplayClass18_0 CS$<>8__locals1 = new GrammarResolver.<>c__DisplayClass18_0();
			CS$<>8__locals1.constants = constants;
			CS$<>8__locals1.extraTags = extraTags;
			CS$<>8__locals1.resolvedTags = resolvedTags;
			CS$<>8__locals1.customizer = customizer;
			List<GrammarResolver.RuleEntry> list = GrammarResolver.rules.TryGetValue(keyword, null);
			if (list == null)
			{
				return null;
			}
			CS$<>8__locals1.maxPriority = float.MinValue;
			for (int i = 0; i < list.Count; i++)
			{
				GrammarResolver.RuleEntry ruleEntry = list[i];
				if (GrammarResolver.ValidateRule(CS$<>8__locals1.constants, CS$<>8__locals1.extraTags, CS$<>8__locals1.resolvedTags, ruleEntry, CS$<>8__locals1.customizer) && ruleEntry.SelectionWeight != 0f)
				{
					CS$<>8__locals1.maxPriority = Mathf.Max(CS$<>8__locals1.maxPriority, ruleEntry.Priority);
				}
			}
			GrammarResolver.<>c__DisplayClass18_0 CS$<>8__locals2 = CS$<>8__locals1;
			GrammarRequest.ICustomizer customizer2 = CS$<>8__locals1.customizer;
			CS$<>8__locals2.customComparer = ((customizer2 != null) ? customizer2.StrictRulePrioritizer() : null);
			if (CS$<>8__locals1.customComparer != null && list.Count > 1)
			{
				IComparer<GrammarResolver.RuleEntry> comparer = Comparer<GrammarResolver.RuleEntry>.Create((GrammarResolver.RuleEntry a, GrammarResolver.RuleEntry b) => CS$<>8__locals1.customComparer.Compare(a.rule, b.rule)).ThenBy(Comparer<GrammarResolver.RuleEntry>.Create((GrammarResolver.RuleEntry a, GrammarResolver.RuleEntry b) => a.SelectionWeight.CompareTo(b.SelectionWeight)).Descending<GrammarResolver.RuleEntry>());
				GrammarResolver.tmpSortedRuleList.Clear();
				foreach (GrammarResolver.RuleEntry ruleEntry2 in list)
				{
					if (GrammarResolver.ValidateRule(CS$<>8__locals1.constants, CS$<>8__locals1.extraTags, CS$<>8__locals1.resolvedTags, ruleEntry2, CS$<>8__locals1.customizer) && ruleEntry2.Priority == CS$<>8__locals1.maxPriority)
					{
						GrammarResolver.tmpSortedRuleList.Add(ruleEntry2);
					}
				}
				GrammarResolver.tmpSortedRuleList.Shuffle<GrammarResolver.RuleEntry>();
				GrammarResolver.RuleEntry result;
				GrammarResolver.tmpSortedRuleList.TryMinBy((GrammarResolver.RuleEntry x) => x, comparer, out result);
				return result;
			}
			return list.RandomElementByWeightWithFallback(delegate(GrammarResolver.RuleEntry rule)
			{
				if (!GrammarResolver.ValidateRule(CS$<>8__locals1.constants, CS$<>8__locals1.extraTags, CS$<>8__locals1.resolvedTags, rule, CS$<>8__locals1.customizer) || rule.Priority != CS$<>8__locals1.maxPriority)
				{
					return 0f;
				}
				return rule.SelectionWeight;
			}, null);
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000F5664 File Offset: 0x000F3864
		private static bool ValidateRule(Dictionary<string, string> constants, List<string> extraTags, List<string> resolvedTags, GrammarResolver.RuleEntry rule, GrammarRequest.ICustomizer customizer)
		{
			return !rule.knownUnresolvable && rule.ValidateConstantConstraints(constants) && rule.ValidateRequiredTag(extraTags, resolvedTags) && rule.ValidateTimesUsed() && (customizer == null || customizer.ValidateRule(rule.rule));
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x000F569E File Offset: 0x000F389E
		public static bool ContainsSpecialChars(string str)
		{
			return str.IndexOfAny(GrammarResolver.SpecialChars) >= 0;
		}

		// Token: 0x040018C7 RID: 6343
		private static SimpleLinearPool<List<GrammarResolver.RuleEntry>> rulePool = new SimpleLinearPool<List<GrammarResolver.RuleEntry>>();

		// Token: 0x040018C8 RID: 6344
		private static Dictionary<string, List<GrammarResolver.RuleEntry>> rules = new Dictionary<string, List<GrammarResolver.RuleEntry>>();

		// Token: 0x040018C9 RID: 6345
		private static int loopCount;

		// Token: 0x040018CA RID: 6346
		private static StringBuilder logSbTrace;

		// Token: 0x040018CB RID: 6347
		private static StringBuilder logSbMid;

		// Token: 0x040018CC RID: 6348
		private static StringBuilder logSbRules;

		// Token: 0x040018CD RID: 6349
		private const int DepthLimit = 50;

		// Token: 0x040018CE RID: 6350
		private const int LoopsLimit = 1000;

		// Token: 0x040018CF RID: 6351
		private static Regex Spaces = new Regex(" +([,.])");

		// Token: 0x040018D0 RID: 6352
		public const char SymbolStartChar = '[';

		// Token: 0x040018D1 RID: 6353
		public const char SymbolEndChar = ']';

		// Token: 0x040018D2 RID: 6354
		private static List<GrammarResolver.RuleEntry> tmpSortedRuleList = new List<GrammarResolver.RuleEntry>();

		// Token: 0x040018D3 RID: 6355
		private static readonly char[] SpecialChars = new char[]
		{
			'[',
			']',
			'{',
			'}'
		};

		// Token: 0x02001CF0 RID: 7408
		private class RuleEntry
		{
			// Token: 0x17001A1C RID: 6684
			// (get) Token: 0x0600A8B1 RID: 43185 RVA: 0x00386CB2 File Offset: 0x00384EB2
			public float SelectionWeight
			{
				get
				{
					return this.rule.BaseSelectionWeight * 100000f / (float)((this.uses + 1) * 1000);
				}
			}

			// Token: 0x17001A1D RID: 6685
			// (get) Token: 0x0600A8B2 RID: 43186 RVA: 0x00386CD5 File Offset: 0x00384ED5
			public float Priority
			{
				get
				{
					return this.rule.Priority;
				}
			}

			// Token: 0x0600A8B3 RID: 43187 RVA: 0x00386CE2 File Offset: 0x00384EE2
			public RuleEntry(Rule rule)
			{
				this.rule = rule;
				this.knownUnresolvable = false;
			}

			// Token: 0x0600A8B4 RID: 43188 RVA: 0x00386CF8 File Offset: 0x00384EF8
			public void MarkKnownUnresolvable()
			{
				this.knownUnresolvable = true;
			}

			// Token: 0x0600A8B5 RID: 43189 RVA: 0x00386D01 File Offset: 0x00384F01
			public bool ValidateConstantConstraints(Dictionary<string, string> constraints)
			{
				if (!this.constantConstraintsChecked)
				{
					this.constantConstraintsValid = true;
					if (this.rule.constantConstraints != null)
					{
						this.constantConstraintsValid = this.rule.ValidateConstraints(constraints);
					}
					this.constantConstraintsChecked = true;
				}
				return this.constantConstraintsValid;
			}

			// Token: 0x0600A8B6 RID: 43190 RVA: 0x00386D3E File Offset: 0x00384F3E
			public bool ValidateRequiredTag(List<string> extraTags, List<string> resolvedTags)
			{
				return this.rule.requiredTag.NullOrEmpty() || (extraTags != null && extraTags.Contains(this.rule.requiredTag)) || resolvedTags.Contains(this.rule.requiredTag);
			}

			// Token: 0x0600A8B7 RID: 43191 RVA: 0x00386D7D File Offset: 0x00384F7D
			public bool ValidateTimesUsed()
			{
				return this.rule.usesLimit == null || this.uses < this.rule.usesLimit.Value;
			}

			// Token: 0x0600A8B8 RID: 43192 RVA: 0x00386DAB File Offset: 0x00384FAB
			public override string ToString()
			{
				return this.rule.ToString();
			}

			// Token: 0x04006F92 RID: 28562
			public Rule rule;

			// Token: 0x04006F93 RID: 28563
			public bool knownUnresolvable;

			// Token: 0x04006F94 RID: 28564
			public bool constantConstraintsChecked;

			// Token: 0x04006F95 RID: 28565
			public bool constantConstraintsValid;

			// Token: 0x04006F96 RID: 28566
			public int uses;
		}
	}
}
