using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020000EF RID: 239
	public class DamageDef : Def
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0000BB64 File Offset: 0x00009D64
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

		// Token: 0x06000702 RID: 1794 RVA: 0x00090A0C File Offset: 0x0008EC0C
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

		// Token: 0x040003E5 RID: 997
		public Type workerClass = typeof(DamageWorker);

		// Token: 0x040003E6 RID: 998
		private bool externalViolence;

		// Token: 0x040003E7 RID: 999
		private bool externalViolenceForMechanoids;

		// Token: 0x040003E8 RID: 1000
		public bool hasForcefulImpact = true;

		// Token: 0x040003E9 RID: 1001
		public bool harmsHealth = true;

		// Token: 0x040003EA RID: 1002
		public bool makesBlood = true;

		// Token: 0x040003EB RID: 1003
		public bool canInterruptJobs = true;

		// Token: 0x040003EC RID: 1004
		public bool isRanged;

		// Token: 0x040003ED RID: 1005
		public bool makesAnimalsFlee;

		// Token: 0x040003EE RID: 1006
		public bool execution;

		// Token: 0x040003EF RID: 1007
		public RulePackDef combatLogRules;

		// Token: 0x040003F0 RID: 1008
		public float buildingDamageFactor = 1f;

		// Token: 0x040003F1 RID: 1009
		public float plantDamageFactor = 1f;

		// Token: 0x040003F2 RID: 1010
		public bool canUseDeflectMetalEffect = true;

		// Token: 0x040003F3 RID: 1011
		public ImpactSoundTypeDef impactSoundType;

		// Token: 0x040003F4 RID: 1012
		[MustTranslate]
		public string deathMessage = "{0} has been killed.";

		// Token: 0x040003F5 RID: 1013
		public EffecterDef damageEffecter;

		// Token: 0x040003F6 RID: 1014
		public int defaultDamage = -1;

		// Token: 0x040003F7 RID: 1015
		public float defaultArmorPenetration = -1f;

		// Token: 0x040003F8 RID: 1016
		public float defaultStoppingPower;

		// Token: 0x040003F9 RID: 1017
		public List<DamageDefAdditionalHediff> additionalHediffs;

		// Token: 0x040003FA RID: 1018
		public DamageArmorCategoryDef armorCategory;

		// Token: 0x040003FB RID: 1019
		public int minDamageToFragment = 99999;

		// Token: 0x040003FC RID: 1020
		public FloatRange overkillPctToDestroyPart = new FloatRange(0f, 0.7f);

		// Token: 0x040003FD RID: 1021
		public bool harmAllLayersUntilOutside;

		// Token: 0x040003FE RID: 1022
		public HediffDef hediff;

		// Token: 0x040003FF RID: 1023
		public HediffDef hediffSkin;

		// Token: 0x04000400 RID: 1024
		public HediffDef hediffSolid;

		// Token: 0x04000401 RID: 1025
		public bool isExplosive;

		// Token: 0x04000402 RID: 1026
		public float explosionSnowMeltAmount = 1f;

		// Token: 0x04000403 RID: 1027
		public bool explosionAffectOutsidePartsOnly = true;

		// Token: 0x04000404 RID: 1028
		public ThingDef explosionCellMote;

		// Token: 0x04000405 RID: 1029
		public Color explosionColorCenter = Color.white;

		// Token: 0x04000406 RID: 1030
		public Color explosionColorEdge = Color.white;

		// Token: 0x04000407 RID: 1031
		public ThingDef explosionInteriorMote;

		// Token: 0x04000408 RID: 1032
		public float explosionHeatEnergyPerCell;

		// Token: 0x04000409 RID: 1033
		public SoundDef soundExplosion;

		// Token: 0x0400040A RID: 1034
		public float stabChanceOfForcedInternal;

		// Token: 0x0400040B RID: 1035
		public float stabPierceBonus;

		// Token: 0x0400040C RID: 1036
		public SimpleCurve cutExtraTargetsCurve;

		// Token: 0x0400040D RID: 1037
		public float cutCleaveBonus;

		// Token: 0x0400040E RID: 1038
		public float bluntInnerHitChance;

		// Token: 0x0400040F RID: 1039
		public FloatRange bluntInnerHitDamageFractionToConvert;

		// Token: 0x04000410 RID: 1040
		public FloatRange bluntInnerHitDamageFractionToAdd;

		// Token: 0x04000411 RID: 1041
		public float bluntStunDuration = 1f;

		// Token: 0x04000412 RID: 1042
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToHeadCurve;

		// Token: 0x04000413 RID: 1043
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToBodyCurve;

		// Token: 0x04000414 RID: 1044
		public float scratchSplitPercentage = 0.5f;

		// Token: 0x04000415 RID: 1045
		public float biteDamageMultiplier = 1f;

		// Token: 0x04000416 RID: 1046
		[Unsaved(false)]
		private DamageWorker workerInt;
	}
}
