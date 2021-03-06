using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200215D RID: 8541
	public static class SiteMaker
	{
		// Token: 0x0600B60E RID: 46606 RVA: 0x000762B4 File Offset: 0x000744B4
		public static Site MakeSite(SitePartDef sitePart, int tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			return SiteMaker.MakeSite((sitePart != null) ? Gen.YieldSingle<SitePartDef>(sitePart) : null, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x0600B60F RID: 46607 RVA: 0x0034A760 File Offset: 0x00348960
		public static Site MakeSite(IEnumerable<SitePartDef> siteParts, int tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			float num = threatPoints ?? StorytellerUtility.DefaultSiteThreatPointsNow();
			List<SitePartDefWithParams> siteParts2;
			SiteMakerHelper.GenerateDefaultParams(num, tile, faction, siteParts, out siteParts2);
			Site site = SiteMaker.MakeSite(siteParts2, tile, faction, ifHostileThenMustRemainHostile);
			site.desiredThreatPoints = num;
			return site;
		}

		// Token: 0x0600B610 RID: 46608 RVA: 0x0034A7A4 File Offset: 0x003489A4
		public static Site MakeSite(IEnumerable<SitePartDefWithParams> siteParts, int tile, Faction faction, bool ifHostileThenMustRemainHostile = true)
		{
			Site site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
			site.Tile = tile;
			site.SetFaction(faction);
			if (ifHostileThenMustRemainHostile && faction != null && faction.HostileTo(Faction.OfPlayer))
			{
				site.factionMustRemainHostile = true;
			}
			if (siteParts != null)
			{
				foreach (SitePartDefWithParams sitePartDefWithParams in siteParts)
				{
					site.AddPart(new SitePart(site, sitePartDefWithParams.def, sitePartDefWithParams.parms));
				}
			}
			site.desiredThreatPoints = site.ActualThreatPoints;
			return site;
		}

		// Token: 0x0600B611 RID: 46609 RVA: 0x0034A848 File Offset: 0x00348A48
		public static Site TryMakeSite_SingleSitePart(IEnumerable<SitePartDef> singleSitePartCandidates, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(singleSitePartCandidates, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x0600B612 RID: 46610 RVA: 0x0034A874 File Offset: 0x00348A74
		public static Site TryMakeSite_SingleSitePart(string singleSitePartTag, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(singleSitePartTag, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x0600B613 RID: 46611 RVA: 0x0034A8A0 File Offset: 0x00348AA0
		public static Site TryMakeSite_MultipleSiteParts(IEnumerable<IEnumerable<SitePartDef>> sitePartsCandidates, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			List<SitePartDef> siteParts;
			if (!SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsCandidates, out siteParts, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x0600B614 RID: 46612 RVA: 0x0034A8CC File Offset: 0x00348ACC
		public static Site TryMakeSite_MultipleSiteParts(List<string> sitePartsTags, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			List<SitePartDef> siteParts;
			if (!SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsTags, out siteParts, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x0600B615 RID: 46613 RVA: 0x0034A8F8 File Offset: 0x00348AF8
		public static Site TryMakeSite(IEnumerable<SitePartDef> siteParts, int tile, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			Faction faction;
			if (!SiteMakerHelper.TryFindRandomFactionFor(siteParts, out faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}
	}
}
