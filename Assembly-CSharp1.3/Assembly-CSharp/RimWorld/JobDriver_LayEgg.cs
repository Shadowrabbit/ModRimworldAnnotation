using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006BF RID: 1727
	public class JobDriver_LayEgg : JobDriver
	{
		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x0011D068 File Offset: 0x0011B268
		public CompEggContainer EggBoxComp
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEggContainer>();
			}
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x0011D08E File Offset: 0x0011B28E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return Toils_General.Wait(500, TargetIndex.None);
			yield return Toils_General.Do(delegate
			{
				Thing thing = this.pawn.GetComp<CompEggLayer>().ProduceEgg();
				if (this.job.GetTarget(TargetIndex.A).HasThing && this.EggBoxComp.Accepts(thing.def))
				{
					this.EggBoxComp.innerContainer.TryAdd(thing, true);
					return;
				}
				Thing t = GenSpawn.Spawn(thing, this.pawn.Position, base.Map, WipeMode.Vanish);
				if (this.pawn.Faction != Faction.OfPlayer)
				{
					t.SetForbidden(true, true);
				}
			});
			yield break;
		}

		// Token: 0x04001D31 RID: 7473
		private const int LayEgg = 500;

		// Token: 0x04001D32 RID: 7474
		private const TargetIndex LaySpotOrEggBoxInd = TargetIndex.A;
	}
}
