using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A65 RID: 2661
	public class FactionDef : Def
	{
		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06003FE0 RID: 16352 RVA: 0x0015A584 File Offset: 0x00158784
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

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06003FE1 RID: 16353 RVA: 0x0015A650 File Offset: 0x00158850
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

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06003FE2 RID: 16354 RVA: 0x0015A710 File Offset: 0x00158910
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

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06003FE3 RID: 16355 RVA: 0x0015A744 File Offset: 0x00158944
		public bool CanEverBeNonHostile
		{
			get
			{
				return !this.permanentEnemy;
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06003FE4 RID: 16356 RVA: 0x0015A750 File Offset: 0x00158950
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

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06003FE5 RID: 16357 RVA: 0x0015A7A0 File Offset: 0x001589A0
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

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06003FE6 RID: 16358 RVA: 0x0015A7ED File Offset: 0x001589ED
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

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06003FE7 RID: 16359 RVA: 0x0015A822 File Offset: 0x00158A22
		public bool HasRoyalTitles
		{
			get
			{
				return this.RoyalTitlesAwardableInSeniorityOrderForReading.Count > 0;
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x0015A832 File Offset: 0x00158A32
		public Color DefaultColor
		{
			get
			{
				if (this.colorSpectrum.NullOrEmpty<Color>())
				{
					return Color.white;
				}
				return this.colorSpectrum[0];
			}
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x0015A854 File Offset: 0x00158A54
		public float MinPointsToGeneratePawnGroup(PawnGroupKindDef groupKind, PawnGroupMakerParms parms = null)
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
			return source.Min((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything(parms));
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x0015A8BB File Offset: 0x00158ABB
		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			return this.apparelStuffFilter == null || this.apparelStuffFilter.Allows(stuffDef);
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x0015A8D3 File Offset: 0x00158AD3
		public float RaidCommonalityFromPoints(float points)
		{
			if (points < 0f || this.raidCommonalityFromPointsCurve == null)
			{
				return 1f;
			}
			return this.raidCommonalityFromPointsCurve.Evaluate(points);
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x0015A8F7 File Offset: 0x00158AF7
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.apparelStuffFilter != null)
			{
				this.apparelStuffFilter.ResolveReferences();
			}
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0015A914 File Offset: 0x00158B14
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

		// Token: 0x06003FEE RID: 16366 RVA: 0x0015A966 File Offset: 0x00158B66
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
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return this.defName + " has no tech level.";
			}
			if (this.humanlikeFaction && this.backstoryFilters.NullOrEmpty<BackstoryCategoryFilter>())
			{
				yield return this.defName + " is humanlikeFaction but has no backstory categories.";
			}
			if (this.permanentEnemy && this.mustStartOneEnemy)
			{
				yield return "permanentEnemy has mustStartOneEnemy = true, which is redundant";
			}
			if (this.disallowedMemes != null && this.allowedMemes != null)
			{
				yield return "both disallowedMemes (black list) and allowedMemes (white list) are defined";
			}
			if (this.requiredMemes != null)
			{
				MemeDef memeDef = this.requiredMemes.FirstOrDefault((MemeDef x) => !IdeoUtility.IsMemeAllowedFor(x, this));
				if (memeDef != null)
				{
					yield return "has a required meme which is not allowed: " + memeDef.defName;
				}
			}
			if (this.raidLootValueFromPointsCurve == null)
			{
				yield return "raidLootValueFromPointsCurve must be defined";
			}
			yield break;
			yield break;
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x0015A976 File Offset: 0x00158B76
		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}

		// Token: 0x040023D5 RID: 9173
		public bool isPlayer;

		// Token: 0x040023D6 RID: 9174
		public RulePackDef factionNameMaker;

		// Token: 0x040023D7 RID: 9175
		public RulePackDef settlementNameMaker;

		// Token: 0x040023D8 RID: 9176
		public RulePackDef playerInitialSettlementNameMaker;

		// Token: 0x040023D9 RID: 9177
		[MustTranslate]
		public string fixedName;

		// Token: 0x040023DA RID: 9178
		public bool humanlikeFaction = true;

		// Token: 0x040023DB RID: 9179
		public bool hidden;

		// Token: 0x040023DC RID: 9180
		public float listOrderPriority;

		// Token: 0x040023DD RID: 9181
		public List<PawnGroupMaker> pawnGroupMakers;

		// Token: 0x040023DE RID: 9182
		public SimpleCurve raidCommonalityFromPointsCurve;

		// Token: 0x040023DF RID: 9183
		public bool autoFlee = true;

		// Token: 0x040023E0 RID: 9184
		public FloatRange attackersDownPercentageRangeForAutoFlee = new FloatRange(0.4f, 0.7f);

		// Token: 0x040023E1 RID: 9185
		public bool canSiege;

		// Token: 0x040023E2 RID: 9186
		public bool canStageAttacks;

		// Token: 0x040023E3 RID: 9187
		public bool canUseAvoidGrid = true;

		// Token: 0x040023E4 RID: 9188
		public float earliestRaidDays;

		// Token: 0x040023E5 RID: 9189
		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		// Token: 0x040023E6 RID: 9190
		public SimpleCurve minSettlementTemperatureChanceCurve;

		// Token: 0x040023E7 RID: 9191
		public PawnKindDef basicMemberKind;

		// Token: 0x040023E8 RID: 9192
		public List<ResearchProjectTagDef> startingResearchTags;

		// Token: 0x040023E9 RID: 9193
		public List<ResearchProjectTagDef> startingTechprintsResearchTags;

		// Token: 0x040023EA RID: 9194
		[NoTranslate]
		public List<string> recipePrerequisiteTags;

		// Token: 0x040023EB RID: 9195
		public bool rescueesCanJoin;

		// Token: 0x040023EC RID: 9196
		[MustTranslate]
		public string pawnSingular = "member";

		// Token: 0x040023ED RID: 9197
		[MustTranslate]
		public string pawnsPlural = "members";

		// Token: 0x040023EE RID: 9198
		public string leaderTitle = "leader";

		// Token: 0x040023EF RID: 9199
		public string leaderTitleFemale;

		// Token: 0x040023F0 RID: 9200
		[MustTranslate]
		public string royalFavorLabel;

		// Token: 0x040023F1 RID: 9201
		[NoTranslate]
		public string royalFavorIconPath;

		// Token: 0x040023F2 RID: 9202
		public List<PawnKindDef> fixedLeaderKinds;

		// Token: 0x040023F3 RID: 9203
		public bool leaderForceGenerateNewPawn;

		// Token: 0x040023F4 RID: 9204
		public float forageabilityFactor = 1f;

		// Token: 0x040023F5 RID: 9205
		public SimpleCurve maxPawnCostPerTotalPointsCurve;

		// Token: 0x040023F6 RID: 9206
		public List<string> royalTitleTags;

		// Token: 0x040023F7 RID: 9207
		public string categoryTag;

		// Token: 0x040023F8 RID: 9208
		public bool hostileToFactionlessHumanlikes;

		// Token: 0x040023F9 RID: 9209
		public int requiredCountAtGameStart;

		// Token: 0x040023FA RID: 9210
		public int maxCountAtGameStart = 9999;

		// Token: 0x040023FB RID: 9211
		public bool canMakeRandomly;

		// Token: 0x040023FC RID: 9212
		public float settlementGenerationWeight;

		// Token: 0x040023FD RID: 9213
		public bool generateNewLeaderFromMapMembersOnly;

		// Token: 0x040023FE RID: 9214
		public int maxConfigurableAtWorldCreation = -1;

		// Token: 0x040023FF RID: 9215
		public int startingCountAtWorldCreation = 1;

		// Token: 0x04002400 RID: 9216
		public int configurationListOrderPriority;

		// Token: 0x04002401 RID: 9217
		public TechLevel techLevel;

		// Token: 0x04002402 RID: 9218
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x04002403 RID: 9219
		[NoTranslate]
		private List<string> backstoryCategories;

		// Token: 0x04002404 RID: 9220
		public ThingFilter apparelStuffFilter;

		// Token: 0x04002405 RID: 9221
		public ThingSetMakerDef raidLootMaker;

		// Token: 0x04002406 RID: 9222
		public SimpleCurve raidLootValueFromPointsCurve;

		// Token: 0x04002407 RID: 9223
		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		// Token: 0x04002408 RID: 9224
		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		// Token: 0x04002409 RID: 9225
		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		// Token: 0x0400240A RID: 9226
		public float geneticVariance = 1f;

		// Token: 0x0400240B RID: 9227
		public bool mustStartOneEnemy;

		// Token: 0x0400240C RID: 9228
		public bool naturalEnemy;

		// Token: 0x0400240D RID: 9229
		public bool permanentEnemy;

		// Token: 0x0400240E RID: 9230
		public bool permanentEnemyToEveryoneExceptPlayer;

		// Token: 0x0400240F RID: 9231
		public List<FactionDef> permanentEnemyToEveryoneExcept;

		// Token: 0x04002410 RID: 9232
		[NoTranslate]
		public string settlementTexturePath;

		// Token: 0x04002411 RID: 9233
		[NoTranslate]
		public string factionIconPath;

		// Token: 0x04002412 RID: 9234
		public List<Color> colorSpectrum;

		// Token: 0x04002413 RID: 9235
		public List<PawnRelationDef> royalTitleInheritanceRelations;

		// Token: 0x04002414 RID: 9236
		public Type royalTitleInheritanceWorkerClass;

		// Token: 0x04002415 RID: 9237
		public List<RoyalImplantRule> royalImplantRules;

		// Token: 0x04002416 RID: 9238
		[Obsolete("Will be removed in the future")]
		public RoyalTitleDef minTitleForBladelinkWeapons;

		// Token: 0x04002417 RID: 9239
		public string renounceTitleMessage;

		// Token: 0x04002418 RID: 9240
		public List<CultureDef> allowedCultures;

		// Token: 0x04002419 RID: 9241
		public List<MemeDef> requiredMemes;

		// Token: 0x0400241A RID: 9242
		public List<MemeDef> allowedMemes;

		// Token: 0x0400241B RID: 9243
		public List<MemeDef> disallowedMemes;

		// Token: 0x0400241C RID: 9244
		public List<PreceptDef> disallowedPrecepts;

		// Token: 0x0400241D RID: 9245
		public List<MemeWeight> structureMemeWeights;

		// Token: 0x0400241E RID: 9246
		public bool classicIdeo;

		// Token: 0x0400241F RID: 9247
		[Unsaved(false)]
		private Texture2D factionIcon;

		// Token: 0x04002420 RID: 9248
		[Unsaved(false)]
		private Texture2D settlementTexture;

		// Token: 0x04002421 RID: 9249
		[Unsaved(false)]
		private Texture2D royalFavorIcon;

		// Token: 0x04002422 RID: 9250
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAwardableInSeniorityOrderForReading;

		// Token: 0x04002423 RID: 9251
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAllInSeniorityOrderForReading;

		// Token: 0x04002424 RID: 9252
		[Unsaved(false)]
		private RoyalTitleInheritanceWorker royalTitleInheritanceWorker;
	}
}
