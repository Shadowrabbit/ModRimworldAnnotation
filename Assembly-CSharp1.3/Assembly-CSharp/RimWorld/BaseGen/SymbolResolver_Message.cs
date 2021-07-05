using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015E7 RID: 5607
	public class SymbolResolver_Message : SymbolResolver
	{
		// Token: 0x060083A7 RID: 33703 RVA: 0x002F0A08 File Offset: 0x002EEC08
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
			signalAction_Message.signalTag = rp.messageSignalTag;
			signalAction_Message.message = rp.message;
			signalAction_Message.messageType = rp.messageType;
			signalAction_Message.lookTargets = rp.lookTargets;
			GenSpawn.Spawn(signalAction_Message, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
