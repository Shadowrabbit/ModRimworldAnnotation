using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE1 RID: 3809
	public static class DefGenerator
	{
		// Token: 0x06005441 RID: 21569 RVA: 0x001C30D0 File Offset: 0x001C12D0
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
			foreach (ThingDef def5 in NeurotrainerDefGenerator.ImpliedThingDefs())
			{
				DefGenerator.AddImpliedDef<ThingDef>(def5);
			}
		}

		// Token: 0x06005442 RID: 21570 RVA: 0x001C3220 File Offset: 0x001C1420
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

		// Token: 0x06005443 RID: 21571 RVA: 0x0003A811 File Offset: 0x00038A11
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

		// Token: 0x04003527 RID: 13607
		public static int StandardItemPathCost = 14;
	}
}
