using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B3 RID: 947
	public static class DebugOutputsTerrain
	{
		// Token: 0x06001D56 RID: 7510 RVA: 0x000B6B04 File Offset: 0x000B4D04
		[DebugOutput]
		public static void Terrains()
		{
			IEnumerable<TerrainDef> allDefs = DefDatabase<TerrainDef>.AllDefs;
			TableDataGetter<TerrainDef>[] array = new TableDataGetter<TerrainDef>[16];
			array[0] = new TableDataGetter<TerrainDef>("defName", (TerrainDef d) => d.defName);
			array[1] = new TableDataGetter<TerrainDef>("work", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.WorkToBuild, null).ToString());
			array[2] = new TableDataGetter<TerrainDef>("beauty", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString());
			array[3] = new TableDataGetter<TerrainDef>("cleanliness", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Cleanliness, null).ToString());
			array[4] = new TableDataGetter<TerrainDef>("flammability", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Flammability, null).ToString());
			array[5] = new TableDataGetter<TerrainDef>("path\ncost", (TerrainDef d) => d.pathCost.ToString());
			array[6] = new TableDataGetter<TerrainDef>("fertility", (TerrainDef d) => d.fertility.ToStringPercentEmptyZero("F0"));
			array[7] = new TableDataGetter<TerrainDef>("acceptance\nmask", (TerrainDef d) => string.Join(",", (from e in d.filthAcceptanceMask.GetAllSelectedItems<FilthSourceFlags>()
			select e.ToString()).ToArray<string>()));
			array[8] = new TableDataGetter<TerrainDef>("generated\nfilth", delegate(TerrainDef d)
			{
				if (d.generatedFilth == null)
				{
					return "";
				}
				return d.generatedFilth.defName;
			});
			array[9] = new TableDataGetter<TerrainDef>("hold\nsnow", (TerrainDef d) => d.holdSnow.ToStringCheckBlank());
			array[10] = new TableDataGetter<TerrainDef>("take\nfootprints", (TerrainDef d) => d.takeFootprints.ToStringCheckBlank());
			array[11] = new TableDataGetter<TerrainDef>("avoid\nwander", (TerrainDef d) => d.avoidWander.ToStringCheckBlank());
			array[12] = new TableDataGetter<TerrainDef>("buildable", (TerrainDef d) => d.BuildableByPlayer.ToStringCheckBlank());
			array[13] = new TableDataGetter<TerrainDef>("cost\nlist", (TerrainDef d) => DebugOutputsEconomy.CostListString(d, false, false));
			array[14] = new TableDataGetter<TerrainDef>("research", delegate(TerrainDef d)
			{
				if (d.researchPrerequisites == null)
				{
					return "";
				}
				return (from pr in d.researchPrerequisites
				select pr.defName).ToCommaList(false, false);
			});
			array[15] = new TableDataGetter<TerrainDef>("affordances", (TerrainDef d) => (from af in d.affordances
			select af.defName).ToCommaList(false, false));
			DebugTables.MakeTablesDialog<TerrainDef>(allDefs, array);
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x000B6DEC File Offset: 0x000B4FEC
		[DebugOutput]
		public static void TerrainAffordances()
		{
			IEnumerable<BuildableDef> dataSources = (from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && !d.IsFrame
			select d).Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>());
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[3];
			array[0] = new TableDataGetter<BuildableDef>("type", delegate(BuildableDef d)
			{
				if (!(d is TerrainDef))
				{
					return "building";
				}
				return "terrain";
			});
			array[1] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[2] = new TableDataGetter<BuildableDef>("terrain\naffordance\nneeded", delegate(BuildableDef d)
			{
				if (d.terrainAffordanceNeeded == null)
				{
					return "";
				}
				return d.terrainAffordanceNeeded.defName;
			});
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}
	}
}
