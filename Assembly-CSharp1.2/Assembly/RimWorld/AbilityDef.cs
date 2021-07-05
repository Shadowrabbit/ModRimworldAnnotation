using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6A RID: 3946
	public class AbilityDef : Def
	{
		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x060056A0 RID: 22176 RVA: 0x0003C16B File Offset: 0x0003A36B
		public float EntropyGain
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EntropyGain, 0f);
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x060056A1 RID: 22177 RVA: 0x0003C182 File Offset: 0x0003A382
		public float PsyfocusCost
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_PsyfocusCost, 0f);
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x060056A2 RID: 22178 RVA: 0x0003C199 File Offset: 0x0003A399
		public float EffectRadius
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EffectRadius, 0f);
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x060056A3 RID: 22179 RVA: 0x0003C1B0 File Offset: 0x0003A3B0
		public float EffectDuration
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_Duration, 0f);
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x060056A4 RID: 22180 RVA: 0x0003C1C7 File Offset: 0x0003A3C7
		public bool HasAreaOfEffect
		{
			get
			{
				return this.EffectRadius > float.Epsilon;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x060056A5 RID: 22181 RVA: 0x0003C1D6 File Offset: 0x0003A3D6
		public float DetectionChance
		{
			get
			{
				if (this.detectionChanceOverride < 0f)
				{
					return this.GetStatValueAbstract(StatDefOf.Ability_DetectChancePerEntropy);
				}
				return this.detectionChanceOverride;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x060056A6 RID: 22182 RVA: 0x0003C1F7 File Offset: 0x0003A3F7
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

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x0003C21D File Offset: 0x0003A41D
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

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x060056A8 RID: 22184 RVA: 0x001CACF0 File Offset: 0x001C8EF0
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

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x060056A9 RID: 22185 RVA: 0x001CAD54 File Offset: 0x001C8F54
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

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x060056AA RID: 22186 RVA: 0x001CADE0 File Offset: 0x001C8FE0
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

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x060056AB RID: 22187 RVA: 0x0003C248 File Offset: 0x0003A448
		public IEnumerable<string> StatSummary
		{
			get
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
				float effectDuration = this.EffectDuration;
				if (effectDuration > 1E-45f)
				{
					int num = effectDuration.SecondsToTicks();
					yield return "AbilityDuration".Translate() + ": " + ((num >= 2500) ? num.ToStringTicksToPeriod(true, false, true, true) : (effectDuration + "LetterSecond".Translate()));
				}
				if (this.HasAreaOfEffect)
				{
					yield return "AbilityEffectRadius".Translate() + ": " + Mathf.Ceil(this.EffectRadius);
				}
				yield break;
			}
		}

		// Token: 0x060056AC RID: 22188 RVA: 0x0003C258 File Offset: 0x0003A458
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

		// Token: 0x060056AD RID: 22189 RVA: 0x001CAE7C File Offset: 0x001C907C
		public string GetTooltip(Pawn pawn = null)
		{
			if (this.cachedTooltip == null)
			{
				this.cachedTooltip = base.LabelCap + ((this.level > 0) ? ("\n" + "Level".Translate() + " " + this.level) : "") + "\n\n" + this.description;
				string text = this.StatSummary.ToLineList(null, false);
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

		// Token: 0x060056AE RID: 22190 RVA: 0x0003C278 File Offset: 0x0003A478
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
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, "Target".Translate(), this.cachedTargets.ToCommaList(false).CapitalizeFirst(), "AbilityTargetDesc".Translate(), 1001, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, "AbilityRequiresLOS".Translate(), this.verbProperties.requireLineOfSight ? "Yes".Translate() : "No".Translate(), "", 1000, null, null, false);
			yield break;
		}

		// Token: 0x060056AF RID: 22191 RVA: 0x0003C28F File Offset: 0x0003A48F
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

		// Token: 0x040037E6 RID: 14310
		public Type abilityClass = typeof(Ability);

		// Token: 0x040037E7 RID: 14311
		public Type gizmoClass = typeof(Command_Ability);

		// Token: 0x040037E8 RID: 14312
		public List<AbilityCompProperties> comps = new List<AbilityCompProperties>();

		// Token: 0x040037E9 RID: 14313
		public AbilityCategoryDef category;

		// Token: 0x040037EA RID: 14314
		public List<StatModifier> statBases;

		// Token: 0x040037EB RID: 14315
		public VerbProperties verbProperties;

		// Token: 0x040037EC RID: 14316
		public KeyBindingDef hotKey;

		// Token: 0x040037ED RID: 14317
		public JobDef jobDef;

		// Token: 0x040037EE RID: 14318
		public ThingDef warmupMote;

		// Token: 0x040037EF RID: 14319
		public SoundDef warmupStartSound;

		// Token: 0x040037F0 RID: 14320
		public SoundDef warmupSound;

		// Token: 0x040037F1 RID: 14321
		public SoundDef warmupPreEndSound;

		// Token: 0x040037F2 RID: 14322
		public float warmupPreEndSoundSeconds;

		// Token: 0x040037F3 RID: 14323
		public Vector3 moteDrawOffset;

		// Token: 0x040037F4 RID: 14324
		public float moteOffsetAmountTowardsTarget;

		// Token: 0x040037F5 RID: 14325
		public bool canUseAoeToGetTargets = true;

		// Token: 0x040037F6 RID: 14326
		public bool targetRequired = true;

		// Token: 0x040037F7 RID: 14327
		public bool targetWorldCell;

		// Token: 0x040037F8 RID: 14328
		public bool showGizmoOnWorldView;

		// Token: 0x040037F9 RID: 14329
		public int level;

		// Token: 0x040037FA RID: 14330
		public IntRange cooldownTicksRange;

		// Token: 0x040037FB RID: 14331
		public bool sendLetterOnCooldownComplete;

		// Token: 0x040037FC RID: 14332
		public bool displayGizmoWhileUndrafted;

		// Token: 0x040037FD RID: 14333
		public bool disableGizmoWhileUndrafted = true;

		// Token: 0x040037FE RID: 14334
		public bool writeCombatLog;

		// Token: 0x040037FF RID: 14335
		public bool stunTargetWhileCasting;

		// Token: 0x04003800 RID: 14336
		public bool showPsycastEffects = true;

		// Token: 0x04003801 RID: 14337
		public bool showCastingProgressBar;

		// Token: 0x04003802 RID: 14338
		public float detectionChanceOverride = -1f;

		// Token: 0x04003803 RID: 14339
		[MustTranslate]
		public string confirmationDialogText;

		// Token: 0x04003804 RID: 14340
		[NoTranslate]
		public string iconPath;

		// Token: 0x04003805 RID: 14341
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x04003806 RID: 14342
		private string cachedTooltip;

		// Token: 0x04003807 RID: 14343
		private List<string> cachedTargets;

		// Token: 0x04003808 RID: 14344
		private int requiredPsyfocusBandCached = -1;

		// Token: 0x04003809 RID: 14345
		private bool? anyCompOverridesPsyfocusCost;

		// Token: 0x0400380A RID: 14346
		private FloatRange psyfocusCostRange = new FloatRange(-1f, -1f);

		// Token: 0x0400380B RID: 14347
		private string psyfocusCostPercent;

		// Token: 0x0400380C RID: 14348
		private string psyfocusCostPercentMax;
	}
}
