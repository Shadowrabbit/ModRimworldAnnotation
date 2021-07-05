using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000139 RID: 313
	public static class PlayLogEntryUtility
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x00028269 File Offset: 0x00026469
		public static IEnumerable<Rule> RulesForOptionalWeapon(string prefix, ThingDef weaponDef, ThingDef projectileDef)
		{
			if (weaponDef != null)
			{
				foreach (Rule rule in GrammarUtility.RulesForDef(prefix, weaponDef))
				{
					yield return rule;
				}
				IEnumerator<Rule> enumerator = null;
				ThingDef thingDef = projectileDef;
				if (thingDef == null && !weaponDef.Verbs.NullOrEmpty<VerbProperties>())
				{
					thingDef = weaponDef.Verbs[0].defaultProjectile;
				}
				if (thingDef != null)
				{
					foreach (Rule rule2 in GrammarUtility.RulesForDef(prefix + "_projectile", thingDef))
					{
						yield return rule2;
					}
					enumerator = null;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00028287 File Offset: 0x00026487
		public static IEnumerable<Rule> RulesForDamagedParts(string prefix, BodyDef body, List<BodyPartRecord> bodyParts, List<bool> bodyPartsDestroyed, Dictionary<string, string> constants)
		{
			if (bodyParts != null)
			{
				int destroyedIndex = 0;
				int damagedIndex = 0;
				int num;
				for (int i = 0; i < bodyParts.Count; i = num)
				{
					yield return new Rule_String(string.Format(prefix + "{0}_label", i), bodyParts[i].Label);
					yield return new Rule_String(string.Format(prefix + "{0}_definite", i), Find.ActiveLanguageWorker.WithDefiniteArticle(bodyParts[i].Label, false, false));
					yield return new Rule_String(string.Format(prefix + "{0}_indefinite", i), Find.ActiveLanguageWorker.WithIndefiniteArticle(bodyParts[i].Label, false, false));
					constants[string.Format(prefix + "{0}_destroyed", i)] = bodyPartsDestroyed[i].ToString();
					constants[string.Format(prefix + "{0}_gender", i)] = LanguageDatabase.activeLanguage.ResolveGender(bodyParts[i].Label, null).ToString();
					if (bodyPartsDestroyed[i])
					{
						yield return new Rule_String(string.Format(prefix + "_destroyed{0}_label", destroyedIndex), bodyParts[i].Label);
						yield return new Rule_String(string.Format(prefix + "_destroyed{0}_definite", destroyedIndex), Find.ActiveLanguageWorker.WithDefiniteArticle(bodyParts[i].Label, false, false));
						yield return new Rule_String(string.Format(prefix + "_destroyed{0}_indefinite", destroyedIndex), Find.ActiveLanguageWorker.WithIndefiniteArticle(bodyParts[i].Label, false, false));
						constants[string.Format("{0}_destroyed{1}_outside", prefix, destroyedIndex)] = (bodyParts[i].depth == BodyPartDepth.Outside).ToString();
						constants[string.Format("{0}_destroyed{1}_gender", prefix, destroyedIndex)] = LanguageDatabase.activeLanguage.ResolveGender(bodyParts[i].Label, null).ToString();
						num = destroyedIndex;
						destroyedIndex = num + 1;
					}
					else
					{
						yield return new Rule_String(string.Format(prefix + "_damaged{0}_label", damagedIndex), bodyParts[i].Label);
						yield return new Rule_String(string.Format(prefix + "_damaged{0}_definite", damagedIndex), Find.ActiveLanguageWorker.WithDefiniteArticle(bodyParts[i].Label, false, false));
						yield return new Rule_String(string.Format(prefix + "_damaged{0}_indefinite", damagedIndex), Find.ActiveLanguageWorker.WithIndefiniteArticle(bodyParts[i].Label, false, false));
						constants[string.Format("{0}_damaged{1}_outside", prefix, damagedIndex)] = (bodyParts[i].depth == BodyPartDepth.Outside).ToString();
						constants[string.Format("{0}_damaged{1}_gender", prefix, damagedIndex)] = LanguageDatabase.activeLanguage.ResolveGender(bodyParts[i].Label, null).ToString();
						num = damagedIndex;
						damagedIndex = num + 1;
					}
					num = i + 1;
				}
				constants[prefix + "_count"] = bodyParts.Count.ToString();
				constants[prefix + "_destroyed_count"] = destroyedIndex.ToString();
				constants[prefix + "_damaged_count"] = damagedIndex.ToString();
			}
			else
			{
				constants[prefix + "_count"] = "0";
				constants[prefix + "_destroyed_count"] = "0";
				constants[prefix + "_damaged_count"] = "0";
			}
			yield break;
		}
	}
}
