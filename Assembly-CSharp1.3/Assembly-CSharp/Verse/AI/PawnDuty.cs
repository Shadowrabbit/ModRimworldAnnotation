using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200060A RID: 1546
	public class PawnDuty : IExposable
	{
		// Token: 0x06002C86 RID: 11398 RVA: 0x00109EC0 File Offset: 0x001080C0
		public PawnDuty()
		{
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x00109F28 File Offset: 0x00108128
		public PawnDuty(DutyDef def)
		{
			this.def = def;
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x00109F95 File Offset: 0x00108195
		public PawnDuty(DutyDef def, LocalTargetInfo focus, float radius = -1f) : this(def)
		{
			this.focus = focus;
			this.radius = radius;
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x00109FAC File Offset: 0x001081AC
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x00109FBF File Offset: 0x001081BF
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, LocalTargetInfo focusThird, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
			this.focusThird = focusThird;
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x00109FDC File Offset: 0x001081DC
		public void ExposeData()
		{
			Scribe_Defs.Look<DutyDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.focus, "focus", LocalTargetInfo.Invalid);
			Scribe_TargetInfo.Look(ref this.focusSecond, "focusSecond", LocalTargetInfo.Invalid);
			Scribe_TargetInfo.Look(ref this.focusThird, "focusThird", LocalTargetInfo.Invalid);
			Scribe_Values.Look<float>(ref this.radius, "radius", -1f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<Danger>(ref this.maxDanger, "maxDanger", Danger.Unspecified, false);
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.All, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", -1, false);
			Scribe_Values.Look<bool>(ref this.attackDownedIfStarving, "attackDownedIfStarving", false, false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<IntRange>(ref this.spectateDistance, "spectateDistance", default(IntRange), false);
			Scribe_Values.Look<int?>(ref this.ropeeLimit, "ropeeLimit", null, false);
			Scribe_Values.Look<bool>(ref this.pickupOpportunisticWeapon, "pickupOpportunisticWeapon", false, false);
			Scribe_Values.Look<Rot4>(ref this.overrideFacing, "overrideFacing", Rot4.Invalid, false);
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x0010A14C File Offset: 0x0010834C
		public override string ToString()
		{
			string text = this.focus.IsValid ? this.focus.ToString() : "";
			string text2 = this.focusSecond.IsValid ? (", second=" + this.focusSecond.ToString()) : "";
			string text3 = this.focusThird.IsValid ? (", third=" + this.focusThird.ToString()) : "";
			string text4 = (this.radius > 0f) ? (", rad=" + this.radius.ToString("F2")) : "";
			return string.Concat(new object[]
			{
				"(",
				this.def,
				" ",
				text,
				text2,
				text3,
				text4,
				")"
			});
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x0010A24C File Offset: 0x0010844C
		internal void DrawDebug(Pawn pawn)
		{
			if (this.focus.IsValid)
			{
				GenDraw.DrawLineBetween(pawn.DrawPos, this.focus.Cell.ToVector3Shifted());
				if (this.radius > 0f)
				{
					GenDraw.DrawRadiusRing(this.focus.Cell, this.radius);
				}
			}
		}

		// Token: 0x04001B30 RID: 6960
		public DutyDef def;

		// Token: 0x04001B31 RID: 6961
		public LocalTargetInfo focus = LocalTargetInfo.Invalid;

		// Token: 0x04001B32 RID: 6962
		public LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;

		// Token: 0x04001B33 RID: 6963
		public LocalTargetInfo focusThird = LocalTargetInfo.Invalid;

		// Token: 0x04001B34 RID: 6964
		public float radius = -1f;

		// Token: 0x04001B35 RID: 6965
		public LocomotionUrgency locomotion;

		// Token: 0x04001B36 RID: 6966
		public Danger maxDanger;

		// Token: 0x04001B37 RID: 6967
		public CellRect spectateRect;

		// Token: 0x04001B38 RID: 6968
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x04001B39 RID: 6969
		public SpectateRectSide spectateRectPreferredSide;

		// Token: 0x04001B3A RID: 6970
		public IntRange spectateDistance = new IntRange(2, 5);

		// Token: 0x04001B3B RID: 6971
		public bool canDig;

		// Token: 0x04001B3C RID: 6972
		public int transportersGroup = -1;

		// Token: 0x04001B3D RID: 6973
		public bool attackDownedIfStarving;

		// Token: 0x04001B3E RID: 6974
		public float? wanderRadius;

		// Token: 0x04001B3F RID: 6975
		public int? ropeeLimit;

		// Token: 0x04001B40 RID: 6976
		public bool pickupOpportunisticWeapon;

		// Token: 0x04001B41 RID: 6977
		public Rot4 overrideFacing = Rot4.Invalid;
	}
}
