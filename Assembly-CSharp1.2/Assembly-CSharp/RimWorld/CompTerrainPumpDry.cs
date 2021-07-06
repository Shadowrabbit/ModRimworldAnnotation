using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200187B RID: 6267
	public class CompTerrainPumpDry : CompTerrainPump
	{
		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x06008B08 RID: 35592 RVA: 0x0005D414 File Offset: 0x0005B614
		private CompProperties_TerrainPumpDry Props
		{
			get
			{
				return (CompProperties_TerrainPumpDry)this.props;
			}
		}

		// Token: 0x06008B09 RID: 35593 RVA: 0x0005D421 File Offset: 0x0005B621
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(this.parent.Map, c);
		}

		// Token: 0x06008B0A RID: 35594 RVA: 0x002886F4 File Offset: 0x002868F4
		public static void AffectCell(Map map, IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(map);
			TerrainDef terrainToDryTo = CompTerrainPumpDry.GetTerrainToDryTo(map, terrain);
			if (terrainToDryTo != null)
			{
				map.terrainGrid.SetTerrain(c, terrainToDryTo);
			}
			TerrainDef terrainDef = map.terrainGrid.UnderTerrainAt(c);
			if (terrainDef != null)
			{
				TerrainDef terrainToDryTo2 = CompTerrainPumpDry.GetTerrainToDryTo(map, terrainDef);
				if (terrainToDryTo2 != null)
				{
					map.terrainGrid.SetUnderTerrain(c, terrainToDryTo2);
				}
			}
		}

		// Token: 0x06008B0B RID: 35595 RVA: 0x0005D434 File Offset: 0x0005B634
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06008B0C RID: 35596 RVA: 0x0028874C File Offset: 0x0028694C
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (!this.Props.soundWorking.NullOrUndefined() && base.Working && base.CurrentRadius < this.Props.radius - 0.0001f)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = this.Props.soundWorking.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				this.sustainer.Maintain();
				return;
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06008B0D RID: 35597 RVA: 0x0005D45D File Offset: 0x0005B65D
		private static TerrainDef GetTerrainToDryTo(Map map, TerrainDef terrainDef)
		{
			if (terrainDef.driesTo == null)
			{
				return null;
			}
			if (map.Biome == BiomeDefOf.SeaIce)
			{
				return TerrainDefOf.Ice;
			}
			return terrainDef.driesTo;
		}

		// Token: 0x0400591F RID: 22815
		private Sustainer sustainer;
	}
}
