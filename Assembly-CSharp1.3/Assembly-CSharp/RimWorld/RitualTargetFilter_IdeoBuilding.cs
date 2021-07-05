using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000FAB RID: 4011
	public class RitualTargetFilter_IdeoBuilding : RitualTargetFilter
	{
		// Token: 0x06005EB8 RID: 24248 RVA: 0x00206F73 File Offset: 0x00205173
		public RitualTargetFilter_IdeoBuilding()
		{
		}

		// Token: 0x06005EB9 RID: 24249 RVA: 0x00206F86 File Offset: 0x00205186
		public RitualTargetFilter_IdeoBuilding(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005EBA RID: 24250 RVA: 0x00206F9C File Offset: 0x0020519C
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			TargetInfo targetInfo = this.BestTarget(initiator, selectedTarget);
			rejectionReason = "";
			if (!targetInfo.IsValid)
			{
				rejectionReason = "AbilitySpeechDisabledNoSpot".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x06005EBB RID: 24251 RVA: 0x00206FD8 File Offset: 0x002051D8
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			Pawn pawn = (Pawn)initiator.Thing;
			Ideo ideo = pawn.Ideo;
			this.candidateSpots.Clear();
			for (int i = 0; i < ideo.PreceptsListForReading.Count; i++)
			{
				Precept_Building precept_Building = ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null)
				{
					Thing thing = precept_Building.presenceDemand.BestBuilding(pawn.Map, false);
					if (thing != null && pawn.CanReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						this.candidateSpots.Add(thing);
					}
				}
			}
			this.candidateSpots.AddRange(this.ExtraCandidates(initiator));
			if (!this.candidateSpots.NullOrEmpty<Thing>())
			{
				return this.candidateSpots.RandomElement<Thing>();
			}
			if (!this.def.fallBackToGatherSpot)
			{
				return TargetInfo.Invalid;
			}
			this.candidateSpots.Clear();
			for (int j = 0; j < pawn.Map.gatherSpotLister.activeSpots.Count; j++)
			{
				ThingWithComps parent = pawn.Map.gatherSpotLister.activeSpots[j].parent;
				if (pawn.CanReach(parent, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
				{
					this.candidateSpots.Add(parent);
				}
			}
			if (!this.candidateSpots.NullOrEmpty<Thing>())
			{
				return (from s in this.candidateSpots
				orderby s.Position.DistanceTo(pawn.Position)
				select s).First<Thing>();
			}
			return TargetInfo.Invalid;
		}

		// Token: 0x06005EBC RID: 24252 RVA: 0x0020718A File Offset: 0x0020538A
		protected virtual IEnumerable<Thing> ExtraCandidates(TargetInfo initiator)
		{
			yield break;
		}

		// Token: 0x06005EBD RID: 24253 RVA: 0x00207193 File Offset: 0x00205393
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			Ideo ideo = ((Pawn)initiator.Thing).Ideo;
			foreach (Precept_Building precept_Building in ideo.cachedPossibleBuildings)
			{
				yield return precept_Building.LabelCap;
			}
			HashSet<Precept_Building>.Enumerator enumerator = default(HashSet<Precept_Building>.Enumerator);
			yield return "RitualTargetGatherSpotInfo".Translate();
			yield break;
			yield break;
		}

		// Token: 0x04003698 RID: 13976
		private List<Thing> candidateSpots = new List<Thing>();
	}
}
