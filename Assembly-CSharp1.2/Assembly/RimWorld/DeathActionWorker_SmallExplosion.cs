using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B2 RID: 5042
	public class DeathActionWorker_SmallExplosion : DeathActionWorker
	{
		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06006D63 RID: 28003 RVA: 0x0004A5A0 File Offset: 0x000487A0
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06006D64 RID: 28004 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006D65 RID: 28005 RVA: 0x002187D4 File Offset: 0x002169D4
		public override void PawnDied(Corpse corpse)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}
	}
}
