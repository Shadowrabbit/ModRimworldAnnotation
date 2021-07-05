using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E4 RID: 2532
	public static class DefGenerator
	{
		// Token: 0x06003E76 RID: 15990 RVA: 0x00155664 File Offset: 0x00153864
		public static void GenerateImpliedDefs_PreResolve()
		{
			foreach (ThingDef def in ThingDefGenerator_Buildings.ImpliedBlueprintAndFrameDefs().Concat(ThingDefGenerator_Meat.ImpliedMeatDefs()).Concat(ThingDefGenerator_Techprints.ImpliedTechprintDefs()).Concat(ThingDefGenerator_Corpses.ImpliedCorpseDefs()))
			{
				DefGenerator.AddImpliedDef<ThingDef>(def);
			}
			DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.Silent);
			foreach (TerrainDef def2 in TerrainDefGenerator_Stone.ImpliedTerrainDefs())
			{
				DefGenerator.AddImpliedDef<TerrainDef>(def2);
			}
			foreach (RecipeDef def3 in RecipeDefGenerator.ImpliedRecipeDefs())
			{
				DefGenerator.AddImpliedDef<RecipeDef>(def3);
			}
			foreach (PawnColumnDef def4 in PawnColumnDefgenerator.ImpliedPawnColumnDefs())
			{
				DefGenerator.AddImpliedDef<PawnColumnDef>(def4);
			}
			foreach (ThingDef def5 in ThingDefGenerator_Neurotrainer.ImpliedThingDefs())
			{
				DefGenerator.AddImpliedDef<ThingDef>(def5);
			}
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x001557B4 File Offset: 0x001539B4
		public static void GenerateImpliedDefs_PostResolve()
		{
			foreach (KeyBindingCategoryDef def in KeyBindingDefGenerator.ImpliedKeyBindingCategoryDefs())
			{
				DefGenerator.AddImpliedDef<KeyBindingCategoryDef>(def);
			}
			foreach (KeyBindingDef def2 in KeyBindingDefGenerator.ImpliedKeyBindingDefs())
			{
				DefGenerator.AddImpliedDef<KeyBindingDef>(def2);
			}
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00155838 File Offset: 0x00153A38
		public static void AddImpliedDef<T>(T def) where T : Def, new()
		{
			def.generated = true;
			ModContentPack modContentPack = def.modContentPack;
			if (modContentPack != null)
			{
				modContentPack.AddDef(def, "ImpliedDefs");
			}
			def.PostLoad();
			DefDatabase<T>.Add(def);
		}

		// Token: 0x040020EE RID: 8430
		public static int StandardItemPathCost = 14;
	}
}
