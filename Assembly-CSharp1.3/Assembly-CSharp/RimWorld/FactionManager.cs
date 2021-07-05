using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBC RID: 3772
	public class FactionManager : IExposable
	{
		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x060058E8 RID: 22760 RVA: 0x001E5543 File Offset: 0x001E3743
		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060058E9 RID: 22761 RVA: 0x001E5543 File Offset: 0x001E3743
		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x060058EA RID: 22762 RVA: 0x001E554B File Offset: 0x001E374B
		public IEnumerable<Faction> AllFactionsVisible
		{
			get
			{
				return from fa in this.allFactions
				where !fa.Hidden
				select fa;
			}
		}

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x060058EB RID: 22763 RVA: 0x001E5577 File Offset: 0x001E3777
		public IEnumerable<Faction> AllFactionsVisibleInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactionsVisible);
			}
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x060058EC RID: 22764 RVA: 0x001E5584 File Offset: 0x001E3784
		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactions);
			}
		}

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x060058ED RID: 22765 RVA: 0x001E5591 File Offset: 0x001E3791
		public Faction OfPlayer
		{
			get
			{
				return this.ofPlayer;
			}
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x060058EE RID: 22766 RVA: 0x001E5599 File Offset: 0x001E3799
		public Faction OfMechanoids
		{
			get
			{
				return this.ofMechanoids;
			}
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x060058EF RID: 22767 RVA: 0x001E55A1 File Offset: 0x001E37A1
		public Faction OfInsects
		{
			get
			{
				return this.ofInsects;
			}
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x060058F0 RID: 22768 RVA: 0x001E55A9 File Offset: 0x001E37A9
		public Faction OfAncients
		{
			get
			{
				return this.ofAncients;
			}
		}

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x060058F1 RID: 22769 RVA: 0x001E55B1 File Offset: 0x001E37B1
		public Faction OfAncientsHostile
		{
			get
			{
				return this.ofAncientsHostile;
			}
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x060058F2 RID: 22770 RVA: 0x001E55B9 File Offset: 0x001E37B9
		public Faction OfEmpire
		{
			get
			{
				return this.empire;
			}
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x060058F3 RID: 22771 RVA: 0x001E55C1 File Offset: 0x001E37C1
		public Faction OfPirates
		{
			get
			{
				return this.ofPirates;
			}
		}

		// Token: 0x060058F4 RID: 22772 RVA: 0x001E55CC File Offset: 0x001E37CC
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
					Log.Error("Some factions were null after loading.");
				}
				this.RecacheFactions();
			}
		}

		// Token: 0x060058F5 RID: 22773 RVA: 0x001E5679 File Offset: 0x001E3879
		public void Add(Faction faction)
		{
			if (this.allFactions.Contains(faction))
			{
				return;
			}
			this.allFactions.Add(faction);
			this.RecacheFactions();
		}

		// Token: 0x060058F6 RID: 22774 RVA: 0x001E569C File Offset: 0x001E389C
		private void Remove(Faction faction)
		{
			if (!faction.temporary)
			{
				Log.Error("Attempting to remove " + faction.Name + " which is not a temporary faction, only temporary factions can be removed");
				return;
			}
			if (!this.allFactions.Contains(faction))
			{
				return;
			}
			faction.RemoveAllRelations();
			this.allFactions.Remove(faction);
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
			Find.PlayLog.Notify_FactionRemoved(faction);
			this.RecacheFactions();
			Find.QuestManager.Notify_FactionRemoved(faction);
			Find.IdeoManager.Notify_FactionRemoved(faction);
			Find.TaleManager.Notify_FactionRemoved(faction);
		}

		// Token: 0x060058F7 RID: 22775 RVA: 0x001E578C File Offset: 0x001E398C
		public void FactionManagerTick()
		{
			this.goodwillSituationManager.GoodwillManagerTick();
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
			}
		}

		// Token: 0x060058F8 RID: 22776 RVA: 0x001E580C File Offset: 0x001E3A0C
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

		// Token: 0x060058F9 RID: 22777 RVA: 0x001E5854 File Offset: 0x001E3A54
		public bool TryGetRandomNonColonyHumanlikeFaction(out Faction faction, bool tryMedievalOrBetter, bool allowDefeated = false, TechLevel minTechLevel = TechLevel.Undefined, bool allowTemporary = false)
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

		// Token: 0x060058FA RID: 22778 RVA: 0x001E58AE File Offset: 0x001E3AAE
		public IEnumerable<Faction> GetFactions(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined, bool allowTemporary = false)
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

		// Token: 0x060058FB RID: 22779 RVA: 0x001E58E4 File Offset: 0x001E3AE4
		public Faction RandomEnemyFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060058FC RID: 22780 RVA: 0x001E5930 File Offset: 0x001E3B30
		public Faction RandomNonHostileFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where !x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060058FD RID: 22781 RVA: 0x001E597C File Offset: 0x001E3B7C
		public Faction RandomAlliedFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.PlayerRelationKind == FactionRelationKind.Ally
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060058FE RID: 22782 RVA: 0x001E59C8 File Offset: 0x001E3BC8
		public Faction RandomRoyalFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.def.HasRoyalTitles
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060058FF RID: 22783 RVA: 0x001E5A14 File Offset: 0x001E3C14
		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:");
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}

		// Token: 0x06005900 RID: 22784 RVA: 0x001E5A58 File Offset: 0x001E3C58
		public void LogAllFactions()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in this.allFactions)
			{
				stringBuilder.AppendLine(string.Format("name: {0}, temporary: {1}, can be deleted?: {2}", faction.Name, faction.temporary, this.FactionCanBeRemoved(faction)));
			}
			stringBuilder.AppendLine(string.Format("{0} factions found.", this.allFactions.Count));
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06005901 RID: 22785 RVA: 0x001E5B04 File Offset: 0x001E3D04
		public void LogFactionsToRemove()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in this.toRemove)
			{
				stringBuilder.AppendLine(string.Format("name: {0}, temporary: {1}, can be deleted?: {2}", faction.Name, faction.temporary, this.FactionCanBeRemoved(faction)));
			}
			stringBuilder.AppendLine(string.Format("{0} factions found.", this.toRemove.Count));
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06005902 RID: 22786 RVA: 0x001E5BB0 File Offset: 0x001E3DB0
		public void LogFactionsOnPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IGrouping<Faction, Pawn> grouping in from p in Find.WorldPawns.AllPawnsAliveOrDead
			group p by p.Faction)
			{
				if (grouping.Key == null)
				{
					stringBuilder.AppendLine(string.Format("no faction: {0} pawns found.", grouping.Count<Pawn>()));
				}
				else
				{
					stringBuilder.AppendLine(string.Format("{0}: {1} pawns found.", grouping.Key, grouping.Count<Pawn>()));
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06005903 RID: 22787 RVA: 0x001E5C78 File Offset: 0x001E3E78
		public static IEnumerable<Faction> GetInViewOrder(IEnumerable<Faction> factions)
		{
			return from x in factions
			orderby x.defeated, x.def.listOrderPriority descending
			select x;
		}

		// Token: 0x06005904 RID: 22788 RVA: 0x001E5CD0 File Offset: 0x001E3ED0
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
			this.ofPirates = this.FirstFactionOfDef(FactionDefOf.Pirate);
		}

		// Token: 0x06005905 RID: 22789 RVA: 0x001E5D88 File Offset: 0x001E3F88
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

		// Token: 0x06005906 RID: 22790 RVA: 0x001E5DCA File Offset: 0x001E3FCA
		public void Notify_PawnKilled(Pawn pawn)
		{
			this.TryQueuePawnFactionForRemoval(pawn);
		}

		// Token: 0x06005907 RID: 22791 RVA: 0x001E5DCA File Offset: 0x001E3FCA
		public void Notify_PawnLeftMap(Pawn pawn)
		{
			this.TryQueuePawnFactionForRemoval(pawn);
		}

		// Token: 0x06005908 RID: 22792 RVA: 0x001E5DD3 File Offset: 0x001E3FD3
		public void Notify_PawnRecruited(Faction oldFaction)
		{
			if (this.FactionCanBeRemoved(oldFaction))
			{
				this.QueueForRemoval(oldFaction);
			}
		}

		// Token: 0x06005909 RID: 22793 RVA: 0x001E5DE5 File Offset: 0x001E3FE5
		public void Notify_WorldObjectDestroyed(WorldObject worldObject)
		{
			if (worldObject.Faction == null)
			{
				return;
			}
			if (this.FactionCanBeRemoved(worldObject.Faction))
			{
				this.QueueForRemoval(worldObject.Faction);
			}
		}

		// Token: 0x0600590A RID: 22794 RVA: 0x001E5E0C File Offset: 0x001E400C
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
			if (pawn.SlaveFaction != null && this.FactionCanBeRemoved(pawn.SlaveFaction))
			{
				this.QueueForRemoval(pawn.SlaveFaction);
			}
		}

		// Token: 0x0600590B RID: 22795 RVA: 0x001E5E93 File Offset: 0x001E4093
		private void QueueForRemoval(Faction faction)
		{
			if (!faction.temporary)
			{
				Log.Error("Cannot queue faction " + faction.Name + " for removal, only temporary factions can be removed");
				return;
			}
			if (!this.toRemove.Contains(faction))
			{
				this.toRemove.Add(faction);
			}
		}

		// Token: 0x0600590C RID: 22796 RVA: 0x001E5ED4 File Offset: 0x001E40D4
		private bool FactionCanBeRemoved(Faction faction)
		{
			FactionManager.<>c__DisplayClass58_0 CS$<>8__locals1;
			CS$<>8__locals1.faction = faction;
			if (!CS$<>8__locals1.faction.temporary || this.toRemove.Contains(CS$<>8__locals1.faction) || Find.QuestManager.IsReservedByAnyQuest(CS$<>8__locals1.faction))
			{
				return false;
			}
			if (!FactionManager.<FactionCanBeRemoved>g__CheckPawns|58_0(PawnsFinder.AllMaps_Spawned, ref CS$<>8__locals1))
			{
				return false;
			}
			if (!FactionManager.<FactionCanBeRemoved>g__CheckPawns|58_0(PawnsFinder.AllCaravansAndTravelingTransportPods_Alive, ref CS$<>8__locals1))
			{
				return false;
			}
			using (List<WorldObject>.Enumerator enumerator = Find.WorldObjects.AllWorldObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Faction == CS$<>8__locals1.faction)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600590E RID: 22798 RVA: 0x001E5FC0 File Offset: 0x001E41C0
		[CompilerGenerated]
		internal static bool <FactionCanBeRemoved>g__CheckPawns|58_0(List<Pawn> pawns, ref FactionManager.<>c__DisplayClass58_0 A_1)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (!pawn.Dead && ((pawn.Faction != null && pawn.Faction == A_1.faction) || A_1.faction == pawn.GetExtraHomeFaction(null) || A_1.faction == pawn.GetExtraMiniFaction(null) || A_1.faction == pawn.SlaveFaction))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400344D RID: 13389
		private List<Faction> allFactions = new List<Faction>();

		// Token: 0x0400344E RID: 13390
		private List<Faction> toRemove = new List<Faction>();

		// Token: 0x0400344F RID: 13391
		public GoodwillSituationManager goodwillSituationManager = new GoodwillSituationManager();

		// Token: 0x04003450 RID: 13392
		private Faction ofPlayer;

		// Token: 0x04003451 RID: 13393
		private Faction ofMechanoids;

		// Token: 0x04003452 RID: 13394
		private Faction ofInsects;

		// Token: 0x04003453 RID: 13395
		private Faction ofAncients;

		// Token: 0x04003454 RID: 13396
		private Faction ofAncientsHostile;

		// Token: 0x04003455 RID: 13397
		private Faction empire;

		// Token: 0x04003456 RID: 13398
		private Faction ofPirates;
	}
}
