using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D0 RID: 208
	public class HediffStage
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001E84E File Offset: 0x0001CA4E
		public bool AffectsMemory
		{
			get
			{
				return this.forgetMemoryThoughtMtbDays > 0f || this.pctConditionalThoughtsNullified > 0f;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0001E86C File Offset: 0x0001CA6C
		public bool AffectsSocialInteractions
		{
			get
			{
				return this.opinionOfOthersFactor != 1f;
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001E87E File Offset: 0x0001CA7E
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001E88C File Offset: 0x0001CA8C
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return HediffStatsUtility.SpecialDisplayStats(this, null);
		}

		// Token: 0x0400046A RID: 1130
		public float minSeverity;

		// Token: 0x0400046B RID: 1131
		[MustTranslate]
		public string label;

		// Token: 0x0400046C RID: 1132
		[MustTranslate]
		public string overrideLabel;

		// Token: 0x0400046D RID: 1133
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x0400046E RID: 1134
		public bool becomeVisible = true;

		// Token: 0x0400046F RID: 1135
		public bool lifeThreatening;

		// Token: 0x04000470 RID: 1136
		public TaleDef tale;

		// Token: 0x04000471 RID: 1137
		public float vomitMtbDays = -1f;

		// Token: 0x04000472 RID: 1138
		public float deathMtbDays = -1f;

		// Token: 0x04000473 RID: 1139
		public float painFactor = 1f;

		// Token: 0x04000474 RID: 1140
		public float painOffset;

		// Token: 0x04000475 RID: 1141
		public float totalBleedFactor = 1f;

		// Token: 0x04000476 RID: 1142
		public float naturalHealingFactor = -1f;

		// Token: 0x04000477 RID: 1143
		public float forgetMemoryThoughtMtbDays = -1f;

		// Token: 0x04000478 RID: 1144
		public float pctConditionalThoughtsNullified;

		// Token: 0x04000479 RID: 1145
		public float opinionOfOthersFactor = 1f;

		// Token: 0x0400047A RID: 1146
		public float hungerRateFactor = 1f;

		// Token: 0x0400047B RID: 1147
		public float hungerRateFactorOffset;

		// Token: 0x0400047C RID: 1148
		public float restFallFactor = 1f;

		// Token: 0x0400047D RID: 1149
		public float restFallFactorOffset;

		// Token: 0x0400047E RID: 1150
		public float socialFightChanceFactor = 1f;

		// Token: 0x0400047F RID: 1151
		public float foodPoisoningChanceFactor = 1f;

		// Token: 0x04000480 RID: 1152
		public float mentalBreakMtbDays = -1f;

		// Token: 0x04000481 RID: 1153
		public string mentalBreakExplanation;

		// Token: 0x04000482 RID: 1154
		public List<MentalBreakIntensity> allowedMentalBreakIntensities;

		// Token: 0x04000483 RID: 1155
		public List<HediffDef> makeImmuneTo;

		// Token: 0x04000484 RID: 1156
		public List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();

		// Token: 0x04000485 RID: 1157
		public List<HediffGiver> hediffGivers;

		// Token: 0x04000486 RID: 1158
		public List<MentalStateGiver> mentalStateGivers;

		// Token: 0x04000487 RID: 1159
		public List<StatModifier> statOffsets;

		// Token: 0x04000488 RID: 1160
		public List<StatModifier> statFactors;

		// Token: 0x04000489 RID: 1161
		public StatDef statOffsetEffectMultiplier;

		// Token: 0x0400048A RID: 1162
		public StatDef statFactorEffectMultiplier;

		// Token: 0x0400048B RID: 1163
		public StatDef capacityFactorEffectMultiplier;

		// Token: 0x0400048C RID: 1164
		public WorkTags disabledWorkTags;

		// Token: 0x0400048D RID: 1165
		[MustTranslate]
		public string overrideTooltip;

		// Token: 0x0400048E RID: 1166
		[MustTranslate]
		public string extraTooltip;

		// Token: 0x0400048F RID: 1167
		public float partEfficiencyOffset;

		// Token: 0x04000490 RID: 1168
		public bool partIgnoreMissingHP;

		// Token: 0x04000491 RID: 1169
		public bool destroyPart;
	}
}
