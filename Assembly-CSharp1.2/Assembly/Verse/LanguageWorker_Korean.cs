using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x02000215 RID: 533
	public class LanguageWorker_Korean : LanguageWorker
	{
		// Token: 0x06000DC2 RID: 3522 RVA: 0x000105D4 File Offset: 0x0000E7D4
		public override string PostProcessed(string str)
		{
			return this.ReplaceJosa(base.PostProcessed(str));
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x000105E3 File Offset: 0x0000E7E3
		public override string PostProcessedKeyedTranslation(string translation)
		{
			return this.ReplaceJosa(base.PostProcessedKeyedTranslation(translation));
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x000AEF68 File Offset: 0x000AD168
		public string ReplaceJosa(string src)
		{
			LanguageWorker_Korean.tmpStringBuilder.Length = 0;
			string text = this.StripTags(src);
			int num = 0;
			MatchCollection matchCollection = LanguageWorker_Korean.JosaPattern.Matches(src);
			MatchCollection matchCollection2 = LanguageWorker_Korean.JosaPattern.Matches(text);
			if (matchCollection2.Count < matchCollection.Count)
			{
				return src;
			}
			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];
				Match match2 = matchCollection2[i];
				LanguageWorker_Korean.JosaPair josaPair = LanguageWorker_Korean.JosaPatternPaired[match.Value];
				LanguageWorker_Korean.tmpStringBuilder.Append(src, num, match.Index - num);
				if (match.Index > 0)
				{
					char inChar = text[match2.Index - 1];
					string value = ((match.Value == "(으)로") ? this.HasJongExceptRieul(inChar) : this.HasJong(inChar)) ? josaPair.josa1 : josaPair.josa2;
					LanguageWorker_Korean.tmpStringBuilder.Append(value);
				}
				else
				{
					LanguageWorker_Korean.tmpStringBuilder.Append(josaPair.josa2);
				}
				num = match.Index + match.Length;
			}
			LanguageWorker_Korean.tmpStringBuilder.Append(src, num, src.Length - num);
			return LanguageWorker_Korean.tmpStringBuilder.ToString();
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x000AF0B0 File Offset: 0x000AD2B0
		private string StripTags(string inString)
		{
			string text = inString;
			if (text.IndexOf("(*") >= 0)
			{
				text = LanguageWorker_Korean.TagPattern.Replace(text, "");
			}
			if (text.IndexOf("<") >= 0)
			{
				text = LanguageWorker_Korean.NodePattern.Replace(text, "");
			}
			return text;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x000105F2 File Offset: 0x0000E7F2
		private bool HasJong(char inChar)
		{
			if (!this.IsKorean(inChar))
			{
				return LanguageWorker_Korean.AlphabetEndPattern.Contains(inChar);
			}
			return this.ExtractJongCode(inChar) > 0;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x000AF100 File Offset: 0x000AD300
		private bool HasJongExceptRieul(char inChar)
		{
			if (!this.IsKorean(inChar))
			{
				return false;
			}
			int num = this.ExtractJongCode(inChar);
			return num != 8 && num != 0;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00010613 File Offset: 0x0000E813
		private int ExtractJongCode(char inChar)
		{
			return (int)((inChar - '가') % '\u001c');
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0001061F File Offset: 0x0000E81F
		private bool IsKorean(char inChar)
		{
			return inChar >= '가' && inChar <= '힣';
		}

		// Token: 0x04000B75 RID: 2933
		private static StringBuilder tmpStringBuilder = new StringBuilder();

		// Token: 0x04000B76 RID: 2934
		private static readonly Regex JosaPattern = new Regex("\\(이\\)가|\\(와\\)과|\\(을\\)를|\\(은\\)는|\\(아\\)야|\\(이\\)여|\\(으\\)로|\\(이\\)라");

		// Token: 0x04000B77 RID: 2935
		private static readonly Dictionary<string, LanguageWorker_Korean.JosaPair> JosaPatternPaired = new Dictionary<string, LanguageWorker_Korean.JosaPair>
		{
			{
				"(이)가",
				new LanguageWorker_Korean.JosaPair("이", "가")
			},
			{
				"(와)과",
				new LanguageWorker_Korean.JosaPair("과", "와")
			},
			{
				"(을)를",
				new LanguageWorker_Korean.JosaPair("을", "를")
			},
			{
				"(은)는",
				new LanguageWorker_Korean.JosaPair("은", "는")
			},
			{
				"(아)야",
				new LanguageWorker_Korean.JosaPair("아", "야")
			},
			{
				"(이)여",
				new LanguageWorker_Korean.JosaPair("이여", "여")
			},
			{
				"(으)로",
				new LanguageWorker_Korean.JosaPair("으로", "로")
			},
			{
				"(이)라",
				new LanguageWorker_Korean.JosaPair("이라", "라")
			}
		};

		// Token: 0x04000B78 RID: 2936
		private static readonly Regex TagPattern = new Regex("\\(/[a-zA-Z]+\\)", RegexOptions.Compiled);

		// Token: 0x04000B79 RID: 2937
		private static readonly Regex NodePattern = new Regex("</[a-zA-Z]+>", RegexOptions.Compiled);

		// Token: 0x04000B7A RID: 2938
		private static readonly List<char> AlphabetEndPattern = new List<char>
		{
			'b',
			'c',
			'k',
			'l',
			'm',
			'n',
			'p',
			'q',
			't'
		};

		// Token: 0x02000216 RID: 534
		private struct JosaPair
		{
			// Token: 0x06000DCC RID: 3532 RVA: 0x00010636 File Offset: 0x0000E836
			public JosaPair(string josa1, string josa2)
			{
				this.josa1 = josa1;
				this.josa2 = josa2;
			}

			// Token: 0x04000B7B RID: 2939
			public readonly string josa1;

			// Token: 0x04000B7C RID: 2940
			public readonly string josa2;
		}
	}
}
