using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011DD RID: 4573
	public static class ShipUtility
	{
		// Token: 0x06006E5A RID: 28250 RVA: 0x0024FDBC File Offset: 0x0024DFBC
		public static Dictionary<ThingDef, int> RequiredParts()
		{
			if (ShipUtility.requiredParts == null)
			{
				ShipUtility.requiredParts = new Dictionary<ThingDef, int>();
				ShipUtility.requiredParts[ThingDefOf.Ship_CryptosleepCasket] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_ComputerCore] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_Reactor] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_Engine] = 3;
				ShipUtility.requiredParts[ThingDefOf.Ship_Beam] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_SensorCluster] = 1;
			}
			return ShipUtility.requiredParts;
		}

		// Token: 0x06006E5B RID: 28251 RVA: 0x0024FE3F File Offset: 0x0024E03F
		public static IEnumerable<string> LaunchFailReasons(Building rootBuilding)
		{
			List<Building> shipParts = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>();
			using (Dictionary<ThingDef, int>.Enumerator enumerator = ShipUtility.RequiredParts().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ThingDef, int> partDef = enumerator.Current;
					int num = shipParts.Count((Building pa) => pa.def == partDef.Key);
					if (num < partDef.Value)
					{
						yield return string.Format("{0}: {1}x {2} ({3} {4})", new object[]
						{
							"ShipReportMissingPart".Translate(),
							partDef.Value - num,
							partDef.Key.label,
							"ShipReportMissingPartRequires".Translate(),
							partDef.Value
						});
					}
				}
			}
			Dictionary<ThingDef, int>.Enumerator enumerator = default(Dictionary<ThingDef, int>.Enumerator);
			bool fullPodFound = false;
			foreach (Building building in shipParts)
			{
				if (building.def == ThingDefOf.Ship_CryptosleepCasket)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = building as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
					{
						fullPodFound = true;
						break;
					}
				}
			}
			foreach (Building building2 in shipParts)
			{
				CompHibernatable compHibernatable = building2.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					yield return string.Format("{0}: {1}", "ShipReportHibernating".Translate(), building2.LabelCap);
				}
				else if (compHibernatable != null && !compHibernatable.Running)
				{
					yield return string.Format("{0}: {1}", "ShipReportNotReady".Translate(), building2.LabelCap);
				}
			}
			List<Building>.Enumerator enumerator3 = default(List<Building>.Enumerator);
			if (!fullPodFound)
			{
				yield return "ShipReportNoFullPods".Translate();
			}
			yield break;
			yield break;
		}

		// Token: 0x06006E5C RID: 28252 RVA: 0x0024FE50 File Offset: 0x0024E050
		public static bool HasHibernatingParts(Building rootBuilding)
		{
			foreach (Building thing in ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006E5D RID: 28253 RVA: 0x0024FEC0 File Offset: 0x0024E0C0
		public static void StartupHibernatingParts(Building rootBuilding)
		{
			foreach (Building thing in ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					compHibernatable.Startup();
				}
			}
			SoundDefOf.ShipReactor_Startup.PlayOneShot(SoundInfo.InMap(rootBuilding, MaintenanceType.None));
		}

		// Token: 0x06006E5E RID: 28254 RVA: 0x0024FF44 File Offset: 0x0024E144
		public static List<Building> ShipBuildingsAttachedTo(Building root)
		{
			ShipUtility.closedSet.Clear();
			if (root == null || root.Destroyed)
			{
				return ShipUtility.closedSet;
			}
			ShipUtility.openSet.Clear();
			ShipUtility.openSet.Add(root);
			while (ShipUtility.openSet.Count > 0)
			{
				Building building = ShipUtility.openSet[ShipUtility.openSet.Count - 1];
				ShipUtility.openSet.Remove(building);
				ShipUtility.closedSet.Add(building);
				foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(building))
				{
					Building edifice = c.GetEdifice(building.Map);
					if (edifice != null && edifice.def.building.shipPart && !ShipUtility.closedSet.Contains(edifice) && !ShipUtility.openSet.Contains(edifice))
					{
						ShipUtility.openSet.Add(edifice);
					}
				}
			}
			return ShipUtility.closedSet;
		}

		// Token: 0x06006E5F RID: 28255 RVA: 0x00250048 File Offset: 0x0024E248
		public static IEnumerable<Gizmo> ShipStartupGizmos(Building building)
		{
			if (ShipUtility.HasHibernatingParts(building))
			{
				Action <>9__1;
				yield return new Command_Action
				{
					action = delegate()
					{
						string text = "HibernateWarning";
						if (building.Map.info.parent.GetComponent<EscapeShipComp>() == null)
						{
							text += "Standalone";
						}
						if (!Find.Storyteller.difficulty.allowBigThreats)
						{
							text += "Pacifist";
						}
						DiaNode diaNode = new DiaNode(text.Translate());
						DiaOption diaOption = new DiaOption("Confirm".Translate());
						DiaOption diaOption2 = diaOption;
						Action action;
						if ((action = <>9__1) == null)
						{
							action = (<>9__1 = delegate()
							{
								ShipUtility.StartupHibernatingParts(building);
							});
						}
						diaOption2.action = action;
						diaOption.resolveTree = true;
						diaNode.options.Add(diaOption);
						DiaOption diaOption3 = new DiaOption("GoBack".Translate());
						diaOption3.resolveTree = true;
						diaNode.options.Add(diaOption3);
						Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
					},
					defaultLabel = "CommandShipStartup".Translate(),
					defaultDesc = "CommandShipStartupDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc1,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/DesirePower", true)
				};
			}
			yield break;
		}

		// Token: 0x04003D33 RID: 15667
		private static Dictionary<ThingDef, int> requiredParts;

		// Token: 0x04003D34 RID: 15668
		private static List<Building> closedSet = new List<Building>();

		// Token: 0x04003D35 RID: 15669
		private static List<Building> openSet = new List<Building>();
	}
}
