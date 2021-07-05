using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038F RID: 911
	public static class ColoredText
	{
		// Token: 0x06001ABE RID: 6846 RVA: 0x00099FD8 File Offset: 0x000981D8
		public static void ResetStaticData()
		{
			ColoredText.DaysRegex = new Regex(string.Format("PeriodDays".Translate(), "\\d+\\.?\\d*"));
			ColoredText.HoursRegex = new Regex(string.Format("PeriodHours".Translate(), "\\d+\\.?\\d*"));
			ColoredText.SecondsRegex = new Regex(string.Format("PeriodSeconds".Translate(), "\\d+\\.?\\d*"));
			string str = string.Concat(new string[]
			{
				"(",
				FactionDefOf.PlayerColony.pawnsPlural,
				"|",
				FactionDefOf.PlayerColony.pawnSingular,
				")"
			});
			ColoredText.ColonistCountRegex = new Regex("\\d+\\.?\\d* " + str);
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x0009A0A1 File Offset: 0x000982A1
		public static void ClearCache()
		{
			ColoredText.cache.Clear();
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x0009A0B0 File Offset: 0x000982B0
		public static TaggedString ApplyTag(this string s, TagType tagType, string arg = null)
		{
			if (arg == null)
			{
				return string.Format("(*{0}){1}(/{0})", tagType.ToString(), s);
			}
			return string.Format("(*{0}={1}){2}(/{0})", tagType.ToString(), arg, s);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x0009A0FC File Offset: 0x000982FC
		public static TaggedString ApplyTag(this string s, Faction faction)
		{
			if (faction == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Faction, faction.GetUniqueLoadID());
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0009A115 File Offset: 0x00098315
		public static TaggedString ApplyTag(this string s, Ideo ideo)
		{
			if (ideo == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Ideo, ideo.GetUniqueLoadID());
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x0009A130 File Offset: 0x00098330
		public static string StripTags(this string s)
		{
			if (s.NullOrEmpty() || (s.IndexOf("(*") < 0 && s.IndexOf('<') < 0))
			{
				return s;
			}
			s = ColoredText.XMLRegex.Replace(s, string.Empty);
			return ColoredText.TagRegex.Replace(s, string.Empty);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x0009A182 File Offset: 0x00098382
		public static string ResolveTags(this string str)
		{
			return ColoredText.Resolve(str);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x0009A190 File Offset: 0x00098390
		public static string Resolve(TaggedString taggedStr)
		{
			if (taggedStr == null)
			{
				return null;
			}
			string rawText = taggedStr.RawText;
			if (rawText.NullOrEmpty())
			{
				return rawText;
			}
			string result;
			if (ColoredText.cache.TryGetValue(rawText, out result))
			{
				return result;
			}
			ColoredText.resultBuffer.Length = 0;
			if (rawText.IndexOf("(*") < 0)
			{
				ColoredText.resultBuffer.Append(rawText);
			}
			else
			{
				for (int i = 0; i < rawText.Length; i++)
				{
					char c = rawText[i];
					if (c == '(' && i < rawText.Length - 1 && rawText[i + 1] == '*' && rawText.IndexOf(')', i) > i + 1)
					{
						bool flag = false;
						int num = i;
						ColoredText.tagBuffer.Length = 0;
						ColoredText.argBuffer.Length = 0;
						ColoredText.capStage = ColoredText.CaptureStage.Tag;
						for (i += 2; i < rawText.Length; i++)
						{
							char c2 = rawText[i];
							if (c2 == ')')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Result;
								if (flag)
								{
									string value = rawText.Substring(num, i - num + 1).SwapTagWithColor(ColoredText.tagBuffer.ToString(), ColoredText.argBuffer.ToString());
									ColoredText.resultBuffer.Append(value);
									break;
								}
							}
							else if (c2 == '/')
							{
								flag = true;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Arg)
							{
								ColoredText.argBuffer.Append(c2);
							}
							if (!flag && c2 == '=')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Arg;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Tag)
							{
								ColoredText.tagBuffer.Append(c2);
							}
						}
						if (!flag)
						{
							ColoredText.resultBuffer.Append(c);
							i = num + 1;
						}
					}
					else
					{
						ColoredText.resultBuffer.Append(c);
					}
				}
			}
			string text = ColoredText.resultBuffer.ToString();
			text = ColoredText.CurrencyRegex.Replace(text, "$&".Colorize(ColoredText.CurrencyColor));
			text = ColoredText.DaysRegex.Replace(text, "$&".Colorize(ColoredText.DateTimeColor));
			text = ColoredText.HoursRegex.Replace(text, "$&".Colorize(ColoredText.DateTimeColor));
			text = ColoredText.SecondsRegex.Replace(text, "$&".Colorize(ColoredText.DateTimeColor));
			text = ColoredText.ColonistCountRegex.Replace(text, "$&".Colorize(ColoredText.ColonistCountColor));
			ColoredText.cache.Add(rawText, text);
			return text;
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x0009A3DE File Offset: 0x000985DE
		public static string Colorize(this TaggedString ts, Color color)
		{
			return ts.Resolve().Colorize(color);
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x0009A3ED File Offset: 0x000985ED
		public static string Colorize(this string s, Color color)
		{
			return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), s);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x0009A400 File Offset: 0x00098600
		private static string SwapTagWithColor(this string str, string tag, string arg)
		{
			TagType tagType = ColoredText.ParseEnum<TagType>(tag.CapitalizeFirst(), true);
			string text = str.StripTags();
			switch (tagType)
			{
			case TagType.Undefined:
				return str;
			case TagType.Name:
				return text.Colorize(ColoredText.NameColor);
			case TagType.Faction:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction = Find.FactionManager.AllFactions.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction == null)
				{
					return text.Colorize(ColoredText.SubtleGrayColor);
				}
				return text.Colorize(ColoredText.GetFactionRelationColor(faction));
			}
			case TagType.Settlement:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction2 = Find.FactionManager.AllFactionsVisible.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction2 == null)
				{
					return text.Colorize(ColoredText.SubtleGrayColor);
				}
				if (faction2 == null)
				{
					return text;
				}
				return text.Colorize(faction2.Color);
			}
			case TagType.DateTime:
				return text.Colorize(ColoredText.DateTimeColor);
			case TagType.ColonistCount:
				return text.Colorize(ColoredText.ColonistCountColor);
			case TagType.Threat:
				return text.Colorize(ColoredText.ThreatColor);
			case TagType.Ideo:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Ideo ideo = Find.IdeoManager.IdeosListForReading.Find((Ideo x) => x.GetUniqueLoadID() == arg);
				if (ideo == null)
				{
					Log.ErrorOnce("No ideoligion found with UniqueLoadID '" + arg + "'", arg.GetHashCode());
				}
				if (ideo == null)
				{
					return text;
				}
				return text.Colorize(ideo.Color);
			}
			case TagType.SectionTitle:
				return text.Colorize(ColoredText.TipSectionTitleColor);
			default:
				Log.ErrorOnce("Invalid tag '" + tag + "'", tag.GetHashCode());
				return text;
			}
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0009A5C4 File Offset: 0x000987C4
		private static Color GetFactionRelationColor(Faction faction)
		{
			if (faction == null)
			{
				return Color.white;
			}
			if (faction.IsPlayer)
			{
				return faction.Color;
			}
			switch (faction.RelationKindWith(Faction.OfPlayer))
			{
			case FactionRelationKind.Hostile:
				return ColoredText.FactionColor_Hostile;
			case FactionRelationKind.Neutral:
				return ColoredText.FactionColor_Neutral;
			case FactionRelationKind.Ally:
				return ColoredText.FactionColor_Ally;
			default:
				return faction.Color;
			}
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x0009A624 File Offset: 0x00098824
		private static T ParseEnum<T>(string value, bool ignoreCase = true)
		{
			if (Enum.IsDefined(typeof(T), value))
			{
				return (T)((object)Enum.Parse(typeof(T), value, ignoreCase));
			}
			return default(T);
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x0009A663 File Offset: 0x00098863
		public static void AppendTagged(this StringBuilder sb, TaggedString taggedString)
		{
			sb.Append(taggedString.Resolve());
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x0009A673 File Offset: 0x00098873
		public static void AppendLineTagged(this StringBuilder sb, TaggedString taggedString)
		{
			sb.AppendLine(taggedString.Resolve());
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0009A683 File Offset: 0x00098883
		public static TaggedString ToTaggedString(this StringBuilder sb)
		{
			return new TaggedString(sb.ToString());
		}

		// Token: 0x04001157 RID: 4439
		private static StringBuilder resultBuffer = new StringBuilder();

		// Token: 0x04001158 RID: 4440
		private static StringBuilder tagBuffer = new StringBuilder();

		// Token: 0x04001159 RID: 4441
		private static StringBuilder argBuffer = new StringBuilder();

		// Token: 0x0400115A RID: 4442
		private static Dictionary<string, string> cache = new Dictionary<string, string>();

		// Token: 0x0400115B RID: 4443
		private static ColoredText.CaptureStage capStage = ColoredText.CaptureStage.Result;

		// Token: 0x0400115C RID: 4444
		private static Regex DaysRegex;

		// Token: 0x0400115D RID: 4445
		private static Regex HoursRegex;

		// Token: 0x0400115E RID: 4446
		private static Regex SecondsRegex;

		// Token: 0x0400115F RID: 4447
		private static Regex ColonistCountRegex;

		// Token: 0x04001160 RID: 4448
		public static readonly Color NameColor = GenColor.FromHex("d09b61");

		// Token: 0x04001161 RID: 4449
		public static readonly Color CurrencyColor = GenColor.FromHex("dbb40c");

		// Token: 0x04001162 RID: 4450
		public static readonly Color TipSectionTitleColor = new Color(0.9f, 0.9f, 0.3f);

		// Token: 0x04001163 RID: 4451
		public static readonly Color DateTimeColor = GenColor.FromHex("87f6f6");

		// Token: 0x04001164 RID: 4452
		public static readonly Color FactionColor_Ally = GenColor.FromHex("00ff00");

		// Token: 0x04001165 RID: 4453
		public static readonly Color FactionColor_Hostile = ColorLibrary.RedReadable;

		// Token: 0x04001166 RID: 4454
		public static readonly Color ThreatColor = GenColor.FromHex("d46f68");

		// Token: 0x04001167 RID: 4455
		public static readonly Color FactionColor_Neutral = GenColor.FromHex("00bfff");

		// Token: 0x04001168 RID: 4456
		public static readonly Color WarningColor = GenColor.FromHex("ff0000");

		// Token: 0x04001169 RID: 4457
		public static readonly Color ColonistCountColor = GenColor.FromHex("dcffaf");

		// Token: 0x0400116A RID: 4458
		public static readonly Color SubtleGrayColor = GenColor.FromHex("999999");

		// Token: 0x0400116B RID: 4459
		public static readonly Color ExpectationsColor = new Color(0.57f, 0.9f, 0.69f);

		// Token: 0x0400116C RID: 4460
		public static readonly Color ImpactColor = GenColor.FromHex("c79fef");

		// Token: 0x0400116D RID: 4461
		private static readonly Regex CurrencyRegex = new Regex("\\$\\d+\\.?\\d*");

		// Token: 0x0400116E RID: 4462
		private static readonly Regex TagRegex = new Regex("\\([\\*\\/][^\\)]*\\)");

		// Token: 0x0400116F RID: 4463
		private static readonly Regex XMLRegex = new Regex("<[^>]*>");

		// Token: 0x04001170 RID: 4464
		private const string Digits = "\\d+\\.?\\d*";

		// Token: 0x04001171 RID: 4465
		private const string Replacement = "$&";

		// Token: 0x04001172 RID: 4466
		private const string TagStartString = "(*";

		// Token: 0x04001173 RID: 4467
		private const char TagStartChar = '(';

		// Token: 0x04001174 RID: 4468
		private const char TagEndChar = ')';

		// Token: 0x02001A8D RID: 6797
		private enum CaptureStage
		{
			// Token: 0x040065A5 RID: 26021
			Tag,
			// Token: 0x040065A6 RID: 26022
			Arg,
			// Token: 0x040065A7 RID: 26023
			Result
		}
	}
}
