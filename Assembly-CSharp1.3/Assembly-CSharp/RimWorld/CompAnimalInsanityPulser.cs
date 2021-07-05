using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010F7 RID: 4343
	public class CompAnimalInsanityPulser : ThingComp
	{
		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x0600681A RID: 26650 RVA: 0x0023366E File Offset: 0x0023186E
		public CompProperties_AnimalInsanityPulser Props
		{
			get
			{
				return (CompProperties_AnimalInsanityPulser)this.props;
			}
		}

		// Token: 0x0600681B RID: 26651 RVA: 0x0023367B File Offset: 0x0023187B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x0023369D File Offset: 0x0023189D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToInsanityPulse, "ticksToInsanityPulse", 0, false);
		}

		// Token: 0x0600681D RID: 26653 RVA: 0x002336B8 File Offset: 0x002318B8
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

		// Token: 0x0600681E RID: 26654 RVA: 0x00233708 File Offset: 0x00231908
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
					if (enumerator.Current.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false, false, false))
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

		// Token: 0x04003A83 RID: 14979
		private int ticksToInsanityPulse;
	}
}
