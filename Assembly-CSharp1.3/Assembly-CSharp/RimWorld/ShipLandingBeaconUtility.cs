using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001196 RID: 4502
	public static class ShipLandingBeaconUtility
	{
		// Token: 0x06006C51 RID: 27729 RVA: 0x002450F8 File Offset: 0x002432F8
		public static List<ShipLandingArea> GetLandingZones(Map map)
		{
			ShipLandingBeaconUtility.tmpShipLandingAreas.Clear();
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null && thing.Faction == Faction.OfPlayer)
				{
					foreach (ShipLandingArea shipLandingArea in compShipLandingBeacon.LandingAreas)
					{
						if (shipLandingArea.Active && !ShipLandingBeaconUtility.tmpShipLandingAreas.Contains(shipLandingArea))
						{
							shipLandingArea.RecalculateBlockingThing();
							ShipLandingBeaconUtility.tmpShipLandingAreas.Add(shipLandingArea);
						}
					}
				}
			}
			return ShipLandingBeaconUtility.tmpShipLandingAreas;
		}

		// Token: 0x06006C52 RID: 27730 RVA: 0x002451E0 File Offset: 0x002433E0
		public static void DrawLinesToNearbyBeacons(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map, Thing thing = null)
		{
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing thing2 in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				if ((thing == null || thing != thing2) && thing2.Faction == Faction.OfPlayer)
				{
					CompShipLandingBeacon compShipLandingBeacon = thing2.TryGetComp<CompShipLandingBeacon>();
					if (compShipLandingBeacon != null && ShipLandingBeaconUtility.CanLinkTo(myPos, compShipLandingBeacon) && !GenThing.CloserThingBetween(myDef, myPos, thing2.Position, map, null))
					{
						GenDraw.DrawLineBetween(a, thing2.TrueCenter(), SimpleColor.White, 0.2f);
					}
				}
			}
			float minEdgeDistance = myDef.GetCompProperties<CompProperties_ShipLandingBeacon>().edgeLengthRange.min - 1f;
			float maxEdgeDistance = myDef.GetCompProperties<CompProperties_ShipLandingBeacon>().edgeLengthRange.max - 1f;
			foreach (Thing thing3 in map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint))
			{
				if ((thing == null || thing != thing3) && thing3.def.entityDefToBuild == myDef && (myPos.x == thing3.Position.x || myPos.z == thing3.Position.z) && !ShipLandingBeaconUtility.AlignedDistanceTooShort(myPos, thing3.Position, minEdgeDistance) && !ShipLandingBeaconUtility.AlignedDistanceTooLong(myPos, thing3.Position, maxEdgeDistance) && !GenThing.CloserThingBetween(myDef, myPos, thing3.Position, map, null))
				{
					GenDraw.DrawLineBetween(a, thing3.TrueCenter(), SimpleColor.White, 0.2f);
				}
			}
		}

		// Token: 0x06006C53 RID: 27731 RVA: 0x0024539C File Offset: 0x0024359C
		public static bool AlignedDistanceTooShort(IntVec3 position, IntVec3 otherPos, float minEdgeDistance)
		{
			if (position.x == otherPos.x)
			{
				return (float)Mathf.Abs(position.z - otherPos.z) < minEdgeDistance;
			}
			return position.z == otherPos.z && (float)Mathf.Abs(position.x - otherPos.x) < minEdgeDistance;
		}

		// Token: 0x06006C54 RID: 27732 RVA: 0x002453F4 File Offset: 0x002435F4
		private static bool AlignedDistanceTooLong(IntVec3 position, IntVec3 otherPos, float maxEdgeDistance)
		{
			if (position.x == otherPos.x)
			{
				return (float)Mathf.Abs(position.z - otherPos.z) >= maxEdgeDistance;
			}
			return position.z == otherPos.z && (float)Mathf.Abs(position.x - otherPos.x) >= maxEdgeDistance;
		}

		// Token: 0x06006C55 RID: 27733 RVA: 0x00245454 File Offset: 0x00243654
		public static bool CanLinkTo(IntVec3 position, CompShipLandingBeacon other)
		{
			if (position.x == other.parent.Position.x)
			{
				return other.parent.def.displayNumbersBetweenSameDefDistRange.Includes((float)(Mathf.Abs(position.z - other.parent.Position.z) + 1));
			}
			return position.z == other.parent.Position.z && other.parent.def.displayNumbersBetweenSameDefDistRange.Includes((float)(Mathf.Abs(position.x - other.parent.Position.x) + 1));
		}

		// Token: 0x04003C2D RID: 15405
		public static List<ShipLandingArea> tmpShipLandingAreas = new List<ShipLandingArea>();
	}
}
