using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001147 RID: 4423
	public class CompHatcher : ThingComp
	{
		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06006A2E RID: 27182 RVA: 0x0023BA90 File Offset: 0x00239C90
		public CompProperties_Hatcher Props
		{
			get
			{
				return (CompProperties_Hatcher)this.props;
			}
		}

		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06006A2F RID: 27183 RVA: 0x0023BA9D File Offset: 0x00239C9D
		private CompTemperatureRuinable FreezerComp
		{
			get
			{
				return this.parent.GetComp<CompTemperatureRuinable>();
			}
		}

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06006A30 RID: 27184 RVA: 0x0023BAAA File Offset: 0x00239CAA
		public bool TemperatureDamaged
		{
			get
			{
				return this.FreezerComp != null && this.FreezerComp.Ruined;
			}
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x0023BAC4 File Offset: 0x00239CC4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.gestateProgress, "gestateProgress", 0f, false);
			Scribe_References.Look<Pawn>(ref this.hatcheeParent, "hatcheeParent", false);
			Scribe_References.Look<Pawn>(ref this.otherParent, "otherParent", false);
			Scribe_References.Look<Faction>(ref this.hatcheeFaction, "hatcheeFaction", false);
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x0023BB20 File Offset: 0x00239D20
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

		// Token: 0x06006A33 RID: 27187 RVA: 0x0023BB70 File Offset: 0x00239D70
		public void Hatch()
		{
			try
			{
				PawnGenerationRequest request = new PawnGenerationRequest(this.Props.hatcherPawn, this.hatcheeFaction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false);
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

		// Token: 0x06006A34 RID: 27188 RVA: 0x0023BD58 File Offset: 0x00239F58
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float b = ((ThingWithComps)otherStack).GetComp<CompHatcher>().gestateProgress;
			this.gestateProgress = Mathf.Lerp(this.gestateProgress, b, t);
		}

		// Token: 0x06006A35 RID: 27189 RVA: 0x0023BD9B File Offset: 0x00239F9B
		public override void PostSplitOff(Thing piece)
		{
			CompHatcher comp = ((ThingWithComps)piece).GetComp<CompHatcher>();
			comp.gestateProgress = this.gestateProgress;
			comp.hatcheeParent = this.hatcheeParent;
			comp.otherParent = this.otherParent;
			comp.hatcheeFaction = this.hatcheeFaction;
		}

		// Token: 0x06006A36 RID: 27190 RVA: 0x0023BDD7 File Offset: 0x00239FD7
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

		// Token: 0x06006A37 RID: 27191 RVA: 0x0023BE02 File Offset: 0x0023A002
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			base.PostPostGeneratedForTrader(trader, forTile, forFaction);
			this.hatcheeFaction = forFaction;
		}

		// Token: 0x06006A38 RID: 27192 RVA: 0x0023BE14 File Offset: 0x0023A014
		public override string CompInspectStringExtra()
		{
			if (!this.TemperatureDamaged)
			{
				return "EggProgress".Translate() + ": " + this.gestateProgress.ToStringPercent() + "\n" + "HatchesIn".Translate() + ": " + "PeriodDays".Translate((this.Props.hatcherDaystoHatch * (1f - this.gestateProgress)).ToString("F1"));
			}
			return null;
		}

		// Token: 0x04003B3E RID: 15166
		private float gestateProgress;

		// Token: 0x04003B3F RID: 15167
		public Pawn hatcheeParent;

		// Token: 0x04003B40 RID: 15168
		public Pawn otherParent;

		// Token: 0x04003B41 RID: 15169
		public Faction hatcheeFaction;
	}
}
