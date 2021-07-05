using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150E RID: 5390
	public class Verb_BeatFire : Verb
	{
		// Token: 0x06008063 RID: 32867 RVA: 0x002D7D39 File Offset: 0x002D5F39
		public Verb_BeatFire()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
		}

		// Token: 0x06008064 RID: 32868 RVA: 0x002D7D50 File Offset: 0x002D5F50
		protected override bool TryCastShot()
		{
			Fire fire = (Fire)this.currentTarget.Thing;
			Pawn casterPawn = this.CasterPawn;
			if (casterPawn.stances.FullBodyBusy || fire.TicksSinceSpawn == 0)
			{
				return false;
			}
			fire.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 32f, 0f, -1f, this.caster, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			casterPawn.Drawer.Notify_MeleeAttackOn(fire);
			return true;
		}

		// Token: 0x04004FFA RID: 20474
		private const int DamageAmount = 32;
	}
}
