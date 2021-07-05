using System;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000F27 RID: 3879
	public class RitualBehaviorWorker_PartyDanceDrums : RitualBehaviorWorker
	{
		// Token: 0x06005C49 RID: 23625 RVA: 0x001FD76F File Offset: 0x001FB96F
		public RitualBehaviorWorker_PartyDanceDrums()
		{
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x001FD77E File Offset: 0x001FB97E
		public RitualBehaviorWorker_PartyDanceDrums(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06005C4B RID: 23627 RVA: 0x001FD78E File Offset: 0x001FB98E
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_PartyDanceDrums(target, ritual, obligation, this.def.stages, assignments, organizer);
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x06005C4C RID: 23628 RVA: 0x001FD7A7 File Offset: 0x001FB9A7
		public override Sustainer SoundPlaying
		{
			get
			{
				return this.soundPlaying;
			}
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x001FD7B0 File Offset: 0x001FB9B0
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			TargetInfo selectedTarget = ritual.selectedTarget;
			if (Find.TickManager.TicksGame % 20 == 0 || this.numActiveDrums == -1)
			{
				this.numActiveDrums = 0;
				foreach (Thing thing in selectedTarget.Map.listerBuldingOfDefInProximity.GetForCell(selectedTarget.Cell, (float)this.def.maxEnhancerDistance, ThingDefOf.Drum, null))
				{
					Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
					if (thing.GetRoom(RegionType.Set_All) == selectedTarget.Thing.GetRoom(RegionType.Set_All) && building_MusicalInstrument != null && building_MusicalInstrument.IsBeingPlayed)
					{
						this.numActiveDrums++;
						this.anyDrumReached = true;
					}
				}
			}
			bool flag = this.numActiveDrums > 0 || this.anyDrumReached;
			SoundDef soundDef = (!this.def.soundDefsPerEnhancerCount.NullOrEmpty<SoundDef>() && flag) ? this.def.soundDefsPerEnhancerCount[Math.Min(this.numActiveDrums, this.def.soundDefsPerEnhancerCount.Count)] : null;
			if (soundDef != null && (this.soundPlaying == null || this.soundPlaying.def != soundDef))
			{
				this.soundPlaying = soundDef.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
			}
			if (flag)
			{
				Sustainer sustainer = this.soundPlaying;
				if (sustainer == null)
				{
					return;
				}
				sustainer.Maintain();
			}
		}

		// Token: 0x040035BD RID: 13757
		private Sustainer soundPlaying;

		// Token: 0x040035BE RID: 13758
		private int numActiveDrums = -1;

		// Token: 0x040035BF RID: 13759
		private bool anyDrumReached;
	}
}
