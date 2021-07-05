using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E76 RID: 3702
	[StaticConstructorOnStartup]
	public class Pawn_RoyaltyTracker : IExposable
	{
		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06005650 RID: 22096 RVA: 0x001D3D54 File Offset: 0x001D1F54
		public List<RoyalTitle> AllTitlesForReading
		{
			get
			{
				return this.titles;
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06005651 RID: 22097 RVA: 0x001D3D5C File Offset: 0x001D1F5C
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

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06005652 RID: 22098 RVA: 0x001D3D78 File Offset: 0x001D1F78
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

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06005653 RID: 22099 RVA: 0x001D3DD5 File Offset: 0x001D1FD5
		public List<Ability> AllAbilitiesForReading
		{
			get
			{
				return this.abilities;
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06005654 RID: 22100 RVA: 0x001D3DDD File Offset: 0x001D1FDD
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

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06005655 RID: 22101 RVA: 0x001D3DF0 File Offset: 0x001D1FF0
		public bool PermitPointsAvailable
		{
			get
			{
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (this.GetPermitPoints(faction) > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06005656 RID: 22102 RVA: 0x001D3E4C File Offset: 0x001D204C
		public List<FactionPermit> AllFactionPermits
		{
			get
			{
				return this.factionPermits;
			}
		}

		// Token: 0x06005657 RID: 22103 RVA: 0x001D3E54 File Offset: 0x001D2054
		public Pawn_RoyaltyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x001D3EE0 File Offset: 0x001D20E0
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

		// Token: 0x06005659 RID: 22105 RVA: 0x001D3F6C File Offset: 0x001D216C
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

		// Token: 0x0600565A RID: 22106 RVA: 0x001D3FC8 File Offset: 0x001D21C8
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

		// Token: 0x0600565B RID: 22107 RVA: 0x001D4024 File Offset: 0x001D2224
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

		// Token: 0x0600565C RID: 22108 RVA: 0x001D4094 File Offset: 0x001D2294
		public void AddPermit(RoyalTitlePermitDef permit, Faction faction)
		{
			if (!ModLister.CheckRoyalty("Permit"))
			{
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
		}

		// Token: 0x0600565D RID: 22109 RVA: 0x001D414C File Offset: 0x001D234C
		public void RefundPermits(int favorCost, Faction faction)
		{
			if (this.favor[faction] < favorCost)
			{
				Log.Error("Not enough favor to refund permits.");
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
			}
		}

		// Token: 0x0600565E RID: 22110 RVA: 0x001D41E4 File Offset: 0x001D23E4
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

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x0600565F RID: 22111 RVA: 0x001D426C File Offset: 0x001D246C
		public bool HasAidPermit
		{
			get
			{
				return this.factionPermits.Any<FactionPermit>();
			}
		}

		// Token: 0x06005660 RID: 22112 RVA: 0x001D427C File Offset: 0x001D247C
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

		// Token: 0x06005661 RID: 22113 RVA: 0x001D42F8 File Offset: 0x001D24F8
		public int GetFavor(Faction faction)
		{
			int result;
			if (!this.favor.TryGetValue(faction, out result))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x06005662 RID: 22114 RVA: 0x001D4318 File Offset: 0x001D2518
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

		// Token: 0x06005663 RID: 22115 RVA: 0x001D4380 File Offset: 0x001D2580
		public int GetPermitPoints(Faction faction)
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
					Log.ErrorOnce("GetPermitPoints exceeded iterations limit.", 1837503);
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
							Log.ErrorOnce("GetPermitPoints exceeded iterations limit.", 1837503);
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x001D4444 File Offset: 0x001D2644
		public void GainFavor(Faction faction, int amount)
		{
			if (!ModLister.CheckRoyalty("Honor"))
			{
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
				this.TryUpdateTitle(faction, true, null);
			}
			this.OnFavorChanged(faction, oldAmount, num);
		}

		// Token: 0x06005665 RID: 22117 RVA: 0x001D44B0 File Offset: 0x001D26B0
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

		// Token: 0x06005666 RID: 22118 RVA: 0x001D4508 File Offset: 0x001D2708
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

		// Token: 0x06005667 RID: 22119 RVA: 0x001D4568 File Offset: 0x001D2768
		public bool CanUpdateTitle(Faction faction)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			int num = this.GetFavor(faction);
			RoyalTitleDef nextTitle = currentTitle.GetNextTitle(faction);
			return nextTitle != null && num >= nextTitle.favorCost;
		}

		// Token: 0x06005668 RID: 22120 RVA: 0x001D459C File Offset: 0x001D279C
		public bool TryUpdateTitle(Faction faction, bool sendLetter = true, RoyalTitleDef updateTo = null)
		{
			RoyalTitleDef currentTitle = this.GetCurrentTitle(faction);
			this.UpdateRoyalTitle(faction, sendLetter, updateTo);
			RoyalTitleDef currentTitle2 = this.GetCurrentTitle(faction);
			if (currentTitle2 != currentTitle)
			{
				this.ApplyRewardsForTitle(faction, currentTitle, currentTitle2, false);
				this.OnPostTitleChanged(faction, currentTitle, currentTitle2);
			}
			return currentTitle2 != currentTitle;
		}

		// Token: 0x06005669 RID: 22121 RVA: 0x001D45E4 File Offset: 0x001D27E4
		public bool TryRemoveFavor(Faction faction, int amount)
		{
			int num = this.GetFavor(faction);
			if (num < amount)
			{
				return false;
			}
			this.SetFavor(faction, num - amount, true);
			return true;
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x001D460C File Offset: 0x001D280C
		public void SetFavor(Faction faction, int amount, bool notifyOnFavorChanged = true)
		{
			if (!ModLister.CheckRoyalty("Honor"))
			{
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

		// Token: 0x0600566B RID: 22123 RVA: 0x001D4674 File Offset: 0x001D2874
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

		// Token: 0x0600566C RID: 22124 RVA: 0x001D4844 File Offset: 0x001D2A44
		public RoyalTitleDef GetCurrentTitle(Faction faction)
		{
			RoyalTitle currentTitleInFaction = this.GetCurrentTitleInFaction(faction);
			if (currentTitleInFaction == null)
			{
				return null;
			}
			return currentTitleInFaction.def;
		}

		// Token: 0x0600566D RID: 22125 RVA: 0x001D4858 File Offset: 0x001D2A58
		public RoyalTitle GetCurrentTitleInFaction(Faction faction)
		{
			if (faction == null)
			{
				Log.Error("Cannot get current title for null faction.");
			}
			int num = this.FindFactionTitleIndex(faction, false);
			if (num == -1)
			{
				return null;
			}
			return this.titles[num];
		}

		// Token: 0x0600566E RID: 22126 RVA: 0x001D4890 File Offset: 0x001D2A90
		public void SetTitle(Faction faction, RoyalTitleDef title, bool grantRewards, bool rewardsOnlyForNewestTitle = false, bool sendLetter = true)
		{
			if (!ModLister.CheckRoyalty("Honor"))
			{
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
			this.SetFavor(faction, 0, true);
			this.OnPostTitleChanged(faction, currentTitle, title);
		}

		// Token: 0x0600566F RID: 22127 RVA: 0x001D4920 File Offset: 0x001D2B20
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
			this.SetFavor(faction, 0, true);
			this.OnPostTitleChanged(faction, currentTitle, previousTitle);
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x001D4A2D File Offset: 0x001D2C2D
		public Pawn GetHeir(Faction faction)
		{
			if (this.heirs != null && this.heirs.ContainsKey(faction))
			{
				return this.heirs[faction];
			}
			return null;
		}

		// Token: 0x06005671 RID: 22129 RVA: 0x001D4A53 File Offset: 0x001D2C53
		public void SetHeir(Pawn heir, Faction faction)
		{
			if (this.heirs != null)
			{
				this.heirs[faction] = heir;
			}
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x001D4A6C File Offset: 0x001D2C6C
		public void AssignHeirIfNone(RoyalTitleDef t, Faction faction)
		{
			if (!this.heirs.ContainsKey(faction) && t.Awardable && (Faction.OfEmpire == null || this.pawn.HomeFaction != Faction.OfEmpire))
			{
				this.SetHeir(t.GetInheritanceWorker(faction).FindHeir(faction, this.pawn, t), faction);
			}
		}

		// Token: 0x06005673 RID: 22131 RVA: 0x001D4AC4 File Offset: 0x001D2CC4
		public void RoyaltyTrackerTick()
		{
			Faction faction;
			if (this.pawn.IsHashIntervalTick(37500) && RoyalTitleUtility.ShouldGetBestowingCeremonyQuest(this.pawn, out faction))
			{
				RoyalTitleUtility.GenerateBestowingCeremonyQuest(this.pawn, faction);
			}
			List<RoyalTitle> allTitlesInEffectForReading = this.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				allTitlesInEffectForReading[i].RoyalTitleTick();
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

		// Token: 0x06005674 RID: 22132 RVA: 0x001D4D30 File Offset: 0x001D2F30
		public void IssueDecree(bool causedByMentalBreak, string mentalBreakReason = null)
		{
			Pawn_RoyaltyTracker.<>c__DisplayClass59_0 CS$<>8__locals1 = new Pawn_RoyaltyTracker.<>c__DisplayClass59_0();
			if (!ModLister.CheckRoyalty("Decree"))
			{
				return;
			}
			Pawn_RoyaltyTracker.<>c__DisplayClass59_0 CS$<>8__locals2 = CS$<>8__locals1;
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

		// Token: 0x06005675 RID: 22133 RVA: 0x001D4EE4 File Offset: 0x001D30E4
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

		// Token: 0x06005676 RID: 22134 RVA: 0x001D4F90 File Offset: 0x001D3190
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

		// Token: 0x06005677 RID: 22135 RVA: 0x001D5064 File Offset: 0x001D3264
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

		// Token: 0x06005678 RID: 22136 RVA: 0x001D509C File Offset: 0x001D329C
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
			}
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x001D511C File Offset: 0x001D331C
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

		// Token: 0x0600567A RID: 22138 RVA: 0x001D524C File Offset: 0x001D344C
		private void OnPostTitleChanged(Faction faction, RoyalTitleDef prevTitle, RoyalTitleDef newTitle)
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
				this.UpdateAvailableAbilities();
				this.pawn.abilities.Notify_TemporaryAbilitiesChanged();
				this.UpdateHighestTitleAchieved(faction, newTitle);
			}
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "TitleChanged", this.pawn.Named("SUBJECT"));
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
			Pawn_ApparelTracker apparel = this.pawn.apparel;
			if (apparel == null)
			{
				return;
			}
			apparel.Notify_TitleChanged();
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x001D5358 File Offset: 0x001D3558
		private void UpdateRoyalTitle(Faction faction, bool sendLetter = true, RoyalTitleDef updateTo = null)
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
				this.SetFavor(faction, num - nextTitle.favorCost, false);
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
				this.UpdateRoyalTitle(faction, sendLetter, null);
			}
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x001D5450 File Offset: 0x001D3650
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
						dropCenter = DropCellFinder.RandomDropSpot(mapHeld, true);
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

		// Token: 0x0600567D RID: 22141 RVA: 0x001D5738 File Offset: 0x001D3938
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

		// Token: 0x0600567E RID: 22142 RVA: 0x001D5770 File Offset: 0x001D3970
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

		// Token: 0x0600567F RID: 22143 RVA: 0x001D57D1 File Offset: 0x001D39D1
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.PermitPointsAvailable && this.pawn.Faction == Faction.OfPlayer)
			{
				Faction faction = null;
				foreach (Faction faction2 in Find.FactionManager.AllFactionsVisibleInViewOrder)
				{
					if (this.GetPermitPoints(faction2) > 0)
					{
						faction = faction2;
						break;
					}
				}
				if (faction == null)
				{
					yield break;
				}
				yield return new Command_Action
				{
					defaultLabel = "ChooseRoyalPermit".Translate(),
					defaultDesc = "ChooseRoyalPermit_Desc".Translate(),
					icon = faction.def.FactionIcon,
					defaultIconColor = faction.Color,
					action = delegate()
					{
						this.OpenPermitWindow();
					},
					order = -100f
				};
			}
			yield break;
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x001D57E4 File Offset: 0x001D39E4
		public void OpenPermitWindow()
		{
			Dialog_InfoCard dialog_InfoCard = new Dialog_InfoCard(this.pawn, null);
			dialog_InfoCard.SetTab(Dialog_InfoCard.InfoCardTab.Permits);
			Find.WindowStack.Add(dialog_InfoCard);
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x001D5810 File Offset: 0x001D3A10
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
											Log.ErrorOnce("Iterations limit exceeded while getting favor for inheritance.", 91727191);
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

		// Token: 0x06005682 RID: 22146 RVA: 0x001D5C5C File Offset: 0x001D3E5C
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

		// Token: 0x06005683 RID: 22147 RVA: 0x001D5D00 File Offset: 0x001D3F00
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
			if (this.pawn.IsSlave)
			{
				command_Action.Disable("CommandCallRoyalAidSlave".Translate());
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

		// Token: 0x06005684 RID: 22148 RVA: 0x001D5E0E File Offset: 0x001D400E
		public bool CanRequireThroneroom()
		{
			return this.pawn.IsFreeColonist && this.allowRoomRequirements && !this.pawn.IsQuestLodger();
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x001D5E38 File Offset: 0x001D4038
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

		// Token: 0x06005686 RID: 22150 RVA: 0x001D5EAB File Offset: 0x001D40AB
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
			Room throneRoom = this.pawn.ownership.AssignedThrone.GetRoom(RegionType.Set_All);
			if (throneRoom == null)
			{
				yield break;
			}
			bool roomValid = RoomRoleWorker_ThroneRoom.Validate(throneRoom) == null;
			RoyalTitleDef prevTitle = highestTitle.def.GetPreviousTitle(highestTitle.faction);
			foreach (RoomRequirement roomRequirement in highestTitle.def.throneRoomRequirements)
			{
				if (!roomValid || !roomRequirement.MetOrDisabled(throneRoom, this.pawn))
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

		// Token: 0x06005687 RID: 22151 RVA: 0x001D5EC9 File Offset: 0x001D40C9
		public bool CanRequireBedroom()
		{
			return this.allowRoomRequirements && !this.pawn.IsPrisoner;
		}

		// Token: 0x06005688 RID: 22152 RVA: 0x001D5EE4 File Offset: 0x001D40E4
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

		// Token: 0x06005689 RID: 22153 RVA: 0x001D5F60 File Offset: 0x001D4160
		public void UpdateAvailableAbilities()
		{
			this.abilities.RemoveAll(delegate(Ability a)
			{
				for (int k = 0; k < this.AllTitlesInEffectForReading.Count; k++)
				{
					if (this.AllTitlesInEffectForReading[k].def.grantedAbilities.Contains(a.def))
					{
						return false;
					}
				}
				return true;
			});
			for (int i = 0; i < this.AllTitlesInEffectForReading.Count; i++)
			{
				RoyalTitle royalTitle = this.AllTitlesInEffectForReading[i];
				for (int j = 0; j < royalTitle.def.grantedAbilities.Count; j++)
				{
					AbilityDef def = royalTitle.def.grantedAbilities[j];
					if (!this.abilities.Any((Ability a) => a.def == def))
					{
						this.abilities.Add(AbilityUtility.MakeAbility(royalTitle.def.grantedAbilities[j], this.pawn));
					}
				}
			}
			if (ModsConfig.RoyaltyActive)
			{
				if (!this.abilities.Any((Ability a) => a.def == AbilityDefOf.AnimaTreeLinking))
				{
					this.abilities.Add(AbilityUtility.MakeAbility(AbilityDefOf.AnimaTreeLinking, this.pawn));
				}
			}
		}

		// Token: 0x0600568A RID: 22154 RVA: 0x001D6074 File Offset: 0x001D4274
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
				if (!roomRequirement.MetOrDisabled(bedroom, this.pawn))
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

		// Token: 0x0600568B RID: 22155 RVA: 0x001D6094 File Offset: 0x001D4294
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

		// Token: 0x0600568C RID: 22156 RVA: 0x001D612C File Offset: 0x001D432C
		public void ExposeData()
		{
			Scribe_Collections.Look<RoyalTitle>(ref this.titles, "titles", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Faction, int>(ref this.favor, "favor", LookMode.Reference, LookMode.Value, ref this.tmpFavorFactions, ref this.tmpAmounts);
			Scribe_Values.Look<int>(ref this.lastDecreeTicks, "lastDecreeTicks", -999999, false);
			Scribe_Collections.Look<Faction, RoyalTitleDef>(ref this.highestTitles, "highestTitles", LookMode.Reference, LookMode.Def, ref this.tmpHighestTitleFactions, ref this.tmpTitleDefs);
			Scribe_Collections.Look<Faction, Pawn>(ref this.heirs, "heirs", LookMode.Reference, LookMode.Reference, ref this.tmpHeirFactions, ref this.tmpPawns);
			Scribe_Values.Look<bool>(ref this.allowRoomRequirements, "allowRoomRequirements", true, false);
			Scribe_Values.Look<bool>(ref this.allowApparelRequirements, "allowApparelRequirements", true, false);
			Scribe_Collections.Look<FactionPermit>(ref this.factionPermits, "permits", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.titles.RemoveAll((RoyalTitle x) => x.def == null) != 0)
				{
					Log.Error("Some RoyalTitles had null defs after loading.");
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
				if (this.factionPermits.RemoveAll((FactionPermit x) => DefDatabase<RoyalTitlePermitDef>.AllDefs.Any((RoyalTitlePermitDef y) => y.prerequisite == x.Permit && this.HasPermit(y, x.Faction) && this.HasPermit(y.prerequisite, x.Faction))) != 0)
				{
					Log.Error("Removed some null permits.");
				}
				foreach (RoyalTitle royalTitle2 in this.titles)
				{
					this.AssignHeirIfNone(royalTitle2.def, royalTitle2.faction);
				}
				if (this.abilities == null)
				{
					this.abilities = new List<Ability>();
				}
				this.UpdateAvailableAbilities();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04003307 RID: 13063
		public Pawn pawn;

		// Token: 0x04003308 RID: 13064
		private List<RoyalTitle> titles = new List<RoyalTitle>();

		// Token: 0x04003309 RID: 13065
		private Dictionary<Faction, int> favor = new Dictionary<Faction, int>();

		// Token: 0x0400330A RID: 13066
		private Dictionary<Faction, RoyalTitleDef> highestTitles = new Dictionary<Faction, RoyalTitleDef>();

		// Token: 0x0400330B RID: 13067
		private Dictionary<Faction, Pawn> heirs = new Dictionary<Faction, Pawn>();

		// Token: 0x0400330C RID: 13068
		private List<FactionPermit> factionPermits = new List<FactionPermit>();

		// Token: 0x0400330D RID: 13069
		private List<Ability> abilities = new List<Ability>();

		// Token: 0x0400330E RID: 13070
		public int lastDecreeTicks = -999999;

		// Token: 0x0400330F RID: 13071
		public bool allowRoomRequirements = true;

		// Token: 0x04003310 RID: 13072
		public bool allowApparelRequirements = true;

		// Token: 0x04003311 RID: 13073
		private static List<RoyalTitle> EmptyTitles = new List<RoyalTitle>();

		// Token: 0x04003312 RID: 13074
		private const int BestowingCeremonyCheckInterval = 37500;

		// Token: 0x04003313 RID: 13075
		private List<string> tmpDecreeTags = new List<string>();

		// Token: 0x04003314 RID: 13076
		private static List<FactionPermit> tmpPermits = new List<FactionPermit>();

		// Token: 0x04003315 RID: 13077
		private List<Faction> factionHeirsToClearTmp = new List<Faction>();

		// Token: 0x04003316 RID: 13078
		private static List<Action> tmpInheritedTitles = new List<Action>();

		// Token: 0x04003317 RID: 13079
		public static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallAid", true);

		// Token: 0x04003318 RID: 13080
		private List<Faction> tmpFavorFactions;

		// Token: 0x04003319 RID: 13081
		private List<Faction> tmpHighestTitleFactions;

		// Token: 0x0400331A RID: 13082
		private List<Faction> tmpHeirFactions;

		// Token: 0x0400331B RID: 13083
		private List<int> tmpAmounts;

		// Token: 0x0400331C RID: 13084
		private List<Pawn> tmpPawns;

		// Token: 0x0400331D RID: 13085
		private List<RoyalTitleDef> tmpTitleDefs;
	}
}
