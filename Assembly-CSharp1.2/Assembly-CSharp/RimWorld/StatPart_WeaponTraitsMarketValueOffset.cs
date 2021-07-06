using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4E RID: 7502
	public class StatPart_WeaponTraitsMarketValueOffset : StatPart
	{
		// Token: 0x0600A2F6 RID: 41718 RVA: 0x002F6848 File Offset: 0x002F4A48
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

		// Token: 0x0600A2F7 RID: 41719 RVA: 0x002F68A0 File Offset: 0x002F4AA0
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
