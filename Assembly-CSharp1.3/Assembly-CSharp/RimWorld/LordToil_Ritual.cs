using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008CD RID: 2253
	public class LordToil_Ritual : LordToil_Gathering
	{
		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003B47 RID: 15175 RVA: 0x0014B1D1 File Offset: 0x001493D1
		public List<LocalTargetInfo> ReservedThings
		{
			get
			{
				return this.reservedThings;
			}
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x0014B1D9 File Offset: 0x001493D9
		public LordToil_Ritual(IntVec3 spot, LordJob_Ritual ritual, RitualStage stage, Pawn organizer) : base(spot, null)
		{
			this.stage = stage;
			this.organizer = organizer;
			this.ritual = ritual;
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x0014B20F File Offset: 0x0014940F
		public override void Init()
		{
			base.Init();
			Action action = this.startAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x000126F5 File Offset: 0x000108F5
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.HighPriority;
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x0014B227 File Offset: 0x00149427
		public override void LordToilTick()
		{
			base.LordToilTick();
			Action action = this.tickAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x0014B240 File Offset: 0x00149440
		public override void UpdateAllDuties()
		{
			this.reservedThings.Clear();
			this.cachedDuties.Clear();
			LocalTargetInfo localTargetInfo = (LocalTargetInfo)this.ritual.selectedTarget;
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				DutyDef dutyDef = this.stage.defaultDuty;
				IntVec3 intVec2 = this.spot;
				LocalTargetInfo secondFocus = LocalTargetInfo.Invalid;
				Thing usedThing = null;
				Rot4 overrideFacing = Rot4.Invalid;
				if (this.ritual.assignments.PawnParticipating(pawn))
				{
					dutyDef = (this.stage.GetDuty(pawn, null, this.ritual) ?? dutyDef);
					PawnStagePosition pawnStagePosition = this.ritual.PawnPositionForStage(pawn, this.stage);
					intVec2 = pawnStagePosition.cell;
					usedThing = pawnStagePosition.thing;
					overrideFacing = pawnStagePosition.orientation;
					TargetInfo targetInfo = this.ritual.SecondFocusForStage(this.stage, pawn);
					secondFocus = ((targetInfo.Thing != null) ? new LocalTargetInfo(targetInfo.Thing) : targetInfo.Cell);
				}
				this.cachedDuties.Add(new CachedPawnRitualDuty
				{
					duty = dutyDef,
					spot = intVec2,
					usedThing = usedThing,
					overrideFacing = overrideFacing,
					secondFocus = secondFocus
				});
				if (dutyDef.ritualSpectateTarget)
				{
					intVec = intVec2;
				}
			}
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn2 = this.lord.ownedPawns[j];
				CachedPawnRitualDuty cachedPawnRitualDuty = this.cachedDuties[j];
				IntVec3 spot = cachedPawnRitualDuty.spot;
				LocalTargetInfo secondFocus2 = cachedPawnRitualDuty.secondFocus;
				Rot4 overrideFacing2 = cachedPawnRitualDuty.overrideFacing;
				Thing usedThing2 = cachedPawnRitualDuty.usedThing;
				PawnDuty pawnDuty = new PawnDuty(cachedPawnRitualDuty.duty, spot, secondFocus2, localTargetInfo, -1f);
				pawnDuty.spectateRect = CellRect.CenteredOn(this.spot, 0);
				Thing thing = localTargetInfo.Thing;
				RitualFocusProperties ritualFocusProperties = (thing != null) ? thing.def.ritualFocus : null;
				if (ritualFocusProperties != null)
				{
					pawnDuty.spectateRectAllowedSides = ritualFocusProperties.allowedSpectateSides;
					pawnDuty.spectateDistance = ritualFocusProperties.spectateDistance;
				}
				else
				{
					pawnDuty.spectateRectAllowedSides = SpectateRectSide.Horizontal;
					pawnDuty.spectateDistance = new IntRange(2, 2);
				}
				if (this.stage.allowedSpectateSidesOverride != SpectateRectSide.None)
				{
					pawnDuty.spectateRectAllowedSides = this.stage.allowedSpectateSidesOverride;
				}
				if (this.stage.spectateDistanceOverride != IntRange.zero)
				{
					pawnDuty.spectateDistance = this.stage.spectateDistanceOverride;
				}
				if (thing != null)
				{
					pawnDuty.spectateRectAllowedSides = pawnDuty.spectateRectAllowedSides.Rotated(thing.Rotation);
					pawnDuty.spectateRect = thing.OccupiedRect();
				}
				if (intVec.IsValid && intVec != spot)
				{
					pawnDuty.spectateRect = CellRect.CenteredOn(intVec, 0);
				}
				if (cachedPawnRitualDuty.overrideFacing.IsValid)
				{
					pawnDuty.spectateRect = CellRect.CenteredOn(spot + overrideFacing2.FacingCell, 0);
					pawnDuty.overrideFacing = cachedPawnRitualDuty.overrideFacing;
				}
				if (pawnDuty.spectateRectAllowedSides == SpectateRectSide.Horizontal)
				{
					pawnDuty.spectateRectPreferredSide = ((intVec.x < spot.x) ? SpectateRectSide.Right : SpectateRectSide.Left);
				}
				else if (pawnDuty.spectateRectAllowedSides == SpectateRectSide.Vertical)
				{
					pawnDuty.spectateRectPreferredSide = ((intVec.y < spot.y) ? SpectateRectSide.Down : SpectateRectSide.Up);
				}
				if (spot.IsValid && !this.reservedThings.Contains(spot))
				{
					this.reservedThings.Add(spot);
				}
				if (secondFocus2.IsValid && !this.reservedThings.Contains(secondFocus2))
				{
					this.reservedThings.Add(secondFocus2);
				}
				if (localTargetInfo.IsValid && !this.reservedThings.Contains(localTargetInfo))
				{
					this.reservedThings.Add(localTargetInfo);
				}
				if (!this.reservedThings.Contains(pawn2))
				{
					this.reservedThings.Add(pawn2);
				}
				foreach (Pawn pawn3 in this.ritual.pawnsDeathIgnored)
				{
					if (!this.reservedThings.Contains(pawn3.Corpse))
					{
						this.reservedThings.Add(pawn3.Corpse);
					}
				}
				if (usedThing2 != null && !this.ritual.usedThings.Contains(usedThing2))
				{
					this.ritual.usedThings.Add(usedThing2);
				}
				pawn2.mindState.duty = pawnDuty;
				pawn2.jobs.CheckForJobOverride();
			}
		}

		// Token: 0x0400204B RID: 8267
		public Pawn organizer;

		// Token: 0x0400204C RID: 8268
		public LordJob_Ritual ritual;

		// Token: 0x0400204D RID: 8269
		public RitualStage stage;

		// Token: 0x0400204E RID: 8270
		public Action startAction;

		// Token: 0x0400204F RID: 8271
		public Action tickAction;

		// Token: 0x04002050 RID: 8272
		protected List<LocalTargetInfo> reservedThings = new List<LocalTargetInfo>();

		// Token: 0x04002051 RID: 8273
		private List<CachedPawnRitualDuty> cachedDuties = new List<CachedPawnRitualDuty>();
	}
}
