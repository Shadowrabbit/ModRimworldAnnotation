using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse.Grammar
{
	// Token: 0x02000536 RID: 1334
	public static class GrammarUtility
	{
		// Token: 0x06002832 RID: 10290 RVA: 0x000F5704 File Offset: 0x000F3904
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Pawn pawn, Dictionary<string, string> constants = null, bool addRelationInfoSymbol = true, bool addTags = true)
		{
			if (pawn == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null pawn", pawnSymbol), 16015097);
				return Enumerable.Empty<Rule>();
			}
			TaggedString taggedString = "";
			if (addRelationInfoSymbol)
			{
				PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
			}
			return GrammarUtility.RulesForPawn(pawnSymbol, pawn.Name, (pawn.story != null) ? pawn.story.Title : null, pawn.kindDef, pawn.gender, pawn.Faction, pawn.ageTracker.AgeBiologicalYears, pawn.ageTracker.AgeChronologicalYears, taggedString, PawnUtility.EverBeenColonistOrTameAnimal(pawn), PawnUtility.EverBeenQuestLodger(pawn), pawn.Faction != null && pawn.Faction.leader == pawn, (pawn.royalty != null) ? pawn.royalty.AllTitlesForReading : null, constants, addTags);
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x000F57D4 File Offset: 0x000F39D4
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Name name, string title, PawnKindDef kind, Gender gender, Faction faction, int age, int chronologicalAge, string relationInfo, bool everBeenColonistOrTameAnimal, bool everBeenQuestLodger, bool isFactionLeader, List<RoyalTitle> royalTitles, Dictionary<string, string> constants = null, bool addTags = true)
		{
			string prefix = "";
			if (!pawnSymbol.NullOrEmpty())
			{
				prefix = prefix + pawnSymbol + "_";
			}
			string kindLabel;
			if (gender == Gender.Female)
			{
				kindLabel = (kind.labelFemale.NullOrEmpty() ? kind.label : kind.labelFemale);
			}
			else if (gender == Gender.Male)
			{
				kindLabel = (kind.labelMale.NullOrEmpty() ? kind.label : kind.labelMale);
			}
			else
			{
				kindLabel = kind.label;
			}
			string text;
			if (name != null)
			{
				text = Find.ActiveLanguageWorker.WithIndefiniteArticle(name.ToStringFull, gender, false, true);
			}
			else
			{
				text = Find.ActiveLanguageWorker.WithIndefiniteArticle(kindLabel, gender, false, false);
			}
			yield return new Rule_String(prefix + "nameFull", addTags ? text.ApplyTag(TagType.Name, null).Resolve() : text);
			string nameShort;
			if (name != null)
			{
				nameShort = name.ToStringShort;
			}
			else
			{
				nameShort = kindLabel;
			}
			yield return new Rule_String(prefix + "label", addTags ? nameShort.ApplyTag(TagType.Name, null).Resolve() : nameShort);
			string nameShortDef;
			if (name != null)
			{
				nameShortDef = Find.ActiveLanguageWorker.WithDefiniteArticle(name.ToStringShort, gender, false, true);
			}
			else
			{
				nameShortDef = Find.ActiveLanguageWorker.WithDefiniteArticle(kindLabel, gender, false, false);
			}
			yield return new Rule_String(prefix + "definite", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			yield return new Rule_String(prefix + "nameDef", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			nameShortDef = null;
			if (name != null)
			{
				nameShortDef = Find.ActiveLanguageWorker.WithIndefiniteArticle(name.ToStringShort, gender, false, true);
			}
			else
			{
				nameShortDef = Find.ActiveLanguageWorker.WithIndefiniteArticle(kindLabel, gender, false, false);
			}
			yield return new Rule_String(prefix + "indefinite", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			yield return new Rule_String(prefix + "nameIndef", addTags ? nameShortDef.ApplyTag(TagType.Name, null).Resolve() : nameShortDef);
			nameShortDef = null;
			yield return new Rule_String(prefix + "pronoun", gender.GetPronoun());
			yield return new Rule_String(prefix + "possessive", gender.GetPossessive());
			yield return new Rule_String(prefix + "objective", gender.GetObjective());
			if (faction != null)
			{
				yield return new Rule_String(prefix + "factionName", addTags ? faction.Name.ApplyTag(faction).Resolve() : faction.Name);
				if (constants != null)
				{
					constants[prefix + "faction"] = faction.def.defName;
				}
			}
			if (constants != null && isFactionLeader)
			{
				constants[prefix + "factionLeader"] = "True";
			}
			if (kind != null)
			{
				yield return new Rule_String(prefix + "kind", GenLabel.BestKindLabel(kind, gender, false, -1));
			}
			if (title != null)
			{
				yield return new Rule_String(prefix + "title", title);
				yield return new Rule_String(prefix + "titleIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(title, gender, false, false));
				yield return new Rule_String(prefix + "titleDef", Find.ActiveLanguageWorker.WithDefiniteArticle(title, gender, false, false));
			}
			if (royalTitles != null)
			{
				int royalTitleIndex = 0;
				RoyalTitle bestTitle = null;
				foreach (RoyalTitle royalTitle in from x in royalTitles
				orderby x.def.index
				select x)
				{
					yield return new Rule_String(prefix + "royalTitle" + royalTitleIndex, royalTitle.def.GetLabelFor(gender));
					yield return new Rule_String(string.Concat(new object[]
					{
						prefix,
						"royalTitle",
						royalTitleIndex,
						"Indef"
					}), Find.ActiveLanguageWorker.WithIndefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(string.Concat(new object[]
					{
						prefix,
						"royalTitle",
						royalTitleIndex,
						"Def"
					}), Find.ActiveLanguageWorker.WithDefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "royalTitleFaction" + royalTitleIndex, royalTitle.faction.Name.ApplyTag(royalTitle.faction).Resolve());
					if (royalTitle.faction == faction)
					{
						yield return new Rule_String(prefix + "royalTitleInCurrentFaction", royalTitle.def.GetLabelFor(gender));
						yield return new Rule_String(prefix + "royalTitleInCurrentFactionIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
						yield return new Rule_String(prefix + "royalTitleInCurrentFactionDef", Find.ActiveLanguageWorker.WithDefiniteArticle(royalTitle.def.GetLabelFor(gender), false, false));
						if (constants != null)
						{
							constants[prefix + "royalInCurrentFaction"] = "True";
						}
					}
					if (bestTitle == null || royalTitle.def.favorCost > bestTitle.def.favorCost)
					{
						bestTitle = royalTitle;
					}
					int num = royalTitleIndex;
					royalTitleIndex = num + 1;
					royalTitle = null;
				}
				IEnumerator<RoyalTitle> enumerator = null;
				if (bestTitle != null)
				{
					yield return new Rule_String(prefix + "bestRoyalTitle", bestTitle.def.GetLabelFor(gender));
					yield return new Rule_String(prefix + "bestRoyalTitleIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(bestTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "bestRoyalTitleDef", Find.ActiveLanguageWorker.WithDefiniteArticle(bestTitle.def.GetLabelFor(gender), false, false));
					yield return new Rule_String(prefix + "bestRoyalTitleFaction", bestTitle.faction.Name);
				}
				bestTitle = null;
			}
			yield return new Rule_String(prefix + "age", age.ToString());
			yield return new Rule_String(prefix + "chronologicalAge", chronologicalAge.ToString());
			if (everBeenColonistOrTameAnimal)
			{
				yield return new Rule_String("formerlyColonistInfo", "PawnWasFormerlyColonist".Translate(nameShort));
				if (constants != null)
				{
					constants[prefix + "formerlyColonist"] = "True";
				}
			}
			else if (everBeenQuestLodger)
			{
				yield return new Rule_String("formerlyColonistInfo", "PawnWasFormerlyLodger".Translate(nameShort));
				if (constants != null)
				{
					constants[prefix + "formerlyColonist"] = "True";
				}
			}
			yield return new Rule_String(prefix + "relationInfo", relationInfo);
			if (constants != null && kind != null)
			{
				constants[prefix + "flesh"] = kind.race.race.FleshType.defName;
			}
			if (constants != null)
			{
				constants[prefix + "gender"] = gender.ToString();
			}
			yield break;
			yield break;
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000F585C File Offset: 0x000F3A5C
		public static IEnumerable<Rule> RulesForDef(string prefix, Def def)
		{
			if (def == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null def", prefix), 79641686);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", def.label);
			if (def is PawnKindDef)
			{
				yield return new Rule_String(prefix + "labelPlural", ((PawnKindDef)def).GetLabelPlural(-1));
			}
			else
			{
				yield return new Rule_String(prefix + "labelPlural", Find.ActiveLanguageWorker.Pluralize(def.label, -1));
			}
			yield return new Rule_String(prefix + "description", def.description);
			yield return new Rule_String(prefix + "definite", Find.ActiveLanguageWorker.WithDefiniteArticle(def.label, false, false));
			yield return new Rule_String(prefix + "indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label, false, false));
			yield return new Rule_String(prefix + "possessive", "Proits".Translate());
			yield break;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000F5873 File Offset: 0x000F3A73
		public static IEnumerable<Rule> RulesForBodyPartRecord(string prefix, BodyPartRecord part)
		{
			if (part == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null body part", prefix), 394876778);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", part.Label);
			yield return new Rule_String(prefix + "definite", Find.ActiveLanguageWorker.WithDefiniteArticle(part.Label, false, false));
			yield return new Rule_String(prefix + "indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(part.Label, false, false));
			yield return new Rule_String(prefix + "possessive", "Proits".Translate());
			yield break;
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x000F588A File Offset: 0x000F3A8A
		public static IEnumerable<Rule> RulesForHediffDef(string prefix, HediffDef def, BodyPartRecord part)
		{
			foreach (Rule rule in GrammarUtility.RulesForDef(prefix, def))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", def.label);
			string output = (!def.labelNoun.NullOrEmpty()) ? def.labelNoun : def.label;
			yield return new Rule_String(prefix + "labelNoun", output);
			string text = def.PrettyTextForPart(part);
			if (!text.NullOrEmpty())
			{
				yield return new Rule_String(prefix + "labelNounPretty", text);
			}
			yield break;
			yield break;
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x000F58A8 File Offset: 0x000F3AA8
		public static IEnumerable<Rule> RulesForFaction(string prefix, Faction faction, Dictionary<string, string> constants = null, bool addTags = true)
		{
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			if (faction == null)
			{
				yield return new Rule_String(prefix + "name", "FactionUnaffiliated".Translate());
				yield break;
			}
			yield return new Rule_String(prefix + "name", addTags ? faction.Name.ApplyTag(faction).Resolve() : faction.Name);
			yield return new Rule_String(prefix + "pawnSingular", faction.def.pawnSingular);
			yield return new Rule_String(prefix + "pawnSingularDef", Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnSingular, false, false));
			yield return new Rule_String(prefix + "pawnSingularIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnSingular, false, false));
			yield return new Rule_String(prefix + "pawnsPlural", faction.def.pawnsPlural);
			yield return new Rule_String(prefix + "pawnsPluralDef", Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular), true, false));
			yield return new Rule_String(prefix + "pawnsPluralIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular), true, false));
			yield return new Rule_String(prefix + "leaderTitle", faction.LeaderTitle);
			yield return new Rule_String(prefix + "royalFavorLabel", faction.def.royalFavorLabel);
			if (constants != null)
			{
				constants.Add(prefix + "temporary", faction.temporary.ToString());
				constants.Add(prefix + "hasLeader", (faction.leader != null) ? "True" : "False");
			}
			yield break;
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000F58CD File Offset: 0x000F3ACD
		public static IEnumerable<Rule> RulesForWorldObject(string prefix, WorldObject worldObject, bool addTags = true)
		{
			GrammarUtility.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.worldObject = worldObject;
			CS$<>8__locals1.addTags = addTags;
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", GrammarUtility.<RulesForWorldObject>g__PossiblyWithTag|6_0(CS$<>8__locals1.worldObject.Label, ref CS$<>8__locals1));
			yield return new Rule_String(prefix + "definite", GrammarUtility.<RulesForWorldObject>g__PossiblyWithTag|6_0(Find.ActiveLanguageWorker.WithDefiniteArticle(CS$<>8__locals1.worldObject.Label, false, CS$<>8__locals1.worldObject.HasName), ref CS$<>8__locals1));
			yield return new Rule_String(prefix + "indefinite", GrammarUtility.<RulesForWorldObject>g__PossiblyWithTag|6_0(Find.ActiveLanguageWorker.WithIndefiniteArticle(CS$<>8__locals1.worldObject.Label, false, CS$<>8__locals1.worldObject.HasName), ref CS$<>8__locals1));
			yield break;
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000F58EB File Offset: 0x000F3AEB
		public static IEnumerable<Rule> RulesForIdeo(string prefix, Ideo ideo)
		{
			if (ideo == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null ideo", prefix), 453454453);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "name", ideo.name.ApplyTag(ideo).Resolve());
			yield return new Rule_String(prefix + "memberName", ideo.memberName);
			yield return new Rule_String(prefix + "memberNamePlural", ideo.MemberNamePlural);
			yield break;
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000F5902 File Offset: 0x000F3B02
		public static IEnumerable<Rule> RulesForPrecept(string prefix, Precept precept)
		{
			if (precept == null)
			{
				Log.ErrorOnce(string.Format("Tried to insert rule {0} for null precet", prefix), 823453451);
				yield break;
			}
			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "name", precept.Label.ApplyTag(precept.ideo).Resolve());
			yield break;
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000F591C File Offset: 0x000F3B1C
		[CompilerGenerated]
		internal static string <RulesForWorldObject>g__PossiblyWithTag|6_0(string str, ref GrammarUtility.<>c__DisplayClass6_0 A_1)
		{
			if (!(A_1.worldObject.Faction != null & A_1.addTags))
			{
				return str;
			}
			return str.ApplyTag(TagType.Settlement, A_1.worldObject.Faction.GetUniqueLoadID()).Resolve();
		}
	}
}
