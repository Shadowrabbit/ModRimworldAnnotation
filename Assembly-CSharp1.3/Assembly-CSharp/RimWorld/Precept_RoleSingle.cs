using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCF RID: 4047
	public class Precept_RoleSingle : Precept_Role
	{
		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x06005F63 RID: 24419 RVA: 0x0020A500 File Offset: 0x00208700
		public Pawn ChosenPawnValue
		{
			get
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
				if (ideoRoleInstance == null)
				{
					return null;
				}
				return ideoRoleInstance.pawn;
			}
		}

		// Token: 0x06005F64 RID: 24420 RVA: 0x0020A513 File Offset: 0x00208713
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			this.chosenPawn = new IdeoRoleInstance(this);
			base.Init(ideo, generatingFor);
		}

		// Token: 0x06005F65 RID: 24421 RVA: 0x0020A529 File Offset: 0x00208729
		public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			base.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			if (p == this.ChosenPawnValue && oldFaction.IsPlayer)
			{
				this.Assign(null, false);
			}
		}

		// Token: 0x06005F66 RID: 24422 RVA: 0x0020A54D File Offset: 0x0020874D
		public override IEnumerable<Pawn> ChosenPawns()
		{
			if (this.ChosenPawnValue != null)
			{
				yield return this.chosenPawn.pawn;
			}
			yield break;
		}

		// Token: 0x06005F67 RID: 24423 RVA: 0x0020A560 File Offset: 0x00208760
		public override void RecacheActivity()
		{
			int colonistBelieverCountCached = this.ideo.ColonistBelieverCountCached;
			bool flag = Faction.OfPlayer.ideos.Has(this.ideo);
			if (this.active && colonistBelieverCountCached <= this.def.deactivationBelieverCount && !this.def.leaderRole)
			{
				this.active = false;
				if (flag)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRoleInactive".Translate(this.name).CapitalizeFirst(), "LetterLabelRoleInactiveDesc".Translate(this.ideo.memberName, this.def.deactivationBelieverCount, this.def.activationBelieverCount, this.Named("ROLE")).CapitalizeFirst(), LetterDefOf.NeutralEvent, null);
				}
				if (this.ChosenPawnValue != null)
				{
					if (flag)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRoleLost".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")), "LetterRoleLostDesc".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")) + " " + "LetterRoleLostReasonLowBelieversDesc".Translate(this.ideo.memberName).CapitalizeFirst(), LetterDefOf.NeutralEvent, this.ChosenPawnValue, null, null, null, null);
					}
					base.Notify_PawnUnassigned(this.ChosenPawnValue);
					this.chosenPawn.pawn = null;
				}
			}
			if (!this.active && (colonistBelieverCountCached >= this.def.activationBelieverCount || this.def.leaderRole))
			{
				this.active = true;
				if (flag && !this.def.leaderRole && this.def.activationBelieverCount > 0 && Find.TickManager.TicksGame > 1)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRoleActive".Translate(this.name).CapitalizeFirst(), "LetterLabelRoleActiveDesc".Translate(this.ideo.memberName, this.def.activationBelieverCount, this.Named("ROLE")).CapitalizeFirst(), LetterDefOf.NeutralEvent, null);
				}
			}
			if (this.ChosenPawnValue != null && !base.ValidatePawn(this.ChosenPawnValue))
			{
				base.Notify_PawnUnassigned(this.ChosenPawnValue);
				this.chosenPawn.pawn = null;
			}
		}

		// Token: 0x06005F68 RID: 24424 RVA: 0x0020A7FC File Offset: 0x002089FC
		public override void Assign(Pawn p, bool addThoughts)
		{
			if (p != this.ChosenPawnValue)
			{
				if (p != null && !base.ValidatePawn(p))
				{
					Log.Error("Invalid pawn assigned to " + base.LabelCap + " role. pawn=" + p.GetUniqueLoadID());
				}
				if (this.ChosenPawnValue != null && addThoughts)
				{
					if (p != null)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRoleLost".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")), "LetterRoleLostDesc".Translate(this.ChosenPawnValue.Named("PAWN"), this.Named("ROLE")) + " " + "LetterRoleLostReasonUnassignedDesc".Translate(this.ChosenPawnValue.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NeutralEvent, this.ChosenPawnValue, null, null, null, null);
					}
					this.ChosenPawnValue.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleLost, this), null);
				}
				Pawn chosenPawnValue = this.ChosenPawnValue;
				this.chosenPawn.pawn = p;
				base.Notify_PawnUnassigned(chosenPawnValue);
				base.Notify_PawnAssigned(p);
				if (this.def.leaderRole && Current.ProgramState == ProgramState.Playing)
				{
					Faction ofPlayer = Faction.OfPlayer;
					if (ofPlayer != null)
					{
						ofPlayer.leader = p;
						foreach (Ideo ideo in ofPlayer.ideos.AllIdeos)
						{
							foreach (Precept precept in ideo.PreceptsListForReading)
							{
								Precept_Role precept_Role;
								if (precept != this && (precept_Role = (precept as Precept_Role)) != null && precept_Role.def.leaderRole)
								{
									precept_Role.Assign(null, true);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06005F69 RID: 24425 RVA: 0x0020AA0C File Offset: 0x00208C0C
		public override void FillOrUpdateAbilities()
		{
			if (!this.def.grantedAbilities.NullOrEmpty<AbilityDef>())
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
				Pawn chosenPawnValue = this.ChosenPawnValue;
				IdeoRoleInstance ideoRoleInstance2 = this.chosenPawn;
				ideoRoleInstance.abilities = base.FillOrUpdateAbilityList(chosenPawnValue, (ideoRoleInstance2 != null) ? ideoRoleInstance2.abilities : null);
			}
		}

		// Token: 0x06005F6A RID: 24426 RVA: 0x0020AA49 File Offset: 0x00208C49
		public override List<Ability> AbilitiesFor(Pawn p)
		{
			return this.chosenPawn.abilities;
		}

		// Token: 0x06005F6B RID: 24427 RVA: 0x0020A500 File Offset: 0x00208700
		public override Pawn ChosenPawnSingle()
		{
			IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
			if (ideoRoleInstance == null)
			{
				return null;
			}
			return ideoRoleInstance.pawn;
		}

		// Token: 0x06005F6C RID: 24428 RVA: 0x0020AA56 File Offset: 0x00208C56
		public override bool IsAssigned(Pawn p)
		{
			IdeoRoleInstance ideoRoleInstance = this.chosenPawn;
			return ((ideoRoleInstance != null) ? ideoRoleInstance.pawn : null) == p;
		}

		// Token: 0x06005F6D RID: 24429 RVA: 0x0020AA6D File Offset: 0x00208C6D
		public override void Unassign(Pawn p, bool generateThoughts)
		{
			this.Assign(null, generateThoughts);
		}

		// Token: 0x06005F6E RID: 24430 RVA: 0x0020AA78 File Offset: 0x00208C78
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<IdeoRoleInstance>(ref this.chosenPawn, "chosenPawn", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ChosenPawnValue != null && !base.ValidatePawn(this.ChosenPawnValue))
				{
					Pawn pawn = this.chosenPawn.pawn;
					this.chosenPawn.pawn = null;
					base.Notify_PawnUnassigned(pawn);
				}
				this.chosenPawn.sourceRole = this;
				this.FillOrUpdateAbilities();
			}
		}

		// Token: 0x040036DC RID: 14044
		public IdeoRoleInstance chosenPawn;
	}
}
