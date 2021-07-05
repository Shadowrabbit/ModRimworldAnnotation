using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020005DA RID: 1498
	public class MentalState_InsultingSpreeAll : MentalState_InsultingSpree
	{
		// Token: 0x06002B50 RID: 11088 RVA: 0x00102F22 File Offset: 0x00101122
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x00102F3C File Offset: 0x0010113C
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x00102F4C File Offset: 0x0010114C
		public override void MentalStateTick()
		{
			if (this.target != null && !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, true))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(250) && (this.target == null || this.insultedTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick();
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x00102FAC File Offset: 0x001011AC
		private void ChooseNextTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_InsultingSpreeAll.candidates, true);
			if (!MentalState_InsultingSpreeAll.candidates.Any<Pawn>())
			{
				this.target = null;
				this.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
				return;
			}
			Pawn pawn;
			if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_InsultingSpreeAll.candidates.Any((Pawn x) => x != this.target))
			{
				pawn = (from x in MentalState_InsultingSpreeAll.candidates
				where x != this.target
				select x).RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
			}
			else
			{
				pawn = MentalState_InsultingSpreeAll.candidates.RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
			}
			if (pawn != this.target)
			{
				this.target = pawn;
				this.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x0010308C File Offset: 0x0010128C
		private float GetCandidateWeight(Pawn candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return 1f - num + 0.01f;
		}

		// Token: 0x04001A77 RID: 6775
		private int targetFoundTicks;

		// Token: 0x04001A78 RID: 6776
		private const int CheckChooseNewTargetIntervalTicks = 250;

		// Token: 0x04001A79 RID: 6777
		private const int MaxSameTargetChaseTicks = 1250;

		// Token: 0x04001A7A RID: 6778
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
