using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001096 RID: 4246
	public class PowerBeam : OrbitalStrike
	{
		// Token: 0x06006544 RID: 25924 RVA: 0x00223654 File Offset: 0x00221854
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakePowerBeamMote(base.Position, base.Map);
		}

		// Token: 0x06006545 RID: 25925 RVA: 0x00223670 File Offset: 0x00221870
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

		// Token: 0x06006546 RID: 25926 RVA: 0x002236A0 File Offset: 0x002218A0
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
				PowerBeam.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator, null, this.weaponDef, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			PowerBeam.tmpThings.Clear();
		}

		// Token: 0x04003901 RID: 14593
		public const float Radius = 15f;

		// Token: 0x04003902 RID: 14594
		private const int FiresStartedPerTick = 4;

		// Token: 0x04003903 RID: 14595
		private static readonly IntRange FlameDamageAmountRange = new IntRange(65, 100);

		// Token: 0x04003904 RID: 14596
		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(5, 10);

		// Token: 0x04003905 RID: 14597
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
