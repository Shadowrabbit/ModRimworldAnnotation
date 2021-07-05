using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF9 RID: 4089
	public class SitePartWorker_WorshippedTerminal : SitePartWorker
	{
		// Token: 0x06006042 RID: 24642 RVA: 0x0020CFB4 File Offset: 0x0020B1B4
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			sitePart.things = new ThingOwner<Thing>(sitePart);
			Thing item = ThingMaker.MakeThing(ThingDefOf.AncientTerminal, null);
			sitePart.things.TryAdd(item, true);
		}

		// Token: 0x06006043 RID: 24643 RVA: 0x0020CFF0 File Offset: 0x0020B1F0
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			return base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets).Formatted(map.Parent.GetComponent<TimedMakeFactionHostile>().TicksLeft.Value.ToStringTicksToPeriod(true, false, true, true).Named("TIMER"));
		}
	}
}
