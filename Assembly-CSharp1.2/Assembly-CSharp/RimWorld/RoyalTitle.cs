using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200151A RID: 5402
	public class RoyalTitle : IExposable
	{
		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x060074BB RID: 29883 RVA: 0x0004ED35 File Offset: 0x0004CF35
		public string Label
		{
			get
			{
				return this.def.GetLabelFor(this.pawn);
			}
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x060074BC RID: 29884 RVA: 0x0004ED48 File Offset: 0x0004CF48
		public float RoomRequirementGracePeriodDaysLeft
		{
			get
			{
				return Mathf.Max((180000 - (GenTicks.TicksGame - this.receivedTick)).TicksToDays(), 0f);
			}
		}

		// Token: 0x060074BD RID: 29885 RVA: 0x0004ED6B File Offset: 0x0004CF6B
		public bool RoomRequirementGracePeriodActive(Pawn pawn)
		{
			return GenTicks.TicksGame - this.receivedTick < 180000 && !pawn.IsQuestLodger();
		}

		// Token: 0x060074BE RID: 29886 RVA: 0x0004ED8B File Offset: 0x0004CF8B
		public RoyalTitle()
		{
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x0004ED9A File Offset: 0x0004CF9A
		public RoyalTitle(RoyalTitle other)
		{
			this.faction = other.faction;
			this.def = other.def;
			this.receivedTick = other.receivedTick;
			this.pawn = other.pawn;
		}

		// Token: 0x060074C0 RID: 29888 RVA: 0x00238C4C File Offset: 0x00236E4C
		public void RoyalTitleTick_NewTemp()
		{
			if (this.pawn.IsHashIntervalTick(833) && this.conceited && this.pawn.Spawned && this.pawn.IsFreeColonist && (!this.pawn.IsQuestLodger() || this.pawn.LodgerAllowedDecrees()) && this.def.decreeMtbDays > 0f && this.pawn.Awake() && Rand.MTBEventOccurs(this.def.decreeMtbDays, 60000f, 833f) && (float)(Find.TickManager.TicksGame - this.pawn.royalty.lastDecreeTicks) >= this.def.decreeMinIntervalDays * 60000f)
			{
				this.pawn.royalty.IssueDecree(false, null);
			}
		}

		// Token: 0x060074C1 RID: 29889 RVA: 0x0004EDD9 File Offset: 0x0004CFD9
		[Obsolete("Will be removed in the future")]
		public void RoyalTitleTick(Pawn pawn)
		{
			this.RoyalTitleTick_NewTemp();
		}

		// Token: 0x060074C2 RID: 29890 RVA: 0x00238D30 File Offset: 0x00236F30
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.receivedTick, "receivedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.wasInherited, "wasInherited", false, false);
			Scribe_Values.Look<bool>(ref this.conceited, "conceited", false, false);
		}

		// Token: 0x04004D04 RID: 19716
		public Faction faction;

		// Token: 0x04004D05 RID: 19717
		public RoyalTitleDef def;

		// Token: 0x04004D06 RID: 19718
		public Pawn pawn;

		// Token: 0x04004D07 RID: 19719
		public int receivedTick = -1;

		// Token: 0x04004D08 RID: 19720
		public bool wasInherited;

		// Token: 0x04004D09 RID: 19721
		public bool conceited;

		// Token: 0x04004D0A RID: 19722
		private const int DecreeCheckInterval = 833;

		// Token: 0x04004D0B RID: 19723
		private const int RoomRequirementsGracePeriodTicks = 180000;
	}
}
