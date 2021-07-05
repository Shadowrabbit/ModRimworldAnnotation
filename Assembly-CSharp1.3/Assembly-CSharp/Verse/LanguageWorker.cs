using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000156 RID: 342
	public abstract class LanguageWorker
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual int TotalNumCaseCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00032840 File Offset: 0x00030A40
		public virtual string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("IndefiniteForm".CanTranslate())
			{
				return "IndefiniteForm".Translate(str);
			}
			return "IndefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x000328A2 File Offset: 0x00030AA2
		public string WithIndefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithIndefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null), plural, name);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x000328B9 File Offset: 0x00030AB9
		public string WithIndefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x000328CC File Offset: 0x00030ACC
		public string WithIndefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, plural, name));
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x000328E0 File Offset: 0x00030AE0
		public virtual string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("DefiniteForm".CanTranslate())
			{
				return "DefiniteForm".Translate(str);
			}
			return "DefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00032942 File Offset: 0x00030B42
		public string WithDefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithDefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null), plural, name);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00032959 File Offset: 0x00030B59
		public string WithDefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0003296C File Offset: 0x00030B6C
		public string WithDefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, plural, name));
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0003297D File Offset: 0x00030B7D
		public virtual string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString();
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00032986 File Offset: 0x00030B86
		public virtual string PostProcessed(string str)
		{
			return str.MergeMultipleSpaces(true);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0003298F File Offset: 0x00030B8F
		public virtual string ToTitleCase(string str)
		{
			return str.CapitalizeFirst();
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00032998 File Offset: 0x00030B98
		public virtual string Pluralize(string str, Gender gender, int count = -1)
		{
			string result;
			if (this.TryLookupPluralForm(str, gender, out result, count))
			{
				return result;
			}
			return str;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x000329B5 File Offset: 0x00030BB5
		public string Pluralize(string str, int count = -1)
		{
			return this.Pluralize(str, LanguageDatabase.activeLanguage.ResolveGender(str, null), count);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x000329CC File Offset: 0x00030BCC
		public virtual bool TryLookupPluralForm(string str, Gender gender, out string plural, int count = -1)
		{
			plural = null;
			if (str.NullOrEmpty())
			{
				return false;
			}
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable("plural");
			if (lookupTable == null)
			{
				return false;
			}
			string key = str.ToLower();
			if (!lookupTable.ContainsKey(key))
			{
				return false;
			}
			string[] array = lookupTable[key];
			if (array.Length < 2)
			{
				return false;
			}
			plural = array[1];
			if (str.Length != 0 && char.IsUpper(str[0]))
			{
				plural = plural.CapitalizeFirst();
			}
			return true;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00032A48 File Offset: 0x00030C48
		public bool TryLookUp(string tableName, string keyName, int index, out string result, string fullStringForReference = null)
		{
			result = null;
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable(tableName);
			if (lookupTable == null)
			{
				return false;
			}
			string key = (keyName != null) ? keyName.ToLower() : null;
			if (keyName.NullOrEmpty() || !lookupTable.ContainsKey(key))
			{
				result = keyName;
				return true;
			}
			string[] array = lookupTable[key];
			if (array.Length < index + 1)
			{
				result = keyName;
				return true;
			}
			result = array[index];
			return true;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x000210E7 File Offset: 0x0001F2E7
		public virtual string PostProcessedKeyedTranslation(string translation)
		{
			return translation;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00032AB0 File Offset: 0x00030CB0
		public virtual string PostProcessThingLabelForRelic(string thingLabel)
		{
			if (thingLabel.IndexOf(' ') != -1)
			{
				return null;
			}
			return thingLabel;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00032AC0 File Offset: 0x00030CC0
		public virtual string ResolveNumCase(float number, List<string> args)
		{
			string formOne = args[0].Trim(new char[]
			{
				'\''
			});
			string text = args[1].Trim(new char[]
			{
				'\''
			});
			string formMany = args[2].Trim(new char[]
			{
				'\''
			});
			if (number - Mathf.Floor(number) > 1E-45f)
			{
				return number + " " + text;
			}
			return number + " " + this.GetFormForNumber((int)number, formOne, text, formMany);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00032B54 File Offset: 0x00030D54
		protected virtual string GetFormForNumber(int num, string formOne, string formSeveral, string formMany)
		{
			int num2 = num % 10;
			if (num / 10 % 10 == 1)
			{
				return formMany;
			}
			if (num2 == 1)
			{
				return formOne;
			}
			if (num2 - 2 > 2)
			{
				return formMany;
			}
			return formSeveral;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00032B88 File Offset: 0x00030D88
		public virtual string ResolveReplace(List<string> args)
		{
			if (args.Count == 0)
			{
				return null;
			}
			string text = args[0];
			if (args.Count == 1)
			{
				return text;
			}
			for (int i = 1; i < args.Count; i++)
			{
				string input = args[i];
				Match match = LanguageWorker.replaceArgRegex.Match(input);
				if (!match.Success)
				{
					return null;
				}
				string value = match.Groups["old"].Value;
				string value2 = match.Groups["new"].Value;
				if (text.Contains(value))
				{
					return text.Replace(value, value2);
				}
			}
			return text;
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00032C25 File Offset: 0x00030E25
		public virtual string ResolveFunction(string functionName, List<string> args, string fullStringForReference)
		{
			if (functionName == "lookup")
			{
				return this.ResolveLookup(args, fullStringForReference);
			}
			if (functionName == "replace")
			{
				return this.ResolveReplace(args);
			}
			return "";
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00032C58 File Offset: 0x00030E58
		protected string ResolveLookup(List<string> args, string fullStringForReference)
		{
			if (args.Count != 2 && args.Count != 3)
			{
				Log.Error("Invalid argument number for 'lookup' function, expected 2 or 3. A key to lookup, table name and optional index if there's more than 1 entry per key. Full string: " + fullStringForReference);
				return "";
			}
			string text = args[1];
			int index = 1;
			if (args.Count == 3 && !int.TryParse(args[2], out index))
			{
				Log.Error("Invalid lookup index value: '" + args[2] + "' Full string: " + fullStringForReference);
				return "";
			}
			string result;
			if (this.TryLookUp(text.ToLower(), args[0], index, out result, fullStringForReference))
			{
				return result;
			}
			return "";
		}

		// Token: 0x04000878 RID: 2168
		private static readonly Regex replaceArgRegex = new Regex("(?<old>[^\"]*?)\"-\"(?<new>[^\"]*?)\"", RegexOptions.Compiled);
	}
}
