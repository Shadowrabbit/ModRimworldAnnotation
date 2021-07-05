using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F5 RID: 5877
	public class PowerBeam : OrbitalStrike
	{
		// Token: 0x0600812C RID: 33068 RVA: 0x00056BCD File Offset: 0x00054DCD
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakePowerBeamMote(base.Position, base.Map);
		}

		// Token: 0x0600812D RID: 33069 RVA: 0x00264F24 File Offset: 0x00263124
		public override void Tick()
		{
			base.Tick();
			if (base.Destroyed)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				this.StartRandomFireAndDoFlameDamage();
			}
		}

		// Token: 0x0600812E RID: 33070 RVA: 0x00264F54 File Offset: 0x00263154
		private void StartRandomFireAndDoFlameDamage()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 15f, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => 1f - Mathf.Min(x.DistanceTo(base.Position) / 15f, 1f) + 0.05f);
			FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
			PowerBeam.tmpThings.Clear();
			PowerBeam.tmpThings.AddRange(c.GetThingList(base.Map));
			for (int i = 0; i < PowerBeam.tmpThings.Count; i++)
			{
				int num = (PowerBeam.tmpThings[i] is Corpse) ? PowerBeam.CorpseFlameDamageAmountRange.RandomInRange : PowerBeam.FlameDamageAmountRange.RandomInRange;
				Pawn pawn = PowerBeam.tmpThings[i] as Pawn;
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				if (pawn != null)
				{
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_PowerBeam, this.instigator as Pawn);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
				}
				PowerBeam.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator, null, this.weaponDef, DamageInfo.SourceCategory.ThingOrUnknown, null)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			PowerBeam.tmpThings.Clear();
		}

		// Token: 0x040053B0 RID: 21424
		public const float Radius = 15f;

		// Token: 0x040053B1 RID: 21425
		private const int FiresStartedPerTick = 4;

		// Token: 0x040053B2 RID: 21426
		private static readonly IntRange FlameDamageAmountRange = new IntRange(65, 100);

		// Token: 0x040053B3 RID: 21427
		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(5, 10);

		// Token: 0x040053B4 RID: 21428
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
