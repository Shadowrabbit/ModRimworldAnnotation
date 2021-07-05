using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7A RID: 3706
	public class Pawn_TimetableTracker : IExposable
	{
		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x060056B6 RID: 22198 RVA: 0x001D6C2E File Offset: 0x001D4E2E
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

		// Token: 0x060056B7 RID: 22199 RVA: 0x001D6C5C File Offset: 0x001D4E5C
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

		// Token: 0x060056B8 RID: 22200 RVA: 0x001D6CB4 File Offset: 0x001D4EB4
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

		// Token: 0x060056B9 RID: 22201 RVA: 0x001D6D20 File Offset: 0x001D4F20
		public TimeAssignmentDef GetAssignment(int hour)
		{
			return this.times[hour];
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x001D6D2E File Offset: 0x001D4F2E
		public void SetAssignment(int hour, TimeAssignmentDef ta)
		{
			this.times[hour] = ta;
		}

		// Token: 0x04003330 RID: 13104
		private Pawn pawn;

		// Token: 0x04003331 RID: 13105
		public List<TimeAssignmentDef> times;
	}
}
