using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E81 RID: 3713
	public class Pawn_PlayerSettings : IExposable
	{
		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x060056E7 RID: 22247 RVA: 0x001D7E3F File Offset: 0x001D603F
		// (set) Token: 0x060056E8 RID: 22248 RVA: 0x001D7E48 File Offset: 0x001D6048
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
					Log.ErrorOnce("Attempted to set master for non-obedient pawn", 73908573);
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

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x060056E9 RID: 22249 RVA: 0x001D7ECC File Offset: 0x001D60CC
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

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x060056EA RID: 22250 RVA: 0x001D7EF6 File Offset: 0x001D60F6
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

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x060056EB RID: 22251 RVA: 0x001D7F08 File Offset: 0x001D6108
		// (set) Token: 0x060056EC RID: 22252 RVA: 0x001D7F10 File Offset: 0x001D6110
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

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x060056ED RID: 22253 RVA: 0x001D7FA7 File Offset: 0x001D61A7
		public bool RespectsAllowedArea
		{
			get
			{
				return this.SupportsAllowedAreas && this.pawn.GetLord() == null && this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x060056EE RID: 22254 RVA: 0x001D7FE4 File Offset: 0x001D61E4
		public bool SupportsAllowedAreas
		{
			get
			{
				return !this.pawn.RaceProps.Roamer;
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x060056EF RID: 22255 RVA: 0x001D7FF9 File Offset: 0x001D61F9
		public bool RespectsMaster
		{
			get
			{
				return this.Master != null && this.pawn.Faction == Faction.OfPlayer && this.Master.Faction == this.pawn.Faction;
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x060056F0 RID: 22256 RVA: 0x001D8031 File Offset: 0x001D6231
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

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x060056F1 RID: 22257 RVA: 0x001D8043 File Offset: 0x001D6243
		public bool UsesConfigurableHostilityResponse
		{
			get
			{
				return this.pawn.IsColonist && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x001D8064 File Offset: 0x001D6264
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

		// Token: 0x060056F3 RID: 22259 RVA: 0x001D80BC File Offset: 0x001D62BC
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
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pawn.def.race != null && this.pawn.def.race.Roamer)
			{
				this.areaAllowedInt = null;
			}
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x001D81B3 File Offset: 0x001D63B3
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

		// Token: 0x060056F5 RID: 22261 RVA: 0x001D81C3 File Offset: 0x001D63C3
		public void Notify_FactionChanged()
		{
			this.ResetMedicalCare();
			this.areaAllowedInt = null;
		}

		// Token: 0x060056F6 RID: 22262 RVA: 0x001D81D4 File Offset: 0x001D63D4
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
				if (this.pawn.IsPrisoner)
				{
					this.medCare = Find.PlaySettings.defaultCareForColonyPrisoner;
					return;
				}
				if (this.pawn.IsSlave)
				{
					this.medCare = Find.PlaySettings.defaultCareForColonySlave;
					return;
				}
				this.medCare = Find.PlaySettings.defaultCareForColonyHumanlike;
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

		// Token: 0x060056F7 RID: 22263 RVA: 0x001D82E1 File Offset: 0x001D64E1
		public void Notify_AreaRemoved(Area area)
		{
			if (this.areaAllowedInt == area)
			{
				this.areaAllowedInt = null;
			}
		}

		// Token: 0x0400334C RID: 13132
		private Pawn pawn;

		// Token: 0x0400334D RID: 13133
		private Area areaAllowedInt;

		// Token: 0x0400334E RID: 13134
		public int joinTick = -1;

		// Token: 0x0400334F RID: 13135
		private Pawn master;

		// Token: 0x04003350 RID: 13136
		public bool followDrafted;

		// Token: 0x04003351 RID: 13137
		public bool followFieldwork;

		// Token: 0x04003352 RID: 13138
		public bool animalsReleased;

		// Token: 0x04003353 RID: 13139
		public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

		// Token: 0x04003354 RID: 13140
		public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

		// Token: 0x04003355 RID: 13141
		public bool selfTend;

		// Token: 0x04003356 RID: 13142
		public int displayOrder;
	}
}
