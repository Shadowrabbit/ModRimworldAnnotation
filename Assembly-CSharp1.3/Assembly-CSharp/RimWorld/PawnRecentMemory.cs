using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5C RID: 3676
	public class PawnRecentMemory : IExposable
	{
		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x0600550D RID: 21773 RVA: 0x001CCD94 File Offset: 0x001CAF94
		public int TicksSinceLastLight
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastLightTick;
			}
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x0600550E RID: 21774 RVA: 0x001CCDA7 File Offset: 0x001CAFA7
		public int TicksSinceOutdoors
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastOutdoorTick;
			}
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x001CCDBA File Offset: 0x001CAFBA
		public PawnRecentMemory(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x001CCDDF File Offset: 0x001CAFDF
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLightTick, "lastLightTick", 999999, false);
			Scribe_Values.Look<int>(ref this.lastOutdoorTick, "lastOutdoorTick", 999999, false);
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x001CCE10 File Offset: 0x001CB010
		public void RecentMemoryInterval()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (this.pawn.Map.glowGrid.PsychGlowAt(this.pawn.Position) != PsychGlow.Dark)
			{
				this.lastLightTick = Find.TickManager.TicksGame;
			}
			if (this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x001CCE78 File Offset: 0x001CB078
		private bool Outdoors()
		{
			Room room = this.pawn.GetRoom(RegionType.Set_All);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x001CCE9E File Offset: 0x001CB09E
		public void Notify_Spawned(bool respawningAfterLoad)
		{
			this.lastLightTick = Find.TickManager.TicksGame;
			if (!respawningAfterLoad && this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04003264 RID: 12900
		private Pawn pawn;

		// Token: 0x04003265 RID: 12901
		private int lastLightTick = 999999;

		// Token: 0x04003266 RID: 12902
		private int lastOutdoorTick = 999999;
	}
}
