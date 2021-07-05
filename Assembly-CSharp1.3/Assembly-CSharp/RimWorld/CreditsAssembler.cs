using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132C RID: 4908
	public class CreditsAssembler
	{
		// Token: 0x060076B7 RID: 30391 RVA: 0x00293BC1 File Offset: 0x00291DC1
		public static IEnumerable<CreditsEntry> AllCredits()
		{
			List<CreditsEntry> testers = new List<CreditsEntry>();
			yield return new CreditRecord_Space(200f);
			yield return new CreditRecord_Title("Credits_Developers".Translate());
			yield return new CreditRecord_Role("", "Tynan Sylvester", null);
			yield return new CreditRecord_Role("", "Piotr Walczak", null);
			yield return new CreditRecord_Role("", "Kenneth Ellersdorfer", null);
			yield return new CreditRecord_Role("", "Igor Lebedev", null);
			yield return new CreditRecord_Role("", "Matt Ritchie", null);
			yield return new CreditRecord_Role("", "Ben Rog-Wilhelm", null);
			yield return new CreditRecord_Role("", "Alex Mulford", null);
			yield return new CreditRecord_Role("", "Matt Quail", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credit_MusicAndSound".Translate());
			yield return new CreditRecord_Role("", "Alistair Lindsay", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credit_GameArt".Translate());
			yield return new CreditRecord_Role("", "Oskar Potocki", null);
			yield return new CreditRecord_Role("", "Tynan Sylvester", null);
			yield return new CreditRecord_Role("", "Rhopunzel", null);
			yield return new CreditRecord_Role("", "Ricardo Tome", null);
			yield return new CreditRecord_Role("", "Kay Fedewa", null);
			yield return new CreditRecord_Role("", "Tamara Osborn", null);
			yield return new CreditRecord_Role("", "Jon Larson", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credits_AdditionalDevelopment".Translate());
			yield return new CreditRecord_Role("", "Gavan Woolery", null);
			yield return new CreditRecord_Role("", "David 'Rez' Graham", null);
			yield return new CreditRecord_Role("", "Ben Grob", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credit_TestLead".Translate());
			yield return new CreditRecord_Role("", "Pheanox", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credits_TitleCommunity".Translate());
			yield return new CreditRecord_Role("Credit_ModDonation".Translate(), "Zhentar", null).Compress();
			yield return new CreditRecord_Role("Credit_ModDonation".Translate(), "Haplo", null).Compress();
			yield return new CreditRecord_Role("Credit_ModDonation".Translate(), "iame6162013", null).Compress();
			yield return new CreditRecord_Role("Credit_ModDonation".Translate(), "Shinzy", null).Compress();
			yield return new CreditRecord_Role("Credit_WritingDonation".Translate(), "John Woolley", null).Compress();
			yield return new CreditRecord_Role("Credit_Moderator".Translate(), "ItchyFlea", null).Compress();
			yield return new CreditRecord_Role("Credit_Moderator".Translate(), "Ramsis", null).Compress();
			yield return new CreditRecord_Role("Credit_Moderator".Translate(), "Calahan", null).Compress();
			yield return new CreditRecord_Role("Credit_Moderator".Translate(), "milon", null).Compress();
			yield return new CreditRecord_Role("Credit_Moderator".Translate(), "Evul", null).Compress();
			yield return new CreditRecord_Role("Credit_Moderator".Translate(), "MarvinKosh", null).Compress();
			yield return new CreditRecord_Role("Credit_WikiMaster".Translate(), "ZestyLemons", null).Compress();
			yield return new CreditRecord_Title("Credits_TitleTester".Translate());
			testers.Add(new CreditRecord_Role("", "ItchyFlea", null).Compress());
			testers.Add(new CreditRecord_Role("", "Haplo", null).Compress());
			testers.Add(new CreditRecord_Role("", "Mehni", null).Compress());
			testers.Add(new CreditRecord_Role("", "Vas", null).Compress());
			testers.Add(new CreditRecord_Role("", "XeoNovaDan", null).Compress());
			testers.Add(new CreditRecord_Role("", "JimmyAgnt007", null).Compress());
			testers.Add(new CreditRecord_Role("", "Goldenpotatoes", null).Compress());
			testers.Add(new CreditRecord_Role("", "_alphaBeta_", null).Compress());
			testers.Add(new CreditRecord_Role("", "TheDee05", null).Compress());
			testers.Add(new CreditRecord_Role("", "Drb89", null).Compress());
			testers.Add(new CreditRecord_Role("", "Skissor", null).Compress());
			testers.Add(new CreditRecord_Role("", "MarvinKosh", null).Compress());
			testers.Add(new CreditRecord_Role("", "Evul", null).Compress());
			testers.Add(new CreditRecord_Role("", "Jimyoda", null).Compress());
			testers.Add(new CreditRecord_Role("", "Semmy", null).Compress());
			testers.Add(new CreditRecord_Role("", "Letharion", null).Compress());
			testers.Add(new CreditRecord_Role("", "Laos", null).Compress());
			testers.Add(new CreditRecord_Role("", "Coenmjc", null).Compress());
			testers.Add(new CreditRecord_Role("", "Gaesatae", null).Compress());
			testers.Add(new CreditRecord_Role("", "Skullywag", null).Compress());
			testers.Add(new CreditRecord_Role("", "Enystrom8734", null).Compress());
			testers.Add(new CreditRecord_Role("", "ChJees", null).Compress());
			testers.Add(new CreditRecord_Role("", "IAmMiko", null).Compress());
			testers.Add(new CreditRecord_Role("", "Jaxxa", null).Compress());
			testers.Add(new CreditRecord_Role("", "Moasseman", null).Compress());
			testers.Add(new CreditRecord_Role("", "Ramsis", null).Compress());
			testers.Add(new CreditRecord_Role("", "ReZpawner", null).Compress());
			testers.Add(new CreditRecord_Role("", "ShauaPuta", null).Compress());
			testers.Add(new CreditRecord_Role("", "Sneaks", null).Compress());
			testers.Add(new CreditRecord_Role("", "tedvs", null).Compress());
			testers.Add(new CreditRecord_Role("", "Zero", null).Compress());
			testers.Add(new CreditRecord_Role("", "AWiseCorn", null).Compress());
			testers.Add(new CreditRecord_Role("", "DianaWinters", null).Compress());
			testers.Add(new CreditRecord_Role("", "Harry Bryant", null).Compress());
			testers.Add(new CreditRecord_Role("", "RawCode", null).Compress());
			testers.Add(new CreditRecord_Role("", "TeiXeR", null).Compress());
			testers.Add(new CreditRecord_Role("", "Zirr", null).Compress());
			testers.Add(new CreditRecord_Role("", "DubskiDude", null).Compress());
			foreach (CreditsEntry creditsEntry in CreditsAssembler.<AllCredits>g__Reformat2Cols|0_0(testers))
			{
				yield return creditsEntry;
			}
			IEnumerator<CreditsEntry> enumerator = null;
			yield return new CreditRecord_Role("", "Many other gracious volunteers!", null);
			yield return new CreditRecord_Space(200f);
			foreach (LoadedLanguage lang in LanguageDatabase.AllLoadedLanguages)
			{
				lang.LoadMetadata();
				if (lang.info.credits.Count > 0)
				{
					yield return new CreditRecord_Title("Credits_TitleLanguage".Translate(lang.FriendlyNameEnglish));
				}
				foreach (CreditsEntry creditsEntry2 in CreditsAssembler.<AllCredits>g__Reformat2Cols|0_0(lang.info.credits))
				{
					yield return creditsEntry2;
				}
				enumerator = null;
				lang = null;
			}
			IEnumerator<LoadedLanguage> enumerator2 = null;
			bool firstModCredit = false;
			HashSet<string> allModders = new HashSet<string>();
			List<string> tmpModders = new List<string>();
			foreach (ModMetaData mod in ModsConfig.ActiveModsInLoadOrder.InRandomOrder(null))
			{
				if (!mod.Official)
				{
					tmpModders.Clear();
					tmpModders.AddRange(mod.Authors);
					for (int i = tmpModders.Count - 1; i >= 0; i--)
					{
						tmpModders[i] = tmpModders[i].Trim();
						if (!allModders.Add(tmpModders[i].ToLowerInvariant()))
						{
							tmpModders.RemoveAt(i);
						}
					}
					if (tmpModders.Count > 0)
					{
						foreach (string modder in tmpModders)
						{
							if (!firstModCredit)
							{
								yield return new CreditRecord_Title("Credits_TitleMods".Translate());
								firstModCredit = true;
							}
							yield return new CreditRecord_Role(mod.Name, modder, null).Compress();
							modder = null;
						}
						List<string>.Enumerator enumerator4 = default(List<string>.Enumerator);
					}
					mod = null;
				}
			}
			IEnumerator<ModMetaData> enumerator3 = null;
			yield break;
			yield break;
		}

		// Token: 0x060076B9 RID: 30393 RVA: 0x00293BCA File Offset: 0x00291DCA
		[CompilerGenerated]
		internal static IEnumerable<CreditsEntry> <AllCredits>g__Reformat2Cols|0_0(List<CreditsEntry> entries)
		{
			string crediteePrev = null;
			int num;
			for (int i = 0; i < entries.Count; i = num + 1)
			{
				CreditsEntry langCred = entries[i];
				CreditRecord_Role creditRecord_Role;
				if ((creditRecord_Role = (langCred as CreditRecord_Role)) != null)
				{
					if (crediteePrev != null)
					{
						yield return new CreditRecord_RoleTwoCols(crediteePrev, creditRecord_Role.creditee, null).Compress();
						crediteePrev = null;
					}
					else
					{
						crediteePrev = creditRecord_Role.creditee;
					}
				}
				else
				{
					if (crediteePrev != null)
					{
						yield return new CreditRecord_RoleTwoCols(crediteePrev, "", null).Compress();
						crediteePrev = null;
					}
					yield return langCred;
				}
				langCred = null;
				num = i;
			}
			if (crediteePrev != null)
			{
				yield return new CreditRecord_RoleTwoCols(crediteePrev, "", null).Compress();
			}
			yield break;
		}
	}
}
