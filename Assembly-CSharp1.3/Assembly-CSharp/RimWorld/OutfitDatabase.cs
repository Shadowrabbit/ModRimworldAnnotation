using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B09 RID: 2825
	public sealed class OutfitDatabase : IExposable
	{
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06004257 RID: 16983 RVA: 0x001632A3 File Offset: 0x001614A3
		public List<Outfit> AllOutfits
		{
			get
			{
				return this.outfits;
			}
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x001632AB File Offset: 0x001614AB
		public OutfitDatabase()
		{
			this.GenerateStartingOutfits();
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x001632C4 File Offset: 0x001614C4
		public void ExposeData()
		{
			Scribe_Collections.Look<Outfit>(ref this.outfits, "outfits", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x001632DC File Offset: 0x001614DC
		public Outfit DefaultOutfit()
		{
			if (this.outfits.Count == 0)
			{
				this.MakeNewOutfit();
			}
			return this.outfits[0];
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x00163300 File Offset: 0x00161500
		public AcceptanceReport TryDelete(Outfit outfit)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
			{
				if (pawn.outfits != null && pawn.outfits.CurrentOutfit == outfit)
				{
					return new AcceptanceReport("OutfitInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.outfits != null && pawn2.outfits.CurrentOutfit == outfit)
				{
					pawn2.outfits.CurrentOutfit = null;
				}
			}
			this.outfits.Remove(outfit);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x001633F0 File Offset: 0x001615F0
		public Outfit MakeNewOutfit()
		{
			int num;
			if (!this.outfits.Any<Outfit>())
			{
				num = 1;
			}
			else
			{
				num = this.outfits.Max((Outfit o) => o.uniqueId) + 1;
			}
			int uniqueId = num;
			Outfit outfit = new Outfit(uniqueId, "Outfit".Translate() + " " + uniqueId.ToString());
			outfit.filter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
			this.outfits.Add(outfit);
			return outfit;
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x00163488 File Offset: 0x00161688
		private void GenerateStartingOutfits()
		{
			this.MakeNewOutfit().label = "OutfitAnything".Translate();
			Outfit outfit = this.MakeNewOutfit();
			outfit.label = "OutfitWorker".Translate();
			outfit.filter.SetDisallowAll(null, null);
			outfit.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.apparel != null && thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Worker"))
				{
					outfit.filter.SetAllow(thingDef, true);
				}
			}
			Outfit outfit2 = this.MakeNewOutfit();
			outfit2.label = "OutfitSoldier".Translate();
			outfit2.filter.SetDisallowAll(null, null);
			outfit2.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef2.apparel != null && thingDef2.apparel.defaultOutfitTags != null && thingDef2.apparel.defaultOutfitTags.Contains("Soldier"))
				{
					outfit2.filter.SetAllow(thingDef2, true);
				}
			}
			Outfit outfit3 = this.MakeNewOutfit();
			outfit3.label = "OutfitNudist".Translate();
			outfit3.filter.SetDisallowAll(null, null);
			outfit3.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef3.apparel != null && !thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs) && !thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
				{
					outfit3.filter.SetAllow(thingDef3, true);
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				Outfit outfit4 = this.MakeNewOutfit();
				outfit4.label = "OutfitSlave".Translate();
				outfit4.filter.SetDisallowAll(null, null);
				outfit4.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
				foreach (ThingDef thingDef4 in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef4.apparel != null && thingDef4.apparel.defaultOutfitTags != null && thingDef4.apparel.defaultOutfitTags.Contains("Slave"))
					{
						outfit4.filter.SetAllow(thingDef4, true);
					}
				}
			}
		}

		// Token: 0x04002865 RID: 10341
		private List<Outfit> outfits = new List<Outfit>();
	}
}
