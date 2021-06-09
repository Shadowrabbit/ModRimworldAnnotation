using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BD3 RID: 3027
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06004732 RID: 18226 RVA: 0x00033DF3 File Offset: 0x00031FF3
		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x00033E00 File Offset: 0x00032000
		public override string GetReport()
		{
			if (this.Takee == null || this.pawn.HostileTo(this.Takee))
			{
				return base.GetReport();
			}
			return JobUtility.GetResolvedJobReport(JobDefOf.Rescue.reportString, this.Takee);
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x00033E3E File Offset: 0x0003203E
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
