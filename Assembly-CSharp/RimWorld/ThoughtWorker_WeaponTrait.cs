using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDC RID: 3804
	public class ThoughtWorker_WeaponTrait : ThoughtWorker
	{
		// Token: 0x0600542F RID: 21551 RVA: 0x0003A74B File Offset: 0x0003894B
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description.Formatted(this.WeaponName(p).Named("WEAPON")));
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x0003A770 File Offset: 0x00038970
		public override string PostProcessLabel(Pawn p, string label)
		{
			return base.PostProcessLabel(p, label.Formatted(this.WeaponName(p).Named("WEAPON")));
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x001C2E94 File Offset: 0x001C1094
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

		// Token: 0x06005432 RID: 21554 RVA: 0x001C2EE0 File Offset: 0x001C10E0
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
