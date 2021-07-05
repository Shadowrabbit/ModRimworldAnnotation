using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200147F RID: 5247
	public static class EquipmentUtility
	{
		// Token: 0x06007D73 RID: 32115 RVA: 0x002C4F28 File Offset: 0x002C3128
		public static bool CanEquip(Thing thing, Pawn pawn)
		{
			string text;
			return EquipmentUtility.CanEquip(thing, pawn, out text, true);
		}

		// Token: 0x06007D74 RID: 32116 RVA: 0x002C4F40 File Offset: 0x002C3140
		public static bool CanEquip(Thing thing, Pawn pawn, out string cantReason, bool checkBonded = true)
		{
			cantReason = null;
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null && compBladelinkWeapon.Biocodable && compBladelinkWeapon.CodedPawn != null && compBladelinkWeapon.CodedPawn != pawn)
			{
				cantReason = "BladelinkBondedToSomeoneElse".Translate();
				return false;
			}
			if (CompBiocodable.IsBiocoded(thing) && !CompBiocodable.IsBiocodedFor(thing, pawn))
			{
				cantReason = "BiocodedCodedForSomeoneElse".Translate();
				return false;
			}
			if (checkBonded && EquipmentUtility.AlreadyBondedToWeapon(thing, pawn))
			{
				cantReason = "BladelinkAlreadyBondedMessage".Translate(pawn.Named("PAWN"), pawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
				return false;
			}
			return !EquipmentUtility.RolePreventsFromUsing(pawn, thing, out cantReason);
		}

		// Token: 0x06007D75 RID: 32117 RVA: 0x002C4FF8 File Offset: 0x002C31F8
		public static bool AlreadyBondedToWeapon(Thing thing, Pawn pawn)
		{
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			return compBladelinkWeapon != null && compBladelinkWeapon.Biocodable && pawn.equipment.bondedWeapon != null && pawn.equipment.bondedWeapon != thing;
		}

		// Token: 0x06007D76 RID: 32118 RVA: 0x002C503C File Offset: 0x002C323C
		public static string GetPersonaWeaponConfirmationText(Thing item, Pawn p)
		{
			CompBladelinkWeapon compBladelinkWeapon = item.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null && compBladelinkWeapon.Biocodable && compBladelinkWeapon.CodedPawn != p)
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

		// Token: 0x06007D77 RID: 32119 RVA: 0x002C5118 File Offset: 0x002C3318
		public static bool IsBondedTo(Thing thing, Pawn pawn)
		{
			CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
			return compBladelinkWeapon != null && compBladelinkWeapon.CodedPawn == pawn;
		}

		// Token: 0x06007D78 RID: 32120 RVA: 0x002C513C File Offset: 0x002C333C
		public static bool QuestLodgerCanEquip(Thing thing, Pawn pawn)
		{
			return (pawn.equipment.Primary == null || EquipmentUtility.QuestLodgerCanUnequip(pawn.equipment.Primary, pawn)) && (CompBiocodable.IsBiocodedFor(thing, pawn) || EquipmentUtility.AlreadyBondedToWeapon(thing, pawn) || thing.def.IsWeapon);
		}

		// Token: 0x06007D79 RID: 32121 RVA: 0x002C518C File Offset: 0x002C338C
		public static bool RolePreventsFromUsing(Pawn pawn, Thing thing, out string reason)
		{
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (role != null && !role.CanEquip(pawn, thing, out reason))
				{
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x06007D7A RID: 32122 RVA: 0x002C51C9 File Offset: 0x002C33C9
		public static bool QuestLodgerCanUnequip(Thing thing, Pawn pawn)
		{
			return !CompBiocodable.IsBiocodedFor(thing, pawn) && !EquipmentUtility.IsBondedTo(thing, pawn);
		}

		// Token: 0x06007D7B RID: 32123 RVA: 0x002C51E4 File Offset: 0x002C33E4
		public static Verb_LaunchProjectile GetRecoilVerb(List<Verb> allWeaponVerbs)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = null;
			using (List<Verb>.Enumerator enumerator = allWeaponVerbs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Verb_LaunchProjectile verb_LaunchProjectile2;
					if ((verb_LaunchProjectile2 = (enumerator.Current as Verb_LaunchProjectile)) != null && (verb_LaunchProjectile == null || verb_LaunchProjectile.LastShotTick < verb_LaunchProjectile2.LastShotTick))
					{
						verb_LaunchProjectile = verb_LaunchProjectile2;
					}
				}
			}
			return verb_LaunchProjectile;
		}

		// Token: 0x06007D7C RID: 32124 RVA: 0x002C524C File Offset: 0x002C344C
		public static void Recoil(ThingDef weaponDef, Verb_LaunchProjectile shootVerb, out Vector3 drawOffset, out float angleOffset, float aimAngle)
		{
			drawOffset = Vector3.zero;
			angleOffset = 0f;
			if (weaponDef.recoilPower > 0f && shootVerb != null)
			{
				Rand.PushState(shootVerb.LastShotTick);
				try
				{
					int num = Find.TickManager.TicksGame - shootVerb.LastShotTick;
					if ((float)num < weaponDef.recoilRelaxation)
					{
						float num2 = Mathf.Clamp01((float)num / weaponDef.recoilRelaxation);
						float num3 = Mathf.Lerp(weaponDef.recoilPower, 0f, num2);
						drawOffset = new Vector3((float)Rand.Sign * EquipmentUtility.RecoilCurveAxisX.Evaluate(num2), 0f, -EquipmentUtility.RecoilCurveAxisY.Evaluate(num2)) * num3;
						angleOffset = (float)Rand.Sign * EquipmentUtility.RecoilCurveRotation.Evaluate(num2) * num3;
						aimAngle += angleOffset;
						drawOffset = drawOffset.RotatedBy(aimAngle);
					}
				}
				finally
				{
					Rand.PopState();
				}
			}
		}

		// Token: 0x04004E46 RID: 20038
		private static readonly SimpleCurve RecoilCurveAxisX = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.02f),
				true
			},
			{
				new CurvePoint(2f, 0.03f),
				true
			}
		};

		// Token: 0x04004E47 RID: 20039
		private static readonly SimpleCurve RecoilCurveAxisY = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.05f),
				true
			},
			{
				new CurvePoint(2f, 0.075f),
				true
			}
		};

		// Token: 0x04004E48 RID: 20040
		private static readonly SimpleCurve RecoilCurveRotation = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 3f),
				true
			},
			{
				new CurvePoint(2f, 4f),
				true
			}
		};
	}
}
