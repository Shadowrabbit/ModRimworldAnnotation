using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E74 RID: 3700
	[StaticConstructorOnStartup]
	public class Pawn_RopeTracker : IExposable
	{
		// Token: 0x0600562D RID: 22061 RVA: 0x001D351F File Offset: 0x001D171F
		public Pawn_RopeTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x0600562E RID: 22062 RVA: 0x001D3544 File Offset: 0x001D1744
		public bool IsRoped
		{
			get
			{
				return this.ropedTo.IsValid;
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x0600562F RID: 22063 RVA: 0x001D3551 File Offset: 0x001D1751
		public bool IsRopedByPawn
		{
			get
			{
				return this.RopedByPawn != null;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06005630 RID: 22064 RVA: 0x001D355C File Offset: 0x001D175C
		public bool IsRopedToSpot
		{
			get
			{
				return this.ropedTo.IsValid && !this.IsRopedByPawn;
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06005631 RID: 22065 RVA: 0x001D3576 File Offset: 0x001D1776
		public bool IsRopingOthers
		{
			get
			{
				return this.Ropees.Any<Pawn>();
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06005632 RID: 22066 RVA: 0x001D3583 File Offset: 0x001D1783
		public bool HasAnyRope
		{
			get
			{
				return this.IsRoped || this.IsRopingOthers;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06005633 RID: 22067 RVA: 0x001D3595 File Offset: 0x001D1795
		public Pawn RopedByPawn
		{
			get
			{
				return this.ropedTo.Thing as Pawn;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06005634 RID: 22068 RVA: 0x001D35A7 File Offset: 0x001D17A7
		public IntVec3 RopedToSpot
		{
			get
			{
				if (!this.IsRopedToSpot)
				{
					return IntVec3.Invalid;
				}
				return this.ropedTo.Cell;
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06005635 RID: 22069 RVA: 0x001D35C2 File Offset: 0x001D17C2
		public LocalTargetInfo RopedTo
		{
			get
			{
				return this.ropedTo;
			}
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06005636 RID: 22070 RVA: 0x001D35CA File Offset: 0x001D17CA
		public List<Pawn> Ropees
		{
			get
			{
				return this.ropees;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06005637 RID: 22071 RVA: 0x001D35D4 File Offset: 0x001D17D4
		public bool AnyRopeesFenceBlocked
		{
			get
			{
				if (this.ropees.Count == 0)
				{
					return false;
				}
				for (int i = 0; i < this.ropees.Count; i++)
				{
					if (this.ropees[i].def.race.FenceBlocked)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06005638 RID: 22072 RVA: 0x001D3628 File Offset: 0x001D1828
		public string InspectLine
		{
			get
			{
				if (this.IsRopedByPawn)
				{
					return "RopedByPawn".Translate() + ": " + this.RopedByPawn.LabelShort;
				}
				if (this.ropedTo.HasThing)
				{
					return "RopedToThing".Translate() + ": " + this.ropedTo.Label;
				}
				return "RopedToSpot".Translate();
			}
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x001D36B0 File Offset: 0x001D18B0
		public void RopingTick()
		{
			if (!this.HasAnyRope)
			{
				return;
			}
			if (this.pawn.Dead || this.pawn.Downed || this.pawn.Drafted || !this.pawn.Awake() || this.ShouldDropRopesDueToMentalState() || this.pawn.IsBurning())
			{
				this.BreakAllRopes();
				return;
			}
			if (this.ropees.Any<Pawn>() && !this.IsStillDoingRopingJob(this.pawn))
			{
				this.BreakAllRopes();
				return;
			}
			if (this.ropedTo.IsValid && !this.pawn.CanReach(this.ropedTo, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				this.BreakRopeWithRoper();
			}
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x001D3763 File Offset: 0x001D1963
		private bool IsStillDoingRopingJob(Pawn roper)
		{
			return roper.jobs.curJob == null || roper.jobs.curDriver is JobDriver_RopeToDestination;
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x001D3787 File Offset: 0x001D1987
		private bool ShouldDropRopesDueToMentalState()
		{
			return this.pawn.InMentalState && this.pawn.MentalStateDef != MentalStateDefOf.Roaming;
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x001D37B0 File Offset: 0x001D19B0
		public void RopingDraw()
		{
			if (!this.ropedTo.IsValid)
			{
				return;
			}
			Vector3 b = this.ropedTo.CenterVector3.Yto0();
			GenDraw.DrawLineBetween(this.pawn.DrawPos.Yto0(), b, AltitudeLayer.PawnRope.AltitudeFor(), Pawn_RopeTracker.RopeLineMat, 0.2f);
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x001D3803 File Offset: 0x001D1A03
		public void Notify_DeSpawned()
		{
			this.BreakAllRopes();
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x001D380B File Offset: 0x001D1A0B
		private void BreakRopeWithRoper()
		{
			Pawn ropedByPawn = this.RopedByPawn;
			if (ropedByPawn != null)
			{
				ropedByPawn.roping.DropRope(this.pawn);
			}
			this.ropedTo = LocalTargetInfo.Invalid;
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x001D3834 File Offset: 0x001D1A34
		private void BreakAllRopes()
		{
			this.BreakRopeWithRoper();
			this.DropRopes();
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x001D3842 File Offset: 0x001D1A42
		public void RopeToSpot(IntVec3 spot)
		{
			Pawn_RopeTracker.CreateRope(spot, this.pawn);
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x001D3855 File Offset: 0x001D1A55
		public void RopePawn(Pawn ropee)
		{
			Pawn_RopeTracker.CreateRope(this.pawn, ropee);
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x001D3868 File Offset: 0x001D1A68
		private static void CreateRope(LocalTargetInfo roperTarget, Pawn ropee)
		{
			Pawn pawn = roperTarget.Thing as Pawn;
			ropee.roping.DropRopes();
			ropee.roping.ropedTo = roperTarget;
			if (pawn != null)
			{
				pawn.roping.ropees.Add(ropee);
				ReachabilityUtility.ClearCacheFor(pawn);
			}
			ReachabilityUtility.ClearCacheFor(ropee);
			if (ropee.jobs != null && ropee.CurJob != null)
			{
				ropee.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x001D38D7 File Offset: 0x001D1AD7
		public void UnropeFromSpot()
		{
			if (!this.IsRopedToSpot)
			{
				Log.Warning(string.Format("Tried to unrope {0} from spot, but not roped to a spot", this.pawn));
				return;
			}
			this.ropedTo = LocalTargetInfo.Invalid;
			ReachabilityUtility.ClearCacheFor(this.pawn);
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x001D3910 File Offset: 0x001D1B10
		public void DropRope(Pawn ropee)
		{
			if (this.pawn != ropee.roping.RopedByPawn)
			{
				Log.Warning(string.Format("{0} tried to drop for {1} but ropee holder was {2}", this.pawn, ropee, ropee.roping.RopedByPawn));
				return;
			}
			ropee.roping.ropedTo = LocalTargetInfo.Invalid;
			this.ropees.Remove(ropee);
			ReachabilityUtility.ClearCacheFor(ropee);
			ReachabilityUtility.ClearCacheFor(this.pawn);
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x001D3980 File Offset: 0x001D1B80
		public void DropRopes()
		{
			if (this.ropees.Count == 0)
			{
				return;
			}
			foreach (Pawn ropee in new List<Pawn>(this.ropees))
			{
				this.DropRope(ropee);
			}
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x001D39E8 File Offset: 0x001D1BE8
		public void ExposeData()
		{
			Scribe_TargetInfo.Look(ref this.ropedTo, "ropedTo", LocalTargetInfo.Invalid);
			Scribe_Collections.Look<Pawn>(ref this.ropees, "ropees", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.ropees.RemoveAll((Pawn x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x001D3A54 File Offset: 0x001D1C54
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("{0}", this.pawn));
			if (this.ropedTo.HasThing)
			{
				stringBuilder.Append(string.Format(" ropedBy: {0}", this.ropedTo.Thing));
			}
			else if (this.ropedTo.IsValid)
			{
				stringBuilder.Append(string.Format(" roped at: {0}", this.ropedTo.Cell));
			}
			stringBuilder.AppendLine();
			foreach (Pawn arg in this.Ropees)
			{
				stringBuilder.AppendLine(string.Format("  ropee: {0}", arg));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040032F9 RID: 13049
		public const float RopeLength = 8f;

		// Token: 0x040032FA RID: 13050
		private static readonly string RopeTexPath = "UI/Overlays/Rope";

		// Token: 0x040032FB RID: 13051
		private static readonly Material RopeLineMat = MaterialPool.MatFrom(Pawn_RopeTracker.RopeTexPath, ShaderDatabase.Transparent, GenColor.FromBytes(99, 70, 41, 255));

		// Token: 0x040032FC RID: 13052
		private Pawn pawn;

		// Token: 0x040032FD RID: 13053
		private LocalTargetInfo ropedTo = LocalTargetInfo.Invalid;

		// Token: 0x040032FE RID: 13054
		private List<Pawn> ropees = new List<Pawn>();
	}
}
