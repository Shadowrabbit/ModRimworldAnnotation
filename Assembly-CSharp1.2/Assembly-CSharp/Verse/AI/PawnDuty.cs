using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A69 RID: 2665
	public class PawnDuty : IExposable
	{
		// Token: 0x06003FA4 RID: 16292 RVA: 0x0002FB09 File Offset: 0x0002DD09
		public PawnDuty()
		{
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0002FB41 File Offset: 0x0002DD41
		public PawnDuty(DutyDef def)
		{
			this.def = def;
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0002FB80 File Offset: 0x0002DD80
		public PawnDuty(DutyDef def, LocalTargetInfo focus, float radius = -1f) : this(def)
		{
			this.focus = focus;
			this.radius = radius;
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0002FB97 File Offset: 0x0002DD97
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x001803BC File Offset: 0x0017E5BC
		public void ExposeData()
		{
			Scribe_Defs.Look<DutyDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.focus, "focus", LocalTargetInfo.Invalid);
			Scribe_TargetInfo.Look(ref this.focusSecond, "focusSecond", LocalTargetInfo.Invalid);
			Scribe_Values.Look<float>(ref this.radius, "radius", -1f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<Danger>(ref this.maxDanger, "maxDanger", Danger.Unspecified, false);
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.All, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", -1, false);
			Scribe_Values.Look<bool>(ref this.attackDownedIfStarving, "attackDownedIfStarving", false, false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x001804BC File Offset: 0x0017E6BC
		public override string ToString()
		{
			string text = this.focus.IsValid ? this.focus.ToString() : "";
			string text2 = this.focusSecond.IsValid ? (", second=" + this.focusSecond.ToString()) : "";
			string text3 = (this.radius > 0f) ? (", rad=" + this.radius.ToString("F2")) : "";
			return string.Concat(new object[]
			{
				"(",
				this.def,
				" ",
				text,
				text2,
				text3,
				")"
			});
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x00180588 File Offset: 0x0017E788
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

		// Token: 0x04002BFC RID: 11260
		public DutyDef def;

		// Token: 0x04002BFD RID: 11261
		public LocalTargetInfo focus = LocalTargetInfo.Invalid;

		// Token: 0x04002BFE RID: 11262
		public LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;

		// Token: 0x04002BFF RID: 11263
		public float radius = -1f;

		// Token: 0x04002C00 RID: 11264
		public LocomotionUrgency locomotion;

		// Token: 0x04002C01 RID: 11265
		public Danger maxDanger;

		// Token: 0x04002C02 RID: 11266
		public CellRect spectateRect;

		// Token: 0x04002C03 RID: 11267
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x04002C04 RID: 11268
		public SpectateRectSide spectateRectPreferredSide;

		// Token: 0x04002C05 RID: 11269
		public bool canDig;

		// Token: 0x04002C06 RID: 11270
		[Obsolete]
		public PawnsToGather pawnsToGather;

		// Token: 0x04002C07 RID: 11271
		public int transportersGroup = -1;

		// Token: 0x04002C08 RID: 11272
		public bool attackDownedIfStarving;

		// Token: 0x04002C09 RID: 11273
		public float? wanderRadius;
	}
}
