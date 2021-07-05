using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008A3 RID: 2211
	public class LordToil_AssaultColony : LordToil
	{
		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06003A83 RID: 14979 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x001477CD File Offset: 0x001459CD
		public LordToil_AssaultColony(bool attackDownedIfStarving = false, bool canPickUpOpportunisticWeapons = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
			this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06003A85 RID: 14981 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x001477E3 File Offset: 0x001459E3
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x001477F8 File Offset: 0x001459F8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				this.lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
				this.lord.ownedPawns[i].mindState.duty.pickupOpportunisticWeapon = this.canPickUpOpportunisticWeapons;
			}
		}

		// Token: 0x04001FFD RID: 8189
		private bool attackDownedIfStarving;

		// Token: 0x04001FFE RID: 8190
		private bool canPickUpOpportunisticWeapons;
	}
}
