using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000486 RID: 1158
	public struct TargetInfo : IEquatable<TargetInfo>
	{
		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x000DD4A8 File Offset: 0x000DB6A8
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x000DD4BF File Offset: 0x000DB6BF
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x000DD4CA File Offset: 0x000DB6CA
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x000DD4D2 File Offset: 0x000DB6D2
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x000DD4E9 File Offset: 0x000DB6E9
		public static TargetInfo Invalid
		{
			get
			{
				return new TargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x000DD4F7 File Offset: 0x000DB6F7
		public bool Fogged
		{
			get
			{
				if (!this.HasThing)
				{
					return this.cellInt.Fogged(this.mapInt);
				}
				return this.thingInt.Fogged();
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x000DD51E File Offset: 0x000DB71E
		public string Label
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.LabelShort;
				}
				return "Location".Translate();
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x000DD543 File Offset: 0x000DB743
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

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x000DD560 File Offset: 0x000DB760
		public int Tile
		{
			get
			{
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

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600234D RID: 9037 RVA: 0x000DD5B4 File Offset: 0x000DB7B4
		public Vector3 CenterVector3
		{
			get
			{
				return ((LocalTargetInfo)this).CenterVector3;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x000DD5D4 File Offset: 0x000DB7D4
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

		// Token: 0x0600234F RID: 9039 RVA: 0x000DD5F0 File Offset: 0x000DB7F0
		public TargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000DD60B File Offset: 0x000DB80B
		public TargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				Log.Warning("Constructed TargetInfo with cell=" + cell + " and a null map.");
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x000DD64B File Offset: 0x000DB84B
		public static implicit operator TargetInfo(Thing t)
		{
			return new TargetInfo(t);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000DD653 File Offset: 0x000DB853
		public static explicit operator LocalTargetInfo(TargetInfo t)
		{
			if (t.HasThing)
			{
				return new LocalTargetInfo(t.Thing);
			}
			return new LocalTargetInfo(t.Cell);
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000DD677 File Offset: 0x000DB877
		public static explicit operator IntVec3(TargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted TargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165);
			}
			return targ.Cell;
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x000DD6A2 File Offset: 0x000DB8A2
		public static explicit operator Thing(TargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted TargetInfo to Thing but it had cell " + targ.cellInt, 631672);
			}
			return targ.thingInt;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000DD6D8 File Offset: 0x000DB8D8
		public static bool operator ==(TargetInfo a, TargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || (a.cellInt == b.cellInt && a.mapInt == b.mapInt);
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000DD749 File Offset: 0x000DB949
		public static bool operator !=(TargetInfo a, TargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x000DD755 File Offset: 0x000DB955
		public override bool Equals(object obj)
		{
			return obj is TargetInfo && this.Equals((TargetInfo)obj);
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000DD76D File Offset: 0x000DB96D
		public bool Equals(TargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000DD77B File Offset: 0x000DB97B
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000DD7B0 File Offset: 0x000DB9B0
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return this.Thing.GetUniqueLoadID();
			}
			if (this.Cell.IsValid)
			{
				return this.Cell.ToString() + ", " + ((this.mapInt != null) ? this.mapInt.GetUniqueLoadID() : "null");
			}
			return "null";
		}

		// Token: 0x04001604 RID: 5636
		private Thing thingInt;

		// Token: 0x04001605 RID: 5637
		private IntVec3 cellInt;

		// Token: 0x04001606 RID: 5638
		private Map mapInt;
	}
}
