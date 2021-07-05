using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200146E RID: 5230
	[StaticConstructorOnStartup]
	public class RoyalTitlePermitWorker_DropResources : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x060070E2 RID: 28898 RVA: 0x0004C10C File Offset: 0x0004A30C
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallResources(target.Cell);
		}

		// Token: 0x060070E3 RID: 28899 RVA: 0x0004C11B File Offset: 0x0004A31B
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				yield return new FloatMenuOption("CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			Action action = null;
			string label = this.def.LabelCap + ": ";
			bool free;
			if (base.FillAidOption(pawn, faction, ref label, out free))
			{
				action = delegate()
				{
					this.BeginCallResources(pawn, faction, map, free);
				};
			}
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield break;
		}

		// Token: 0x060070E4 RID: 28900 RVA: 0x0004C140 File Offset: 0x0004A340
		public override IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
		{
			string defaultDesc;
			bool flag;
			if (!base.FillCaravanAidOption(pawn, faction, out defaultDesc, out this.free, out flag))
			{
				yield break;
			}
			Action <>9__1;
			Command_Action command_Action = new Command_Action
			{
				defaultLabel = this.def.LabelCap + " (" + pawn.LabelShort + ")",
				defaultDesc = defaultDesc,
				icon = RoyalTitlePermitWorker_DropResources.CommandTex,
				action = delegate()
				{
					Caravan caravan = pawn.GetCaravan();
					float num = caravan.MassUsage;
					List<ThingDefCountClass> itemsToDrop = this.def.royalAid.itemsToDrop;
					for (int i = 0; i < itemsToDrop.Count; i++)
					{
						num += itemsToDrop[i].thingDef.BaseMass * (float)itemsToDrop[i].count;
					}
					if (num > caravan.MassCapacity)
					{
						WindowStack windowStack = Find.WindowStack;
						TaggedString text = "DropResourcesOverweightConfirm".Translate();
						Action confirmedAct;
						if ((confirmedAct = <>9__1) == null)
						{
							confirmedAct = (<>9__1 = delegate()
							{
								this.CallResourcesToCaravan(pawn, faction, this.free);
							});
						}
						windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null));
						return;
					}
					this.CallResourcesToCaravan(pawn, faction, this.free);
				}
			};
			if (faction.HostileTo(Faction.OfPlayer))
			{
				command_Action.Disable("CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")));
			}
			if (flag)
			{
				command_Action.Disable("CommandCallRoyalAidNotEnoughFavor".Translate());
			}
			yield return command_Action;
			yield break;
		}

		// Token: 0x060070E5 RID: 28901 RVA: 0x00228584 File Offset: 0x00226784
		private void BeginCallResources(Pawn caller, Faction faction, Map map, bool free)
		{
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetBuildings = false;
			this.targetingParameters.canTargetPawns = false;
			this.caller = caller;
			this.map = map;
			this.faction = faction;
			this.free = free;
			this.targetingParameters.validator = ((TargetInfo target) => (this.def.royalAid.targetingRange <= 0f || target.Cell.DistanceTo(caller.Position) <= this.def.royalAid.targetingRange) && target.Cell.Walkable(map) && !target.Cell.Fogged(map));
			Find.Targeter.BeginTargeting(this, null);
		}

		// Token: 0x060070E6 RID: 28902 RVA: 0x00228628 File Offset: 0x00226828
		private void CallResources(IntVec3 cell)
		{
			List<Thing> list = new List<Thing>();
			for (int i = 0; i < this.def.royalAid.itemsToDrop.Count; i++)
			{
				Thing thing = ThingMaker.MakeThing(this.def.royalAid.itemsToDrop[i].thingDef, null);
				thing.stackCount = this.def.royalAid.itemsToDrop[i].count;
				list.Add(thing);
			}
			if (list.Any<Thing>())
			{
				ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
				activeDropPodInfo.innerContainer.TryAddRangeOrTransfer(list, true, false);
				DropPodUtility.MakeDropPodAt(cell, this.map, activeDropPodInfo);
				Messages.Message("MessagePermitTransportDrop".Translate(this.faction.Named("FACTION")), new LookTargets(cell, this.map), MessageTypeDefOf.NeutralEvent, true);
				this.caller.royalty.GetPermit(this.def, this.faction).Notify_Used();
				if (!this.free)
				{
					this.caller.royalty.TryRemoveFavor(this.faction, this.def.royalAid.favorCost);
				}
			}
		}

		// Token: 0x060070E7 RID: 28903 RVA: 0x00228758 File Offset: 0x00226958
		private void CallResourcesToCaravan(Pawn caller, Faction faction, bool free)
		{
			Caravan caravan = caller.GetCaravan();
			for (int i = 0; i < this.def.royalAid.itemsToDrop.Count; i++)
			{
				Thing thing = ThingMaker.MakeThing(this.def.royalAid.itemsToDrop[i].thingDef, null);
				thing.stackCount = this.def.royalAid.itemsToDrop[i].count;
				CaravanInventoryUtility.GiveThing(caravan, thing);
			}
			Messages.Message("MessagePermitTransportDropCaravan".Translate(faction.Named("FACTION"), caller.Named("PAWN")), caravan, MessageTypeDefOf.NeutralEvent, true);
			caller.royalty.GetPermit(this.def, faction).Notify_Used();
			if (!free)
			{
				caller.royalty.TryRemoveFavor(faction, this.def.royalAid.favorCost);
			}
		}

		// Token: 0x04004A76 RID: 19062
		private Faction faction;

		// Token: 0x04004A77 RID: 19063
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallAid", true);
	}
}
