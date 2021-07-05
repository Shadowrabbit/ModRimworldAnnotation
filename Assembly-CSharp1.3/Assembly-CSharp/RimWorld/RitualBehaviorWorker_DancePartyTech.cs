using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000F28 RID: 3880
	public class RitualBehaviorWorker_DancePartyTech : RitualBehaviorWorker
	{
		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06005C4E RID: 23630 RVA: 0x001FD93C File Offset: 0x001FBB3C
		public override Sustainer SoundPlaying
		{
			get
			{
				return this.soundPlaying;
			}
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x001FD352 File Offset: 0x001FB552
		public RitualBehaviorWorker_DancePartyTech()
		{
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x001FD35A File Offset: 0x001FB55A
		public RitualBehaviorWorker_DancePartyTech(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x001FD944 File Offset: 0x001FBB44
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			Thing thing = ritual.selectedTarget.Thing;
			if (Find.TickManager.TicksGame % 20 == 0)
			{
				SoundDef soundDef = null;
				using (List<Pawn>.Enumerator enumerator = ritual.lord.ownedPawns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (GatheringsUtility.InGatheringArea(enumerator.Current.Position, ritual.selectedTarget.Cell, ritual.Map))
						{
							soundDef = SoundDefOf.DanceParty_NoMusic;
							break;
						}
					}
				}
				if (thing != null)
				{
					CompLightball compLightball = thing.TryGetComp<CompLightball>();
					if (compLightball != null)
					{
						soundDef = (compLightball.Playing ? compLightball.SoundToPlay : null);
					}
				}
				if (soundDef != null && (this.soundPlaying == null || this.soundPlaying.def != soundDef))
				{
					this.soundPlaying = soundDef.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(ritual.selectedTarget.Cell, ritual.selectedTarget.Map, false), MaintenanceType.PerTick));
				}
			}
			if (this.soundPlaying != null && !this.soundPlaying.Ended)
			{
				this.soundPlaying.Maintain();
			}
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x001FDA68 File Offset: 0x001FBC68
		public override void Cleanup(LordJob_Ritual ritual)
		{
			base.Cleanup(ritual);
			this.soundPlaying = null;
		}

		// Token: 0x040035C0 RID: 13760
		private Sustainer soundPlaying;
	}
}
