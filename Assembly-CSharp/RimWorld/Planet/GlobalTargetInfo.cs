using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002030 RID: 8240
	public struct GlobalTargetInfo : IEquatable<GlobalTargetInfo>
	{
		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x0600AEB8 RID: 44728 RVA: 0x00071AC0 File Offset: 0x0006FCC0
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid || this.worldObjectInt != null || this.tileInt >= 0;
			}
		}

		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x0600AEB9 RID: 44729 RVA: 0x00071AED File Offset: 0x0006FCED
		public bool IsMapTarget
		{
			get
			{
				return this.HasThing || this.cellInt.IsValid;
			}
		}

		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x0600AEBA RID: 44730 RVA: 0x00071B04 File Offset: 0x0006FD04
		public bool IsWorldTarget
		{
			get
			{
				return this.HasWorldObject || this.tileInt >= 0;
			}
		}

		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x0600AEBB RID: 44731 RVA: 0x00071B1C File Offset: 0x0006FD1C
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x0600AEBC RID: 44732 RVA: 0x00071B27 File Offset: 0x0006FD27
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x0600AEBD RID: 44733 RVA: 0x00071B2F File Offset: 0x0006FD2F
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x0600AEBE RID: 44734 RVA: 0x00071B46 File Offset: 0x0006FD46
		public bool HasWorldObject
		{
			get
			{
				return this.WorldObject != null;
			}
		}

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x0600AEBF RID: 44735 RVA: 0x00071B51 File Offset: 0x0006FD51
		public WorldObject WorldObject
		{
			get
			{
				return this.worldObjectInt;
			}
		}

		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x0600AEC0 RID: 44736 RVA: 0x00071B59 File Offset: 0x0006FD59
		public static GlobalTargetInfo Invalid
		{
			get
			{
				return new GlobalTargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x0600AEC1 RID: 44737 RVA: 0x00071B67 File Offset: 0x0006FD67
		public string Label
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.LabelShort;
				}
				if (this.worldObjectInt != null)
				{
					return this.worldObjectInt.LabelShort;
				}
				return "Location".Translate();
			}
		}

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x0600AEC2 RID: 44738 RVA: 0x00071BA0 File Offset: 0x0006FDA0
		public IntVec3 Cell
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.PositionHeld;
				}
				return this.cellInt;
			}
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x0600AEC3 RID: 44739 RVA: 0x00071BBC File Offset: 0x0006FDBC
		public Map Map
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.MapHeld;
				}
				return this.mapInt;
			}
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x0600AEC4 RID: 44740 RVA: 0x0032C810 File Offset: 0x0032AA10
		public int Tile
		{
			get
			{
				if (this.worldObjectInt != null)
				{
					return this.worldObjectInt.Tile;
				}
				if (this.tileInt >= 0)
				{
					return this.tileInt;
				}
				if (this.thingInt != null && this.thingInt.Tile >= 0)
				{
					return this.thingInt.Tile;
				}
				if (this.cellInt.IsValid && this.mapInt != null)
				{
					return this.mapInt.Tile;
				}
				return -1;
			}
		}

		// Token: 0x0600AEC5 RID: 44741 RVA: 0x00071BD8 File Offset: 0x0006FDD8
		public GlobalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		// Token: 0x0600AEC6 RID: 44742 RVA: 0x0032C888 File Offset: 0x0032AA88
		public GlobalTargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				Log.Warning("Constructed GlobalTargetInfo with cell=" + cell + " and a null map.", false);
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		// Token: 0x0600AEC7 RID: 44743 RVA: 0x00071C01 File Offset: 0x0006FE01
		public GlobalTargetInfo(WorldObject worldObject)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = worldObject;
			this.tileInt = -1;
		}

		// Token: 0x0600AEC8 RID: 44744 RVA: 0x00071C2A File Offset: 0x0006FE2A
		public GlobalTargetInfo(int tile)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = tile;
		}

		// Token: 0x0600AEC9 RID: 44745 RVA: 0x00071C53 File Offset: 0x0006FE53
		public static implicit operator GlobalTargetInfo(TargetInfo target)
		{
			if (target.HasThing)
			{
				return new GlobalTargetInfo(target.Thing);
			}
			return new GlobalTargetInfo(target.Cell, target.Map, false);
		}

		// Token: 0x0600AECA RID: 44746 RVA: 0x00071C7F File Offset: 0x0006FE7F
		public static implicit operator GlobalTargetInfo(Thing t)
		{
			return new GlobalTargetInfo(t);
		}

		// Token: 0x0600AECB RID: 44747 RVA: 0x00071C87 File Offset: 0x0006FE87
		public static implicit operator GlobalTargetInfo(WorldObject o)
		{
			return new GlobalTargetInfo(o);
		}

		// Token: 0x0600AECC RID: 44748 RVA: 0x0032C8E4 File Offset: 0x0032AAE4
		public static explicit operator LocalTargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had WorldObject " + targ.worldObjectInt, 134566, false);
				return LocalTargetInfo.Invalid;
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had tile " + targ.tileInt, 7833122, false);
				return LocalTargetInfo.Invalid;
			}
			if (!targ.IsValid)
			{
				return LocalTargetInfo.Invalid;
			}
			if (targ.thingInt != null)
			{
				return new LocalTargetInfo(targ.thingInt);
			}
			return new LocalTargetInfo(targ.cellInt);
		}

		// Token: 0x0600AECD RID: 44749 RVA: 0x0032C978 File Offset: 0x0032AB78
		public static explicit operator TargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had WorldObject " + targ.worldObjectInt, 134566, false);
				return TargetInfo.Invalid;
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had tile " + targ.tileInt, 7833122, false);
				return TargetInfo.Invalid;
			}
			if (!targ.IsValid)
			{
				return TargetInfo.Invalid;
			}
			if (targ.thingInt != null)
			{
				return new TargetInfo(targ.thingInt);
			}
			return new TargetInfo(targ.cellInt, targ.mapInt, false);
		}

		// Token: 0x0600AECE RID: 44750 RVA: 0x0032CA14 File Offset: 0x0032AC14
		public static explicit operator IntVec3(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had WorldObject " + targ.worldObjectInt, 134566, false);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had tile " + targ.tileInt, 7833122, false);
			}
			return targ.Cell;
		}

		// Token: 0x0600AECF RID: 44751 RVA: 0x0032CA98 File Offset: 0x0032AC98
		public static explicit operator Thing(GlobalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had WorldObject " + targ.worldObjectInt, 134566, false);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had tile " + targ.tileInt, 7833122, false);
			}
			return targ.thingInt;
		}

		// Token: 0x0600AED0 RID: 44752 RVA: 0x0032CB28 File Offset: 0x0032AD28
		public static explicit operator WorldObject(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had Thing " + targ.thingInt, 6324165, false);
			}
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had cell " + targ.cellInt, 631672, false);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had tile " + targ.tileInt, 7833122, false);
			}
			return targ.worldObjectInt;
		}

		// Token: 0x0600AED1 RID: 44753 RVA: 0x0032CBB8 File Offset: 0x0032ADB8
		public static bool operator ==(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			if (a.cellInt.IsValid || b.cellInt.IsValid)
			{
				return a.cellInt == b.cellInt && a.mapInt == b.mapInt;
			}
			if (a.WorldObject != null || b.WorldObject != null)
			{
				return a.WorldObject == b.WorldObject;
			}
			return (a.tileInt < 0 && b.tileInt < 0) || a.tileInt == b.tileInt;
		}

		// Token: 0x0600AED2 RID: 44754 RVA: 0x00071C8F File Offset: 0x0006FE8F
		public static bool operator !=(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x0600AED3 RID: 44755 RVA: 0x00071C9B File Offset: 0x0006FE9B
		public override bool Equals(object obj)
		{
			return obj is GlobalTargetInfo && this.Equals((GlobalTargetInfo)obj);
		}

		// Token: 0x0600AED4 RID: 44756 RVA: 0x00071CB3 File Offset: 0x0006FEB3
		public bool Equals(GlobalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x0600AED5 RID: 44757 RVA: 0x0032CC70 File Offset: 0x0032AE70
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			if (this.cellInt.IsValid)
			{
				return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
			}
			if (this.worldObjectInt != null)
			{
				return this.worldObjectInt.GetHashCode();
			}
			if (this.tileInt >= 0)
			{
				return this.tileInt;
			}
			return -1;
		}

		// Token: 0x0600AED6 RID: 44758 RVA: 0x0032CCE0 File Offset: 0x0032AEE0
		public override string ToString()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetUniqueLoadID();
			}
			if (this.cellInt.IsValid)
			{
				return this.cellInt.ToString() + ", " + ((this.mapInt != null) ? this.mapInt.GetUniqueLoadID() : "null");
			}
			if (this.worldObjectInt != null)
			{
				return "@" + this.worldObjectInt.GetUniqueLoadID();
			}
			if (this.tileInt >= 0)
			{
				return this.tileInt.ToString();
			}
			return "null";
		}

		// Token: 0x040077E0 RID: 30688
		private Thing thingInt;

		// Token: 0x040077E1 RID: 30689
		private IntVec3 cellInt;

		// Token: 0x040077E2 RID: 30690
		private Map mapInt;

		// Token: 0x040077E3 RID: 30691
		private WorldObject worldObjectInt;

		// Token: 0x040077E4 RID: 30692
		private int tileInt;

		// Token: 0x040077E5 RID: 30693
		public const char WorldObjectLoadIDMarker = '@';
	}
}
