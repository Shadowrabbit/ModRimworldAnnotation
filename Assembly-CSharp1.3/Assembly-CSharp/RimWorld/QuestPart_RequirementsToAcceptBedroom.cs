using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B98 RID: 2968
	public class QuestPart_RequirementsToAcceptBedroom : QuestPart_RequirementsToAccept
	{
		// Token: 0x0600455C RID: 17756 RVA: 0x0016FF94 File Offset: 0x0016E194
		public override AcceptanceReport CanAccept()
		{
			int num = this.CulpritsAre().Count<Pawn>();
			if (num > 0)
			{
				return ((num > 1) ? "QuestBedroomRequirementsUnsatisfied" : "QuestBedroomRequirementsUnsatisfiedSingle").Translate() + " " + (from p in this.CulpritsAre()
				select p.royalty.MainTitle().GetLabelFor(p).CapitalizeFirst() + " " + p.LabelShort).ToCommaList(true, false);
			}
			return true;
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x00170014 File Offset: 0x0016E214
		private List<Pawn> CulpritsAre()
		{
			this.culpritsResult.Clear();
			if (this.targetPawns.Any<Pawn>())
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
				{
					if (pawn.royalty != null && pawn.royalty.HighestTitleWithBedroomRequirements() != null && !pawn.Suspended && (!pawn.royalty.HasPersonalBedroom() || pawn.royalty.GetUnmetBedroomRequirements(true, false).Any<string>()))
					{
						this.culpritsResult.Add(pawn);
					}
				}
			}
			this.tmpOccupiedBeds.Clear();
			List<Thing> list = this.mapParent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Bed);
			foreach (Pawn pawn2 in this.targetPawns)
			{
				RoyalTitle royalTitle = pawn2.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null)
				{
					Thing thing = null;
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing2 = list[i];
						if (thing2.Faction == Faction.OfPlayer && thing2.GetRoom(RegionType.Set_All) != null && !this.tmpOccupiedBeds.Contains(thing2))
						{
							CompAssignableToPawn compAssignableToPawn = thing2.TryGetComp<CompAssignableToPawn>();
							if (compAssignableToPawn != null && compAssignableToPawn.AssignedPawnsForReading.Count <= 0 && RoyalTitleUtility.BedroomSatisfiesRequirements(thing2.GetRoom(RegionType.Set_All), royalTitle))
							{
								thing = thing2;
								break;
							}
						}
					}
					if (thing != null)
					{
						this.tmpOccupiedBeds.Add(thing);
					}
					else
					{
						this.culpritsResult.Add(pawn2);
					}
				}
			}
			this.tmpOccupiedBeds.Clear();
			return this.culpritsResult;
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x001701E8 File Offset: 0x0016E3E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Pawn>(ref this.targetPawns, "targetPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.targetPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x0600455F RID: 17759 RVA: 0x00170255 File Offset: 0x0016E455
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				return this.CulpritsAre().Select(delegate(Pawn p)
				{
					RoyalTitle royalTitle = p.royalty.HighestTitleWithBedroomRequirements();
					return new Dialog_InfoCard.Hyperlink(royalTitle.def, royalTitle.faction, -1);
				});
			}
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x00170281 File Offset: 0x0016E481
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.targetPawns.Replace(replace, with);
		}

		// Token: 0x04002A44 RID: 10820
		public List<Pawn> targetPawns = new List<Pawn>();

		// Token: 0x04002A45 RID: 10821
		public MapParent mapParent;

		// Token: 0x04002A46 RID: 10822
		private List<Thing> tmpOccupiedBeds = new List<Thing>();

		// Token: 0x04002A47 RID: 10823
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
