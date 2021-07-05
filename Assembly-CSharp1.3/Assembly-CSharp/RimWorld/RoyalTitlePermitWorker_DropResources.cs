using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEB RID: 3563
	[StaticConstructorOnStartup]
	public class RoyalTitlePermitWorker_DropResources : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x0600528A RID: 21130 RVA: 0x001BD89D File Offset: 0x001BBA9D
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallResources(target.Cell);
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x001BD8AC File Offset: 0x001BBAAC
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				yield return new FloatMenuOption("CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
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
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x001BD8D1 File Offset: 0x001BBAD1
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

		// Token: 0x0600528D RID: 21133 RVA: 0x001BD8F0 File Offset: 0x001BBAF0
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

		// Token: 0x0600528E RID: 21134 RVA: 0x001BD994 File Offset: 0x001BBB94
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

		// Token: 0x0600528F RID: 21135 RVA: 0x001BDAC4 File Offset: 0x001BBCC4
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

		// Token: 0x040030B0 RID: 12464
		private Faction faction;

		// Token: 0x040030B1 RID: 12465
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallAid", true);
	}
}
