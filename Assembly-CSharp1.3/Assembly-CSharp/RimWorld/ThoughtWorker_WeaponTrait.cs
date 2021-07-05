using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CF RID: 2511
	public class ThoughtWorker_WeaponTrait : ThoughtWorker
	{
		// Token: 0x06003E41 RID: 15937 RVA: 0x00154C7F File Offset: 0x00152E7F
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description.Formatted(this.WeaponName(p).Named("WEAPON")));
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x00154CA4 File Offset: 0x00152EA4
		public override string PostProcessLabel(Pawn p, string label)
		{
			return base.PostProcessLabel(p, label.Formatted(this.WeaponName(p).Named("WEAPON")));
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x00154CCC File Offset: 0x00152ECC
		protected string WeaponName(Pawn pawn)
		{
			if (pawn.equipment.bondedWeapon == null)
			{
				return string.Empty;
			}
			CompGeneratedNames compGeneratedNames = pawn.equipment.bondedWeapon.TryGetComp<CompGeneratedNames>();
			if (compGeneratedNames != null)
			{
				return compGeneratedNames.Name;
			}
			return pawn.equipment.bondedWeapon.LabelNoCount;
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x00154D18 File Offset: 0x00152F18
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Pawn_EquipmentTracker equipment = p.equipment;
			if (((equipment != null) ? equipment.bondedWeapon : null) == null || p.equipment.bondedWeapon.Destroyed)
			{
				return ThoughtState.Inactive;
			}
			CompBladelinkWeapon compBladelinkWeapon = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon == null || compBladelinkWeapon.TraitsListForReading.NullOrEmpty<WeaponTraitDef>())
			{
				return ThoughtState.Inactive;
			}
			return true;
		}
	}
}
