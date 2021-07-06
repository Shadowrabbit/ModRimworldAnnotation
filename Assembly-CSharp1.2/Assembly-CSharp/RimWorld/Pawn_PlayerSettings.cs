using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200153D RID: 5437
	public class Pawn_PlayerSettings : IExposable
	{
		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x060075AE RID: 30126 RVA: 0x0004F5CE File Offset: 0x0004D7CE
		// (set) Token: 0x060075AF RID: 30127 RVA: 0x0023D310 File Offset: 0x0023B510
		public Pawn Master
		{
			get
			{
				return this.master;
			}
			set
			{
				if (this.master == value)
				{
					return;
				}
				if (value != null && !this.pawn.training.HasLearned(TrainableDefOf.Obedience))
				{
					Log.ErrorOnce("Attempted to set master for non-obedient pawn", 73908573, false);
					return;
				}
				bool flag = ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn);
				this.master = value;
				if (this.pawn.Spawned && (flag || ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn)))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x060075B0 RID: 30128 RVA: 0x0004F5D6 File Offset: 0x0004D7D6
		public Area EffectiveAreaRestrictionInPawnCurrentMap
		{
			get
			{
				if (this.areaAllowedInt != null && this.areaAllowedInt.Map != this.pawn.MapHeld)
				{
					return null;
				}
				return this.EffectiveAreaRestriction;
			}
		}

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x060075B1 RID: 30129 RVA: 0x0004F600 File Offset: 0x0004D800
		public Area EffectiveAreaRestriction
		{
			get
			{
				if (!this.RespectsAllowedArea)
				{
					return null;
				}
				return this.areaAllowedInt;
			}
		}

		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x060075B2 RID: 30130 RVA: 0x0004F612 File Offset: 0x0004D812
		// (set) Token: 0x060075B3 RID: 30131 RVA: 0x0023D398 File Offset: 0x0023B598
		public Area AreaRestriction
		{
			get
			{
				return this.areaAllowedInt;
			}
			set
			{
				if (this.areaAllowedInt == value)
				{
					return;
				}
				this.areaAllowedInt = value;
				if (this.pawn.Spawned && !this.pawn.Drafted && value != null && value == this.EffectiveAreaRestrictionInPawnCurrentMap && value.TrueCount > 0 && this.pawn.jobs != null && this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.AnyTargetOutsideArea(value))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x060075B4 RID: 30132 RVA: 0x0004F61A File Offset: 0x0004D81A
		public bool RespectsAllowedArea
		{
			get
			{
				return this.pawn.GetLord() == null && this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x060075B5 RID: 30133 RVA: 0x0004F64D File Offset: 0x0004D84D
		public bool RespectsMaster
		{
			get
			{
				return this.Master != null && this.pawn.Faction == Faction.OfPlayer && this.Master.Faction == this.pawn.Faction;
			}
		}

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x060075B6 RID: 30134 RVA: 0x0004F685 File Offset: 0x0004D885
		public Pawn RespectedMaster
		{
			get
			{
				if (!this.RespectsMaster)
				{
					return null;
				}
				return this.Master;
			}
		}

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x060075B7 RID: 30135 RVA: 0x0004F697 File Offset: 0x0004D897
		public bool UsesConfigurableHostilityResponse
		{
			get
			{
				return this.pawn.IsColonist && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x060075B8 RID: 30136 RVA: 0x0023D430 File Offset: 0x0023B630
		public Pawn_PlayerSettings(Pawn pawn)
		{
			this.pawn = pawn;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.joinTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.joinTick = 0;
			}
			this.Notify_FactionChanged();
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x0023D488 File Offset: 0x0023B688
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.joinTick, "joinTick", 0, false);
			Scribe_Values.Look<bool>(ref this.animalsReleased, "animalsReleased", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.medCare, "medCare", MedicalCareCategory.NoCare, false);
			Scribe_References.Look<Area>(ref this.areaAllowedInt, "areaAllowed", false);
			Scribe_References.Look<Pawn>(ref this.master, "master", false);
			Scribe_Values.Look<bool>(ref this.followDrafted, "followDrafted", false, false);
			Scribe_Values.Look<bool>(ref this.followFieldwork, "followFieldwork", false, false);
			Scribe_Values.Look<HostilityResponseMode>(ref this.hostilityResponse, "hostilityResponse", HostilityResponseMode.Flee, false);
			Scribe_Values.Look<bool>(ref this.selfTend, "selfTend", false, false);
			Scribe_Values.Look<int>(ref this.displayOrder, "displayOrder", 0, false);
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x0004F6B6 File Offset: 0x0004D8B6
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.Drafted)
			{
				int num = 0;
				bool flag = false;
				foreach (Pawn pawn in PawnUtility.SpawnedMasteredPawns(this.pawn))
				{
					if (pawn.training.HasLearned(TrainableDefOf.Release))
					{
						flag = true;
						if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
						{
							num++;
						}
					}
				}
				if (flag)
				{
					Command_Toggle command_Toggle = new Command_Toggle();
					command_Toggle.defaultLabel = "CommandReleaseAnimalsLabel".Translate() + ((num != 0) ? (" (" + num + ")") : "");
					command_Toggle.defaultDesc = "CommandReleaseAnimalsDesc".Translate();
					command_Toggle.icon = TexCommand.ReleaseAnimals;
					command_Toggle.hotKey = KeyBindingDefOf.Misc7;
					command_Toggle.isActive = (() => this.animalsReleased);
					command_Toggle.toggleAction = delegate()
					{
						this.animalsReleased = !this.animalsReleased;
						if (this.animalsReleased)
						{
							foreach (Pawn pawn2 in PawnUtility.SpawnedMasteredPawns(this.pawn))
							{
								if (pawn2.caller != null)
								{
									pawn2.caller.Notify_Released();
								}
								pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							}
						}
					};
					if (num == 0)
					{
						command_Toggle.Disable("CommandReleaseAnimalsFail_NoAnimals".Translate());
					}
					yield return command_Toggle;
				}
			}
			yield break;
		}

		// Token: 0x060075BB RID: 30139 RVA: 0x0004F6C6 File Offset: 0x0004D8C6
		public void Notify_FactionChanged()
		{
			this.ResetMedicalCare();
			this.areaAllowedInt = null;
		}

		// Token: 0x060075BC RID: 30140 RVA: 0x0004F6D5 File Offset: 0x0004D8D5
		public void Notify_MadePrisoner()
		{
			this.ResetMedicalCare();
		}

		// Token: 0x060075BD RID: 30141 RVA: 0x0023D548 File Offset: 0x0023B748
		public void ResetMedicalCare()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				return;
			}
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				if (this.pawn.RaceProps.Animal)
				{
					this.medCare = Find.PlaySettings.defaultCareForColonyAnimal;
					return;
				}
				if (!this.pawn.IsPrisoner)
				{
					this.medCare = Find.PlaySettings.defaultCareForColonyHumanlike;
					return;
				}
				this.medCare = Find.PlaySettings.defaultCareForColonyPrisoner;
				return;
			}
			else
			{
				if (this.pawn.Faction == null && this.pawn.RaceProps.Animal)
				{
					this.medCare = Find.PlaySettings.defaultCareForNeutralAnimal;
					return;
				}
				if (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					this.medCare = Find.PlaySettings.defaultCareForNeutralFaction;
					return;
				}
				this.medCare = Find.PlaySettings.defaultCareForHostileFaction;
				return;
			}
		}

		// Token: 0x060075BE RID: 30142 RVA: 0x0004F6DD File Offset: 0x0004D8DD
		public void Notify_AreaRemoved(Area area)
		{
			if (this.areaAllowedInt == area)
			{
				this.areaAllowedInt = null;
			}
		}

		// Token: 0x04004DA1 RID: 19873
		private Pawn pawn;

		// Token: 0x04004DA2 RID: 19874
		private Area areaAllowedInt;

		// Token: 0x04004DA3 RID: 19875
		public int joinTick = -1;

		// Token: 0x04004DA4 RID: 19876
		private Pawn master;

		// Token: 0x04004DA5 RID: 19877
		public bool followDrafted;

		// Token: 0x04004DA6 RID: 19878
		public bool followFieldwork;

		// Token: 0x04004DA7 RID: 19879
		public bool animalsReleased;

		// Token: 0x04004DA8 RID: 19880
		public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

		// Token: 0x04004DA9 RID: 19881
		public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

		// Token: 0x04004DAA RID: 19882
		public bool selfTend;

		// Token: 0x04004DAB RID: 19883
		public int displayOrder;
	}
}
