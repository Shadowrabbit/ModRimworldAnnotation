﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200039D RID: 925
	public static class ArmorUtility
	{
		// Token: 0x06001707 RID: 5895 RVA: 0x000DA6FC File Offset: 0x000D88FC
		public static float GetPostArmorDamage(Pawn pawn, float amount, float armorPenetration, BodyPartRecord part, ref DamageDef damageDef, out bool deflectedByMetalArmor, out bool diminishedByMetalArmor)
		{
			deflectedByMetalArmor = false;
			diminishedByMetalArmor = false;
			if (damageDef.armorCategory == null)
			{
				return amount;
			}
			StatDef armorRatingStat = damageDef.armorCategory.armorRatingStat;
			if (pawn.apparel != null)
			{
				List<Apparel> wornApparel = pawn.apparel.WornApparel;
				for (int i = wornApparel.Count - 1; i >= 0; i--)
				{
					Apparel apparel = wornApparel[i];
					if (apparel.def.apparel.CoversBodyPart(part))
					{
						float num = amount;
						bool flag;
						ArmorUtility.ApplyArmor(ref amount, armorPenetration, apparel.GetStatValue(armorRatingStat, true), apparel, ref damageDef, pawn, out flag);
						if (amount < 0.001f)
						{
							deflectedByMetalArmor = flag;
							return 0f;
						}
						if (amount < num && flag)
						{
							diminishedByMetalArmor = true;
						}
					}
				}
			}
			float num2 = amount;
			bool flag2;
			ArmorUtility.ApplyArmor(ref amount, armorPenetration, pawn.GetStatValue(armorRatingStat, true), null, ref damageDef, pawn, out flag2);
			if (amount < 0.001f)
			{
				deflectedByMetalArmor = flag2;
				return 0f;
			}
			if (amount < num2 && flag2)
			{
				diminishedByMetalArmor = true;
			}
			return amount;
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x000DA7E8 File Offset: 0x000D89E8
		private static void ApplyArmor(ref float damAmount, float armorPenetration, float armorRating, Thing armorThing, ref DamageDef damageDef, Pawn pawn, out bool metalArmor)
		{
			if (armorThing != null)
			{
				metalArmor = (armorThing.def.apparel.useDeflectMetalEffect || (armorThing.Stuff != null && armorThing.Stuff.IsMetal));
			}
			else
			{
				metalArmor = pawn.RaceProps.IsMechanoid;
			}
			if (armorThing != null)
			{
				float f = damAmount * 0.25f;
				armorThing.TakeDamage(new DamageInfo(damageDef, (float)GenMath.RoundRandom(f), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			float num = Mathf.Max(armorRating - armorPenetration, 0f);
			float value = Rand.Value;
			float num2 = num * 0.5f;
			float num3 = num;
			if (value < num2)
			{
				damAmount = 0f;
				return;
			}
			if (value < num3)
			{
				damAmount = (float)GenMath.RoundRandom(damAmount / 2f);
				if (damageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					damageDef = DamageDefOf.Blunt;
				}
			}
		}

		// Token: 0x040011AE RID: 4526
		public const float MaxArmorRating = 2f;

		// Token: 0x040011AF RID: 4527
		public const float DeflectThresholdFactor = 0.5f;
	}
}
