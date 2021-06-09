using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D86 RID: 7558
	public class Verb_Ignite : Verb
	{
		// Token: 0x0600A43B RID: 42043 RVA: 0x0006CE3E File Offset: 0x0006B03E
		public Verb_Ignite()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
		}

		// Token: 0x0600A43C RID: 42044 RVA: 0x002FCFBC File Offset: 0x002FB1BC
		protected override bool TryCastShot()
		{
			Thing thing = this.currentTarget.Thing;
			Pawn casterPawn = this.CasterPawn;
			FireUtility.TryStartFireIn(thing.OccupiedRect().ClosestCellTo(casterPawn.Position), casterPawn.Map, 0.3f);
			if (casterPawn.Spawned)
			{
				casterPawn.Drawer.Notify_MeleeAttackOn(thing);
			}
			return true;
		}
	}
}
