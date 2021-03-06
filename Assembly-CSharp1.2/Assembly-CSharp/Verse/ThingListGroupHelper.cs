using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200027B RID: 635
	public static class ThingListGroupHelper
	{
		// Token: 0x0600105F RID: 4191 RVA: 0x000B9354 File Offset: 0x000B7554
		static ThingListGroupHelper()
		{
			int num = 0;
			foreach (object obj in Enum.GetValues(typeof(ThingRequestGroup)))
			{
				ThingListGroupHelper.AllGroups[num] = (ThingRequestGroup)obj;
				num++;
			}
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x000B93DC File Offset: 0x000B75DC
		public static bool Includes(this ThingRequestGroup group, ThingDef def)
		{
			switch (group)
			{
			case ThingRequestGroup.Undefined:
				return false;
			case ThingRequestGroup.Nothing:
				return false;
			case ThingRequestGroup.Everything:
				return true;
			case ThingRequestGroup.HaulableEver:
				return def.EverHaulable;
			case ThingRequestGroup.HaulableAlways:
				return def.alwaysHaulable;
			case ThingRequestGroup.FoodSource:
				return def.IsNutritionGivingIngestible || def.thingClass == typeof(Building_NutrientPasteDispenser);
			case ThingRequestGroup.FoodSourceNotPlantOrTree:
				return (def.IsNutritionGivingIngestible && (def.ingestible.foodType & ~FoodTypeFlags.Plant & ~FoodTypeFlags.Tree) != FoodTypeFlags.None) || def.thingClass == typeof(Building_NutrientPasteDispenser);
			case ThingRequestGroup.Corpse:
				return def.thingClass == typeof(Corpse);
			case ThingRequestGroup.Blueprint:
				return def.IsBlueprint;
			case ThingRequestGroup.BuildingArtificial:
				return def.IsBuildingArtificial;
			case ThingRequestGroup.BuildingFrame:
				return def.IsFrame;
			case ThingRequestGroup.Pawn:
				return def.category == ThingCategory.Pawn;
			case ThingRequestGroup.PotentialBillGiver:
				return !def.AllRecipes.NullOrEmpty<RecipeDef>();
			case ThingRequestGroup.Medicine:
				return def.IsMedicine;
			case ThingRequestGroup.Filth:
				return def.filth != null;
			case ThingRequestGroup.AttackTarget:
				return typeof(IAttackTarget).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Weapon:
				return def.IsWeapon;
			case ThingRequestGroup.Refuelable:
				return def.HasComp(typeof(CompRefuelable));
			case ThingRequestGroup.HaulableEverOrMinifiable:
				return def.EverHaulable || def.Minifiable;
			case ThingRequestGroup.Drug:
				return def.IsDrug;
			case ThingRequestGroup.Shell:
				return def.IsShell;
			case ThingRequestGroup.HarvestablePlant:
				return def.category == ThingCategory.Plant && def.plant.Harvestable;
			case ThingRequestGroup.Fire:
				return typeof(Fire).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Bed:
				return def.IsBed;
			case ThingRequestGroup.Plant:
				return def.category == ThingCategory.Plant;
			case ThingRequestGroup.Construction:
				return def.IsBlueprint || def.IsFrame;
			case ThingRequestGroup.HasGUIOverlay:
				return def.drawGUIOverlay;
			case ThingRequestGroup.Apparel:
				return def.IsApparel;
			case ThingRequestGroup.MinifiedThing:
				return typeof(MinifiedThing).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Grave:
				return typeof(Building_Grave).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Art:
				return def.HasComp(typeof(CompArt));
			case ThingRequestGroup.ThingHolder:
				return def.ThisOrAnyCompIsThingHolder();
			case ThingRequestGroup.ActiveDropPod:
				return typeof(IActiveDropPod).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Transporter:
				return def.HasComp(typeof(CompTransporter));
			case ThingRequestGroup.LongRangeMineralScanner:
				return def.HasComp(typeof(CompLongRangeMineralScanner));
			case ThingRequestGroup.AffectsSky:
				return def.HasComp(typeof(CompAffectsSky));
			case ThingRequestGroup.WindSource:
				return def.HasComp(typeof(CompWindSource));
			case ThingRequestGroup.AlwaysFlee:
				return def.alwaysFlee;
			case ThingRequestGroup.ResearchBench:
				return typeof(Building_ResearchBench).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Facility:
				return def.HasComp(typeof(CompFacility));
			case ThingRequestGroup.AffectedByFacilities:
				return def.HasComp(typeof(CompAffectedByFacilities));
			case ThingRequestGroup.CreatesInfestations:
				return def.HasComp(typeof(CompCreatesInfestations));
			case ThingRequestGroup.WithCustomRectForSelector:
				return def.hasCustomRectForSelector;
			case ThingRequestGroup.ProjectileInterceptor:
				return def.HasComp(typeof(CompProjectileInterceptor));
			case ThingRequestGroup.ConditionCauser:
				return def.GetCompProperties<CompProperties_CausesGameCondition>() != null;
			case ThingRequestGroup.MusicalInstrument:
				return typeof(Building_MusicalInstrument).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Throne:
				return typeof(Building_Throne).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.FoodDispenser:
				return def.IsFoodDispenser;
			case ThingRequestGroup.Projectile:
				return def.projectile != null;
			case ThingRequestGroup.Chunk:
				return !def.thingCategories.NullOrEmpty<ThingCategoryDef>() && (def.thingCategories.Contains(ThingCategoryDefOf.Chunks) || def.thingCategories.Contains(ThingCategoryDefOf.StoneChunks));
			case ThingRequestGroup.MeditationFocus:
				return def.HasComp(typeof(CompMeditationFocus));
			default:
				throw new ArgumentException("group");
			}
		}

		// Token: 0x04000D2F RID: 3375
		public static readonly ThingRequestGroup[] AllGroups = new ThingRequestGroup[Enum.GetValues(typeof(ThingRequestGroup)).Length];
	}
}
