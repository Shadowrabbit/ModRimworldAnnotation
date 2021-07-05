using System;

namespace Verse
{
	// Token: 0x0200020C RID: 524
	public class LanguageWorker_Catalan : LanguageWorker
	{
		// Token: 0x06000D95 RID: 3477 RVA: 0x00010396 File Offset: 0x0000E596
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "unes " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x000103D6 File Offset: 0x0000E5D6
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "les " : "els ") + str;
			}
			return this.WithElLaArticle(str, gender, false);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000ADDB8 File Offset: 0x000ABFB8
		private string WithElLaArticle(string str, Gender gender, bool name)
		{
			if (str.Length == 0 || (!this.IsVowel(str[0]) && str[0] != 'h' && str[0] != 'H'))
			{
				return ((gender == Gender.Female) ? "la " : "el ") + str;
			}
			if (name)
			{
				return ((gender == Gender.Female) ? "l'" : "n'") + str;
			}
			return "l'" + str;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x000ADE30 File Offset: 0x000AC030
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			if (gender == Gender.Female)
			{
				return number + "a";
			}
			if (number == 1 || number == 3)
			{
				return number + "r";
			}
			if (number == 2)
			{
				return number + "n";
			}
			if (number == 4)
			{
				return number + "t";
			}
			return number + "è";
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00010409 File Offset: 0x0000E609
		public bool IsVowel(char ch)
		{
			return "ieɛaoɔuəuàêèéòóüúIEƐAOƆUƏUÀÊÈÉÒÓÜÚ".IndexOf(ch) >= 0;
		}
	}
}
