using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187F RID: 6271
	public class CompThrownMoteEmitter : ThingComp
	{
		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x06008B1A RID: 35610 RVA: 0x0005D518 File Offset: 0x0005B718
		private CompProperties_ThrownMoteEmitter Props
		{
			get
			{
				return (CompProperties_ThrownMoteEmitter)this.props;
			}
		}

		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x06008B1B RID: 35611 RVA: 0x00288890 File Offset: 0x00286A90
		private Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.Props.offsetMin.x, this.Props.offsetMax.x), Rand.Range(this.Props.offsetMin.y, this.Props.offsetMax.y), Rand.Range(this.Props.offsetMin.z, this.Props.offsetMax.z));
			}
		}

		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x06008B1C RID: 35612 RVA: 0x0005D525 File Offset: 0x0005B725
		private Color EmissionColor
		{
			get
			{
				return Color.Lerp(this.Props.colorA, this.Props.colorB, Rand.Value);
			}
		}

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x06008B1D RID: 35613 RVA: 0x00288914 File Offset: 0x00286B14
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
				return comp3 == null || comp3.Initiated;
			}
		}

		// Token: 0x06008B1E RID: 35614 RVA: 0x00288998 File Offset: 0x00286B98
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

		// Token: 0x06008B1F RID: 35615 RVA: 0x00288A04 File Offset: 0x00286C04
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
				GenSpawn.Spawn(moteThrown, moteThrown.exactPosition.ToIntVec3(), this.parent.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x06008B20 RID: 35616 RVA: 0x0005D547 File Offset: 0x0005B747
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, "ticksSinceLastEmitted", 0, false);
			Scribe_Values.Look<bool>(ref this.emittedBefore, "emittedBefore", false, false);
		}

		// Token: 0x04005930 RID: 22832
		public bool emittedBefore;

		// Token: 0x04005931 RID: 22833
		public int ticksSinceLastEmitted;
	}
}
