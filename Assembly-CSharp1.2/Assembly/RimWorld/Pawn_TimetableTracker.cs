using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001528 RID: 5416
	public class Pawn_TimetableTracker : IExposable
	{
		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06007540 RID: 30016 RVA: 0x0004F180 File Offset: 0x0004D380
		public TimeAssignmentDef CurrentAssignment
		{
			get
			{
				if (!this.pawn.IsColonist)
				{
					return TimeAssignmentDefOf.Anything;
				}
				return this.times[GenLocalDate.HourOfDay(this.pawn)];
			}
		}

		// Token: 0x06007541 RID: 30017 RVA: 0x0023BB88 File Offset: 0x00239D88
		public Pawn_TimetableTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.times = new List<TimeAssignmentDef>(24);
			for (int i = 0; i < 24; i++)
			{
				TimeAssignmentDef item;
				if (i <= 5 || i > 21)
				{
					item = TimeAssignmentDefOf.Sleep;
				}
				else
				{
					item = TimeAssignmentDefOf.Anything;
				}
				this.times.Add(item);
			}
		}

		// Token: 0x06007542 RID: 30018 RVA: 0x0023BBE0 File Offset: 0x00239DE0
		public void ExposeData()
		{
			Scribe_Collections.Look<TimeAssignmentDef>(ref this.times, "times", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !ModsConfig.RoyaltyActive)
			{
				for (int i = 0; i < this.times.Count; i++)
				{
					if (this.times[i] == TimeAssignmentDefOf.Meditate)
					{
						this.times[i] = TimeAssignmentDefOf.Anything;
					}
				}
			}
		}

		// Token: 0x06007543 RID: 30019 RVA: 0x0004F1AB File Offset: 0x0004D3AB
		public TimeAssignmentDef GetAssignment(int hour)
		{
			return this.times[hour];
		}

		// Token: 0x06007544 RID: 30020 RVA: 0x0004F1B9 File Offset: 0x0004D3B9
		public void SetAssignment(int hour, TimeAssignmentDef ta)
		{
			this.times[hour] = ta;
		}

		// Token: 0x04004D55 RID: 19797
		private Pawn pawn;

		// Token: 0x04004D56 RID: 19798
		public List<TimeAssignmentDef> times;
	}
}
