using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017BD RID: 6077
	public class CompFadesInOut : ThingComp
	{
		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x0600866C RID: 34412 RVA: 0x0005A29F File Offset: 0x0005849F
		public CompProperties_FadesInOut Props
		{
			get
			{
				return (CompProperties_FadesInOut)this.props;
			}
		}

		// Token: 0x0600866D RID: 34413 RVA: 0x0005A2AC File Offset: 0x000584AC
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned)
			{
				this.ageTicks++;
			}
		}

		// Token: 0x0600866E RID: 34414 RVA: 0x00278DA8 File Offset: 0x00276FA8
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

		// Token: 0x0600866F RID: 34415 RVA: 0x0005A2CF File Offset: 0x000584CF
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
		}

		// Token: 0x04005689 RID: 22153
		private int ageTicks;
	}
}
