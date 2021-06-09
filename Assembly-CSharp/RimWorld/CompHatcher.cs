using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CE RID: 6094
	public class CompHatcher : ThingComp
	{
		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x060086C4 RID: 34500 RVA: 0x0005A69B File Offset: 0x0005889B
		public CompProperties_Hatcher Props
		{
			get
			{
				return (CompProperties_Hatcher)this.props;
			}
		}

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x060086C5 RID: 34501 RVA: 0x0005A6A8 File Offset: 0x000588A8
		private CompTemperatureRuinable FreezerComp
		{
			get
			{
				return this.parent.GetComp<CompTemperatureRuinable>();
			}
		}

		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x060086C6 RID: 34502 RVA: 0x0005A6B5 File Offset: 0x000588B5
		public bool TemperatureDamaged
		{
			get
			{
				return this.FreezerComp != null && this.FreezerComp.Ruined;
			}
		}

		// Token: 0x060086C7 RID: 34503 RVA: 0x00279A64 File Offset: 0x00277C64
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.gestateProgress, "gestateProgress", 0f, false);
			Scribe_References.Look<Pawn>(ref this.hatcheeParent, "hatcheeParent", false);
			Scribe_References.Look<Pawn>(ref this.otherParent, "otherParent", false);
			Scribe_References.Look<Faction>(ref this.hatcheeFaction, "hatcheeFaction", false);
		}

		// Token: 0x060086C8 RID: 34504 RVA: 0x00279AC0 File Offset: 0x00277CC0
		public override void CompTick()
		{
			if (!this.TemperatureDamaged)
			{
				float num = 1f / (this.Props.hatcherDaystoHatch * 60000f);
				this.gestateProgress += num;
				if (this.gestateProgress > 1f)
				{
					this.Hatch();
				}
			}
		}

		// Token: 0x060086C9 RID: 34505 RVA: 0x00279B10 File Offset: 0x00277D10
		public void Hatch()
		{
			try
			{
				PawnGenerationRequest request = new PawnGenerationRequest(this.Props.hatcherPawn, this.hatcheeFaction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
				for (int i = 0; i < this.parent.stackCount; i++)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(request);
					if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, this.parent))
					{
						if (pawn != null)
						{
							if (this.hatcheeParent != null)
							{
								if (pawn.playerSettings != null && this.hatcheeParent.playerSettings != null && this.hatcheeParent.Faction == this.hatcheeFaction)
								{
									pawn.playerSettings.AreaRestriction = this.hatcheeParent.playerSettings.AreaRestriction;
								}
								if (pawn.RaceProps.IsFlesh)
								{
									pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.hatcheeParent);
								}
							}
							if (this.otherParent != null && (this.hatcheeParent == null || this.hatcheeParent.gender != this.otherParent.gender) && pawn.RaceProps.IsFlesh)
							{
								pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.otherParent);
							}
						}
						if (this.parent.Spawned)
						{
							FilthMaker.TryMakeFilth(this.parent.Position, this.parent.Map, ThingDefOf.Filth_AmnioticFluid, 1, FilthSourceFlags.None);
						}
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
				}
			}
			finally
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060086CA RID: 34506 RVA: 0x00279CF0 File Offset: 0x00277EF0
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float b = ((ThingWithComps)otherStack).GetComp<CompHatcher>().gestateProgress;
			this.gestateProgress = Mathf.Lerp(this.gestateProgress, b, t);
		}

		// Token: 0x060086CB RID: 34507 RVA: 0x0005A6CC File Offset: 0x000588CC
		public override void PostSplitOff(Thing piece)
		{
			CompHatcher comp = ((ThingWithComps)piece).GetComp<CompHatcher>();
			comp.gestateProgress = this.gestateProgress;
			comp.hatcheeParent = this.hatcheeParent;
			comp.otherParent = this.otherParent;
			comp.hatcheeFaction = this.hatcheeFaction;
		}

		// Token: 0x060086CC RID: 34508 RVA: 0x0005A708 File Offset: 0x00058908
		public override void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PrePreTraded(action, playerNegotiator, trader);
			if (action == TradeAction.PlayerBuys)
			{
				this.hatcheeFaction = Faction.OfPlayer;
				return;
			}
			if (action == TradeAction.PlayerSells)
			{
				this.hatcheeFaction = trader.Faction;
			}
		}

		// Token: 0x060086CD RID: 34509 RVA: 0x0005A733 File Offset: 0x00058933
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			base.PostPostGeneratedForTrader(trader, forTile, forFaction);
			this.hatcheeFaction = forFaction;
		}

		// Token: 0x060086CE RID: 34510 RVA: 0x0005A745 File Offset: 0x00058945
		public override string CompInspectStringExtra()
		{
			if (!this.TemperatureDamaged)
			{
				return "EggProgress".Translate() + ": " + this.gestateProgress.ToStringPercent();
			}
			return null;
		}

		// Token: 0x040056A6 RID: 22182
		private float gestateProgress;

		// Token: 0x040056A7 RID: 22183
		public Pawn hatcheeParent;

		// Token: 0x040056A8 RID: 22184
		public Pawn otherParent;

		// Token: 0x040056A9 RID: 22185
		public Faction hatcheeFaction;
	}
}
