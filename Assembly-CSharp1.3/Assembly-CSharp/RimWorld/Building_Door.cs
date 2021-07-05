using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001077 RID: 4215
	public class Building_Door : Building
	{
		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x06006412 RID: 25618 RVA: 0x0021C34C File Offset: 0x0021A54C
		public bool Open
		{
			get
			{
				return this.openInt;
			}
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x06006413 RID: 25619 RVA: 0x0021C354 File Offset: 0x0021A554
		public bool HoldOpen
		{
			get
			{
				return this.holdOpenInt;
			}
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x06006414 RID: 25620 RVA: 0x0021C35C File Offset: 0x0021A55C
		public bool FreePassage
		{
			get
			{
				return this.openInt && (this.holdOpenInt || !this.WillCloseSoon);
			}
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x06006415 RID: 25621 RVA: 0x0021C37C File Offset: 0x0021A57C
		public int TicksTillFullyOpened
		{
			get
			{
				int num = this.TicksToOpenNow - this.ticksSinceOpen;
				if (num < 0)
				{
					num = 0;
				}
				return num;
			}
		}

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x06006416 RID: 25622 RVA: 0x0021C3A0 File Offset: 0x0021A5A0
		public bool WillCloseSoon
		{
			get
			{
				if (!base.Spawned)
				{
					return true;
				}
				if (!this.openInt)
				{
					return true;
				}
				if (this.holdOpenInt)
				{
					return false;
				}
				if (this.ticksUntilClose > 0 && this.ticksUntilClose <= 111 && !this.BlockedOpenMomentary)
				{
					return true;
				}
				if (this.CanTryCloseAutomatically && !this.BlockedOpenMomentary)
				{
					return true;
				}
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c = base.Position + GenAdj.CardinalDirectionsAndInside[i];
					if (c.InBounds(base.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							Pawn pawn = thingList[j] as Pawn;
							if (pawn != null && !pawn.HostileTo(this) && !pawn.Downed && (pawn.Position == base.Position || (pawn.pather.Moving && pawn.pather.nextCell == base.Position)))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x06006417 RID: 25623 RVA: 0x0021C4B8 File Offset: 0x0021A6B8
		public bool BlockedOpenMomentary
		{
			get
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x06006418 RID: 25624 RVA: 0x0021C50F File Offset: 0x0021A70F
		public bool DoorPowerOn
		{
			get
			{
				return this.powerComp != null && this.powerComp.PowerOn;
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x06006419 RID: 25625 RVA: 0x0021C526 File Offset: 0x0021A726
		public bool SlowsPawns
		{
			get
			{
				return !this.DoorPowerOn || this.TicksToOpenNow > 20;
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x0600641A RID: 25626 RVA: 0x0021C53C File Offset: 0x0021A73C
		public int TicksToOpenNow
		{
			get
			{
				float num = 45f / this.GetStatValue(StatDefOf.DoorOpenSpeed, true);
				if (this.DoorPowerOn)
				{
					num *= 0.25f;
				}
				return Mathf.RoundToInt(num);
			}
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x0600641B RID: 25627 RVA: 0x0021C572 File Offset: 0x0021A772
		private bool CanTryCloseAutomatically
		{
			get
			{
				return this.FriendlyTouchedRecently && !this.HoldOpen;
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x0600641C RID: 25628 RVA: 0x0021C587 File Offset: 0x0021A787
		private bool FriendlyTouchedRecently
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastFriendlyTouchTick + 120;
			}
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x0600641D RID: 25629 RVA: 0x0021C59E File Offset: 0x0021A79E
		public override bool FireBulwark
		{
			get
			{
				return !this.Open && base.FireBulwark;
			}
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x0600641E RID: 25630 RVA: 0x0021C5B0 File Offset: 0x0021A7B0
		private float OpenPct
		{
			get
			{
				return Mathf.Clamp01((float)this.ticksSinceOpen / (float)this.TicksToOpenNow);
			}
		}

		// Token: 0x0600641F RID: 25631 RVA: 0x0021C5C6 File Offset: 0x0021A7C6
		public override void PostMake()
		{
			base.PostMake();
			this.powerComp = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x06006420 RID: 25632 RVA: 0x0021C5DA File Offset: 0x0021A7DA
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.ClearReachabilityCache(map);
			if (this.BlockedOpenMomentary)
			{
				this.DoorOpen(110);
			}
		}

		// Token: 0x06006421 RID: 25633 RVA: 0x0021C608 File Offset: 0x0021A808
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			this.ClearReachabilityCache(map);
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x0021C62C File Offset: 0x0021A82C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.openInt, "open", false, false);
			Scribe_Values.Look<bool>(ref this.holdOpenInt, "holdOpen", false, false);
			Scribe_Values.Look<int>(ref this.lastFriendlyTouchTick, "lastFriendlyTouchTick", 0, false);
			Scribe_References.Look<Pawn>(ref this.approachingPawn, "approachingPawn", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.openInt)
			{
				this.ticksSinceOpen = this.TicksToOpenNow;
			}
		}

		// Token: 0x06006423 RID: 25635 RVA: 0x0021C6A2 File Offset: 0x0021A8A2
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				this.ClearReachabilityCache(base.Map);
			}
		}

		// Token: 0x06006424 RID: 25636 RVA: 0x0021C6C0 File Offset: 0x0021A8C0
		public override void Tick()
		{
			base.Tick();
			if (this.FreePassage != this.freePassageWhenClearedReachabilityCache)
			{
				this.ClearReachabilityCache(base.Map);
			}
			if (!this.openInt)
			{
				if (this.ticksSinceOpen > 0)
				{
					this.ticksSinceOpen--;
				}
				if ((Find.TickManager.TicksGame + this.thingIDNumber.HashOffset()) % 375 == 0)
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, 1f, false);
					return;
				}
			}
			else if (this.openInt)
			{
				if (this.ticksSinceOpen < this.TicksToOpenNow)
				{
					this.ticksSinceOpen++;
				}
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null)
					{
						this.CheckFriendlyTouched(pawn);
					}
				}
				if (this.ticksUntilClose > 0)
				{
					if (base.Map.thingGrid.CellContains(base.Position, ThingCategory.Pawn))
					{
						this.ticksUntilClose = 110;
					}
					this.ticksUntilClose--;
					if (this.ticksUntilClose <= 0 && !this.holdOpenInt && !this.DoorTryClose())
					{
						this.ticksUntilClose = 1;
					}
				}
				else if (this.CanTryCloseAutomatically)
				{
					this.ticksUntilClose = 110;
				}
				if ((Find.TickManager.TicksGame + this.thingIDNumber.HashOffset()) % 34 == 0)
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, 1f, false);
				}
				if (this.OpenPct >= 0.4f && this.approachingPawn != null)
				{
					base.Map.fogGrid.Notify_PawnEnteringDoor(this, this.approachingPawn);
					this.approachingPawn = null;
				}
			}
		}

		// Token: 0x06006425 RID: 25637 RVA: 0x0021C85F File Offset: 0x0021AA5F
		public void CheckFriendlyTouched(Pawn p)
		{
			if (!p.HostileTo(this) && this.PawnCanOpen(p))
			{
				this.lastFriendlyTouchTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06006426 RID: 25638 RVA: 0x0021C884 File Offset: 0x0021AA84
		public void Notify_PawnApproaching(Pawn p, int moveCost)
		{
			this.CheckFriendlyTouched(p);
			bool flag = this.PawnCanOpen(p);
			if (flag || this.Open)
			{
				this.approachingPawn = p;
			}
			if (flag && !this.SlowsPawns)
			{
				int ticksToClose = Mathf.Max(300, moveCost + 1);
				this.DoorOpen(ticksToClose);
			}
		}

		// Token: 0x06006427 RID: 25639 RVA: 0x0021C8D0 File Offset: 0x0021AAD0
		public bool CanPhysicallyPass(Pawn p)
		{
			return this.FreePassage || this.PawnCanOpen(p) || (this.Open && p.HostileTo(this));
		}

		// Token: 0x06006428 RID: 25640 RVA: 0x0021C8F8 File Offset: 0x0021AAF8
		public virtual bool PawnCanOpen(Pawn p)
		{
			if (base.Map != null && base.Map.Parent.doorsAlwaysOpenForPlayerPawns && p.Faction == Faction.OfPlayer)
			{
				return true;
			}
			Lord lord = p.GetLord();
			return (lord != null && lord.LordJob != null && lord.LordJob.CanOpenAnyDoor(p)) || WildManUtility.WildManShouldReachOutsideNow(p) || ((!p.RaceProps.FenceBlocked || this.def.building.roamerCanOpen || (p.roping.IsRopedByPawn && this.PawnCanOpen(p.roping.RopedByPawn))) && (base.Faction == null || (p.guest != null && p.guest.Released) || GenAI.MachinesLike(base.Faction, p)));
		}

		// Token: 0x06006429 RID: 25641 RVA: 0x0021C9C8 File Offset: 0x0021ABC8
		public override bool BlocksPawn(Pawn p)
		{
			return !this.openInt && !this.PawnCanOpen(p);
		}

		// Token: 0x0600642A RID: 25642 RVA: 0x0021C9E0 File Offset: 0x0021ABE0
		protected void DoorOpen(int ticksToClose = 110)
		{
			if (this.openInt)
			{
				this.ticksUntilClose = ticksToClose;
			}
			else
			{
				this.ticksUntilClose = this.TicksToOpenNow + ticksToClose;
			}
			if (!this.openInt)
			{
				this.openInt = true;
				this.CheckClearReachabilityCacheBecauseOpenedOrClosed();
				if (this.DoorPowerOn)
				{
					this.def.building.soundDoorOpenPowered.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
					return;
				}
				this.def.building.soundDoorOpenManual.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
		}

		// Token: 0x0600642B RID: 25643 RVA: 0x0021CA84 File Offset: 0x0021AC84
		protected bool DoorTryClose()
		{
			if (this.holdOpenInt || this.BlockedOpenMomentary)
			{
				return false;
			}
			this.openInt = false;
			this.CheckClearReachabilityCacheBecauseOpenedOrClosed();
			if (this.DoorPowerOn)
			{
				this.def.building.soundDoorClosePowered.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			else
			{
				this.def.building.soundDoorCloseManual.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			return true;
		}

		// Token: 0x0600642C RID: 25644 RVA: 0x0021CB13 File Offset: 0x0021AD13
		public void StartManualOpenBy(Pawn opener)
		{
			this.DoorOpen(110);
		}

		// Token: 0x0600642D RID: 25645 RVA: 0x0021CB1D File Offset: 0x0021AD1D
		public void StartManualCloseBy(Pawn closer)
		{
			this.ticksUntilClose = 110;
		}

		// Token: 0x0600642E RID: 25646 RVA: 0x0021CB28 File Offset: 0x0021AD28
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			float d = 0f + 0.45f * this.OpenPct;
			for (int i = 0; i < 2; i++)
			{
				Vector3 vector = default(Vector3);
				Mesh mesh;
				if (i == 0)
				{
					vector = new Vector3(0f, 0f, -1f);
					mesh = MeshPool.plane10;
				}
				else
				{
					vector = new Vector3(0f, 0f, 1f);
					mesh = MeshPool.plane10Flip;
				}
				Rot4 rotation = base.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				vector = rotation.AsQuat * vector;
				Vector3 vector2 = this.DrawPos;
				vector2.y = AltitudeLayer.DoorMoveable.AltitudeFor();
				vector2 += vector * d;
				Graphics.DrawMesh(mesh, vector2, base.Rotation.AsQuat, this.Graphic.MatAt(base.Rotation, null), 0);
				Graphic_Shadow shadowGraphic = this.Graphic.ShadowGraphic;
				if (shadowGraphic != null)
				{
					shadowGraphic.DrawWorker(vector2, base.Rotation, this.def, this, 0f);
				}
			}
			base.Comps_PostDraw();
		}

		// Token: 0x0600642F RID: 25647 RVA: 0x0021CC54 File Offset: 0x0021AE54
		private static int AlignQualityAgainst(IntVec3 c, IntVec3 offset, Map map)
		{
			IntVec3 c2 = c + offset;
			if (!c2.InBounds(map))
			{
				return 0;
			}
			if (!c2.WalkableByNormal(map))
			{
				return 9;
			}
			List<Thing> thingList = c2.GetThingList(map);
			int i = 0;
			while (i < thingList.Count)
			{
				Thing thing = thingList[i];
				if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
				{
					if ((c - offset).GetDoor(map) == null)
					{
						return 1;
					}
					return 5;
				}
				else
				{
					if (thing.def.IsFence)
					{
						return 1;
					}
					Thing thing2 = thing as Blueprint;
					if (thing2 != null)
					{
						if (thing2.def.entityDefToBuild.passability == Traversability.Impassable)
						{
							return 9;
						}
						ThingDef thingDef;
						if ((thingDef = (thing2.def.entityDefToBuild as ThingDef)) != null && thingDef.IsFence)
						{
							return 1;
						}
						if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
						{
							return 1;
						}
					}
					i++;
				}
			}
			return 0;
		}

		// Token: 0x06006430 RID: 25648 RVA: 0x0021CD48 File Offset: 0x0021AF48
		public static Rot4 DoorRotationAt(IntVec3 loc, Map map)
		{
			int num = 0;
			int num2 = 0;
			int num3 = num + Building_Door.AlignQualityAgainst(loc, IntVec3.East, map) + Building_Door.AlignQualityAgainst(loc, IntVec3.West, map);
			num2 += Building_Door.AlignQualityAgainst(loc, IntVec3.North, map);
			num2 += Building_Door.AlignQualityAgainst(loc, IntVec3.South, map);
			if (num3 >= num2)
			{
				return Rot4.North;
			}
			return Rot4.East;
		}

		// Token: 0x06006431 RID: 25649 RVA: 0x0021CD9E File Offset: 0x0021AF9E
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.Faction == Faction.OfPlayer)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandToggleDoorHoldOpen".Translate(),
					defaultDesc = "CommandToggleDoorHoldOpenDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc3,
					icon = TexCommand.HoldOpen,
					isActive = (() => this.holdOpenInt),
					toggleAction = delegate()
					{
						this.holdOpenInt = !this.holdOpenInt;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06006432 RID: 25650 RVA: 0x0021CDAE File Offset: 0x0021AFAE
		private void ClearReachabilityCache(Map map)
		{
			map.reachability.ClearCache();
			this.freePassageWhenClearedReachabilityCache = this.FreePassage;
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x0021CDC7 File Offset: 0x0021AFC7
		private void CheckClearReachabilityCacheBecauseOpenedOrClosed()
		{
			if (base.Spawned)
			{
				base.Map.reachability.ClearCacheForHostile(this);
			}
		}

		// Token: 0x04003881 RID: 14465
		public CompPowerTrader powerComp;

		// Token: 0x04003882 RID: 14466
		private bool openInt;

		// Token: 0x04003883 RID: 14467
		private bool holdOpenInt;

		// Token: 0x04003884 RID: 14468
		private int lastFriendlyTouchTick = -9999;

		// Token: 0x04003885 RID: 14469
		protected int ticksUntilClose;

		// Token: 0x04003886 RID: 14470
		protected int ticksSinceOpen;

		// Token: 0x04003887 RID: 14471
		private bool freePassageWhenClearedReachabilityCache;

		// Token: 0x04003888 RID: 14472
		private Pawn approachingPawn;

		// Token: 0x04003889 RID: 14473
		private const float OpenTicks = 45f;

		// Token: 0x0400388A RID: 14474
		private const int CloseDelayTicks = 110;

		// Token: 0x0400388B RID: 14475
		private const int WillCloseSoonThreshold = 111;

		// Token: 0x0400388C RID: 14476
		private const int ApproachCloseDelayTicks = 300;

		// Token: 0x0400388D RID: 14477
		private const int MaxTicksSinceFriendlyTouchToAutoClose = 120;

		// Token: 0x0400388E RID: 14478
		private const float PowerOffDoorOpenSpeedFactor = 0.25f;

		// Token: 0x0400388F RID: 14479
		private const float VisualDoorOffsetStart = 0f;

		// Token: 0x04003890 RID: 14480
		private const float VisualDoorOffsetEnd = 0.45f;

		// Token: 0x04003891 RID: 14481
		private const float NotifyFogGridDoorOpenPct = 0.4f;
	}
}
