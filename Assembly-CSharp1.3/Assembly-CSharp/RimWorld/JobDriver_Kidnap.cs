using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200071F RID: 1823
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x0012325A File Offset: 0x0012145A
		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x00123267 File Offset: 0x00121467
		public override string GetReport()
		{
			if (this.Takee == null || this.pawn.HostileTo(this.Takee))
			{
				return base.GetReport();
			}
			return JobUtility.GetResolvedJobReport(JobDefOf.Rescue.reportString, this.Takee);
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x001232A5 File Offset: 0x001214A5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.Takee == null || (!this.Takee.Downed && this.Takee.Awake()));
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}
	}
}
