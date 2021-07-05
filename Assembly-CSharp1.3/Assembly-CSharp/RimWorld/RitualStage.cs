using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000F1A RID: 3866
	public class RitualStage : IExposable
	{
		// Token: 0x06005C03 RID: 23555 RVA: 0x001FC560 File Offset: 0x001FA760
		public RitualRoleBehavior BehaviorForRole(string roleId)
		{
			RitualRoleBehavior result;
			if (!this.behaviorForRole.TryGetValue(roleId, out result) && this.roleBehaviors != null)
			{
				for (int i = 0; i < this.roleBehaviors.Count; i++)
				{
					RitualRoleBehavior ritualRoleBehavior = this.roleBehaviors[i];
					if (ritualRoleBehavior.roleId == roleId)
					{
						this.behaviorForRole[roleId] = ritualRoleBehavior;
						result = ritualRoleBehavior;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x001FC5C8 File Offset: 0x001FA7C8
		public DutyDef GetDuty(Pawn pawn, RitualRole forcedRole = null, LordJob_Ritual ritual = null)
		{
			if (this.parent.roles != null)
			{
				if (forcedRole != null)
				{
					RitualRoleBehavior ritualRoleBehavior = this.BehaviorForRole(forcedRole.id);
					if (ritualRoleBehavior == null)
					{
						return null;
					}
					return ritualRoleBehavior.dutyDef;
				}
				else
				{
					RitualRole ritualRole = ritual.assignments.RoleForPawn(pawn, true);
					if (ritualRole != null)
					{
						RitualRoleBehavior ritualRoleBehavior2 = this.BehaviorForRole(ritualRole.id);
						if (ritualRoleBehavior2 != null)
						{
							return ritualRoleBehavior2.dutyDef;
						}
					}
				}
			}
			return this.defaultDuty;
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x001FC62C File Offset: 0x001FA82C
		public bool HasRole(Pawn p)
		{
			List<RitualRole> roles = this.parent.roles;
			for (int i = 0; i < roles.Count; i++)
			{
				string text;
				if (roles[i].AppliesToPawn(p, out text, null, null, null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x001FC670 File Offset: 0x001FA870
		public bool Applies(Precept_Ritual ritual, List<Pawn> participants)
		{
			foreach (RitualRole ritualRole in this.parent.RequiredRoles())
			{
				bool flag = false;
				Precept_Role precept_Role = (Precept_Role)Faction.OfPlayer.ideos.GetPrecept(ritualRole.precept);
				if (precept_Role == null)
				{
					return false;
				}
				foreach (Pawn p in participants)
				{
					if (precept_Role.Active && precept_Role.IsAssigned(p))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x001FC73C File Offset: 0x001FA93C
		public PawnStagePosition GetPawnPosition(IntVec3 spot, Pawn pawn, LordJob_Ritual ritual, RitualRole forcedRole = null)
		{
			if (this.parent.roles != null)
			{
				if (forcedRole != null)
				{
					RitualRoleBehavior ritualRoleBehavior = this.BehaviorForRole(forcedRole.id);
					RitualPosition ritualPosition = (ritualRoleBehavior != null) ? ritualRoleBehavior.GetPosition(spot, pawn, ritual) : null;
					if (ritualPosition != null)
					{
						return ritualPosition.GetCell(spot, pawn, ritual);
					}
				}
				RitualRole ritualRole = ritual.assignments.RoleForPawn(pawn, true);
				if (ritualRole != null)
				{
					RitualRoleBehavior ritualRoleBehavior2 = this.BehaviorForRole(ritualRole.id);
					if (ritualRoleBehavior2 != null)
					{
						RitualPosition position = ritualRoleBehavior2.GetPosition(spot, pawn, ritual);
						if (position != null)
						{
							return position.GetCell(spot, pawn, ritual);
						}
					}
				}
			}
			return new PawnStagePosition(spot, null, Rot4.Invalid, false);
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x0001F15E File Offset: 0x0001D35E
		public virtual float ProgressPerTick(LordJob_Ritual ritual)
		{
			return 1f;
		}

		// Token: 0x06005C09 RID: 23561 RVA: 0x001FC7C9 File Offset: 0x001FA9C9
		public virtual TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return TargetInfo.Invalid;
		}

		// Token: 0x06005C0A RID: 23562 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<RitualStagePawnSecondFocus> GetPawnSecondFoci(LordJob_Ritual ritual)
		{
			return null;
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x001FC7D0 File Offset: 0x001FA9D0
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualBehaviorDef>(ref this.parent, "parent");
			Scribe_Defs.Look<DutyDef>(ref this.defaultDuty, "defaultDuty");
			Scribe_Collections.Look<StageEndTrigger>(ref this.endTriggers, "endTriggers", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.spectatorsRequired, "spectatorsRequired", false, false);
			Scribe_Values.Look<SpectateRectSide>(ref this.allowedSpectateSidesOverride, "allowedSpectateSidesOverride", SpectateRectSide.None, false);
			Scribe_Values.Look<IntRange>(ref this.spectateDistanceOverride, "spectateDistanceOverride", default(IntRange), false);
			Scribe_Collections.Look<StageFailTrigger>(ref this.failTriggers, "failTriggers", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualRoleBehavior>(ref this.roleBehaviors, "roleBehaviors", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.highlightRolePositions, "highlightRolePositions", LookMode.Value, Array.Empty<object>());
			Scribe_Defs.Look<RitualVisualEffectDef>(ref this.visualEffectDef, "visualEffectDef");
			Scribe_Values.Look<bool>(ref this.ignoreDurationToFinishAfterStage, "ignoreDurationToFinishAfterStage", false, false);
			Scribe_Deep.Look<RitualStageAction>(ref this.postAction, "postAction", Array.Empty<object>());
			Scribe_Deep.Look<RitualStageAction>(ref this.preAction, "preAction", Array.Empty<object>());
			Scribe_Deep.Look<RitualStageAction>(ref this.interruptedAction, "interruptedAction", Array.Empty<object>());
			Scribe_Deep.Look<RitualStageAction>(ref this.pawnLeaveAction, "pawnLeaveAction", Array.Empty<object>());
		}

		// Token: 0x04003594 RID: 13716
		public RitualBehaviorDef parent;

		// Token: 0x04003595 RID: 13717
		public DutyDef defaultDuty;

		// Token: 0x04003596 RID: 13718
		public List<StageEndTrigger> endTriggers;

		// Token: 0x04003597 RID: 13719
		public List<StageFailTrigger> failTriggers;

		// Token: 0x04003598 RID: 13720
		public RitualStageAction preAction;

		// Token: 0x04003599 RID: 13721
		public RitualStageAction postAction;

		// Token: 0x0400359A RID: 13722
		public RitualStageAction interruptedAction;

		// Token: 0x0400359B RID: 13723
		public RitualStageAction pawnLeaveAction;

		// Token: 0x0400359C RID: 13724
		[NoTranslate]
		public List<string> highlightRolePositions = new List<string>();

		// Token: 0x0400359D RID: 13725
		[NoTranslate]
		public List<string> highlightRolePawns = new List<string>();

		// Token: 0x0400359E RID: 13726
		public RitualStageTickActionMaker tickActionMaker;

		// Token: 0x0400359F RID: 13727
		public RitualVisualEffectDef visualEffectDef;

		// Token: 0x040035A0 RID: 13728
		public bool spectatorsRequired;

		// Token: 0x040035A1 RID: 13729
		public bool essential;

		// Token: 0x040035A2 RID: 13730
		public bool ignoreDurationToFinishAfterStage;

		// Token: 0x040035A3 RID: 13731
		public SpectateRectSide allowedSpectateSidesOverride;

		// Token: 0x040035A4 RID: 13732
		public IntRange spectateDistanceOverride = IntRange.zero;

		// Token: 0x040035A5 RID: 13733
		public List<RitualRoleBehavior> roleBehaviors;

		// Token: 0x040035A6 RID: 13734
		private Dictionary<string, RitualRoleBehavior> behaviorForRole = new Dictionary<string, RitualRoleBehavior>();
	}
}
