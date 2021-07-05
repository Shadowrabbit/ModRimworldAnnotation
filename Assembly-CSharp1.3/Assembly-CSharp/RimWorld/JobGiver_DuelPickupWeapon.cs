using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000799 RID: 1945
	public class JobGiver_DuelPickupWeapon : ThinkNode_JobGiver
	{
		// Token: 0x0600352F RID: 13615 RVA: 0x0012CEF7 File Offset: 0x0012B0F7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DuelPickupWeapon jobGiver_DuelPickupWeapon = (JobGiver_DuelPickupWeapon)base.DeepCopy(resolve);
			jobGiver_DuelPickupWeapon.scanRadius = this.scanRadius;
			return jobGiver_DuelPickupWeapon;
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x0012CF14 File Offset: 0x0012B114
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment.AllEquipmentListForReading.Any((ThingWithComps e) => e.def.IsMeleeWeapon))
			{
				return null;
			}
			LordJob_Ritual_Duel lordJob_Ritual_Duel = (LordJob_Ritual_Duel)pawn.GetLord().LordJob;
			Pawn pawn2 = lordJob_Ritual_Duel.Opponent(pawn);
			TargetInfo selectedTarget = lordJob_Ritual_Duel.selectedTarget;
			Thing thing = null;
			float num = float.PositiveInfinity;
			for (int i = 0; i < GenRadial.NumCellsInRadius((float)this.scanRadius); i++)
			{
				IntVec3 c = selectedTarget.Cell + GenRadial.RadialPattern[i];
				if (c.InBounds(selectedTarget.Map))
				{
					foreach (Thing thing2 in c.GetThingList(selectedTarget.Map))
					{
						float num2 = pawn.Position.DistanceTo(thing2.Position);
						if (thing2.def.IsMeleeWeapon && pawn.CanReserveAndReach(thing2, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false) && num2 < pawn.Position.DistanceTo(pawn2.Position) && num2 < pawn2.Position.DistanceTo(thing2.Position) && num > num2)
						{
							thing = thing2;
							num = num2;
						}
					}
				}
			}
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.Equip, thing);
			}
			return null;
		}

		// Token: 0x04001E7D RID: 7805
		public int scanRadius = 3;
	}
}
