using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200161C RID: 5660
	public static class QuestGen_Sites
	{
		// Token: 0x060084B5 RID: 33973 RVA: 0x002FADAC File Offset: 0x002F8FAC
		public static QuestPart_SpawnWorldObject SpawnWorldObject(this Quest quest, WorldObject worldObject, List<ThingDef> defsToExcludeFromHyperlinks = null, string inSignal = null)
		{
			QuestPart_SpawnWorldObject questPart_SpawnWorldObject = new QuestPart_SpawnWorldObject();
			questPart_SpawnWorldObject.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SpawnWorldObject.defsToExcludeFromHyperlinks = defsToExcludeFromHyperlinks;
			questPart_SpawnWorldObject.worldObject = worldObject;
			quest.AddPart(questPart_SpawnWorldObject);
			return questPart_SpawnWorldObject;
		}

		// Token: 0x060084B6 RID: 33974 RVA: 0x002FADF8 File Offset: 0x002F8FF8
		public static QuestPart_StartDetectionRaids StartRecurringRaids(this Quest quest, WorldObject worldObject, FloatRange? delayRangeHours = null, int? firstRaidDelayTicks = null, string inSignal = null)
		{
			QuestPart_StartDetectionRaids questPart_StartDetectionRaids = new QuestPart_StartDetectionRaids();
			questPart_StartDetectionRaids.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_StartDetectionRaids.worldObject = worldObject;
			questPart_StartDetectionRaids.delayRangeHours = delayRangeHours;
			questPart_StartDetectionRaids.firstRaidDelayTicks = firstRaidDelayTicks;
			quest.AddPart(questPart_StartDetectionRaids);
			return questPart_StartDetectionRaids;
		}

		// Token: 0x060084B7 RID: 33975 RVA: 0x002FAE4C File Offset: 0x002F904C
		public static Site GenerateSite(IEnumerable<SitePartDefWithParams> sitePartsParams, int tile, Faction faction, bool hiddenSitePartsPossible = false, RulePack singleSitePartRules = null)
		{
			Slate slate = QuestGen.slate;
			bool flag = false;
			using (IEnumerator<SitePartDefWithParams> enumerator = sitePartsParams.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.defaultHidden)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag || hiddenSitePartsPossible)
			{
				SitePartParams parms = SitePartDefOf.PossibleUnknownThreatMarker.Worker.GenerateDefaultParams(0f, tile, faction);
				SitePartDefWithParams val = new SitePartDefWithParams(SitePartDefOf.PossibleUnknownThreatMarker, parms);
				sitePartsParams = sitePartsParams.Concat(Gen.YieldSingle<SitePartDefWithParams>(val));
			}
			Site site = SiteMaker.MakeSite(sitePartsParams, tile, faction, true);
			List<Rule> list = new List<Rule>();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			List<string> list2 = new List<string>();
			int num = 0;
			for (int i = 0; i < site.parts.Count; i++)
			{
				List<Rule> list3 = new List<Rule>();
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				site.parts[i].def.Worker.Notify_GeneratedByQuestGen(site.parts[i], QuestGen.slate, list3, dictionary2);
				if (!site.parts[i].hidden)
				{
					if (singleSitePartRules != null)
					{
						List<Rule> list4 = new List<Rule>();
						list4.AddRange(list3);
						list4.AddRange(singleSitePartRules.Rules);
						string text = QuestGenUtility.ResolveLocalText(list4, dictionary2, "root", false);
						list.Add(new Rule_String("sitePart" + num + "_description", text));
						if (!text.NullOrEmpty())
						{
							list2.Add(text);
						}
					}
					for (int j = 0; j < list3.Count; j++)
					{
						Rule rule = list3[j].DeepCopy();
						Rule_String rule_String = rule as Rule_String;
						if (rule_String != null && num != 0)
						{
							rule_String.keyword = string.Concat(new object[]
							{
								"sitePart",
								num,
								"_",
								rule_String.keyword
							});
						}
						list.Add(rule);
					}
					foreach (KeyValuePair<string, string> keyValuePair in dictionary2)
					{
						string text2 = keyValuePair.Key;
						if (num != 0)
						{
							text2 = string.Concat(new object[]
							{
								"sitePart",
								num,
								"_",
								text2
							});
						}
						if (!dictionary.ContainsKey(text2))
						{
							dictionary.Add(text2, keyValuePair.Value);
						}
					}
					num++;
				}
			}
			if (!list2.Any<string>())
			{
				list.Add(new Rule_String("allSitePartsDescriptions", "HiddenOrNoSitePartDescription".Translate()));
				list.Add(new Rule_String("allSitePartsDescriptionsExceptFirst", "HiddenOrNoSitePartDescription".Translate()));
			}
			else
			{
				list.Add(new Rule_String("allSitePartsDescriptions", list2.ToClauseSequence().Resolve()));
				if (list2.Count >= 2)
				{
					list.Add(new Rule_String("allSitePartsDescriptionsExceptFirst", list2.Skip(1).ToList<string>().ToClauseSequence()));
				}
				else
				{
					list.Add(new Rule_String("allSitePartsDescriptionsExceptFirst", "HiddenOrNoSitePartDescription".Translate()));
				}
			}
			QuestGen.AddQuestDescriptionRules(list);
			QuestGen.AddQuestNameRules(list);
			QuestGen.AddQuestDescriptionConstants(dictionary);
			QuestGen.AddQuestNameConstants(dictionary);
			QuestGen.AddQuestNameRules(new List<Rule>
			{
				new Rule_String("site_label", site.Label)
			});
			return site;
		}

		// Token: 0x060084B8 RID: 33976 RVA: 0x002FB1D8 File Offset: 0x002F93D8
		public static QuestPart_SetSitePartThreatPointsToCurrent SetSitePartThreatPointsToCurrent(this Quest quest, Site site, SitePartDef sitePartDef, MapParent useMapParentThreatPoints, string inSignal = null, float threatPointsFactor = 1f)
		{
			QuestPart_SetSitePartThreatPointsToCurrent questPart_SetSitePartThreatPointsToCurrent = new QuestPart_SetSitePartThreatPointsToCurrent();
			questPart_SetSitePartThreatPointsToCurrent.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetSitePartThreatPointsToCurrent.site = site;
			questPart_SetSitePartThreatPointsToCurrent.sitePartDef = sitePartDef;
			questPart_SetSitePartThreatPointsToCurrent.useMapParentThreatPoints = useMapParentThreatPoints;
			questPart_SetSitePartThreatPointsToCurrent.threatPointsFactor = threatPointsFactor;
			quest.AddPart(questPart_SetSitePartThreatPointsToCurrent);
			return questPart_SetSitePartThreatPointsToCurrent;
		}

		// Token: 0x04005274 RID: 21108
		private const string RootSymbol = "root";
	}
}
