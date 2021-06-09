using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007DC RID: 2012
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06003288 RID: 12936 RVA: 0x00027961 File Offset: 0x00025B61
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06003289 RID: 12937 RVA: 0x00027978 File Offset: 0x00025B78
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x0600328A RID: 12938 RVA: 0x00027983 File Offset: 0x00025B83
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x0600328B RID: 12939 RVA: 0x0002798B File Offset: 0x00025B8B
		public Pawn Pawn
		{
			get
			{
				return this.Thing as Pawn;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x0600328C RID: 12940 RVA: 0x00027998 File Offset: 0x00025B98
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x000279AF File Offset: 0x00025BAF
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600328E RID: 12942 RVA: 0x000279BB File Offset: 0x00025BBB
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

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600328F RID: 12943 RVA: 0x000279E0 File Offset: 0x00025BE0
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

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06003290 RID: 12944 RVA: 0x0014D95C File Offset: 0x0014BB5C
		public Vector3 CenterVector3
		{
			get
			{
				if (this.thingInt != null)
				{
					if (this.thingInt.Spawned)
					{
						return this.thingInt.DrawPos;
					}
					if (this.thingInt.SpawnedOrAnyParentSpawned)
					{
						return this.thingInt.PositionHeld.ToVector3Shifted();
					}
					return this.thingInt.Position.ToVector3Shifted();
				}
				else
				{
					if (this.cellInt.IsValid)
					{
						return this.cellInt.ToVector3Shifted();
					}
					return default(Vector3);
				}
			}
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x000279FC File Offset: 0x00025BFC
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x00027A10 File Offset: 0x00025C10
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00027A20 File Offset: 0x00025C20
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x00027A28 File Offset: 0x00025C28
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x00027A30 File Offset: 0x00025C30
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x00027A5C File Offset: 0x00025C5C
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x00027A92 File Offset: 0x00025C92
		public TargetInfo ToTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return TargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new TargetInfo(this.Thing);
			}
			return new TargetInfo(this.Cell, map, false);
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x00027AC3 File Offset: 0x00025CC3
		public GlobalTargetInfo ToGlobalTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new GlobalTargetInfo(this.Thing);
			}
			return new GlobalTargetInfo(this.Cell, map, false);
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x0014D9E4 File Offset: 0x0014BBE4
		public static bool operator ==(LocalTargetInfo a, LocalTargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || a.cellInt == b.cellInt;
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x00027AF4 File Offset: 0x00025CF4
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x00027B00 File Offset: 0x00025D00
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x00027B18 File Offset: 0x00025D18
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x00027B26 File Offset: 0x00025D26
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return this.cellInt.GetHashCode();
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x0014DA44 File Offset: 0x0014BC44
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return this.Thing.GetUniqueLoadID();
			}
			if (this.Cell.IsValid)
			{
				return this.Cell.ToString();
			}
			return "null";
		}

		// Token: 0x04002303 RID: 8963
		private Thing thingInt;

		// Token: 0x04002304 RID: 8964
		private IntVec3 cellInt;
	}
}
