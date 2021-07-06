using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D83 RID: 7555
	public class Verb_BeatFire : Verb
	{
		// Token: 0x0600A432 RID: 42034 RVA: 0x0006CDD4 File Offset: 0x0006AFD4
		public Verb_BeatFire()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
		}

		// Token: 0x0600A433 RID: 42035 RVA: 0x002FCDE4 File Offset: 0x002FAFE4
		protected override bool TryCastShot()
		{
			Fire fire = (Fire)this.currentTarget.Thing;
			Pawn casterPawn = this.CasterPawn;
			if (casterPawn.stances.FullBodyBusy || fire.TicksSinceSpawn == 0)
			{
				return false;
			}
			fire.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 32f, 0f, -1f, this.caster, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			casterPawn.Drawer.Notify_MeleeAttackOn(fire);
			return true;
		}

		// Token: 0x04006F57 RID: 28503
		private const int DamageAmount = 32;
	}
}
