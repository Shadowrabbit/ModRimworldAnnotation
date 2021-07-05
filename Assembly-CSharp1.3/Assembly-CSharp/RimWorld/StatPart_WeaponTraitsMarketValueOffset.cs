using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E9 RID: 5353
	public class StatPart_WeaponTraitsMarketValueOffset : StatPart
	{
		// Token: 0x06007F81 RID: 32641 RVA: 0x002D0E08 File Offset: 0x002CF008
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				CompBladelinkWeapon compBladelinkWeapon = req.Thing.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
					if (!traitsListForReading.NullOrEmpty<WeaponTraitDef>())
					{
						for (int i = 0; i < traitsListForReading.Count; i++)
						{
							val += traitsListForReading[i].marketValueOffset;
						}
					}
				}
			}
		}

		// Token: 0x06007F82 RID: 32642 RVA: 0x002D0E60 File Offset: 0x002CF060
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				CompBladelinkWeapon compBladelinkWeapon = req.Thing.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
					if (!traitsListForReading.NullOrEmpty<WeaponTraitDef>())
					{
						for (int i = 0; i < traitsListForReading.Count; i++)
						{
							if (traitsListForReading[i].marketValueOffset != 0f)
							{
								stringBuilder.AppendLine(traitsListForReading[i].LabelCap + ": " + traitsListForReading[i].marketValueOffset.ToStringByStyle(ToStringStyle.Money, ToStringNumberSense.Offset));
							}
						}
					}
					return stringBuilder.ToString();
				}
			}
			return null;
		}
	}
}
