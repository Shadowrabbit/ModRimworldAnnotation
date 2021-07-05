using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8B RID: 3467
	public class DeathActionWorker_SmallExplosion : DeathActionWorker
	{
		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x0600506F RID: 20591 RVA: 0x001AE0B7 File Offset: 0x001AC2B7
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06005070 RID: 20592 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x001AE150 File Offset: 0x001AC350
		public override void PawnDied(Corpse corpse)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}
	}
}
