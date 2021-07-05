using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010B1 RID: 4273
	public class Building_SteamGeyser : Building
	{
		// Token: 0x06006613 RID: 26131 RVA: 0x00227678 File Offset: 0x00225878
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this);
			this.steamSprayer.startSprayCallback = new Action(this.StartSpray);
			this.steamSprayer.endSprayCallback = new Action(this.EndSpray);
		}

		// Token: 0x06006614 RID: 26132 RVA: 0x002276C8 File Offset: 0x002258C8
		private void StartSpray()
		{
			SnowUtility.AddSnowRadial(this.OccupiedRect().RandomCell, base.Map, 4f, -0.06f);
			this.spraySustainer = SoundDefOf.GeyserSpray.TrySpawnSustainer(new TargetInfo(base.Position, base.Map, false));
			this.spraySustainerStartTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06006615 RID: 26133 RVA: 0x0022772F File Offset: 0x0022592F
		private void EndSpray()
		{
			if (this.spraySustainer != null)
			{
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x06006616 RID: 26134 RVA: 0x0022774C File Offset: 0x0022594C
		public override void Tick()
		{
			if (this.harvester == null)
			{
				this.steamSprayer.SteamSprayerTick();
			}
			if (this.spraySustainer != null && Find.TickManager.TicksGame > this.spraySustainerStartTick + 1000)
			{
				Log.Message("Geyser spray sustainer still playing after 1000 ticks. Force-ending.");
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x04003999 RID: 14745
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x0400399A RID: 14746
		public Building harvester;

		// Token: 0x0400399B RID: 14747
		private Sustainer spraySustainer;

		// Token: 0x0400399C RID: 14748
		private int spraySustainerStartTick = -999;
	}
}
