using System;
using System.Text.RegularExpressions;
using RimWorld.QuestGen;

namespace Verse.Grammar
{
	// Token: 0x02000908 RID: 2312
	public class Rule_String : Rule
	{
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003967 RID: 14695 RVA: 0x0002C4F9 File Offset: 0x0002A6F9
		public override float BaseSelectionWeight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003968 RID: 14696 RVA: 0x0002C501 File Offset: 0x0002A701
		public override float Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x0002C509 File Offset: 0x0002A709
		public override Rule DeepCopy()
		{
			Rule_String rule_String = (Rule_String)base.DeepCopy();
			rule_String.output = this.output;
			rule_String.weight = this.weight;
			rule_String.priority = this.priority;
			return rule_String;
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x0002C53A File Offset: 0x0002A73A
		public Rule_String()
		{
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x0002C54D File Offset: 0x0002A74D
		public Rule_String(string keyword, string output)
		{
			this.keyword = keyword;
			this.output = output;
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x001673D0 File Offset: 0x001655D0
		public Rule_String(string rawString)
		{
			Match match = Rule_String.pattern.Match(rawString);
			if (!match.Success)
			{
				Log.Error(string.Format("Bad string pass when reading rule {0}", rawString), false);
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
						Log.Error(string.Format("Attempt to compare p instead of assigning in rule {0}", rawString), false);
					}
					this.weight = float.Parse(value3);
				}
				else if (value == "priority")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare priority instead of assigning in rule {0}", rawString), false);
					}
					this.priority = float.Parse(value3);
				}
				else if (value == "tag")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare tag instead of assigning in rule {0}", rawString), false);
					}
					this.tag = value3;
				}
				else if (value == "requiredTag")
				{
					if (value2 != "=")
					{
						Log.Error(string.Format("Attempt to compare requiredTag instead of assigning in rule {0}", rawString), false);
					}
					this.requiredTag = value3;
				}
				else if (value == "debug")
				{
					Log.Error(string.Format("Rule {0} contains debug flag; fix before commit", rawString), false);
				}
				else if (value2 == "==" || value2 == "!=" || value2 == ">" || value2 == "<" || value2 == ">=" || value2 == "<=")
				{
					base.AddConstantConstraint(value, value3, value2);
				}
				else
				{
					Log.Error(string.Format("Unknown parameter {0} in rule {1}", value, rawString), false);
				}
			}
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x0002C56E File Offset: 0x0002A76E
		public override string Generate()
		{
			return this.output;
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x00167650 File Offset: 0x00165850
		public override string ToString()
		{
			return ((this.keyword != null) ? this.keyword : "null_keyword") + " → " + ((this.output != null) ? this.output.Replace("\n", "\\n") : "null_output");
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x001676A0 File Offset: 0x001658A0
		public void AppendPrefixToAllKeywords(string prefix)
		{
			Rule_String.tmpPrefix = prefix;
			if (this.output == null)
			{
				Log.Error("Rule_String output was null.", false);
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

		// Token: 0x06003970 RID: 14704 RVA: 0x001677AC File Offset: 0x001659AC
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

		// Token: 0x040027C9 RID: 10185
		[MustTranslate]
		private string output;

		// Token: 0x040027CA RID: 10186
		private float weight = 1f;

		// Token: 0x040027CB RID: 10187
		private float priority;

		// Token: 0x040027CC RID: 10188
		private static Regex pattern = new Regex("\r\n\t\t# hold on to your butts, this is gonna get weird\r\n\r\n\t\t^\r\n\t\t(?<keyword>[a-zA-Z0-9_/]+)\t\t\t\t\t# keyword; roughly limited to standard C# identifier rules\r\n\t\t(\t\t\t\t\t\t\t\t\t\t\t# parameter list is optional, open the capture group so we can keep it or ignore it\r\n\t\t\t\\(\t\t\t\t\t\t\t\t\t\t# this is the actual parameter list opening\r\n\t\t\t\t(\t\t\t\t\t\t\t\t\t# unlimited number of parameter groups\r\n\t\t\t\t\t(?<paramname>[a-zA-Z0-9_/]+)\t# parameter name is similar\r\n\t\t\t\t\t(?<paramoperator>==|=|!=|>=|<=|>|<|) # operators; empty operator is allowed\r\n\t\t\t\t\t(?<paramvalue>[^\\,\\)]*)\t\t\t# parameter value, however, allows everything except comma and closeparen!\r\n\t\t\t\t\t,?\t\t\t\t\t\t\t\t# comma can be used to separate blocks; it is also silently ignored if it's a trailing comma\r\n\t\t\t\t)*\r\n\t\t\t\\)\r\n\t\t)?\r\n\t\t->(?<output>.*)\t\t\t\t\t\t\t\t# output is anything-goes\r\n\t\t$\r\n\r\n\t\t", RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

		// Token: 0x040027CD RID: 10189
		private static string tmpPrefix;
	}
}
