using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008A4 RID: 2212
	public class LordToilData_AssaultColonyBreaching : LordToilData
	{
		// Token: 0x06003A88 RID: 14984 RVA: 0x00147894 File Offset: 0x00145A94
		public LordToilData_AssaultColonyBreaching()
		{
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x001478BD File Offset: 0x00145ABD
		public LordToilData_AssaultColonyBreaching(Lord lord)
		{
			this.breachingGrid = new BreachingGrid(lord.Map, lord);
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x001478F8 File Offset: 0x00145AF8
		public void Reset()
		{
			this.breachDest = IntVec3.Invalid;
			this.breachStart = IntVec3.Invalid;
			this.currentTarget = null;
			this.soloAttacker = null;
			this.breachingGrid.Reset();
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x0014792C File Offset: 0x00145B2C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.breachDest, "breachDest", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.breachStart, "breachStart", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.preferMelee, "preferMelee", false, false);
			Scribe_Deep.Look<BreachingGrid>(ref this.breachingGrid, "breachingGrid", Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.currentTarget, "currentTarget", false);
			Scribe_References.Look<Pawn>(ref this.soloAttacker, "soloAttacker", false);
			Scribe_Values.Look<float>(ref this.maxRange, "maxRange", 0f, false);
		}

		// Token: 0x04001FFF RID: 8191
		public IntVec3 breachDest = IntVec3.Invalid;

		// Token: 0x04002000 RID: 8192
		public IntVec3 breachStart = IntVec3.Invalid;

		// Token: 0x04002001 RID: 8193
		public bool preferMelee;

		// Token: 0x04002002 RID: 8194
		public BreachingGrid breachingGrid;

		// Token: 0x04002003 RID: 8195
		public Thing currentTarget;

		// Token: 0x04002004 RID: 8196
		public Pawn soloAttacker;

		// Token: 0x04002005 RID: 8197
		public float maxRange = 12f;
	}
}
