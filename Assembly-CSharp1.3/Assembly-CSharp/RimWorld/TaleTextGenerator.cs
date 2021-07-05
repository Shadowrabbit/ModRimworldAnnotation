using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200102E RID: 4142
	public static class TaleTextGenerator
	{
		// Token: 0x060061CC RID: 25036 RVA: 0x00213DA4 File Offset: 0x00211FA4
		public static TaggedString GenerateTextFromTale(TextGenerationPurpose purpose, Tale tale, int seed, RulePackDef extraInclude)
		{
			Rand.PushState();
			Rand.Seed = seed;
			string rootKeyword = null;
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(extraInclude);
			if (purpose == TextGenerationPurpose.ArtDescription)
			{
				rootKeyword = "r_art_description";
				if (tale != null && !Rand.Chance(0.2f))
				{
					request.Includes.Add(RulePackDefOf.ArtDescriptionRoot_HasTale);
					request.IncludesBare.AddRange(tale.GetTextGenerationIncludes());
					request.Rules.AddRange(tale.GetTextGenerationRules(request.Constants));
				}
				else
				{
					request.Includes.Add(RulePackDefOf.ArtDescriptionRoot_Taleless);
					request.Includes.Add(RulePackDefOf.TalelessImages);
				}
				request.Includes.Add(RulePackDefOf.ArtDescriptionUtility_Global);
			}
			else if (purpose == TextGenerationPurpose.ArtName)
			{
				rootKeyword = "r_art_name";
				if (tale != null)
				{
					request.IncludesBare.AddRange(tale.GetTextGenerationIncludes());
					request.Rules.AddRange(tale.GetTextGenerationRules(request.Constants));
				}
			}
			string str = GrammarResolver.Resolve(rootKeyword, request, (tale != null) ? tale.def.defName : "null_tale", false, null, null, null, true);
			Rand.PopState();
			return str;
		}

		// Token: 0x040037C3 RID: 14275
		private const float TalelessChanceWithTales = 0.2f;
	}
}
