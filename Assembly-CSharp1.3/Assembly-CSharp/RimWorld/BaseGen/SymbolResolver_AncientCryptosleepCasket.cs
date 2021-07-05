using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015AF RID: 5551
	public class SymbolResolver_AncientCryptosleepCasket : SymbolResolver
	{
		// Token: 0x060082EB RID: 33515 RVA: 0x002E8E8C File Offset: 0x002E708C
		public override void Resolve(ResolveParams rp)
		{
			int groupID = rp.ancientCryptosleepCasketGroupID ?? Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
			PodContentsType value = rp.podContentsType ?? Gen.RandomEnumValue<PodContentsType>(true);
			Rot4 rot = rp.thingRot ?? Rot4.North;
			Building_AncientCryptosleepCasket building_AncientCryptosleepCasket = (Building_AncientCryptosleepCasket)ThingMaker.MakeThing(ThingDefOf.AncientCryptosleepCasket, null);
			building_AncientCryptosleepCasket.groupID = groupID;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.podContentsType = new PodContentsType?(value);
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientPodContents.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				if (!building_AncientCryptosleepCasket.TryAcceptThing(list[i], false))
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					else
					{
						list[i].Destroy(DestroyMode.Vanish);
					}
				}
			}
			IntVec3 randomCell = rp.rect.RandomCell;
			GenSpawn.Spawn(building_AncientCryptosleepCasket, randomCell, BaseGen.globalSettings.map, rot, WipeMode.Vanish, false);
			if (rp.ancientCryptosleepCasketOpenSignalTag != null)
			{
				SignalAction_OpenCasket signalAction_OpenCasket = (SignalAction_OpenCasket)ThingMaker.MakeThing(ThingDefOf.SignalAction_OpenCasket, null);
				signalAction_OpenCasket.signalTag = rp.ancientCryptosleepCasketOpenSignalTag;
				signalAction_OpenCasket.caskets.Add(building_AncientCryptosleepCasket);
				GenSpawn.Spawn(signalAction_OpenCasket, randomCell, BaseGen.globalSettings.map, WipeMode.Vanish);
			}
		}
	}
}
