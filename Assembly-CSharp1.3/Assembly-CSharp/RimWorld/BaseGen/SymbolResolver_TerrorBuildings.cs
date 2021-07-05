using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015F1 RID: 5617
	public class SymbolResolver_TerrorBuildings : SymbolResolver
	{
		// Token: 0x060083CA RID: 33738 RVA: 0x002F212D File Offset: 0x002F032D
		public static bool FactionShouldHaveTerrorBuildings(Faction faction)
		{
			return ModsConfig.IdeologyActive && faction != null && faction.ideos != null && faction.ideos.PrimaryIdeo != null && faction.ideos.PrimaryIdeo.IdeoApprovesOfSlavery();
		}

		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x060083CB RID: 33739 RVA: 0x002F2160 File Offset: 0x002F0360
		public static IEnumerable<ThingDef> TerrorBuildings
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefs
				where def.StatBaseDefined(StatDefOf.TerrorSource) && typeof(Building_Casket).IsAssignableFrom(def.thingClass)
				select def;
			}
		}

		// Token: 0x060083CC RID: 33740 RVA: 0x002F218B File Offset: 0x002F038B
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && SymbolResolver_TerrorBuildings.FactionShouldHaveTerrorBuildings(rp.faction);
		}

		// Token: 0x060083CD RID: 33741 RVA: 0x002F21A8 File Offset: 0x002F03A8
		private void SpawnBuildings(List<IntVec3> potentialSpots, int buildingCount, ResolveParams rp, List<IntVec3> usedSpots, List<ThingDef> usedThingDefs)
		{
			Func<IntVec3, float> <>9__0;
			Func<ThingDef, float> <>9__1;
			Func<Faction, bool> <>9__2;
			while (potentialSpots.Count > 0 && buildingCount > 0)
			{
				Func<IntVec3, float> selector;
				if ((selector = <>9__0) == null)
				{
					selector = (<>9__0 = delegate(IntVec3 pSpot)
					{
						float num = float.PositiveInfinity;
						foreach (IntVec3 a in usedSpots)
						{
							float num2 = a.DistanceTo(pSpot);
							if (num > num2)
							{
								num = num2;
							}
						}
						return num;
					});
				}
				IntVec3 intVec = potentialSpots.MaxBy(selector);
				potentialSpots.Remove(intVec);
				usedSpots.Add(intVec);
				IEnumerable<ThingDef> terrorBuildings = SymbolResolver_TerrorBuildings.TerrorBuildings;
				Func<ThingDef, float> weightSelector;
				if ((weightSelector = <>9__1) == null)
				{
					weightSelector = (<>9__1 = delegate(ThingDef def)
					{
						int num = usedThingDefs.Count((ThingDef d) => d == def);
						if (num == 0)
						{
							return 1f;
						}
						return 1f / (float)num;
					});
				}
				ThingDef thingDef = terrorBuildings.RandomElementByWeight(weightSelector);
				Building_Casket building_Casket = (Building_Casket)ThingMaker.MakeThing(thingDef, BaseGenUtility.CheapStuffFor(thingDef, rp.faction));
				IEnumerable<Faction> factions = Find.FactionManager.GetFactions(false, false, false, TechLevel.Undefined, false);
				Func<Faction, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = ((Faction f) => !f.IsPlayer && f.HostileTo(rp.faction)));
				}
				Faction faction = factions.Where(predicate).RandomElementWithFallback(rp.faction);
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Slave, faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, false, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
				pawn.Kill(null, null);
				building_Casket.TryAcceptThing(pawn.Corpse, true);
				ResolveParams rp2 = rp;
				rp2.singleThingToSpawn = building_Casket;
				rp2.rect = CellRect.CenteredOn(intVec, building_Casket.def.size.x, building_Casket.def.size.z);
				BaseGen.symbolStack.Push("thing", rp2, null);
				buildingCount--;
			}
		}

		// Token: 0x060083CE RID: 33742 RVA: 0x002F2398 File Offset: 0x002F0598
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			int buildingCount = (int)SymbolResolver_TerrorBuildings.IndoorBuildingCountCurve.Evaluate((float)rp.rect.Area);
			int buildingCount2 = (int)SymbolResolver_TerrorBuildings.OutdoorsBuildingCountCurve.Evaluate((float)rp.rect.Area);
			List<IntVec3> list = new List<IntVec3>();
			List<IntVec3> list2 = new List<IntVec3>();
			List<IntVec3> usedSpots = new List<IntVec3>();
			List<ThingDef> usedThingDefs = new List<ThingDef>();
			foreach (IntVec3 intVec in rp.rect)
			{
				if (intVec.InBounds(map) && intVec.Standable(map) && intVec.GetDoor(map) == null)
				{
					int num = 0;
					bool flag = false;
					foreach (IntVec3 b in GenAdj.AdjacentCells)
					{
						IntVec3 c = intVec + b;
						if (c.InBounds(map))
						{
							foreach (Thing thing in c.GetThingList(map))
							{
								if (thing.def.IsEdifice())
								{
									if (thing.def != ThingDefOf.Wall)
									{
										flag = true;
										break;
									}
									num++;
								}
							}
						}
					}
					if (!flag && (num <= 0 || num % 2 != 0))
					{
						if (intVec.Roofed(map))
						{
							list.Add(intVec);
						}
						else
						{
							list2.Add(intVec);
						}
					}
				}
			}
			this.SpawnBuildings(list, buildingCount, rp, usedSpots, usedThingDefs);
			this.SpawnBuildings(list2, buildingCount2, rp, usedSpots, usedThingDefs);
		}

		// Token: 0x04005237 RID: 21047
		private static readonly SimpleCurve IndoorBuildingCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(40f, 1f),
				true
			},
			{
				new CurvePoint(120f, 2f),
				true
			}
		};

		// Token: 0x04005238 RID: 21048
		private static readonly SimpleCurve OutdoorsBuildingCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(30f, 2f),
				true
			},
			{
				new CurvePoint(80f, 3f),
				true
			},
			{
				new CurvePoint(120f, 4f),
				true
			}
		};
	}
}
