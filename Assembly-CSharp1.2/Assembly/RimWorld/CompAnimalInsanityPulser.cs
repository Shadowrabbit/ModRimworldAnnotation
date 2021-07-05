using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200178C RID: 6028
	public class CompAnimalInsanityPulser : ThingComp
	{
		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x06008511 RID: 34065 RVA: 0x00059249 File Offset: 0x00057449
		public CompProperties_AnimalInsanityPulser Props
		{
			get
			{
				return (CompProperties_AnimalInsanityPulser)this.props;
			}
		}

		// Token: 0x06008512 RID: 34066 RVA: 0x00059256 File Offset: 0x00057456
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x06008513 RID: 34067 RVA: 0x00059278 File Offset: 0x00057478
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToInsanityPulse, "ticksToInsanityPulse", 0, false);
		}

		// Token: 0x06008514 RID: 34068 RVA: 0x00275968 File Offset: 0x00273B68
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.ticksToInsanityPulse--;
			if (this.ticksToInsanityPulse <= 0)
			{
				this.DoAnimalInsanityPulse();
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x06008515 RID: 34069 RVA: 0x002759B8 File Offset: 0x00273BB8
		private void DoAnimalInsanityPulse()
		{
			IEnumerable<Pawn> enumerable = from p in this.parent.Map.mapPawns.AllPawnsSpawned
			where p.RaceProps.Animal && p.Position.InHorDistOf(this.parent.Position, (float)this.Props.radius)
			select p;
			bool flag = false;
			using (IEnumerator<Pawn> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				Messages.Message("MessageAnimalInsanityPulse".Translate(this.parent.Named("SOURCE")), this.parent, MessageTypeDefOf.ThreatSmall, true);
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
				if (this.parent.Map == Find.CurrentMap)
				{
					Find.CameraDriver.shaker.DoShake(4f);
				}
			}
		}

		// Token: 0x04005614 RID: 22036
		private int ticksToInsanityPulse;
	}
}
