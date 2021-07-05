using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047D RID: 1149
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x000DB636 File Offset: 0x000D9836
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x000DB64D File Offset: 0x000D984D
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060022C3 RID: 8899 RVA: 0x000DB658 File Offset: 0x000D9858
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x000DB660 File Offset: 0x000D9860
		public Pawn Pawn
		{
			get
			{
				return this.Thing as Pawn;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x000DB66D File Offset: 0x000D986D
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x000DB684 File Offset: 0x000D9884
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x000DB690 File Offset: 0x000D9890
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

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x000DB6B5 File Offset: 0x000D98B5
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

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060022C9 RID: 8905 RVA: 0x000DB6D4 File Offset: 0x000D98D4
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

		// Token: 0x060022CA RID: 8906 RVA: 0x000DB759 File Offset: 0x000D9959
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000DB76D File Offset: 0x000D996D
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000DB77D File Offset: 0x000D997D
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000DB785 File Offset: 0x000D9985
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000DB78D File Offset: 0x000D998D
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165);
			}
			return targ.Cell;
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000DB7B8 File Offset: 0x000D99B8
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672);
			}
			return targ.thingInt;
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000DB7ED File Offset: 0x000D99ED
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

		// Token: 0x060022D1 RID: 8913 RVA: 0x000DB81E File Offset: 0x000D9A1E
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

		// Token: 0x060022D2 RID: 8914 RVA: 0x000DB850 File Offset: 0x000D9A50
		public static bool operator ==(LocalTargetInfo a, LocalTargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || a.cellInt == b.cellInt;
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x000DB8AF File Offset: 0x000D9AAF
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x000DB8BB File Offset: 0x000D9ABB
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x000DB8D3 File Offset: 0x000D9AD3
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x000DB8E1 File Offset: 0x000D9AE1
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return this.cellInt.GetHashCode();
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000DB908 File Offset: 0x000D9B08
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

		// Token: 0x040015DB RID: 5595
		private Thing thingInt;

		// Token: 0x040015DC RID: 5596
		private IntVec3 cellInt;
	}
}
