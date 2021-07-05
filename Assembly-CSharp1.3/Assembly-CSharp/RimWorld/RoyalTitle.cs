using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E75 RID: 3701
	public class RoyalTitle : IExposable
	{
		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06005649 RID: 22089 RVA: 0x001D3B68 File Offset: 0x001D1D68
		public string Label
		{
			get
			{
				return this.def.GetLabelFor(this.pawn);
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x0600564A RID: 22090 RVA: 0x001D3B7B File Offset: 0x001D1D7B
		public float RoomRequirementGracePeriodDaysLeft
		{
			get
			{
				return Mathf.Max((180000 - (GenTicks.TicksGame - this.receivedTick)).TicksToDays(), 0f);
			}
		}

		// Token: 0x0600564B RID: 22091 RVA: 0x001D3B9E File Offset: 0x001D1D9E
		public bool RoomRequirementGracePeriodActive(Pawn pawn)
		{
			return GenTicks.TicksGame - this.receivedTick < 180000 && !pawn.IsQuestLodger();
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x001D3BBE File Offset: 0x001D1DBE
		public RoyalTitle()
		{
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x001D3BCD File Offset: 0x001D1DCD
		public RoyalTitle(RoyalTitle other)
		{
			this.faction = other.faction;
			this.def = other.def;
			this.receivedTick = other.receivedTick;
			this.pawn = other.pawn;
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x001D3C0C File Offset: 0x001D1E0C
		public void RoyalTitleTick()
		{
			if (this.pawn.IsHashIntervalTick(833) && this.conceited && this.pawn.Spawned && this.pawn.IsFreeColonist && (!this.pawn.IsQuestLodger() || this.pawn.LodgerAllowedDecrees()) && this.def.decreeMtbDays > 0f && this.pawn.Awake() && Rand.MTBEventOccurs(this.def.decreeMtbDays, 60000f, 833f) && (float)(Find.TickManager.TicksGame - this.pawn.royalty.lastDecreeTicks) >= this.def.decreeMinIntervalDays * 60000f)
			{
				this.pawn.royalty.IssueDecree(false, null);
			}
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x001D3CF0 File Offset: 0x001D1EF0
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.receivedTick, "receivedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.wasInherited, "wasInherited", false, false);
			Scribe_Values.Look<bool>(ref this.conceited, "conceited", false, false);
		}

		// Token: 0x040032FF RID: 13055
		public Faction faction;

		// Token: 0x04003300 RID: 13056
		public RoyalTitleDef def;

		// Token: 0x04003301 RID: 13057
		public Pawn pawn;

		// Token: 0x04003302 RID: 13058
		public int receivedTick = -1;

		// Token: 0x04003303 RID: 13059
		public bool wasInherited;

		// Token: 0x04003304 RID: 13060
		public bool conceited;

		// Token: 0x04003305 RID: 13061
		private const int DecreeCheckInterval = 833;

		// Token: 0x04003306 RID: 13062
		private const int RoomRequirementsGracePeriodTicks = 180000;
	}
}
