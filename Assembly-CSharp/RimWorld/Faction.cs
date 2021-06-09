using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001587 RID: 5511
	public class Faction : IExposable, ILoadReferenceable, ICommunicable
	{
		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06007775 RID: 30581 RVA: 0x00050945 File Offset: 0x0004EB45
		// (set) Token: 0x06007776 RID: 30582 RVA: 0x00050966 File Offset: 0x0004EB66
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

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06007777 RID: 30583 RVA: 0x0005096F File Offset: 0x0004EB6F
		public bool HasName
		{
			get
			{
				return this.name != null;
			}
		}

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06007778 RID: 30584 RVA: 0x0005097A File Offset: 0x0004EB7A
		public bool IsPlayer
		{
			get
			{
				return this.def.isPlayer;
			}
		}

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06007779 RID: 30585 RVA: 0x00050987 File Offset: 0x0004EB87
		public int PlayerGoodwill
		{
			get
			{
				return this.GoodwillWith(Faction.OfPlayer);
			}
		}

		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x0600777A RID: 30586 RVA: 0x00050994 File Offset: 0x0004EB94
		public FactionRelationKind PlayerRelationKind
		{
			get
			{
				return this.RelationKindWith(Faction.OfPlayer);
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x0600777B RID: 30587 RVA: 0x00244964 File Offset: 0x00242B64
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

		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x0600777C RID: 30588 RVA: 0x002449B8 File Offset: 0x00242BB8
		public string LeaderTitle
		{
			get
			{
				if (this.leader != null && this.leader.gender == Gender.Female && !string.IsNullOrEmpty(this.def.leaderTitleFemale))
				{
					return this.def.leaderTitleFemale;
				}
				return this.def.leaderTitle;
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x0600777D RID: 30589 RVA: 0x000509A1 File Offset: 0x0004EBA1
		private bool ShouldHaveLeader
		{
			get
			{
				return !this.IsPlayer && !this.Hidden && !this.temporary;
			}
		}

		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x0600777E RID: 30590 RVA: 0x000509BE File Offset: 0x0004EBBE
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

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x0600777F RID: 30591 RVA: 0x000509E0 File Offset: 0x0004EBE0
		[Obsolete]
		public bool CanGiveGoodwillRewards
		{
			get
			{
				return this.CanEverGiveGoodwillRewards;
			}
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06007780 RID: 30592 RVA: 0x000509E8 File Offset: 0x0004EBE8
		public bool CanEverGiveGoodwillRewards
		{
			get
			{
				return !this.def.permanentEnemy && this.HasGoodwill;
			}
		}

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06007781 RID: 30593 RVA: 0x000509FF File Offset: 0x0004EBFF
		public string GetReportText
		{
			get
			{
				return this.def.description + (this.def.HasRoyalTitles ? ("\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(this, null)) : "");
			}
		}

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06007782 RID: 30594 RVA: 0x00244A04 File Offset: 0x00242C04
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

		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06007783 RID: 30595 RVA: 0x00050A36 File Offset: 0x0004EC36
		public bool HasGoodwill
		{
			get
			{
				return !this.Hidden && !this.temporary;
			}
		}

		// Token: 0x06007784 RID: 30596 RVA: 0x00244A34 File Offset: 0x00242C34
		public Faction()
		{
			this.randomKey = Rand.Range(0, int.MaxValue);
			this.kidnapped = new KidnappedPawnsTracker(this);
		}

		// Token: 0x06007785 RID: 30597 RVA: 0x00244AC4 File Offset: 0x00242CC4
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
			Scribe_Collections.Look<PredatorThreat>(ref this.predatorThreats, "predatorThreats", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.defeated, "defeated", false, false);
			Scribe_Values.Look<int>(ref this.lastTraderRequestTick, "lastTraderRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastMilitaryAidRequestTick, "lastMilitaryAidRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.naturalGoodwillTimer, "naturalGoodwillTimer", 0, false);
			Scribe_Values.Look<bool>(ref this.allowRoyalFavorRewards, "allowRoyalFavorRewards", true, false);
			Scribe_Values.Look<bool>(ref this.allowGoodwillRewards, "allowGoodwillRewards", true, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool?>(ref this.hidden, "hidden", null, false);
			Scribe_Values.Look<bool>(ref this.temporary, "temporary", false, false);
			Scribe_Values.Look<Color?>(ref this.color, "color", null, false);
			Scribe_Values.Look<bool>(ref this.hostileFromMemberCapture, "hostileFromMemberCapture", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.predatorThreats.RemoveAll((PredatorThreat x) => x.predator == null);
			}
		}

		// Token: 0x06007786 RID: 30598 RVA: 0x00244CB0 File Offset: 0x00242EB0
		public void FactionTick()
		{
			this.CheckNaturalTendencyToReachGoodwillThreshold();
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
				Log.ErrorOnce("Faction leader for " + this.Name + " is null.", this.loadID ^ 441821, false);
			}
		}

		// Token: 0x06007787 RID: 30599 RVA: 0x00244E44 File Offset: 0x00243044
		private void CheckNaturalTendencyToReachGoodwillThreshold()
		{
			if (this.IsPlayer)
			{
				return;
			}
			int playerGoodwill = this.PlayerGoodwill;
			if (this.def.naturalColonyGoodwill.Includes(playerGoodwill))
			{
				this.naturalGoodwillTimer = 0;
				return;
			}
			this.naturalGoodwillTimer++;
			if (playerGoodwill < this.def.naturalColonyGoodwill.min)
			{
				if (this.def.goodwillDailyGain != 0f)
				{
					int num = (int)(10f / this.def.goodwillDailyGain * 60000f);
					if (this.naturalGoodwillTimer >= num)
					{
						Faction ofPlayer = Faction.OfPlayer;
						int goodwillChange = Mathf.Min(10, this.def.naturalColonyGoodwill.min - playerGoodwill);
						bool canSendMessage = true;
						string reason = "GoodwillChangedReason_NaturallyOverTime".Translate(this.def.naturalColonyGoodwill.min.ToString());
						this.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, !this.temporary, reason, null);
						this.naturalGoodwillTimer = 0;
						return;
					}
				}
			}
			else if (playerGoodwill > this.def.naturalColonyGoodwill.max && this.def.goodwillDailyFall != 0f)
			{
				int num2 = (int)(10f / this.def.goodwillDailyFall * 60000f);
				if (this.naturalGoodwillTimer >= num2)
				{
					Faction ofPlayer2 = Faction.OfPlayer;
					int goodwillChange2 = -Mathf.Min(10, playerGoodwill - this.def.naturalColonyGoodwill.max);
					bool canSendMessage2 = true;
					string reason = "GoodwillChangedReason_NaturallyOverTime".Translate(this.def.naturalColonyGoodwill.max.ToString());
					this.TryAffectGoodwillWith(ofPlayer2, goodwillChange2, canSendMessage2, !this.temporary, reason, null);
					this.naturalGoodwillTimer = 0;
				}
			}
		}

		// Token: 0x06007788 RID: 30600 RVA: 0x00245004 File Offset: 0x00243204
		public void TryMakeInitialRelationsWith(Faction other)
		{
			if (this.RelationWith(other, true) != null)
			{
				return;
			}
			int a = this.def.permanentEnemy ? -100 : this.def.startingGoodwill.RandomInRange;
			if (this.IsPlayer)
			{
				a = 100;
			}
			if (this.def.permanentEnemyToEveryoneExceptPlayer && !other.IsPlayer)
			{
				a = -100;
			}
			if (this.def.permanentEnemyToEveryoneExcept != null && !this.def.permanentEnemyToEveryoneExcept.Contains(other.def))
			{
				a = -100;
			}
			int b = other.def.permanentEnemy ? -100 : other.def.startingGoodwill.RandomInRange;
			if (other.IsPlayer)
			{
				b = 100;
			}
			if (other.def.permanentEnemyToEveryoneExceptPlayer && !this.IsPlayer)
			{
				b = -100;
			}
			if (other.def.permanentEnemyToEveryoneExcept != null && !other.def.permanentEnemyToEveryoneExcept.Contains(this.def))
			{
				b = -100;
			}
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
			factionRelation.goodwill = num;
			factionRelation.kind = kind;
			this.relations.Add(factionRelation);
			FactionRelation factionRelation2 = new FactionRelation();
			factionRelation2.other = this;
			factionRelation2.goodwill = num;
			factionRelation2.kind = kind;
			other.relations.Add(factionRelation2);
		}

		// Token: 0x06007789 RID: 30601 RVA: 0x0024516C File Offset: 0x0024336C
		public void SetRelation(FactionRelation relation)
		{
			if (relation.other == this)
			{
				Log.Error("Tried to set relation between faction " + this + " and itself.", false);
				return;
			}
			if (relation.other == null)
			{
				Log.Error("Relation is missing faction.", false);
				return;
			}
			this.relations.RemoveAll((FactionRelation r) => r.other == relation.other);
			relation.other.relations.RemoveAll((FactionRelation r) => r.other == this);
			this.relations.Add(relation);
			FactionRelation factionRelation = new FactionRelation();
			factionRelation.other = this;
			factionRelation.goodwill = relation.goodwill;
			factionRelation.kind = relation.kind;
			relation.other.relations.Add(factionRelation);
		}

		// Token: 0x0600778A RID: 30602 RVA: 0x0024525C File Offset: 0x0024345C
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

		// Token: 0x0600778B RID: 30603 RVA: 0x00245308 File Offset: 0x00243508
		public FactionRelation RelationWith(Faction other, bool allowNull = false)
		{
			if (other == this)
			{
				Log.Error("Tried to get relation between faction " + this + " and itself.", false);
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
				}), false);
				return new FactionRelation();
			}
			return null;
		}

		// Token: 0x0600778C RID: 30604 RVA: 0x00050A4B File Offset: 0x0004EC4B
		public int GoodwillWith(Faction other)
		{
			return this.RelationWith(other, false).goodwill;
		}

		// Token: 0x0600778D RID: 30605 RVA: 0x00050A5A File Offset: 0x0004EC5A
		public FactionRelationKind RelationKindWith(Faction other)
		{
			return this.RelationWith(other, false).kind;
		}

		// Token: 0x0600778E RID: 30606 RVA: 0x002453AC File Offset: 0x002435AC
		public bool CanChangeGoodwillFor(Faction other, int goodwillChange)
		{
			return this.HasGoodwill && other.HasGoodwill && !this.def.permanentEnemy && !other.def.permanentEnemy && !this.defeated && !other.defeated && other != this && (!this.def.permanentEnemyToEveryoneExceptPlayer || other.IsPlayer) && (!other.def.permanentEnemyToEveryoneExceptPlayer || this.IsPlayer) && (this.def.permanentEnemyToEveryoneExcept == null || this.def.permanentEnemyToEveryoneExcept.Contains(other.def)) && (other.def.permanentEnemyToEveryoneExcept == null || other.def.permanentEnemyToEveryoneExcept.Contains(this.def)) && (goodwillChange <= 0 || ((!this.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(other)) && (!other.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(this))));
		}

		// Token: 0x0600778F RID: 30607 RVA: 0x002454A4 File Offset: 0x002436A4
		public bool TryAffectGoodwillWith(Faction other, int goodwillChange, bool canSendMessage = true, bool canSendHostilityLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
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
			int num2 = Mathf.Clamp(num + goodwillChange, -100, 100);
			if (num == num2)
			{
				return true;
			}
			FactionRelation factionRelation = this.RelationWith(other, false);
			factionRelation.goodwill = num2;
			bool flag;
			factionRelation.CheckKindThresholds(this, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			FactionRelationKind kind = factionRelation2.kind;
			factionRelation2.goodwill = factionRelation.goodwill;
			factionRelation2.kind = factionRelation.kind;
			bool flag2;
			if (kind != factionRelation2.kind)
			{
				other.Notify_RelationKindChanged(this, kind, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag2);
			}
			else
			{
				flag2 = false;
			}
			if (canSendMessage && !flag && !flag2 && Current.ProgramState == ProgramState.Playing && (this.IsPlayer || other.IsPlayer))
			{
				Faction faction = this.IsPlayer ? other : this;
				string text;
				if (!reason.NullOrEmpty())
				{
					text = "MessageGoodwillChangedWithReason".Translate(faction.name, num.ToString("F0"), factionRelation.goodwill.ToString("F0"), reason);
				}
				else
				{
					text = "MessageGoodwillChanged".Translate(faction.name, num.ToString("F0"), factionRelation.goodwill.ToString("F0"));
				}
				Messages.Message(text, lookTarget ?? GlobalTargetInfo.Invalid, ((float)goodwillChange > 0f) ? MessageTypeDefOf.PositiveEvent : MessageTypeDefOf.NegativeEvent, true);
			}
			return true;
		}

		// Token: 0x06007790 RID: 30608 RVA: 0x00245698 File Offset: 0x00243898
		public void SetRelationDirect(Faction other, FactionRelationKind kind, bool canSendHostilityLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
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

		// Token: 0x06007791 RID: 30609 RVA: 0x00050A69 File Offset: 0x0004EC69
		public bool TrySetNotHostileTo(Faction other, bool canSendLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (this.RelationKindWith(other) == FactionRelationKind.Hostile)
			{
				this.TrySetRelationKind(other, FactionRelationKind.Neutral, canSendLetter, reason, lookTarget);
			}
			return this.RelationKindWith(other) > FactionRelationKind.Hostile;
		}

		// Token: 0x06007792 RID: 30610 RVA: 0x00050A8B File Offset: 0x0004EC8B
		public bool TrySetNotAlly(Faction other, bool canSendLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (this.RelationKindWith(other) == FactionRelationKind.Ally)
			{
				this.TrySetRelationKind(other, FactionRelationKind.Neutral, canSendLetter, reason, lookTarget);
			}
			return this.RelationKindWith(other) != FactionRelationKind.Ally;
		}

		// Token: 0x06007793 RID: 30611 RVA: 0x00245724 File Offset: 0x00243924
		public bool TrySetRelationKind(Faction other, FactionRelationKind kind, bool canSendLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			FactionRelation factionRelation = this.RelationWith(other, false);
			if (factionRelation.kind == kind)
			{
				return true;
			}
			if (!this.HasGoodwill)
			{
				this.SetRelationDirect(other, kind, canSendLetter, reason, lookTarget);
				return true;
			}
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				this.TryAffectGoodwillWith(other, -75 - factionRelation.goodwill, false, canSendLetter, reason, lookTarget);
				return factionRelation.kind == FactionRelationKind.Hostile;
			case FactionRelationKind.Neutral:
				this.TryAffectGoodwillWith(other, 0 - factionRelation.goodwill, false, canSendLetter, reason, lookTarget);
				return factionRelation.kind == FactionRelationKind.Neutral;
			case FactionRelationKind.Ally:
				this.TryAffectGoodwillWith(other, 75 - factionRelation.goodwill, false, canSendLetter, reason, lookTarget);
				return factionRelation.kind == FactionRelationKind.Ally;
			default:
				throw new NotSupportedException(kind.ToString());
			}
		}

		// Token: 0x06007794 RID: 30612 RVA: 0x002457E4 File Offset: 0x002439E4
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

		// Token: 0x06007795 RID: 30613 RVA: 0x0024585C File Offset: 0x00243A5C
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

		// Token: 0x06007796 RID: 30614 RVA: 0x0024589C File Offset: 0x00243A9C
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

		// Token: 0x06007797 RID: 30615 RVA: 0x00245C10 File Offset: 0x00243E10
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
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo))
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int num2 = (int)(-1.3f * num);
				Faction faction = dinfo.Instigator.Faction;
				int goodwillChange = num2;
				bool canSendMessage = true;
				string reason = "GoodwillChangedReason_AttackedPawn".Translate(member.LabelShort, member);
				GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(member);
				this.TryAffectGoodwillWith(faction, goodwillChange, canSendMessage, !this.temporary, reason, lookTarget);
			}
		}

		// Token: 0x06007798 RID: 30616 RVA: 0x00245D80 File Offset: 0x00243F80
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
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo))
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int goodwillChange = (int)(-1f * num);
				this.TryAffectGoodwillWith(dinfo.Instigator.Faction, goodwillChange, true, true, "GoodwillChangedReason_AttackedPawn".Translate(building.LabelShort, building), new GlobalTargetInfo?(building));
			}
		}

		// Token: 0x06007799 RID: 30617 RVA: 0x00245E4C File Offset: 0x0024404C
		public void Notify_MemberCaptured(Pawn member, Faction violator)
		{
			if (violator == this)
			{
				return;
			}
			if (this.RelationKindWith(violator) != FactionRelationKind.Hostile && this.hostileFromMemberCapture)
			{
				FactionRelationKind kind = FactionRelationKind.Hostile;
				string reason = "GoodwillChangedReason_CapturedPawn".Translate(member.LabelShort, member);
				GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(member);
				this.TrySetRelationKind(violator, kind, !this.temporary, reason, lookTarget);
			}
		}

		// Token: 0x0600779A RID: 30618 RVA: 0x00245EB4 File Offset: 0x002440B4
		public void Notify_MemberStripped(Pawn member, Faction violator)
		{
			if (violator == this || this.Hidden || member.Dead)
			{
				return;
			}
			if (violator == Faction.OfPlayer && this.RelationKindWith(violator) != FactionRelationKind.Hostile)
			{
				Faction ofPlayer = Faction.OfPlayer;
				int goodwillChange = -40;
				bool canSendMessage = true;
				string reason = "GoodwillChangedReason_PawnStripped".Translate(member);
				GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(member);
				this.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, !this.temporary, reason, lookTarget);
			}
		}

		// Token: 0x0600779B RID: 30619 RVA: 0x00245F24 File Offset: 0x00244124
		public void Notify_MemberDied(Pawn member, DamageInfo? dinfo, bool wasWorldPawn, Map map)
		{
			if (this.IsPlayer)
			{
				return;
			}
			if (!wasWorldPawn && !PawnGenerator.IsBeingGenerated(member) && Current.ProgramState == ProgramState.Playing && map != null && map.IsPlayerHome && !this.HostileTo(Faction.OfPlayer))
			{
				if (dinfo != null && dinfo.Value.Category == DamageInfo.SourceCategory.Collapse)
				{
					bool flag = MessagesRepeatAvoider.MessageShowAllowed("FactionRelationAdjustmentCrushed-" + this.Name, 5f);
					Faction ofPlayer = Faction.OfPlayer;
					int goodwillChange = member.RaceProps.Humanlike ? -25 : -15;
					bool canSendMessage = flag;
					string reason = "GoodwillChangedReason_PawnCrushed".Translate(member.LabelShort, member);
					GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(new TargetInfo(member.Position, map, false));
					this.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, !this.temporary, reason, lookTarget);
				}
				else if (dinfo != null && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == null))
				{
					Pawn pawn = dinfo.Value.Instigator as Pawn;
					if (pawn == null || !pawn.RaceProps.Animal || pawn.mindState.mentalStateHandler.CurStateDef != MentalStateDefOf.ManhunterPermanent)
					{
						Faction ofPlayer2 = Faction.OfPlayer;
						int goodwillChange2 = member.RaceProps.Humanlike ? -5 : -3;
						bool canSendMessage2 = true;
						string reason = "GoodwillChangedReason_PawnDied".Translate(member.LabelShort, member);
						GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(member);
						this.TryAffectGoodwillWith(ofPlayer2, goodwillChange2, canSendMessage2, !this.temporary, reason, lookTarget);
					}
				}
				else if (member.kindDef.factionHostileOnDeath || (member.kindDef.factionHostileOnKill && dinfo != null && dinfo.Value.Instigator != null && dinfo.Value.Instigator.Faction == Faction.OfPlayer))
				{
					Faction ofPlayer3 = Faction.OfPlayer;
					FactionRelationKind kind = FactionRelationKind.Hostile;
					bool canSendLetter = !this.temporary;
					string reason2 = null;
					GlobalTargetInfo? lookTarget = null;
					this.TrySetRelationKind(ofPlayer3, kind, canSendLetter, reason2, lookTarget);
				}
			}
			if (member == this.leader)
			{
				this.Notify_LeaderDied();
			}
		}

		// Token: 0x0600779C RID: 30620 RVA: 0x00246174 File Offset: 0x00244374
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

		// Token: 0x0600779D RID: 30621 RVA: 0x002462E4 File Offset: 0x002444E4
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

		// Token: 0x0600779E RID: 30622 RVA: 0x00246420 File Offset: 0x00244620
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
							pawn.guest.SetGuestStatus(pawn.HostFaction, true);
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

		// Token: 0x0600779F RID: 30623 RVA: 0x00246980 File Offset: 0x00244B80
		public void Notify_PlayerTraded(float marketValueSentByPlayer, Pawn playerNegotiator)
		{
			Faction ofPlayer = Faction.OfPlayer;
			int goodwillChange = (int)(marketValueSentByPlayer / 600f);
			bool canSendMessage = true;
			string reason = "GoodwillChangedReason_Traded".Translate();
			GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(playerNegotiator);
			this.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, !this.temporary, reason, lookTarget);
		}

		// Token: 0x060077A0 RID: 30624 RVA: 0x002469CC File Offset: 0x00244BCC
		public void Notify_MemberExitedMap(Pawn member, bool free)
		{
			if (free && member.HostFaction != null)
			{
				bool flag;
				int goodwillGainForPrisonerRelease = this.GetGoodwillGainForPrisonerRelease(member, out flag);
				if (member.mindState.AvailableForGoodwillReward)
				{
					Faction hostFaction = member.HostFaction;
					int goodwillChange = goodwillGainForPrisonerRelease;
					bool canSendMessage = true;
					string reason = flag ? "GoodwillChangedReason_ExitedMapHealthy".Translate(member.LabelShort, member) : "GoodwillChangedReason_Tended".Translate(member.LabelShort, member);
					this.TryAffectGoodwillWith(hostFaction, goodwillChange, canSendMessage, !this.temporary, reason, null);
				}
			}
			member.mindState.timesGuestTendedToByPlayer = 0;
		}

		// Token: 0x060077A1 RID: 30625 RVA: 0x00246A70 File Offset: 0x00244C70
		public int GetGoodwillGainForPrisonerRelease(Pawn member, out bool isHealthy)
		{
			isHealthy = false;
			float num = 0f;
			if (!member.InMentalState && member.health.hediffSet.BleedRateTotal < 0.001f)
			{
				isHealthy = true;
				num += 12f;
				if (PawnUtility.IsFactionLeader(member))
				{
					num += 40f;
				}
			}
			return (int)(num + (float)Mathf.Min(member.mindState.timesGuestTendedToByPlayer, 10) * 1f);
		}

		// Token: 0x060077A2 RID: 30626 RVA: 0x00050AB1 File Offset: 0x0004ECB1
		[Obsolete("Will be removed in the future")]
		public void GenerateNewLeader()
		{
			this.TryGenerateNewLeader();
		}

		// Token: 0x060077A3 RID: 30627 RVA: 0x00246ADC File Offset: 0x00244CDC
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
						if (map.mapPawns.AllPawns[j] != pawn && !map.mapPawns.AllPawns[j].Destroyed && map.mapPawns.AllPawns[j].FactionOrExtraMiniOrHomeFaction == this)
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
					PawnGenerationRequest request = new PawnGenerationRequest(kind, this, PawnGenerationContext.NonPlayer, -1, this.def.leaderForceGenerateNewPawn, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
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

		// Token: 0x060077A4 RID: 30628 RVA: 0x00050ABA File Offset: 0x0004ECBA
		public string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x060077A5 RID: 30629 RVA: 0x00246D70 File Offset: 0x00244F70
		public string GetInfoText()
		{
			return this.def.LabelCap + ("\n" + "goodwill".Translate().CapitalizeFirst() + ": " + this.PlayerGoodwill.ToStringWithSign());
		}

		// Token: 0x060077A6 RID: 30630 RVA: 0x000187F7 File Offset: 0x000169F7
		Faction ICommunicable.GetFaction()
		{
			return this;
		}

		// Token: 0x060077A7 RID: 30631 RVA: 0x00246DD0 File Offset: 0x00244FD0
		public void TryOpenComms(Pawn negotiator)
		{
			Dialog_Negotiation dialog_Negotiation = new Dialog_Negotiation(negotiator, this, FactionDialogMaker.FactionDialogFor(negotiator, this), true);
			dialog_Negotiation.soundAmbient = SoundDefOf.RadioComms_Ambience;
			Find.WindowStack.Add(dialog_Negotiation);
		}

		// Token: 0x060077A8 RID: 30632 RVA: 0x00246E04 File Offset: 0x00245004
		private bool LeaderIsAvailableToTalk()
		{
			return this.leader != null && (!this.leader.Spawned || (!this.leader.Downed && !this.leader.IsPrisoner && this.leader.Awake() && !this.leader.InMentalState));
		}

		// Token: 0x060077A9 RID: 30633 RVA: 0x00246E60 File Offset: 0x00245060
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
				this.PlayerRelationKind.GetLabel(),
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
				return new FloatMenuOption(text + " (" + str + ")", null, this.def.FactionIcon, this.Color, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
			{
				console.GiveUseCommsJob(negotiator, this);
			}, this.def.FactionIcon, this.Color, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null), negotiator, console, "ReservedBy");
		}

		// Token: 0x060077AA RID: 30634 RVA: 0x00246FBC File Offset: 0x002451BC
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

		// Token: 0x060077AB RID: 30635 RVA: 0x00247038 File Offset: 0x00245238
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

		// Token: 0x060077AC RID: 30636 RVA: 0x00050AC2 File Offset: 0x0004ECC2
		private bool IsMutuallyHostileCrossfire(DamageInfo dinfo)
		{
			return dinfo.Instigator != null && dinfo.IntendedTarget != null && dinfo.IntendedTarget.HostileTo(dinfo.Instigator) && dinfo.IntendedTarget.HostileTo(this);
		}

		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x060077AD RID: 30637 RVA: 0x00050AFA File Offset: 0x0004ECFA
		public static Faction OfPlayer
		{
			get
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				if (ofPlayerSilentFail == null)
				{
					Log.Error("Could not find player faction.", false);
				}
				return ofPlayerSilentFail;
			}
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x060077AE RID: 30638 RVA: 0x00050B0F File Offset: 0x0004ED0F
		public static Faction OfMechanoids
		{
			get
			{
				return Find.FactionManager.OfMechanoids;
			}
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x060077AF RID: 30639 RVA: 0x00050B1B File Offset: 0x0004ED1B
		public static Faction OfInsects
		{
			get
			{
				return Find.FactionManager.OfInsects;
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x060077B0 RID: 30640 RVA: 0x00050B27 File Offset: 0x0004ED27
		public static Faction OfAncients
		{
			get
			{
				return Find.FactionManager.OfAncients;
			}
		}

		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x060077B1 RID: 30641 RVA: 0x00050B33 File Offset: 0x0004ED33
		public static Faction OfAncientsHostile
		{
			get
			{
				return Find.FactionManager.OfAncientsHostile;
			}
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x060077B2 RID: 30642 RVA: 0x00050B3F File Offset: 0x0004ED3F
		public static Faction Empire
		{
			get
			{
				return Find.FactionManager.Empire;
			}
		}

		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x060077B3 RID: 30643 RVA: 0x00247074 File Offset: 0x00245274
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

		// Token: 0x060077B4 RID: 30644 RVA: 0x002470AC File Offset: 0x002452AC
		public void Notify_RoyalThingUseViolation(Def implantOrWeapon, Pawn pawn, string violationSourceName, float detectionChance, int violationSourceLevel = 0)
		{
			if (!this.HostileTo(Faction.OfPlayer))
			{
				RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(implantOrWeapon, this, violationSourceLevel);
				string arg = (minTitleToUse == null) ? "None".Translate() : minTitleToUse.GetLabelCapFor(pawn);
				this.TryAffectGoodwillWith(pawn.Faction, -4, true, true, "GoodwillChangedReason_UsedForbiddenThing".Translate(pawn.Named("PAWN"), violationSourceName.Named("CULPRIT")), new GlobalTargetInfo?(pawn));
				Find.LetterStack.ReceiveLetter("LetterLawViolationDetectedLabel".Translate(pawn.Named("PAWN")).CapitalizeFirst(), "LetterLawViolationDetectedForbiddenThingUse".Translate(arg.Named("TITLE"), pawn.Named("PAWN"), violationSourceName.Named("CULPRIT"), this.Named("FACTION"), 4.ToString().Named("GOODWILL"), detectionChance.ToStringPercent().Named("CHANCE")), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
			}
		}

		// Token: 0x060077B5 RID: 30645 RVA: 0x002471C0 File Offset: 0x002453C0
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

		// Token: 0x060077B6 RID: 30646 RVA: 0x00247254 File Offset: 0x00245454
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

		// Token: 0x060077B7 RID: 30647 RVA: 0x002472D4 File Offset: 0x002454D4
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

		// Token: 0x060077B8 RID: 30648 RVA: 0x00050B4B File Offset: 0x0004ED4B
		public string GetUniqueLoadID()
		{
			return "Faction_" + this.loadID;
		}

		// Token: 0x060077B9 RID: 30649 RVA: 0x00050B62 File Offset: 0x0004ED62
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

		// Token: 0x04004ECF RID: 20175
		public FactionDef def;

		// Token: 0x04004ED0 RID: 20176
		private string name;

		// Token: 0x04004ED1 RID: 20177
		public int loadID = -1;

		// Token: 0x04004ED2 RID: 20178
		public int randomKey;

		// Token: 0x04004ED3 RID: 20179
		public float colorFromSpectrum = -999f;

		// Token: 0x04004ED4 RID: 20180
		public float centralMelanin = 0.5f;

		// Token: 0x04004ED5 RID: 20181
		private List<FactionRelation> relations = new List<FactionRelation>();

		// Token: 0x04004ED6 RID: 20182
		public Pawn leader;

		// Token: 0x04004ED7 RID: 20183
		public KidnappedPawnsTracker kidnapped;

		// Token: 0x04004ED8 RID: 20184
		private List<PredatorThreat> predatorThreats = new List<PredatorThreat>();

		// Token: 0x04004ED9 RID: 20185
		public bool defeated;

		// Token: 0x04004EDA RID: 20186
		public int lastTraderRequestTick = -9999999;

		// Token: 0x04004EDB RID: 20187
		public int lastMilitaryAidRequestTick = -9999999;

		// Token: 0x04004EDC RID: 20188
		private int naturalGoodwillTimer;

		// Token: 0x04004EDD RID: 20189
		public bool allowRoyalFavorRewards = true;

		// Token: 0x04004EDE RID: 20190
		public bool allowGoodwillRewards = true;

		// Token: 0x04004EDF RID: 20191
		public List<string> questTags;

		// Token: 0x04004EE0 RID: 20192
		public Color? color;

		// Token: 0x04004EE1 RID: 20193
		public bool? hidden;

		// Token: 0x04004EE2 RID: 20194
		public bool temporary;

		// Token: 0x04004EE3 RID: 20195
		public bool hostileFromMemberCapture = true;

		// Token: 0x04004EE4 RID: 20196
		private List<Map> avoidGridsBasicKeysWorkingList;

		// Token: 0x04004EE5 RID: 20197
		private List<ByteGrid> avoidGridsBasicValuesWorkingList;

		// Token: 0x04004EE6 RID: 20198
		private List<Map> avoidGridsSmartKeysWorkingList;

		// Token: 0x04004EE7 RID: 20199
		private List<ByteGrid> avoidGridsSmartValuesWorkingList;

		// Token: 0x04004EE8 RID: 20200
		private static List<PawnKindDef> allPawnKinds = new List<PawnKindDef>();

		// Token: 0x04004EE9 RID: 20201
		public const int RoyalThingUseViolationGoodwillImpact = 4;
	}
}
