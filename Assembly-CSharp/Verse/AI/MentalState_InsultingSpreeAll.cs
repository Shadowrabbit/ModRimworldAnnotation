using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A23 RID: 2595
	public class MentalState_InsultingSpreeAll : MentalState_InsultingSpree
	{
		// Token: 0x06003DF3 RID: 15859 RVA: 0x0002EA61 File Offset: 0x0002CC61
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x0002EA7B File Offset: 0x0002CC7B
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x0017749C File Offset: 0x0017569C
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

		// Token: 0x06003DF6 RID: 15862 RVA: 0x001774FC File Offset: 0x001756FC
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

		// Token: 0x06003DF7 RID: 15863 RVA: 0x001775DC File Offset: 0x001757DC
		private float GetCandidateWeight(Pawn candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return 1f - num + 0.01f;
		}

		// Token: 0x04002ADF RID: 10975
		private int targetFoundTicks;

		// Token: 0x04002AE0 RID: 10976
		private const int CheckChooseNewTargetIntervalTicks = 250;

		// Token: 0x04002AE1 RID: 10977
		private const int MaxSameTargetChaseTicks = 1250;

		// Token: 0x04002AE2 RID: 10978
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
