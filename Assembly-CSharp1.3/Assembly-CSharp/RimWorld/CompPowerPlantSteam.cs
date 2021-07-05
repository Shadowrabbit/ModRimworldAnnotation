using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CCD RID: 3277
	public class CompPowerPlantSteam : CompPowerPlant
	{
		// Token: 0x06004C60 RID: 19552 RVA: 0x00197203 File Offset: 0x00195403
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this.parent);
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x00197220 File Offset: 0x00195420
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

		// Token: 0x06004C62 RID: 19554 RVA: 0x00197294 File Offset: 0x00195494
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.geyser != null)
			{
				this.geyser.harvester = null;
			}
		}

		// Token: 0x04002E2B RID: 11819
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04002E2C RID: 11820
		private Building_SteamGeyser geyser;
	}
}
