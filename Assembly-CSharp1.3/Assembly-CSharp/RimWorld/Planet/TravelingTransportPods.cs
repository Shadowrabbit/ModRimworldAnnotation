using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017EB RID: 6123
	public class TravelingTransportPods : WorldObject, IThingHolder
	{
		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x06008E8E RID: 36494 RVA: 0x00332EE3 File Offset: 0x003310E3
		private Vector3 Start
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.initialTile);
			}
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x06008E8F RID: 36495 RVA: 0x00332EF5 File Offset: 0x003310F5
		private Vector3 End
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.destinationTile);
			}
		}

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x06008E90 RID: 36496 RVA: 0x00332F07 File Offset: 0x00331107
		public override Vector3 DrawPos
		{
			get
			{
				return Vector3.Slerp(this.Start, this.End, this.traveledPct);
			}
		}

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x06008E91 RID: 36497 RVA: 0x00332F20 File Offset: 0x00331120
		public override bool ExpandingIconFlipHorizontal
		{
			get
			{
				return GenWorldUI.WorldToUIPosition(this.Start).x > GenWorldUI.WorldToUIPosition(this.End).x;
			}
		}

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x06008E92 RID: 36498 RVA: 0x00332F44 File Offset: 0x00331144
		public override float ExpandingIconRotation
		{
			get
			{
				if (!this.def.rotateGraphicWhenTraveling)
				{
					return base.ExpandingIconRotation;
				}
				Vector2 vector = GenWorldUI.WorldToUIPosition(this.Start);
				Vector2 vector2 = GenWorldUI.WorldToUIPosition(this.End);
				float num = Mathf.Atan2(vector2.y - vector.y, vector2.x - vector.x) * 57.29578f;
				if (num > 180f)
				{
					num -= 180f;
				}
				return num + 90f;
			}
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x06008E93 RID: 36499 RVA: 0x00332FBC File Offset: 0x003311BC
		private float TraveledPctStepPerTick
		{
			get
			{
				Vector3 start = this.Start;
				Vector3 end = this.End;
				if (start == end)
				{
					return 1f;
				}
				float num = GenMath.SphericalDistance(start.normalized, end.normalized);
				if (num == 0f)
				{
					return 1f;
				}
				return 0.00025f / num;
			}
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x06008E94 RID: 36500 RVA: 0x00333010 File Offset: 0x00331210
		private bool PodsHaveAnyPotentialCaravanOwner
		{
			get
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					ThingOwner innerContainer = this.pods[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && CaravanUtility.IsOwner(pawn, base.Faction))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x06008E95 RID: 36501 RVA: 0x00333078 File Offset: 0x00331278
		public bool PodsHaveAnyFreeColonist
		{
			get
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					ThingOwner innerContainer = this.pods[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && pawn.IsColonist && pawn.HostFaction == null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x06008E96 RID: 36502 RVA: 0x003330E1 File Offset: 0x003312E1
		public IEnumerable<Pawn> Pawns
		{
			get
			{
				int num;
				for (int i = 0; i < this.pods.Count; i = num + 1)
				{
					ThingOwner things = this.pods[i].innerContainer;
					for (int j = 0; j < things.Count; j = num + 1)
					{
						Pawn pawn = things[j] as Pawn;
						if (pawn != null)
						{
							yield return pawn;
						}
						num = j;
					}
					things = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06008E97 RID: 36503 RVA: 0x003330F4 File Offset: 0x003312F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ActiveDropPodInfo>(ref this.pods, "pods", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<TransportPodsArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.arrived, "arrived", false, false);
			Scribe_Values.Look<int>(ref this.initialTile, "initialTile", 0, false);
			Scribe_Values.Look<float>(ref this.traveledPct, "traveledPct", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.pods.Count; i++)
				{
					this.pods[i].parent = this;
				}
			}
		}

		// Token: 0x06008E98 RID: 36504 RVA: 0x003331AE File Offset: 0x003313AE
		public override void PostAdd()
		{
			base.PostAdd();
			this.initialTile = base.Tile;
		}

		// Token: 0x06008E99 RID: 36505 RVA: 0x003331C2 File Offset: 0x003313C2
		public override void Tick()
		{
			base.Tick();
			this.traveledPct += this.TraveledPctStepPerTick;
			if (this.traveledPct >= 1f)
			{
				this.traveledPct = 1f;
				this.Arrived();
			}
		}

		// Token: 0x06008E9A RID: 36506 RVA: 0x003331FC File Offset: 0x003313FC
		public void AddPod(ActiveDropPodInfo contents, bool justLeftTheMap)
		{
			contents.parent = this;
			this.pods.Add(contents);
			ThingOwner innerContainer = contents.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn = innerContainer[i] as Pawn;
				if (pawn != null && !pawn.IsWorldPawn())
				{
					if (!base.Spawned)
					{
						Log.Warning("Passing pawn " + pawn + " to world, but the TravelingTransportPod is not spawned. This means that WorldPawns can discard this pawn which can cause bugs.");
					}
					if (justLeftTheMap)
					{
						pawn.ExitMap(false, Rot4.Invalid);
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			contents.savePawnsWithReferenceMode = true;
		}

		// Token: 0x06008E9B RID: 36507 RVA: 0x00333290 File Offset: 0x00331490
		public bool ContainsPawn(Pawn p)
		{
			for (int i = 0; i < this.pods.Count; i++)
			{
				if (this.pods[i].innerContainer.Contains(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008E9C RID: 36508 RVA: 0x003332D0 File Offset: 0x003314D0
		private void Arrived()
		{
			if (this.arrived)
			{
				return;
			}
			this.arrived = true;
			if (this.arrivalAction == null || !this.arrivalAction.StillValid(this.pods.Cast<IThingHolder>(), this.destinationTile))
			{
				this.arrivalAction = null;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].Tile == this.destinationTile)
					{
						this.arrivalAction = new TransportPodsArrivalAction_LandInSpecificCell(maps[i].Parent, DropCellFinder.RandomDropSpot(maps[i], true));
						break;
					}
				}
				if (this.arrivalAction == null)
				{
					if (TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(this.pods.Cast<IThingHolder>(), this.destinationTile))
					{
						this.arrivalAction = new TransportPodsArrivalAction_FormCaravan();
					}
					else
					{
						List<Caravan> caravans = Find.WorldObjects.Caravans;
						for (int j = 0; j < caravans.Count; j++)
						{
							if (caravans[j].Tile == this.destinationTile && TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(this.pods.Cast<IThingHolder>(), caravans[j]))
							{
								this.arrivalAction = new TransportPodsArrivalAction_GiveToCaravan(caravans[j]);
								break;
							}
						}
					}
				}
			}
			if (this.arrivalAction != null && this.arrivalAction.ShouldUseLongEvent(this.pods, this.destinationTile))
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.DoArrivalAction();
				}, "GeneratingMapForNewEncounter", false, null, true);
				return;
			}
			this.DoArrivalAction();
		}

		// Token: 0x06008E9D RID: 36509 RVA: 0x00333448 File Offset: 0x00331648
		private void DoArrivalAction()
		{
			for (int i = 0; i < this.pods.Count; i++)
			{
				this.pods[i].savePawnsWithReferenceMode = false;
				this.pods[i].parent = null;
			}
			if (this.arrivalAction != null)
			{
				try
				{
					this.arrivalAction.Arrived(this.pods, this.destinationTile);
				}
				catch (Exception arg)
				{
					Log.Error("Exception in transport pods arrival action: " + arg);
				}
				this.arrivalAction = null;
			}
			else
			{
				for (int j = 0; j < this.pods.Count; j++)
				{
					for (int k = 0; k < this.pods[j].innerContainer.Count; k++)
					{
						Pawn pawn = this.pods[j].innerContainer[k] as Pawn;
						if (pawn != null && (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer))
						{
							PawnBanishUtility.Banish(pawn, this.destinationTile);
						}
					}
				}
				for (int l = 0; l < this.pods.Count; l++)
				{
					this.pods[l].innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
				}
				string key = "MessageTransportPodsArrivedAndLost";
				if (this.def == WorldObjectDefOf.TravelingShuttle)
				{
					key = "MessageShuttleArrivedContentsLost";
				}
				Messages.Message(key.Translate(), new GlobalTargetInfo(this.destinationTile), MessageTypeDefOf.NegativeEvent, true);
			}
			this.pods.Clear();
			this.Destroy();
		}

		// Token: 0x06008E9E RID: 36510 RVA: 0x00002688 File Offset: 0x00000888
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06008E9F RID: 36511 RVA: 0x003335EC File Offset: 0x003317EC
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			for (int i = 0; i < this.pods.Count; i++)
			{
				outChildren.Add(this.pods[i]);
			}
		}

		// Token: 0x040059EA RID: 23018
		public int destinationTile = -1;

		// Token: 0x040059EB RID: 23019
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x040059EC RID: 23020
		private List<ActiveDropPodInfo> pods = new List<ActiveDropPodInfo>();

		// Token: 0x040059ED RID: 23021
		private bool arrived;

		// Token: 0x040059EE RID: 23022
		private int initialTile = -1;

		// Token: 0x040059EF RID: 23023
		private float traveledPct;

		// Token: 0x040059F0 RID: 23024
		private const float TravelSpeed = 0.00025f;
	}
}
