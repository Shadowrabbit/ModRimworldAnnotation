using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200165E RID: 5726
	public static class TaleUtility
	{
		// Token: 0x06007CB6 RID: 31926 RVA: 0x0025481C File Offset: 0x00252A1C
		public static void Notify_PawnDied(Pawn victim, DamageInfo? dinfo)
		{
			if (Current.ProgramState == ProgramState.Playing && dinfo != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null && pawn.CurJob != null && pawn.jobs.curDriver is JobDriver_Execute)
				{
					return;
				}
				bool flag = !victim.RaceProps.Humanlike && dinfo.Value.Instigator != null && dinfo.Value.Instigator.Spawned && dinfo.Value.Instigator is Pawn && ((Pawn)dinfo.Value.Instigator).jobs.curDriver is JobDriver_Slaughter;
				if (pawn != null)
				{
					if (victim.IsColonist)
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledColonist, new object[]
						{
							pawn,
							victim
						});
					}
					else if (victim.Faction == Faction.OfPlayer && victim.RaceProps.Animal && !flag)
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledColonyAnimal, new object[]
						{
							pawn,
							victim
						});
					}
				}
				if ((victim.Faction == Faction.OfPlayer || (pawn != null && pawn.Faction == Faction.OfPlayer)) && !flag)
				{
					TaleRecorder.RecordTale(TaleDefOf.KilledBy, new object[]
					{
						victim,
						dinfo.Value
					});
				}
				if (pawn != null)
				{
					if (dinfo.Value.Weapon != null && dinfo.Value.Weapon.building != null && dinfo.Value.Weapon.building.IsMortar)
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledMortar, new object[]
						{
							pawn,
							victim,
							dinfo.Value.Weapon
						});
					}
					else if (pawn != null && pawn.Position.DistanceTo(victim.Position) >= 35f)
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledLongRange, new object[]
						{
							pawn,
							victim,
							dinfo.Value.Weapon
						});
					}
					else if (dinfo.Value.Weapon != null && dinfo.Value.Weapon.IsMeleeWeapon)
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledMelee, new object[]
						{
							pawn,
							victim,
							dinfo.Value.Weapon
						});
					}
					bool flag2 = false;
					if (victim.Faction != null && victim.Faction.leader == victim && victim.Faction != pawn.Faction && victim.Faction.HostileTo(pawn.Faction))
					{
						flag2 = true;
						TaleRecorder.RecordTale(TaleDefOf.DefeatedHostileFactionLeader, new object[]
						{
							pawn,
							victim
						});
					}
					if (victim.HostileTo(pawn) && Rand.Chance(TaleUtility.MajorThreatCurve.Evaluate(victim.kindDef.combatPower)))
					{
						flag2 = true;
					}
					if (flag2)
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledMajorThreat, new object[]
						{
							pawn,
							victim,
							dinfo.Value.Weapon
						});
					}
					PawnCapacityDef pawnCapacityDef = victim.health.ShouldBeDeadFromRequiredCapacity();
					if (pawnCapacityDef != null)
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledCapacity, new object[]
						{
							pawn,
							victim,
							pawnCapacityDef
						});
					}
				}
			}
		}

		// Token: 0x04005186 RID: 20870
		private const float KilledTaleLongRangeThreshold = 35f;

		// Token: 0x04005187 RID: 20871
		private const float KilledTaleMeleeRangeThreshold = 2f;

		// Token: 0x04005188 RID: 20872
		private const float MajorEnemyThreshold = 250f;

		// Token: 0x04005189 RID: 20873
		private static readonly SimpleCurve MajorThreatCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 0f),
				true
			},
			{
				new CurvePoint(400f, 1f),
				true
			}
		};
	}
}
