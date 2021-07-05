using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001743 RID: 5955
	public struct GlobalTargetInfo : IEquatable<GlobalTargetInfo>
	{
		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x0600898F RID: 35215 RVA: 0x0031652F File Offset: 0x0031472F
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid || this.worldObjectInt != null || this.tileInt >= 0;
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x06008990 RID: 35216 RVA: 0x0031655C File Offset: 0x0031475C
		public bool IsMapTarget
		{
			get
			{
				return this.HasThing || this.cellInt.IsValid;
			}
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x06008991 RID: 35217 RVA: 0x00316573 File Offset: 0x00314773
		public bool IsWorldTarget
		{
			get
			{
				return this.HasWorldObject || this.tileInt >= 0;
			}
		}

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x06008992 RID: 35218 RVA: 0x0031658B File Offset: 0x0031478B
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x06008993 RID: 35219 RVA: 0x00316596 File Offset: 0x00314796
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x06008994 RID: 35220 RVA: 0x0031659E File Offset: 0x0031479E
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x06008995 RID: 35221 RVA: 0x003165B5 File Offset: 0x003147B5
		public bool HasWorldObject
		{
			get
			{
				return this.WorldObject != null;
			}
		}

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x06008996 RID: 35222 RVA: 0x003165C0 File Offset: 0x003147C0
		public WorldObject WorldObject
		{
			get
			{
				return this.worldObjectInt;
			}
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x06008997 RID: 35223 RVA: 0x003165C8 File Offset: 0x003147C8
		public static GlobalTargetInfo Invalid
		{
			get
			{
				return new GlobalTargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x06008998 RID: 35224 RVA: 0x003165D6 File Offset: 0x003147D6
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

		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x06008999 RID: 35225 RVA: 0x0031660F File Offset: 0x0031480F
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

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x0600899A RID: 35226 RVA: 0x0031662B File Offset: 0x0031482B
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

		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x0600899B RID: 35227 RVA: 0x00316648 File Offset: 0x00314848
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

		// Token: 0x0600899C RID: 35228 RVA: 0x003166BD File Offset: 0x003148BD
		public GlobalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		// Token: 0x0600899D RID: 35229 RVA: 0x003166E8 File Offset: 0x003148E8
		public GlobalTargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				Log.Warning("Constructed GlobalTargetInfo with cell=" + cell + " and a null map.");
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		// Token: 0x0600899E RID: 35230 RVA: 0x00316741 File Offset: 0x00314941
		public GlobalTargetInfo(WorldObject worldObject)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = worldObject;
			this.tileInt = -1;
		}

		// Token: 0x0600899F RID: 35231 RVA: 0x0031676A File Offset: 0x0031496A
		public GlobalTargetInfo(int tile)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = tile;
		}

		// Token: 0x060089A0 RID: 35232 RVA: 0x00316793 File Offset: 0x00314993
		public static implicit operator GlobalTargetInfo(TargetInfo target)
		{
			if (target.HasThing)
			{
				return new GlobalTargetInfo(target.Thing);
			}
			return new GlobalTargetInfo(target.Cell, target.Map, false);
		}

		// Token: 0x060089A1 RID: 35233 RVA: 0x003167BF File Offset: 0x003149BF
		public static implicit operator GlobalTargetInfo(Thing t)
		{
			return new GlobalTargetInfo(t);
		}

		// Token: 0x060089A2 RID: 35234 RVA: 0x003167C7 File Offset: 0x003149C7
		public static implicit operator GlobalTargetInfo(WorldObject o)
		{
			return new GlobalTargetInfo(o);
		}

		// Token: 0x060089A3 RID: 35235 RVA: 0x003167D0 File Offset: 0x003149D0
		public static explicit operator LocalTargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had WorldObject " + targ.worldObjectInt, 134566);
				return LocalTargetInfo.Invalid;
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had tile " + targ.tileInt, 7833122);
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

		// Token: 0x060089A4 RID: 35236 RVA: 0x00316864 File Offset: 0x00314A64
		public static explicit operator TargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had WorldObject " + targ.worldObjectInt, 134566);
				return TargetInfo.Invalid;
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had tile " + targ.tileInt, 7833122);
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

		// Token: 0x060089A5 RID: 35237 RVA: 0x003168FC File Offset: 0x00314AFC
		public static explicit operator IntVec3(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had WorldObject " + targ.worldObjectInt, 134566);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had tile " + targ.tileInt, 7833122);
			}
			return targ.Cell;
		}

		// Token: 0x060089A6 RID: 35238 RVA: 0x0031697C File Offset: 0x00314B7C
		public static explicit operator Thing(GlobalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had cell " + targ.cellInt, 631672);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had WorldObject " + targ.worldObjectInt, 134566);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had tile " + targ.tileInt, 7833122);
			}
			return targ.thingInt;
		}

		// Token: 0x060089A7 RID: 35239 RVA: 0x00316A08 File Offset: 0x00314C08
		public static explicit operator WorldObject(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had Thing " + targ.thingInt, 6324165);
			}
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had cell " + targ.cellInt, 631672);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had tile " + targ.tileInt, 7833122);
			}
			return targ.worldObjectInt;
		}

		// Token: 0x060089A8 RID: 35240 RVA: 0x00316A94 File Offset: 0x00314C94
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

		// Token: 0x060089A9 RID: 35241 RVA: 0x00316B49 File Offset: 0x00314D49
		public static bool operator !=(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x060089AA RID: 35242 RVA: 0x00316B55 File Offset: 0x00314D55
		public override bool Equals(object obj)
		{
			return obj is GlobalTargetInfo && this.Equals((GlobalTargetInfo)obj);
		}

		// Token: 0x060089AB RID: 35243 RVA: 0x00316B6D File Offset: 0x00314D6D
		public bool Equals(GlobalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x060089AC RID: 35244 RVA: 0x00316B7C File Offset: 0x00314D7C
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

		// Token: 0x060089AD RID: 35245 RVA: 0x00316BEC File Offset: 0x00314DEC
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

		// Token: 0x04005733 RID: 22323
		private Thing thingInt;

		// Token: 0x04005734 RID: 22324
		private IntVec3 cellInt;

		// Token: 0x04005735 RID: 22325
		private Map mapInt;

		// Token: 0x04005736 RID: 22326
		private WorldObject worldObjectInt;

		// Token: 0x04005737 RID: 22327
		private int tileInt;

		// Token: 0x04005738 RID: 22328
		public const char WorldObjectLoadIDMarker = '@';
	}
}
