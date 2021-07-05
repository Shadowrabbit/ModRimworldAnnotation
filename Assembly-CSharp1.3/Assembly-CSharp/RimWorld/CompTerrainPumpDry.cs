using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011B7 RID: 4535
	public class CompTerrainPumpDry : CompTerrainPump
	{
		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x06006D40 RID: 27968 RVA: 0x00249FC2 File Offset: 0x002481C2
		private CompProperties_TerrainPumpDry Props
		{
			get
			{
				return (CompProperties_TerrainPumpDry)this.props;
			}
		}

		// Token: 0x06006D41 RID: 27969 RVA: 0x00249FCF File Offset: 0x002481CF
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(this.parent.Map, c);
		}

		// Token: 0x06006D42 RID: 27970 RVA: 0x00249FE4 File Offset: 0x002481E4
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

		// Token: 0x06006D43 RID: 27971 RVA: 0x0024A039 File Offset: 0x00248239
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06006D44 RID: 27972 RVA: 0x0024A064 File Offset: 0x00248264
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

		// Token: 0x06006D45 RID: 27973 RVA: 0x0024A112 File Offset: 0x00248312
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

		// Token: 0x04003CB1 RID: 15537
		private Sustainer sustainer;
	}
}
