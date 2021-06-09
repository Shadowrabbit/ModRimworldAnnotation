using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FB RID: 5371
	public class PawnRecentMemory : IExposable
	{
		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x060073A6 RID: 29606 RVA: 0x0004DE12 File Offset: 0x0004C012
		public int TicksSinceLastLight
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastLightTick;
			}
		}

		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x060073A7 RID: 29607 RVA: 0x0004DE25 File Offset: 0x0004C025
		public int TicksSinceOutdoors
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastOutdoorTick;
			}
		}

		// Token: 0x060073A8 RID: 29608 RVA: 0x0004DE38 File Offset: 0x0004C038
		public PawnRecentMemory(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060073A9 RID: 29609 RVA: 0x0004DE5D File Offset: 0x0004C05D
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLightTick, "lastLightTick", 999999, false);
			Scribe_Values.Look<int>(ref this.lastOutdoorTick, "lastOutdoorTick", 999999, false);
		}

		// Token: 0x060073AA RID: 29610 RVA: 0x00234D58 File Offset: 0x00232F58
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

		// Token: 0x060073AB RID: 29611 RVA: 0x00234DC0 File Offset: 0x00232FC0
		private bool Outdoors()
		{
			Room room = this.pawn.GetRoom(RegionType.Set_Passable);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x060073AC RID: 29612 RVA: 0x0004DE8B File Offset: 0x0004C08B
		public void Notify_Spawned(bool respawningAfterLoad)
		{
			this.lastLightTick = Find.TickManager.TicksGame;
			if (!respawningAfterLoad && this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04004C6C RID: 19564
		private Pawn pawn;

		// Token: 0x04004C6D RID: 19565
		private int lastLightTick = 999999;

		// Token: 0x04004C6E RID: 19566
		private int lastOutdoorTick = 999999;
	}
}
