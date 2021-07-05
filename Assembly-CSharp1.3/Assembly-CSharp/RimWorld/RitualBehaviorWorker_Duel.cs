using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000F24 RID: 3876
	public class RitualBehaviorWorker_Duel : RitualBehaviorWorker
	{
		// Token: 0x06005C38 RID: 23608 RVA: 0x001FD352 File Offset: 0x001FB552
		public RitualBehaviorWorker_Duel()
		{
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x001FD35A File Offset: 0x001FB55A
		public RitualBehaviorWorker_Duel(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06005C3A RID: 23610 RVA: 0x001FD37C File Offset: 0x001FB57C
		public override Sustainer SoundPlaying
		{
			get
			{
				return this.soundPlaying;
			}
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x001FD384 File Offset: 0x001FB584
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			RitualBehaviorWorker_Duel.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.target = target;
			string text = base.CanStartRitualNow(CS$<>8__locals1.target, ritual, selectedPawn, forcedForRole);
			if (text != null)
			{
				return text;
			}
			CS$<>8__locals1.room = CS$<>8__locals1.target.Cell.GetRoom(CS$<>8__locals1.target.Map);
			if (RitualBehaviorWorker_Duel.<CanStartRitualNow>g__StandableCellsInRange|8_0(new IntRange(RitualBehaviorWorker_Duel.RadiusRangeLeader, RitualBehaviorWorker_Duel.RadiusRangeLeader), ref CS$<>8__locals1) < 1)
			{
				return "CantStartNotEnoughSpaceDuel".Translate(RitualBehaviorWorker_Duel.RadiusRangeSpectators.Average.Named("MINRADIUS"));
			}
			if (RitualBehaviorWorker_Duel.<CanStartRitualNow>g__StandableCellsInRange|8_0(new IntRange(RitualBehaviorWorker_Duel.RadiusRangeDuelists, RitualBehaviorWorker_Duel.RadiusRangeDuelists), ref CS$<>8__locals1) < 2)
			{
				return "CantStartNotEnoughSpaceDuel".Translate(RitualBehaviorWorker_Duel.RadiusRangeSpectators.Average.Named("MINRADIUS"));
			}
			if (RitualBehaviorWorker_Duel.<CanStartRitualNow>g__StandableCellsInRange|8_0(RitualBehaviorWorker_Duel.RadiusRangeSpectators, ref CS$<>8__locals1) < 20)
			{
				return "CantStartNotEnoughSpaceDuel".Translate(RitualBehaviorWorker_Duel.RadiusRangeSpectators.Average.Named("MINRADIUS"));
			}
			return null;
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x001FD49A File Offset: 0x001FB69A
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Ritual_Duel(target, ritual, obligation, this.def.stages, assignments, organizer);
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x001FD4B4 File Offset: 0x001FB6B4
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			if (ritual.StageIndex == 4)
			{
				if (this.soundPlaying == null || this.soundPlaying.Ended)
				{
					TargetInfo selectedTarget = ritual.selectedTarget;
					this.soundPlaying = SoundDefOf.DuelMusic.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
				}
				Sustainer sustainer = this.soundPlaying;
				if (sustainer == null)
				{
					return;
				}
				sustainer.Maintain();
			}
		}

		// Token: 0x06005C3E RID: 23614 RVA: 0x001FD527 File Offset: 0x001FB727
		public override void Cleanup(LordJob_Ritual ritual)
		{
			this.soundPlaying = null;
		}

		// Token: 0x06005C40 RID: 23616 RVA: 0x001FD54C File Offset: 0x001FB74C
		[CompilerGenerated]
		internal static int <CanStartRitualNow>g__StandableCellsInRange|8_0(IntRange range, ref RitualBehaviorWorker_Duel.<>c__DisplayClass8_0 A_1)
		{
			int num = 0;
			foreach (IntVec3 intVec in CellRect.CenteredOn(A_1.target.Cell, range.max))
			{
				float num2 = intVec.DistanceTo(A_1.target.Cell);
				if (num2 >= (float)range.min && num2 <= (float)range.max && intVec.Standable(A_1.target.Map) && intVec.GetRoom(A_1.target.Map) == A_1.room)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x040035B9 RID: 13753
		private Sustainer soundPlaying;

		// Token: 0x040035BA RID: 13754
		public static readonly IntRange RadiusRangeSpectators = new IntRange(5, 7);

		// Token: 0x040035BB RID: 13755
		public static readonly int RadiusRangeLeader = 3;

		// Token: 0x040035BC RID: 13756
		public static readonly int RadiusRangeDuelists = 2;
	}
}
