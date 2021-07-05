using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001511 RID: 5393
	public class Verb_Ignite : Verb
	{
		// Token: 0x0600806C RID: 32876 RVA: 0x002D7F76 File Offset: 0x002D6176
		public Verb_Ignite()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
		}

		// Token: 0x0600806D RID: 32877 RVA: 0x002D7F8C File Offset: 0x002D618C
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
