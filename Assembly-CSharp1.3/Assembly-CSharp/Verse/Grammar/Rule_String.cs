using System;
using System.Text.RegularExpressions;
using RimWorld.QuestGen;

namespace Verse.Grammar
{
	// Token: 0x02000539 RID: 1337
	public class Rule_String : Rule
	{
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x0600284A RID: 10314 RVA: 0x000F5F5A File Offset: 0x000F415A
		public override float BaseSelectionWeight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x0600284B RID: 10315 RVA: 0x000F5F62 File Offset: 0x000F4162
		public override float Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000F5F6A File Offset: 0x000F416A
		public override Rule DeepCopy()
		{
			Rule_String rule_String = (Rule_String)base.DeepCopy();
			rule_String.output = this.output;
			rule_String.weight = this.weight;
			rule_String.priority = this.priority;
			return rule_String;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000F5F9B File Offset: 0x000F419B
		public Rule_String()
		{
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000F5FAE File Offset: 0x000F41AE
		public Rule_String(string keyword, string output)
		{
			this.keyword = keyword;
			this.output = output;
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000F5FD0 File Offset: 0x000F41D0
		public Rule_String(string rawString)
		{
			Match match = Rule_String.pattern.Match(rawString);
			if (!match.Success)
			{
				Log.Error(string.Format("Bad string pass when reading rule {0}", rawString));
				return;
			}
			this.keyword = match.Groups["keyword"].Value;
			this.output = match.Groups["output"].Value;
			for (int i = 0; i < match.Groups["paramname"].Captures.Count; i++)
			{
				string value = match.Groups["paramname"].Captures[i].Value;
				string value2 = match.Groups["paramoperator"].Captures[i].Value;
				string value3 = match.Groups["paramvalue"].Captures[i].Value;
				if (value == "p")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare p instead of assigning in rule {0}", rawString));
					}
					this.weight = float.Parse(value3);
				}
				else if (value == "priority")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare priority instead of assigning in rule {0}", rawString));
					}
					this.priority = float.Parse(value3);
				}
				else if (value == "tag")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare tag instead of assigning in rule {0}", rawString));
					}
					this.tag = value3;
				}
				else if (value == "requiredTag")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare requiredTag instead of assigning in rule {0}", rawString));
					}
					this.requiredTag = value3;
				}
				else if (value == "uses")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare uses instead of assigning in rule {0}", rawString));
					}
					this.usesLimit = new int?(int.Parse(value3));
				}
				else if (value == "debug")
				{
					Log.Error(string.Format("Rule {0} contains debug flag; fix before commit", rawString));
				}
				else if (value2 == "==" || value2 == "!=" || value2 == ">" || value2 == "<" || value2 == ">=" || value2 == "<=")
				{
					base.AddConstantConstraint(value, value3, value2);
				}
				else
				{
					Log.Error(string.Format("Unknown parameter {0} in rule {1}", value, rawString));
				}
			}
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000F6289 File Offset: 0x000F4489
		public override string Generate()
		{
			return this.output;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000F6294 File Offset: 0x000F4494
		public override string ToString()
		{
			return ((this.keyword != null) ? this.keyword : "null_keyword") + " → " + ((this.output != null) ? this.output.Replace("\n", "\\n") : "null_output");
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000F62E4 File Offset: 0x000F44E4
		public void AppendPrefixToAllKeywords(string prefix)
		{
			Rule_String.tmpPrefix = prefix;
			if (this.output == null)
			{
				Log.Error("Rule_String output was null.");
				this.output = "";
			}
			this.output = Regex.Replace(this.output, "\\[(.*?)\\]", new MatchEvaluator(Rule_String.RegexMatchEvaluatorAppendPrefix));
			if (this.constantConstraints != null)
			{
				for (int i = 0; i < this.constantConstraints.Count; i++)
				{
					Rule.ConstantConstraint constantConstraint = default(Rule.ConstantConstraint);
					constantConstraint.key = this.constantConstraints[i].key;
					if (!prefix.NullOrEmpty())
					{
						constantConstraint.key = prefix + "/" + constantConstraint.key;
					}
					constantConstraint.key = QuestGenUtility.NormalizeVarPath(constantConstraint.key);
					constantConstraint.value = this.constantConstraints[i].value;
					constantConstraint.type = this.constantConstraints[i].type;
					this.constantConstraints[i] = constantConstraint;
				}
			}
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x000F63EC File Offset: 0x000F45EC
		private static string RegexMatchEvaluatorAppendPrefix(Match match)
		{
			string text = match.Groups[1].Value;
			if (!Rule_String.tmpPrefix.NullOrEmpty())
			{
				text = Rule_String.tmpPrefix + "/" + text;
			}
			text = QuestGenUtility.NormalizeVarPath(text);
			return "[" + text + "]";
		}

		// Token: 0x040018E2 RID: 6370
		[MustTranslate]
		private string output;

		// Token: 0x040018E3 RID: 6371
		private float weight = 1f;

		// Token: 0x040018E4 RID: 6372
		private float priority;

		// Token: 0x040018E5 RID: 6373
		private static Regex pattern = new Regex("\r\n\t\t# hold on to your butts, this is gonna get weird\r\n\r\n\t\t^\r\n\t\t(?<keyword>[a-zA-Z0-9_/]+)\t\t\t\t\t# keyword; roughly limited to standard C# identifier rules\r\n\t\t(\t\t\t\t\t\t\t\t\t\t\t# parameter list is optional, open the capture group so we can keep it or ignore it\r\n\t\t\t\\(\t\t\t\t\t\t\t\t\t\t# this is the actual parameter list opening\r\n\t\t\t\t(\t\t\t\t\t\t\t\t\t# unlimited number of parameter groups\r\n                    [ ]*                            # leading whitespace for readability\r\n\t\t\t\t\t(?<paramname>[a-zA-Z0-9_/]+)\t# parameter name is similar\r\n\t\t\t\t\t(?<paramoperator>==|=|!=|>=|<=|>|<|) # operators; empty operator is allowed\r\n\t\t\t\t\t(?<paramvalue>[^\\,\\)]*)\t\t\t# parameter value, however, allows everything except comma and closeparen!\r\n\t\t\t\t\t,?\t\t\t\t\t\t\t\t# comma can be used to separate blocks; it is also silently ignored if it's a trailing comma\r\n\t\t\t\t)*\r\n\t\t\t\\)\r\n\t\t)?\r\n        [ ]*                                        # leading whitespace before -> for readability\r\n\t\t->(?<output>.*)\t\t\t\t\t\t\t\t# output is anything-goes\r\n\t\t$\r\n\r\n\t\t", RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

		// Token: 0x040018E6 RID: 6374
		private static string tmpPrefix;
	}
}
