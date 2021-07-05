using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CA1 RID: 3233
	public static class TrashUtility
	{
		// Token: 0x06004B40 RID: 19264 RVA: 0x001A4BB0 File Offset: 0x001A2DB0
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

		// Token: 0x06004B41 RID: 19265 RVA: 0x001A4C94 File Offset: 0x001A2E94
		public static bool ShouldTrashBuilding(Pawn pawn, Building b, bool attackAllInert = false)
		{
			if (!b.def.useHitPoints || (b.def.building != null && b.def.building.ai_neverTrashThis))
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
			if (b.def.building.isTrap)
			{
				return false;
			}
			CompCanBeDormant comp = b.GetComp<CompCanBeDormant>();
			return (comp == null || comp.Awake) && b.Faction != Faction.OfMechanoids && TrashUtility.CanTrash(pawn, b) && pawn.HostileTo(b);
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x00035B11 File Offset: 0x00033D11
		private static bool CanTrash(Pawn pawn, Thing t)
		{
			return pawn.CanReach(t, PathEndMode.Touch, Danger.Some, false, TraverseMode.ByPawn) && !t.IsBurning();
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x00035B30 File Offset: 0x00033D30
		public static Job TrashJob(Pawn pawn, Thing t, bool allowPunchingInert = false)
		{
			return TrashUtility.TrashJob_NewTemp(pawn, t, allowPunchingInert, false);
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x001A4DA0 File Offset: 0x001A2FA0
		public static Job TrashJob_NewTemp(Pawn pawn, Thing t, bool allowPunchingInert = false, bool killIncappedTarget = false)
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

		// Token: 0x06004B45 RID: 19269 RVA: 0x001A4EF4 File Offset: 0x001A30F4
		private static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = TrashUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}

		// Token: 0x040031C7 RID: 12743
		private const float ChanceHateInertBuilding = 0.008f;

		// Token: 0x040031C8 RID: 12744
		private static readonly IntRange TrashJobCheckOverrideInterval = new IntRange(450, 500);
	}
}
