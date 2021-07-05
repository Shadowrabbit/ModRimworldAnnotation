using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE4 RID: 4068
	public class SitePartWorker_AncientAltar : SitePartWorker
	{
		// Token: 0x06005FE6 RID: 24550 RVA: 0x0020BCEA File Offset: 0x00209EEA
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			sitePart.relicThing = sitePart.parms.relicThing;
		}

		// Token: 0x06005FE7 RID: 24551 RVA: 0x0020BD05 File Offset: 0x00209F05
		public bool ShouldKeepMapForRelic(SitePart sitePart)
		{
			return sitePart.relicThing != null && !sitePart.relicThing.DestroyedOrNull() && sitePart.relicThing.MapHeld == sitePart.site.Map;
		}

		// Token: 0x06005FE8 RID: 24552 RVA: 0x0020BD38 File Offset: 0x00209F38
		public override void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
			base.Notify_SiteMapAboutToBeRemoved(sitePart);
			if (this.ShouldKeepMapForRelic(sitePart))
			{
				if (sitePart.relicThing.Spawned)
				{
					sitePart.relicThing.DeSpawn(DestroyMode.Vanish);
				}
				if (sitePart.relicThing.holdingOwner != null)
				{
					sitePart.relicThing.holdingOwner.Remove(sitePart.relicThing);
				}
				sitePart.relicWasSpawned = false;
				return;
			}
			if (!sitePart.parms.relicLostSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(sitePart.parms.relicLostSignal));
			}
		}

		// Token: 0x06005FE9 RID: 24553 RVA: 0x0020BDC6 File Offset: 0x00209FC6
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == this.def)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}
	}
}
