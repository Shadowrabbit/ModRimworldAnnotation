using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001493 RID: 5267
	public static class NameGenerator
	{
		// Token: 0x06007DE5 RID: 32229 RVA: 0x002C9958 File Offset: 0x002C7B58
		public static string GenerateName(RulePackDef rootPack, IEnumerable<string> extantNames, bool appendNumberIfNameUsed = false, string rootKeyword = null)
		{
			return NameGenerator.GenerateName(rootPack, (string x) => !extantNames.Contains(x), appendNumberIfNameUsed, rootKeyword, null);
		}

		// Token: 0x06007DE6 RID: 32230 RVA: 0x002C9988 File Offset: 0x002C7B88
		public static string GenerateName(RulePackDef rootPack, Predicate<string> validator = null, bool appendNumberIfNameUsed = false, string rootKeyword = null, string testPawnNameSymbol = null)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(rootPack);
			if (testPawnNameSymbol != null)
			{
				request.Rules.Add(new Rule_String("ANYPAWN_nameDef", testPawnNameSymbol));
				request.Rules.Add(new Rule_String("ANYPAWN_nameIndef", testPawnNameSymbol));
			}
			string rootKeyword2 = (rootKeyword != null) ? rootKeyword : rootPack.FirstRuleKeyword;
			string untranslatedRootKeyword = (rootKeyword != null) ? rootKeyword : rootPack.FirstUntranslatedRuleKeyword;
			return NameGenerator.GenerateName(request, validator, appendNumberIfNameUsed, rootKeyword2, untranslatedRootKeyword);
		}

		// Token: 0x06007DE7 RID: 32231 RVA: 0x002C9A04 File Offset: 0x002C7C04
		public static string GenerateName(GrammarRequest request, Predicate<string> validator = null, bool appendNumberIfNameUsed = false, string rootKeyword = null, string untranslatedRootKeyword = null)
		{
			if (untranslatedRootKeyword == null)
			{
				untranslatedRootKeyword = rootKeyword;
			}
			string text = "ErrorName";
			if (appendNumberIfNameUsed)
			{
				for (int i = 0; i < 100; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						text = GenText.CapitalizeAsTitle(GrammarResolver.Resolve(rootKeyword, request, null, false, untranslatedRootKeyword, null, null, true));
						if (i != 0)
						{
							text = text + " " + (i + 1);
						}
						if (validator == null || validator(text))
						{
							return text;
						}
					}
				}
				return GenText.CapitalizeAsTitle(GrammarResolver.Resolve(rootKeyword, request, null, false, untranslatedRootKeyword, null, null, true));
			}
			for (int k = 0; k < 150; k++)
			{
				text = GenText.CapitalizeAsTitle(GrammarResolver.Resolve(rootKeyword, request, null, false, untranslatedRootKeyword, null, null, true));
				if (validator == null || validator(text))
				{
					return text;
				}
			}
			Log.Error("Could not get new name (first rule pack: " + request.Includes.FirstOrDefault<RulePackDef>().ToStringSafe<RulePackDef>() + ")");
			return text;
		}
	}
}
