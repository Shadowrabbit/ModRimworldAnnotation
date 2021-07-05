using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DEB RID: 3563
	public class   LordToil_AssaultColony : LordToil
	{
		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06005131 RID: 20785 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x00038E33 File Offset: 0x00037033
		public LordToil_AssaultColony(bool attackDownedIfStarving = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06005133 RID: 20787 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x00038E42 File Offset: 0x00037042
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x001BAA70 File Offset: 0x001B8C70
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				this.lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
			}
		}

		// Token: 0x0400342E RID: 13358
		private bool attackDownedIfStarving;
	}
}
