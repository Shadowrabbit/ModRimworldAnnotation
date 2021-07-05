using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000FA5 RID: 4005
	public class ActionOnTick_ReleaseLantern : ActionOnTick
	{
		// Token: 0x06005EA4 RID: 24228 RVA: 0x00206CFC File Offset: 0x00204EFC
		public override void Apply(LordJob_Ritual ritual)
		{
			if (!ritual.lord.ownedPawns.Contains(this.pawn))
			{
				return;
			}
			if (this.pawn.carryTracker.CarriedCount(ThingDefOf.WoodLog) >= this.woodCost)
			{
				this.pawn.carryTracker.DestroyCarriedThing();
				Thing newThing = ThingMaker.MakeThing(ThingDefOf.SkyLantern, null);
				IntVec3 intVec = this.pawn.Position + new IntVec3(-1, 0, 1);
				if (intVec.InBounds(this.pawn.Map))
				{
					GenSpawn.Spawn(newThing, intVec, this.pawn.Map, WipeMode.Vanish);
					ritual.AddTagForPawn(this.pawn, "LaunchedSkyLantern");
				}
				SoundDefOf.Interact_ReleaseSkylantern.PlayOneShot(new TargetInfo(intVec, this.pawn.Map, false));
			}
		}

		// Token: 0x06005EA5 RID: 24229 RVA: 0x00206DD0 File Offset: 0x00204FD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.woodCost, "woodCost", 0, false);
		}

		// Token: 0x04003690 RID: 13968
		public Pawn pawn;

		// Token: 0x04003691 RID: 13969
		public int woodCost = 4;
	}
}
