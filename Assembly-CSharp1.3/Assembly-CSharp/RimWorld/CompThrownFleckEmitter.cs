using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BF RID: 4543
	public class CompThrownFleckEmitter : ThingComp
	{
		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x06006D68 RID: 28008 RVA: 0x0024A789 File Offset: 0x00248989
		private CompProperties_ThrownFleckEmitter Props
		{
			get
			{
				return (CompProperties_ThrownFleckEmitter)this.props;
			}
		}

		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x06006D69 RID: 28009 RVA: 0x0024A798 File Offset: 0x00248998
		private Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.Props.offsetMin.x, this.Props.offsetMax.x), Rand.Range(this.Props.offsetMin.y, this.Props.offsetMax.y), Rand.Range(this.Props.offsetMin.z, this.Props.offsetMax.z));
			}
		}

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06006D6A RID: 28010 RVA: 0x0024A819 File Offset: 0x00248A19
		private Color EmissionColor
		{
			get
			{
				return Color.Lerp(this.Props.colorA, this.Props.colorB, Rand.Value);
			}
		}

		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06006D6B RID: 28011 RVA: 0x0024A83C File Offset: 0x00248A3C
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
				CompLoudspeaker comp4 = this.parent.GetComp<CompLoudspeaker>();
				if (comp4 != null && !comp4.Active)
				{
					return false;
				}
				CompHackable comp5 = this.parent.GetComp<CompHackable>();
				Building_Crate building_Crate;
				return (comp5 == null || !comp5.IsHacked) && ((building_Crate = (this.parent as Building_Crate)) == null || building_Crate.HasAnyContents);
			}
		}

		// Token: 0x06006D6C RID: 28012 RVA: 0x0024A914 File Offset: 0x00248B14
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

		// Token: 0x06006D6D RID: 28013 RVA: 0x0024A980 File Offset: 0x00248B80
		private void Emit()
		{
			for (int i = 0; i < this.Props.burstCount; i++)
			{
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(this.parent.DrawPos + this.EmissionOffset, this.parent.Map, this.Props.fleck, this.Props.scale.RandomInRange);
				dataStatic.rotationRate = this.Props.rotationRate.RandomInRange;
				dataStatic.instanceColor = new Color?(this.EmissionColor);
				dataStatic.velocityAngle = this.Props.velocityX.RandomInRange;
				dataStatic.velocitySpeed = this.Props.velocityY.RandomInRange;
				this.parent.Map.flecks.CreateFleck(dataStatic);
			}
		}

		// Token: 0x06006D6E RID: 28014 RVA: 0x0024AA58 File Offset: 0x00248C58
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, "ticksSinceLastEmitted", 0, false);
			Scribe_Values.Look<bool>(ref this.emittedBefore, "emittedBefore", false, false);
		}

		// Token: 0x04003CC4 RID: 15556
		public bool emittedBefore;

		// Token: 0x04003CC5 RID: 15557
		public int ticksSinceLastEmitted;
	}
}
