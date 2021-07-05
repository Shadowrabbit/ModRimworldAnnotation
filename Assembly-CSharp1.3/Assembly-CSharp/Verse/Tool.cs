using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200038D RID: 909
	public class Tool
	{
		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x00099E3B File Offset: 0x0009803B
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

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001AB3 RID: 6835 RVA: 0x00099E5C File Offset: 0x0009805C
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where this.capacities.Contains(x.requiredCapacity)
				select x;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x00099E74 File Offset: 0x00098074
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x00099EA0 File Offset: 0x000980A0
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

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00099EEC File Offset: 0x000980EC
		public float AdjustedBaseMeleeDamageAmount(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff, DamageDef damageDef)
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

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00099F2E File Offset: 0x0009812E
		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true));
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x00099F4D File Offset: 0x0009814D
		public float AdjustedCooldown(ThingDef ownerEquipment, ThingDef ownerEquipmentStuff)
		{
			return this.cooldownTime * ((ownerEquipment == null) ? 1f : ownerEquipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_CooldownMultiplier, ownerEquipmentStuff));
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x00099F6C File Offset: 0x0009816C
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x00099F74 File Offset: 0x00098174
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x00099F82 File Offset: 0x00098182
		public IEnumerable<string> ConfigErrors()
		{
			if (this.id.NullOrEmpty())
			{
				yield return "tool has null id (power=" + this.power.ToString("0.##") + ")";
			}
			yield break;
		}

		// Token: 0x0400113B RID: 4411
		[Unsaved(false)]
		public string id;

		// Token: 0x0400113C RID: 4412
		[MustTranslate]
		public string label;

		// Token: 0x0400113D RID: 4413
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x0400113E RID: 4414
		public bool labelUsedInLogging = true;

		// Token: 0x0400113F RID: 4415
		public List<ToolCapacityDef> capacities = new List<ToolCapacityDef>();

		// Token: 0x04001140 RID: 4416
		public float power;

		// Token: 0x04001141 RID: 4417
		public float armorPenetration = -1f;

		// Token: 0x04001142 RID: 4418
		public float cooldownTime;

		// Token: 0x04001143 RID: 4419
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04001144 RID: 4420
		public HediffDef hediff;

		// Token: 0x04001145 RID: 4421
		public float chanceFactor = 1f;

		// Token: 0x04001146 RID: 4422
		public bool alwaysTreatAsWeapon;

		// Token: 0x04001147 RID: 4423
		public List<ExtraDamage> extraMeleeDamages;

		// Token: 0x04001148 RID: 4424
		public SoundDef soundMeleeHit;

		// Token: 0x04001149 RID: 4425
		public SoundDef soundMeleeMiss;

		// Token: 0x0400114A RID: 4426
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x0400114B RID: 4427
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x0400114C RID: 4428
		[Unsaved(false)]
		private string cachedLabelCap;
	}
}
