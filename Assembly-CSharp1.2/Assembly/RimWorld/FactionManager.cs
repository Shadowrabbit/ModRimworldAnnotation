using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200159E RID: 5534
	public class FactionManager : IExposable
	{
		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06007814 RID: 30740 RVA: 0x00050DB6 File Offset: 0x0004EFB6
		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06007815 RID: 30741 RVA: 0x00050DB6 File Offset: 0x0004EFB6
		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x06007816 RID: 30742 RVA: 0x00050DBE File Offset: 0x0004EFBE
		public IEnumerable<Faction> AllFactionsVisible
		{
			get
			{
				return from fa in this.allFactions
				where !fa.Hidden
				select fa;
			}
		}

		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06007817 RID: 30743 RVA: 0x00050DEA File Offset: 0x0004EFEA
		public IEnumerable<Faction> AllFactionsVisibleInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactionsVisible);
			}
		}

		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06007818 RID: 30744 RVA: 0x00050DF7 File Offset: 0x0004EFF7
		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactions);
			}
		}

		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06007819 RID: 30745 RVA: 0x00050E04 File Offset: 0x0004F004
		public Faction OfPlayer
		{
			get
			{
				return this.ofPlayer;
			}
		}

		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x0600781A RID: 30746 RVA: 0x00050E0C File Offset: 0x0004F00C
		public Faction OfMechanoids
		{
			get
			{
				return this.ofMechanoids;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x0600781B RID: 30747 RVA: 0x00050E14 File Offset: 0x0004F014
		public Faction OfInsects
		{
			get
			{
				return this.ofInsects;
			}
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x0600781C RID: 30748 RVA: 0x00050E1C File Offset: 0x0004F01C
		public Faction OfAncients
		{
			get
			{
				return this.ofAncients;
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x0600781D RID: 30749 RVA: 0x00050E24 File Offset: 0x0004F024
		public Faction OfAncientsHostile
		{
			get
			{
				return this.ofAncientsHostile;
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x0600781E RID: 30750 RVA: 0x00050E2C File Offset: 0x0004F02C
		public Faction Empire
		{
			get
			{
				return this.empire;
			}
		}

		// Token: 0x0600781F RID: 30751 RVA: 0x00248F10 File Offset: 0x00247110
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction>(ref this.allFactions, "allFactions", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Faction>(ref this.toRemove, "toRemove", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.FactionManagerPostLoadInit();
				if (this.toRemove == null)
				{
					this.toRemove = new List<Faction>();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.allFactions.RemoveAll((Faction x) => x == null || x.def == null) != 0)
				{
					Log.Error("Some factions were null after loading.", false);
				}
				this.RecacheFactions();
			}
		}

		// Token: 0x06007820 RID: 30752 RVA: 0x00050E34 File Offset: 0x0004F034
		public void Add(Faction faction)
		{
			if (this.allFactions.Contains(faction))
			{
				return;
			}
			this.allFactions.Add(faction);
			this.RecacheFactions();
		}

		// Token: 0x06007821 RID: 30753 RVA: 0x00248FC0 File Offset: 0x002471C0
		private void Remove(Faction faction)
		{
			if (!faction.temporary)
			{
				Log.Error("Attempting to remove " + faction.Name + " which is not a temporary faction, only temporary factions can be removed", false);
				return;
			}
			if (!this.allFactions.Contains(faction))
			{
				return;
			}
			List<Pawn> allMapsWorldAndTemporary_AliveOrDead = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead;
			for (int i = 0; i < allMapsWorldAndTemporary_AliveOrDead.Count; i++)
			{
				if (allMapsWorldAndTemporary_AliveOrDead[i].Faction == faction)
				{
					allMapsWorldAndTemporary_AliveOrDead[i].SetFaction(null, null);
				}
			}
			for (int j = 0; j < Find.Maps.Count; j++)
			{
				Find.Maps[j].pawnDestinationReservationManager.Notify_FactionRemoved(faction);
			}
			Find.LetterStack.Notify_FactionRemoved(faction);
			faction.RemoveAllRelations();
			this.allFactions.Remove(faction);
			this.RecacheFactions();
		}

		// Token: 0x06007822 RID: 30754 RVA: 0x00249084 File Offset: 0x00247284
		public void FactionManagerTick()
		{
			SettlementProximityGoodwillUtility.CheckSettlementProximityGoodwillChange();
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].FactionTick();
			}
			for (int j = this.toRemove.Count - 1; j >= 0; j--)
			{
				Faction faction = this.toRemove[j];
				this.toRemove.Remove(faction);
				this.Remove(faction);
				Find.QuestManager.Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x06007823 RID: 30755 RVA: 0x00249104 File Offset: 0x00247304
		public Faction FirstFactionOfDef(FactionDef facDef)
		{
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].def == facDef)
				{
					return this.allFactions[i];
				}
			}
			return null;
		}

		// Token: 0x06007824 RID: 30756 RVA: 0x0024914C File Offset: 0x0024734C
		public bool TryGetRandomNonColonyHumanlikeFaction_NewTemp(out Faction faction, bool tryMedievalOrBetter, bool allowDefeated = false, TechLevel minTechLevel = TechLevel.Undefined, bool allowTemporary = false)
		{
			return (from x in this.AllFactions
			where !x.IsPlayer && !x.Hidden && x.def.humanlikeFaction && (allowDefeated || !x.defeated) && (allowTemporary || !x.temporary) && (minTechLevel == TechLevel.Undefined || x.def.techLevel >= minTechLevel)
			select x).TryRandomElementByWeight(delegate(Faction x)
			{
				if (tryMedievalOrBetter && x.def.techLevel < TechLevel.Medieval)
				{
					return 0.1f;
				}
				return 1f;
			}, out faction);
		}

		// Token: 0x06007825 RID: 30757 RVA: 0x00050E57 File Offset: 0x0004F057
		[Obsolete]
		public bool TryGetRandomNonColonyHumanlikeFaction(out Faction faction, bool tryMedievalOrBetter, bool allowDefeated = false, TechLevel minTechLevel = TechLevel.Undefined)
		{
			return this.TryGetRandomNonColonyHumanlikeFaction_NewTemp(out faction, tryMedievalOrBetter, allowDefeated, minTechLevel, false);
		}

		// Token: 0x06007826 RID: 30758 RVA: 0x00050E65 File Offset: 0x0004F065
		public IEnumerable<Faction> GetFactions_NewTemp(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined, bool allowTemporary = false)
		{
			int num;
			for (int i = 0; i < this.allFactions.Count; i = num + 1)
			{
				Faction faction = this.allFactions[i];
				if (!faction.IsPlayer && (allowHidden || !faction.Hidden) && (allowTemporary || !faction.temporary) && (allowDefeated || !faction.defeated) && (allowNonHumanlike || faction.def.humanlikeFaction) && (minTechLevel == TechLevel.Undefined || faction.def.techLevel >= minTechLevel))
				{
					yield return faction;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06007827 RID: 30759 RVA: 0x00050E9A File Offset: 0x0004F09A
		[Obsolete]
		public IEnumerable<Faction> GetFactions(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			return this.GetFactions_NewTemp(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false);
		}

		// Token: 0x06007828 RID: 30760 RVA: 0x002491A8 File Offset: 0x002473A8
		public Faction RandomEnemyFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions_NewTemp(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06007829 RID: 30761 RVA: 0x002491F4 File Offset: 0x002473F4
		public Faction RandomNonHostileFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions_NewTemp(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where !x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600782A RID: 30762 RVA: 0x00249240 File Offset: 0x00247440
		public Faction RandomAlliedFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions_NewTemp(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.PlayerRelationKind == FactionRelationKind.Ally
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600782B RID: 30763 RVA: 0x0024928C File Offset: 0x0024748C
		public Faction RandomRoyalFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions_NewTemp(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.def.HasRoyalTitles
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600782C RID: 30764 RVA: 0x002492D8 File Offset: 0x002474D8
		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:", false);
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}

		// Token: 0x0600782D RID: 30765 RVA: 0x0024931C File Offset: 0x0024751C
		public static IEnumerable<Faction> GetInViewOrder(IEnumerable<Faction> factions)
		{
			return from x in factions
			orderby x.defeated, x.def.listOrderPriority descending
			select x;
		}

		// Token: 0x0600782E RID: 30766 RVA: 0x00249374 File Offset: 0x00247574
		private void RecacheFactions()
		{
			this.ofPlayer = null;
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].IsPlayer)
				{
					this.ofPlayer = this.allFactions[i];
					break;
				}
			}
			this.ofMechanoids = this.FirstFactionOfDef(FactionDefOf.Mechanoid);
			this.ofInsects = this.FirstFactionOfDef(FactionDefOf.Insect);
			this.ofAncients = this.FirstFactionOfDef(FactionDefOf.Ancients);
			this.ofAncientsHostile = this.FirstFactionOfDef(FactionDefOf.AncientsHostile);
			this.empire = this.FirstFactionOfDef(FactionDefOf.Empire);
		}

		// Token: 0x0600782F RID: 30767 RVA: 0x0024941C File Offset: 0x0024761C
		public void Notify_QuestCleanedUp(Quest quest)
		{
			for (int i = this.allFactions.Count - 1; i >= 0; i--)
			{
				Faction faction = this.allFactions[i];
				if (this.FactionCanBeRemoved(faction))
				{
					this.QueueForRemoval(faction);
				}
			}
		}

		// Token: 0x06007830 RID: 30768 RVA: 0x00050EA8 File Offset: 0x0004F0A8
		public void Notify_PawnKilled(Pawn pawn)
		{
			this.TryQueuePawnFactionForRemoval(pawn);
		}

		// Token: 0x06007831 RID: 30769 RVA: 0x00050EA8 File Offset: 0x0004F0A8
		public void Notify_PawnLeftMap(Pawn pawn)
		{
			this.TryQueuePawnFactionForRemoval(pawn);
		}

		// Token: 0x06007832 RID: 30770 RVA: 0x00249460 File Offset: 0x00247660
		private void TryQueuePawnFactionForRemoval(Pawn pawn)
		{
			if (pawn.Faction != null && this.FactionCanBeRemoved(pawn.Faction))
			{
				this.QueueForRemoval(pawn.Faction);
			}
			Faction extraHomeFaction = pawn.GetExtraHomeFaction(null);
			if (extraHomeFaction != null && this.FactionCanBeRemoved(extraHomeFaction))
			{
				this.QueueForRemoval(extraHomeFaction);
			}
			Faction extraMiniFaction = pawn.GetExtraMiniFaction(null);
			if (extraMiniFaction != null && this.FactionCanBeRemoved(extraMiniFaction))
			{
				this.QueueForRemoval(extraMiniFaction);
			}
		}

		// Token: 0x06007833 RID: 30771 RVA: 0x00050EB1 File Offset: 0x0004F0B1
		private void QueueForRemoval(Faction faction)
		{
			if (!faction.temporary)
			{
				Log.Error("Cannot queue faction " + faction.Name + " for removal, only temporary factions can be removed", false);
				return;
			}
			if (!this.toRemove.Contains(faction))
			{
				this.toRemove.Add(faction);
			}
		}

		// Token: 0x06007834 RID: 30772 RVA: 0x002494C8 File Offset: 0x002476C8
		private bool FactionCanBeRemoved(Faction faction)
		{
			if (!faction.temporary || this.toRemove.Contains(faction) || Find.QuestManager.IsReservedByAnyQuest(faction))
			{
				return false;
			}
			List<Pawn> allMaps_Spawned = PawnsFinder.AllMaps_Spawned;
			for (int i = 0; i < allMaps_Spawned.Count; i++)
			{
				Pawn pawn = allMaps_Spawned[i];
				if (!pawn.Dead && ((pawn.Faction != null && pawn.Faction == faction) || faction == pawn.GetExtraHomeFaction(null) || faction == pawn.GetExtraMiniFaction(null)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04004F2B RID: 20267
		private List<Faction> allFactions = new List<Faction>();

		// Token: 0x04004F2C RID: 20268
		private List<Faction> toRemove = new List<Faction>();

		// Token: 0x04004F2D RID: 20269
		private Faction ofPlayer;

		// Token: 0x04004F2E RID: 20270
		private Faction ofMechanoids;

		// Token: 0x04004F2F RID: 20271
		private Faction ofInsects;

		// Token: 0x04004F30 RID: 20272
		private Faction ofAncients;

		// Token: 0x04004F31 RID: 20273
		private Faction ofAncientsHostile;

		// Token: 0x04004F32 RID: 20274
		private Faction empire;
	}
}
