using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015C1 RID: 5569
	public class SymbolResolver_OpenDoorAction : SymbolResolver
	{
		// Token: 0x0600832E RID: 33582 RVA: 0x002EB8D0 File Offset: 0x002E9AD0
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_OpenDoor signalAction_OpenDoor = (SignalAction_OpenDoor)ThingMaker.MakeThing(ThingDefOf.SignalAction_OpenDoor, null);
			signalAction_OpenDoor.signalTag = rp.openDoorActionSignalTag;
			signalAction_OpenDoor.door = rp.openDoorActionDoor;
			GenSpawn.Spawn(signalAction_OpenDoor, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
