using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047A RID: 1146
	public struct DamageInfo
	{
		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x0600228E RID: 8846 RVA: 0x000DAEE4 File Offset: 0x000D90E4
		// (set) Token: 0x0600228F RID: 8847 RVA: 0x000DAEEC File Offset: 0x000D90EC
		public DamageDef Def
		{
			get
			{
				return this.defInt;
			}
			set
			{
				this.defInt = value;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002290 RID: 8848 RVA: 0x000DAEF5 File Offset: 0x000D90F5
		public float Amount
		{
			get
			{
				if (!DebugSettings.enableDamage)
				{
					return 0f;
				}
				return this.amountInt;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002291 RID: 8849 RVA: 0x000DAF0A File Offset: 0x000D910A
		public float ArmorPenetrationInt
		{
			get
			{
				return this.armorPenetrationInt;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002292 RID: 8850 RVA: 0x000DAF12 File Offset: 0x000D9112
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x000DAF1A File Offset: 0x000D911A
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x000DAF22 File Offset: 0x000D9122
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002295 RID: 8853 RVA: 0x000DAF2A File Offset: 0x000D912A
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x000DAF32 File Offset: 0x000D9132
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x000DAF3A File Offset: 0x000D913A
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x000DAF42 File Offset: 0x000D9142
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x000DAF4A File Offset: 0x000D914A
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600229A RID: 8858 RVA: 0x000DAF52 File Offset: 0x000D9152
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x000DAF5A File Offset: 0x000D915A
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x000DAF62 File Offset: 0x000D9162
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x000DAF6A File Offset: 0x000D916A
		public bool InstigatorGuilty
		{
			get
			{
				return this.instigatorGuilty;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x000DAF72 File Offset: 0x000D9172
		public bool SpawnFilth
		{
			get
			{
				return this.spawnFilth;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600229F RID: 8863 RVA: 0x000DAF7A File Offset: 0x000D917A
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x000DAF8C File Offset: 0x000D918C
		public bool IgnoreArmor
		{
			get
			{
				return this.ignoreArmorInt;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060022A1 RID: 8865 RVA: 0x000DAF94 File Offset: 0x000D9194
		public bool IgnoreInstantKillProtection
		{
			get
			{
				return this.ignoreInstantKillProtectionInt;
			}
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x000DAF9C File Offset: 0x000D919C
		public DamageInfo(DamageDef def, float amount, float armorPenetration = 0f, float angle = -1f, Thing instigator = null, BodyPartRecord hitPart = null, ThingDef weapon = null, DamageInfo.SourceCategory category = DamageInfo.SourceCategory.ThingOrUnknown, Thing intendedTarget = null, bool instigatorGuilty = true, bool spawnFilth = true)
		{
			this.defInt = def;
			this.amountInt = amount;
			this.armorPenetrationInt = armorPenetration;
			if (angle < 0f)
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				this.angleInt = angle;
			}
			this.instigatorInt = instigator;
			this.categoryInt = category;
			this.hitPartInt = hitPart;
			this.heightInt = BodyPartHeight.Undefined;
			this.depthInt = BodyPartDepth.Undefined;
			this.weaponInt = weapon;
			this.weaponBodyPartGroupInt = null;
			this.weaponHediffInt = null;
			this.instantPermanentInjuryInt = false;
			this.allowDamagePropagationInt = true;
			this.ignoreArmorInt = false;
			this.ignoreInstantKillProtectionInt = false;
			this.instigatorGuilty = instigatorGuilty;
			this.intendedTargetInt = intendedTarget;
			this.spawnFilth = spawnFilth;
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x000DB054 File Offset: 0x000D9254
		public DamageInfo(DamageInfo cloneSource)
		{
			this.defInt = cloneSource.defInt;
			this.amountInt = cloneSource.amountInt;
			this.armorPenetrationInt = cloneSource.armorPenetrationInt;
			this.angleInt = cloneSource.angleInt;
			this.instigatorInt = cloneSource.instigatorInt;
			this.categoryInt = cloneSource.categoryInt;
			this.hitPartInt = cloneSource.hitPartInt;
			this.heightInt = cloneSource.heightInt;
			this.depthInt = cloneSource.depthInt;
			this.weaponInt = cloneSource.weaponInt;
			this.weaponBodyPartGroupInt = cloneSource.weaponBodyPartGroupInt;
			this.weaponHediffInt = cloneSource.weaponHediffInt;
			this.instantPermanentInjuryInt = cloneSource.instantPermanentInjuryInt;
			this.allowDamagePropagationInt = cloneSource.allowDamagePropagationInt;
			this.intendedTargetInt = cloneSource.intendedTargetInt;
			this.ignoreArmorInt = cloneSource.ignoreArmorInt;
			this.ignoreInstantKillProtectionInt = cloneSource.ignoreInstantKillProtectionInt;
			this.instigatorGuilty = cloneSource.instigatorGuilty;
			this.spawnFilth = cloneSource.spawnFilth;
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x000DB145 File Offset: 0x000D9345
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x000DB14E File Offset: 0x000D934E
		public void SetIgnoreArmor(bool ignoreArmor)
		{
			this.ignoreArmorInt = ignoreArmor;
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x000DB157 File Offset: 0x000D9357
		public void SetIgnoreInstantKillProtection(bool ignore)
		{
			this.ignoreInstantKillProtectionInt = ignore;
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000DB160 File Offset: 0x000D9360
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x000DB170 File Offset: 0x000D9370
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x000DB179 File Offset: 0x000D9379
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x000DB182 File Offset: 0x000D9382
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000DB18B File Offset: 0x000D938B
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x000DB194 File Offset: 0x000D9394
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x000DB1A0 File Offset: 0x000D93A0
		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0f || vec.z != 0f)
			{
				this.angleInt = Quaternion.LookRotation(vec).eulerAngles.y;
				return;
			}
			this.angleInt = (float)Rand.RangeInclusive(0, 359);
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000DB1F4 File Offset: 0x000D93F4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(def=",
				this.defInt,
				", amount= ",
				this.amountInt,
				", instigator=",
				(this.instigatorInt != null) ? this.instigatorInt.ToString() : this.categoryInt.ToString(),
				", angle=",
				this.angleInt.ToString("F1"),
				")"
			});
		}

		// Token: 0x040015C4 RID: 5572
		private DamageDef defInt;

		// Token: 0x040015C5 RID: 5573
		private float amountInt;

		// Token: 0x040015C6 RID: 5574
		private float armorPenetrationInt;

		// Token: 0x040015C7 RID: 5575
		private float angleInt;

		// Token: 0x040015C8 RID: 5576
		private Thing instigatorInt;

		// Token: 0x040015C9 RID: 5577
		private DamageInfo.SourceCategory categoryInt;

		// Token: 0x040015CA RID: 5578
		public Thing intendedTargetInt;

		// Token: 0x040015CB RID: 5579
		private bool ignoreArmorInt;

		// Token: 0x040015CC RID: 5580
		private bool ignoreInstantKillProtectionInt;

		// Token: 0x040015CD RID: 5581
		private BodyPartRecord hitPartInt;

		// Token: 0x040015CE RID: 5582
		private BodyPartHeight heightInt;

		// Token: 0x040015CF RID: 5583
		private BodyPartDepth depthInt;

		// Token: 0x040015D0 RID: 5584
		private ThingDef weaponInt;

		// Token: 0x040015D1 RID: 5585
		private BodyPartGroupDef weaponBodyPartGroupInt;

		// Token: 0x040015D2 RID: 5586
		private HediffDef weaponHediffInt;

		// Token: 0x040015D3 RID: 5587
		private bool instantPermanentInjuryInt;

		// Token: 0x040015D4 RID: 5588
		private bool allowDamagePropagationInt;

		// Token: 0x040015D5 RID: 5589
		private bool instigatorGuilty;

		// Token: 0x040015D6 RID: 5590
		private bool spawnFilth;

		// Token: 0x02001C88 RID: 7304
		public enum SourceCategory
		{
			// Token: 0x04006E19 RID: 28185
			ThingOrUnknown,
			// Token: 0x04006E1A RID: 28186
			Collapse
		}
	}
}
