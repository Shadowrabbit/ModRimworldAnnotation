using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E42 RID: 7746
	public class SymbolResolver_AncientCryptosleepCasket : SymbolResolver
	{
		// Token: 0x0600A75A RID: 42842 RVA: 0x0030B00C File Offset: 0x0030920C
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
			GenSpawn.Spawn(building_AncientCryptosleepCasket, rp.rect.RandomCell, BaseGen.globalSettings.map, rot, WipeMode.Vanish, false);
		}
	}
}
