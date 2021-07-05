using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000536 RID: 1334
	public static class ColoredText
	{
		// Token: 0x0600224D RID: 8781 RVA: 0x00108080 File Offset: 0x00106280
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

		// Token: 0x0600224E RID: 8782 RVA: 0x0001D804 File Offset: 0x0001BA04
		public static void ClearCache()
		{
			ColoredText.cache.Clear();
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x0010814C File Offset: 0x0010634C
		public static TaggedString ApplyTag(this string s, TagType tagType, string arg = null)
		{
			if (arg == null)
			{
				return string.Format("(*{0}){1}(/{0})", tagType.ToString(), s);
			}
			return string.Format("(*{0}={1}){2}(/{0})", tagType.ToString(), arg, s);
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x0001D810 File Offset: 0x0001BA10
		public static TaggedString ApplyTag(this string s, Faction faction)
		{
			if (faction == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Faction, faction.GetUniqueLoadID());
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00108198 File Offset: 0x00106398
		public static string StripTags(this string s)
		{
			if (s.NullOrEmpty() || (s.IndexOf("(*") < 0 && s.IndexOf('<') < 0))
			{
				return s;
			}
			s = ColoredText.XMLRegex.Replace(s, string.Empty);
			return ColoredText.TagRegex.Replace(s, string.Empty);
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x0001D829 File Offset: 0x0001BA29
		public static string ResolveTags(this string str)
		{
			return ColoredText.Resolve(str);
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x001081EC File Offset: 0x001063EC
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

		// Token: 0x06002254 RID: 8788 RVA: 0x0001D836 File Offset: 0x0001BA36
		public static string Colorize(this string s, Color color)
		{
			return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), s);
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x0010843C File Offset: 0x0010663C
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
					Log.Error("No faction found with UniqueLoadID '" + arg + "'", false);
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
					Log.Error("No faction found with UniqueLoadID '" + arg + "'", false);
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
			default:
				Log.Error("Invalid tag '" + tag + "'", false);
				return text;
			}
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x0010859C File Offset: 0x0010679C
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

		// Token: 0x06002257 RID: 8791 RVA: 0x001085FC File Offset: 0x001067FC
		private static T ParseEnum<T>(string value, bool ignoreCase = true)
		{
			if (Enum.IsDefined(typeof(T), value))
			{
				return (T)((object)Enum.Parse(typeof(T), value, ignoreCase));
			}
			return default(T);
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x0001D849 File Offset: 0x0001BA49
		public static void AppendTagged(this StringBuilder sb, TaggedString taggedString)
		{
			sb.Append(taggedString.Resolve());
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x0001D859 File Offset: 0x0001BA59
		public static void AppendLineTagged(this StringBuilder sb, TaggedString taggedString)
		{
			sb.AppendLine(taggedString.Resolve());
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x0001D869 File Offset: 0x0001BA69
		public static TaggedString ToTaggedString(this StringBuilder sb)
		{
			return new TaggedString(sb.ToString());
		}

		// Token: 0x0400172F RID: 5935
		private static StringBuilder resultBuffer = new StringBuilder();

		// Token: 0x04001730 RID: 5936
		private static StringBuilder tagBuffer = new StringBuilder();

		// Token: 0x04001731 RID: 5937
		private static StringBuilder argBuffer = new StringBuilder();

		// Token: 0x04001732 RID: 5938
		private static Dictionary<string, string> cache = new Dictionary<string, string>();

		// Token: 0x04001733 RID: 5939
		private static ColoredText.CaptureStage capStage = ColoredText.CaptureStage.Result;

		// Token: 0x04001734 RID: 5940
		private static Regex DaysRegex;

		// Token: 0x04001735 RID: 5941
		private static Regex HoursRegex;

		// Token: 0x04001736 RID: 5942
		private static Regex SecondsRegex;

		// Token: 0x04001737 RID: 5943
		private static Regex ColonistCountRegex;

		// Token: 0x04001738 RID: 5944
		public static readonly Color RedReadable = new Color(1f, 0.2f, 0.2f);

		// Token: 0x04001739 RID: 5945
		public static readonly Color NameColor = GenColor.FromHex("d09b61");

		// Token: 0x0400173A RID: 5946
		public static readonly Color CurrencyColor = GenColor.FromHex("dbb40c");

		// Token: 0x0400173B RID: 5947
		public static readonly Color DateTimeColor = GenColor.FromHex("87f6f6");

		// Token: 0x0400173C RID: 5948
		public static readonly Color FactionColor_Ally = GenColor.FromHex("00ff00");

		// Token: 0x0400173D RID: 5949
		public static readonly Color FactionColor_Hostile = ColoredText.RedReadable;

		// Token: 0x0400173E RID: 5950
		public static readonly Color ThreatColor = GenColor.FromHex("d46f68");

		// Token: 0x0400173F RID: 5951
		public static readonly Color FactionColor_Neutral = GenColor.FromHex("00bfff");

		// Token: 0x04001740 RID: 5952
		public static readonly Color WarningColor = GenColor.FromHex("ff0000");

		// Token: 0x04001741 RID: 5953
		public static readonly Color ColonistCountColor = GenColor.FromHex("dcffaf");

		// Token: 0x04001742 RID: 5954
		private static readonly Regex CurrencyRegex = new Regex("\\$\\d+\\.?\\d*");

		// Token: 0x04001743 RID: 5955
		private static readonly Regex TagRegex = new Regex("\\([\\*\\/][^\\)]*\\)");

		// Token: 0x04001744 RID: 5956
		private static readonly Regex XMLRegex = new Regex("<[^>]*>");

		// Token: 0x04001745 RID: 5957
		private const string Digits = "\\d+\\.?\\d*";

		// Token: 0x04001746 RID: 5958
		private const string Replacement = "$&";

		// Token: 0x04001747 RID: 5959
		private const string TagStartString = "(*";

		// Token: 0x04001748 RID: 5960
		private const char TagStartChar = '(';

		// Token: 0x04001749 RID: 5961
		private const char TagEndChar = ')';

		// Token: 0x02000537 RID: 1335
		private enum CaptureStage
		{
			// Token: 0x0400174B RID: 5963
			Tag,
			// Token: 0x0400174C RID: 5964
			Arg,
			// Token: 0x0400174D RID: 5965
			Result
		}
	}
}
