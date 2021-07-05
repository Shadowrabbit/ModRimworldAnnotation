using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001164 RID: 4452
	public class CompMoteEmitterProximityScan : CompMoteEmitter
	{
		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06006AF1 RID: 27377 RVA: 0x0023E6C3 File Offset: 0x0023C8C3
		private CompProperties_MoteEmitterProximityScan Props
		{
			get
			{
				return (CompProperties_MoteEmitterProximityScan)this.props;
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06006AF2 RID: 27378 RVA: 0x0023E6D0 File Offset: 0x0023C8D0
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

		// Token: 0x06006AF3 RID: 27379 RVA: 0x0023E6FB File Offset: 0x0023C8FB
		public override void PostDeSpawn(Map map)
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06006AF4 RID: 27380 RVA: 0x0023E720 File Offset: 0x0023C920
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
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
				this.Emit();
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

		// Token: 0x04003B76 RID: 15222
		private CompSendSignalOnPawnProximity proximityCompCached;

		// Token: 0x04003B77 RID: 15223
		private Sustainer sustainer;
	}
}
