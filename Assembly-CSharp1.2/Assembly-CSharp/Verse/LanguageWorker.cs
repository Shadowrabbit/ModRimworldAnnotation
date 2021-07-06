using System;

namespace Verse
{
	// Token: 0x0200020B RID: 523
	public abstract class LanguageWorker
	{
		// Token: 0x06000D86 RID: 3462 RVA: 0x000ADCF0 File Offset: 0x000ABEF0
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

		// Token: 0x06000D87 RID: 3463 RVA: 0x000102EA File Offset: 0x0000E4EA
		public string WithIndefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithIndefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null), plural, name);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00010301 File Offset: 0x0000E501
		public string WithIndefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00010314 File Offset: 0x0000E514
		public string WithIndefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, plural, name));
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000ADD54 File Offset: 0x000ABF54
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

		// Token: 0x06000D8B RID: 3467 RVA: 0x00010325 File Offset: 0x0000E525
		public string WithDefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithDefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null), plural, name);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0001033C File Offset: 0x0000E53C
		public string WithDefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0001034F File Offset: 0x0000E54F
		public string WithDefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, plural, name));
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00010360 File Offset: 0x0000E560
		public virtual string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString();
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00010369 File Offset: 0x0000E569
		public virtual string PostProcessed(string str)
		{
			str = str.MergeMultipleSpaces(true);
			return str;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00010375 File Offset: 0x0000E575
		public virtual string ToTitleCase(string str)
		{
			return str.CapitalizeFirst();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0001037D File Offset: 0x0000E57D
		public virtual string Pluralize(string str, Gender gender, int count = -1)
		{
			return str;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00010380 File Offset: 0x0000E580
		public string Pluralize(string str, int count = -1)
		{
			return this.Pluralize(str, LanguageDatabase.activeLanguage.ResolveGender(str, null), count);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0001037D File Offset: 0x0000E57D
		public virtual string PostProcessedKeyedTranslation(string translation)
		{
			return translation;
		}
	}
}
