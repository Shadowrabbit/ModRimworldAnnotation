using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000785 RID: 1925
	public static class TrashUtility
	{
		// Token: 0x060034E6 RID: 13542 RVA: 0x0012B8CC File Offset: 0x00129ACC
		public static bool ShouldTrashPlant(Pawn pawn, Plant p)
		{
			if (!p.sown || p.def.plant.IsTree || !p.FlammableNow || !TrashUtility.CanTrash(pawn, p))
			{
				return false;
			}
			foreach (IntVec3 c in CellRect.CenteredOn(p.Position, 2).ClipInsideMap(p.Map))
			{
				if (c.InBounds(p.Map) && c.ContainsStaticFire(p.Map))
				{
					return false;
				}
			}
			return p.Position.Roofed(p.Map) || p.Map.weatherManager.RainRate <= 0.25f;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x0012B9B0 File Offset: 0x00129BB0
		public static bool ShouldTrashBuilding(Pawn pawn, Building b, bool attackAllInert = false)
		{
			if (!TrashUtility.ShouldTrashBuilding(b))
			{
				return false;
			}
			if (pawn.mindState.spawnedByInfestationThingComp && b.GetComp<CompCreatesInfestations>() != null)
			{
				return false;
			}
			if (((b.def.building.isInert || b.def.IsFrame) && !attackAllInert) || b.def.building.isTrap)
			{
				int num = GenLocalDate.HourOfDay(pawn) / 3;
				int specialSeed = b.GetHashCode() * 612361 ^ pawn.GetHashCode() * 391 ^ num * 73427324;
				if (!Rand.ChanceSeeded(0.008f, specialSeed))
				{
					return false;
				}
			}
			return TrashUtility.CanTrash(pawn, b) && pawn.HostileTo(b);
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x0012BA64 File Offset: 0x00129C64
		public static bool ShouldTrashBuilding(Building b)
		{
			if (((b != null) ? b.def.building : null) == null)
			{
				return false;
			}
			if (!b.def.useHitPoints || b.def.building.ai_neverTrashThis)
			{
				return false;
			}
			if (b.def.building.isTrap)
			{
				return false;
			}
			CompCanBeDormant comp = b.GetComp<CompCanBeDormant>();
			return (comp == null || comp.Awake) && (b.Faction == null || b.Faction != Faction.OfMechanoids);
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x0012BAE7 File Offset: 0x00129CE7
		private static bool CanTrash(Pawn pawn, Thing t)
		{
			return pawn.CanReach(t, PathEndMode.Touch, Danger.Some, false, false, TraverseMode.ByPawn) && !t.IsBurning();
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x0012BB08 File Offset: 0x00129D08
		public static Job TrashJob(Pawn pawn, Thing t, bool allowPunchingInert = false, bool killIncappedTarget = false)
		{
			if (t is Plant)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Ignite, t);
				TrashUtility.FinalizeTrashJob(job);
				return job;
			}
			if (pawn.equipment != null && Rand.Value < 0.7f)
			{
				foreach (Verb verb in pawn.equipment.AllEquipmentVerbs)
				{
					if (verb.verbProps.ai_IsBuildingDestroyer)
					{
						Job job2 = JobMaker.MakeJob(JobDefOf.UseVerbOnThing, t);
						job2.verbToUse = verb;
						TrashUtility.FinalizeTrashJob(job2);
						return job2;
					}
				}
			}
			Job job3;
			if (Rand.Value < 0.35f && pawn.natives.IgniteVerb != null && pawn.natives.IgniteVerb.IsStillUsableBy(pawn) && t.FlammableNow && !t.IsBurning() && !(t is Building_Door))
			{
				job3 = JobMaker.MakeJob(JobDefOf.Ignite, t);
			}
			else
			{
				Building building = t as Building;
				if (building != null && building.def.building.isInert && !allowPunchingInert)
				{
					return null;
				}
				job3 = JobMaker.MakeJob(JobDefOf.AttackMelee, t);
			}
			job3.killIncappedTarget = killIncappedTarget;
			TrashUtility.FinalizeTrashJob(job3);
			return job3;
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x0012BC5C File Offset: 0x00129E5C
		private static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = TrashUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}

		// Token: 0x04001E72 RID: 7794
		private const float ChanceHateInertBuilding = 0.008f;

		// Token: 0x04001E73 RID: 7795
		private static readonly IntRange TrashJobCheckOverrideInterval = new IntRange(450, 500);
	}
}
