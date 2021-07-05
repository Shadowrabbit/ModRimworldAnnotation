using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8A RID: 3978
	public class FactionDef : Def
	{
		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06005740 RID: 22336 RVA: 0x001CCB94 File Offset: 0x001CAD94
		public List<RoyalTitleDef> RoyalTitlesAwardableInSeniorityOrderForReading
		{
			get
			{
				if (this.royalTitlesAwardableInSeniorityOrderForReading == null)
				{
					this.royalTitlesAwardableInSeniorityOrderForReading = new List<RoyalTitleDef>();
					if (this.royalTitleTags != null && ModLister.RoyaltyInstalled)
					{
						foreach (RoyalTitleDef royalTitleDef in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
						{
							if (royalTitleDef.Awardable && royalTitleDef.tags.SharesElementWith(this.royalTitleTags))
							{
								this.royalTitlesAwardableInSeniorityOrderForReading.Add(royalTitleDef);
							}
						}
						this.royalTitlesAwardableInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
					}
				}
				return this.royalTitlesAwardableInSeniorityOrderForReading;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06005741 RID: 22337 RVA: 0x001CCC60 File Offset: 0x001CAE60
		public List<RoyalTitleDef> RoyalTitlesAllInSeniorityOrderForReading
		{
			get
			{
				if (this.royalTitlesAllInSeniorityOrderForReading == null)
				{
					this.royalTitlesAllInSeniorityOrderForReading = new List<RoyalTitleDef>();
					if (this.royalTitleTags != null && ModLister.RoyaltyInstalled)
					{
						foreach (RoyalTitleDef royalTitleDef in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
						{
							if (royalTitleDef.tags.SharesElementWith(this.royalTitleTags))
							{
								this.royalTitlesAllInSeniorityOrderForReading.Add(royalTitleDef);
							}
						}
						this.royalTitlesAllInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
					}
				}
				return this.royalTitlesAllInSeniorityOrderForReading;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06005742 RID: 22338 RVA: 0x0003C824 File Offset: 0x0003AA24
		public RoyalTitleInheritanceWorker RoyalTitleInheritanceWorker
		{
			get
			{
				if (this.royalTitleInheritanceWorker == null && this.royalTitleInheritanceWorkerClass != null)
				{
					this.royalTitleInheritanceWorker = (RoyalTitleInheritanceWorker)Activator.CreateInstance(this.royalTitleInheritanceWorkerClass);
				}
				return this.royalTitleInheritanceWorker;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06005743 RID: 22339 RVA: 0x0003C858 File Offset: 0x0003AA58
		public bool CanEverBeNonHostile
		{
			get
			{
				return !this.permanentEnemy;
			}
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06005744 RID: 22340 RVA: 0x001CCD20 File Offset: 0x001CAF20
		public Texture2D SettlementTexture
		{
			get
			{
				if (this.settlementTexture == null)
				{
					if (!this.settlementTexturePath.NullOrEmpty())
					{
						this.settlementTexture = ContentFinder<Texture2D>.Get(this.settlementTexturePath, true);
					}
					else
					{
						this.settlementTexture = BaseContent.BadTex;
					}
				}
				return this.settlementTexture;
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06005745 RID: 22341 RVA: 0x001CCD70 File Offset: 0x001CAF70
		public Texture2D FactionIcon
		{
			get
			{
				if (this.factionIcon == null)
				{
					if (!this.factionIconPath.NullOrEmpty())
					{
						this.factionIcon = ContentFinder<Texture2D>.Get(this.factionIconPath, true);
					}
					else
					{
						this.factionIcon = BaseContent.BadTex;
					}
				}
				return this.factionIcon;
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06005746 RID: 22342 RVA: 0x0003C863 File Offset: 0x0003AA63
		public Texture2D RoyalFavorIcon
		{
			get
			{
				if (this.royalFavorIcon == null && !this.royalFavorIconPath.NullOrEmpty())
				{
					this.royalFavorIcon = ContentFinder<Texture2D>.Get(this.royalFavorIconPath, true);
				}
				return this.royalFavorIcon;
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06005747 RID: 22343 RVA: 0x0003C898 File Offset: 0x0003AA98
		public bool HasRoyalTitles
		{
			get
			{
				return this.RoyalTitlesAwardableInSeniorityOrderForReading.Count > 0;
			}
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x001CCDC0 File Offset: 0x001CAFC0
		public float MinPointsToGeneratePawnGroup(PawnGroupKindDef groupKind)
		{
			if (this.pawnGroupMakers == null)
			{
				return 0f;
			}
			IEnumerable<PawnGroupMaker> source = from x in this.pawnGroupMakers
			where x.kindDef == groupKind
			select x;
			if (!source.Any<PawnGroupMaker>())
			{
				return 0f;
			}
			return source.Min((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything);
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x0003C8A8 File Offset: 0x0003AAA8
		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			return this.apparelStuffFilter == null || this.apparelStuffFilter.Allows(stuffDef);
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x0003C8C0 File Offset: 0x0003AAC0
		public float RaidCommonalityFromPoints(float points)
		{
			if (points < 0f || this.raidCommonalityFromPointsCurve == null)
			{
				return 1f;
			}
			return this.raidCommonalityFromPointsCurve.Evaluate(points);
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x0003C8E4 File Offset: 0x0003AAE4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.apparelStuffFilter != null)
			{
				this.apparelStuffFilter.ResolveReferences();
			}
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x001CCE34 File Offset: 0x001CB034
		public override void PostLoad()
		{
			if (this.backstoryCategories != null && this.backstoryCategories.Count > 0)
			{
				if (this.backstoryFilters == null)
				{
					this.backstoryFilters = new List<BackstoryCategoryFilter>();
				}
				this.backstoryFilters.Add(new BackstoryCategoryFilter
				{
					categories = this.backstoryCategories
				});
			}
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x0003C8FF File Offset: 0x0003AAFF
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.pawnGroupMakers != null && this.maxPawnCostPerTotalPointsCurve == null)
			{
				yield return "has pawnGroupMakers but missing maxPawnCostPerTotalPointsCurve";
			}
			if (!this.isPlayer && this.factionNameMaker == null && this.fixedName == null)
			{
				yield return "FactionTypeDef " + this.defName + " lacks a factionNameMaker and a fixedName.";
			}
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return this.defName + " has no tech level.";
			}
			if (this.humanlikeFaction)
			{
				if (this.backstoryFilters.NullOrEmpty<BackstoryCategoryFilter>())
				{
					yield return this.defName + " is humanlikeFaction but has no backstory categories.";
				}
				if (this.hairTags.Count == 0)
				{
					yield return this.defName + " is humanlikeFaction but has no hairTags.";
				}
			}
			if (this.isPlayer)
			{
				if (this.settlementNameMaker == null)
				{
					yield return "isPlayer is true but settlementNameMaker is null";
				}
				if (this.factionNameMaker == null)
				{
					yield return "isPlayer is true but factionNameMaker is null";
				}
				if (this.playerInitialSettlementNameMaker == null)
				{
					yield return "isPlayer is true but playerInitialSettlementNameMaker is null";
				}
			}
			if (this.permanentEnemy)
			{
				if (this.mustStartOneEnemy)
				{
					yield return "permanentEnemy has mustStartOneEnemy = true, which is redundant";
				}
				if (this.goodwillDailyFall != 0f || this.goodwillDailyGain != 0f)
				{
					yield return "permanentEnemy has a goodwillDailyFall or goodwillDailyGain";
				}
				if (this.startingGoodwill != IntRange.zero)
				{
					yield return "permanentEnemy has a startingGoodwill defined";
				}
				if (this.naturalColonyGoodwill != IntRange.zero)
				{
					yield return "permanentEnemy has a naturalColonyGoodwill defined";
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x0003C90F File Offset: 0x0003AB0F
		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x0003C918 File Offset: 0x0003AB18
		public RulePackDef GetNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.pawnNameMakerFemale != null)
			{
				return this.pawnNameMakerFemale;
			}
			return this.pawnNameMaker;
		}

		// Token: 0x040038B8 RID: 14520
		public bool isPlayer;

		// Token: 0x040038B9 RID: 14521
		public RulePackDef factionNameMaker;

		// Token: 0x040038BA RID: 14522
		public RulePackDef settlementNameMaker;

		// Token: 0x040038BB RID: 14523
		public RulePackDef playerInitialSettlementNameMaker;

		// Token: 0x040038BC RID: 14524
		[MustTranslate]
		public string fixedName;

		// Token: 0x040038BD RID: 14525
		public bool humanlikeFaction = true;

		// Token: 0x040038BE RID: 14526
		public bool hidden;

		// Token: 0x040038BF RID: 14527
		public float listOrderPriority;

		// Token: 0x040038C0 RID: 14528
		public List<PawnGroupMaker> pawnGroupMakers;

		// Token: 0x040038C1 RID: 14529
		public SimpleCurve raidCommonalityFromPointsCurve;

		// Token: 0x040038C2 RID: 14530
		public bool autoFlee = true;

		// Token: 0x040038C3 RID: 14531
		public FloatRange attackersDownPercentageRangeForAutoFlee = new FloatRange(0.4f, 0.7f);

		// Token: 0x040038C4 RID: 14532
		public bool canSiege;

		// Token: 0x040038C5 RID: 14533
		public bool canStageAttacks;

		// Token: 0x040038C6 RID: 14534
		public bool canUseAvoidGrid = true;

		// Token: 0x040038C7 RID: 14535
		public float earliestRaidDays;

		// Token: 0x040038C8 RID: 14536
		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		// Token: 0x040038C9 RID: 14537
		public PawnKindDef basicMemberKind;

		// Token: 0x040038CA RID: 14538
		public List<ResearchProjectTagDef> startingResearchTags;

		// Token: 0x040038CB RID: 14539
		public List<ResearchProjectTagDef> startingTechprintsResearchTags;

		// Token: 0x040038CC RID: 14540
		[NoTranslate]
		public List<string> recipePrerequisiteTags;

		// Token: 0x040038CD RID: 14541
		public bool rescueesCanJoin;

		// Token: 0x040038CE RID: 14542
		[MustTranslate]
		public string pawnSingular = "member";

		// Token: 0x040038CF RID: 14543
		[MustTranslate]
		public string pawnsPlural = "members";

		// Token: 0x040038D0 RID: 14544
		public string leaderTitle = "leader";

		// Token: 0x040038D1 RID: 14545
		public string leaderTitleFemale;

		// Token: 0x040038D2 RID: 14546
		[MustTranslate]
		public string royalFavorLabel;

		// Token: 0x040038D3 RID: 14547
		[NoTranslate]
		public string royalFavorIconPath;

		// Token: 0x040038D4 RID: 14548
		public List<PawnKindDef> fixedLeaderKinds;

		// Token: 0x040038D5 RID: 14549
		public bool leaderForceGenerateNewPawn;

		// Token: 0x040038D6 RID: 14550
		public float forageabilityFactor = 1f;

		// Token: 0x040038D7 RID: 14551
		public SimpleCurve maxPawnCostPerTotalPointsCurve;

		// Token: 0x040038D8 RID: 14552
		public List<string> royalTitleTags;

		// Token: 0x040038D9 RID: 14553
		public string categoryTag;

		// Token: 0x040038DA RID: 14554
		public bool hostileToFactionlessHumanlikes;

		// Token: 0x040038DB RID: 14555
		public int requiredCountAtGameStart;

		// Token: 0x040038DC RID: 14556
		public int maxCountAtGameStart = 9999;

		// Token: 0x040038DD RID: 14557
		public bool canMakeRandomly;

		// Token: 0x040038DE RID: 14558
		public float settlementGenerationWeight;

		// Token: 0x040038DF RID: 14559
		public bool generateNewLeaderFromMapMembersOnly;

		// Token: 0x040038E0 RID: 14560
		public RulePackDef pawnNameMaker;

		// Token: 0x040038E1 RID: 14561
		public RulePackDef pawnNameMakerFemale;

		// Token: 0x040038E2 RID: 14562
		public TechLevel techLevel;

		// Token: 0x040038E3 RID: 14563
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x040038E4 RID: 14564
		[NoTranslate]
		private List<string> backstoryCategories;

		// Token: 0x040038E5 RID: 14565
		[NoTranslate]
		public List<string> hairTags = new List<string>();

		// Token: 0x040038E6 RID: 14566
		public ThingFilter apparelStuffFilter;

		// Token: 0x040038E7 RID: 14567
		public ThingSetMakerDef raidLootMaker;

		// Token: 0x040038E8 RID: 14568
		public SimpleCurve raidLootValueFromPointsCurve = FactionDef.DefaultRaidLootValueFromPointsCurve_NewTemp;

		// Token: 0x040038E9 RID: 14569
		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		// Token: 0x040038EA RID: 14570
		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		// Token: 0x040038EB RID: 14571
		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		// Token: 0x040038EC RID: 14572
		public float geneticVariance = 1f;

		// Token: 0x040038ED RID: 14573
		public IntRange startingGoodwill = IntRange.zero;

		// Token: 0x040038EE RID: 14574
		public bool mustStartOneEnemy;

		// Token: 0x040038EF RID: 14575
		public IntRange naturalColonyGoodwill = IntRange.zero;

		// Token: 0x040038F0 RID: 14576
		public float goodwillDailyGain;

		// Token: 0x040038F1 RID: 14577
		public float goodwillDailyFall;

		// Token: 0x040038F2 RID: 14578
		public bool permanentEnemy;

		// Token: 0x040038F3 RID: 14579
		public bool permanentEnemyToEveryoneExceptPlayer;

		// Token: 0x040038F4 RID: 14580
		public List<FactionDef> permanentEnemyToEveryoneExcept;

		// Token: 0x040038F5 RID: 14581
		[NoTranslate]
		public string settlementTexturePath;

		// Token: 0x040038F6 RID: 14582
		[NoTranslate]
		public string factionIconPath;

		// Token: 0x040038F7 RID: 14583
		public List<Color> colorSpectrum;

		// Token: 0x040038F8 RID: 14584
		public List<PawnRelationDef> royalTitleInheritanceRelations;

		// Token: 0x040038F9 RID: 14585
		public Type royalTitleInheritanceWorkerClass;

		// Token: 0x040038FA RID: 14586
		public List<RoyalImplantRule> royalImplantRules;

		// Token: 0x040038FB RID: 14587
		[Obsolete("Will be removed in the future")]
		public RoyalTitleDef minTitleForBladelinkWeapons;

		// Token: 0x040038FC RID: 14588
		public string renounceTitleMessage;

		// Token: 0x040038FD RID: 14589
		[Unsaved(false)]
		private Texture2D factionIcon;

		// Token: 0x040038FE RID: 14590
		[Unsaved(false)]
		private Texture2D settlementTexture;

		// Token: 0x040038FF RID: 14591
		[Unsaved(false)]
		private Texture2D royalFavorIcon;

		// Token: 0x04003900 RID: 14592
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAwardableInSeniorityOrderForReading;

		// Token: 0x04003901 RID: 14593
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAllInSeniorityOrderForReading;

		// Token: 0x04003902 RID: 14594
		[Unsaved(false)]
		private RoyalTitleInheritanceWorker royalTitleInheritanceWorker;

		// Token: 0x04003903 RID: 14595
		[Obsolete]
		private static readonly SimpleCurve DefaultRaidLootValueFromPointsCurve_NewTemp = new SimpleCurve
		{
			{
				new CurvePoint(35f, 15f),
				true
			},
			{
				new CurvePoint(100f, 120f),
				true
			},
			{
				new CurvePoint(1000f, 500f),
				true
			},
			{
				new CurvePoint(2000f, 800f),
				true
			},
			{
				new CurvePoint(4000f, 1000f),
				true
			}
		};
	}
}
