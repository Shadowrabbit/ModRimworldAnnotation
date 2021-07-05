using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001587 RID: 5511
	public class SketchResolver_AncientEquipmentRoom : SketchResolver
	{
		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x0600823B RID: 33339 RVA: 0x002E1A68 File Offset: 0x002DFC68
		private static IEnumerable<ThingDef> CentralThings
		{
			get
			{
				yield return ThingDefOf.AncientMachine;
				yield return ThingDefOf.AncientStorageCylinder;
				yield break;
			}
		}

		// Token: 0x17001601 RID: 5633
		// (get) Token: 0x0600823C RID: 33340 RVA: 0x002E1A71 File Offset: 0x002DFC71
		private static IEnumerable<ThingDef> EdgeThings
		{
			get
			{
				yield return ThingDefOf.AncientSystemRack;
				yield return ThingDefOf.AncientEquipmentBlocks;
				yield break;
			}
		}

		// Token: 0x0600823D RID: 33341 RVA: 0x002E15AF File Offset: 0x002DF7AF
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600823E RID: 33342 RVA: 0x002E1A7C File Offset: 0x002DFC7C
		protected override void ResolveInt(ResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient equipment room"))
			{
				return;
			}
			ResolveParams parms2 = parms;
			parms2.cornerThing = ThingDefOf.AncientLamp;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			foreach (ThingDef thingCentral in SketchResolver_AncientEquipmentRoom.CentralThings)
			{
				ResolveParams parms3 = parms;
				parms3.thingCentral = thingCentral;
				parms3.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
			}
			foreach (ThingDef wallEdgeThing in SketchResolver_AncientEquipmentRoom.EdgeThings)
			{
				ResolveParams parms4 = parms;
				parms4.wallEdgeThing = wallEdgeThing;
				parms4.requireFloor = new bool?(true);
				SketchResolverDefOf.AddWallEdgeThings.Resolve(parms4);
			}
		}
	}
}
