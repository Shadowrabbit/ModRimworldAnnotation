using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141F RID: 5151
	public static class PawnWeaponGenerator
	{
		// Token: 0x06006F31 RID: 28465 RVA: 0x00221400 File Offset: 0x0021F600
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

		// Token: 0x06006F32 RID: 28466 RVA: 0x0022156C File Offset: 0x0021F76C
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
				if (w.Price <= randomInRange && (pawn.kindDef.weaponTags == null || pawn.kindDef.weaponTags.Any((string tag) => w.thing.weaponTags.Contains(tag))) && (w.thing.generateAllowChance >= 1f || Rand.ChanceSeeded(w.thing.generateAllowChance, pawn.thingIDNumber ^ (int)w.thing.shortHash ^ 28554824)))
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
			if (PawnWeaponGenerator.workingWeapons.TryRandomElementByWeight((ThingStuffPair w) => w.Commonality * w.Price, out thingStuffPair))
			{
				ThingWithComps thingWithComps = (ThingWithComps)ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
				PawnGenerator.PostProcessGeneratedGear(thingWithComps, pawn);
				float num = (request.BiocodeWeaponChance > 0f) ? request.BiocodeWeaponChance : pawn.kindDef.biocodeWeaponChance;
				if (Rand.Value < num)
				{
					CompBiocodableWeapon compBiocodableWeapon = thingWithComps.TryGetComp<CompBiocodableWeapon>();
					if (compBiocodableWeapon != null)
					{
						compBiocodableWeapon.CodeFor(pawn);
					}
				}
				pawn.equipment.AddEquipment(thingWithComps);
			}
			PawnWeaponGenerator.workingWeapons.Clear();
		}

		// Token: 0x06006F33 RID: 28467 RVA: 0x0022176C File Offset: 0x0021F96C
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

		// Token: 0x06006F34 RID: 28468 RVA: 0x002217CC File Offset: 0x0021F9CC
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

		// Token: 0x06006F35 RID: 28469 RVA: 0x00221830 File Offset: 0x0021FA30
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

		// Token: 0x06006F36 RID: 28470 RVA: 0x0004B364 File Offset: 0x00049564
		[DebugOutput]
		private static void WeaponPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnWeaponGenerator.allWeaponPairs);
		}

		// Token: 0x04004970 RID: 18800
		private static List<ThingStuffPair> allWeaponPairs;

		// Token: 0x04004971 RID: 18801
		private static List<ThingStuffPair> workingWeapons = new List<ThingStuffPair>();
	}
}
