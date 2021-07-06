﻿using System;

namespace Verse
{
	// Token: 0x02000212 RID: 530
	public class LanguageWorker_German : LanguageWorker
	{
		// Token: 0x06000DB3 RID: 3507 RVA: 0x000AEB24 File Offset: 0x000ACD24
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			switch (gender)
			{
			case Gender.None:
				return "ein " + str;
			case Gender.Male:
				return "ein " + str;
			case Gender.Female:
				return "eine " + str;
			default:
				return str;
			}
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x000AEB70 File Offset: 0x000ACD70
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			switch (gender)
			{
			case Gender.None:
				return "das " + str;
			case Gender.Male:
				return "der " + str;
			case Gender.Female:
				return "die " + str;
			default:
				return str;
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00010577 File Offset: 0x0000E777
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + ".";
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000AEBBC File Offset: 0x000ACDBC
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			char c = str[str.Length - 1];
			char c2 = (str.Length >= 2) ? str[str.Length - 2] : '\0';
			switch (gender)
			{
			case Gender.None:
				if (c == 'r' && c2 == 'e')
				{
					return str;
				}
				if (c == 'l' && c2 == 'e')
				{
					return str;
				}
				if (c == 'n' && c2 == 'e')
				{
					return str;
				}
				if (c == 'R' && c2 == 'E')
				{
					return str;
				}
				if (c == 'L' && c2 == 'E')
				{
					return str;
				}
				if (c == 'N' && c2 == 'E')
				{
					return str;
				}
				if (char.IsUpper(c))
				{
					return str + "EN";
				}
				return str + "en";
			case Gender.Male:
				if (c == 'r' && c2 == 'e')
				{
					return str;
				}
				if (c == 'l' && c2 == 'e')
				{
					return str;
				}
				if (c == 'R' && c2 == 'E')
				{
					return str;
				}
				if (c == 'L' && c2 == 'E')
				{
					return str;
				}
				if (char.IsUpper(c))
				{
					return str + "E";
				}
				return str + "e";
			case Gender.Female:
				if (c == 'e')
				{
					return str + "n";
				}
				if (c == 'E')
				{
					return str + "N";
				}
				if (c == 'n' && c2 == 'i')
				{
					return str + "nen";
				}
				if (c == 'N' && c2 == 'I')
				{
					return str + "NEN";
				}
				if (char.IsUpper(c))
				{
					return str + "EN";
				}
				return str + "en";
			default:
				return str;
			}
		}
	}
}
