using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020007DD RID: 2013
	public class LookTargets : IExposable
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600329F RID: 12959 RVA: 0x0000C32E File Offset: 0x0000A52E
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x0014DA90 File Offset: 0x0014BC90
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x00027B4D File Offset: 0x00025D4D
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x060032A2 RID: 12962 RVA: 0x0014DAAC File Offset: 0x0014BCAC
		public GlobalTargetInfo PrimaryTarget
		{
			get
			{
				for (int i = 0; i < this.targets.Count; i++)
				{
					if (this.targets[i].IsValid)
					{
						return this.targets[i];
					}
				}
				if (this.targets.Count != 0)
				{
					return this.targets[0];
				}
				return GlobalTargetInfo.Invalid;
			}
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x00027B5D File Offset: 0x00025D5D
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, Array.Empty<object>());
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x00027B75 File Offset: 0x00025D75
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x00027B88 File Offset: 0x00025D88
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x00027BAC File Offset: 0x00025DAC
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x00027BD0 File Offset: 0x00025DD0
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x00027BF6 File Offset: 0x00025DF6
		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x00027C1A File Offset: 0x00025E1A
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x0014DB14 File Offset: 0x0014BD14
		public LookTargets(params GlobalTargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x0014DB58 File Offset: 0x0014BD58
		public LookTargets(IEnumerable<TargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				IList<TargetInfo> list = targets as IList<TargetInfo>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						this.targets.Add(list[i]);
					}
					return;
				}
				foreach (TargetInfo target in targets)
				{
					this.targets.Add(target);
				}
			}
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x0014DBF4 File Offset: 0x0014BDF4
		public LookTargets(params TargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x00027C3C File Offset: 0x00025E3C
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x00027C56 File Offset: 0x00025E56
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x00027C70 File Offset: 0x00025E70
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x00027C8A File Offset: 0x00025E8A
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x00027CA4 File Offset: 0x00025EA4
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x00027CBE File Offset: 0x00025EBE
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x00027CD8 File Offset: 0x00025ED8
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x00027CF2 File Offset: 0x00025EF2
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x00027CFA File Offset: 0x00025EFA
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x00027D02 File Offset: 0x00025F02
		public static implicit operator LookTargets(TargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x00027D25 File Offset: 0x00025F25
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x00027D2D File Offset: 0x00025F2D
		public static implicit operator LookTargets(GlobalTargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x00027D4B File Offset: 0x00025F4B
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x00027D53 File Offset: 0x00025F53
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x00027D5B File Offset: 0x00025F5B
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x00027D63 File Offset: 0x00025F63
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x00027D6B File Offset: 0x00025F6B
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x00027D73 File Offset: 0x00025F73
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x00027D7B File Offset: 0x00025F7B
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x00027D83 File Offset: 0x00025F83
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x0014DC3C File Offset: 0x0014BE3C
		public static bool SameTargets(LookTargets a, LookTargets b)
		{
			if (a == null)
			{
				return b == null || !b.Any;
			}
			if (b == null)
			{
				return a == null || !a.Any;
			}
			if (a.targets.Count != b.targets.Count)
			{
				return false;
			}
			for (int i = 0; i < a.targets.Count; i++)
			{
				if (a.targets[i] != b.targets[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x0014DCC0 File Offset: 0x0014BEC0
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x0014DCF8 File Offset: 0x0014BEF8
		private void AppendThingTargets<T>(IEnumerable<T> things) where T : Thing
		{
			if (things == null)
			{
				return;
			}
			IList<T> list = things as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in things)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x0014DD8C File Offset: 0x0014BF8C
		private void AppendWorldObjectTargets<T>(IEnumerable<T> worldObjects) where T : WorldObject
		{
			if (worldObjects == null)
			{
				return;
			}
			IList<T> list = worldObjects as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in worldObjects)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x0014DE20 File Offset: 0x0014C020
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}

		// Token: 0x04002305 RID: 8965
		public List<GlobalTargetInfo> targets;
	}
}
