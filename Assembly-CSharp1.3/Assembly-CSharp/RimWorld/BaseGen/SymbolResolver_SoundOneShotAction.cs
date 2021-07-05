using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015C9 RID: 5577
	public class SymbolResolver_SoundOneShotAction : SymbolResolver
	{
		// Token: 0x06008350 RID: 33616 RVA: 0x002ECB4C File Offset: 0x002EAD4C
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_SoundOneShot signalAction_SoundOneShot = (SignalAction_SoundOneShot)ThingMaker.MakeThing(ThingDefOf.SignalAction_SoundOneShot, null);
			signalAction_SoundOneShot.signalTag = rp.soundOneShotActionSignalTag;
			signalAction_SoundOneShot.sound = rp.sound;
			GenSpawn.Spawn(signalAction_SoundOneShot, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x04005202 RID: 20994
		private const string SoundSignalPrefix = "SoundTrigger";
	}
}
