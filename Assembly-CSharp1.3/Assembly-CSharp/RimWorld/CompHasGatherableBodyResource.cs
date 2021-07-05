using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001145 RID: 4421
	public abstract class CompHasGatherableBodyResource : ThingComp
	{
		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06006A22 RID: 27170
		protected abstract int GatherResourcesIntervalDays { get; }

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06006A23 RID: 27171
		protected abstract int ResourceAmount { get; }

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06006A24 RID: 27172
		protected abstract ThingDef ResourceDef { get; }

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06006A25 RID: 27173
		protected abstract string SaveKey { get; }

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06006A26 RID: 27174 RVA: 0x0023B8AD File Offset: 0x00239AAD
		public float Fullness
		{
			get
			{
				return this.fullness;
			}
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06006A27 RID: 27175 RVA: 0x0023B8B5 File Offset: 0x00239AB5
		protected virtual bool Active
		{
			get
			{
				return this.parent.Faction != null && !this.parent.Suspended;
			}
		}

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06006A28 RID: 27176 RVA: 0x0023B8D6 File Offset: 0x00239AD6
		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1f;
			}
		}

		// Token: 0x06006A29 RID: 27177 RVA: 0x0023B8F2 File Offset: 0x00239AF2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.SaveKey, 0f, false);
		}

		// Token: 0x06006A2A RID: 27178 RVA: 0x0023B914 File Offset: 0x00239B14
		public override void CompTick()
		{
			if (this.Active)
			{
				float num = 1f / (float)(this.GatherResourcesIntervalDays * 60000);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
				}
				this.fullness += num;
				if (this.fullness > 1f)
				{
					this.fullness = 1f;
				}
			}
		}

		// Token: 0x06006A2B RID: 27179 RVA: 0x0023B97C File Offset: 0x00239B7C
		public void Gathered(Pawn doer)
		{
			if (!this.Active)
			{
				Log.Error(doer + " gathered body resources while not Active: " + this.parent);
			}
			if (!Rand.Chance(doer.GetStatValue(StatDefOf.AnimalGatherYield, true)))
			{
				MoteMaker.ThrowText((doer.DrawPos + this.parent.DrawPos) / 2f, this.parent.Map, "TextMote_ProductWasted".Translate(), 3.65f);
			}
			else
			{
				int i = GenMath.RoundRandom((float)this.ResourceAmount * this.fullness);
				while (i > 0)
				{
					int num = Mathf.Clamp(i, 1, this.ResourceDef.stackLimit);
					i -= num;
					Thing thing = ThingMaker.MakeThing(this.ResourceDef, null);
					thing.stackCount = num;
					GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
			this.fullness = 0f;
		}

		// Token: 0x04003B3B RID: 15163
		protected float fullness;
	}
}
