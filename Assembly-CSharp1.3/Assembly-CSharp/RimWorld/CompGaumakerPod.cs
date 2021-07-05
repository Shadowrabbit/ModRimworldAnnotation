using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001126 RID: 4390
	public class CompGaumakerPod : CompDryadHolder
	{
		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06006971 RID: 26993 RVA: 0x00238A5C File Offset: 0x00236C5C
		public bool Full
		{
			get
			{
				return this.innerContainer.Count >= 3;
			}
		}

		// Token: 0x06006972 RID: 26994 RVA: 0x00238A6F File Offset: 0x00236C6F
		public override void PostDeSpawn(Map map)
		{
			if (Find.TickManager.TicksGame < this.tickComplete)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			}
		}

		// Token: 0x06006973 RID: 26995 RVA: 0x00238AA0 File Offset: 0x00236CA0
		public override void TryAcceptPawn(Pawn p)
		{
			base.TryAcceptPawn(p);
			if (this.Full)
			{
				Pawn connectedPawn = this.tree.TryGetComp<CompTreeConnection>().ConnectedPawn;
				this.tickComplete = Find.TickManager.TicksGame + (int)(60000f * base.Props.daysToComplete);
			}
		}

		// Token: 0x06006974 RID: 26996 RVA: 0x00238AF0 File Offset: 0x00236CF0
		protected override void Complete()
		{
			this.tickComplete = Find.TickManager.TicksGame;
			CompTreeConnection compTreeConnection = this.tree.TryGetComp<CompTreeConnection>();
			if (compTreeConnection != null)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Pawn pawn;
					if ((pawn = (this.innerContainer[i] as Pawn)) != null)
					{
						compTreeConnection.RemoveDryad(pawn);
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
				}
				compTreeConnection.gaumakerPod = null;
				((Plant)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Plant_PodGauranlen, null), this.parent.Position, this.parent.Map, WipeMode.Vanish)).Growth = 1f;
			}
			this.parent.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06006975 RID: 26997 RVA: 0x00238BA4 File Offset: 0x00236DA4
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.innerContainer.Count < 3)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text = string.Concat(new object[]
				{
					text,
					GenLabel.BestKindLabel(PawnKindDefOf.Dryad_Gaumaker, Gender.Male, true, -1).CapitalizeFirst(),
					": ",
					this.innerContainer.Count,
					"/",
					3
				});
			}
			return text;
		}
	}
}
