using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016B7 RID: 5815
	public class Building_Door : Building
	{
		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x06007F53 RID: 32595 RVA: 0x000557F5 File Offset: 0x000539F5
		public bool Open
		{
			get
			{
				return this.openInt;
			}
		}

		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x06007F54 RID: 32596 RVA: 0x000557FD File Offset: 0x000539FD
		public bool HoldOpen
		{
			get
			{
				return this.holdOpenInt;
			}
		}

		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x06007F55 RID: 32597 RVA: 0x00055805 File Offset: 0x00053A05
		public bool FreePassage
		{
			get
			{
				return this.openInt && (this.holdOpenInt || !this.WillCloseSoon);
			}
		}

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x06007F56 RID: 32598 RVA: 0x0025CC18 File Offset: 0x0025AE18
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

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06007F57 RID: 32599 RVA: 0x0025CC3C File Offset: 0x0025AE3C
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

		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x06007F58 RID: 32600 RVA: 0x0025CD54 File Offset: 0x0025AF54
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

		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x06007F59 RID: 32601 RVA: 0x00055824 File Offset: 0x00053A24
		public bool DoorPowerOn
		{
			get
			{
				return this.powerComp != null && this.powerComp.PowerOn;
			}
		}

		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x06007F5A RID: 32602 RVA: 0x0005583B File Offset: 0x00053A3B
		public bool SlowsPawns
		{
			get
			{
				return !this.DoorPowerOn || this.TicksToOpenNow > 20;
			}
		}

		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06007F5B RID: 32603 RVA: 0x0025CDAC File Offset: 0x0025AFAC
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

		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x06007F5C RID: 32604 RVA: 0x00055851 File Offset: 0x00053A51
		private bool CanTryCloseAutomatically
		{
			get
			{
				return this.FriendlyTouchedRecently && !this.HoldOpen;
			}
		}

		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x06007F5D RID: 32605 RVA: 0x00055866 File Offset: 0x00053A66
		private bool FriendlyTouchedRecently
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastFriendlyTouchTick + 120;
			}
		}

		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x06007F5E RID: 32606 RVA: 0x0005587D File Offset: 0x00053A7D
		public override bool FireBulwark
		{
			get
			{
				return !this.Open && base.FireBulwark;
			}
		}

		// Token: 0x06007F5F RID: 32607 RVA: 0x0005588F File Offset: 0x00053A8F
		public override void PostMake()
		{
			base.PostMake();
			this.powerComp = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x06007F60 RID: 32608 RVA: 0x000558A3 File Offset: 0x00053AA3
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

		// Token: 0x06007F61 RID: 32609 RVA: 0x0025CDE4 File Offset: 0x0025AFE4
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			this.ClearReachabilityCache(map);
		}

		// Token: 0x06007F62 RID: 32610 RVA: 0x0025CE08 File Offset: 0x0025B008
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.openInt, "open", false, false);
			Scribe_Values.Look<bool>(ref this.holdOpenInt, "holdOpen", false, false);
			Scribe_Values.Look<int>(ref this.lastFriendlyTouchTick, "lastFriendlyTouchTick", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.openInt)
			{
				this.ticksSinceOpen = this.TicksToOpenNow;
			}
		}

		// Token: 0x06007F63 RID: 32611 RVA: 0x000558D0 File Offset: 0x00053AD0
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				this.ClearReachabilityCache(base.Map);
			}
		}

		// Token: 0x06007F64 RID: 32612 RVA: 0x0025CE70 File Offset: 0x0025B070
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
			}
		}

		// Token: 0x06007F65 RID: 32613 RVA: 0x000558EE File Offset: 0x00053AEE
		public void CheckFriendlyTouched(Pawn p)
		{
			if (!p.HostileTo(this) && this.PawnCanOpen(p))
			{
				this.lastFriendlyTouchTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06007F66 RID: 32614 RVA: 0x0025CFDC File Offset: 0x0025B1DC
		public void Notify_PawnApproaching(Pawn p, int moveCost)
		{
			this.CheckFriendlyTouched(p);
			bool flag = this.PawnCanOpen(p);
			if (flag || this.Open)
			{
				base.Map.fogGrid.Notify_PawnEnteringDoor(this, p);
			}
			if (flag && !this.SlowsPawns)
			{
				int ticksToClose = Mathf.Max(300, moveCost + 1);
				this.DoorOpen(ticksToClose);
			}
		}

		// Token: 0x06007F67 RID: 32615 RVA: 0x00055912 File Offset: 0x00053B12
		public bool CanPhysicallyPass(Pawn p)
		{
			return this.FreePassage || this.PawnCanOpen(p) || (this.Open && p.HostileTo(this));
		}

		// Token: 0x06007F68 RID: 32616 RVA: 0x0025D034 File Offset: 0x0025B234
		public virtual bool PawnCanOpen(Pawn p)
		{
			Lord lord = p.GetLord();
			return (lord != null && lord.LordJob != null && lord.LordJob.CanOpenAnyDoor(p)) || WildManUtility.WildManShouldReachOutsideNow(p) || base.Faction == null || (p.guest != null && p.guest.Released) || GenAI.MachinesLike(base.Faction, p);
		}

		// Token: 0x06007F69 RID: 32617 RVA: 0x00055938 File Offset: 0x00053B38
		public override bool BlocksPawn(Pawn p)
		{
			return !this.openInt && !this.PawnCanOpen(p);
		}

		// Token: 0x06007F6A RID: 32618 RVA: 0x0025D09C File Offset: 0x0025B29C
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

		// Token: 0x06007F6B RID: 32619 RVA: 0x0025D140 File Offset: 0x0025B340
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

		// Token: 0x06007F6C RID: 32620 RVA: 0x0005594E File Offset: 0x00053B4E
		public void StartManualOpenBy(Pawn opener)
		{
			this.DoorOpen(110);
		}

		// Token: 0x06007F6D RID: 32621 RVA: 0x00055958 File Offset: 0x00053B58
		public void StartManualCloseBy(Pawn closer)
		{
			this.ticksUntilClose = 110;
		}

		// Token: 0x06007F6E RID: 32622 RVA: 0x0025D1D0 File Offset: 0x0025B3D0
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			float num = Mathf.Clamp01((float)this.ticksSinceOpen / (float)this.TicksToOpenNow);
			float d = 0f + 0.45f * num;
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
			}
			base.Comps_PostDraw();
		}

		// Token: 0x06007F6F RID: 32623 RVA: 0x0025D2E8 File Offset: 0x0025B4E8
		private static int AlignQualityAgainst(IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return 0;
			}
			if (!c.Walkable(map))
			{
				return 9;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
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
					if (typeof(Building_Door).IsAssignableFrom(thing.def.thingClass))
					{
						return 1;
					}
				}
			}
			return 0;
		}

		// Token: 0x06007F70 RID: 32624 RVA: 0x0025D38C File Offset: 0x0025B58C
		public static Rot4 DoorRotationAt(IntVec3 loc, Map map)
		{
			int num = 0;
			int num2 = 0;
			int num3 = num + Building_Door.AlignQualityAgainst(loc + IntVec3.East, map) + Building_Door.AlignQualityAgainst(loc + IntVec3.West, map);
			num2 += Building_Door.AlignQualityAgainst(loc + IntVec3.North, map);
			num2 += Building_Door.AlignQualityAgainst(loc + IntVec3.South, map);
			if (num3 >= num2)
			{
				return Rot4.North;
			}
			return Rot4.East;
		}

		// Token: 0x06007F71 RID: 32625 RVA: 0x00055962 File Offset: 0x00053B62
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

		// Token: 0x06007F72 RID: 32626 RVA: 0x00055972 File Offset: 0x00053B72
		private void ClearReachabilityCache(Map map)
		{
			map.reachability.ClearCache();
			this.freePassageWhenClearedReachabilityCache = this.FreePassage;
		}

		// Token: 0x06007F73 RID: 32627 RVA: 0x0005598B File Offset: 0x00053B8B
		private void CheckClearReachabilityCacheBecauseOpenedOrClosed()
		{
			if (base.Spawned)
			{
				base.Map.reachability.ClearCacheForHostile(this);
			}
		}

		// Token: 0x040052B6 RID: 21174
		public CompPowerTrader powerComp;

		// Token: 0x040052B7 RID: 21175
		private bool openInt;

		// Token: 0x040052B8 RID: 21176
		private bool holdOpenInt;

		// Token: 0x040052B9 RID: 21177
		private int lastFriendlyTouchTick = -9999;

		// Token: 0x040052BA RID: 21178
		protected int ticksUntilClose;

		// Token: 0x040052BB RID: 21179
		protected int ticksSinceOpen;

		// Token: 0x040052BC RID: 21180
		private bool freePassageWhenClearedReachabilityCache;

		// Token: 0x040052BD RID: 21181
		private const float OpenTicks = 45f;

		// Token: 0x040052BE RID: 21182
		private const int CloseDelayTicks = 110;

		// Token: 0x040052BF RID: 21183
		private const int WillCloseSoonThreshold = 111;

		// Token: 0x040052C0 RID: 21184
		private const int ApproachCloseDelayTicks = 300;

		// Token: 0x040052C1 RID: 21185
		private const int MaxTicksSinceFriendlyTouchToAutoClose = 120;

		// Token: 0x040052C2 RID: 21186
		private const float PowerOffDoorOpenSpeedFactor = 0.25f;

		// Token: 0x040052C3 RID: 21187
		private const float VisualDoorOffsetStart = 0f;

		// Token: 0x040052C4 RID: 21188
		private const float VisualDoorOffsetEnd = 0.45f;
	}
}
