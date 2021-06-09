using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000174 RID: 372
	public class ResearchProjectDef : Def
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0000D584 File Offset: 0x0000B784
		public float ResearchViewX
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0000D58C File Offset: 0x0000B78C
		public float ResearchViewY
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0000D594 File Offset: 0x0000B794
		public float CostApparent
		{
			get
			{
				return this.baseCost * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0000D5B2 File Offset: 0x0000B7B2
		public float ProgressReal
		{
			get
			{
				return Find.ResearchManager.GetProgress(this);
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0000D5BF File Offset: 0x0000B7BF
		public float ProgressApparent
		{
			get
			{
				return this.ProgressReal * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0000D5DD File Offset: 0x0000B7DD
		public float ProgressPercent
		{
			get
			{
				return Find.ResearchManager.GetProgress(this) / this.baseCost;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0000D5F1 File Offset: 0x0000B7F1
		public bool IsFinished
		{
			get
			{
				return this.ProgressReal >= this.baseCost;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0000D604 File Offset: 0x0000B804
		public bool CanStartNow
		{
			get
			{
				return !this.IsFinished && this.PrerequisitesCompleted && this.TechprintRequirementMet && (this.requiredResearchBuilding == null || this.PlayerHasAnyAppropriateResearchBench);
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x00098E94 File Offset: 0x00097094
		public bool PrerequisitesCompleted
		{
			get
			{
				if (this.prerequisites != null)
				{
					for (int i = 0; i < this.prerequisites.Count; i++)
					{
						if (!this.prerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				if (this.hiddenPrerequisites != null)
				{
					for (int j = 0; j < this.hiddenPrerequisites.Count; j++)
					{
						if (!this.hiddenPrerequisites[j].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0000D630 File Offset: 0x0000B830
		public int TechprintCount
		{
			get
			{
				if (!ModLister.RoyaltyInstalled)
				{
					return 0;
				}
				return this.techprintCount;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0000D641 File Offset: 0x0000B841
		public int TechprintsApplied
		{
			get
			{
				return Find.ResearchManager.GetTechprints(this);
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0000D64E File Offset: 0x0000B84E
		public bool TechprintRequirementMet
		{
			get
			{
				return this.TechprintCount <= 0 || Find.ResearchManager.GetTechprints(this) >= this.TechprintCount;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x00098F08 File Offset: 0x00097108
		public ThingDef Techprint
		{
			get
			{
				if (this.TechprintCount <= 0)
				{
					return null;
				}
				if (this.cachedTechprint == null)
				{
					this.cachedTechprint = DefDatabase<ThingDef>.AllDefs.FirstOrDefault(delegate(ThingDef x)
					{
						CompProperties_Techprint compProperties = x.GetCompProperties<CompProperties_Techprint>();
						return compProperties != null && compProperties.project == this;
					});
					if (this.cachedTechprint == null)
					{
						Log.ErrorOnce("Could not find techprint for research project " + this, (int)this.shortHash ^ 873231450, false);
					}
				}
				return this.cachedTechprint;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x00098F70 File Offset: 0x00097170
		public List<Def> UnlockedDefs
		{
			get
			{
				if (this.cachedUnlockedDefs == null)
				{
					this.cachedUnlockedDefs = (from x in (from x in DefDatabase<RecipeDef>.AllDefs
					where x.researchPrerequisite == this || (x.researchPrerequisites != null && x.researchPrerequisites.Contains(this))
					select x).SelectMany((RecipeDef x) => from y in x.products
					select y.thingDef)
					orderby x.label
					select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					where x.plant != null && x.plant.sowResearchPrerequisites != null && x.plant.sowResearchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<TerrainDef>.AllDefs
					where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Distinct<Def>().ToList<Def>();
				}
				return this.cachedUnlockedDefs;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x000990BC File Offset: 0x000972BC
		public List<Dialog_InfoCard.Hyperlink> InfoCardHyperlinks
		{
			get
			{
				if (this.cachedHyperlinks == null)
				{
					this.cachedHyperlinks = new List<Dialog_InfoCard.Hyperlink>();
					List<Def> unlockedDefs = this.UnlockedDefs;
					if (unlockedDefs != null)
					{
						for (int i = 0; i < unlockedDefs.Count; i++)
						{
							this.cachedHyperlinks.Add(new Dialog_InfoCard.Hyperlink(unlockedDefs[i], -1));
						}
					}
				}
				return this.cachedHyperlinks;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x00099118 File Offset: 0x00097318
		private bool PlayerHasAnyAppropriateResearchBench
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Building> allBuildingsColonist = maps[i].listerBuildings.allBuildingsColonist;
					for (int j = 0; j < allBuildingsColonist.Count; j++)
					{
						Building_ResearchBench building_ResearchBench = allBuildingsColonist[j] as Building_ResearchBench;
						if (building_ResearchBench != null && this.CanBeResearchedAt(building_ResearchBench, true))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0000D66F File Offset: 0x0000B86F
		public override void ResolveReferences()
		{
			if (this.tab == null)
			{
				this.tab = ResearchTabDefOf.Main;
			}
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0000D684 File Offset: 0x0000B884
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return "techLevel is Undefined";
			}
			if (this.ResearchViewX < 0f || this.ResearchViewY < 0f)
			{
				yield return "researchViewX and/or researchViewY not set";
			}
			if (this.techprintCount == 0 && !this.heldByFactionCategoryTags.NullOrEmpty<string>())
			{
				yield return "requires no techprints but has heldByFactionCategoryTags.";
			}
			if (this.techprintCount > 0 && this.heldByFactionCategoryTags.NullOrEmpty<string>())
			{
				yield return "requires techprints but has no heldByFactionCategoryTags.";
			}
			List<ResearchProjectDef> rpDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < rpDefs.Count; i = num + 1)
			{
				if (rpDefs[i] != this && rpDefs[i].tab == this.tab && rpDefs[i].ResearchViewX == this.ResearchViewX && rpDefs[i].ResearchViewY == this.ResearchViewY)
				{
					yield return string.Concat(new object[]
					{
						"same research view coords and tab as ",
						rpDefs[i],
						": ",
						this.ResearchViewX,
						", ",
						this.ResearchViewY,
						"(",
						this.tab,
						")"
					});
				}
				num = i;
			}
			if (!ModLister.RoyaltyInstalled && this.techprintCount > 0)
			{
				yield return "defines techprintCount, but techprints are a Royalty-specific game system and only work with Royalty installed.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0000D694 File Offset: 0x0000B894
		public override void PostLoad()
		{
			base.PostLoad();
			if (!ModLister.RoyaltyInstalled)
			{
				this.techprintCount = 0;
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x00099180 File Offset: 0x00097380
		public float CostFactor(TechLevel researcherTechLevel)
		{
			TechLevel techLevel = (TechLevel)Mathf.Min((int)this.techLevel, 4);
			if (researcherTechLevel >= techLevel)
			{
				return 1f;
			}
			int num = (int)(techLevel - researcherTechLevel);
			return 1f + (float)num * 0.5f;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0000D6AA File Offset: 0x0000B8AA
		public bool HasTag(ResearchProjectTagDef tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000991B8 File Offset: 0x000973B8
		public bool CanBeResearchedAt(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
		{
			if (this.requiredResearchBuilding != null && bench.def != this.requiredResearchBuilding)
			{
				return false;
			}
			if (!ignoreResearchBenchPowerStatus)
			{
				CompPowerTrader comp = bench.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
			}
			if (!this.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			{
				ResearchProjectDef.<>c__DisplayClass67_0 CS$<>8__locals1 = new ResearchProjectDef.<>c__DisplayClass67_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.affectedByFacilities = bench.TryGetComp<CompAffectedByFacilities>();
				if (CS$<>8__locals1.affectedByFacilities == null)
				{
					return false;
				}
				List<Thing> linkedFacilitiesListForReading = CS$<>8__locals1.affectedByFacilities.LinkedFacilitiesListForReading;
				int j;
				int i;
				for (i = 0; i < this.requiredResearchFacilities.Count; i = j + 1)
				{
					if (linkedFacilitiesListForReading.Find((Thing x) => x.def == CS$<>8__locals1.<>4__this.requiredResearchFacilities[i] && CS$<>8__locals1.affectedByFacilities.IsFacilityActive(x)) == null)
					{
						return false;
					}
					j = i;
				}
			}
			return true;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00099288 File Offset: 0x00097488
		public void ReapplyAllMods()
		{
			if (this.researchMods != null)
			{
				for (int i = 0; i < this.researchMods.Count; i++)
				{
					try
					{
						this.researchMods[i].Apply();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception applying research mod for project ",
							this,
							": ",
							ex.ToString()
						}), false);
					}
				}
			}
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0000D6C2 File Offset: 0x0000B8C2
		public static ResearchProjectDef Named(string defName)
		{
			return DefDatabase<ResearchProjectDef>.GetNamed(defName, true);
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00099308 File Offset: 0x00097508
		public static void GenerateNonOverlappingCoordinates()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				researchProjectDef.x = researchProjectDef.researchViewX;
				researchProjectDef.y = researchProjectDef.researchViewY;
			}
			int num = 0;
			do
			{
				bool flag = false;
				foreach (ResearchProjectDef researchProjectDef2 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
				{
					foreach (ResearchProjectDef researchProjectDef3 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
					{
						if (researchProjectDef2 != researchProjectDef3 && researchProjectDef2.tab == researchProjectDef3.tab)
						{
							bool flag2 = Mathf.Abs(researchProjectDef2.x - researchProjectDef3.x) < 0.5f;
							bool flag3 = Mathf.Abs(researchProjectDef2.y - researchProjectDef3.y) < 0.25f;
							if (flag2 && flag3)
							{
								flag = true;
								if (researchProjectDef2.x <= researchProjectDef3.x)
								{
									researchProjectDef2.x -= 0.1f;
									researchProjectDef3.x += 0.1f;
								}
								else
								{
									researchProjectDef2.x += 0.1f;
									researchProjectDef3.x -= 0.1f;
								}
								if (researchProjectDef2.y <= researchProjectDef3.y)
								{
									researchProjectDef2.y -= 0.1f;
									researchProjectDef3.y += 0.1f;
								}
								else
								{
									researchProjectDef2.y += 0.1f;
									researchProjectDef3.y -= 0.1f;
								}
								researchProjectDef2.x += 0.001f;
								researchProjectDef2.y += 0.001f;
								researchProjectDef3.x -= 0.001f;
								researchProjectDef3.y -= 0.001f;
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef2);
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef3);
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
				num++;
			}
			while (num <= 200);
			Log.Error("Couldn't relax research project coordinates apart after " + 200 + " passes.", false);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x000995AC File Offset: 0x000977AC
		private static void ClampInCoordinateLimits(ResearchProjectDef rp)
		{
			if (rp.x < 0f)
			{
				rp.x = 0f;
			}
			if (rp.y < 0f)
			{
				rp.y = 0f;
			}
			if (rp.y > 6.5f)
			{
				rp.y = 6.5f;
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00099604 File Offset: 0x00097804
		public void Debug_ApplyPositionDelta(Vector2 delta)
		{
			bool flag = Mathf.Abs(delta.x) > 0.01f;
			bool flag2 = Mathf.Abs(delta.y) > 0.01f;
			if (flag)
			{
				this.x += delta.x;
			}
			if (flag2)
			{
				this.y += delta.y;
			}
			this.positionModified = true;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00099668 File Offset: 0x00097868
		public void Debug_SnapPositionData()
		{
			this.x = Mathf.Round(this.x * 1f) / 1f;
			this.y = Mathf.Round(this.y * 20f) / 20f;
			ResearchProjectDef.ClampInCoordinateLimits(this);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0000D6CB File Offset: 0x0000B8CB
		public bool Debug_IsPositionModified()
		{
			return this.positionModified;
		}

		// Token: 0x04000800 RID: 2048
		public float baseCost = 100f;

		// Token: 0x04000801 RID: 2049
		public List<ResearchProjectDef> prerequisites;

		// Token: 0x04000802 RID: 2050
		public List<ResearchProjectDef> hiddenPrerequisites;

		// Token: 0x04000803 RID: 2051
		public TechLevel techLevel;

		// Token: 0x04000804 RID: 2052
		public List<ResearchProjectDef> requiredByThis;

		// Token: 0x04000805 RID: 2053
		private List<ResearchMod> researchMods;

		// Token: 0x04000806 RID: 2054
		public ThingDef requiredResearchBuilding;

		// Token: 0x04000807 RID: 2055
		public List<ThingDef> requiredResearchFacilities;

		// Token: 0x04000808 RID: 2056
		public List<ResearchProjectTagDef> tags;

		// Token: 0x04000809 RID: 2057
		public ResearchTabDef tab;

		// Token: 0x0400080A RID: 2058
		public float researchViewX = 1f;

		// Token: 0x0400080B RID: 2059
		public float researchViewY = 1f;

		// Token: 0x0400080C RID: 2060
		[MustTranslate]
		public string discoveredLetterTitle;

		// Token: 0x0400080D RID: 2061
		[MustTranslate]
		public string discoveredLetterText;

		// Token: 0x0400080E RID: 2062
		[Obsolete]
		public int discoveredLetterMinDifficulty;

		// Token: 0x0400080F RID: 2063
		public DifficultyConditionConfig discoveredLetterDisabledWhen = new DifficultyConditionConfig();

		// Token: 0x04000810 RID: 2064
		[Obsolete]
		public bool unlockExtremeDifficulty;

		// Token: 0x04000811 RID: 2065
		public int techprintCount;

		// Token: 0x04000812 RID: 2066
		public float techprintCommonality = 1f;

		// Token: 0x04000813 RID: 2067
		public float techprintMarketValue = 1000f;

		// Token: 0x04000814 RID: 2068
		public List<string> heldByFactionCategoryTags;

		// Token: 0x04000815 RID: 2069
		public DifficultyConditionConfig hideWhen = new DifficultyConditionConfig();

		// Token: 0x04000816 RID: 2070
		[Unsaved(false)]
		private float x = 1f;

		// Token: 0x04000817 RID: 2071
		[Unsaved(false)]
		private float y = 1f;

		// Token: 0x04000818 RID: 2072
		[Unsaved(false)]
		private bool positionModified;

		// Token: 0x04000819 RID: 2073
		[Unsaved(false)]
		private ThingDef cachedTechprint;

		// Token: 0x0400081A RID: 2074
		[Unsaved(false)]
		private List<Def> cachedUnlockedDefs;

		// Token: 0x0400081B RID: 2075
		[Unsaved(false)]
		private List<Dialog_InfoCard.Hyperlink> cachedHyperlinks;

		// Token: 0x0400081C RID: 2076
		public const TechLevel MaxEffectiveTechLevel = TechLevel.Industrial;

		// Token: 0x0400081D RID: 2077
		private const float ResearchCostFactorPerTechLevelDiff = 0.5f;
	}
}
