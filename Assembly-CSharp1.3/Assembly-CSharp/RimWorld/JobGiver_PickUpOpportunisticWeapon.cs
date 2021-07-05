using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000791 RID: 1937
	public class JobGiver_PickUpOpportunisticWeapon : ThinkNode_JobGiver
	{
		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06003513 RID: 13587 RVA: 0x0012C5B4 File Offset: 0x0012A7B4
		private float MinMeleeWeaponDPSThreshold
		{
			get
			{
				List<Tool> tools = ThingDefOf.Human.tools;
				float num = 0f;
				for (int i = 0; i < tools.Count; i++)
				{
					if (tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.LeftHand || tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.RightHand)
					{
						num = tools[i].power / tools[i].cooldownTime;
						break;
					}
				}
				return num + 2f;
			}
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x0012C62C File Offset: 0x0012A82C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PickUpOpportunisticWeapon jobGiver_PickUpOpportunisticWeapon = (JobGiver_PickUpOpportunisticWeapon)base.DeepCopy(resolve);
			jobGiver_PickUpOpportunisticWeapon.preferBuildingDestroyers = this.preferBuildingDestroyers;
			jobGiver_PickUpOpportunisticWeapon.pickUpUtilityItems = this.pickUpUtilityItems;
			return jobGiver_PickUpOpportunisticWeapon;
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x0012C654 File Offset: 0x0012A854
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment == null && pawn.apparel == null)
			{
				return null;
			}
			if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			if (pawn.GetRegion(RegionType.Set_Passable) == null)
			{
				return null;
			}
			if (pawn.equipment != null && !this.AlreadySatisfiedWithCurrentWeapon(pawn))
			{
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Weapon), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 8f, (Thing x) => pawn.CanReserve(x, 1, -1, null, false) && !x.IsBurning() && this.ShouldEquipWeapon(x, pawn), null, 0, 15, false, RegionType.Set_Passable, false);
				if (thing != null)
				{
					return JobMaker.MakeJob(JobDefOf.Equip, thing);
				}
			}
			if (this.pickUpUtilityItems && pawn.apparel != null && this.WouldPickupUtilityItem(pawn))
			{
				Thing thing2 = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Apparel), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 8f, (Thing x) => pawn.CanReserve(x, 1, -1, null, false) && !x.IsBurning() && this.ShouldEquipUtilityItem(x, pawn), null, 0, 15, false, RegionType.Set_Passable, false);
				if (thing2 != null)
				{
					return JobMaker.MakeJob(JobDefOf.Wear, thing2);
				}
			}
			return null;
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x0012C7E4 File Offset: 0x0012A9E4
		private bool AlreadySatisfiedWithCurrentWeapon(Pawn pawn)
		{
			ThingWithComps primary = pawn.equipment.Primary;
			if (primary == null)
			{
				return false;
			}
			if (this.preferBuildingDestroyers)
			{
				if (!pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
				{
					return false;
				}
			}
			else if (!primary.def.IsRangedWeapon)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x0012C838 File Offset: 0x0012AA38
		private bool ShouldEquipWeapon(Thing newWep, Pawn pawn)
		{
			return (!newWep.def.IsRangedWeapon || !pawn.WorkTagIsDisabled(WorkTags.Shooting)) && EquipmentUtility.CanEquip(newWep, pawn) && this.GetWeaponScore(newWep) > this.GetWeaponScore(pawn.equipment.Primary);
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x0012C888 File Offset: 0x0012AA88
		private int GetWeaponScore(Thing wep)
		{
			if (wep == null)
			{
				return 0;
			}
			if (wep.def.IsMeleeWeapon && wep.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, true) < this.MinMeleeWeaponDPSThreshold)
			{
				return 0;
			}
			if (this.preferBuildingDestroyers && wep.TryGetComp<CompEquippable>().PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
			{
				return 3;
			}
			if (wep.def.IsRangedWeapon)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x0012C8EE File Offset: 0x0012AAEE
		private bool WouldPickupUtilityItem(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			return ((equipment != null) ? equipment.Primary : null) == null && pawn.apparel.FirstApparelVerb == null;
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x0012C918 File Offset: 0x0012AB18
		private bool ShouldEquipUtilityItem(Thing thing, Pawn pawn)
		{
			Apparel apparel;
			return (apparel = (thing as Apparel)) != null && apparel.def.apparel.ai_pickUpOpportunistically && (EquipmentUtility.CanEquip(apparel, pawn) && ApparelUtility.HasPartsToWear(pawn, apparel.def)) && !pawn.apparel.WouldReplaceLockedApparel(apparel);
		}

		// Token: 0x04001E77 RID: 7799
		private bool preferBuildingDestroyers;

		// Token: 0x04001E78 RID: 7800
		private bool pickUpUtilityItems;
	}
}
