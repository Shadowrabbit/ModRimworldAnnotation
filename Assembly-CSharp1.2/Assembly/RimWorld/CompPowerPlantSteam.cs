using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E8 RID: 4840
	public class CompPowerPlantSteam : CompPowerPlant
	{
		// Token: 0x060068E0 RID: 26848 RVA: 0x0004775C File Offset: 0x0004595C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this.parent);
		}

		// Token: 0x060068E1 RID: 26849 RVA: 0x00204C94 File Offset: 0x00202E94
		public override void CompTick()
		{
			base.CompTick();
			if (this.geyser == null)
			{
				this.geyser = (Building_SteamGeyser)this.parent.Map.thingGrid.ThingAt(this.parent.Position, ThingDefOf.SteamGeyser);
			}
			if (this.geyser != null)
			{
				this.geyser.harvester = (Building)this.parent;
				this.steamSprayer.SteamSprayerTick();
			}
		}

		// Token: 0x060068E2 RID: 26850 RVA: 0x00047776 File Offset: 0x00045976
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.geyser != null)
			{
				this.geyser.harvester = null;
			}
		}

		// Token: 0x040045BE RID: 17854
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x040045BF RID: 17855
		private Building_SteamGeyser geyser;
	}
}
