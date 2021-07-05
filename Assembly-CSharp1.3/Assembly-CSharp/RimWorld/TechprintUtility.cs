using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001220 RID: 4640
	public static class TechprintUtility
	{
		// Token: 0x06006F54 RID: 28500 RVA: 0x00252764 File Offset: 0x00250964
		public static IEnumerable<ResearchProjectDef> GetResearchProjectsNeedingTechprintsNow(Faction faction, List<ThingDef> alreadyGeneratedTechprints = null, float maxMarketValue = 3.4028235E+38f)
		{
			return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Where(delegate(ResearchProjectDef p)
			{
				if (p.TechprintCount == 0)
				{
					return false;
				}
				if (p.IsFinished || p.TechprintRequirementMet)
				{
					return false;
				}
				if (faction != null && (p.heldByFactionCategoryTags == null || !p.heldByFactionCategoryTags.Contains(faction.def.categoryTag)))
				{
					return false;
				}
				if (maxMarketValue != 3.4028235E+38f && p.Techprint.BaseMarketValue > maxMarketValue)
				{
					return false;
				}
				if (alreadyGeneratedTechprints != null)
				{
					CompProperties_Techprint compProperties = p.Techprint.GetCompProperties<CompProperties_Techprint>();
					if (compProperties != null)
					{
						int num = compProperties.project.TechprintCount - compProperties.project.TechprintsApplied;
						int num2 = 0;
						for (int i = 0; i < alreadyGeneratedTechprints.Count; i++)
						{
							if (alreadyGeneratedTechprints[i] == p.Techprint)
							{
								num2++;
							}
						}
						if (num2 >= num)
						{
							return false;
						}
					}
				}
				return true;
			});
		}

		// Token: 0x06006F55 RID: 28501 RVA: 0x002527A2 File Offset: 0x002509A2
		public static float GetSelectionWeight(ResearchProjectDef project)
		{
			return project.techprintCommonality * (project.PrerequisitesCompleted ? 1f : 0.02f);
		}

		// Token: 0x06006F56 RID: 28502 RVA: 0x002527C0 File Offset: 0x002509C0
		public static bool TryGetTechprintDefToGenerate(Faction faction, out ThingDef result, List<ThingDef> alreadyGeneratedTechprints = null, float maxMarketValue = 3.4028235E+38f)
		{
			ResearchProjectDef researchProjectDef;
			if (!TechprintUtility.GetResearchProjectsNeedingTechprintsNow(faction, alreadyGeneratedTechprints, maxMarketValue).TryRandomElementByWeight(new Func<ResearchProjectDef, float>(TechprintUtility.GetSelectionWeight), out researchProjectDef))
			{
				result = null;
				return false;
			}
			result = researchProjectDef.Techprint;
			return true;
		}

		// Token: 0x06006F57 RID: 28503 RVA: 0x002527F8 File Offset: 0x002509F8
		[DebugOutput]
		public static void TechprintsFromFactions()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Techprints generated from these factions:");
			foreach (Faction faction in from fa in Find.FactionManager.AllFactions
			where fa.def.humanlikeFaction && !fa.Hidden
			select fa)
			{
				stringBuilder.AppendLine(faction.Name);
				for (int i = 0; i < 30; i++)
				{
					ThingDef thingDef;
					if (!TechprintUtility.TryGetTechprintDefToGenerate(faction, out thingDef, null, 3.4028235E+38f))
					{
						stringBuilder.AppendLine("    none possible");
						break;
					}
					stringBuilder.AppendLine("    " + thingDef.LabelCap);
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06006F58 RID: 28504 RVA: 0x002528DC File Offset: 0x00250ADC
		[DebugOutput]
		public static void TechprintsFromFactionsChances()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in from fa in Find.FactionManager.AllFactions
			where fa.def.humanlikeFaction && !fa.Hidden
			select fa)
			{
				Faction localFac = localFac2;
				list.Add(new FloatMenuOption(localFac.Name + " (" + localFac.def.defName + ")", delegate()
				{
					List<TableDataGetter<ResearchProjectDef>> list2 = new List<TableDataGetter<ResearchProjectDef>>();
					list2.Add(new TableDataGetter<ResearchProjectDef>("defName", (ResearchProjectDef d) => d.defName));
					IEnumerable<ResearchProjectDef> researchProjectsNeedingTechprintsNow = TechprintUtility.GetResearchProjectsNeedingTechprintsNow(localFac, null, float.MaxValue);
					if (researchProjectsNeedingTechprintsNow.Any<ResearchProjectDef>())
					{
						float sum = researchProjectsNeedingTechprintsNow.Sum((ResearchProjectDef x) => TechprintUtility.GetSelectionWeight(x));
						list2.Add(new TableDataGetter<ResearchProjectDef>("chance", (ResearchProjectDef x) => (TechprintUtility.GetSelectionWeight(x) / sum).ToStringPercent()));
						list2.Add(new TableDataGetter<ResearchProjectDef>("weight", (ResearchProjectDef x) => TechprintUtility.GetSelectionWeight(x).ToString("0.###")));
					}
					DebugTables.MakeTablesDialog<ResearchProjectDef>(researchProjectsNeedingTechprintsNow, list2.ToArray());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}
	}
}
