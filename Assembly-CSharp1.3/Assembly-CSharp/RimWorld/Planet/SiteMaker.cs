using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017D6 RID: 6102
	public static class SiteMaker
	{
		// Token: 0x06008E1A RID: 36378 RVA: 0x00330B48 File Offset: 0x0032ED48
		public static Site MakeSite(SitePartDef sitePart, int tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			return SiteMaker.MakeSite((sitePart != null) ? Gen.YieldSingle<SitePartDef>(sitePart) : null, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x06008E1B RID: 36379 RVA: 0x00330B60 File Offset: 0x0032ED60
		public static Site MakeSite(IEnumerable<SitePartDef> siteParts, int tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			float num = threatPoints ?? StorytellerUtility.DefaultSiteThreatPointsNow();
			List<SitePartDefWithParams> siteParts2;
			SiteMakerHelper.GenerateDefaultParams(num, tile, faction, siteParts, out siteParts2);
			Site site = SiteMaker.MakeSite(siteParts2, tile, faction, ifHostileThenMustRemainHostile);
			site.desiredThreatPoints = num;
			return site;
		}

		// Token: 0x06008E1C RID: 36380 RVA: 0x00330BA4 File Offset: 0x0032EDA4
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

		// Token: 0x06008E1D RID: 36381 RVA: 0x00330C48 File Offset: 0x0032EE48
		public static Site TryMakeSite_SingleSitePart(IEnumerable<SitePartDef> singleSitePartCandidates, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(singleSitePartCandidates, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x06008E1E RID: 36382 RVA: 0x00330C74 File Offset: 0x0032EE74
		public static Site TryMakeSite_SingleSitePart(string singleSitePartTag, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(singleSitePartTag, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x06008E1F RID: 36383 RVA: 0x00330CA0 File Offset: 0x0032EEA0
		public static Site TryMakeSite_MultipleSiteParts(IEnumerable<IEnumerable<SitePartDef>> sitePartsCandidates, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			List<SitePartDef> siteParts;
			if (!SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsCandidates, out siteParts, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x06008E20 RID: 36384 RVA: 0x00330CCC File Offset: 0x0032EECC
		public static Site TryMakeSite_MultipleSiteParts(List<string> sitePartsTags, int tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			List<SitePartDef> siteParts;
			if (!SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsTags, out siteParts, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints);
		}

		// Token: 0x06008E21 RID: 36385 RVA: 0x00330CF8 File Offset: 0x0032EEF8
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
