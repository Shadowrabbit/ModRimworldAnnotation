using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001052 RID: 4178
	public class Building_TrapDamager : Building_Trap
	{
		// Token: 0x060062D6 RID: 25302 RVA: 0x00217C8F File Offset: 0x00215E8F
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				SoundDefOf.TrapArm.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
		}

		// Token: 0x060062D7 RID: 25303 RVA: 0x00217CB8 File Offset: 0x00215EB8
		protected override void SpringSub(Pawn p)
		{
			SoundDefOf.TrapSpring.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			if (p == null)
			{
				return;
			}
			float num = this.GetStatValue(StatDefOf.TrapMeleeDamage, true) * Building_TrapDamager.DamageRandomFactorRange.RandomInRange / Building_TrapDamager.DamageCount;
			float armorPenetration = num * 0.015f;
			int num2 = 0;
			while ((float)num2 < Building_TrapDamager.DamageCount)
			{
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Stab, num, armorPenetration, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				DamageWorker.DamageResult damageResult = p.TakeDamage(dinfo);
				if (num2 == 0)
				{
					BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(p, RulePackDefOf.DamageEvent_TrapSpike, null);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
					damageResult.AssociateWithLog(battleLogEntry_DamageTaken);
				}
				num2++;
			}
		}

		// Token: 0x04003822 RID: 14370
		private static readonly FloatRange DamageRandomFactorRange = new FloatRange(0.8f, 1.2f);

		// Token: 0x04003823 RID: 14371
		private static readonly float DamageCount = 5f;
	}
}
