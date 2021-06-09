using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200151B RID: 5403
	[StaticConstructorOnStartup]
	public class Pawn_RoyaltyTracker : IExposable
	{
		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x060074C3 RID: 29891 RVA: 0x0004EDE1 File Offset: 0x0004CFE1
		public List<RoyalTitle> AllTitlesForReading
		{
			get
			{
				return this.titles;
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x060074C4 RID: 29892 RVA: 0x0004EDE9 File Offset: 0x0004CFE9
		public List<RoyalTitle> AllTitlesInEffectForReading
		{
			get
			{
				if (!this.pawn.IsWildMan())
				{
					return this.titles;
				}
				return Pawn_RoyaltyTracker.EmptyTitles;
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x060074C5 RID: 29893 RVA: 0x00238D94 File Offset: 0x00236F94
		public RoyalTitle MostSeniorTitle
		{
			get
			{
				List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
				int num = -1;
				RoyalTitle royalTitle = null;
				for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
				{
					if (allTitlesInEffectForReading[i].def.seniority > num)
					{
						num = allTitlesInEffectForReading[i].def.seniority;
						royalTitle = allTitlesInEffectForReading[i];
					}
				}
				return royalTitle ?? null;
			}
		}

		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x060074C6 RID: 29894 RVA: 0x0004EE04 File Offset: 0x0004D004
		public IEnumerable<QuestScriptDef> PossibleDecreeQuests
		{
			get
			{
				this.tmpDecreeTags.Clear();
				List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
				for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
				{
					if (allTitlesInEffectForReading[i].def.decreeTags != null)
					{
						this.tmpDecreeTags.AddRange(allTitlesInEffectForReading[i].def.decreeTags);
					}
				}
				foreach (QuestScriptDef questScriptDef in DefDatabase<QuestScriptDef>.AllDefs)
				{
					if (questScriptDef.decreeTags != null && questScriptDef.decreeTags.Any((string x) => this.tmpDecreeTags.Contains(x)))
					{
						Slate slate = new Slate();
						Slate slate2 = slate;
						string name = "points";
						IIncidentTarget mapHeld = this.pawn.MapHeld;
						slate2.Set<float>(name, StorytellerUtility.DefaultThreatPointsNow(mapHeld ?? Find.World), false);
						slate.Set<Pawn>("asker", this.pawn, false);
						if (questScriptDef.CanRun(slate))
						{
							yield return questScriptDef;
						}
					}
				}
				IEnumerator<QuestScriptDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x060074C7 RID: 29895 RVA: 0x00238DF4 File Offset: 0x00236FF4
		public bool PermitPointsAvailable
		{
			get
			{
				foreach (KeyValuePair<Faction, int> keyValuePair in this.permitPoints)
				{
					if (keyValuePair.Value > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x060074C8 RID: 29896 RVA: 0x0004EE14 File Offset: 0x0004D014
		public List<FactionPermit> AllFactionPermits
		{
			get
			{
				return this.factionPermits;
			}
		}

		// Token: 0x060074C9 RID: 29897 RVA: 0x00238E54 File Offset: 0x00237054
		public Pawn_RoyaltyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060074CA RID: 29898 RVA: 0x00238EE0 File Offset: 0x002370E0
		private int FindFactionTitleIndex(Faction faction, bool createIfNotExisting = false)
		{
			for (int i = 0; i < this.titles.Count; i++)
			{
				if (this.titles[i].faction == faction)
				{
					return i;
				}
			}
			if (createIfNotExisting)
			{
				this.titles.Add(new RoyalTitle
				{
					faction = faction,
					receivedTick = GenTicks.TicksGame,
					conceited = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(this.pawn),
					pawn = this.pawn
				});
				return this.titles.Count - 1;
			}
			return -1;
		}

		// Token: 0x060074CB RID: 29899 RVA: 0x00238F6C File Offset: 0x0023716C
		public bool HasAnyTitleIn(Faction faction)
		{
			using (List<RoyalTitle>.Enumerator enumerator = this.titles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.faction == faction)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060074CC RID: 29900 RVA: 0x00238FC8 File Offset: 0x002371C8
		public bool HasTitle(RoyalTitleDef title)
		{
			using (List<RoyalTitle>.Enumerator enumerator = this.titles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == title)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060074CD RID: 29901 RVA: 0x00239024 File Offset: 0x00237224
		public List<FactionPermit> PermitsFromFaction(Faction faction)
		{
			Pawn_RoyaltyTracker.tmpPermits.Clear();
			foreach (FactionPermit factionPermit in this.factionPermits)
			{
				if (factionPermit.Faction == faction)
				{
					Pawn_RoyaltyTracker.tmpPermits.Add(factionPermit);
				}
			}
			return Pawn_RoyaltyTracker.tmpPermits;
		}

		// Token: 0x060074CE RID: 29902 RVA: 0x00239094 File Offset: 0x00237294
		public void AddPermit(RoyalTitlePermitDef permit, Faction faction)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Permits are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 63693412, false);
				return;
			}
			if (this.HasPermit(permit, faction))
			{
				return;
			}
			if (permit.prerequisite != null && this.HasPermit(permit.prerequisite, faction))
			{
				FactionPermit item = this.factionPermits.Find((FactionPermit x) => x.Permit == permit.prerequisite && x.Faction == faction);
				this.factionPermits.Remove(item);
			}
			this.factionPermits.Add(new FactionPermit(faction, this.GetCurrentTitle(faction), permit));
			this.RecalculatePermitPoints(faction);
		}

		// Token: 0x060074CF RID: 29903 RVA: 0x00239164 File Offset: 0x00237364
		public void RefundPermits(int favorCost, Faction faction)
		{
			if (this.favor[faction] < favorCost)
			{
				Log.Error("Not enough favor to refund permits.", false);
				return;
			}
			bool flag = false;
			for (int i = this.factionPermits.Count - 1; i >= 0; i--)
			{
				if (this.factionPermits[i].Faction == faction)
				{
					this.factionPermits.RemoveAt(i);
					flag = true;
				}
			}
			if (flag)
			{
				int num = this.GetFavor(faction);
				Dictionary<Faction, int> dictionary = this.favor;
				dictionary[faction] -= favorCost;
				this.OnFavorChanged(faction, num, num - favorCost);
				this.RecalculatePermitPoints(faction);
			}
		}

		// Token: 0x060074D0 RID: 29904 RVA: 0x00239204 File Offset: 0x00237404
		public bool HasPermit(RoyalTitlePermitDef permit, Faction faction)
		{
			foreach (FactionPermit factionPermit in this.factionPermits)
			{
				if (factionPermit.Permit == permit && factionPermit.Faction == faction)
				{
					return true;
				}
			}
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			return currentTitle != null && currentTitle.permits != null && currentTitle.permits.Contains(permit);
		}

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x060074D1 RID: 29905 RVA: 0x0004EE1C File Offset: 0x0004D01C
		public bool HasAidPermit
		{
			get
			{
				return this.factionPermits.Any<FactionPermit>();
			}
		}

		// Token: 0x060074D2 RID: 29906 RVA: 0x000236C9 File Offset: 0x000218C9
		[Obsolete]
		public int GetPermitLastUsedTick(RoyalTitlePermitDef permitDef)
		{
			return -1;
		}

		// Token: 0x060074D3 RID: 29907 RVA: 0x0000A2E4 File Offset: 0x000084E4
		[Obsolete]
		public bool PermitOnCooldown(RoyalTitlePermitDef permitDef)
		{
			return false;
		}

		// Token: 0x060074D4 RID: 29908 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete]
		public void Notify_PermitUsed(RoyalTitlePermitDef permitDef)
		{
		}

		// Token: 0x060074D5 RID: 29909 RVA: 0x0023928C File Offset: 0x0023748C
		public RoyalTitleDef MainTitle()
		{
			if (this.titles.Count == 0)
			{
				return null;
			}
			RoyalTitleDef royalTitleDef = null;
			foreach (RoyalTitle royalTitle in this.titles)
			{
				if (royalTitleDef == null || royalTitle.def.seniority > royalTitleDef.seniority)
				{
					royalTitleDef = royalTitle.def;
				}
			}
			return royalTitleDef;
		}

		// Token: 0x060074D6 RID: 29910 RVA: 0x00239308 File Offset: 0x00237508
		public int GetFavor(Faction faction)
		{
			int result;
			if (!this.favor.TryGetValue(faction, out result))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x060074D7 RID: 29911 RVA: 0x00239328 File Offset: 0x00237528
		public FactionPermit GetPermit(RoyalTitlePermitDef permit, Faction faction)
		{
			foreach (FactionPermit factionPermit in this.factionPermits)
			{
				if (factionPermit.Permit == permit && factionPermit.Faction == faction)
				{
					return factionPermit;
				}
			}
			return null;
		}

		// Token: 0x060074D8 RID: 29912 RVA: 0x00239390 File Offset: 0x00237590
		public int GetPermitPoints(Faction faction)
		{
			int result;
			if (!this.permitPoints.TryGetValue(faction, out result))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x060074D9 RID: 29913 RVA: 0x002393B0 File Offset: 0x002375B0
		public void GainFavor(Faction faction, int amount)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Honor is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 63699999, false);
				return;
			}
			int oldAmount = this.GetFavor(faction);
			int num;
			if (!this.favor.TryGetValue(faction, out num))
			{
				num = 0;
				this.favor.Add(faction, 0);
			}
			num += amount;
			this.favor[faction] = num;
			if (amount < 0)
			{
				this.TryUpdateTitle_NewTemp(faction, true, null);
			}
			this.OnFavorChanged(faction, oldAmount, num);
		}

		// Token: 0x060074DA RID: 29914 RVA: 0x00239424 File Offset: 0x00237624
		public RoyalTitleDef GetTitleAwardedWhenUpdating(Faction faction, int favor)
		{
			RoyalTitleDef royalTitleDef = this.GetCurrentTitle(faction);
			RoyalTitleDef result = null;
			while (favor > 0 && royalTitleDef.GetNextTitle(faction) != null)
			{
				if (royalTitleDef == null)
				{
					royalTitleDef = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.First<RoyalTitleDef>();
				}
				else
				{
					royalTitleDef = royalTitleDef.GetNextTitle(faction);
				}
				favor -= royalTitleDef.favorCost;
				if (favor >= 0)
				{
					result = royalTitleDef;
				}
			}
			return result;
		}

		// Token: 0x060074DB RID: 29915 RVA: 0x0023947C File Offset: 0x0023767C
		public bool CanUpdateTitleOfAnyFaction(out Faction faction)
		{
			foreach (Faction faction2 in Find.FactionManager.AllFactions)
			{
				if (this.CanUpdateTitle(faction2))
				{
					faction = faction2;
					return true;
				}
			}
			faction = null;
			return false;
		}

		// Token: 0x060074DC RID: 29916 RVA: 0x002394DC File Offset: 0x002376DC
		public bool CanUpdateTitle(Faction faction)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			int num = this.GetFavor(faction);
			RoyalTitleDef nextTitle = currentTitle.GetNextTitle(faction);
			return nextTitle != null && num >= nextTitle.favorCost;
		}

		// Token: 0x060074DD RID: 29917 RVA: 0x00239510 File Offset: 0x00237710
		public bool TryUpdateTitle_NewTemp(Faction faction, bool sendLetter = true, RoyalTitleDef updateTo = null)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			this.UpdateRoyalTitle_NewTemp(faction, sendLetter, updateTo);
			RoyalTitleDef currentTitle2 = this.GetCurrentTitle(faction);
			if (currentTitle2 != currentTitle)
			{
				this.ApplyRewardsForTitle(faction, currentTitle, currentTitle2, false);
				this.OnPostTitleChanged_NewTemp(faction, currentTitle, currentTitle2);
			}
			return currentTitle2 != currentTitle;
		}

		// Token: 0x060074DE RID: 29918 RVA: 0x0004EE29 File Offset: 0x0004D029
		[Obsolete("Will be removed in a future update")]
		public bool TryUpdateTitle(Faction faction, bool sendLetter = true)
		{
			return this.TryUpdateTitle_NewTemp(faction, sendLetter, null);
		}

		// Token: 0x060074DF RID: 29919 RVA: 0x00239558 File Offset: 0x00237758
		public bool TryRemoveFavor(Faction faction, int amount)
		{
			int num = this.GetFavor(faction);
			if (num < amount)
			{
				return false;
			}
			this.SetFavor_NewTemp(faction, num - amount, true);
			return true;
		}

		// Token: 0x060074E0 RID: 29920 RVA: 0x00239580 File Offset: 0x00237780
		public void SetFavor_NewTemp(Faction faction, int amount, bool notifyOnFavorChanged = true)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Honor is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 7641236, false);
				return;
			}
			int oldAmount = this.GetFavor(faction);
			if (amount == 0 && this.favor.ContainsKey(faction) && this.FindFactionTitleIndex(faction, false) == -1)
			{
				this.favor.Remove(faction);
			}
			else
			{
				this.favor.SetOrAdd(faction, amount);
			}
			if (notifyOnFavorChanged)
			{
				this.OnFavorChanged(faction, oldAmount, amount);
			}
		}

		// Token: 0x060074E1 RID: 29921 RVA: 0x0004EE34 File Offset: 0x0004D034
		[Obsolete("Will be removed in the future.")]
		public void SetFavor(Faction faction, int amount)
		{
			this.SetFavor_NewTemp(faction, amount, true);
		}

		// Token: 0x060074E2 RID: 29922 RVA: 0x002395F4 File Offset: 0x002377F4
		private void OnFavorChanged(Faction faction, int oldAmount, int newAmount)
		{
			RoyalTitleDef titleAwardedWhenUpdating = this.GetTitleAwardedWhenUpdating(faction, oldAmount);
			RoyalTitleDef titleAwardedWhenUpdating2 = this.GetTitleAwardedWhenUpdating(faction, newAmount);
			RoyalTitle currentTitleInFaction = this.GetCurrentTitleInFaction(faction);
			RoyalTitleDef royalTitleDef = null;
			if (currentTitleInFaction != null && titleAwardedWhenUpdating != null)
			{
				royalTitleDef = ((currentTitleInFaction.def.seniority >= titleAwardedWhenUpdating.seniority) ? currentTitleInFaction.def : titleAwardedWhenUpdating);
			}
			else if (currentTitleInFaction != null)
			{
				royalTitleDef = currentTitleInFaction.def;
			}
			else if (titleAwardedWhenUpdating != null)
			{
				royalTitleDef = titleAwardedWhenUpdating;
			}
			if (royalTitleDef != titleAwardedWhenUpdating2)
			{
				List<RoyalTitleDef> list = new List<RoyalTitleDef>();
				int previousTitleSeniority = (royalTitleDef == null) ? -1 : royalTitleDef.seniority;
				int newAwardedSeniority = (titleAwardedWhenUpdating2 == null) ? -1 : titleAwardedWhenUpdating2.seniority;
				if (previousTitleSeniority < newAwardedSeniority)
				{
					list.AddRange(from t in faction.def.RoyalTitlesAllInSeniorityOrderForReading
					where t.seniority > previousTitleSeniority && t.seniority <= newAwardedSeniority
					select t);
				}
				else
				{
					list.Add(titleAwardedWhenUpdating2);
				}
				foreach (RoyalTitleDef royalTitleDef2 in list)
				{
					if (royalTitleDef2 != null && this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
					{
						royalTitleDef2.AwardWorker.OnPreAward(this.pawn, faction, royalTitleDef, royalTitleDef2);
					}
					QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "TitleAwardedWhenUpdatingChanged");
					if (royalTitleDef2 != null && this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
					{
						royalTitleDef2.AwardWorker.DoAward(this.pawn, faction, royalTitleDef, royalTitleDef2);
					}
					royalTitleDef = royalTitleDef2;
				}
				if (previousTitleSeniority < newAwardedSeniority)
				{
					RoyalTitleUtility.EndExistingBestowingCeremonyQuest(this.pawn, faction);
					RoyalTitleUtility.GenerateBestowingCeremonyQuest(this.pawn, faction);
				}
			}
		}

		// Token: 0x060074E3 RID: 29923 RVA: 0x0004EE3F File Offset: 0x0004D03F
		public RoyalTitleDef GetCurrentTitle(Faction faction)
		{
			RoyalTitle currentTitleInFaction = this.GetCurrentTitleInFaction(faction);
			if (currentTitleInFaction == null)
			{
				return null;
			}
			return currentTitleInFaction.def;
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x002397C4 File Offset: 0x002379C4
		public RoyalTitle GetCurrentTitleInFaction(Faction faction)
		{
			if (faction == null)
			{
				Log.Error("Cannot get current title for null faction.", false);
			}
			int num = this.FindFactionTitleIndex(faction, false);
			if (num == -1)
			{
				return null;
			}
			return this.titles[num];
		}

		// Token: 0x060074E5 RID: 29925 RVA: 0x002397FC File Offset: 0x002379FC
		public void SetTitle(Faction faction, RoyalTitleDef title, bool grantRewards, bool rewardsOnlyForNewestTitle = false, bool sendLetter = true)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Honor is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 7445532, false);
				return;
			}
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			this.OnPreTitleChanged(faction, currentTitle, title, sendLetter);
			if (grantRewards)
			{
				this.ApplyRewardsForTitle(faction, currentTitle, title, rewardsOnlyForNewestTitle);
			}
			int index = this.FindFactionTitleIndex(faction, true);
			if (title != null)
			{
				this.titles[index].def = title;
				this.titles[index].receivedTick = GenTicks.TicksGame;
			}
			else
			{
				this.titles.RemoveAt(index);
			}
			this.SetFavor_NewTemp(faction, 0, true);
			this.OnPostTitleChanged_NewTemp(faction, currentTitle, title);
		}

		// Token: 0x060074E6 RID: 29926 RVA: 0x00239898 File Offset: 0x00237A98
		public void ReduceTitle(Faction faction)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			if (currentTitle == null || !currentTitle.Awardable)
			{
				return;
			}
			for (int i = this.factionPermits.Count - 1; i >= 0; i--)
			{
				if (this.factionPermits[i].Title == currentTitle)
				{
					this.factionPermits.RemoveAt(i);
				}
			}
			RoyalTitleDef previousTitle = currentTitle.GetPreviousTitle(faction);
			this.OnPreTitleChanged(faction, currentTitle, previousTitle, true);
			this.CleanupThoughts(currentTitle);
			this.CleanupThoughts(previousTitle);
			if (currentTitle.awardThought != null && this.pawn.needs.mood != null)
			{
				Thought_MemoryRoyalTitle thought_MemoryRoyalTitle = (Thought_MemoryRoyalTitle)ThoughtMaker.MakeThought(currentTitle.lostThought);
				thought_MemoryRoyalTitle.titleDef = currentTitle;
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_MemoryRoyalTitle, null);
			}
			int index = this.FindFactionTitleIndex(faction, false);
			if (previousTitle == null)
			{
				this.titles.RemoveAt(index);
			}
			else
			{
				this.titles[index].def = previousTitle;
			}
			this.SetFavor_NewTemp(faction, 0, true);
			this.OnPostTitleChanged_NewTemp(faction, currentTitle, previousTitle);
		}

		// Token: 0x060074E7 RID: 29927 RVA: 0x0004EE53 File Offset: 0x0004D053
		public Pawn GetHeir(Faction faction)
		{
			if (this.heirs != null && this.heirs.ContainsKey(faction))
			{
				return this.heirs[faction];
			}
			return null;
		}

		// Token: 0x060074E8 RID: 29928 RVA: 0x0004EE79 File Offset: 0x0004D079
		public void SetHeir(Pawn heir, Faction faction)
		{
			if (this.heirs != null)
			{
				this.heirs[faction] = heir;
			}
		}

		// Token: 0x060074E9 RID: 29929 RVA: 0x002399A8 File Offset: 0x00237BA8
		public void AssignHeirIfNone(RoyalTitleDef t, Faction faction)
		{
			if (!this.heirs.ContainsKey(faction) && t.Awardable && this.pawn.FactionOrExtraMiniOrHomeFaction != Faction.Empire)
			{
				this.SetHeir(t.GetInheritanceWorker(faction).FindHeir(faction, this.pawn, t), faction);
			}
		}

		// Token: 0x060074EA RID: 29930 RVA: 0x002399F8 File Offset: 0x00237BF8
		public void RoyaltyTrackerTick()
		{
			Faction faction;
			if (this.pawn.IsHashIntervalTick(900000) && RoyalTitleUtility.ShouldGetBestowingCeremonyQuest(this.pawn, out faction))
			{
				RoyalTitleUtility.GenerateBestowingCeremonyQuest(this.pawn, faction);
			}
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				allTitlesInEffectForReading[i].RoyalTitleTick_NewTemp();
			}
			if (!this.pawn.Spawned || this.pawn.RaceProps.Animal)
			{
				return;
			}
			this.factionHeirsToClearTmp.Clear();
			foreach (KeyValuePair<Faction, Pawn> keyValuePair in this.heirs)
			{
				RoyalTitleDef currentTitle = this.GetCurrentTitle(keyValuePair.Key);
				if (currentTitle != null && currentTitle.canBeInherited)
				{
					Pawn value = keyValuePair.Value;
					if (value != null && value.Dead)
					{
						Find.LetterStack.ReceiveLetter("LetterTitleHeirLostLabel".Translate(), "LetterTitleHeirLost".Translate(this.pawn.Named("HOLDER"), value.Named("HEIR"), keyValuePair.Key.Named("FACTION")), LetterDefOf.NegativeEvent, this.pawn, null, null, null, null);
						this.factionHeirsToClearTmp.Add(keyValuePair.Key);
					}
				}
			}
			foreach (Faction key in this.factionHeirsToClearTmp)
			{
				this.heirs[key] = null;
			}
			foreach (FactionPermit factionPermit in this.factionPermits)
			{
				if (factionPermit.LastUsedTick > 0 && Find.TickManager.TicksGame == factionPermit.LastUsedTick + factionPermit.Permit.CooldownTicks)
				{
					Messages.Message("MessagePermitCooldownFinished".Translate(this.pawn, factionPermit.Permit.LabelCap), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
		}

		// Token: 0x060074EB RID: 29931 RVA: 0x00239C64 File Offset: 0x00237E64
		public void IssueDecree(bool causedByMentalBreak, string mentalBreakReason = null)
		{
			Pawn_RoyaltyTracker.<>c__DisplayClass62_0 CS$<>8__locals1 = new Pawn_RoyaltyTracker.<>c__DisplayClass62_0();
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Decrees are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 281653, false);
				return;
			}
			Pawn_RoyaltyTracker.<>c__DisplayClass62_0 CS$<>8__locals2 = CS$<>8__locals1;
			IIncidentTarget mapHeld = this.pawn.MapHeld;
			CS$<>8__locals2.target = (mapHeld ?? Find.World);
			QuestScriptDef questScriptDef;
			if (this.PossibleDecreeQuests.TryRandomElementByWeight((QuestScriptDef x) => NaturalRandomQuestChooser.GetNaturalDecreeSelectionWeight(x, CS$<>8__locals1.target.StoryState), out questScriptDef))
			{
				this.lastDecreeTicks = Find.TickManager.TicksGame;
				Slate slate = new Slate();
				slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(CS$<>8__locals1.target), false);
				slate.Set<Pawn>("asker", this.pawn, false);
				Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questScriptDef, slate);
				CS$<>8__locals1.target.StoryState.RecordDecreeFired(questScriptDef);
				string str;
				if (causedByMentalBreak)
				{
					str = "WildDecree".Translate() + ": " + this.pawn.LabelShortCap;
				}
				else
				{
					str = "LetterLabelRandomDecree".Translate(this.pawn);
				}
				string text;
				if (causedByMentalBreak)
				{
					text = "LetterDecreeMentalBreak".Translate(this.pawn);
				}
				else
				{
					text = "LetterRandomDecree".Translate(this.pawn);
				}
				if (mentalBreakReason != null)
				{
					text = text + "\n\n" + mentalBreakReason;
				}
				text += "\n\n" + "LetterDecree_Quest".Translate(quest.name);
				ChoiceLetter let = LetterMaker.MakeLetter(str, text, IncidentDefOf.GiveQuest_Random.letterDef, LookTargets.Invalid, null, quest, null);
				Find.LetterStack.ReceiveLetter(let, null);
			}
		}

		// Token: 0x060074EC RID: 29932 RVA: 0x00239E20 File Offset: 0x00238020
		private void CleanupThoughts(RoyalTitleDef title)
		{
			if (title == null)
			{
				return;
			}
			if (title.awardThought != null && this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(title.awardThought);
			}
			if (title.lostThought != null && this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(title.lostThought);
			}
		}

		// Token: 0x060074ED RID: 29933 RVA: 0x00239ECC File Offset: 0x002380CC
		public static void MakeLetterTextForTitleChange(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle, out string headline, out string body)
		{
			if (currentTitle == null || faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) < faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle))
			{
				headline = "LetterGainedRoyalTitle".Translate(pawn.Named("PAWN"), faction.Named("FACTION"), newTitle.GetLabelCapFor(pawn).Named("TITLE")).Resolve();
			}
			else
			{
				headline = "LetterLostRoyalTitle".Translate(pawn.Named("PAWN"), faction.Named("FACTION"), currentTitle.GetLabelCapFor(pawn).Named("TITLE")).Resolve();
			}
			body = RoyalTitleUtility.BuildDifferenceExplanationText(currentTitle, newTitle, faction, pawn);
			body = body.Resolve().TrimEndNewlines();
		}

		// Token: 0x060074EE RID: 29934 RVA: 0x00239FA0 File Offset: 0x002381A0
		public static string MakeLetterTextForTitleChange(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
			string str;
			string text;
			Pawn_RoyaltyTracker.MakeLetterTextForTitleChange(pawn, faction, currentTitle, newTitle, out str, out text);
			if (text.Length > 0)
			{
				text = "\n\n" + text;
			}
			return str + text;
		}

		// Token: 0x060074EF RID: 29935 RVA: 0x00239FD8 File Offset: 0x002381D8
		public void ResetPermitsAndPoints(Faction faction, RoyalTitleDef currentTitle)
		{
			if (currentTitle == null)
			{
				return;
			}
			for (int i = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) + 1 - 1; i >= 0; i--)
			{
				RoyalTitleDef royalTitleDef = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[i];
				List<FactionPermit> list = this.PermitsFromFaction(faction);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Title == royalTitleDef)
					{
						this.factionPermits.Remove(list[j]);
					}
				}
				this.RecalculatePermitPoints(faction);
			}
		}

		// Token: 0x060074F0 RID: 29936 RVA: 0x0023A060 File Offset: 0x00238260
		private void OnPreTitleChanged(Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle, bool sendLetter = true)
		{
			this.AssignHeirIfNone(newTitle, faction);
			if (Current.ProgramState == ProgramState.Playing && sendLetter && this.pawn.IsColonist)
			{
				TaggedString label = null;
				if (currentTitle == null || faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) < faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle))
				{
					label = "LetterLabelGainedRoyalTitle".Translate(this.pawn.Named("PAWN"), newTitle.GetLabelCapFor(this.pawn).Named("TITLE"));
				}
				else
				{
					label = "LetterLabelLostRoyalTitle".Translate(this.pawn.Named("PAWN"), currentTitle.GetLabelCapFor(this.pawn).Named("TITLE"));
				}
				Find.LetterStack.ReceiveLetter(label, Pawn_RoyaltyTracker.MakeLetterTextForTitleChange(this.pawn, faction, currentTitle, newTitle), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
			}
			if (currentTitle != null)
			{
				for (int i = 0; i < currentTitle.grantedAbilities.Count; i++)
				{
					this.pawn.abilities.RemoveAbility(currentTitle.grantedAbilities[i]);
				}
			}
		}

		// Token: 0x060074F1 RID: 29937 RVA: 0x0023A190 File Offset: 0x00238390
		private void OnPostTitleChanged_NewTemp(Faction faction, RoyalTitleDef prevTitle, RoyalTitleDef newTitle)
		{
			this.pawn.Notify_DisabledWorkTypesChanged();
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				needs.AddOrRemoveNeedsAsAppropriate();
			}
			if (newTitle != null)
			{
				if (newTitle.disabledJoyKinds != null && this.pawn.jobs != null && RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(this.pawn))
				{
					foreach (JoyKindDef joyKind in newTitle.disabledJoyKinds)
					{
						this.pawn.jobs.Notify_JoyKindDisabled(joyKind);
					}
				}
				for (int i = 0; i < newTitle.grantedAbilities.Count; i++)
				{
					this.pawn.abilities.GainAbility(newTitle.grantedAbilities[i]);
				}
				this.UpdateHighestTitleAchieved(faction, newTitle);
			}
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "TitleChanged", this.pawn.Named("SUBJECT"));
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
			this.RecalculatePermitPoints(faction);
		}

		// Token: 0x060074F2 RID: 29938 RVA: 0x0004EE90 File Offset: 0x0004D090
		[Obsolete("Will be removed in a future update")]
		private void OnPostTitleChanged(Faction faction, RoyalTitleDef newTitle)
		{
			this.OnPostTitleChanged_NewTemp(faction, newTitle, newTitle);
		}

		// Token: 0x060074F3 RID: 29939 RVA: 0x0023A2A8 File Offset: 0x002384A8
		private void UpdateRoyalTitle_NewTemp(Faction faction, bool sendLetter = true, RoyalTitleDef updateTo = null)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			if (currentTitle != null && !currentTitle.Awardable)
			{
				return;
			}
			RoyalTitleDef nextTitle = currentTitle.GetNextTitle(faction);
			if (nextTitle == null)
			{
				return;
			}
			int num = this.GetFavor(faction);
			if (num >= nextTitle.favorCost)
			{
				this.OnPreTitleChanged(faction, currentTitle, nextTitle, sendLetter);
				this.SetFavor_NewTemp(faction, num - nextTitle.favorCost, false);
				int index = this.FindFactionTitleIndex(faction, true);
				this.titles[index].def = nextTitle;
				this.CleanupThoughts(currentTitle);
				this.CleanupThoughts(nextTitle);
				if (nextTitle.awardThought != null && this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					Thought_MemoryRoyalTitle thought_MemoryRoyalTitle = (Thought_MemoryRoyalTitle)ThoughtMaker.MakeThought(nextTitle.awardThought);
					thought_MemoryRoyalTitle.titleDef = nextTitle;
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_MemoryRoyalTitle, null);
				}
				if (nextTitle == updateTo)
				{
					return;
				}
				this.UpdateRoyalTitle_NewTemp(faction, sendLetter, null);
			}
		}

		// Token: 0x060074F4 RID: 29940 RVA: 0x0004EE9B File Offset: 0x0004D09B
		[Obsolete("Will be removed in a future update.")]
		private void UpdateRoyalTitle(Faction faction)
		{
			this.UpdateRoyalTitle_NewTemp(faction, true, null);
		}

		// Token: 0x060074F5 RID: 29941 RVA: 0x0023A3A0 File Offset: 0x002385A0
		public List<Thing> ApplyRewardsForTitle(Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle, bool onlyForNewestTitle = false)
		{
			List<Thing> list = new List<Thing>();
			List<ThingCount> list2 = new List<ThingCount>();
			if (newTitle != null && newTitle.Awardable && this.pawn.IsColonist && this.NewHighestTitle(faction, newTitle))
			{
				int num = ((currentTitle != null) ? faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) : 0) + 1;
				int num2 = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle);
				if (onlyForNewestTitle)
				{
					num = num2;
				}
				IntVec3 dropCenter = IntVec3.Invalid;
				Map mapHeld = this.pawn.MapHeld;
				if (mapHeld != null)
				{
					if (mapHeld.IsPlayerHome)
					{
						dropCenter = DropCellFinder.TradeDropSpot(mapHeld);
					}
					else if (!DropCellFinder.TryFindDropSpotNear(this.pawn.Position, mapHeld, out dropCenter, false, false, true, null))
					{
						dropCenter = DropCellFinder.RandomDropSpot(mapHeld);
					}
				}
				for (int i = num; i <= num2; i++)
				{
					RoyalTitleDef royalTitleDef = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[i];
					if (royalTitleDef.rewards != null)
					{
						List<Thing> list3 = royalTitleDef.rewards.Select(delegate(ThingDefCountClass r)
						{
							Thing thing = ThingMaker.MakeThing(r.thingDef, null);
							thing.stackCount = r.count;
							return thing;
						}).ToList<Thing>();
						for (int j = 0; j < list3.Count; j++)
						{
							if (list3[j].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								break;
							}
						}
						if (this.pawn.Spawned)
						{
							DropPodUtility.DropThingsNear(dropCenter, mapHeld, list3, 110, false, false, false, false);
						}
						else
						{
							foreach (Thing item in list3)
							{
								this.pawn.inventory.TryAddItemNotForSale(item);
							}
						}
						for (int k = 0; k < list3.Count; k++)
						{
							list2.Add(new ThingCount(list3[k], list3[k].stackCount));
						}
						list.AddRange(list3);
					}
				}
				if (list.Count > 0)
				{
					TaggedString text = "LetterRewardsForNewTitle".Translate(this.pawn.Named("PAWN"), faction.Named("FACTION"), newTitle.GetLabelCapFor(this.pawn).Named("TITLE")) + "\n\n" + GenLabel.ThingsLabel(list2, "  - ", true) + "\n\n" + (this.pawn.Spawned ? "LetterRewardsForNewTitleDeliveryBase" : "LetterRewardsForNewTitleDeliveryDirect").Translate(this.pawn.Named("PAWN"));
					Find.LetterStack.ReceiveLetter("LetterLabelRewardsForNewTitle".Translate(), text, LetterDefOf.PositiveEvent, list, null, null, null, null);
				}
			}
			return list;
		}

		// Token: 0x060074F6 RID: 29942 RVA: 0x0004EEA6 File Offset: 0x0004D0A6
		private void UpdateHighestTitleAchieved(Faction faction, RoyalTitleDef title)
		{
			if (!this.highestTitles.ContainsKey(faction))
			{
				this.highestTitles.Add(faction, title);
				return;
			}
			if (this.NewHighestTitle(faction, title))
			{
				this.highestTitles[faction] = title;
			}
		}

		// Token: 0x060074F7 RID: 29943 RVA: 0x0023A688 File Offset: 0x00238888
		public bool NewHighestTitle(Faction faction, RoyalTitleDef newTitle)
		{
			if (this.highestTitles == null)
			{
				this.highestTitles = new Dictionary<Faction, RoyalTitleDef>();
			}
			if (!this.highestTitles.ContainsKey(faction))
			{
				return true;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(this.highestTitles[faction]);
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(newTitle) > num;
		}

		// Token: 0x060074F8 RID: 29944 RVA: 0x0004EEDB File Offset: 0x0004D0DB
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.PermitPointsAvailable && this.pawn.Faction == Faction.OfPlayer)
			{
				Faction key = this.permitPoints.FirstOrDefault<KeyValuePair<Faction, int>>().Key;
				if (key == null)
				{
					yield break;
				}
				yield return new Command_Action
				{
					defaultLabel = "ChooseRoyalPermit".Translate(),
					defaultDesc = "ChooseRoyalPermit_Desc".Translate(),
					icon = key.def.FactionIcon,
					defaultIconColor = key.Color,
					action = delegate()
					{
						this.OpenPermitWindow();
					}
				};
			}
			yield break;
		}

		// Token: 0x060074F9 RID: 29945 RVA: 0x0023A6EC File Offset: 0x002388EC
		public void OpenPermitWindow()
		{
			Dialog_InfoCard dialog_InfoCard = new Dialog_InfoCard(this.pawn);
			dialog_InfoCard.SetTab(Dialog_InfoCard.InfoCardTab.Permits);
			Find.WindowStack.Add(dialog_InfoCard);
		}

		// Token: 0x060074FA RID: 29946 RVA: 0x0023A718 File Offset: 0x00238918
		public void Notify_PawnKilled()
		{
			if (PawnGenerator.IsBeingGenerated(this.pawn) || this.AllTitlesForReading.Count == 0)
			{
				return;
			}
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				stringBuilder.AppendLine("LetterTitleInheritance_Base".Translate(this.pawn.Named("PAWN")));
				stringBuilder.AppendLine();
				foreach (RoyalTitle royalTitle in this.AllTitlesForReading)
				{
					if (royalTitle.def.canBeInherited)
					{
						if (this.pawn.IsFreeColonist && !this.pawn.IsQuestLodger())
						{
							flag = true;
						}
						RoyalTitleInheritanceOutcome outcome;
						if (royalTitle.def.TryInherit(this.pawn, royalTitle.faction, out outcome))
						{
							if (outcome.HeirHasTitle && !outcome.heirTitleHigher)
							{
								stringBuilder.AppendLineTagged("LetterTitleInheritance_AsReplacement".Translate(this.pawn.Named("PAWN"), royalTitle.faction.Named("FACTION"), outcome.heir.Named("HEIR"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE"), outcome.heirCurrentTitle.GetLabelFor(outcome.heir).Named("REPLACEDTITLE")).CapitalizeFirst());
								stringBuilder.AppendLine();
							}
							else if (outcome.heirTitleHigher)
							{
								stringBuilder.AppendLineTagged("LetterTitleInheritance_NoEffectHigherTitle".Translate(this.pawn.Named("PAWN"), royalTitle.faction.Named("FACTION"), outcome.heir.Named("HEIR"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE"), outcome.heirCurrentTitle.GetLabelFor(outcome.heir).Named("HIGHERTITLE")).CapitalizeFirst());
								stringBuilder.AppendLine();
							}
							else
							{
								stringBuilder.AppendLineTagged("LetterTitleInheritance_WasInherited".Translate(this.pawn.Named("PAWN"), royalTitle.faction.Named("FACTION"), outcome.heir.Named("HEIR"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE")).CapitalizeFirst());
								stringBuilder.AppendLine();
							}
							if (!outcome.heirTitleHigher)
							{
								RoyalTitle titleLocal = royalTitle;
								Pawn_RoyaltyTracker.tmpInheritedTitles.Add(delegate
								{
									int num = titleLocal.def.favorCost;
									RoyalTitleDef previousTitle_IncludeNonRewardable = titleLocal.def.GetPreviousTitle_IncludeNonRewardable(titleLocal.faction);
									int num2 = 1000;
									while (previousTitle_IncludeNonRewardable != null)
									{
										num += previousTitle_IncludeNonRewardable.favorCost;
										previousTitle_IncludeNonRewardable = previousTitle_IncludeNonRewardable.GetPreviousTitle_IncludeNonRewardable(titleLocal.faction);
										num2--;
										if (num2 <= 0)
										{
											Log.ErrorOnce("Iterations limit exceeded while getting favor for inheritance.", 91727191, false);
											break;
										}
									}
									outcome.heir.royalty.GainFavor(titleLocal.faction, num);
									titleLocal.wasInherited = true;
								});
								if (outcome.heir.IsFreeColonist && !outcome.heir.IsQuestLodger())
								{
									flag = true;
								}
							}
						}
						else
						{
							stringBuilder.AppendLineTagged("LetterTitleInheritance_NoHeirFound".Translate(this.pawn.Named("PAWN"), royalTitle.def.GetLabelFor(this.pawn).Named("TITLE"), royalTitle.faction.Named("FACTION")).CapitalizeFirst());
						}
					}
				}
				if (stringBuilder.Length > 0 && flag)
				{
					Find.LetterStack.ReceiveLetter("LetterTitleInheritance".Translate(), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.PositiveEvent, null);
				}
				foreach (Action action in Pawn_RoyaltyTracker.tmpInheritedTitles)
				{
					action();
				}
			}
			finally
			{
				Pawn_RoyaltyTracker.tmpInheritedTitles.Clear();
			}
		}

		// Token: 0x060074FB RID: 29947 RVA: 0x0023AB64 File Offset: 0x00238D64
		public void Notify_Resurrected()
		{
			foreach (Faction faction in (from t in this.titles
			select t.faction).Distinct<Faction>().ToList<Faction>())
			{
				int index = this.FindFactionTitleIndex(faction, false);
				if (this.titles[index].wasInherited)
				{
					this.SetTitle(faction, null, false, false, false);
				}
			}
		}

		// Token: 0x060074FC RID: 29948 RVA: 0x0023AC08 File Offset: 0x00238E08
		public Gizmo RoyalAidGizmo()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandCallRoyalAid".Translate();
			command_Action.defaultDesc = "CommandCallRoyalAidDesc".Translate();
			command_Action.icon = Pawn_RoyaltyTracker.CommandTex;
			if (Find.Selector.NumSelected > 1)
			{
				Command_Action command_Action2 = command_Action;
				command_Action2.defaultLabel = command_Action2.defaultLabel + " (" + this.pawn.LabelShort + ")";
			}
			if (this.pawn.Downed)
			{
				command_Action.Disable("CommandDisabledUnconscious".TranslateWithBackup("CommandCallRoyalAidUnconscious").Formatted(this.pawn));
			}
			if (this.pawn.IsQuestLodger())
			{
				command_Action.Disable("CommandCallRoyalAidLodger".Translate());
			}
			command_Action.action = delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FactionPermit factionPermit in this.factionPermits)
				{
					IEnumerable<FloatMenuOption> royalAidOptions = factionPermit.Permit.Worker.GetRoyalAidOptions(this.pawn.MapHeld, this.pawn, factionPermit.Faction);
					if (royalAidOptions != null)
					{
						list.AddRange(royalAidOptions);
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			};
			return command_Action;
		}

		// Token: 0x060074FD RID: 29949 RVA: 0x0004EEEB File Offset: 0x0004D0EB
		public bool CanRequireThroneroom()
		{
			return this.pawn.IsFreeColonist && this.allowRoomRequirements && !this.pawn.IsQuestLodger();
		}

		// Token: 0x060074FE RID: 29950 RVA: 0x0023ACF4 File Offset: 0x00238EF4
		public RoyalTitle HighestTitleWithThroneRoomRequirements()
		{
			if (!this.CanRequireThroneroom())
			{
				return null;
			}
			RoyalTitle royalTitle = null;
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				if (!allTitlesInEffectForReading[i].def.throneRoomRequirements.EnumerableNullOrEmpty<RoomRequirement>() && (royalTitle == null || allTitlesInEffectForReading[i].def.seniority > royalTitle.def.seniority))
				{
					royalTitle = allTitlesInEffectForReading[i];
				}
			}
			return royalTitle;
		}

		// Token: 0x060074FF RID: 29951 RVA: 0x0004EF12 File Offset: 0x0004D112
		public IEnumerable<string> GetUnmetThroneroomRequirements(bool includeOnGracePeriod = true, bool onlyOnGracePeriod = false)
		{
			if (this.pawn.ownership.AssignedThrone == null)
			{
				yield break;
			}
			RoyalTitle highestTitle = this.HighestTitleWithThroneRoomRequirements();
			if (highestTitle == null)
			{
				yield break;
			}
			Room throneRoom = this.pawn.ownership.AssignedThrone.GetRoom(RegionType.Set_Passable);
			if (throneRoom == null)
			{
				yield break;
			}
			bool roomValid = RoomRoleWorker_ThroneRoom.Validate(throneRoom) == null;
			RoyalTitleDef prevTitle = highestTitle.def.GetPreviousTitle(highestTitle.faction);
			foreach (RoomRequirement roomRequirement in highestTitle.def.throneRoomRequirements)
			{
				if (!roomValid || !roomRequirement.Met(throneRoom, this.pawn))
				{
					bool flag = highestTitle.RoomRequirementGracePeriodActive(this.pawn);
					bool flag2 = prevTitle != null && !prevTitle.HasSameThroneroomRequirement(roomRequirement);
					if ((!onlyOnGracePeriod || (flag2 && flag)) && (!flag || !flag2 || includeOnGracePeriod))
					{
						yield return roomRequirement.LabelCap(throneRoom);
					}
				}
			}
			List<RoomRequirement>.Enumerator enumerator = default(List<RoomRequirement>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06007500 RID: 29952 RVA: 0x0004EF30 File Offset: 0x0004D130
		public bool CanRequireBedroom()
		{
			return this.allowRoomRequirements && !this.pawn.IsPrisoner;
		}

		// Token: 0x06007501 RID: 29953 RVA: 0x0023AD68 File Offset: 0x00238F68
		public RoyalTitle HighestTitleWithBedroomRequirements()
		{
			if (!this.CanRequireBedroom())
			{
				return null;
			}
			RoyalTitle royalTitle = null;
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				if (!allTitlesInEffectForReading[i].def.GetBedroomRequirements(this.pawn).EnumerableNullOrEmpty<RoomRequirement>() && (royalTitle == null || allTitlesInEffectForReading[i].def.seniority > royalTitle.def.seniority))
				{
					royalTitle = allTitlesInEffectForReading[i];
				}
			}
			return royalTitle;
		}

		// Token: 0x06007502 RID: 29954 RVA: 0x0004EF4A File Offset: 0x0004D14A
		public IEnumerable<string> GetUnmetBedroomRequirements(bool includeOnGracePeriod = true, bool onlyOnGracePeriod = false)
		{
			RoyalTitle royalTitle = this.HighestTitleWithBedroomRequirements();
			if (royalTitle == null)
			{
				yield break;
			}
			bool gracePeriodActive = royalTitle.RoomRequirementGracePeriodActive(this.pawn);
			RoyalTitleDef prevTitle = royalTitle.def.GetPreviousTitle(royalTitle.faction);
			if (!this.HasPersonalBedroom())
			{
				yield break;
			}
			Room bedroom = this.pawn.ownership.OwnedRoom;
			foreach (RoomRequirement roomRequirement in royalTitle.def.GetBedroomRequirements(this.pawn))
			{
				if (!roomRequirement.Met(bedroom, null))
				{
					bool flag = prevTitle != null && !prevTitle.HasSameBedroomRequirement(roomRequirement);
					if ((!onlyOnGracePeriod || (flag && gracePeriodActive)) && (!gracePeriodActive || !flag || includeOnGracePeriod))
					{
						yield return roomRequirement.LabelCap(bedroom);
					}
				}
			}
			IEnumerator<RoomRequirement> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007503 RID: 29955 RVA: 0x0023ADE4 File Offset: 0x00238FE4
		public void RecalculatePermitPoints(Faction faction)
		{
			int num = 0;
			RoyalTitleDef royalTitleDef = this.GetCurrentTitle(faction);
			int num2 = 200;
			while (royalTitleDef != null)
			{
				num += royalTitleDef.permitPointsAwarded;
				royalTitleDef = royalTitleDef.GetPreviousTitle_IncludeNonRewardable(faction);
				num2--;
				if (num2 <= 0)
				{
					Log.ErrorOnce("GetPermitPoints exceeded iterations limit.", 1837503, false);
					break;
				}
			}
			for (int i = 0; i < this.factionPermits.Count; i++)
			{
				if (this.factionPermits[i].Faction == faction)
				{
					RoyalTitlePermitDef royalTitlePermitDef = this.factionPermits[i].Permit;
					num2 = 200;
					while (royalTitlePermitDef != null)
					{
						num -= royalTitlePermitDef.permitPointCost;
						royalTitlePermitDef = royalTitlePermitDef.prerequisite;
						num2--;
						if (num2 <= 0)
						{
							Log.ErrorOnce("GetPermitPoints exceeded iterations limit.", 1837503, false);
							break;
						}
					}
				}
			}
			this.permitPoints[faction] = num;
		}

		// Token: 0x06007504 RID: 29956 RVA: 0x0023AEB4 File Offset: 0x002390B4
		public bool HasPersonalBedroom()
		{
			Building_Bed ownedBed = this.pawn.ownership.OwnedBed;
			if (ownedBed == null)
			{
				return false;
			}
			Room ownedRoom = this.pawn.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return false;
			}
			foreach (Building_Bed building_Bed in ownedRoom.ContainedBeds)
			{
				if (building_Bed != ownedBed && building_Bed.OwnersForReading.Any((Pawn p) => p != this.pawn && !p.RaceProps.Animal && !LovePartnerRelationUtility.LovePartnerRelationExists(p, this.pawn)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007505 RID: 29957 RVA: 0x0023AF4C File Offset: 0x0023914C
		public void ExposeData()
		{
			Scribe_Collections.Look<RoyalTitle>(ref this.titles, "titles", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Faction, int>(ref this.favor, "favor", LookMode.Reference, LookMode.Value, ref this.tmpFavorFactions, ref this.tmpAmounts);
			Scribe_Values.Look<int>(ref this.lastDecreeTicks, "lastDecreeTicks", -999999, false);
			Scribe_Collections.Look<Faction, RoyalTitleDef>(ref this.highestTitles, "highestTitles", LookMode.Reference, LookMode.Def, ref this.tmpHighestTitleFactions, ref this.tmpTitleDefs);
			Scribe_Collections.Look<Faction, Pawn>(ref this.heirs, "heirs", LookMode.Reference, LookMode.Reference, ref this.tmpHeirFactions, ref this.tmpPawns);
			Scribe_Values.Look<bool>(ref this.allowRoomRequirements, "allowRoomRequirements", true, false);
			Scribe_Values.Look<bool>(ref this.allowApparelRequirements, "allowApparelRequirements", true, false);
			Scribe_Collections.Look<Faction, int>(ref this.permitPoints, "permitPoints", LookMode.Reference, LookMode.Value, ref this.tmpPermitFactions, ref this.tmpPermitPointsAmounts);
			Scribe_Collections.Look<FactionPermit>(ref this.factionPermits, "permits", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.titles.RemoveAll((RoyalTitle x) => x.def == null) != 0)
				{
					Log.Error("Some RoyalTitles had null defs after loading.", false);
				}
				foreach (RoyalTitle royalTitle in this.titles)
				{
					royalTitle.pawn = this.pawn;
				}
				if (this.heirs == null)
				{
					this.heirs = new Dictionary<Faction, Pawn>();
				}
				if (this.factionPermits == null)
				{
					this.factionPermits = new List<FactionPermit>();
				}
				if (this.permitPoints == null)
				{
					this.permitPoints = new Dictionary<Faction, int>();
					for (int i = 0; i < this.AllTitlesInEffectForReading.Count; i++)
					{
						this.RecalculatePermitPoints(this.AllTitlesInEffectForReading[i].faction);
					}
				}
				if (this.factionPermits.RemoveAll((FactionPermit x) => DefDatabase<RoyalTitlePermitDef>.AllDefs.Any((RoyalTitlePermitDef y) => y.prerequisite == x.Permit && this.HasPermit(y, x.Faction) && this.HasPermit(y.prerequisite, x.Faction))) != 0)
				{
					Log.Error("Removed some null permits.", false);
				}
				foreach (RoyalTitle royalTitle2 in this.titles)
				{
					this.AssignHeirIfNone(royalTitle2.def, royalTitle2.faction);
				}
				for (int j = 0; j < this.AllTitlesInEffectForReading.Count; j++)
				{
					RoyalTitle royalTitle3 = this.AllTitlesInEffectForReading[j];
					for (int k = 0; k < royalTitle3.def.grantedAbilities.Count; k++)
					{
						this.pawn.abilities.GainAbility(royalTitle3.def.grantedAbilities[k]);
					}
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04004D0C RID: 19724
		public Pawn pawn;

		// Token: 0x04004D0D RID: 19725
		private List<RoyalTitle> titles = new List<RoyalTitle>();

		// Token: 0x04004D0E RID: 19726
		private Dictionary<Faction, int> favor = new Dictionary<Faction, int>();

		// Token: 0x04004D0F RID: 19727
		private Dictionary<Faction, RoyalTitleDef> highestTitles = new Dictionary<Faction, RoyalTitleDef>();

		// Token: 0x04004D10 RID: 19728
		private Dictionary<Faction, Pawn> heirs = new Dictionary<Faction, Pawn>();

		// Token: 0x04004D11 RID: 19729
		private List<FactionPermit> factionPermits = new List<FactionPermit>();

		// Token: 0x04004D12 RID: 19730
		private Dictionary<Faction, int> permitPoints = new Dictionary<Faction, int>();

		// Token: 0x04004D13 RID: 19731
		public int lastDecreeTicks = -999999;

		// Token: 0x04004D14 RID: 19732
		public bool allowRoomRequirements = true;

		// Token: 0x04004D15 RID: 19733
		public bool allowApparelRequirements = true;

		// Token: 0x04004D16 RID: 19734
		private static List<RoyalTitle> EmptyTitles = new List<RoyalTitle>();

		// Token: 0x04004D17 RID: 19735
		private const int BestowingCeremonyCheckInterval = 900000;

		// Token: 0x04004D18 RID: 19736
		private List<string> tmpDecreeTags = new List<string>();

		// Token: 0x04004D19 RID: 19737
		private static List<FactionPermit> tmpPermits = new List<FactionPermit>();

		// Token: 0x04004D1A RID: 19738
		private List<Faction> factionHeirsToClearTmp = new List<Faction>();

		// Token: 0x04004D1B RID: 19739
		private static List<Action> tmpInheritedTitles = new List<Action>();

		// Token: 0x04004D1C RID: 19740
		public static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallAid", true);

		// Token: 0x04004D1D RID: 19741
		private List<Faction> tmpFavorFactions;

		// Token: 0x04004D1E RID: 19742
		private List<Faction> tmpHighestTitleFactions;

		// Token: 0x04004D1F RID: 19743
		private List<Faction> tmpHeirFactions;

		// Token: 0x04004D20 RID: 19744
		private List<Faction> tmpPermitFactions;

		// Token: 0x04004D21 RID: 19745
		private List<int> tmpAmounts;

		// Token: 0x04004D22 RID: 19746
		private List<int> tmpPermitPointsAmounts;

		// Token: 0x04004D23 RID: 19747
		private List<Pawn> tmpPawns;

		// Token: 0x04004D24 RID: 19748
		private List<RoyalTitleDef> tmpTitleDefs;
	}
}
