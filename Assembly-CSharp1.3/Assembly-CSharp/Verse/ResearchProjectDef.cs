using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F8 RID: 248
	public class ResearchProjectDef : Def
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x00020595 File Offset: 0x0001E795
		public float ResearchViewX
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0002059D File Offset: 0x0001E79D
		public float ResearchViewY
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x000205A5 File Offset: 0x0001E7A5
		public float CostApparent
		{
			get
			{
				return this.baseCost * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x000205C3 File Offset: 0x0001E7C3
		public float ProgressReal
		{
			get
			{
				return Find.ResearchManager.GetProgress(this);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x000205D0 File Offset: 0x0001E7D0
		public float ProgressApparent
		{
			get
			{
				return this.ProgressReal * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x000205EE File Offset: 0x0001E7EE
		public float ProgressPercent
		{
			get
			{
				return Find.ResearchManager.GetProgress(this) / this.baseCost;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x00020602 File Offset: 0x0001E802
		public bool IsFinished
		{
			get
			{
				return this.ProgressReal >= this.baseCost;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00020615 File Offset: 0x0001E815
		public bool CanStartNow
		{
			get
			{
				return !this.IsFinished && this.PrerequisitesCompleted && this.TechprintRequirementMet && (this.requiredResearchBuilding == null || this.PlayerHasAnyAppropriateResearchBench);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x00020644 File Offset: 0x0001E844
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

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x000206B8 File Offset: 0x0001E8B8
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

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x000206C9 File Offset: 0x0001E8C9
		public int TechprintsApplied
		{
			get
			{
				return Find.ResearchManager.GetTechprints(this);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x000206D6 File Offset: 0x0001E8D6
		public bool TechprintRequirementMet
		{
			get
			{
				return this.TechprintCount <= 0 || Find.ResearchManager.GetTechprints(this) >= this.TechprintCount;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x000206F8 File Offset: 0x0001E8F8
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
						Log.ErrorOnce("Could not find techprint for research project " + this, (int)this.shortHash ^ 873231450);
					}
				}
				return this.cachedTechprint;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00020760 File Offset: 0x0001E960
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

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x000208AC File Offset: 0x0001EAAC
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

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00020908 File Offset: 0x0001EB08
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

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002096F File Offset: 0x0001EB6F
		public override void ResolveReferences()
		{
			if (this.tab == null)
			{
				this.tab = ResearchTabDefOf.Main;
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00020984 File Offset: 0x0001EB84
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

		// Token: 0x060006B6 RID: 1718 RVA: 0x00020994 File Offset: 0x0001EB94
		public override void PostLoad()
		{
			base.PostLoad();
			if (!ModLister.RoyaltyInstalled)
			{
				this.techprintCount = 0;
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000209AC File Offset: 0x0001EBAC
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

		// Token: 0x060006B8 RID: 1720 RVA: 0x000209E3 File Offset: 0x0001EBE3
		public bool HasTag(ResearchProjectTagDef tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000209FC File Offset: 0x0001EBFC
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

		// Token: 0x060006BA RID: 1722 RVA: 0x00020ACC File Offset: 0x0001ECCC
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
						}));
					}
				}
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00020B4C File Offset: 0x0001ED4C
		public static ResearchProjectDef Named(string defName)
		{
			return DefDatabase<ResearchProjectDef>.GetNamed(defName, true);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00020B58 File Offset: 0x0001ED58
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
			Log.Error("Couldn't relax research project coordinates apart after " + 200 + " passes.");
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00020DFC File Offset: 0x0001EFFC
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

		// Token: 0x060006BE RID: 1726 RVA: 0x00020E54 File Offset: 0x0001F054
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

		// Token: 0x060006BF RID: 1727 RVA: 0x00020EB8 File Offset: 0x0001F0B8
		public void Debug_SnapPositionData()
		{
			this.x = Mathf.Round(this.x * 1f) / 1f;
			this.y = Mathf.Round(this.y * 20f) / 20f;
			ResearchProjectDef.ClampInCoordinateLimits(this);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00020F05 File Offset: 0x0001F105
		public bool Debug_IsPositionModified()
		{
			return this.positionModified;
		}

		// Token: 0x040005EB RID: 1515
		public float baseCost = 100f;

		// Token: 0x040005EC RID: 1516
		public List<ResearchProjectDef> prerequisites;

		// Token: 0x040005ED RID: 1517
		public List<ResearchProjectDef> hiddenPrerequisites;

		// Token: 0x040005EE RID: 1518
		public TechLevel techLevel;

		// Token: 0x040005EF RID: 1519
		public List<ResearchProjectDef> requiredByThis;

		// Token: 0x040005F0 RID: 1520
		private List<ResearchMod> researchMods;

		// Token: 0x040005F1 RID: 1521
		public ThingDef requiredResearchBuilding;

		// Token: 0x040005F2 RID: 1522
		public List<ThingDef> requiredResearchFacilities;

		// Token: 0x040005F3 RID: 1523
		public List<ResearchProjectTagDef> tags;

		// Token: 0x040005F4 RID: 1524
		public ResearchTabDef tab;

		// Token: 0x040005F5 RID: 1525
		public float researchViewX = 1f;

		// Token: 0x040005F6 RID: 1526
		public float researchViewY = 1f;

		// Token: 0x040005F7 RID: 1527
		[MustTranslate]
		public string discoveredLetterTitle;

		// Token: 0x040005F8 RID: 1528
		[MustTranslate]
		public string discoveredLetterText;

		// Token: 0x040005F9 RID: 1529
		[Obsolete]
		public int discoveredLetterMinDifficulty;

		// Token: 0x040005FA RID: 1530
		public DifficultyConditionConfig discoveredLetterDisabledWhen = new DifficultyConditionConfig();

		// Token: 0x040005FB RID: 1531
		[Obsolete]
		public bool unlockExtremeDifficulty;

		// Token: 0x040005FC RID: 1532
		public int techprintCount;

		// Token: 0x040005FD RID: 1533
		public float techprintCommonality = 1f;

		// Token: 0x040005FE RID: 1534
		public float techprintMarketValue = 1000f;

		// Token: 0x040005FF RID: 1535
		public List<string> heldByFactionCategoryTags;

		// Token: 0x04000600 RID: 1536
		public DifficultyConditionConfig hideWhen = new DifficultyConditionConfig();

		// Token: 0x04000601 RID: 1537
		[Unsaved(false)]
		private float x = 1f;

		// Token: 0x04000602 RID: 1538
		[Unsaved(false)]
		private float y = 1f;

		// Token: 0x04000603 RID: 1539
		[Unsaved(false)]
		private bool positionModified;

		// Token: 0x04000604 RID: 1540
		[Unsaved(false)]
		private ThingDef cachedTechprint;

		// Token: 0x04000605 RID: 1541
		[Unsaved(false)]
		private List<Def> cachedUnlockedDefs;

		// Token: 0x04000606 RID: 1542
		[Unsaved(false)]
		private List<Dialog_InfoCard.Hyperlink> cachedHyperlinks;

		// Token: 0x04000607 RID: 1543
		public const TechLevel MaxEffectiveTechLevel = TechLevel.Industrial;

		// Token: 0x04000608 RID: 1544
		private const float ResearchCostFactorPerTechLevelDiff = 0.5f;
	}
}
