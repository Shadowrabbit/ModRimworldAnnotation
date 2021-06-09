using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CC RID: 6092
	public abstract class CompHasGatherableBodyResource : ThingComp
	{
		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x060086B8 RID: 34488
		protected abstract int GatherResourcesIntervalDays { get; }

		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x060086B9 RID: 34489
		protected abstract int ResourceAmount { get; }

		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x060086BA RID: 34490
		protected abstract ThingDef ResourceDef { get; }

		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x060086BB RID: 34491
		protected abstract string SaveKey { get; }

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x060086BC RID: 34492 RVA: 0x0005A614 File Offset: 0x00058814
		public float Fullness
		{
			get
			{
				return this.fullness;
			}
		}

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x060086BD RID: 34493 RVA: 0x0005A61C File Offset: 0x0005881C
		protected virtual bool Active
		{
			get
			{
				return this.parent.Faction != null && !this.parent.Suspended;
			}
		}

		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x060086BE RID: 34494 RVA: 0x0005A63D File Offset: 0x0005883D
		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1f;
			}
		}

		// Token: 0x060086BF RID: 34495 RVA: 0x0005A659 File Offset: 0x00058859
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.SaveKey, 0f, false);
		}

		// Token: 0x060086C0 RID: 34496 RVA: 0x00279908 File Offset: 0x00277B08
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

		// Token: 0x060086C1 RID: 34497 RVA: 0x00279970 File Offset: 0x00277B70
		public void Gathered(Pawn doer)
		{
			if (!this.Active)
			{
				Log.Error(doer + " gathered body resources while not Active: " + this.parent, false);
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

		// Token: 0x040056A3 RID: 22179
		protected float fullness;
	}
}
