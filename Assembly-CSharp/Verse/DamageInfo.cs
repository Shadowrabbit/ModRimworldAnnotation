using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007D8 RID: 2008
	public struct DamageInfo
	{
		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x00027795 File Offset: 0x00025995
		// (set) Token: 0x06003258 RID: 12888 RVA: 0x0002779D File Offset: 0x0002599D
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

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06003259 RID: 12889 RVA: 0x000277A6 File Offset: 0x000259A6
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

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x0600325A RID: 12890 RVA: 0x000277BB File Offset: 0x000259BB
		public float ArmorPenetrationInt
		{
			get
			{
				return this.armorPenetrationInt;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600325B RID: 12891 RVA: 0x000277C3 File Offset: 0x000259C3
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600325C RID: 12892 RVA: 0x000277CB File Offset: 0x000259CB
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x0600325D RID: 12893 RVA: 0x000277D3 File Offset: 0x000259D3
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x0600325E RID: 12894 RVA: 0x000277DB File Offset: 0x000259DB
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x0600325F RID: 12895 RVA: 0x000277E3 File Offset: 0x000259E3
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x000277EB File Offset: 0x000259EB
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06003261 RID: 12897 RVA: 0x000277F3 File Offset: 0x000259F3
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06003262 RID: 12898 RVA: 0x000277FB File Offset: 0x000259FB
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06003263 RID: 12899 RVA: 0x00027803 File Offset: 0x00025A03
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06003264 RID: 12900 RVA: 0x0002780B File Offset: 0x00025A0B
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06003265 RID: 12901 RVA: 0x00027813 File Offset: 0x00025A13
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06003266 RID: 12902 RVA: 0x0002781B File Offset: 0x00025A1B
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06003267 RID: 12903 RVA: 0x0002782D File Offset: 0x00025A2D
		public bool IgnoreArmor
		{
			get
			{
				return this.ignoreArmorInt;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06003268 RID: 12904 RVA: 0x00027835 File Offset: 0x00025A35
		public bool IgnoreInstantKillProtection
		{
			get
			{
				return this.ignoreInstantKillProtectionInt;
			}
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x0014D410 File Offset: 0x0014B610
		public DamageInfo(DamageDef def, float amount, float armorPenetration = 0f, float angle = -1f, Thing instigator = null, BodyPartRecord hitPart = null, ThingDef weapon = null, DamageInfo.SourceCategory category = DamageInfo.SourceCategory.ThingOrUnknown, Thing intendedTarget = null)
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
			this.intendedTargetInt = intendedTarget;
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x0014D4B8 File Offset: 0x0014B6B8
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
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x0002783D File Offset: 0x00025A3D
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x00027846 File Offset: 0x00025A46
		public void SetIgnoreArmor(bool ignoreArmor)
		{
			this.ignoreArmorInt = ignoreArmor;
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x0002784F File Offset: 0x00025A4F
		public void SetIgnoreInstantKillProtection(bool ignore)
		{
			this.ignoreInstantKillProtectionInt = ignore;
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x00027858 File Offset: 0x00025A58
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x00027868 File Offset: 0x00025A68
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x00027871 File Offset: 0x00025A71
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x0002787A File Offset: 0x00025A7A
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x00027883 File Offset: 0x00025A83
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x0002788C File Offset: 0x00025A8C
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x0014D594 File Offset: 0x0014B794
		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0f || vec.z != 0f)
			{
				this.angleInt = Quaternion.LookRotation(vec).eulerAngles.y;
				return;
			}
			this.angleInt = (float)Rand.RangeInclusive(0, 359);
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x0014D5E8 File Offset: 0x0014B7E8
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

		// Token: 0x040022EB RID: 8939
		private DamageDef defInt;

		// Token: 0x040022EC RID: 8940
		private float amountInt;

		// Token: 0x040022ED RID: 8941
		private float armorPenetrationInt;

		// Token: 0x040022EE RID: 8942
		private float angleInt;

		// Token: 0x040022EF RID: 8943
		private Thing instigatorInt;

		// Token: 0x040022F0 RID: 8944
		private DamageInfo.SourceCategory categoryInt;

		// Token: 0x040022F1 RID: 8945
		public Thing intendedTargetInt;

		// Token: 0x040022F2 RID: 8946
		private bool ignoreArmorInt;

		// Token: 0x040022F3 RID: 8947
		private bool ignoreInstantKillProtectionInt;

		// Token: 0x040022F4 RID: 8948
		private BodyPartRecord hitPartInt;

		// Token: 0x040022F5 RID: 8949
		private BodyPartHeight heightInt;

		// Token: 0x040022F6 RID: 8950
		private BodyPartDepth depthInt;

		// Token: 0x040022F7 RID: 8951
		private ThingDef weaponInt;

		// Token: 0x040022F8 RID: 8952
		private BodyPartGroupDef weaponBodyPartGroupInt;

		// Token: 0x040022F9 RID: 8953
		private HediffDef weaponHediffInt;

		// Token: 0x040022FA RID: 8954
		private bool instantPermanentInjuryInt;

		// Token: 0x040022FB RID: 8955
		private bool allowDamagePropagationInt;

		// Token: 0x020007D9 RID: 2009
		public enum SourceCategory
		{
			// Token: 0x040022FD RID: 8957
			ThingOrUnknown,
			// Token: 0x040022FE RID: 8958
			Collapse
		}
	}
}
