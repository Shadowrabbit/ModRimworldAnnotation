using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A3A RID: 2618
	public abstract class MentalState_TantrumRandom : MentalState_Tantrum
	{
		// Token: 0x06003E67 RID: 15975
		protected abstract void GetPotentialTargets(List<Thing> outThings);

		// Token: 0x06003E68 RID: 15976 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual Predicate<Thing> GetCustomValidator()
		{
			return null;
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x0002EEE4 File Offset: 0x0002D0E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x0002EEFE File Offset: 0x0002D0FE
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x00178484 File Offset: 0x00176684
		public override void MentalStateTick()
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) || (this.target is Pawn && ((Pawn)this.target).Downed)))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(500) && (this.target == null || this.hitTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick();
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x00178518 File Offset: 0x00176718
		private void ChooseNextTarget()
		{
			MentalState_TantrumRandom.candidates.Clear();
			this.GetPotentialTargets(MentalState_TantrumRandom.candidates);
			if (!MentalState_TantrumRandom.candidates.Any<Thing>())
			{
				this.target = null;
				this.hitTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
			}
			else
			{
				Thing thing;
				if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_TantrumRandom.candidates.Any((Thing x) => x != this.target))
				{
					thing = (from x in MentalState_TantrumRandom.candidates
					where x != this.target
					select x).RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				else
				{
					thing = MentalState_TantrumRandom.candidates.RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				if (thing != this.target)
				{
					this.target = thing;
					this.hitTargetAtLeastOnce = false;
					this.targetFoundTicks = Find.TickManager.TicksGame;
				}
			}
			MentalState_TantrumRandom.candidates.Clear();
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x0017860C File Offset: 0x0017680C
		private float GetCandidateWeight(Thing candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return (1f - num) * (1f - num) + 0.01f;
		}

		// Token: 0x04002AFE RID: 11006
		private int targetFoundTicks;

		// Token: 0x04002AFF RID: 11007
		private const int CheckChooseNewTargetIntervalTicks = 500;

		// Token: 0x04002B00 RID: 11008
		private const int MaxSameTargetAttackTicks = 1250;

		// Token: 0x04002B01 RID: 11009
		private static List<Thing> candidates = new List<Thing>();
	}
}
