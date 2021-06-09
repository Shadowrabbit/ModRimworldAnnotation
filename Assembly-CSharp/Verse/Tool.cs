using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000532 RID: 1330
	public class Tool
	{
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x0001D6C2 File Offset: 0x0001B8C2
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002237 RID: 8759 RVA: 0x0001D6E3 File Offset: 0x0001B8E3
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where this.capacities.Contains(x.requiredCapacity)
				select x;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x0001D6FB File Offset: 0x0001B8FB
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x00107F3C File Offset: 0x0010613C
		public float AdjustedBaseMeleeDamageAmount(Thing ownerEquipment, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipment != null)
			{
				num *= ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true);
				if (ownerEquipment.Stuff != null && damageDef != null)
				{
					num *= ownerEquipment.Stuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x00107F88 File Offset: 0x00106188
		public float AdjustedBaseMeleeDamageAmount_NewTmp(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipmentStuff != null)
			{
				num *= ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, ownerEquipmentStuff);
				if (ownerEquipmentStuff != null && damageDef != null)
				{
					num *= ownerEquipmentStuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x0001D727 File Offset: 0x0001B927
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true));
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x0001D746 File Offset: 0x0001B946
		public float AdjustedCooldown_NewTmp(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_CooldownMultiplier, ownerEquipmentStuff));
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x0001D765 File Offset: 0x0001B965
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x0001D76D File Offset: 0x0001B96D
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x0001D77B File Offset: 0x0001B97B
		public IEnumerable<string> ConfigErrors()
		{
			if (this.id.NullOrEmpty())
			{
				yield return "tool has null id (power=" + this.power.ToString("0.##") + ")";
			}
			yield break;
		}

		// Token: 0x0400170F RID: 5903
		[Unsaved(false)]
		public string id;

		// Token: 0x04001710 RID: 5904
		[MustTranslate]
		public string label;

		// Token: 0x04001711 RID: 5905
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04001712 RID: 5906
		public bool labelUsedInLogging = true;

		// Token: 0x04001713 RID: 5907
		public List<ToolCapacityDef> capacities = new List<ToolCapacityDef>();

		// Token: 0x04001714 RID: 5908
		public float power;

		// Token: 0x04001715 RID: 5909
		public float armorPenetration = -1f;

		// Token: 0x04001716 RID: 5910
		public float cooldownTime;

		// Token: 0x04001717 RID: 5911
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04001718 RID: 5912
		public HediffDef hediff;

		// Token: 0x04001719 RID: 5913
		public float chanceFactor = 1f;

		// Token: 0x0400171A RID: 5914
		public bool alwaysTreatAsWeapon;

		// Token: 0x0400171B RID: 5915
		public List<ExtraDamage> extraMeleeDamages;

		// Token: 0x0400171C RID: 5916
		public SoundDef soundMeleeHit;

		// Token: 0x0400171D RID: 5917
		public SoundDef soundMeleeMiss;

		// Token: 0x0400171E RID: 5918
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x0400171F RID: 5919
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x04001720 RID: 5920
		[Unsaved(false)]
		private string cachedLabelCap;
	}
}
