using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001036 RID: 4150
	public sealed class OutfitDatabase : IExposable
	{
		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06005A66 RID: 23142 RVA: 0x0003EA60 File Offset: 0x0003CC60
		public List<Outfit> AllOutfits
		{
			get
			{
				return this.outfits;
			}
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x0003EA68 File Offset: 0x0003CC68
		public OutfitDatabase()
		{
			this.GenerateStartingOutfits();
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x0003EA81 File Offset: 0x0003CC81
		public void ExposeData()
		{
			Scribe_Collections.Look<Outfit>(ref this.outfits, "outfits", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06005A69 RID: 23145 RVA: 0x0003EA99 File Offset: 0x0003CC99
		public Outfit DefaultOutfit()
		{
			if (this.outfits.Count == 0)
			{
				this.MakeNewOutfit();
			}
			return this.outfits[0];
		}

		// Token: 0x06005A6A RID: 23146 RVA: 0x001D519C File Offset: 0x001D339C
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

		// Token: 0x06005A6B RID: 23147 RVA: 0x001D528C File Offset: 0x001D348C
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

		// Token: 0x06005A6C RID: 23148 RVA: 0x001D5324 File Offset: 0x001D3524
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
		}

		// Token: 0x04003CC4 RID: 15556
		private List<Outfit> outfits = new List<Outfit>();
	}
}
