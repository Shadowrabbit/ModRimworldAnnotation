using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200112F RID: 4399
	public class CompFadesInOut : ThingComp
	{
		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x060069BE RID: 27070 RVA: 0x0023A267 File Offset: 0x00238467
		public CompProperties_FadesInOut Props
		{
			get
			{
				return (CompProperties_FadesInOut)this.props;
			}
		}

		// Token: 0x060069BF RID: 27071 RVA: 0x0023A274 File Offset: 0x00238474
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned)
			{
				this.ageTicks++;
			}
		}

		// Token: 0x060069C0 RID: 27072 RVA: 0x0023A298 File Offset: 0x00238498
		public float Opacity()
		{
			float num = this.ageTicks.TicksToSeconds();
			if (num <= this.Props.fadeInSecs)
			{
				if (this.Props.fadeInSecs > 0f)
				{
					return num / this.Props.fadeInSecs;
				}
				return 1f;
			}
			else
			{
				if (num <= this.Props.fadeInSecs + this.Props.solidTimeSecs)
				{
					return 1f;
				}
				if (this.Props.fadeOutSecs > 0f)
				{
					return 1f - Mathf.InverseLerp(this.Props.fadeInSecs + this.Props.solidTimeSecs, this.Props.fadeInSecs + this.Props.solidTimeSecs + this.Props.fadeOutSecs, num);
				}
				return 1f;
			}
		}

		// Token: 0x060069C1 RID: 27073 RVA: 0x0023A363 File Offset: 0x00238563
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
		}

		// Token: 0x04003B0D RID: 15117
		private int ageTicks;
	}
}
