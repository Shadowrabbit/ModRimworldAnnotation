using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000ED2 RID: 3794
	internal static class IdeoDescriptionUtility
	{
		// Token: 0x060059E4 RID: 23012 RVA: 0x001ECBD0 File Offset: 0x001EADD0
		private static string MemeGroupOf(Rule rule)
		{
			if (rule.tag == null || !rule.tag.StartsWith("meme_"))
			{
				return null;
			}
			return rule.tag;
		}

		// Token: 0x060059E5 RID: 23013 RVA: 0x001ECBF4 File Offset: 0x001EADF4
		private static IdeoDescriptionUtility.SegmentEvaluator BuildGrammarRequest(Ideo ideo, IdeoStoryPatternDef pattern, Dictionary<string, string> tokens)
		{
			GrammarRequest request = default(GrammarRequest);
			IdeoDescriptionUtility.DescriptionGrammarCustomizer customizer = new IdeoDescriptionUtility.DescriptionGrammarCustomizer();
			request.customizer = customizer;
			IdeoDescriptionUtility.AddRandomPawn("founder", ideo, ref request);
			IdeoDescriptionUtility.AddRandomPawn("believer", ideo, ref request);
			if (!ideo.memberName.NullOrEmpty())
			{
				tokens.AddDistinct("memberName", ideo.memberName);
			}
			if (!ideo.MemberNamePlural.NullOrEmpty())
			{
				tokens.AddDistinct("memberNamePlural", ideo.MemberNamePlural);
			}
			if (!ideo.adjective.NullOrEmpty())
			{
				tokens.AddDistinct("adjective", ideo.adjective);
			}
			foreach (MemeDef memeDef in ideo.memes)
			{
				if (memeDef.generalRules != null)
				{
					request.IncludesBare.Add(memeDef.generalRules);
				}
				IdeoDescriptionMaker descriptionMaker = memeDef.descriptionMaker;
				if (((descriptionMaker != null) ? descriptionMaker.rules : null) != null)
				{
					request.IncludesBare.Add(memeDef.descriptionMaker.rules);
				}
				IdeoDescriptionMaker descriptionMaker2 = memeDef.descriptionMaker;
				if (((descriptionMaker2 != null) ? descriptionMaker2.constants : null) != null)
				{
					IDictionary<string, string> constants = request.Constants;
					IdeoDescriptionMaker descriptionMaker3 = memeDef.descriptionMaker;
					constants.AddRange((descriptionMaker3 != null) ? descriptionMaker3.constants : null);
				}
			}
			IdeoDescriptionUtility.AddPreceptRole(ideo, "leader", PreceptDefOf.IdeoRole_Leader, tokens);
			IdeoDescriptionUtility.AddPreceptRole(ideo, "moralist", PreceptDefOf.IdeoRole_Moralist, tokens);
			IdeoDescriptionUtility.AddPreceptRules<Precept_Relic>(ideo, "relic", tokens, null);
			IdeoDescriptionUtility.AddPreceptRules<Precept_Animal>(ideo, "animal", tokens, null);
			IdeoDescriptionUtility.AddPreceptRules<Precept_Ritual>(ideo, "ritual", tokens, null);
			IdeoDescriptionUtility.AddPreceptRules<Precept_Building>(ideo, "altar", tokens, (Precept_Building p) => p.ThingDef.isAltar);
			string worshipRoomLabel = ideo.WorshipRoomLabel;
			if (worshipRoomLabel != null)
			{
				tokens.AddDistinct("altarRoomLabel", worshipRoomLabel);
			}
			ideo.foundation.AddPlaceRules(ref request);
			IdeoFoundation_Deity ideoFoundation_Deity;
			if ((ideoFoundation_Deity = (ideo.foundation as IdeoFoundation_Deity)) != null)
			{
				ideoFoundation_Deity.AddDeityRules(tokens);
			}
			if (pattern.rules != null)
			{
				request.IncludesBare.Add(pattern.rules);
			}
			request.Rules.Add(new Rule_String("foeSoldiers", GrammarResolver.Resolve("place_foeSoldiers", request, null, false, null, null, null, false)));
			request.Rules.Add(new Rule_String("foeLeader", GrammarResolver.Resolve("place_foeLeader", request, null, false, null, null, null, false)));
			IdeoDescriptionUtility.DecorateRule("memeConcept", ref request);
			IdeoDescriptionUtility.DecorateRule("memeHyphenPrefix", ref request);
			IdeoDescriptionUtility.DecorateRule("attributionJob", ref request);
			IdeoDescriptionUtility.DecorateRule("attributionSource", ref request);
			IdeoDescriptionUtility.DecorateRule("memeConference", ref request);
			return new IdeoDescriptionUtility.SegmentEvaluator(customizer, request);
		}

		// Token: 0x060059E6 RID: 23014 RVA: 0x001ECEA8 File Offset: 0x001EB0A8
		private static void DecorateRule(string keyword, ref GrammarRequest request)
		{
			if (request.HasRule(keyword))
			{
				string str = GrammarResolver.Resolve(keyword, request, null, false, null, null, null, false);
				request.Rules.Add(new Rule_String(keyword + "_titleCase", Find.ActiveLanguageWorker.ToTitleCase(str)));
			}
		}

		// Token: 0x060059E7 RID: 23015 RVA: 0x001ECEF8 File Offset: 0x001EB0F8
		private static void AddPreceptRules<T>(Ideo ideo, string rulePrefix, Dictionary<string, string> tokens, Func<T, bool> filter = null) where T : Precept
		{
			IEnumerable<T> source = ideo.PreceptsListForReading.OfType<T>();
			if (filter != null)
			{
				source = source.Where(filter);
			}
			List<T> list = source.ToList<T>();
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i];
				string str = string.Format("{0}{1}_", rulePrefix, i);
				tokens.AddDistinct(str + "name", t.Label.ApplyTag(TagType.Name, null).Resolve());
				tokens.AddDistinct(str + "label", t.Label.ApplyTag(TagType.Name, null).Resolve());
				Precept_ThingDef precept_ThingDef;
				if ((precept_ThingDef = (t as Precept_ThingDef)) != null)
				{
					tokens.AddDistinct(str + "thingDef_label", precept_ThingDef.ThingDef.label.ApplyTag(TagType.Name, null).Resolve());
				}
			}
		}

		// Token: 0x060059E8 RID: 23016 RVA: 0x001ECFF0 File Offset: 0x001EB1F0
		private static void AddPreceptRole(Ideo ideo, string prefix, PreceptDef def, Dictionary<string, string> tokens)
		{
			foreach (Precept_Role precept_Role in ideo.PreceptsListForReading.OfType<Precept_Role>())
			{
				if (precept_Role.def == def)
				{
					tokens.AddDistinct(prefix + "Title", precept_Role.Label);
					break;
				}
			}
		}

		// Token: 0x060059E9 RID: 23017 RVA: 0x001ED060 File Offset: 0x001EB260
		private static void AddRandomPawn(string key, Ideo ideo, ref GrammarRequest request)
		{
			Gender gender = ideo.SupremeGender;
			if (gender == Gender.None)
			{
				gender = ((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
			}
			Name name = PawnBioAndNameGenerator.GenerateFullPawnName(ThingDefOf.Human, null, null, null, ideo.culture, gender, PawnNameCategory.HumanStandard, null);
			int num = 21;
			int chronologicalAge = num;
			string relationInfo = "";
			foreach (Rule item in GrammarUtility.RulesForPawn(key, name, null, PawnKindDefOf.Colonist, gender, null, num, chronologicalAge, relationInfo, false, false, false, null, null, false))
			{
				request.Rules.Add(item);
			}
		}

		// Token: 0x060059EA RID: 23018 RVA: 0x001ED10C File Offset: 0x001EB30C
		public static List<string> ShuffleSegmentPreferences(IdeoStoryPatternDef pattern, List<MemeDef> memes, Dictionary<string, string> constants)
		{
			List<string> list = new List<string>();
			List<Rule> list2 = (from rule in memes.Where(delegate(MemeDef meme)
			{
				IdeoDescriptionMaker descriptionMaker = meme.descriptionMaker;
				return ((descriptionMaker != null) ? descriptionMaker.rules : null) != null;
			}).SelectMany(delegate(MemeDef meme)
			{
				IdeoDescriptionMaker descriptionMaker = meme.descriptionMaker;
				if (descriptionMaker == null)
				{
					return null;
				}
				return descriptionMaker.rules.Rules;
			})
			where IdeoDescriptionUtility.MemeGroupOf(rule) != null
			where pattern.segments.Contains(rule.keyword)
			where rule.ValidateConstraints(constants)
			select rule).ToList<Rule>();
			List<string> source = (from rule in list2
			select IdeoDescriptionUtility.MemeGroupOf(rule)).Distinct<string>().ToList<string>();
			for (int i = 0; i < 100; i++)
			{
				list.Clear();
				list.AddRange(source.InRandomOrder(null).Take(pattern.segments.Count));
				while (list.Count < pattern.segments.Count)
				{
					list.Add(null);
				}
				list.Shuffle<string>();
				bool flag = true;
				for (int j = 0; j < pattern.segments.Count; j++)
				{
					string segment = pattern.segments[j];
					string tag = list[j];
					if (tag != null && !list2.Any((Rule rule) => rule.tag == tag && rule.keyword == segment))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			return list;
		}

		// Token: 0x060059EB RID: 23019 RVA: 0x001ED2D8 File Offset: 0x001EB4D8
		public static IdeoDescriptionResult ResolveDescription(Ideo ideo, IdeoStoryPatternDef pattern, bool force)
		{
			Dictionary<string, string> tokens = new Dictionary<string, string>();
			IdeoDescriptionUtility.SegmentEvaluator segmentEvaluator = IdeoDescriptionUtility.BuildGrammarRequest(ideo, pattern, tokens);
			if (!force && ideo.descriptionTemplate != null && IdeoDescriptionUtility.TokensInTemplate(ideo.descriptionTemplate).TrueForAll((string t) => tokens.ContainsKey(t)))
			{
				return IdeoDescriptionUtility.EvaluateTemplate(ideo.descriptionTemplate, tokens);
			}
			foreach (string text in tokens.Keys)
			{
				segmentEvaluator.request.Rules.Add(new Rule_String(text, "%%" + text + "%%"));
			}
			List<string> list = IdeoDescriptionUtility.ShuffleSegmentPreferences(pattern, ideo.memes, segmentEvaluator.request.Constants);
			List<string> list2 = new List<string>();
			for (int i = 0; i < pattern.segments.Count; i++)
			{
				string text2 = pattern.segments[i];
				string targetTag = list[i];
				bool capitalizeFirstSentence = !pattern.noCapitalizeFirstSentence.Contains(text2);
				string item = segmentEvaluator.NextSegment(text2, targetTag, capitalizeFirstSentence);
				list2.Add(item);
			}
			string tokenTemplate;
			if (segmentEvaluator.request.HasRule("r_pattern"))
			{
				for (int j = 0; j < pattern.segments.Count; j++)
				{
					string str = pattern.segments[j];
					string output = list2[j];
					segmentEvaluator.request.Rules.Add(new Rule_String(string.Format("r_segment{0}", j), output));
					segmentEvaluator.request.Rules.Add(new Rule_String("r_" + str, output));
				}
				tokenTemplate = GrammarResolver.Resolve("r_pattern", segmentEvaluator.request, null, false, null, null, null, true);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text3 in list2)
				{
					if (!text3.NullOrEmpty())
					{
						stringBuilder.Append(text3).Append(" ");
					}
				}
				tokenTemplate = stringBuilder.ToString().TrimEnd(Array.Empty<char>());
			}
			return IdeoDescriptionUtility.EvaluateTemplate(tokenTemplate, tokens);
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x001ED568 File Offset: 0x001EB768
		private static string ConvertTokensToRules(string template, List<string> tokensSeen = null)
		{
			IdeoDescriptionUtility.<>c__DisplayClass12_0 CS$<>8__locals1 = new IdeoDescriptionUtility.<>c__DisplayClass12_0();
			CS$<>8__locals1.tokensSeen = tokensSeen;
			return IdeoDescriptionUtility.TokenPattern.Replace(template, new MatchEvaluator(CS$<>8__locals1.<ConvertTokensToRules>g__Replacement|0));
		}

		// Token: 0x060059ED RID: 23021 RVA: 0x001ED59C File Offset: 0x001EB79C
		private static List<string> TokensInTemplate(string template)
		{
			List<string> list = new List<string>();
			IdeoDescriptionUtility.ConvertTokensToRules(template, list);
			return list;
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x001ED5B8 File Offset: 0x001EB7B8
		private static IdeoDescriptionResult EvaluateTemplate(string tokenTemplate, Dictionary<string, string> tokens)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Rules.AddRange(from kv in tokens
			select new Rule_String(kv.Key, kv.Value));
			request.Rules.Add(new Rule_String("r_result", IdeoDescriptionUtility.ConvertTokensToRules(tokenTemplate, null)));
			return new IdeoDescriptionResult
			{
				template = tokenTemplate,
				text = GrammarResolver.Resolve("r_result", request, null, false, null, null, null, true)
			};
		}

		// Token: 0x040034A9 RID: 13481
		private const string CustomPatternName = "r_pattern";

		// Token: 0x040034AA RID: 13482
		private static readonly Regex TokenPattern = new Regex("%%(?<keyword>[^%]+)%%");

		// Token: 0x0200233E RID: 9022
		internal class DescriptionGrammarCustomizer : GrammarRequest.ICustomizer
		{
			// Token: 0x0600C654 RID: 50772 RVA: 0x003DF92F File Offset: 0x003DDB2F
			public DescriptionGrammarCustomizer()
			{
				this.prioritizer = Comparer<Rule>.Create(new Comparison<Rule>(this.Prioritize));
			}

			// Token: 0x0600C655 RID: 50773 RVA: 0x003DF959 File Offset: 0x003DDB59
			private int PriorityBucket(Rule rule)
			{
				if (this.targetTag != null && this.targetTag.Equals(IdeoDescriptionUtility.MemeGroupOf(rule)))
				{
					return 1;
				}
				return 2;
			}

			// Token: 0x0600C656 RID: 50774 RVA: 0x003DF979 File Offset: 0x003DDB79
			private int Prioritize(Rule a, Rule b)
			{
				return Comparer<int>.Default.Compare(this.PriorityBucket(a), this.PriorityBucket(b));
			}

			// Token: 0x0600C657 RID: 50775 RVA: 0x003DF993 File Offset: 0x003DDB93
			public IComparer<Rule> StrictRulePrioritizer()
			{
				return this.prioritizer;
			}

			// Token: 0x0600C658 RID: 50776 RVA: 0x003DF99B File Offset: 0x003DDB9B
			public void Notify_RuleUsed(Rule rule)
			{
				this.usedMemeRules.Increment(rule);
			}

			// Token: 0x0600C659 RID: 50777 RVA: 0x003DF9A9 File Offset: 0x003DDBA9
			public bool ValidateRule(Rule rule)
			{
				return rule.usesLimit == null || this.usedMemeRules.TryGetValue(rule, 0) < rule.usesLimit.Value;
			}

			// Token: 0x0400865F RID: 34399
			private readonly Dictionary<Rule, int> usedMemeRules = new Dictionary<Rule, int>();

			// Token: 0x04008660 RID: 34400
			private IComparer<Rule> prioritizer;

			// Token: 0x04008661 RID: 34401
			public string targetTag;
		}

		// Token: 0x0200233F RID: 9023
		public class SegmentEvaluator
		{
			// Token: 0x0600C65A RID: 50778 RVA: 0x003DF9D4 File Offset: 0x003DDBD4
			internal SegmentEvaluator(IdeoDescriptionUtility.DescriptionGrammarCustomizer customizer, GrammarRequest request)
			{
				this.customizer = customizer;
				this.request = request;
			}

			// Token: 0x0600C65B RID: 50779 RVA: 0x003DF9EA File Offset: 0x003DDBEA
			public string NextSegment(string segment, string targetTag, bool capitalizeFirstSentence)
			{
				this.customizer.targetTag = targetTag;
				string result = GrammarResolver.Resolve(segment, this.request, null, false, null, null, null, capitalizeFirstSentence);
				this.customizer.targetTag = null;
				return result;
			}

			// Token: 0x04008662 RID: 34402
			private readonly IdeoDescriptionUtility.DescriptionGrammarCustomizer customizer;

			// Token: 0x04008663 RID: 34403
			public readonly GrammarRequest request;
		}
	}
}
