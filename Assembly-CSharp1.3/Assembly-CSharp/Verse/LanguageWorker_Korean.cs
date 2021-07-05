using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x02000160 RID: 352
	public class LanguageWorker_Korean : LanguageWorker
	{
		// Token: 0x060009D1 RID: 2513 RVA: 0x000341A7 File Offset: 0x000323A7
		public override string PostProcessed(string str)
		{
			return this.ReplaceJosa(base.PostProcessed(str));
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x000341B6 File Offset: 0x000323B6
		public override string PostProcessedKeyedTranslation(string translation)
		{
			return this.ReplaceJosa(base.PostProcessedKeyedTranslation(translation));
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000341C8 File Offset: 0x000323C8
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

		// Token: 0x060009D4 RID: 2516 RVA: 0x00034310 File Offset: 0x00032510
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

		// Token: 0x060009D5 RID: 2517 RVA: 0x0003435E File Offset: 0x0003255E
		private bool HasJong(char inChar)
		{
			if (!this.IsKorean(inChar))
			{
				return LanguageWorker_Korean.AlphabetEndPattern.Contains(inChar);
			}
			return this.ExtractJongCode(inChar) > 0;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00034380 File Offset: 0x00032580
		private bool HasJongExceptRieul(char inChar)
		{
			if (!this.IsKorean(inChar))
			{
				return false;
			}
			int num = this.ExtractJongCode(inChar);
			return num != 8 && num != 0;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x000343AA File Offset: 0x000325AA
		private int ExtractJongCode(char inChar)
		{
			return (int)((inChar - '가') % '\u001c');
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x000343B6 File Offset: 0x000325B6
		private bool IsKorean(char inChar)
		{
			return inChar >= '가' && inChar <= '힣';
		}

		// Token: 0x0400087D RID: 2173
		private static StringBuilder tmpStringBuilder = new StringBuilder();

		// Token: 0x0400087E RID: 2174
		private static readonly Regex JosaPattern = new Regex("\\(이\\)가|\\(와\\)과|\\(을\\)를|\\(은\\)는|\\(아\\)야|\\(이\\)여|\\(으\\)로|\\(이\\)라");

		// Token: 0x0400087F RID: 2175
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

		// Token: 0x04000880 RID: 2176
		private static readonly Regex TagPattern = new Regex("\\(/[a-zA-Z]+\\)", RegexOptions.Compiled);

		// Token: 0x04000881 RID: 2177
		private static readonly Regex NodePattern = new Regex("</[a-zA-Z]+>", RegexOptions.Compiled);

		// Token: 0x04000882 RID: 2178
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

		// Token: 0x02001931 RID: 6449
		private struct JosaPair
		{
			// Token: 0x06009784 RID: 38788 RVA: 0x0035D005 File Offset: 0x0035B205
			public JosaPair(string josa1, string josa2)
			{
				this.josa1 = josa1;
				this.josa2 = josa2;
			}

			// Token: 0x040060BC RID: 24764
			public readonly string josa1;

			// Token: 0x040060BD RID: 24765
			public readonly string josa2;
		}
	}
}
