using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC1 RID: 3521
	public static class PawnWeaponGenerator
	{
		// Token: 0x0600517A RID: 20858 RVA: 0x001B6E94 File Offset: 0x001B5094
		public static void Reset()
		{
			Predicate<ThingDef> isWeapon = (ThingDef td) => td.equipmentType == EquipmentType.Primary && !td.weaponTags.NullOrEmpty<string>();
			PawnWeaponGenerator.allWeaponPairs = ThingStuffPair.AllWith(isWeapon);
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			Func<ThingDef, bool> <>9__1;
			Func<ThingDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((ThingDef td) => isWeapon(td)));
			}
			using (IEnumerator<ThingDef> enumerator = allDefs.Where(predicate).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef thingDef = enumerator.Current;
					float num = (from pa in PawnWeaponGenerator.allWeaponPairs
					where pa.thing == thingDef
					select pa).Sum((ThingStuffPair pa) => pa.Commonality);
					float num2 = thingDef.generateCommonality / num;
					if (num2 != 1f)
					{
						for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
						{
							ThingStuffPair thingStuffPair = PawnWeaponGenerator.allWeaponPairs[i];
							if (thingStuffPair.thing == thingDef)
							{
								PawnWeaponGenerator.allWeaponPairs[i] = new ThingStuffPair(thingStuffPair.thing, thingStuffPair.stuff, thingStuffPair.commonalityMultiplier * num2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x001B7000 File Offset: 0x001B5200
		public static void TryGenerateWeaponFor(Pawn pawn, PawnGenerationRequest request)
		{
			PawnWeaponGenerator.workingWeapons.Clear();
			if (pawn.kindDef.weaponTags == null || pawn.kindDef.weaponTags.Count == 0)
			{
				return;
			}
			if (!pawn.RaceProps.ToolUser)
			{
				return;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return;
			}
			float randomInRange = pawn.kindDef.weaponMoney.RandomInRange;
			for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
			{
				ThingStuffPair w = PawnWeaponGenerator.allWeaponPairs[i];
				if (w.Price <= randomInRange && (pawn.kindDef.weaponTags == null || pawn.kindDef.weaponTags.Any((string tag) => w.thing.weaponTags.Contains(tag))) && (pawn.kindDef.weaponStuffOverride == null || w.stuff == pawn.kindDef.weaponStuffOverride) && (!w.thing.IsRangedWeapon || !pawn.WorkTagIsDisabled(WorkTags.Shooting)) && (w.thing.generateAllowChance >= 1f || Rand.ChanceSeeded(w.thing.generateAllowChance, pawn.thingIDNumber ^ (int)w.thing.shortHash ^ 28554824)))
				{
					PawnWeaponGenerator.workingWeapons.Add(w);
				}
			}
			if (PawnWeaponGenerator.workingWeapons.Count == 0)
			{
				return;
			}
			pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			ThingStuffPair thingStuffPair;
			if (PawnWeaponGenerator.workingWeapons.TryRandomElementByWeight((ThingStuffPair w) => w.Commonality * w.Price * PawnWeaponGenerator.GetWeaponCommonalityFromIdeo(pawn, w), out thingStuffPair))
			{
				ThingWithComps thingWithComps = (ThingWithComps)ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
				PawnGenerator.PostProcessGeneratedGear(thingWithComps, pawn);
				CompEquippable compEquippable = thingWithComps.TryGetComp<CompEquippable>();
				if (compEquippable != null)
				{
					if (pawn.kindDef.weaponStyleDef != null)
					{
						compEquippable.parent.StyleDef = pawn.kindDef.weaponStyleDef;
					}
					else if (pawn.Ideo != null)
					{
						compEquippable.parent.StyleDef = pawn.Ideo.GetStyleFor(thingWithComps.def);
					}
				}
				float num = (request.BiocodeWeaponChance > 0f) ? request.BiocodeWeaponChance : pawn.kindDef.biocodeWeaponChance;
				if (Rand.Value < num)
				{
					CompBiocodable compBiocodable = thingWithComps.TryGetComp<CompBiocodable>();
					if (compBiocodable != null)
					{
						compBiocodable.CodeFor(pawn);
					}
				}
				pawn.equipment.AddEquipment(thingWithComps);
			}
			PawnWeaponGenerator.workingWeapons.Clear();
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x001B7314 File Offset: 0x001B5514
		private static float GetWeaponCommonalityFromIdeo(Pawn pawn, ThingStuffPair pair)
		{
			if (pawn.Ideo == null)
			{
				return 1f;
			}
			switch (pawn.Ideo.GetDispositionForWeapon(pair.thing))
			{
			case IdeoWeaponDisposition.Noble:
				return 100f;
			case IdeoWeaponDisposition.Despised:
				return 0.001f;
			}
			return 1f;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x001B7368 File Offset: 0x001B5568
		public static bool IsDerpWeapon(ThingDef thing, ThingDef stuff)
		{
			if (stuff == null)
			{
				return false;
			}
			if (thing.IsMeleeWeapon)
			{
				if (thing.tools.NullOrEmpty<Tool>())
				{
					return false;
				}
				DamageDef damageDef = ThingUtility.PrimaryMeleeWeaponDamageType(thing);
				if (damageDef == null)
				{
					return false;
				}
				DamageArmorCategoryDef armorCategory = damageDef.armorCategory;
				if (armorCategory != null && armorCategory.multStat != null && stuff.GetStatValueAbstract(armorCategory.multStat, null) < 0.7f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x001B73C8 File Offset: 0x001B55C8
		public static float CheapestNonDerpPriceFor(ThingDef weaponDef)
		{
			float num = 9999999f;
			for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
			{
				ThingStuffPair thingStuffPair = PawnWeaponGenerator.allWeaponPairs[i];
				if (thingStuffPair.thing == weaponDef && !PawnWeaponGenerator.IsDerpWeapon(thingStuffPair.thing, thingStuffPair.stuff) && thingStuffPair.Price < num)
				{
					num = thingStuffPair.Price;
				}
			}
			return num;
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x001B742C File Offset: 0x001B562C
		[DebugOutput]
		private static void WeaponPairs()
		{
			IEnumerable<ThingStuffPair> dataSources = from p in PawnWeaponGenerator.allWeaponPairs
			orderby p.thing.defName descending
			select p;
			TableDataGetter<ThingStuffPair>[] array = new TableDataGetter<ThingStuffPair>[7];
			array[0] = new TableDataGetter<ThingStuffPair>("thing", (ThingStuffPair p) => p.thing.defName);
			array[1] = new TableDataGetter<ThingStuffPair>("stuff", delegate(ThingStuffPair p)
			{
				if (p.stuff == null)
				{
					return "";
				}
				return p.stuff.defName;
			});
			array[2] = new TableDataGetter<ThingStuffPair>("price", (ThingStuffPair p) => p.Price.ToString());
			array[3] = new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => p.Commonality.ToString("F5"));
			array[4] = new TableDataGetter<ThingStuffPair>("commMult", (ThingStuffPair p) => p.commonalityMultiplier.ToString("F5"));
			array[5] = new TableDataGetter<ThingStuffPair>("generateCommonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F2"));
			array[6] = new TableDataGetter<ThingStuffPair>("derp", delegate(ThingStuffPair p)
			{
				if (!PawnWeaponGenerator.IsDerpWeapon(p.thing, p.stuff))
				{
					return "";
				}
				return "D";
			});
			DebugTables.MakeTablesDialog<ThingStuffPair>(dataSources, array);
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x001B75A1 File Offset: 0x001B57A1
		[DebugOutput]
		private static void WeaponPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnWeaponGenerator.allWeaponPairs);
		}

		// Token: 0x04003049 RID: 12361
		private static List<ThingStuffPair> allWeaponPairs;

		// Token: 0x0400304A RID: 12362
		private static List<ThingStuffPair> workingWeapons = new List<ThingStuffPair>();

		// Token: 0x0400304B RID: 12363
		private const float WeaponSelectFactor_NobleByIdeo = 100f;

		// Token: 0x0400304C RID: 12364
		private const float WeaponSelectFactor_DespisedByIdeo = 0.001f;
	}
}
