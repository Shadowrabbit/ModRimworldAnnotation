﻿using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000066 RID: 102
	public static class TitleCaseHelper
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x00009B0D File Offset: 0x00007D0D
		public static bool IsUppercaseTitleWord(string word)
		{
			return word.Length > 1 && !TitleCaseHelper.NonUppercaseWords.Contains(word);
		}

		// Token: 0x040001D0 RID: 464
		private static HashSet<string> NonUppercaseWords = new HashSet<string>
		{
			"a",
			"aboard",
			"about",
			"above",
			"absent",
			"across",
			"after",
			"against",
			"along",
			"alongside",
			"amid",
			"amidst",
			"among",
			"amongst",
			"an",
			"and",
			"around",
			"as",
			"as",
			"aslant",
			"astride",
			"at",
			"athwart",
			"atop",
			"barring",
			"before",
			"behind",
			"below",
			"beneath",
			"beside",
			"besides",
			"between",
			"beyond",
			"but",
			"by",
			"despite",
			"down",
			"during",
			"except",
			"failing",
			"following",
			"for",
			"from",
			"in",
			"inside",
			"into",
			"like",
			"mid",
			"minus",
			"near",
			"next",
			"nor",
			"of",
			"off",
			"on",
			"onto",
			"opposite",
			"or",
			"out",
			"outside",
			"over",
			"past",
			"per",
			"plus",
			"regarding",
			"round",
			"save",
			"since",
			"so",
			"than",
			"the",
			"through",
			"throughout",
			"till",
			"times",
			"to",
			"toward",
			"towards",
			"under",
			"underneath",
			"unlike",
			"until",
			"up",
			"upon",
			"via",
			"vs.",
			"vs",
			"when",
			"with",
			"within",
			"without",
			"worth",
			"yet"
		};
	}
}
