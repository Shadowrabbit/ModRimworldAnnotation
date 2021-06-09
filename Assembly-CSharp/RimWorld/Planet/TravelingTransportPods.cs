using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002183 RID: 8579
	public class TravelingTransportPods : WorldObject, IThingHolder
	{
		// Token: 0x17001AF7 RID: 6903
		// (get) Token: 0x0600B6D1 RID: 46801 RVA: 0x00076940 File Offset: 0x00074B40
		private Vector3 Start
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.initialTile);
			}
		}

		// Token: 0x17001AF8 RID: 6904
		// (get) Token: 0x0600B6D2 RID: 46802 RVA: 0x00076952 File Offset: 0x00074B52
		private Vector3 End
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.destinationTile);
			}
		}

		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x0600B6D3 RID: 46803 RVA: 0x00076964 File Offset: 0x00074B64
		public override Vector3 DrawPos
		{
			get
			{
				return Vector3.Slerp(this.Start, this.End, this.traveledPct);
			}
		}

		// Token: 0x17001AFA RID: 6906
		// (get) Token: 0x0600B6D4 RID: 46804 RVA: 0x0007697D File Offset: 0x00074B7D
		public override bool ExpandingIconFlipHorizontal
		{
			get
			{
				return GenWorldUI.WorldToUIPosition(this.Start).x > GenWorldUI.WorldToUIPosition(this.End).x;
			}
		}

		// Token: 0x17001AFB RID: 6907
		// (get) Token: 0x0600B6D5 RID: 46805 RVA: 0x0034D008 File Offset: 0x0034B208
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

		// Token: 0x17001AFC RID: 6908
		// (get) Token: 0x0600B6D6 RID: 46806 RVA: 0x0034D080 File Offset: 0x0034B280
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

		// Token: 0x17001AFD RID: 6909
		// (get) Token: 0x0600B6D7 RID: 46807 RVA: 0x0034D0D4 File Offset: 0x0034B2D4
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

		// Token: 0x17001AFE RID: 6910
		// (get) Token: 0x0600B6D8 RID: 46808 RVA: 0x0034D13C File Offset: 0x0034B33C
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

		// Token: 0x17001AFF RID: 6911
		// (get) Token: 0x0600B6D9 RID: 46809 RVA: 0x000769A1 File Offset: 0x00074BA1
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

		// Token: 0x0600B6DA RID: 46810 RVA: 0x0034D1A8 File Offset: 0x0034B3A8
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

		// Token: 0x0600B6DB RID: 46811 RVA: 0x000769B1 File Offset: 0x00074BB1
		public override void PostAdd()
		{
			base.PostAdd();
			this.initialTile = base.Tile;
		}

		// Token: 0x0600B6DC RID: 46812 RVA: 0x000769C5 File Offset: 0x00074BC5
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

		// Token: 0x0600B6DD RID: 46813 RVA: 0x0034D264 File Offset: 0x0034B464
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
						Log.Warning("Passing pawn " + pawn + " to world, but the TravelingTransportPod is not spawned. This means that WorldPawns can discard this pawn which can cause bugs.", false);
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

		// Token: 0x0600B6DE RID: 46814 RVA: 0x0034D2F8 File Offset: 0x0034B4F8
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

		// Token: 0x0600B6DF RID: 46815 RVA: 0x0034D338 File Offset: 0x0034B538
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
						this.arrivalAction = new TransportPodsArrivalAction_LandInSpecificCell(maps[i].Parent, DropCellFinder.RandomDropSpot(maps[i]));
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

		// Token: 0x0600B6E0 RID: 46816 RVA: 0x0034D4B0 File Offset: 0x0034B6B0
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
					Log.Error("Exception in transport pods arrival action: " + arg, false);
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

		// Token: 0x0600B6E1 RID: 46817 RVA: 0x0000C32E File Offset: 0x0000A52E
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x0600B6E2 RID: 46818 RVA: 0x0034D654 File Offset: 0x0034B854
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			for (int i = 0; i < this.pods.Count; i++)
			{
				outChildren.Add(this.pods[i]);
			}
		}

		// Token: 0x04007D3A RID: 32058
		public int destinationTile = -1;

		// Token: 0x04007D3B RID: 32059
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x04007D3C RID: 32060
		private List<ActiveDropPodInfo> pods = new List<ActiveDropPodInfo>();

		// Token: 0x04007D3D RID: 32061
		private bool arrived;

		// Token: 0x04007D3E RID: 32062
		private int initialTile = -1;

		// Token: 0x04007D3F RID: 32063
		private float traveledPct;

		// Token: 0x04007D40 RID: 32064
		private const float TravelSpeed = 0.00025f;
	}
}
