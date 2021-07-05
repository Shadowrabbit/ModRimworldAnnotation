using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000031 RID: 49
	public static class GenText
	{
		// Token: 0x06000298 RID: 664 RVA: 0x0000DD2C File Offset: 0x0000BF2C
		public static string Possessive(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "Proher".Translate();
			}
			return "Prohis".Translate();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000DD56 File Offset: 0x0000BF56
		public static string PossessiveCap(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "ProherCap".Translate();
			}
			return "ProhisCap".Translate();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000DD80 File Offset: 0x0000BF80
		public static string ProObj(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "ProherObj".Translate();
			}
			return "ProhimObj".Translate();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000DDAA File Offset: 0x0000BFAA
		public static string ProObjCap(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "ProherObjCap".Translate();
			}
			return "ProhimObjCap".Translate();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
		public static string ProSubj(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "Proshe".Translate();
			}
			return "Prohe".Translate();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000DDFE File Offset: 0x0000BFFE
		public static string ProSubjCap(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "ProsheCap".Translate();
			}
			return "ProheCap".Translate();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000DE28 File Offset: 0x0000C028
		public static string AdjustedFor(this string text, Pawn p, string pawnSymbol = "PAWN", bool addRelationInfoSymbol = true)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.DynamicWrapper);
			request.Rules.Add(new Rule_String("RULE", text));
			request.Rules.AddRange(GrammarUtility.RulesForPawn(pawnSymbol, p, null, addRelationInfoSymbol, true));
			return GrammarResolver.Resolve("r_root", request, null, false, null, null, null, true);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000DE8C File Offset: 0x0000C08C
		public static string AdjustedForKeys(this string text, List<string> outErrors = null, bool resolveKeys = true)
		{
			if (outErrors != null)
			{
				outErrors.Clear();
			}
			if (text.NullOrEmpty())
			{
				return text;
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 500000)
				{
					break;
				}
				int num2 = text.IndexOf("{Key:");
				if (num2 < 0)
				{
					return text;
				}
				int num3 = num2;
				while (text[num3] != '}')
				{
					num3++;
					if (num3 >= text.Length)
					{
						goto Block_6;
					}
				}
				string text2 = text.Substring(num2 + 5, num3 - (num2 + 5));
				KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(text2);
				string text3 = text.Substring(0, num2);
				if (namedSilentFail != null)
				{
					if (resolveKeys)
					{
						text3 += namedSilentFail.MainKeyLabel;
					}
					else
					{
						text3 += "placeholder";
					}
				}
				else
				{
					text3 += "error";
					if (outErrors != null)
					{
						string text4 = "Could not find key '" + text2 + "'";
						string text5 = BackCompatibility.BackCompatibleDefName(typeof(KeyBindingDef), text2, false, null);
						if (text5 != text2)
						{
							text4 = text4 + " (hint: it was renamed to '" + text5 + "')";
						}
						outErrors.Add(text4);
					}
				}
				text3 += text.Substring(num3 + 1);
				text = text3;
			}
			Log.Error("Too many iterations.");
			if (outErrors != null)
			{
				outErrors.Add("The parsed string caused an infinite loop");
				return text;
			}
			return text;
			Block_6:
			if (outErrors != null)
			{
				outErrors.Add("Mismatched braces");
			}
			return text;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000DFDD File Offset: 0x0000C1DD
		public static string LabelIndefinite(this Pawn pawn)
		{
			if (pawn.Name != null && !pawn.Name.Numerical)
			{
				return pawn.LabelShort;
			}
			return pawn.KindLabelIndefinite();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000E001 File Offset: 0x0000C201
		public static string LabelDefinite(this Pawn pawn)
		{
			if (pawn.Name != null && !pawn.Name.Numerical)
			{
				return pawn.LabelShort;
			}
			return pawn.KindLabelDefinite();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000E025 File Offset: 0x0000C225
		public static string KindLabelIndefinite(this Pawn pawn)
		{
			return Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(pawn.KindLabel, pawn.gender, false, false);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000E03F File Offset: 0x0000C23F
		public static string KindLabelDefinite(this Pawn pawn)
		{
			return Find.ActiveLanguageWorker.WithDefiniteArticlePostProcessed(pawn.KindLabel, pawn.gender, false, false);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000E05C File Offset: 0x0000C25C
		public static string RandomSeedString()
		{
			return GrammarResolver.Resolve("r_seed", new GrammarRequest
			{
				Includes = 
				{
					RulePackDefOf.SeedGenerator
				}
			}, null, false, null, null, null, true).ToLower();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000E098 File Offset: 0x0000C298
		public static string Shorten(this string s)
		{
			if (s.NullOrEmpty() || s.Length <= 4)
			{
				return s;
			}
			s = s.Trim();
			string[] array = s.Split(new char[]
			{
				' '
			});
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					text += " ";
				}
				if (array[i].Length > 2)
				{
					text = text + array[i].Substring(0, 1) + array[i].Substring(1, array[i].Length - 2).WithoutVowels() + array[i].Substring(array[i].Length - 1, 1);
				}
			}
			return text;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000E140 File Offset: 0x0000C340
		private static string WithoutVowels(this string s)
		{
			string vowels = "aeiouy";
			return new string((from c in s
			where !vowels.Contains(c)
			select c).ToArray<char>());
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000E17C File Offset: 0x0000C37C
		public static string MarchingEllipsis(float offset = 0f)
		{
			switch (Mathf.FloorToInt(Time.realtimeSinceStartup + offset) % 3)
			{
			case 0:
				return ".";
			case 1:
				return "..";
			case 2:
				return "...";
			default:
				throw new Exception();
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000E1C3 File Offset: 0x0000C3C3
		public static void SetTextSizeToFit(string text, Rect r)
		{
			Text.Font = GameFont.Small;
			if (Text.CalcHeight(text, r.width) > r.height)
			{
				Text.Font = GameFont.Tiny;
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000E1E7 File Offset: 0x0000C3E7
		public static string TrimEndNewlines(this string s)
		{
			return s.TrimEnd(new char[]
			{
				'\r',
				'\n'
			});
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000E1FF File Offset: 0x0000C3FF
		public static string Indented(this string s, string indentation = "    ")
		{
			if (s.NullOrEmpty())
			{
				return s;
			}
			return indentation + s.Replace("\r", "").Replace("\n", "\n" + indentation);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000E238 File Offset: 0x0000C438
		public static string ReplaceFirst(this string source, string key, string replacement)
		{
			int num = source.IndexOf(key);
			if (num < 0)
			{
				return source;
			}
			return source.Substring(0, num) + replacement + source.Substring(num + key.Length);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000E270 File Offset: 0x0000C470
		public static int StableStringHash(string str)
		{
			if (str == null)
			{
				return 0;
			}
			int num = 23;
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				num = num * 31 + (int)str[i];
			}
			return num;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000E2A8 File Offset: 0x0000C4A8
		public static string StringFromEnumerable(IEnumerable source)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in source)
			{
				stringBuilder.AppendLine("• " + obj.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000E314 File Offset: 0x0000C514
		public static IEnumerable<string> LinesFromString(string text)
		{
			string[] separator = new string[]
			{
				"\r\n",
				"\n"
			};
			string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i].Trim();
				if (!text2.StartsWith("//"))
				{
					text2 = text2.Split(new string[]
					{
						"//"
					}, StringSplitOptions.None)[0];
					if (text2.Length != 0)
					{
						yield return text2;
					}
				}
			}
			array = null;
			yield break;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000E324 File Offset: 0x0000C524
		public static string GetInvalidFilenameCharacters()
		{
			return new string(Path.GetInvalidFileNameChars()) + "/\\{}<>:*|!@#$%^&*?";
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000E33A File Offset: 0x0000C53A
		public static bool IsValidFilename(string str)
		{
			return str.Length <= 40 && !new Regex("[" + Regex.Escape(GenText.GetInvalidFilenameCharacters()) + "]").IsMatch(str);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000E36F File Offset: 0x0000C56F
		public static string SanitizeFilename(string str)
		{
			return string.Join("_", str.Split(GenText.GetInvalidFilenameCharacters().ToArray<char>(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd(new char[]
			{
				'.'
			});
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000E39C File Offset: 0x0000C59C
		public static bool NullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		public static string SplitCamelCase(string Str)
		{
			return Regex.Replace(Str, "(\\B[A-Z]+?(?=[A-Z][^A-Z])|\\B[A-Z]+?(?=[^A-Z]))", " $1");
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E3B8 File Offset: 0x0000C5B8
		public static string CapitalizedNoSpaces(string s)
		{
			string[] array = s.Split(new char[]
			{
				' '
			});
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in array)
			{
				if (text.Length > 0)
				{
					stringBuilder.Append(char.ToUpper(text[0]));
				}
				if (text.Length > 1)
				{
					stringBuilder.Append(text.Substring(1));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000E428 File Offset: 0x0000C628
		public static string RemoveNonAlphanumeric(string s)
		{
			GenText.tmpSb.Length = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsLetterOrDigit(s[i]))
				{
					GenText.tmpSb.Append(s[i]);
				}
			}
			return GenText.tmpSb.ToString();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000E47B File Offset: 0x0000C67B
		public static bool EqualsIgnoreCase(this string A, string B)
		{
			return string.Compare(A, B, true) == 0;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000E488 File Offset: 0x0000C688
		public static string WithoutByteOrderMark(this string str)
		{
			return str.Trim().Trim(new char[]
			{
				'﻿'
			});
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E4A4 File Offset: 0x0000C6A4
		public static bool NamesOverlap(string A, string B)
		{
			A = A.ToLower();
			B = B.ToLower();
			string[] array = A.Split(new char[]
			{
				' '
			});
			string[] source = B.Split(new char[]
			{
				' '
			});
			foreach (string text in array)
			{
				if (TitleCaseHelper.IsUppercaseTitleWord(text) && source.Contains(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000E510 File Offset: 0x0000C710
		public static int FirstLetterBetweenTags(this string str)
		{
			int num = 0;
			if (str[num] == '<' && str.IndexOf('>') > num && num < str.Length - 1 && str[num + 1] != '/')
			{
				num = str.IndexOf('>') + 1;
			}
			return num;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000E55C File Offset: 0x0000C75C
		public static string CapitalizeFirst(this string str)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (char.IsUpper(str[0]))
			{
				return str;
			}
			if (str.Length == 1)
			{
				return str.ToUpper();
			}
			int num = str.FirstLetterBetweenTags();
			if (num == 0)
			{
				return char.ToUpper(str[num]).ToString() + str.Substring(num + 1);
			}
			return str.Substring(0, num) + char.ToUpper(str[num]).ToString() + str.Substring(num + 1);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000E5EA File Offset: 0x0000C7EA
		public static string CapitalizeFirst(this string str, Def possibleDef)
		{
			if (possibleDef != null && str == possibleDef.label)
			{
				return possibleDef.LabelCap;
			}
			return str.CapitalizeFirst();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000E610 File Offset: 0x0000C810
		public static string UncapitalizeFirst(this string str)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (char.IsLower(str[0]))
			{
				return str;
			}
			if (str.Length == 1)
			{
				return str.ToLower();
			}
			int num = str.FirstLetterBetweenTags();
			if (num == 0)
			{
				return char.ToLower(str[num]).ToString() + str.Substring(num + 1);
			}
			return str.Substring(0, num) + char.ToLower(str[num]).ToString() + str.Substring(num + 1);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000E6A0 File Offset: 0x0000C8A0
		public static string ToNewsCase(string str)
		{
			string[] array = str.Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.Length >= 2)
				{
					if (i == 0)
					{
						array[i] = text[0].ToString().ToUpper() + text.Substring(1);
					}
					else
					{
						array[i] = text.ToLower();
					}
				}
			}
			return string.Join(" ", array);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000E716 File Offset: 0x0000C916
		public static string ToTitleCaseSmart(string str)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			return GenText.CapitalizeFirstLetterAfterSeparator(GenText.CapitalizeFirstLetterAfterSeparator(str.MergeMultipleSpaces(false).Trim(), ' '), '-');
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000E73C File Offset: 0x0000C93C
		private static string CapitalizeFirstLetterAfterSeparator(string str, char separator)
		{
			string[] array = str.Split(new char[]
			{
				separator
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if ((i == 0 || i == array.Length - 1 || TitleCaseHelper.IsUppercaseTitleWord(text)) && !text.NullOrEmpty())
				{
					int num = text.FirstLetterBetweenTags();
					string str2 = (num == 0) ? text[num].ToString().ToUpper() : (text.Substring(0, num) + char.ToUpper(text[num]).ToString());
					string str3 = text.Substring(num + 1);
					array[i] = str2 + str3;
				}
			}
			return string.Join(separator.ToString(), array);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E7F4 File Offset: 0x0000C9F4
		public static string CapitalizeSentences(string input, bool capitalizeFirstSentence = true)
		{
			if (input.NullOrEmpty())
			{
				return input;
			}
			if (input.Length != 1)
			{
				bool flag = capitalizeFirstSentence;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				GenText.tmpSbForCapitalizedSentences.Length = 0;
				for (int i = 0; i < input.Length; i++)
				{
					if (flag && char.IsLetterOrDigit(input[i]) && !flag2 && !flag3 && !flag4)
					{
						GenText.tmpSbForCapitalizedSentences.Append(char.ToUpper(input[i]));
						flag = false;
					}
					else
					{
						GenText.tmpSbForCapitalizedSentences.Append(input[i]);
					}
					if (input[i] == '\r' || input[i] == '\n' || (input[i] == '.' && i < input.Length - 1 && !char.IsLetter(input[i + 1])) || input[i] == '!' || input[i] == '?' || input[i] == ':')
					{
						flag = true;
					}
					else if (input[i] == '<' && i < input.Length - 1 && input[i + 1] != '/')
					{
						flag2 = true;
					}
					else if (flag2 && input[i] == '>')
					{
						flag2 = false;
					}
					else if (input[i] == '(' && i < input.Length - 1 && input[i + 1] == '*')
					{
						flag4 = true;
					}
					else if (flag4 && input[i] == ')')
					{
						flag4 = false;
					}
					else if (input[i] == '{')
					{
						flag3 = true;
						flag = false;
					}
					else if (flag3 && input[i] == '}')
					{
						flag3 = false;
						flag = false;
					}
				}
				return GenText.tmpSbForCapitalizedSentences.ToString();
			}
			if (capitalizeFirstSentence)
			{
				return input.ToUpper();
			}
			return input;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000E9AD File Offset: 0x0000CBAD
		public static string CapitalizeAsTitle(string str)
		{
			return Find.ActiveLanguageWorker.ToTitleCase(str);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000E9BC File Offset: 0x0000CBBC
		public static string ToCommaList(this IEnumerable<string> items, bool useAnd = false, bool emptyIfNone = false)
		{
			if (items == null)
			{
				return "";
			}
			string value;
			string text;
			int num;
			StringBuilder stringBuilder = GenText.ToCommaListInner(items, out value, out text, out num);
			if (num == 0)
			{
				return emptyIfNone ? "" : "NoneLower".Translate();
			}
			if (num == 1)
			{
				return text;
			}
			if (!useAnd)
			{
				stringBuilder.Append(text);
				return stringBuilder.ToString();
			}
			if (num == 2)
			{
				return "ToCommaListAnd".Translate(value, text);
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			return "ToCommaListAnd".Translate(stringBuilder.ToString(), text);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		public static string ToCommaListOr(this IEnumerable<string> items, bool emptyIfNone = false)
		{
			if (items == null)
			{
				return "";
			}
			string value;
			string text;
			int num;
			StringBuilder stringBuilder = GenText.ToCommaListInner(items, out value, out text, out num);
			if (num == 0)
			{
				return emptyIfNone ? "" : "NoneLower".Translate();
			}
			if (num == 1)
			{
				return text;
			}
			if (num == 2)
			{
				return "ToCommaListOr".Translate(value, text);
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			return "ToCommaListOr".Translate(stringBuilder.ToString(), text);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000EB08 File Offset: 0x0000CD08
		private static StringBuilder ToCommaListInner(IEnumerable<string> items, out string first, out string last, out int count)
		{
			first = null;
			last = null;
			count = 0;
			StringBuilder stringBuilder = new StringBuilder();
			IList<string> list = items as IList<string>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					string text = list[i];
					if (!text.NullOrEmpty())
					{
						if (first == null)
						{
							first = text;
						}
						if (last != null)
						{
							stringBuilder.Append(last + ", ");
						}
						last = text;
						count++;
					}
				}
			}
			else
			{
				foreach (string text2 in items)
				{
					if (!text2.NullOrEmpty())
					{
						if (first == null)
						{
							first = text2;
						}
						if (last != null)
						{
							stringBuilder.Append(last + ", ");
						}
						last = text2;
						count++;
					}
				}
			}
			return stringBuilder;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000EBE8 File Offset: 0x0000CDE8
		public static TaggedString ToClauseSequence(this List<string> entries)
		{
			switch (entries.Count)
			{
			case 0:
				return "None".Translate() + ".";
			case 1:
				return entries[0] + ".";
			case 2:
				return "ClauseSequence2".Translate(entries[0], entries[1]);
			case 3:
				return "ClauseSequence3".Translate(entries[0], entries[1], entries[2]);
			case 4:
				return "ClauseSequence4".Translate(entries[0], entries[1], entries[2], entries[3]);
			case 5:
				return "ClauseSequence5".Translate(entries[0], entries[1], entries[2], entries[3], entries[4]);
			case 6:
				return "ClauseSequence6".Translate(entries[0], entries[1], entries[2], entries[3], entries[4], entries[5]);
			case 7:
				return "ClauseSequence7".Translate(entries[0], entries[1], entries[2], entries[3], entries[4], entries[5], entries[6]);
			case 8:
				return "ClauseSequence8".Translate(entries[0], entries[1], entries[2], entries[3], entries[4], entries[5], entries[6], entries[7]);
			default:
				return entries.ToCommaList(true, false);
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000EE58 File Offset: 0x0000D058
		public static string ToLineList(this IList<string> entries, string prefix = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < entries.Count; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append("\n");
				}
				if (prefix != null)
				{
					stringBuilder.Append(prefix);
				}
				stringBuilder.Append(entries[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000EEAC File Offset: 0x0000D0AC
		public static string ToLineList(this IList<string> entries, Color color, string prefix = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < entries.Count; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append("\n");
				}
				if (prefix != null)
				{
					stringBuilder.Append(prefix);
				}
				stringBuilder.Append(entries[i].Colorize(color));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000EF04 File Offset: 0x0000D104
		public static string ToLineList(this IEnumerable<string> entries, string prefix = null, bool capitalizeItems = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string text in entries)
			{
				if (!flag)
				{
					stringBuilder.Append("\n");
				}
				if (prefix != null)
				{
					stringBuilder.Append(prefix);
				}
				stringBuilder.Append(capitalizeItems ? text.CapitalizeFirst() : text);
				flag = false;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000EF84 File Offset: 0x0000D184
		public static string ToSpaceList(IEnumerable<string> entries)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string value in entries)
			{
				if (!flag)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(value);
				flag = false;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000EFEC File Offset: 0x0000D1EC
		public static string ToCamelCase(string str)
		{
			string text = "";
			foreach (string str2 in str.Split(new char[]
			{
				' '
			}))
			{
				if (text.Length == 0)
				{
					text += str2.UncapitalizeFirst();
				}
				else
				{
					text += str2.CapitalizeFirst();
				}
			}
			return text;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000F048 File Offset: 0x0000D248
		public static string Truncate(this string str, float width, Dictionary<string, string> cache = null)
		{
			string text;
			if (cache != null && cache.TryGetValue(str, out text))
			{
				return text;
			}
			if (Text.CalcSize(str).x <= width)
			{
				if (cache != null)
				{
					cache.Add(str, str);
				}
				return str;
			}
			text = str;
			do
			{
				text = text.Substring(0, text.Length - 1);
			}
			while (text.Length > 0 && Text.CalcSize(text + "...").x > width);
			text += "...";
			if (cache != null)
			{
				cache.Add(str, text);
			}
			return text;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000F0CC File Offset: 0x0000D2CC
		public static TaggedString Truncate(this TaggedString str, float width, Dictionary<string, TaggedString> cache = null)
		{
			TaggedString taggedString;
			if (cache != null && cache.TryGetValue(str.RawText, out taggedString))
			{
				return taggedString;
			}
			if (Text.CalcSize(str.RawText.StripTags()).x < width)
			{
				if (cache != null)
				{
					cache.Add(str.RawText, str);
				}
				return str;
			}
			taggedString = str;
			do
			{
				taggedString = taggedString.RawText.Substring(0, taggedString.RawText.Length - 1);
			}
			while (taggedString.RawText.StripTags().Length > 0 && Text.CalcSize(GenText.AddEllipses(taggedString.RawText.StripTags())).x > width);
			taggedString = GenText.AddEllipses(taggedString);
			if (cache != null)
			{
				cache.Add(str.RawText, str);
			}
			return taggedString;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000F194 File Offset: 0x0000D394
		public static string TruncateHeight(this string str, float width, float height, Dictionary<string, string> cache = null)
		{
			string text;
			if (cache != null && cache.TryGetValue(str, out text))
			{
				return text;
			}
			if (Text.CalcHeight(str, width) <= height)
			{
				if (cache != null)
				{
					cache.Add(str, str);
				}
				return str;
			}
			text = str;
			do
			{
				text = text.Substring(0, text.Length - 1);
			}
			while (text.Length > 0 && Text.CalcHeight(GenText.AddEllipses(text), width) > height);
			text = GenText.AddEllipses(text);
			if (cache != null)
			{
				cache.Add(str, text);
			}
			return text;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000F205 File Offset: 0x0000D405
		public static string AddEllipses(string s)
		{
			if (s.Length > 0 && s[s.Length - 1] == '.')
			{
				return s + " ...";
			}
			return s + "...";
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000F23C File Offset: 0x0000D43C
		public static string Flatten(this string str)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (str.Contains("\n"))
			{
				str = str.Replace("\n", " ");
			}
			if (str.Contains("\r"))
			{
				str = str.Replace("\r", "");
			}
			str = str.MergeMultipleSpaces(false);
			return str.Trim(new char[]
			{
				' ',
				'\n',
				'\r',
				'\t'
			});
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
		public static string MergeMultipleSpaces(this string str, bool leaveMultipleSpacesAtLineBeginning = true)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (!str.Contains("  "))
			{
				return str;
			}
			bool flag = true;
			GenText.tmpStringBuilder.Length = 0;
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '\r' || str[i] == '\n')
				{
					flag = true;
				}
				if ((leaveMultipleSpacesAtLineBeginning && flag) || str[i] != ' ' || i == 0 || str[i - 1] != ' ')
				{
					GenText.tmpStringBuilder.Append(str[i]);
				}
				if (!char.IsWhiteSpace(str[i]))
				{
					flag = false;
				}
			}
			return GenText.tmpStringBuilder.ToString();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000F35C File Offset: 0x0000D55C
		public static string TrimmedToLength(this string str, int length)
		{
			if (str == null || str.Length <= length)
			{
				return str;
			}
			return str.Substring(0, length);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F374 File Offset: 0x0000D574
		public static bool ContainsEmptyLines(string str)
		{
			return str.NullOrEmpty() || (str[0] == '\n' || str[0] == '\r') || (str[str.Length - 1] == '\n' || str[str.Length - 1] == '\r') || (str.Contains("\n\n") || str.Contains("\r\n\r\n") || str.Contains("\r\r"));
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F3F4 File Offset: 0x0000D5F4
		public static string ToStringByStyle(this float f, ToStringStyle style, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			if (style == ToStringStyle.Temperature && numberSense == ToStringNumberSense.Offset)
			{
				style = ToStringStyle.TemperatureOffset;
			}
			if (numberSense == ToStringNumberSense.Factor)
			{
				if (f >= 10f)
				{
					style = ToStringStyle.FloatMaxTwo;
				}
				else
				{
					style = ToStringStyle.PercentZero;
				}
			}
			string text;
			switch (style)
			{
			case ToStringStyle.Integer:
				text = Mathf.RoundToInt(f).ToString();
				break;
			case ToStringStyle.FloatOne:
				text = f.ToString("F1");
				break;
			case ToStringStyle.FloatTwo:
				text = f.ToString("F2");
				break;
			case ToStringStyle.FloatThree:
				text = f.ToString("F3");
				break;
			case ToStringStyle.FloatMaxOne:
				text = f.ToString("0.#");
				break;
			case ToStringStyle.FloatMaxTwo:
				text = f.ToString("0.##");
				break;
			case ToStringStyle.FloatMaxThree:
				text = f.ToString("0.###");
				break;
			case ToStringStyle.FloatTwoOrThree:
				text = f.ToString((f == 0f || Mathf.Abs(f) >= 0.01f) ? "F2" : "F3");
				break;
			case ToStringStyle.PercentZero:
				text = f.ToStringPercent();
				break;
			case ToStringStyle.PercentOne:
				text = f.ToStringPercent("F1");
				break;
			case ToStringStyle.PercentTwo:
				text = f.ToStringPercent("F2");
				break;
			case ToStringStyle.Temperature:
				text = f.ToStringTemperature("F1");
				break;
			case ToStringStyle.TemperatureOffset:
				text = f.ToStringTemperatureOffset("F1");
				break;
			case ToStringStyle.WorkAmount:
				text = f.ToStringWorkAmount();
				break;
			case ToStringStyle.Money:
				text = f.ToStringMoney(null);
				break;
			default:
				Log.Error("Unknown ToStringStyle " + style);
				text = f.ToString();
				break;
			}
			if (numberSense == ToStringNumberSense.Offset)
			{
				if (f >= 0f)
				{
					text = "+" + text;
				}
			}
			else if (numberSense == ToStringNumberSense.Factor)
			{
				text = "x" + text;
			}
			return text;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F5B0 File Offset: 0x0000D7B0
		public static string ToStringDecimalIfSmall(this float f)
		{
			if (Mathf.Abs(f) < 1f)
			{
				return Math.Round((double)f, 2).ToString("0.##");
			}
			if (Mathf.Abs(f) < 10f)
			{
				return Math.Round((double)f, 1).ToString("0.#");
			}
			return Mathf.RoundToInt(f).ToStringCached();
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000F60E File Offset: 0x0000D80E
		public static string ToStringPercent(this float f)
		{
			return (f * 100f).ToStringDecimalIfSmall() + "%";
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F628 File Offset: 0x0000D828
		public static string ToStringPercent(this float f, string format)
		{
			return ((f + 1E-05f) * 100f).ToString(format) + "%";
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000F658 File Offset: 0x0000D858
		public static string ToStringMoney(this float f, string format = null)
		{
			if (format == null)
			{
				if (f >= 10f || f == 0f)
				{
					format = "F0";
				}
				else
				{
					format = "F2";
				}
			}
			return "MoneyFormat".Translate(f.ToString(format));
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
		public static string ToStringMoneyOffset(this float f, string format = null)
		{
			string text = Mathf.Abs(f).ToStringMoney(format);
			if (f > 0f && text != "$0")
			{
				return "+" + text;
			}
			if (f < 0f)
			{
				return "-" + text;
			}
			return text;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F6F4 File Offset: 0x0000D8F4
		public static string ToStringWithSign(this int i)
		{
			return i.ToString("+#;-#;0");
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000F702 File Offset: 0x0000D902
		public static string ToStringWithSign(this float f, string format = "0.##")
		{
			if (f > 0f)
			{
				return "+" + f.ToString(format);
			}
			return f.ToString(format);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000F728 File Offset: 0x0000D928
		public static string ToStringKilobytes(this int bytes, string format = "F2")
		{
			return ((float)bytes / 1024f).ToString(format) + "Kb";
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F750 File Offset: 0x0000D950
		public static string ToStringYesNo(this bool b)
		{
			return b ? "Yes".Translate() : "No".Translate();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F770 File Offset: 0x0000D970
		public static string ToStringLongitude(this float longitude)
		{
			bool flag = longitude < 0f;
			if (flag)
			{
				longitude = -longitude;
			}
			return longitude.ToString("F2") + "°" + (flag ? "W" : "E");
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
		public static string ToStringLatitude(this float latitude)
		{
			bool flag = latitude < 0f;
			if (flag)
			{
				latitude = -latitude;
			}
			return latitude.ToString("F2") + "°" + (flag ? "S" : "N");
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F7F8 File Offset: 0x0000D9F8
		public static string ToStringMass(this float mass)
		{
			if (mass == 0f)
			{
				return "0 g";
			}
			float num = Mathf.Abs(mass);
			if (num >= 100f)
			{
				return mass.ToString("F0") + " kg";
			}
			if (num >= 10f)
			{
				return mass.ToString("0.#") + " kg";
			}
			if (num >= 0.1f)
			{
				return mass.ToString("0.##") + " kg";
			}
			float num2 = mass * 1000f;
			if (num >= 0.01f)
			{
				return num2.ToString("F0") + " g";
			}
			if (num >= 0.001f)
			{
				return num2.ToString("0.#") + " g";
			}
			return num2.ToString("0.##") + " g";
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F8D4 File Offset: 0x0000DAD4
		public static string ToStringMassOffset(this float mass)
		{
			string text = mass.ToStringMass();
			if (mass > 0f)
			{
				return "+" + text;
			}
			return text;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F8FD File Offset: 0x0000DAFD
		public static string ToStringSign(this float val)
		{
			if (val >= 0f)
			{
				return "+";
			}
			return "";
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F914 File Offset: 0x0000DB14
		public static string ToStringEnsureThreshold(this float value, float threshold, int decimalPlaces)
		{
			if (value > threshold && Math.Round((double)value, decimalPlaces) <= Math.Round((double)threshold, decimalPlaces))
			{
				return (value + 1f / Mathf.Pow(10f, (float)decimalPlaces)).ToString("F" + decimalPlaces);
			}
			return value.ToString("F" + decimalPlaces);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F97B File Offset: 0x0000DB7B
		public static string ToStringTemperature(this float celsiusTemp, string format = "F1")
		{
			celsiusTemp = GenTemperature.CelsiusTo(celsiusTemp, Prefs.TemperatureMode);
			return celsiusTemp.ToStringTemperatureRaw(format);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F991 File Offset: 0x0000DB91
		public static string ToStringTemperatureOffset(this float celsiusTemp, string format = "F1")
		{
			celsiusTemp = GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode);
			return celsiusTemp.ToStringTemperatureRaw(format);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F9A8 File Offset: 0x0000DBA8
		public static string ToStringTemperatureRaw(this float temp, string format = "F1")
		{
			switch (Prefs.TemperatureMode)
			{
			case TemperatureDisplayMode.Celsius:
				return temp.ToString(format) + "C";
			case TemperatureDisplayMode.Fahrenheit:
				return temp.ToString(format) + "F";
			case TemperatureDisplayMode.Kelvin:
				return temp.ToString(format) + "K";
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000FA10 File Offset: 0x0000DC10
		public static string ToStringTwoDigits(this Vector2 v)
		{
			return string.Concat(new string[]
			{
				"(",
				v.x.ToString("F2"),
				", ",
				v.y.ToString("F2"),
				")"
			});
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000FA68 File Offset: 0x0000DC68
		public static string ToStringWorkAmount(this float workAmount)
		{
			return Mathf.CeilToInt(workAmount / 60f).ToString();
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000FA8C File Offset: 0x0000DC8C
		public static string ToStringBytes(this int b, string format = "F2")
		{
			return ((float)b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000FABC File Offset: 0x0000DCBC
		public static string ToStringBytes(this uint b, string format = "F2")
		{
			return (b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000FAEC File Offset: 0x0000DCEC
		public static string ToStringBytes(this long b, string format = "F2")
		{
			return ((float)b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000FB1C File Offset: 0x0000DD1C
		public static string ToStringBytes(this ulong b, string format = "F2")
		{
			return (b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000FB4C File Offset: 0x0000DD4C
		public static string ToStringReadable(this KeyCode k)
		{
			if (k <= KeyCode.BackQuote)
			{
				switch (k)
				{
				case KeyCode.Backspace:
					return "Bksp";
				case KeyCode.Tab:
				case (KeyCode)10:
				case (KeyCode)11:
				case (KeyCode)14:
				case (KeyCode)15:
				case (KeyCode)16:
				case (KeyCode)17:
				case (KeyCode)18:
				case KeyCode.Pause:
				case (KeyCode)20:
				case (KeyCode)21:
				case (KeyCode)22:
				case (KeyCode)23:
				case (KeyCode)24:
				case (KeyCode)25:
				case (KeyCode)26:
				case (KeyCode)28:
				case (KeyCode)29:
				case (KeyCode)30:
				case (KeyCode)31:
				case KeyCode.Space:
				case KeyCode.Percent:
				case KeyCode.Equals:
					break;
				case KeyCode.Clear:
					return "Clr";
				case KeyCode.Return:
					return "Ent";
				case KeyCode.Escape:
					return "Esc";
				case KeyCode.Exclaim:
					return "!";
				case KeyCode.DoubleQuote:
					return "\"";
				case KeyCode.Hash:
					return "#";
				case KeyCode.Dollar:
					return "$";
				case KeyCode.Ampersand:
					return "&";
				case KeyCode.Quote:
					return "'";
				case KeyCode.LeftParen:
					return "(";
				case KeyCode.RightParen:
					return ")";
				case KeyCode.Asterisk:
					return "*";
				case KeyCode.Plus:
					return "+";
				case KeyCode.Comma:
					return ",";
				case KeyCode.Minus:
					return "-";
				case KeyCode.Period:
					return ".";
				case KeyCode.Slash:
					return "/";
				case KeyCode.Alpha0:
					return "0";
				case KeyCode.Alpha1:
					return "1";
				case KeyCode.Alpha2:
					return "2";
				case KeyCode.Alpha3:
					return "3";
				case KeyCode.Alpha4:
					return "4";
				case KeyCode.Alpha5:
					return "5";
				case KeyCode.Alpha6:
					return "6";
				case KeyCode.Alpha7:
					return "7";
				case KeyCode.Alpha8:
					return "8";
				case KeyCode.Alpha9:
					return "9";
				case KeyCode.Colon:
					return ":";
				case KeyCode.Semicolon:
					return ";";
				case KeyCode.Less:
					return "<";
				case KeyCode.Greater:
					return ">";
				case KeyCode.Question:
					return "?";
				case KeyCode.At:
					return "@";
				default:
					switch (k)
					{
					case KeyCode.LeftBracket:
						return "[";
					case KeyCode.Backslash:
						return "\\";
					case KeyCode.RightBracket:
						return "]";
					case KeyCode.Caret:
						return "^";
					case KeyCode.Underscore:
						return "_";
					case KeyCode.BackQuote:
						return "`";
					}
					break;
				}
			}
			else
			{
				if (k == KeyCode.Delete)
				{
					return "Del";
				}
				switch (k)
				{
				case KeyCode.Keypad0:
					return "Kp0";
				case KeyCode.Keypad1:
					return "Kp1";
				case KeyCode.Keypad2:
					return "Kp2";
				case KeyCode.Keypad3:
					return "Kp3";
				case KeyCode.Keypad4:
					return "Kp4";
				case KeyCode.Keypad5:
					return "Kp5";
				case KeyCode.Keypad6:
					return "Kp6";
				case KeyCode.Keypad7:
					return "Kp7";
				case KeyCode.Keypad8:
					return "Kp8";
				case KeyCode.Keypad9:
					return "Kp9";
				case KeyCode.KeypadPeriod:
					return "Kp.";
				case KeyCode.KeypadDivide:
					return "Kp/";
				case KeyCode.KeypadMultiply:
					return "Kp*";
				case KeyCode.KeypadMinus:
					return "Kp-";
				case KeyCode.KeypadPlus:
					return "Kp+";
				case KeyCode.KeypadEnter:
					return "KpEnt";
				case KeyCode.KeypadEquals:
					return "Kp=";
				case KeyCode.UpArrow:
					return "Up";
				case KeyCode.DownArrow:
					return "Down";
				case KeyCode.RightArrow:
					return "Right";
				case KeyCode.LeftArrow:
					return "Left";
				case KeyCode.Insert:
					return "Ins";
				case KeyCode.Home:
					return "Home";
				case KeyCode.End:
					return "End";
				case KeyCode.PageUp:
					return "PgUp";
				case KeyCode.PageDown:
					return "PgDn";
				case KeyCode.Numlock:
					return "NumL";
				case KeyCode.CapsLock:
					return "CapL";
				case KeyCode.ScrollLock:
					return "ScrL";
				case KeyCode.RightShift:
					return "RShf";
				case KeyCode.LeftShift:
					return "LShf";
				case KeyCode.RightControl:
					return "RCtrl";
				case KeyCode.LeftControl:
					return "LCtrl";
				case KeyCode.RightAlt:
					return "RAlt";
				case KeyCode.LeftAlt:
					return "LAlt";
				case KeyCode.RightCommand:
					return "Appl";
				case KeyCode.LeftCommand:
					return "Cmd";
				case KeyCode.LeftWindows:
					return "Win";
				case KeyCode.RightWindows:
					return "Win";
				case KeyCode.AltGr:
					return "AltGr";
				case KeyCode.Help:
					return "Help";
				case KeyCode.Print:
					return "Prnt";
				case KeyCode.SysReq:
					return "SysReq";
				case KeyCode.Break:
					return "Brk";
				case KeyCode.Menu:
					return "Menu";
				}
			}
			return k.ToString();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000FF9D File Offset: 0x0000E19D
		public static void AppendWithComma(this StringBuilder sb, string text)
		{
			sb.AppendWithSeparator(text, ", ");
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000FFAB File Offset: 0x0000E1AB
		public static void AppendInNewLine(this StringBuilder sb, string text)
		{
			sb.AppendWithSeparator(text, "\n");
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000FFB9 File Offset: 0x0000E1B9
		public static void AppendWithSeparator(this StringBuilder sb, string text, string separator)
		{
			if (text.NullOrEmpty())
			{
				return;
			}
			if (sb.Length > 0)
			{
				sb.Append(separator);
			}
			sb.Append(text);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000FFE0 File Offset: 0x0000E1E0
		public static string WordWrapAt(this string text, float length)
		{
			Text.Font = GameFont.Medium;
			if (text.GetWidthCached() < length)
			{
				return text;
			}
			IEnumerable<Pair<char, int>> source = from p in text.Select((char c, int idx) => new Pair<char, int>(c, idx))
			where p.First == ' '
			select p;
			if (!source.Any<Pair<char, int>>())
			{
				return text;
			}
			Pair<char, int> pair = source.MinBy((Pair<char, int> p) => Mathf.Abs(text.Substring(0, p.Second).GetWidthCached() - text.Substring(p.Second + 1).GetWidthCached()));
			return text.Substring(0, pair.Second) + "\n" + text.Substring(pair.Second + 1);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000100B8 File Offset: 0x0000E2B8
		public static string EventTypeToStringCached(EventType eventType)
		{
			if (GenText.eventTypesCached == null)
			{
				int num = 0;
				foreach (object obj in Enum.GetValues(typeof(EventType)))
				{
					num = Mathf.Max(num, (int)obj);
				}
				GenText.eventTypesCached = new string[num + 1];
				foreach (object obj2 in Enum.GetValues(typeof(EventType)))
				{
					GenText.eventTypesCached[(int)obj2] = obj2.ToString();
				}
			}
			if (eventType >= EventType.MouseDown && eventType < (EventType)GenText.eventTypesCached.Length)
			{
				return GenText.eventTypesCached[(int)eventType];
			}
			return "Unknown";
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x000101AC File Offset: 0x0000E3AC
		public static string FieldsToString<T>(T obj)
		{
			if (obj == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(fieldInfo.Name);
				stringBuilder.Append("=");
				object value = fieldInfo.GetValue(obj);
				if (value == null)
				{
					stringBuilder.Append("null");
				}
				else
				{
					stringBuilder.Append(value.ToString());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00010254 File Offset: 0x0000E454
		public static bool TryGetSeparatedValues(string str, char separator, out string[] output)
		{
			if (str.NullOrEmpty() || !str.Contains(separator))
			{
				output = null;
				return false;
			}
			GenText.separatorArrayTmp[0] = separator;
			output = str.Split(GenText.separatorArrayTmp, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < output.Length; i++)
			{
				output[i] = output[i].Trim();
			}
			return true;
		}

		// Token: 0x0400008A RID: 138
		private const int SaveNameMaxLength = 40;

		// Token: 0x0400008B RID: 139
		private const char DegreeSymbol = '°';

		// Token: 0x0400008C RID: 140
		private static StringBuilder tmpSb = new StringBuilder();

		// Token: 0x0400008D RID: 141
		private static StringBuilder tmpSbForCapitalizedSentences = new StringBuilder();

		// Token: 0x0400008E RID: 142
		private static StringBuilder tmpStringBuilder = new StringBuilder();

		// Token: 0x0400008F RID: 143
		private static string[] eventTypesCached;

		// Token: 0x04000090 RID: 144
		private static readonly char[] separatorArrayTmp = new char[]
		{
			' '
		};
	}
}
