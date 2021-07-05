using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B1 RID: 5041
	public class DeathActionWorker_BigExplosion : DeathActionWorker
	{
		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06006D5F RID: 27999 RVA: 0x0004A5A0 File Offset: 0x000487A0
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x06006D60 RID: 28000 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006D61 RID: 28001 RVA: 0x00218744 File Offset: 0x00216944
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
