using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004FB RID: 1275
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x000EFEBD File Offset: 0x000EE0BD
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000EFECC File Offset: 0x000EE0CC
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Pawn pawn = this.currentTarget.Thing as Pawn;
			if (pawn != null && !pawn.Downed && this.CasterIsPawn && this.CasterPawn.skills != null)
			{
				float num = pawn.HostileTo(this.caster) ? 170f : 20f;
				float num2 = this.verbProps.AdjustedFullCycleTime(this, this.CasterPawn);
				this.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false);
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000EFF57 File Offset: 0x000EE157
		protected override bool TryCastShot()
		{
			bool flag = base.TryCastShot();
			if (flag && this.CasterIsPawn)
			{
				this.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
			}
			return flag;
		}
	}
}
