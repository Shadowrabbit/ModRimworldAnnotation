using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200008F RID: 143
	public class DamageDef : Def
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001A33E File Offset: 0x0001853E
		public DamageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (DamageWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001A370 File Offset: 0x00018570
		public bool ExternalViolenceFor(Thing thing)
		{
			if (this.externalViolence)
			{
				return true;
			}
			if (this.externalViolenceForMechanoids)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (thing is Building_Turret)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000219 RID: 537
		public Type workerClass = typeof(DamageWorker);

		// Token: 0x0400021A RID: 538
		private bool externalViolence;

		// Token: 0x0400021B RID: 539
		private bool externalViolenceForMechanoids;

		// Token: 0x0400021C RID: 540
		public bool hasForcefulImpact = true;

		// Token: 0x0400021D RID: 541
		public bool harmsHealth = true;

		// Token: 0x0400021E RID: 542
		public bool makesBlood = true;

		// Token: 0x0400021F RID: 543
		public bool canInterruptJobs = true;

		// Token: 0x04000220 RID: 544
		public bool isRanged;

		// Token: 0x04000221 RID: 545
		public bool makesAnimalsFlee;

		// Token: 0x04000222 RID: 546
		public bool execution;

		// Token: 0x04000223 RID: 547
		public RulePackDef combatLogRules;

		// Token: 0x04000224 RID: 548
		public float buildingDamageFactor = 1f;

		// Token: 0x04000225 RID: 549
		public float plantDamageFactor = 1f;

		// Token: 0x04000226 RID: 550
		public bool canUseDeflectMetalEffect = true;

		// Token: 0x04000227 RID: 551
		public ImpactSoundTypeDef impactSoundType;

		// Token: 0x04000228 RID: 552
		[MustTranslate]
		public string deathMessage = "{0} has been killed.";

		// Token: 0x04000229 RID: 553
		public EffecterDef damageEffecter;

		// Token: 0x0400022A RID: 554
		public int defaultDamage = -1;

		// Token: 0x0400022B RID: 555
		public float defaultArmorPenetration = -1f;

		// Token: 0x0400022C RID: 556
		public float defaultStoppingPower;

		// Token: 0x0400022D RID: 557
		public List<DamageDefAdditionalHediff> additionalHediffs;

		// Token: 0x0400022E RID: 558
		public bool applyAdditionalHediffsIfHuntingForFood = true;

		// Token: 0x0400022F RID: 559
		public DamageArmorCategoryDef armorCategory;

		// Token: 0x04000230 RID: 560
		public int minDamageToFragment = 99999;

		// Token: 0x04000231 RID: 561
		public FloatRange overkillPctToDestroyPart = new FloatRange(0f, 0.7f);

		// Token: 0x04000232 RID: 562
		public bool harmAllLayersUntilOutside;

		// Token: 0x04000233 RID: 563
		public HediffDef hediff;

		// Token: 0x04000234 RID: 564
		public HediffDef hediffSkin;

		// Token: 0x04000235 RID: 565
		public HediffDef hediffSolid;

		// Token: 0x04000236 RID: 566
		public bool isExplosive;

		// Token: 0x04000237 RID: 567
		public float explosionSnowMeltAmount = 1f;

		// Token: 0x04000238 RID: 568
		public bool explosionAffectOutsidePartsOnly = true;

		// Token: 0x04000239 RID: 569
		public ThingDef explosionCellMote;

		// Token: 0x0400023A RID: 570
		public FleckDef explosionCellFleck;

		// Token: 0x0400023B RID: 571
		public Color explosionColorCenter = Color.white;

		// Token: 0x0400023C RID: 572
		public Color explosionColorEdge = Color.white;

		// Token: 0x0400023D RID: 573
		public ThingDef explosionInteriorMote;

		// Token: 0x0400023E RID: 574
		public FleckDef explosionInteriorFleck;

		// Token: 0x0400023F RID: 575
		public float explosionHeatEnergyPerCell;

		// Token: 0x04000240 RID: 576
		public SoundDef soundExplosion;

		// Token: 0x04000241 RID: 577
		public float stabChanceOfForcedInternal;

		// Token: 0x04000242 RID: 578
		public float stabPierceBonus;

		// Token: 0x04000243 RID: 579
		public SimpleCurve cutExtraTargetsCurve;

		// Token: 0x04000244 RID: 580
		public float cutCleaveBonus;

		// Token: 0x04000245 RID: 581
		public float bluntInnerHitChance;

		// Token: 0x04000246 RID: 582
		public FloatRange bluntInnerHitDamageFractionToConvert;

		// Token: 0x04000247 RID: 583
		public FloatRange bluntInnerHitDamageFractionToAdd;

		// Token: 0x04000248 RID: 584
		public float bluntStunDuration = 1f;

		// Token: 0x04000249 RID: 585
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToHeadCurve;

		// Token: 0x0400024A RID: 586
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToBodyCurve;

		// Token: 0x0400024B RID: 587
		public float scratchSplitPercentage = 0.5f;

		// Token: 0x0400024C RID: 588
		public float biteDamageMultiplier = 1f;

		// Token: 0x0400024D RID: 589
		[Unsaved(false)]
		private DamageWorker workerInt;
	}
}
