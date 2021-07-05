using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A49 RID: 2633
	public class AbilityDef : Def
	{
		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06003F7D RID: 16253 RVA: 0x00158F70 File Offset: 0x00157170
		public float EntropyGain
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EntropyGain, 0f);
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06003F7E RID: 16254 RVA: 0x00158F87 File Offset: 0x00157187
		public float PsyfocusCost
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_PsyfocusCost, 0f);
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06003F7F RID: 16255 RVA: 0x00158F9E File Offset: 0x0015719E
		public float EffectRadius
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EffectRadius, 0f);
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06003F80 RID: 16256 RVA: 0x00158FB5 File Offset: 0x001571B5
		public bool HasAreaOfEffect
		{
			get
			{
				return this.EffectRadius > float.Epsilon;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06003F81 RID: 16257 RVA: 0x00158FC4 File Offset: 0x001571C4
		public float DetectionChance
		{
			get
			{
				if (this.detectionChanceOverride < 0f)
				{
					return this.GetStatValueAbstract(StatDefOf.Ability_DetectChancePerEntropy, null);
				}
				return this.detectionChanceOverride;
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06003F82 RID: 16258 RVA: 0x00158FE6 File Offset: 0x001571E6
		public bool IsPsycast
		{
			get
			{
				return typeof(Psycast).IsAssignableFrom(this.abilityClass);
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06003F83 RID: 16259 RVA: 0x00158FFD File Offset: 0x001571FD
		public string PsyfocusCostPercent
		{
			get
			{
				if (this.psyfocusCostPercent.NullOrEmpty())
				{
					this.psyfocusCostPercent = this.PsyfocusCost.ToStringPercent();
				}
				return this.psyfocusCostPercent;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06003F84 RID: 16260 RVA: 0x00159023 File Offset: 0x00157223
		public string PsyfocusCostPercentMax
		{
			get
			{
				if (this.psyfocusCostPercentMax.NullOrEmpty())
				{
					this.psyfocusCostPercentMax = this.PsyfocusCostRange.max.ToStringPercent();
				}
				return this.psyfocusCostPercentMax;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06003F85 RID: 16261 RVA: 0x00159050 File Offset: 0x00157250
		public int RequiredPsyfocusBand
		{
			get
			{
				if (this.requiredPsyfocusBandCached == -1)
				{
					this.requiredPsyfocusBandCached = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count - 1;
					for (int i = 0; i < Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count; i++)
					{
						int num = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[i];
						if (this.level <= num)
						{
							this.requiredPsyfocusBandCached = i;
							break;
						}
					}
				}
				return this.requiredPsyfocusBandCached;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06003F86 RID: 16262 RVA: 0x001590B4 File Offset: 0x001572B4
		public bool AnyCompOverridesPsyfocusCost
		{
			get
			{
				if (this.anyCompOverridesPsyfocusCost == null)
				{
					this.anyCompOverridesPsyfocusCost = new bool?(false);
					if (this.comps != null)
					{
						using (List<AbilityCompProperties>.Enumerator enumerator = this.comps.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.OverridesPsyfocusCost)
								{
									this.anyCompOverridesPsyfocusCost = new bool?(true);
									break;
								}
							}
						}
					}
				}
				return this.anyCompOverridesPsyfocusCost.Value;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06003F87 RID: 16263 RVA: 0x00159140 File Offset: 0x00157340
		public FloatRange PsyfocusCostRange
		{
			get
			{
				if (this.psyfocusCostRange.min < 0f)
				{
					if (!this.AnyCompOverridesPsyfocusCost)
					{
						this.psyfocusCostRange = new FloatRange(this.PsyfocusCost, this.PsyfocusCost);
					}
					else
					{
						foreach (AbilityCompProperties abilityCompProperties in this.comps)
						{
							if (abilityCompProperties.OverridesPsyfocusCost)
							{
								this.psyfocusCostRange = abilityCompProperties.PsyfocusCostRange;
								break;
							}
						}
					}
				}
				return this.psyfocusCostRange;
			}
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x001591DC File Offset: 0x001573DC
		public IEnumerable<string> StatSummary(Pawn forPawn = null)
		{
			string text = null;
			foreach (AbilityCompProperties abilityCompProperties in this.comps)
			{
				if (abilityCompProperties.OverridesPsyfocusCost)
				{
					text = abilityCompProperties.PsyfocusCostExplanation;
					break;
				}
			}
			if (text == null)
			{
				if (this.PsyfocusCost > 1E-45f)
				{
					yield return "AbilityPsyfocusCost".Translate() + ": " + this.PsyfocusCost.ToStringPercent();
				}
			}
			else
			{
				yield return text;
			}
			if (this.EntropyGain > 1E-45f)
			{
				yield return "AbilityEntropyGain".Translate() + ": " + this.EntropyGain;
			}
			if (this.verbProperties.warmupTime > 1E-45f)
			{
				yield return "AbilityCastingTime".Translate() + ": " + this.verbProperties.warmupTime + "LetterSecond".Translate();
			}
			float num = this.EffectDuration(forPawn);
			if (num > 1E-45f)
			{
				int num2 = num.SecondsToTicks();
				yield return "AbilityDuration".Translate() + ": " + ((num2 >= 2500) ? num2.ToStringTicksToPeriod(true, false, true, true) : (num + "LetterSecond".Translate()));
			}
			if (this.HasAreaOfEffect)
			{
				yield return "AbilityEffectRadius".Translate() + ": " + Mathf.Ceil(this.EffectRadius);
			}
			yield break;
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x001591F3 File Offset: 0x001573F3
		public float EffectDuration(Pawn forPawn = null)
		{
			return this.GetStatValueAbstract(StatDefOf.Ability_Duration, forPawn);
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x00159201 File Offset: 0x00157401
		public override void PostLoad()
		{
			if (!string.IsNullOrEmpty(this.iconPath))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x00159224 File Offset: 0x00157424
		public string GetTooltip(Pawn pawn = null)
		{
			if (this.cachedTooltip == null || this.cachedTooltipPawn != pawn)
			{
				this.cachedTooltip = this.LabelCap + ((this.level > 0) ? ("\n" + "Level".Translate().CapitalizeFirst() + " " + this.level) : "") + "\n\n" + this.description;
				this.cachedTooltipPawn = pawn;
				string text = this.StatSummary(pawn).ToLineList(null, false);
				if (!text.NullOrEmpty())
				{
					this.cachedTooltip = this.cachedTooltip + "\n\n" + text;
				}
			}
			if (pawn != null && ModsConfig.RoyaltyActive && this.abilityClass == typeof(Psycast) && this.level > 0)
			{
				Faction first = Faction.GetMinTitleForImplantAllFactions(HediffDefOf.PsychicAmplifier).First;
				if (first != null)
				{
					RoyalTitleDef minTitleForImplant = first.GetMinTitleForImplant(HediffDefOf.PsychicAmplifier, this.level);
					RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(first);
					if (minTitleForImplant != null && (currentTitle == null || currentTitle.seniority < minTitleForImplant.seniority) && this.DetectionChance > 0f)
					{
						return this.cachedTooltip + "\n\n" + "PsycastIsIllegal".Translate(pawn.Named("PAWN"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE")).Colorize(ColoredText.WarningColor);
					}
				}
			}
			return this.cachedTooltip;
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x001593D2 File Offset: 0x001575D2
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.cachedTargets == null)
			{
				this.cachedTargets = new List<string>();
				if (this.verbProperties.targetParams.canTargetPawns && this.verbProperties.targetParams.canTargetSelf)
				{
					this.cachedTargets.Add("TargetSelf".Translate());
				}
				if (this.verbProperties.targetParams.canTargetLocations)
				{
					this.cachedTargets.Add("TargetGround".Translate());
				}
				if (this.verbProperties.targetParams.canTargetPawns && this.verbProperties.targetParams.canTargetHumans)
				{
					this.cachedTargets.Add("TargetHuman".Translate());
				}
				if (this.verbProperties.targetParams.canTargetPawns && this.verbProperties.targetParams.canTargetAnimals)
				{
					this.cachedTargets.Add("TargetAnimal".Translate());
				}
			}
			int num = this.comps.OfType<CompProperties_AbilityEffect>().Sum((CompProperties_AbilityEffect e) => e.goodwillImpact);
			if (num != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_GoodwillImpact, (float)num, req, ToStringNumberSense.Undefined, null, false);
			}
			if (this.level != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_RequiredPsylink, (float)this.level, req, ToStringNumberSense.Undefined, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_CastingTime, this.verbProperties.warmupTime, req, ToStringNumberSense.Undefined, null, false);
			if (this.verbProperties.range > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_Range, this.verbProperties.range, req, ToStringNumberSense.Undefined, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, "Target".Translate(), this.cachedTargets.ToCommaList(false, false).CapitalizeFirst(), "AbilityTargetDesc".Translate(), 1001, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, "AbilityRequiresLOS".Translate(), this.verbProperties.requireLineOfSight ? "Yes".Translate() : "No".Translate(), "", 1000, null, null, false);
			yield break;
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003F8D RID: 16269 RVA: 0x001593E9 File Offset: 0x001575E9
		public Texture2D WarmupMoteSocialSymbol
		{
			get
			{
				if (!this.warmupMoteSocialSymbol.NullOrEmpty() && this.warmupMoteSocialSymbolCached == null)
				{
					this.warmupMoteSocialSymbolCached = ContentFinder<Texture2D>.Get(this.warmupMoteSocialSymbol, true);
				}
				return this.warmupMoteSocialSymbolCached;
			}
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x0015941E File Offset: 0x0015761E
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.abilityClass == null)
			{
				yield return "abilityClass is null";
			}
			if (this.verbProperties == null)
			{
				yield return "verbProperties are null";
			}
			if (this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			if (this.statBases != null)
			{
				using (List<StatModifier>.Enumerator enumerator2 = this.statBases.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						StatModifier statBase = enumerator2.Current;
						if (this.statBases.Count((StatModifier st) => st.stat == statBase.stat) > 1)
						{
							yield return "defines the stat base " + statBase.stat + " more than once.";
						}
					}
				}
				List<StatModifier>.Enumerator enumerator2 = default(List<StatModifier>.Enumerator);
			}
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (string text2 in this.comps[i].ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x040022EE RID: 8942
		public Type abilityClass = typeof(Ability);

		// Token: 0x040022EF RID: 8943
		public Type gizmoClass = typeof(Command_Ability);

		// Token: 0x040022F0 RID: 8944
		public List<AbilityCompProperties> comps = new List<AbilityCompProperties>();

		// Token: 0x040022F1 RID: 8945
		public AbilityCategoryDef category;

		// Token: 0x040022F2 RID: 8946
		public List<StatModifier> statBases;

		// Token: 0x040022F3 RID: 8947
		public VerbProperties verbProperties;

		// Token: 0x040022F4 RID: 8948
		public KeyBindingDef hotKey;

		// Token: 0x040022F5 RID: 8949
		public JobDef jobDef;

		// Token: 0x040022F6 RID: 8950
		public ThingDef warmupMote;

		// Token: 0x040022F7 RID: 8951
		public FleckDef emittedFleck;

		// Token: 0x040022F8 RID: 8952
		public int emissionInterval;

		// Token: 0x040022F9 RID: 8953
		public string warmupMoteSocialSymbol;

		// Token: 0x040022FA RID: 8954
		public SoundDef warmupStartSound;

		// Token: 0x040022FB RID: 8955
		public SoundDef warmupSound;

		// Token: 0x040022FC RID: 8956
		public SoundDef warmupPreEndSound;

		// Token: 0x040022FD RID: 8957
		public float warmupPreEndSoundSeconds;

		// Token: 0x040022FE RID: 8958
		public Vector3 moteDrawOffset;

		// Token: 0x040022FF RID: 8959
		public float moteOffsetAmountTowardsTarget;

		// Token: 0x04002300 RID: 8960
		public bool canUseAoeToGetTargets = true;

		// Token: 0x04002301 RID: 8961
		public bool targetRequired = true;

		// Token: 0x04002302 RID: 8962
		public bool targetWorldCell;

		// Token: 0x04002303 RID: 8963
		public bool showGizmoOnWorldView;

		// Token: 0x04002304 RID: 8964
		public int level;

		// Token: 0x04002305 RID: 8965
		public IntRange cooldownTicksRange;

		// Token: 0x04002306 RID: 8966
		public AbilityGroupDef groupDef;

		// Token: 0x04002307 RID: 8967
		public bool overrideGroupCooldown;

		// Token: 0x04002308 RID: 8968
		public List<MemeDef> requiredMemes;

		// Token: 0x04002309 RID: 8969
		public bool sendLetterOnCooldownComplete;

		// Token: 0x0400230A RID: 8970
		public bool displayGizmoWhileUndrafted;

		// Token: 0x0400230B RID: 8971
		public bool disableGizmoWhileUndrafted = true;

		// Token: 0x0400230C RID: 8972
		public bool writeCombatLog;

		// Token: 0x0400230D RID: 8973
		public bool stunTargetWhileCasting;

		// Token: 0x0400230E RID: 8974
		public bool showPsycastEffects = true;

		// Token: 0x0400230F RID: 8975
		public bool showCastingProgressBar;

		// Token: 0x04002310 RID: 8976
		public float detectionChanceOverride = -1f;

		// Token: 0x04002311 RID: 8977
		public float uiOrder;

		// Token: 0x04002312 RID: 8978
		[MustTranslate]
		public string confirmationDialogText;

		// Token: 0x04002313 RID: 8979
		[NoTranslate]
		public string iconPath;

		// Token: 0x04002314 RID: 8980
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x04002315 RID: 8981
		private string cachedTooltip;

		// Token: 0x04002316 RID: 8982
		private Pawn cachedTooltipPawn;

		// Token: 0x04002317 RID: 8983
		private List<string> cachedTargets;

		// Token: 0x04002318 RID: 8984
		private int requiredPsyfocusBandCached = -1;

		// Token: 0x04002319 RID: 8985
		private bool? anyCompOverridesPsyfocusCost;

		// Token: 0x0400231A RID: 8986
		private FloatRange psyfocusCostRange = new FloatRange(-1f, -1f);

		// Token: 0x0400231B RID: 8987
		private string psyfocusCostPercent;

		// Token: 0x0400231C RID: 8988
		private string psyfocusCostPercentMax;

		// Token: 0x0400231D RID: 8989
		private Texture2D warmupMoteSocialSymbolCached;
	}
}
