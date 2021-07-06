using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200170F RID: 5903
	public class Building_SteamGeyser : Building
	{
		// Token: 0x060081F2 RID: 33266 RVA: 0x00268354 File Offset: 0x00266554
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this);
			this.steamSprayer.startSprayCallback = new Action(this.StartSpray);
			this.steamSprayer.endSprayCallback = new Action(this.EndSpray);
		}

		// Token: 0x060081F3 RID: 33267 RVA: 0x002683A4 File Offset: 0x002665A4
		private void StartSpray()
		{
			SnowUtility.AddSnowRadial(this.OccupiedRect().RandomCell, base.Map, 4f, -0.06f);
			this.spraySustainer = SoundDefOf.GeyserSpray.TrySpawnSustainer(new TargetInfo(base.Position, base.Map, false));
			this.spraySustainerStartTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060081F4 RID: 33268 RVA: 0x00057484 File Offset: 0x00055684
		private void EndSpray()
		{
			if (this.spraySustainer != null)
			{
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x060081F5 RID: 33269 RVA: 0x0026840C File Offset: 0x0026660C
		public override void Tick()
		{
			if (this.harvester == null)
			{
				this.steamSprayer.SteamSprayerTick();
			}
			if (this.spraySustainer != null && Find.TickManager.TicksGame > this.spraySustainerStartTick + 1000)
			{
				Log.Message("Geyser spray sustainer still playing after 1000 ticks. Force-ending.", false);
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x0400544C RID: 21580
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x0400544D RID: 21581
		public Building harvester;

		// Token: 0x0400544E RID: 21582
		private Sustainer spraySustainer;

		// Token: 0x0400544F RID: 21583
		private int spraySustainerStartTick = -999;
	}
}
