using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020017FB RID: 6139
	public class CompMoteEmitterProximityScan : CompMoteEmitter
	{
		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x060087D9 RID: 34777 RVA: 0x0005B210 File Offset: 0x00059410
		private CompProperties_MoteEmitterProximityScan Props
		{
			get
			{
				return (CompProperties_MoteEmitterProximityScan)this.props;
			}
		}

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x060087DA RID: 34778 RVA: 0x0027CA38 File Offset: 0x0027AC38
		private CompSendSignalOnPawnProximity ProximityComp
		{
			get
			{
				CompSendSignalOnPawnProximity result;
				if ((result = this.proximityCompCached) == null)
				{
					result = (this.proximityCompCached = this.parent.GetComp<CompSendSignalOnPawnProximity>());
				}
				return result;
			}
		}

		// Token: 0x060087DB RID: 34779 RVA: 0x0005B21D File Offset: 0x0005941D
		public override void PostDeSpawn(Map map)
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x060087DC RID: 34780 RVA: 0x0027CA64 File Offset: 0x0027AC64
		public override void CompTick()
		{
			if (this.ProximityComp == null || this.ProximityComp.Sent)
			{
				if (this.sustainer != null && !this.sustainer.Ended)
				{
					this.sustainer.End();
				}
				return;
			}
			if (this.mote == null)
			{
				base.Emit();
			}
			if (!this.Props.soundEmitting.NullOrUndefined())
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = this.Props.soundEmitting.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				this.sustainer.Maintain();
			}
			if (this.mote == null)
			{
				return;
			}
			Mote mote = this.mote;
			if (mote != null)
			{
				mote.Maintain();
			}
			float a;
			if (!this.ProximityComp.Enabled)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.ticksSinceLastEmitted = 0;
				}
				else
				{
					this.ticksSinceLastEmitted++;
				}
				float num = (float)this.ticksSinceLastEmitted / 60f;
				if (num <= this.Props.warmupPulseFadeInTime)
				{
					if (this.Props.warmupPulseFadeInTime > 0f)
					{
						a = num / this.Props.warmupPulseFadeInTime;
					}
					else
					{
						a = 1f;
					}
				}
				else if (num <= this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime)
				{
					a = 1f;
				}
				else if (this.Props.warmupPulseFadeOutTime > 0f)
				{
					a = 1f - Mathf.InverseLerp(this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime, this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime + this.Props.warmupPulseFadeOutTime, num);
				}
				else
				{
					a = 1f;
				}
			}
			else
			{
				a = 1f;
			}
			this.mote.instanceColor.a = a;
		}

		// Token: 0x04005720 RID: 22304
		private CompSendSignalOnPawnProximity proximityCompCached;

		// Token: 0x04005721 RID: 22305
		private Sustainer sustainer;
	}
}
