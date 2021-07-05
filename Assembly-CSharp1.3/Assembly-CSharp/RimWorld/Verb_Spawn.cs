using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200151A RID: 5402
	public class Verb_Spawn : Verb_CastBase
	{
		// Token: 0x06008099 RID: 32921 RVA: 0x002D9084 File Offset: 0x002D7284
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			GenSpawn.Spawn(this.verbProps.spawnDef, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			if (this.verbProps.colonyWideTaleDef != null)
			{
				Pawn pawn = this.caster.Map.mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
				TaleRecorder.RecordTale(this.verbProps.colonyWideTaleDef, new object[]
				{
					this.caster,
					pawn
				});
			}
			CompReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}
	}
}
