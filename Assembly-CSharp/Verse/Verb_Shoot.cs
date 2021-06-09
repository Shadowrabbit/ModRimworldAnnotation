using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020008AC RID: 2220
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x0600373B RID: 14139 RVA: 0x0002ACBB File Offset: 0x00028EBB
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x0015FF38 File Offset: 0x0015E138
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

		// Token: 0x0600373D RID: 14141 RVA: 0x0002ACC8 File Offset: 0x00028EC8
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
