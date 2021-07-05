using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000F9B RID: 3995
	public class RitualRoleBehavior : IExposable
	{
		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06005E85 RID: 24197 RVA: 0x00206930 File Offset: 0x00204B30
		public List<RitualPosition> CustomPositionsForReading
		{
			get
			{
				return this.customPositions;
			}
		}

		// Token: 0x06005E86 RID: 24198 RVA: 0x00206938 File Offset: 0x00204B38
		public RitualPosition GetPosition(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			if (!this.customPositions.NullOrEmpty<RitualPosition>())
			{
				foreach (RitualPosition ritualPosition in this.customPositions)
				{
					if (ritualPosition.CanUse(spot, p, ritual))
					{
						return ritualPosition;
					}
				}
			}
			return null;
		}

		// Token: 0x06005E87 RID: 24199 RVA: 0x002069A4 File Offset: 0x00204BA4
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
			Scribe_Defs.Look<DutyDef>(ref this.dutyDef, "dutyDef");
			Scribe_Collections.Look<RitualPosition>(ref this.customPositions, "customPositions", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<InteractionDef>(ref this.speakerInteraction, "speakerInteraction");
		}

		// Token: 0x04003684 RID: 13956
		[NoTranslate]
		public string roleId;

		// Token: 0x04003685 RID: 13957
		public DutyDef dutyDef;

		// Token: 0x04003686 RID: 13958
		protected List<RitualPosition> customPositions;

		// Token: 0x04003687 RID: 13959
		public InteractionDef speakerInteraction;
	}
}
