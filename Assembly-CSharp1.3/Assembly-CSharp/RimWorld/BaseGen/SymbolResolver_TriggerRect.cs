using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015CD RID: 5581
	public class SymbolResolver_TriggerRect : SymbolResolver
	{
		// Token: 0x0600835A RID: 33626 RVA: 0x002ECD8C File Offset: 0x002EAF8C
		public override void Resolve(ResolveParams rp)
		{
			RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
			rectTrigger.signalTag = rp.rectTriggerSignalTag;
			rectTrigger.Rect = rp.rect;
			rectTrigger.destroyIfUnfogged = (rp.destroyIfUnfogged ?? false);
			GenSpawn.Spawn(rectTrigger, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
