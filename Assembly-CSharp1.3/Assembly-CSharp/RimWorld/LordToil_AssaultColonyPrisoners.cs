using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008A6 RID: 2214
	public class LordToil_AssaultColonyPrisoners : LordToil
	{
		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06003A9D RID: 15005 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003A9F RID: 15007 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x001477E3 File Offset: 0x001459E3
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x00148130 File Offset: 0x00146330
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrisonerAssaultColony);
			}
		}
	}
}
