using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAD RID: 7341
	public static class EquipmentUtility
	{
		// Token: 0x06009FB0 RID: 40880 RVA: 0x002EA9C8 File Offset: 0x002E8BC8
		public static bool CanEquip(Thing thing, Pawn pawn)
		{
			string text;
			return EquipmentUtility.CanEquip_NewTmp(thing, pawn, out text, true);
		}

		// Token: 0x06009FB1 RID: 40881 RVA: 0x0006A80D File Offset: 0x00068A0D
		[Obsolete("Only used for mod compatibility")]
		public static bool CanEquip(Thing thing, Pawn pawn, out string cantReason)
		{
			return EquipmentUtility.CanEquip_NewTmp(thing, pawn, out cantReason, true);
		}

		// Token: 0x06009FB2 RID: 40882 RVA: 0x002EA9E0 File Offset: 0x002E8BE0
		public static bool CanEquip_NewTmp(Thing thing, Pawn pawn, out string cantReason, bool checkBonded = true)
		{
			cantReason = null;
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null && compBladelinkWeapon.Bondable && compBladelinkWeapon.bondedPawn != null && compBladelinkWeapon.bondedPawn != pawn)
			{
				cantReason = "BladelinkBondedToSomeoneElse".Translate();
				return false;
			}
			if (EquipmentUtility.IsBiocoded(thing) && !EquipmentUtility.IsBiocodedFor(thing, pawn))
			{
				cantReason = "BiocodedCodedForSomeoneElse".Translate();
				return false;
			}
			if (checkBonded && EquipmentUtility.AlreadyBondedToWeapon(thing, pawn))
			{
				cantReason = "BladelinkAlreadyBondedMessage".Translate(pawn.Named("PAWN"), pawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
				return false;
			}
			return true;
		}

		// Token: 0x06009FB3 RID: 40883 RVA: 0x002EAA8C File Offset: 0x002E8C8C
		public static bool AlreadyBondedToWeapon(Thing thing, Pawn pawn)
		{
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			return compBladelinkWeapon != null && compBladelinkWeapon.Bondable && pawn.equipment.bondedWeapon != null && pawn.equipment.bondedWeapon != thing;
		}

		// Token: 0x06009FB4 RID: 40884 RVA: 0x002EAAD0 File Offset: 0x002E8CD0
		public static string GetPersonaWeaponConfirmationText(Thing item, Pawn p)
		{
			CompBladelinkWeapon compBladelinkWeapon = item.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null && compBladelinkWeapon.Bondable && compBladelinkWeapon.bondedPawn != p)
			{
				TaggedString taggedString = "BladelinkEquipWarning".Translate();
				List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
				if (!traitsListForReading.NullOrEmpty<WeaponTraitDef>())
				{
					taggedString += "\n\n" + "BladelinkEquipWarningTraits".Translate() + ":";
					for (int i = 0; i < traitsListForReading.Count; i++)
					{
						taggedString += "\n\n" + traitsListForReading[i].LabelCap + ": " + traitsListForReading[i].description;
					}
				}
				taggedString += "\n\n" + "RoyalWeaponEquipConfirmation".Translate();
				return taggedString;
			}
			return null;
		}

		// Token: 0x06009FB5 RID: 40885 RVA: 0x002EABAC File Offset: 0x002E8DAC
		public static bool IsBiocoded(Thing thing)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.Biocoded;
		}

		// Token: 0x06009FB6 RID: 40886 RVA: 0x002EABCC File Offset: 0x002E8DCC
		public static bool IsBiocodedFor(Thing thing, Pawn pawn)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.CodedPawn == pawn;
		}

		// Token: 0x06009FB7 RID: 40887 RVA: 0x002EABF0 File Offset: 0x002E8DF0
		public static bool IsBondedTo(Thing thing, Pawn pawn)
		{
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			return compBladelinkWeapon != null && compBladelinkWeapon.bondedPawn == pawn;
		}

		// Token: 0x06009FB8 RID: 40888 RVA: 0x002EAC14 File Offset: 0x002E8E14
		public static bool QuestLodgerCanEquip(Thing thing, Pawn pawn)
		{
			return (pawn.equipment.Primary == null || EquipmentUtility.QuestLodgerCanUnequip(pawn.equipment.Primary, pawn)) && (EquipmentUtility.IsBiocodedFor(thing, pawn) || EquipmentUtility.AlreadyBondedToWeapon(thing, pawn) || thing.def.IsWeapon);
		}

		// Token: 0x06009FB9 RID: 40889 RVA: 0x0006A818 File Offset: 0x00068A18
		public static bool QuestLodgerCanUnequip(Thing thing, Pawn pawn)
		{
			return !EquipmentUtility.IsBiocodedFor(thing, pawn) && !EquipmentUtility.IsBondedTo(thing, pawn);
		}
	}
}
