using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D85 RID: 3461
	public class WorkGiver_HunterHunt : WorkGiver_Scanner
	{
		// Token: 0x06004EEA RID: 20202 RVA: 0x00037938 File Offset: 0x00035B38
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Hunt))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06004EEB RID: 20203 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x00037948 File Offset: 0x00035B48
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !WorkGiver_HunterHunt.HasHuntingWeapon(pawn) || WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn) || !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Hunt);
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x001B36E4 File Offset: 0x001B18E4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2.AnimalOrWildMan() && pawn.CanReserve(t, 1, -1, null, forced) && pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Hunt) != null;
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x00037978 File Offset: 0x00035B78
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Hunt, t);
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x001B3734 File Offset: 0x001B1934
		public static bool HasHuntingWeapon(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon && p.equipment.PrimaryEq.PrimaryVerb.HarmsHealth() && !p.equipment.PrimaryEq.PrimaryVerb.UsesExplosiveProjectiles();
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x001B3798 File Offset: 0x001B1998
		public static bool HasShieldAndRangedWeapon(Pawn p)
		{
			if (p.equipment.Primary != null && p.equipment.Primary.def.IsWeaponUsingProjectiles)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i] is ShieldBelt)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
