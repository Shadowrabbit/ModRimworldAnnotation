using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8A RID: 3466
	public class DeathActionWorker_BigExplosion : DeathActionWorker
	{
		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x0600506B RID: 20587 RVA: 0x001AE0B7 File Offset: 0x001AC2B7
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x0600506C RID: 20588 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x001AE0C0 File Offset: 0x001AC2C0
		public override void PawnDied(Corpse corpse)
		{
			float radius;
			if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
			{
				radius = 1.9f;
			}
			else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
			{
				radius = 2.9f;
			}
			else
			{
				radius = 4.9f;
			}
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}
	}
}
