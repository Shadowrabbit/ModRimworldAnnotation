using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000968 RID: 2408
	public class ThoughtWorker_Precept_WieldingNobleOrDespisedWeapon : ThoughtWorker_Precept
	{
		// Token: 0x06003D42 RID: 15682 RVA: 0x00151A60 File Offset: 0x0014FC60
		public override string PostProcessLabel(Pawn p, string label)
		{
			Pawn_EquipmentTracker equipment = p.equipment;
			ThingWithComps thingWithComps = (equipment != null) ? equipment.Primary : null;
			if (thingWithComps == null)
			{
				return label;
			}
			return label.Formatted(thingWithComps.Named("WEAPON"));
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x00151A9C File Offset: 0x0014FC9C
		public override string PostProcessDescription(Pawn p, string description)
		{
			Pawn_EquipmentTracker equipment = p.equipment;
			ThingWithComps thingWithComps = (equipment != null) ? equipment.Primary : null;
			if (thingWithComps == null)
			{
				return description;
			}
			return description.Formatted(thingWithComps.Named("WEAPON"));
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x00151AD8 File Offset: 0x0014FCD8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Ideo != null)
			{
				Pawn_EquipmentTracker equipment = p.equipment;
				if (((equipment != null) ? equipment.Primary : null) != null)
				{
					IdeoWeaponDisposition dispositionForWeapon = p.Ideo.GetDispositionForWeapon(p.equipment.Primary.def);
					if (dispositionForWeapon != IdeoWeaponDisposition.None)
					{
						if (dispositionForWeapon == IdeoWeaponDisposition.Noble)
						{
							return ThoughtState.ActiveAtStage(0);
						}
						if (dispositionForWeapon == IdeoWeaponDisposition.Despised)
						{
							return ThoughtState.ActiveAtStage(1);
						}
					}
					return ThoughtState.Inactive;
				}
			}
			return false;
		}
	}
}
