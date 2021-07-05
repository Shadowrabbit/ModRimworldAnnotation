using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000EB7 RID: 3767
	public class Faction : IExposable, ILoadReferenceable, ICommunicable
	{
		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x0600586D RID: 22637 RVA: 0x001E09CB File Offset: 0x001DEBCB
		// (set) Token: 0x0600586E RID: 22638 RVA: 0x001E09EC File Offset: 0x001DEBEC
		public string Name
		{
			get
			{
				if (this.HasName)
				{
					return this.name;
				}
				return this.def.LabelCap;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x0600586F RID: 22639 RVA: 0x001E09F5 File Offset: 0x001DEBF5
		public bool HasName
		{
			get
			{
				return this.name != null;
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06005870 RID: 22640 RVA: 0x001E0A00 File Offset: 0x001DEC00
		public bool IsPlayer
		{
			get
			{
				return this.def.isPlayer;
			}
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06005871 RID: 22641 RVA: 0x001E0A0D File Offset: 0x001DEC0D
		public int PlayerGoodwill
		{
			get
			{
				return this.GoodwillWith(Faction.OfPlayer);
			}
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06005872 RID: 22642 RVA: 0x001E0A1A File Offset: 0x001DEC1A
		public FactionRelationKind PlayerRelationKind
		{
			get
			{
				return this.RelationKindWith(Faction.OfPlayer);
			}
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06005873 RID: 22643 RVA: 0x001E0A27 File Offset: 0x001DEC27
		public int NaturalGoodwill
		{
			get
			{
				return Find.GoodwillSituationManager.GetNaturalGoodwill(this);
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x06005874 RID: 22644 RVA: 0x001E0A34 File Offset: 0x001DEC34
		public Color Color
		{
			get
			{
				if (this.color != null)
				{
					return this.color.Value;
				}
				if (this.def.colorSpectrum.NullOrEmpty<Color>())
				{
					return Color.white;
				}
				return ColorsFromSpectrum.Get(this.def.colorSpectrum, this.colorFromSpectrum);
			}
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x06005875 RID: 22645 RVA: 0x001E0A88 File Offset: 0x001DEC88
		public string LeaderTitle
		{
			get
			{
				if (this.ideos == null || this.ideos.PrimaryIdeo == null || this.ideos.PrimaryIdeo.leaderTitleMale.NullOrEmpty())
				{
					if (this.leader != null && this.leader.gender == Gender.Female && !this.def.leaderTitleFemale.NullOrEmpty())
					{
						return this.def.leaderTitleFemale;
					}
					return this.def.leaderTitle;
				}
				else
				{
					if (this.leader != null && this.leader.gender == Gender.Female)
					{
						return this.ideos.PrimaryIdeo.leaderTitleFemale;
					}
					return this.ideos.PrimaryIdeo.leaderTitleMale;
				}
			}
		}

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06005876 RID: 22646 RVA: 0x001E0B38 File Offset: 0x001DED38
		private bool ShouldHaveLeader
		{
			get
			{
				return !this.IsPlayer && !this.Hidden && !this.temporary && this.def.humanlikeFaction;
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06005877 RID: 22647 RVA: 0x001E0B5F File Offset: 0x001DED5F
		public TaggedString NameColored
		{
			get
			{
				if (this.HasName)
				{
					return this.name.ApplyTag(this);
				}
				return this.def.LabelCap;
			}
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06005878 RID: 22648 RVA: 0x001E0B81 File Offset: 0x001DED81
		public bool CanEverGiveGoodwillRewards
		{
			get
			{
				return !this.def.permanentEnemy && this.HasGoodwill;
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06005879 RID: 22649 RVA: 0x001E0B98 File Offset: 0x001DED98
		public string GetReportText
		{
			get
			{
				return this.def.description + (this.def.HasRoyalTitles ? ("\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(this, null)) : "");
			}
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x0600587A RID: 22650 RVA: 0x001E0BD0 File Offset: 0x001DEDD0
		public bool Hidden
		{
			get
			{
				bool? flag = this.hidden;
				if (flag == null)
				{
					return this.def.hidden;
				}
				return flag.GetValueOrDefault();
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x0600587B RID: 22651 RVA: 0x001E0C00 File Offset: 0x001DEE00
		public bool HasGoodwill
		{
			get
			{
				return !this.Hidden && !this.temporary;
			}
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x001E0C18 File Offset: 0x001DEE18
		public Faction()
		{
			this.randomKey = Rand.Range(0, int.MaxValue);
			this.kidnapped = new KidnappedPawnsTracker(this);
		}

		// Token: 0x0600587D RID: 22653 RVA: 0x001E0CAC File Offset: 0x001DEEAC
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.leader, "leader", false);
			Scribe_Defs.Look<FactionDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.randomKey, "randomKey", 0, false);
			Scribe_Values.Look<float>(ref this.colorFromSpectrum, "colorFromSpectrum", 0f, false);
			Scribe_Values.Look<float>(ref this.centralMelanin, "centralMelanin", 0f, false);
			Scribe_Collections.Look<FactionRelation>(ref this.relations, "relations", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<KidnappedPawnsTracker>(ref this.kidnapped, "kidnapped", new object[]
			{
				this
			});
			Scribe_Deep.Look<FactionIdeosTracker>(ref this.ideos, "ideos", new object[]
			{
				this
			});
			Scribe_Collections.Look<PredatorThreat>(ref this.predatorThreats, "predatorThreats", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.defeated, "defeated", false, false);
			Scribe_Values.Look<int>(ref this.lastTraderRequestTick, "lastTraderRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastMilitaryAidRequestTick, "lastMilitaryAidRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastExecutionTick, "lastExecutionTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.naturalGoodwillTimer, "naturalGoodwillTimer", 0, false);
			Scribe_Values.Look<bool>(ref this.allowRoyalFavorRewards, "allowRoyalFavorRewards", true, false);
			Scribe_Values.Look<bool>(ref this.allowGoodwillRewards, "allowGoodwillRewards", true, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool?>(ref this.hidden, "hidden", null, false);
			Scribe_Values.Look<bool>(ref this.temporary, "temporary", false, false);
			Scribe_Values.Look<bool>(ref this.factionHostileOnHarmByPlayer, "factionHostileOnHarmByPlayer", false, false);
			Scribe_Values.Look<Color?>(ref this.color, "color", null, false);
			Scribe_Values.Look<bool>(ref this.neverFlee, "neverFlee", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.predatorThreats.RemoveAll((PredatorThreat x) => x.predator == null);
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600587E RID: 22654 RVA: 0x001E0EE0 File Offset: 0x001DF0E0
		public void FactionTick()
		{
			this.CheckReachNaturalGoodwill();
			this.kidnapped.KidnappedPawnsTrackerTick();
			for (int i = this.predatorThreats.Count - 1; i >= 0; i--)
			{
				PredatorThreat predatorThreat = this.predatorThreats[i];
				if (predatorThreat.Expired)
				{
					this.predatorThreats.RemoveAt(i);
					if (predatorThreat.predator.Spawned)
					{
						predatorThreat.predator.Map.attackTargetsCache.UpdateTarget(predatorThreat.predator);
					}
				}
			}
			if (Find.TickManager.TicksGame % 1000 == 200 && this.IsPlayer)
			{
				if (NamePlayerFactionAndSettlementUtility.CanNameFactionNow())
				{
					Settlement settlement = Find.WorldObjects.Settlements.Find((Settlement x) => NamePlayerFactionAndSettlementUtility.CanNameSettlementSoon(x));
					if (settlement != null)
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement));
					}
					else
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFaction());
					}
				}
				else
				{
					Settlement settlement2 = Find.WorldObjects.Settlements.Find((Settlement x) => NamePlayerFactionAndSettlementUtility.CanNameSettlementNow(x));
					if (settlement2 != null)
					{
						if (NamePlayerFactionAndSettlementUtility.CanNameFactionSoon())
						{
							Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement2));
						}
						else
						{
							Find.WindowStack.Add(new Dialog_NamePlayerSettlement(settlement2));
						}
					}
				}
			}
			if (this.ShouldHaveLeader && this.leader == null)
			{
				Log.ErrorOnce("Faction leader for " + this.Name + " is null.", this.loadID ^ 441821);
			}
		}

		// Token: 0x0600587F RID: 22655 RVA: 0x001E1074 File Offset: 0x001DF274
		private void CheckReachNaturalGoodwill()
		{
			if (this.IsPlayer || !this.HasGoodwill || this.def.permanentEnemy)
			{
				return;
			}
			int num = this.BaseGoodwillWith(Faction.OfPlayer);
			IntRange intRange = new IntRange(this.NaturalGoodwill - 50, this.NaturalGoodwill + 50);
			if (intRange.Includes(num))
			{
				this.naturalGoodwillTimer = 0;
				return;
			}
			this.naturalGoodwillTimer++;
			if (num < intRange.min)
			{
				int num2 = 3000000;
				if (this.naturalGoodwillTimer >= num2)
				{
					Faction ofPlayer = Faction.OfPlayer;
					int goodwillChange = Mathf.Min(10, intRange.min - num);
					bool canSendMessage = true;
					HistoryEventDef reachNaturalGoodwill = HistoryEventDefOf.ReachNaturalGoodwill;
					this.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, !this.temporary, reachNaturalGoodwill, null);
					this.naturalGoodwillTimer = 0;
					return;
				}
			}
			else if (num > intRange.max)
			{
				int num3 = 3000000;
				if (this.naturalGoodwillTimer >= num3)
				{
					Faction ofPlayer2 = Faction.OfPlayer;
					int goodwillChange2 = -Mathf.Min(10, num - intRange.max);
					bool canSendMessage2 = true;
					HistoryEventDef reachNaturalGoodwill = HistoryEventDefOf.ReachNaturalGoodwill;
					this.TryAffectGoodwillWith(ofPlayer2, goodwillChange2, canSendMessage2, !this.temporary, reachNaturalGoodwill, null);
					this.naturalGoodwillTimer = 0;
				}
			}
		}

		// Token: 0x06005880 RID: 22656 RVA: 0x001E1198 File Offset: 0x001DF398
		public void TryMakeInitialRelationsWith(Faction other)
		{
			if (this.RelationWith(other, true) != null)
			{
				return;
			}
			int a = Faction.<TryMakeInitialRelationsWith>g__GetInitialGoodwill|61_0(this, other);
			int b = Faction.<TryMakeInitialRelationsWith>g__GetInitialGoodwill|61_0(other, this);
			int num = Mathf.Min(a, b);
			FactionRelationKind kind;
			if (num <= -10)
			{
				kind = FactionRelationKind.Hostile;
			}
			else if (num >= 75)
			{
				kind = FactionRelationKind.Ally;
			}
			else
			{
				kind = FactionRelationKind.Neutral;
			}
			FactionRelation factionRelation = new FactionRelation();
			factionRelation.other = other;
			factionRelation.baseGoodwill = num;
			factionRelation.kind = kind;
			this.relations.Add(factionRelation);
			FactionRelation factionRelation2 = new FactionRelation();
			factionRelation2.other = this;
			factionRelation2.baseGoodwill = num;
			factionRelation2.kind = kind;
			other.relations.Add(factionRelation2);
		}

		// Token: 0x06005881 RID: 22657 RVA: 0x001E1230 File Offset: 0x001DF430
		public void SetRelation(FactionRelation relation)
		{
			if (relation.other == this)
			{
				Log.Error("Tried to set relation between faction " + this + " and itself.");
				return;
			}
			if (relation.other == null)
			{
				Log.Error("Relation is missing faction.");
				return;
			}
			this.relations.RemoveAll((FactionRelation r) => r.other == relation.other);
			relation.other.relations.RemoveAll((FactionRelation r) => r.other == this);
			this.relations.Add(relation);
			FactionRelation factionRelation = new FactionRelation();
			factionRelation.other = this;
			factionRelation.kind = relation.kind;
			relation.other.relations.Add(factionRelation);
		}

		// Token: 0x06005882 RID: 22658 RVA: 0x001E130C File Offset: 0x001DF50C
		public PawnKindDef RandomPawnKind()
		{
			Faction.allPawnKinds.Clear();
			if (this.def.pawnGroupMakers != null)
			{
				for (int i = 0; i < this.def.pawnGroupMakers.Count; i++)
				{
					List<PawnGenOption> options = this.def.pawnGroupMakers[i].options;
					for (int j = 0; j < options.Count; j++)
					{
						Faction.allPawnKinds.Add(options[j].kind);
					}
				}
			}
			if (!Faction.allPawnKinds.Any<PawnKindDef>())
			{
				return this.def.basicMemberKind;
			}
			PawnKindDef result = Faction.allPawnKinds.RandomElement<PawnKindDef>();
			Faction.allPawnKinds.Clear();
			return result;
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x001E13B8 File Offset: 0x001DF5B8
		public FactionRelation RelationWith(Faction other, bool allowNull = false)
		{
			if (other == this)
			{
				Log.Error("Tried to get relation between faction " + this + " and itself.");
				return new FactionRelation();
			}
			for (int i = 0; i < this.relations.Count; i++)
			{
				if (this.relations[i].other == other)
				{
					return this.relations[i];
				}
			}
			if (!allowNull)
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					this.name,
					" has null relation with ",
					other,
					". Returning dummy relation."
				}));
				return new FactionRelation();
			}
			return null;
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x001E145A File Offset: 0x001DF65A
		public int BaseGoodwillWith(Faction other)
		{
			return this.RelationWith(other, false).baseGoodwill;
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x001E146C File Offset: 0x001DF66C
		public int GoodwillWith(Faction other)
		{
			int num = this.BaseGoodwillWith(other);
			if (this.IsPlayer)
			{
				num = Mathf.Min(num, Find.GoodwillSituationManager.GetMaxGoodwill(other));
			}
			else if (other.IsPlayer)
			{
				num = Mathf.Min(num, Find.GoodwillSituationManager.GetMaxGoodwill(this));
			}
			return num;
		}

		// Token: 0x06005886 RID: 22662 RVA: 0x001E14B8 File Offset: 0x001DF6B8
		public FactionRelationKind RelationKindWith(Faction other)
		{
			return this.RelationWith(other, false).kind;
		}

		// Token: 0x06005887 RID: 22663 RVA: 0x001E14C8 File Offset: 0x001DF6C8
		public bool CanChangeGoodwillFor(Faction other, int goodwillChange)
		{
			return this.HasGoodwill && other.HasGoodwill && !this.def.permanentEnemy && !other.def.permanentEnemy && !this.defeated && !other.defeated && other != this && (!this.def.permanentEnemyToEveryoneExceptPlayer || other.IsPlayer) && (!other.def.permanentEnemyToEveryoneExceptPlayer || this.IsPlayer) && (this.def.permanentEnemyToEveryoneExcept == null || this.def.permanentEnemyToEveryoneExcept.Contains(other.def)) && (other.def.permanentEnemyToEveryoneExcept == null || other.def.permanentEnemyToEveryoneExcept.Contains(this.def)) && (goodwillChange <= 0 || ((!this.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(other)) && (!other.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(this)))) && !QuestUtility.IsGoodwillLockedByQuest(this, other);
		}

		// Token: 0x06005888 RID: 22664 RVA: 0x001E15CA File Offset: 0x001DF7CA
		public int GoodwillToMakeHostile(Faction other)
		{
			if (this.HostileTo(other))
			{
				return 0;
			}
			return -75 - this.GoodwillWith(other);
		}

		// Token: 0x06005889 RID: 22665 RVA: 0x001E15E4 File Offset: 0x001DF7E4
		public int CalculateAdjustedGoodwillChange(Faction other, int goodwillChange)
		{
			int num = this.GoodwillWith(other);
			if (this.IsPlayer || other.IsPlayer)
			{
				int num2 = this.IsPlayer ? other.NaturalGoodwill : this.NaturalGoodwill;
				if ((num2 < num && goodwillChange < 0) || (num2 > num && goodwillChange > 0))
				{
					int num3 = Mathf.Min(Mathf.Abs(num - num2), Mathf.Abs(goodwillChange));
					int num4 = Mathf.RoundToInt(0.25f * (float)num3);
					if (goodwillChange < 0)
					{
						num4 = -num4;
					}
					goodwillChange += num4;
				}
			}
			return goodwillChange;
		}

		// Token: 0x0600588A RID: 22666 RVA: 0x001E1660 File Offset: 0x001DF860
		public bool TryAffectGoodwillWith(Faction other, int goodwillChange, bool canSendMessage = true, bool canSendHostilityLetter = true, HistoryEventDef reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (!this.CanChangeGoodwillFor(other, goodwillChange))
			{
				return false;
			}
			if (goodwillChange == 0)
			{
				return true;
			}
			int num = this.GoodwillWith(other);
			goodwillChange = this.CalculateAdjustedGoodwillChange(other, goodwillChange);
			int num2 = this.BaseGoodwillWith(other);
			int num3 = Mathf.Clamp(num2 + goodwillChange, -100, 100);
			if (num2 == num3)
			{
				return true;
			}
			if (reason != null && (this.IsPlayer || other.IsPlayer))
			{
				Faction arg = this.IsPlayer ? other : this;
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(reason, arg.Named(HistoryEventArgsNames.AffectedFaction), goodwillChange.Named(HistoryEventArgsNames.CustomGoodwill)), true);
			}
			FactionRelation factionRelation = this.RelationWith(other, false);
			factionRelation.baseGoodwill = num3;
			bool flag;
			factionRelation.CheckKindThresholds(this, canSendHostilityLetter, (reason != null) ? reason.LabelCap : null, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			FactionRelationKind kind = factionRelation2.kind;
			factionRelation2.baseGoodwill = factionRelation.baseGoodwill;
			factionRelation2.kind = factionRelation.kind;
			bool flag2;
			if (kind != factionRelation2.kind)
			{
				other.Notify_RelationKindChanged(this, kind, canSendHostilityLetter, (reason != null) ? reason.LabelCap : null, lookTarget ?? GlobalTargetInfo.Invalid, out flag2);
			}
			else
			{
				flag2 = false;
			}
			int num4 = this.GoodwillWith(other);
			if (canSendMessage && num != num4 && !flag && !flag2 && Current.ProgramState == ProgramState.Playing && (this.IsPlayer || other.IsPlayer))
			{
				Faction faction = this.IsPlayer ? other : this;
				string text;
				if (reason != null)
				{
					text = "MessageGoodwillChangedWithReason".Translate(faction.name, num.ToString("F0"), num4.ToString("F0"), reason.LabelCap);
				}
				else
				{
					text = "MessageGoodwillChanged".Translate(faction.name, num.ToString("F0"), num4.ToString("F0"));
				}
				Messages.Message(text, lookTarget ?? GlobalTargetInfo.Invalid, ((float)goodwillChange > 0f) ? MessageTypeDefOf.PositiveEvent : MessageTypeDefOf.NegativeEvent, true);
			}
			return true;
		}

		// Token: 0x0600588B RID: 22667 RVA: 0x001E18E8 File Offset: 0x001DFAE8
		public void Notify_GoodwillSituationsChanged(Faction other, bool canSendHostilityLetter, string reason, GlobalTargetInfo? lookTarget)
		{
			FactionRelation factionRelation = this.RelationWith(other, false);
			bool flag;
			factionRelation.CheckKindThresholds(this, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			FactionRelationKind kind = factionRelation2.kind;
			factionRelation2.kind = factionRelation.kind;
			if (kind != factionRelation2.kind)
			{
				other.Notify_RelationKindChanged(this, kind, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			}
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x001E1970 File Offset: 0x001DFB70
		public void SetRelationDirect(Faction other, FactionRelationKind kind, bool canSendHostilityLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (this.HasGoodwill && other.HasGoodwill)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to use SetRelationDirect for factions which use goodwill. The relation would be overriden by goodwill anyway. faction=",
					this,
					", other=",
					other
				}));
				return;
			}
			FactionRelation factionRelation = this.RelationWith(other, false);
			if (factionRelation.kind != kind)
			{
				FactionRelationKind kind2 = factionRelation.kind;
				factionRelation.kind = kind;
				bool flag;
				this.Notify_RelationKindChanged(other, kind2, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
				other.RelationWith(this, false).kind = kind;
				other.Notify_RelationKindChanged(this, kind2, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			}
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x001E1A34 File Offset: 0x001DFC34
		public void RemoveAllRelations()
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (faction != this)
				{
					faction.relations.RemoveAll((FactionRelation x) => x.other == this);
				}
			}
			this.relations.Clear();
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x001E1AAC File Offset: 0x001DFCAC
		public void TryAppendRelationKindChangedInfo(StringBuilder text, FactionRelationKind previousKind, FactionRelationKind newKind, string reason = null)
		{
			TaggedString taggedString = null;
			this.TryAppendRelationKindChangedInfo(ref taggedString, previousKind, newKind, reason);
			if (!taggedString.NullOrEmpty())
			{
				text.AppendLine();
				text.AppendLine();
				text.AppendTagged(taggedString);
			}
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x001E1AEC File Offset: 0x001DFCEC
		public void TryAppendRelationKindChangedInfo(ref TaggedString text, FactionRelationKind previousKind, FactionRelationKind newKind, string reason = null)
		{
			if (previousKind == newKind)
			{
				return;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			if (newKind == FactionRelationKind.Hostile)
			{
				text += "LetterRelationsChange_Hostile".Translate(this.NameColored);
				if (this.HasGoodwill)
				{
					text += "\n\n" + "LetterRelationsChange_HostileGoodwillDescription".Translate(this.PlayerGoodwill.ToStringWithSign(), -75.ToStringWithSign(), 0.ToStringWithSign());
				}
				if (!reason.NullOrEmpty())
				{
					text += "\n\n" + "FinalStraw".Translate(reason.CapitalizeFirst());
					return;
				}
			}
			else if (newKind == FactionRelationKind.Ally)
			{
				text += "LetterRelationsChange_Ally".Translate(this.NameColored);
				if (this.HasGoodwill)
				{
					text += "\n\n" + "LetterRelationsChange_AllyGoodwillDescription".Translate(this.PlayerGoodwill.ToStringWithSign(), 75.ToStringWithSign(), 0.ToStringWithSign());
				}
				if (!reason.NullOrEmpty())
				{
					text += "\n\n" + "LastFactionRelationsEvent".Translate() + ": " + reason.CapitalizeFirst();
					return;
				}
			}
			else if (newKind == FactionRelationKind.Neutral)
			{
				if (previousKind == FactionRelationKind.Hostile)
				{
					text += "LetterRelationsChange_NeutralFromHostile".Translate(this.NameColored);
					if (this.HasGoodwill)
					{
						text += "\n\n" + "LetterRelationsChange_NeutralFromHostileGoodwillDescription".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), 0.ToStringWithSign(), -75.ToStringWithSign(), 75.ToStringWithSign());
					}
					if (!reason.NullOrEmpty())
					{
						text += "\n\n" + "LastFactionRelationsEvent".Translate() + ": " + reason.CapitalizeFirst();
						return;
					}
				}
				else
				{
					text += "LetterRelationsChange_NeutralFromAlly".Translate(this.NameColored);
					if (this.HasGoodwill)
					{
						text += "\n\n" + "LetterRelationsChange_NeutralFromAllyGoodwillDescription".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), 0.ToStringWithSign(), -75.ToStringWithSign(), 75.ToStringWithSign());
					}
					if (!reason.NullOrEmpty())
					{
						text += "\n\n" + "Reason".Translate() + ": " + reason.CapitalizeFirst();
					}
				}
			}
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x001E1E60 File Offset: 0x001E0060
		public void Notify_MemberTookDamage(Pawn member, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null)
			{
				return;
			}
			if (this.IsPlayer)
			{
				return;
			}
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn != null && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.PredatorHunt)
			{
				this.TookDamageFromPredator(pawn);
			}
			if (dinfo.Instigator.Faction == null || !dinfo.Def.ExternalViolenceFor(member) || this.HostileTo(dinfo.Instigator.Faction))
			{
				return;
			}
			if (this.factionHostileOnHarmByPlayer && dinfo.Instigator.Faction == Faction.OfPlayer)
			{
				this.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
			}
			if (member.InAggroMentalState)
			{
				return;
			}
			if (pawn != null && pawn.InAggroMentalState)
			{
				return;
			}
			if (member.InMentalState && member.MentalStateDef.IsExtreme && member.MentalStateDef.category == MentalStateCategory.Malicious && this.PlayerRelationKind == FactionRelationKind.Ally)
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && (PrisonBreakUtility.IsPrisonBreaking(member) || member.IsQuestHelper()))
			{
				return;
			}
			if (pawn != null && SlaveRebellionUtility.IsRebelling(pawn))
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo))
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int goodwillChange = (int)(-1.3f * num);
				Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, true, !this.temporary, HistoryEventDefOf.AttackedMember, new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x001E1FE8 File Offset: 0x001E01E8
		public void Notify_BuildingRemoved(Building building, Pawn deconstructor)
		{
			if (this.IsPlayer)
			{
				return;
			}
			if (this.factionHostileOnHarmByPlayer && deconstructor != null && deconstructor.Faction == Faction.OfPlayer)
			{
				this.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
			}
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x001E2030 File Offset: 0x001E0230
		public void Notify_BuildingTookDamage(Building building, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null || this.IsPlayer)
			{
				return;
			}
			if (dinfo.Instigator.Faction == null || !dinfo.Def.ExternalViolenceFor(building) || this.HostileTo(dinfo.Instigator.Faction))
			{
				return;
			}
			if (this.factionHostileOnHarmByPlayer && dinfo.Instigator.Faction == Faction.OfPlayer)
			{
				this.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo))
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int goodwillChange = (int)(-1f * num);
				Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, true, true, HistoryEventDefOf.AttackedBuilding, new GlobalTargetInfo?(building));
			}
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x001E210C File Offset: 0x001E030C
		public void Notify_MemberCaptured(Pawn member, Faction violator)
		{
			if (this.ideos != null)
			{
				this.ideos.Notify_MemberGainedOrLost();
			}
			if (violator == this)
			{
				return;
			}
			if (!this.temporary)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this, Faction.OfPlayer.GoodwillToMakeHostile(this), true, !this.temporary, HistoryEventDefOf.MemberCaptured, new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x06005894 RID: 22676 RVA: 0x001E216C File Offset: 0x001E036C
		public void Notify_MemberStripped(Pawn member, Faction violator)
		{
			if (violator == this || this.Hidden || member.Dead)
			{
				return;
			}
			if (violator == Faction.OfPlayer && this.RelationKindWith(violator) != FactionRelationKind.Hostile)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this, -40, true, !this.temporary, HistoryEventDefOf.MemberStripped, new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x06005895 RID: 22677 RVA: 0x001E21C8 File Offset: 0x001E03C8
		public void Notify_MemberDied(Pawn member, DamageInfo? dinfo, bool wasWorldPawn, bool wasGuilty, Map map)
		{
			if (this.ideos != null)
			{
				this.ideos.Notify_MemberGainedOrLost();
			}
			if (this.IsPlayer)
			{
				return;
			}
			if (!wasWorldPawn && !PawnGenerator.IsBeingGenerated(member) && Current.ProgramState == ProgramState.Playing && map != null && map.IsPlayerHome && !this.HostileTo(Faction.OfPlayer))
			{
				if (dinfo != null && dinfo.Value.Category == DamageInfo.SourceCategory.Collapse)
				{
					bool canSendMessage = MessagesRepeatAvoider.MessageShowAllowed("FactionRelationAdjustmentCrushed-" + this.Name, 5f);
					int goodwillChange = member.RaceProps.Humanlike ? -25 : -15;
					Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, canSendMessage, !this.temporary, HistoryEventDefOf.MemberCrushed, new GlobalTargetInfo?(new TargetInfo(member.Position, map, false)));
				}
				else if (dinfo != null && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == null))
				{
					Pawn pawn = dinfo.Value.Instigator as Pawn;
					if (pawn == null || !pawn.RaceProps.Animal || pawn.mindState.mentalStateHandler.CurStateDef != MentalStateDefOf.ManhunterPermanent)
					{
						int goodwillChange2 = member.RaceProps.Humanlike ? -5 : -3;
						Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange2, true, !this.temporary, HistoryEventDefOf.MemberNeutrallyDied, new GlobalTargetInfo?(member));
					}
				}
				else if ((member.kindDef.factionHostileOnDeath || (member.kindDef.factionHostileOnKill && dinfo != null && dinfo.Value.Instigator != null && dinfo.Value.Instigator.Faction == Faction.OfPlayer)) && !wasGuilty)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(this, Faction.OfPlayer.GoodwillToMakeHostile(this), true, !this.temporary, HistoryEventDefOf.MemberKilled, null);
				}
			}
			if (member == this.leader)
			{
				this.Notify_LeaderDied();
			}
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x001E23FC File Offset: 0x001E05FC
		public void Notify_PawnJoined(Pawn p)
		{
			if (this.ideos != null)
			{
				this.ideos.Notify_MemberGainedOrLost();
			}
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x001E2414 File Offset: 0x001E0614
		public void Notify_LeaderDied()
		{
			Pawn pawn = this.leader;
			string str = "LetterLeadersDeathLabel".Translate(this.name, this.LeaderTitle).Resolve().CapitalizeFirst();
			string text = "LetterLeadersDeath".Translate(this.NameColored, this.LeaderTitle, pawn.Named("OLDLEADER")).Resolve().CapitalizeFirst();
			if (this.TryGenerateNewLeader())
			{
				string str2 = "LetterNewLeader".Translate(this.LeaderTitle, this.leader.Named("NEWLEADER")).Resolve().CapitalizeFirst();
				if (!this.temporary)
				{
					Find.LetterStack.ReceiveLetter(str, text + "\n\n" + str2, LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null);
				}
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"), this.leader.Named("NEWFACTIONLEADER"));
				return;
			}
			if (!this.temporary)
			{
				Find.LetterStack.ReceiveLetter(str, text, LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null);
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"));
		}

		// Token: 0x06005898 RID: 22680 RVA: 0x001E2584 File Offset: 0x001E0784
		public void Notify_LeaderLost()
		{
			Pawn pawn = this.leader;
			if (this.TryGenerateNewLeader())
			{
				if (!this.temporary)
				{
					Find.LetterStack.ReceiveLetter("LetterLeaderChangedLabel".Translate(this.name, this.LeaderTitle).Resolve().CapitalizeFirst(), "LetterLeaderChanged".Translate(this.NameColored, this.LeaderTitle, pawn.Named("OLDLEADER")).Resolve().CapitalizeFirst() + "\n\n" + "LetterNewLeader".Translate(this.LeaderTitle, this.leader.Named("NEWLEADER")).Resolve().CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null);
				}
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"), this.leader.Named("NEWFACTIONLEADER"));
				return;
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"));
		}

		// Token: 0x06005899 RID: 22681 RVA: 0x001E26C0 File Offset: 0x001E08C0
		public void Notify_RelationKindChanged(Faction other, FactionRelationKind previousKind, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			if (Current.ProgramState != ProgramState.Playing || other != Faction.OfPlayer)
			{
				canSendLetter = false;
			}
			sentLetter = false;
			ColoredText.ClearCache();
			FactionRelationKind factionRelationKind = this.RelationKindWith(other);
			if (factionRelationKind == FactionRelationKind.Hostile)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive.ToList<Pawn>())
					{
						if ((pawn.Faction == this && pawn.HostFaction == other) || (pawn.Faction == other && pawn.HostFaction == this))
						{
							pawn.guest.SetGuestStatus(pawn.HostFaction, GuestStatus.Prisoner);
						}
					}
				}
				if (other == Faction.OfPlayer)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "BecameHostileToPlayer", this.Named("SUBJECT"));
				}
			}
			if (other == Faction.OfPlayer && !this.HostileTo(Faction.OfPlayer))
			{
				List<Site> list = new List<Site>();
				List<Site> sites = Find.WorldObjects.Sites;
				for (int i = 0; i < sites.Count; i++)
				{
					if (sites[i].factionMustRemainHostile && sites[i].Faction == this && !sites[i].HasMap)
					{
						list.Add(sites[i]);
					}
				}
				if (list.Any<Site>())
				{
					string str;
					string str2;
					if (list.Count == 1)
					{
						str = "LetterLabelSiteNoLongerHostile".Translate();
						str2 = "LetterSiteNoLongerHostile".Translate(this.NameColored, list[0].Label);
					}
					else
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int j = 0; j < list.Count; j++)
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.AppendLine();
							}
							stringBuilder.Append("  - " + list[j].LabelCap);
							ImportantPawnComp component = list[j].GetComponent<ImportantPawnComp>();
							if (component != null && component.pawn.Any)
							{
								stringBuilder.Append(" (" + component.pawn[0].LabelCap + ")");
							}
						}
						str = "LetterLabelSiteNoLongerHostileMulti".Translate();
						str2 = "LetterSiteNoLongerHostileMulti".Translate(this.NameColored) + ":\n\n" + stringBuilder;
					}
					Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.NeutralEvent, new LookTargets(from x in list
					select new GlobalTargetInfo(x.Tile)), null, null, null, null);
					for (int k = 0; k < list.Count; k++)
					{
						list[k].Destroy();
					}
				}
			}
			if (other == Faction.OfPlayer && this.HostileTo(Faction.OfPlayer))
			{
				List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
				for (int l = 0; l < allWorldObjects.Count; l++)
				{
					if (allWorldObjects[l].Faction == this)
					{
						TradeRequestComp component2 = allWorldObjects[l].GetComponent<TradeRequestComp>();
						if (component2 != null && component2.ActiveRequest)
						{
							component2.Disable();
						}
					}
				}
				foreach (Map map in Find.Maps)
				{
					map.passingShipManager.RemoveAllShipsOfFaction(this);
				}
			}
			if (canSendLetter)
			{
				TaggedString text = "";
				this.TryAppendRelationKindChangedInfo(ref text, previousKind, factionRelationKind, reason);
				if (factionRelationKind == FactionRelationKind.Hostile)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_Hostile".Translate(this.Name), text, LetterDefOf.NegativeEvent, lookTarget, this, null, null, null);
					sentLetter = true;
				}
				else if (factionRelationKind == FactionRelationKind.Ally)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_Ally".Translate(this.Name), text, LetterDefOf.PositiveEvent, lookTarget, this, null, null, null);
					sentLetter = true;
				}
				else if (factionRelationKind == FactionRelationKind.Neutral)
				{
					if (previousKind == FactionRelationKind.Hostile)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_NeutralFromHostile".Translate(this.Name), text, LetterDefOf.PositiveEvent, lookTarget, this, null, null, null);
						sentLetter = true;
					}
					else
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_NeutralFromAlly".Translate(this.Name), text, LetterDefOf.NeutralEvent, lookTarget, this, null, null, null);
						sentLetter = true;
					}
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Map> maps = Find.Maps;
				for (int m = 0; m < maps.Count; m++)
				{
					maps[m].attackTargetsCache.Notify_FactionHostilityChanged(this, other);
					LordManager lordManager = maps[m].lordManager;
					for (int n = 0; n < lordManager.lords.Count; n++)
					{
						Lord lord = lordManager.lords[n];
						if (lord.faction == other)
						{
							lord.Notify_FactionRelationsChanged(this, previousKind);
						}
						else if (lord.faction == this)
						{
							lord.Notify_FactionRelationsChanged(other, previousKind);
						}
					}
				}
			}
		}

		// Token: 0x0600589A RID: 22682 RVA: 0x001E2C20 File Offset: 0x001E0E20
		public void Notify_PlayerTraded(float marketValueSentByPlayer, Pawn playerNegotiator)
		{
			int goodwillChange = (int)(marketValueSentByPlayer / 600f);
			Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, true, !this.temporary, HistoryEventDefOf.Traded, new GlobalTargetInfo?(playerNegotiator));
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x001E2C60 File Offset: 0x001E0E60
		public void Notify_MemberExitedMap(Pawn member, bool free)
		{
			if (free && (member.HostFaction == Faction.OfPlayer || member.Faction == Faction.OfPlayer))
			{
				bool flag;
				bool flag2;
				int goodwillGainForPrisonerRelease = this.GetGoodwillGainForPrisonerRelease(member, out flag, out flag2);
				if (member.mindState.AvailableForGoodwillReward)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillGainForPrisonerRelease, true, !this.temporary, HistoryEventDefOf.MemberExitedMapHealthy, null);
				}
			}
			member.mindState.timesGuestTendedToByPlayer = 0;
		}

		// Token: 0x0600589C RID: 22684 RVA: 0x001E2CD4 File Offset: 0x001E0ED4
		public int GetGoodwillGainForPrisonerRelease(Pawn member, out bool isHealthy, out bool isInMentalState)
		{
			isHealthy = false;
			isInMentalState = member.InMentalState;
			float num = 0f;
			if (member.health.hediffSet.BleedRateTotal < 0.001f)
			{
				isHealthy = true;
				if (!isInMentalState)
				{
					num += 12f;
					if (PawnUtility.IsFactionLeader(member))
					{
						num += 40f;
					}
				}
			}
			return (int)(num + (float)Mathf.Min(member.mindState.timesGuestTendedToByPlayer, 10) * 1f);
		}

		// Token: 0x0600589D RID: 22685 RVA: 0x001E2D44 File Offset: 0x001E0F44
		public void Notify_MemberLeftExtraFaction(Pawn member)
		{
			if (member.HomeFaction == this)
			{
				return;
			}
			if (this.leader == member)
			{
				this.Notify_LeaderLost();
			}
		}

		// Token: 0x0600589E RID: 22686 RVA: 0x001E2D5F File Offset: 0x001E0F5F
		[Obsolete("Will be removed in the future")]
		public void GenerateNewLeader()
		{
			this.TryGenerateNewLeader();
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x001E2D68 File Offset: 0x001E0F68
		public bool TryGenerateNewLeader()
		{
			Pawn pawn = this.leader;
			this.leader = null;
			if (this.def.generateNewLeaderFromMapMembersOnly)
			{
				for (int i = 0; i < Find.Maps.Count; i++)
				{
					Map map = Find.Maps[i];
					for (int j = 0; j < map.mapPawns.AllPawnsCount; j++)
					{
						if (map.mapPawns.AllPawns[j] != pawn && !map.mapPawns.AllPawns[j].Destroyed && map.mapPawns.AllPawns[j].HomeFaction == this)
						{
							this.leader = map.mapPawns.AllPawns[j];
						}
					}
				}
			}
			else if (this.def.pawnGroupMakers != null)
			{
				List<PawnKindDef> list = new List<PawnKindDef>();
				foreach (PawnGroupMaker pawnGroupMaker in from x in this.def.pawnGroupMakers
				where x.kindDef == PawnGroupKindDefOf.Combat
				select x)
				{
					foreach (PawnGenOption pawnGenOption in pawnGroupMaker.options)
					{
						if (pawnGenOption.kind.factionLeader)
						{
							list.Add(pawnGenOption.kind);
						}
					}
				}
				if (this.def.fixedLeaderKinds != null)
				{
					list.AddRange(this.def.fixedLeaderKinds);
				}
				PawnKindDef kind;
				if (list.TryRandomElement(out kind))
				{
					PawnGenerationRequest request = new PawnGenerationRequest(kind, this, PawnGenerationContext.NonPlayer, -1, this.def.leaderForceGenerateNewPawn, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false);
					Gender supremeGender = this.ideos.PrimaryIdeo.SupremeGender;
					if (supremeGender != Gender.None)
					{
						request.FixedGender = new Gender?(supremeGender);
					}
					this.leader = PawnGenerator.GeneratePawn(request);
					if (this.leader.RaceProps.IsFlesh)
					{
						this.leader.relations.everSeenByPlayer = true;
					}
					if (!Find.WorldPawns.Contains(this.leader))
					{
						Find.WorldPawns.PassToWorld(this.leader, PawnDiscardDecideMode.KeepForever);
					}
				}
			}
			return this.leader != null;
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x001E3028 File Offset: 0x001E1228
		public string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x001E3030 File Offset: 0x001E1230
		public string GetInfoText()
		{
			return this.def.LabelCap + ("\n" + "goodwill".Translate().CapitalizeFirst() + ": " + this.PlayerGoodwill.ToStringWithSign());
		}

		// Token: 0x060058A2 RID: 22690 RVA: 0x00072AAA File Offset: 0x00070CAA
		Faction ICommunicable.GetFaction()
		{
			return this;
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x001E3090 File Offset: 0x001E1290
		public void TryOpenComms(Pawn negotiator)
		{
			Dialog_Negotiation dialog_Negotiation = new Dialog_Negotiation(negotiator, this, FactionDialogMaker.FactionDialogFor(negotiator, this), true);
			dialog_Negotiation.soundAmbient = SoundDefOf.RadioComms_Ambience;
			Find.WindowStack.Add(dialog_Negotiation);
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x001E30C4 File Offset: 0x001E12C4
		private bool LeaderIsAvailableToTalk()
		{
			return this.leader != null && (!this.leader.Spawned || (!this.leader.Downed && !this.leader.IsPrisoner && this.leader.Awake() && !this.leader.InMentalState));
		}

		// Token: 0x060058A5 RID: 22693 RVA: 0x001E3120 File Offset: 0x001E1320
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			if (this.IsPlayer)
			{
				return null;
			}
			string text = "CallOnRadio".Translate(this.GetCallLabel());
			text = string.Concat(new string[]
			{
				text,
				" (",
				this.PlayerRelationKind.GetLabelCap(),
				", ",
				this.PlayerGoodwill.ToStringWithSign(),
				")"
			});
			if (!this.LeaderIsAvailableToTalk())
			{
				string str;
				if (this.leader != null)
				{
					str = "LeaderUnavailable".Translate(this.leader.LabelShort, this.leader);
				}
				else
				{
					str = "LeaderUnavailableNoLeader".Translate();
				}
				return new FloatMenuOption(text + " (" + str + ")", null, this.def.FactionIcon, this.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
			{
				console.GiveUseCommsJob(negotiator, this);
			}, this.def.FactionIcon, this.Color, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null, true, 0), negotiator, console, "ReservedBy");
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x001E3280 File Offset: 0x001E1480
		private void TookDamageFromPredator(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					this.predatorThreats[i].lastAttackTicks = Find.TickManager.TicksGame;
					return;
				}
			}
			PredatorThreat predatorThreat = new PredatorThreat();
			predatorThreat.predator = predator;
			predatorThreat.lastAttackTicks = Find.TickManager.TicksGame;
			this.predatorThreats.Add(predatorThreat);
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x001E32FC File Offset: 0x001E14FC
		public bool HasPredatorRecentlyAttackedAnyone(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060058A8 RID: 22696 RVA: 0x001E3336 File Offset: 0x001E1536
		private bool IsMutuallyHostileCrossfire(DamageInfo dinfo)
		{
			return dinfo.Instigator != null && dinfo.IntendedTarget != null && dinfo.IntendedTarget.HostileTo(dinfo.Instigator) && dinfo.IntendedTarget.HostileTo(this);
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x060058A9 RID: 22697 RVA: 0x001E336E File Offset: 0x001E156E
		public static Faction OfPlayer
		{
			get
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				if (ofPlayerSilentFail == null)
				{
					Log.Error("Could not find player faction.");
				}
				return ofPlayerSilentFail;
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x060058AA RID: 22698 RVA: 0x001E3382 File Offset: 0x001E1582
		public static Faction OfMechanoids
		{
			get
			{
				return Find.FactionManager.OfMechanoids;
			}
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x060058AB RID: 22699 RVA: 0x001E338E File Offset: 0x001E158E
		public static Faction OfInsects
		{
			get
			{
				return Find.FactionManager.OfInsects;
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x060058AC RID: 22700 RVA: 0x001E339A File Offset: 0x001E159A
		public static Faction OfAncients
		{
			get
			{
				return Find.FactionManager.OfAncients;
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x060058AD RID: 22701 RVA: 0x001E33A6 File Offset: 0x001E15A6
		public static Faction OfAncientsHostile
		{
			get
			{
				return Find.FactionManager.OfAncientsHostile;
			}
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x060058AE RID: 22702 RVA: 0x001E33B2 File Offset: 0x001E15B2
		public static Faction OfEmpire
		{
			get
			{
				return Find.FactionManager.OfEmpire;
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x060058AF RID: 22703 RVA: 0x001E33BE File Offset: 0x001E15BE
		public static Faction OfPirates
		{
			get
			{
				return Find.FactionManager.OfPirates;
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x060058B0 RID: 22704 RVA: 0x001E33CC File Offset: 0x001E15CC
		public static Faction OfPlayerSilentFail
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					GameInitData gameInitData = Find.GameInitData;
					if (gameInitData != null && gameInitData.playerFaction != null)
					{
						return gameInitData.playerFaction;
					}
				}
				return Find.FactionManager.OfPlayer;
			}
		}

		// Token: 0x060058B1 RID: 22705 RVA: 0x001E3404 File Offset: 0x001E1604
		public void Notify_RoyalThingUseViolation(Def implantOrWeapon, Pawn pawn, string violationSourceName, float detectionChance, int violationSourceLevel = 0)
		{
			if (!this.HostileTo(Faction.OfPlayer))
			{
				RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(implantOrWeapon, this, violationSourceLevel);
				string arg = (minTitleToUse == null) ? "None".Translate() : minTitleToUse.GetLabelCapFor(pawn);
				Faction.OfPlayer.TryAffectGoodwillWith(this, -4, true, true, HistoryEventDefOf.UsedForbiddenThing, new GlobalTargetInfo?(pawn));
				Find.LetterStack.ReceiveLetter("LetterLawViolationDetectedLabel".Translate(pawn.Named("PAWN")).CapitalizeFirst(), "LetterLawViolationDetectedForbiddenThingUse".Translate(arg.Named("TITLE"), pawn.Named("PAWN"), violationSourceName.Named("CULPRIT"), this.Named("FACTION"), 4.ToString().Named("GOODWILL"), detectionChance.ToStringPercent().Named("CHANCE")), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
			}
		}

		// Token: 0x060058B2 RID: 22706 RVA: 0x001E34F8 File Offset: 0x001E16F8
		public RoyalTitleDef GetMinTitleForImplant(HediffDef implantDef, int level = 0)
		{
			if (this.def.royalImplantRules == null || this.def.royalImplantRules.Count == 0)
			{
				return null;
			}
			foreach (RoyalImplantRule royalImplantRule in this.def.royalImplantRules)
			{
				if (royalImplantRule.implantHediff == implantDef && (royalImplantRule.maxLevel >= level || level == 0))
				{
					return royalImplantRule.minTitle;
				}
			}
			return null;
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x001E358C File Offset: 0x001E178C
		public RoyalImplantRule GetMaxAllowedImplantLevel(HediffDef implantDef, RoyalTitleDef title)
		{
			if (this.def.royalImplantRules == null || this.def.royalImplantRules.Count == 0)
			{
				return null;
			}
			if (title == null)
			{
				return null;
			}
			int myTitleIdx = this.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(title);
			return this.def.royalImplantRules.Where(delegate(RoyalImplantRule r)
			{
				if (r.implantHediff == implantDef)
				{
					RoyalTitleDef minTitleForImplant = this.GetMinTitleForImplant(implantDef, r.maxLevel);
					int num = this.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant);
					return myTitleIdx == -1 || myTitleIdx >= num;
				}
				return false;
			}).Last<RoyalImplantRule>();
		}

		// Token: 0x060058B4 RID: 22708 RVA: 0x001E360C File Offset: 0x001E180C
		public static Pair<Faction, RoyalTitleDef> GetMinTitleForImplantAllFactions(HediffDef implantDef)
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				RoyalTitleDef minTitleForImplant = faction.GetMinTitleForImplant(implantDef, 0);
				if (minTitleForImplant != null)
				{
					return new Pair<Faction, RoyalTitleDef>(faction, minTitleForImplant);
				}
			}
			return default(Pair<Faction, RoyalTitleDef>);
		}

		// Token: 0x060058B5 RID: 22709 RVA: 0x001E3680 File Offset: 0x001E1880
		public string GetUniqueLoadID()
		{
			return "Faction_" + this.loadID;
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x001E3697 File Offset: 0x001E1897
		public override string ToString()
		{
			if (this.name != null)
			{
				return this.name;
			}
			if (this.def != null)
			{
				return this.def.defName;
			}
			return "[faction of no def]";
		}

		// Token: 0x060058B8 RID: 22712 RVA: 0x001E36D0 File Offset: 0x001E18D0
		[CompilerGenerated]
		internal static int <TryMakeInitialRelationsWith>g__GetInitialGoodwill|61_0(Faction a, Faction b)
		{
			if (a.def.permanentEnemy)
			{
				return -100;
			}
			if (a.def.permanentEnemyToEveryoneExceptPlayer && !b.IsPlayer)
			{
				return -100;
			}
			if (a.def.permanentEnemyToEveryoneExcept != null && !a.def.permanentEnemyToEveryoneExcept.Contains(b.def))
			{
				return -100;
			}
			if (a.def.naturalEnemy)
			{
				return -80;
			}
			return 0;
		}

		// Token: 0x04003421 RID: 13345
		public FactionDef def;

		// Token: 0x04003422 RID: 13346
		private string name;

		// Token: 0x04003423 RID: 13347
		public int loadID = -1;

		// Token: 0x04003424 RID: 13348
		public int randomKey;

		// Token: 0x04003425 RID: 13349
		public float colorFromSpectrum = -999f;

		// Token: 0x04003426 RID: 13350
		public float centralMelanin = 0.5f;

		// Token: 0x04003427 RID: 13351
		private List<FactionRelation> relations = new List<FactionRelation>();

		// Token: 0x04003428 RID: 13352
		public Pawn leader;

		// Token: 0x04003429 RID: 13353
		public KidnappedPawnsTracker kidnapped;

		// Token: 0x0400342A RID: 13354
		private List<PredatorThreat> predatorThreats = new List<PredatorThreat>();

		// Token: 0x0400342B RID: 13355
		public bool defeated;

		// Token: 0x0400342C RID: 13356
		public int lastTraderRequestTick = -9999999;

		// Token: 0x0400342D RID: 13357
		public int lastMilitaryAidRequestTick = -9999999;

		// Token: 0x0400342E RID: 13358
		public int lastExecutionTick = -9999999;

		// Token: 0x0400342F RID: 13359
		private int naturalGoodwillTimer;

		// Token: 0x04003430 RID: 13360
		public bool allowRoyalFavorRewards = true;

		// Token: 0x04003431 RID: 13361
		public bool allowGoodwillRewards = true;

		// Token: 0x04003432 RID: 13362
		public List<string> questTags;

		// Token: 0x04003433 RID: 13363
		public Color? color;

		// Token: 0x04003434 RID: 13364
		public bool? hidden;

		// Token: 0x04003435 RID: 13365
		public bool temporary;

		// Token: 0x04003436 RID: 13366
		public bool factionHostileOnHarmByPlayer;

		// Token: 0x04003437 RID: 13367
		public bool neverFlee;

		// Token: 0x04003438 RID: 13368
		public FactionIdeosTracker ideos;

		// Token: 0x04003439 RID: 13369
		private List<Map> avoidGridsBasicKeysWorkingList;

		// Token: 0x0400343A RID: 13370
		private List<ByteGrid> avoidGridsBasicValuesWorkingList;

		// Token: 0x0400343B RID: 13371
		private List<Map> avoidGridsSmartKeysWorkingList;

		// Token: 0x0400343C RID: 13372
		private List<ByteGrid> avoidGridsSmartValuesWorkingList;

		// Token: 0x0400343D RID: 13373
		private static List<PawnKindDef> allPawnKinds = new List<PawnKindDef>();

		// Token: 0x0400343E RID: 13374
		public const int RoyalThingUseViolationGoodwillImpact = 4;
	}
}
