using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007E9 RID: 2025
	public struct TargetInfo : IEquatable<TargetInfo>
	{
		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x0600331E RID: 13086 RVA: 0x00028099 File Offset: 0x00026299
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x0600331F RID: 13087 RVA: 0x000280B0 File Offset: 0x000262B0
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06003320 RID: 13088 RVA: 0x000280BB File Offset: 0x000262BB
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06003321 RID: 13089 RVA: 0x000280C3 File Offset: 0x000262C3
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06003322 RID: 13090 RVA: 0x000280DA File Offset: 0x000262DA
		public static TargetInfo Invalid
		{
			get
			{
				return new TargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06003323 RID: 13091 RVA: 0x000280E8 File Offset: 0x000262E8
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

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06003324 RID: 13092 RVA: 0x0002810D File Offset: 0x0002630D
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

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06003325 RID: 13093 RVA: 0x0014F30C File Offset: 0x0014D50C
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

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06003326 RID: 13094 RVA: 0x0014F360 File Offset: 0x0014D560
		public Vector3 CenterVector3
		{
			get
			{
				return ((LocalTargetInfo)this).CenterVector3;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06003327 RID: 13095 RVA: 0x00028129 File Offset: 0x00026329
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

		// Token: 0x06003328 RID: 13096 RVA: 0x00028145 File Offset: 0x00026345
		public TargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x0014F380 File Offset: 0x0014D580
		public TargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				Log.Warning("Constructed TargetInfo with cell=" + cell + " and a null map.", false);
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x00028160 File Offset: 0x00026360
		public static implicit operator TargetInfo(Thing t)
		{
			return new TargetInfo(t);
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x00028168 File Offset: 0x00026368
		public static explicit operator LocalTargetInfo(TargetInfo t)
		{
			if (t.HasThing)
			{
				return new LocalTargetInfo(t.Thing);
			}
			return new LocalTargetInfo(t.Cell);
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x0002818C File Offset: 0x0002638C
		public static explicit operator IntVec3(TargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted TargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x000281B8 File Offset: 0x000263B8
		public static explicit operator Thing(TargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted TargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x0014F3CC File Offset: 0x0014D5CC
		public static bool operator ==(TargetInfo a, TargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || (a.cellInt == b.cellInt && a.mapInt == b.mapInt);
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000281EE File Offset: 0x000263EE
		public static bool operator !=(TargetInfo a, TargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x000281FA File Offset: 0x000263FA
		public override bool Equals(object obj)
		{
			return obj is TargetInfo && this.Equals((TargetInfo)obj);
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x00028212 File Offset: 0x00026412
		public bool Equals(TargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x00028220 File Offset: 0x00026420
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x0014F440 File Offset: 0x0014D640
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

		// Token: 0x04002338 RID: 9016
		private Thing thingInt;

		// Token: 0x04002339 RID: 9017
		private IntVec3 cellInt;

		// Token: 0x0400233A RID: 9018
		private Map mapInt;
	}
}
