using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200167E RID: 5758
	public class Building_TrapDamager : Building_Trap
	{
		// Token: 0x06007DA6 RID: 32166 RVA: 0x00054740 File Offset: 0x00052940
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				SoundDefOf.TrapArm.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
		}

		// Token: 0x06007DA7 RID: 32167 RVA: 0x00257B5C File Offset: 0x00255D5C
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
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Stab, num, armorPenetration, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
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

		// Token: 0x040051EF RID: 20975
		private static readonly FloatRange DamageRandomFactorRange = new FloatRange(0.8f, 1.2f);

		// Token: 0x040051F0 RID: 20976
		private static readonly float DamageCount = 5f;
	}
}
