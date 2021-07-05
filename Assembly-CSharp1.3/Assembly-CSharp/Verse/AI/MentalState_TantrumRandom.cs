using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020005F5 RID: 1525
	public abstract class MentalState_TantrumRandom : MentalState_Tantrum
	{
		// Token: 0x06002BD8 RID: 11224
		protected abstract void GetPotentialTargets(List<Thing> outThings);

		// Token: 0x06002BD9 RID: 11225 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual Predicate<Thing> GetCustomValidator()
		{
			return null;
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x00104B46 File Offset: 0x00102D46
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x00104B60 File Offset: 0x00102D60
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x00104B70 File Offset: 0x00102D70
		public override void MentalStateTick()
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) || (this.target is Pawn && ((Pawn)this.target).Downed)))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(500) && (this.target == null || this.hitTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick();
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x00104C04 File Offset: 0x00102E04
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

		// Token: 0x06002BDE RID: 11230 RVA: 0x00104CF8 File Offset: 0x00102EF8
		private float GetCandidateWeight(Thing candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return (1f - num) * (1f - num) + 0.01f;
		}

		// Token: 0x04001AA4 RID: 6820
		private int targetFoundTicks;

		// Token: 0x04001AA5 RID: 6821
		private const int CheckChooseNewTargetIntervalTicks = 500;

		// Token: 0x04001AA6 RID: 6822
		private const int MaxSameTargetAttackTicks = 1250;

		// Token: 0x04001AA7 RID: 6823
		private static List<Thing> candidates = new List<Thing>();
	}
}
