using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001162 RID: 4450
	public class CompMoteEmitter : ThingComp
	{
		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06006AEA RID: 27370 RVA: 0x0023E383 File Offset: 0x0023C583
		private CompProperties_MoteEmitter Props
		{
			get
			{
				return (CompProperties_MoteEmitter)this.props;
			}
		}

		// Token: 0x06006AEB RID: 27371 RVA: 0x0023E390 File Offset: 0x0023C590
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.Props.ticksSinceLastEmittedMaxOffset > 0)
			{
				this.ticksSinceLastEmitted = Rand.Range(0, this.Props.ticksSinceLastEmittedMaxOffset);
			}
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x0023E3C0 File Offset: 0x0023C5C0
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return;
			}
			CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
			if (comp2 != null && comp2.ticksLeft <= 0)
			{
				return;
			}
			CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
			if (comp3 != null && !comp3.Initiated)
			{
				return;
			}
			Skyfaller skyfaller = this.parent as Skyfaller;
			if (skyfaller != null && skyfaller.FadingOut)
			{
				return;
			}
			CompStudiable comp4 = this.parent.GetComp<CompStudiable>();
			if (comp4 != null && comp4.Completed)
			{
				return;
			}
			if (this.Props.emissionInterval != -1 && !this.Props.maintain)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.Emit();
					this.ticksSinceLastEmitted = 0;
				}
				else
				{
					this.ticksSinceLastEmitted++;
				}
			}
			else if (this.mote == null || this.mote.Destroyed)
			{
				this.Emit();
			}
			if (this.mote != null && !this.mote.Destroyed)
			{
				if (typeof(MoteAttached).IsAssignableFrom(this.Props.mote.thingClass) && skyfaller != null)
				{
					this.mote.exactRotation = skyfaller.DrawAngle();
				}
				if (this.Props.maintain)
				{
					this.mote.Maintain();
				}
			}
		}

		// Token: 0x06006AED RID: 27373 RVA: 0x0023E524 File Offset: 0x0023C724
		protected virtual void Emit()
		{
			if (!this.parent.Spawned)
			{
				Log.Error("Thing tried spawning mote without being spawned!");
				return;
			}
			Vector3 vector = this.Props.offset;
			if (this.Props.offsetMin != Vector3.zero || this.Props.offsetMax != Vector3.zero)
			{
				vector = this.Props.EmissionOffset;
			}
			if (typeof(MoteAttached).IsAssignableFrom(this.Props.mote.thingClass))
			{
				this.mote = MoteMaker.MakeAttachedOverlay(this.parent, this.Props.mote, vector, 1f, -1f);
			}
			else
			{
				Vector3 vector2 = this.parent.DrawPos + vector;
				if (vector2.InBounds(this.parent.Map))
				{
					this.mote = MoteMaker.MakeStaticMote(vector2, this.parent.Map, this.Props.mote, 1f);
				}
			}
			if (!this.Props.soundOnEmission.NullOrUndefined())
			{
				this.Props.soundOnEmission.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
		}

		// Token: 0x06006AEE RID: 27374 RVA: 0x0023E658 File Offset: 0x0023C858
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		// Token: 0x04003B70 RID: 15216
		public int ticksSinceLastEmitted;

		// Token: 0x04003B71 RID: 15217
		protected Mote mote;
	}
}
