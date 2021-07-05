using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011DF RID: 4575
	public class CompSkyfallerRandomizeDirection : ThingComp
	{
		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06006E62 RID: 28258 RVA: 0x00250086 File Offset: 0x0024E286
		public CompProperties_SkyfallerRandomizeDirection Props
		{
			get
			{
				return (CompProperties_SkyfallerRandomizeDirection)this.props;
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06006E63 RID: 28259 RVA: 0x00250093 File Offset: 0x0024E293
		public Skyfaller Skyfaller
		{
			get
			{
				return (Skyfaller)this.parent;
			}
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x06006E64 RID: 28260 RVA: 0x002500A0 File Offset: 0x0024E2A0
		public Vector3 Offset
		{
			get
			{
				return this.currentOffset;
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x06006E65 RID: 28261 RVA: 0x002500A8 File Offset: 0x0024E2A8
		public float ExtraDrawAngle
		{
			get
			{
				return Mathf.Lerp(this.lastAngle, this.currentAngle, Mathf.Clamp((float)(Find.TickManager.TicksGame + CompSkyfallerRandomizeDirection.DirectionChangeBlendDuration / 2 - this.lastDirectionChange), 0f, (float)CompSkyfallerRandomizeDirection.DirectionChangeBlendDuration) / (float)CompSkyfallerRandomizeDirection.DirectionChangeBlendDuration) / 2f;
			}
		}

		// Token: 0x06006E66 RID: 28262 RVA: 0x002500FD File Offset: 0x0024E2FD
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.initialAngle = this.Skyfaller.angle;
			this.directionChangeInterval = this.Props.directionChangeInterval.RandomInRange;
			this.lastDirectionChange = Find.TickManager.TicksGame;
		}

		// Token: 0x06006E67 RID: 28263 RVA: 0x0025013C File Offset: 0x0024E33C
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(this.directionChangeInterval))
			{
				this.lastAngle = this.currentAngle;
				this.currentAngle = Rand.Value * this.Props.maxDeviationFromStartingAngle * (float)Rand.Sign;
				this.lastDirectionChange = Find.TickManager.TicksGame;
			}
			Quaternion rotation = Quaternion.AngleAxis(this.currentAngle - this.initialAngle, Vector3.up);
			this.currentOffset += rotation * Vector3.forward * Time.deltaTime;
		}

		// Token: 0x06006E68 RID: 28264 RVA: 0x002501DC File Offset: 0x0024E3DC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.directionChangeInterval, "directionChangeInterval", 0, false);
			Scribe_Values.Look<float>(ref this.currentAngle, "currentAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.lastAngle, "lastAngle", 0f, false);
			Scribe_Values.Look<Vector3>(ref this.currentOffset, "currentOffset", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.initialAngle, "initialAngle", 0f, false);
		}

		// Token: 0x04003D38 RID: 15672
		private static int DirectionChangeBlendDuration = 100;

		// Token: 0x04003D39 RID: 15673
		private int directionChangeInterval;

		// Token: 0x04003D3A RID: 15674
		private int lastDirectionChange;

		// Token: 0x04003D3B RID: 15675
		private float initialAngle;

		// Token: 0x04003D3C RID: 15676
		private float currentAngle;

		// Token: 0x04003D3D RID: 15677
		private float lastAngle;

		// Token: 0x04003D3E RID: 15678
		private Vector3 currentOffset;
	}
}
