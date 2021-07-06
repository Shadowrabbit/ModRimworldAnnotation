using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000140 RID: 320
	public class HediffStage
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0000CB19 File Offset: 0x0000AD19
		public bool AffectsMemory
		{
			get
			{
				return this.forgetMemoryThoughtMtbDays > 0f || this.pctConditionalThoughtsNullified > 0f;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x0000CB37 File Offset: 0x0000AD37
		public bool AffectsSocialInteractions
		{
			get
			{
				return this.opinionOfOthersFactor != 1f;
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0000CB49 File Offset: 0x0000AD49
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0000CB57 File Offset: 0x0000AD57
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return HediffStatsUtility.SpecialDisplayStats(this, null);
		}

		// Token: 0x04000662 RID: 1634
		public float minSeverity;

		// Token: 0x04000663 RID: 1635
		[MustTranslate]
		public string label;

		// Token: 0x04000664 RID: 1636
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04000665 RID: 1637
		public bool becomeVisible = true;

		// Token: 0x04000666 RID: 1638
		public bool lifeThreatening;

		// Token: 0x04000667 RID: 1639
		public TaleDef tale;

		// Token: 0x04000668 RID: 1640
		public float vomitMtbDays = -1f;

		// Token: 0x04000669 RID: 1641
		public float deathMtbDays = -1f;

		// Token: 0x0400066A RID: 1642
		public float painFactor = 1f;

		// Token: 0x0400066B RID: 1643
		public float painOffset;

		// Token: 0x0400066C RID: 1644
		public float totalBleedFactor = 1f;

		// Token: 0x0400066D RID: 1645
		public float naturalHealingFactor = -1f;

		// Token: 0x0400066E RID: 1646
		public float forgetMemoryThoughtMtbDays = -1f;

		// Token: 0x0400066F RID: 1647
		public float pctConditionalThoughtsNullified;

		// Token: 0x04000670 RID: 1648
		public float opinionOfOthersFactor = 1f;

		// Token: 0x04000671 RID: 1649
		public float hungerRateFactor = 1f;

		// Token: 0x04000672 RID: 1650
		public float hungerRateFactorOffset;

		// Token: 0x04000673 RID: 1651
		public float restFallFactor = 1f;

		// Token: 0x04000674 RID: 1652
		public float restFallFactorOffset;

		// Token: 0x04000675 RID: 1653
		public float socialFightChanceFactor = 1f;

		// Token: 0x04000676 RID: 1654
		public float foodPoisoningChanceFactor = 1f;

		// Token: 0x04000677 RID: 1655
		public float mentalBreakMtbDays = -1f;

		// Token: 0x04000678 RID: 1656
		public List<MentalBreakIntensity> allowedMentalBreakIntensities;

		// Token: 0x04000679 RID: 1657
		public List<HediffDef> makeImmuneTo;

		// Token: 0x0400067A RID: 1658
		public List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();

		// Token: 0x0400067B RID: 1659
		public List<HediffGiver> hediffGivers;

		// Token: 0x0400067C RID: 1660
		public List<MentalStateGiver> mentalStateGivers;

		// Token: 0x0400067D RID: 1661
		public List<StatModifier> statOffsets;

		// Token: 0x0400067E RID: 1662
		public List<StatModifier> statFactors;

		// Token: 0x0400067F RID: 1663
		public StatDef statOffsetEffectMultiplier;

		// Token: 0x04000680 RID: 1664
		public StatDef statFactorEffectMultiplier;

		// Token: 0x04000681 RID: 1665
		public StatDef capacityFactorEffectMultiplier;

		// Token: 0x04000682 RID: 1666
		public WorkTags disabledWorkTags;

		// Token: 0x04000683 RID: 1667
		public float partEfficiencyOffset;

		// Token: 0x04000684 RID: 1668
		public bool partIgnoreMissingHP;

		// Token: 0x04000685 RID: 1669
		public bool destroyPart;
	}
}
