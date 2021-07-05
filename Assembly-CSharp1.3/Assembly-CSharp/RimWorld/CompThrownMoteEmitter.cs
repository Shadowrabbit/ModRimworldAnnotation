using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C1 RID: 4545
	public class CompThrownMoteEmitter : ThingComp
	{
		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x06006D72 RID: 28018 RVA: 0x0024AAE6 File Offset: 0x00248CE6
		private CompProperties_ThrownMoteEmitter Props
		{
			get
			{
				return (CompProperties_ThrownMoteEmitter)this.props;
			}
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06006D73 RID: 28019 RVA: 0x0024AAF4 File Offset: 0x00248CF4
		private Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.Props.offsetMin.x, this.Props.offsetMax.x), Rand.Range(this.Props.offsetMin.y, this.Props.offsetMax.y), Rand.Range(this.Props.offsetMin.z, this.Props.offsetMax.z));
			}
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06006D74 RID: 28020 RVA: 0x0024AB75 File Offset: 0x00248D75
		private Color EmissionColor
		{
			get
			{
				return Color.Lerp(this.Props.colorA, this.Props.colorB, Rand.Value);
			}
		}

		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x06006D75 RID: 28021 RVA: 0x0024AB98 File Offset: 0x00248D98
		private bool IsOn
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
				CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
				if (comp2 != null && comp2.ticksLeft <= 0)
				{
					return false;
				}
				Building_MusicalInstrument building_MusicalInstrument = this.parent as Building_MusicalInstrument;
				if (building_MusicalInstrument != null && !building_MusicalInstrument.IsBeingPlayed)
				{
					return false;
				}
				CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
				if (comp3 != null && !comp3.Initiated)
				{
					return false;
				}
				Skyfaller skyfaller = this.parent as Skyfaller;
				return skyfaller == null || !skyfaller.FadingOut;
			}
		}

		// Token: 0x06006D76 RID: 28022 RVA: 0x0024AC38 File Offset: 0x00248E38
		public override void CompTick()
		{
			if (!this.IsOn)
			{
				return;
			}
			if (this.Props.emissionInterval == -1)
			{
				if (!this.emittedBefore)
				{
					this.Emit();
					this.emittedBefore = true;
				}
				return;
			}
			if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
			{
				this.Emit();
				this.ticksSinceLastEmitted = 0;
				return;
			}
			this.ticksSinceLastEmitted++;
		}

		// Token: 0x06006D77 RID: 28023 RVA: 0x0024ACA4 File Offset: 0x00248EA4
		private void Emit()
		{
			for (int i = 0; i < this.Props.burstCount; i++)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(this.Props.mote, null);
				moteThrown.Scale = this.Props.scale.RandomInRange;
				moteThrown.rotationRate = this.Props.rotationRate.RandomInRange;
				moteThrown.exactPosition = this.parent.DrawPos + this.EmissionOffset;
				moteThrown.instanceColor = this.EmissionColor;
				moteThrown.SetVelocity(this.Props.velocityX.RandomInRange, this.Props.velocityY.RandomInRange);
				if (moteThrown.exactPosition.ToIntVec3().InBounds(this.parent.Map))
				{
					GenSpawn.Spawn(moteThrown, moteThrown.exactPosition.ToIntVec3(), this.parent.Map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06006D78 RID: 28024 RVA: 0x0024AD99 File Offset: 0x00248F99
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, "ticksSinceLastEmitted", 0, false);
			Scribe_Values.Look<bool>(ref this.emittedBefore, "emittedBefore", false, false);
		}

		// Token: 0x04003CD1 RID: 15569
		public bool emittedBefore;

		// Token: 0x04003CD2 RID: 15570
		public int ticksSinceLastEmitted;
	}
}
