using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200047E RID: 1150
	public class LookTargets : IExposable
	{
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x00002688 File Offset: 0x00000888
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x000DB954 File Offset: 0x000D9B54
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x000DB96F File Offset: 0x000D9B6F
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x000DB980 File Offset: 0x000D9B80
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

		// Token: 0x060022DC RID: 8924 RVA: 0x000DB9E5 File Offset: 0x000D9BE5
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, Array.Empty<object>());
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000DB9FD File Offset: 0x000D9BFD
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000DBA10 File Offset: 0x000D9C10
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000DBA34 File Offset: 0x000D9C34
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000DBA58 File Offset: 0x000D9C58
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000DBA7E File Offset: 0x000D9C7E
		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x000DBAA2 File Offset: 0x000D9CA2
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x000DBAC4 File Offset: 0x000D9CC4
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

		// Token: 0x060022E4 RID: 8932 RVA: 0x000DBB08 File Offset: 0x000D9D08
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

		// Token: 0x060022E5 RID: 8933 RVA: 0x000DBBA4 File Offset: 0x000D9DA4
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

		// Token: 0x060022E6 RID: 8934 RVA: 0x000DBBEA File Offset: 0x000D9DEA
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x000DBC04 File Offset: 0x000D9E04
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x000DBC1E File Offset: 0x000D9E1E
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x000DBC38 File Offset: 0x000D9E38
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x000DBC52 File Offset: 0x000D9E52
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000DBC6C File Offset: 0x000D9E6C
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x000DBC86 File Offset: 0x000D9E86
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000DBCA0 File Offset: 0x000D9EA0
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000DBCA8 File Offset: 0x000D9EA8
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x000DBCB0 File Offset: 0x000D9EB0
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

		// Token: 0x060022F0 RID: 8944 RVA: 0x000DBCD3 File Offset: 0x000D9ED3
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x000DBCDB File Offset: 0x000D9EDB
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

		// Token: 0x060022F2 RID: 8946 RVA: 0x000DBCF9 File Offset: 0x000D9EF9
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x000DBD01 File Offset: 0x000D9F01
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x000DBD09 File Offset: 0x000D9F09
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000DBD11 File Offset: 0x000D9F11
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000DBD19 File Offset: 0x000D9F19
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000DBD21 File Offset: 0x000D9F21
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000DBD29 File Offset: 0x000D9F29
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000DBD31 File Offset: 0x000D9F31
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x000DBD3C File Offset: 0x000D9F3C
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

		// Token: 0x060022FB RID: 8955 RVA: 0x000DBDC0 File Offset: 0x000D9FC0
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x000DBDF8 File Offset: 0x000D9FF8
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

		// Token: 0x060022FD RID: 8957 RVA: 0x000DBE8C File Offset: 0x000DA08C
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

		// Token: 0x060022FE RID: 8958 RVA: 0x000DBF20 File Offset: 0x000DA120
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}

		// Token: 0x040015DD RID: 5597
		public List<GlobalTargetInfo> targets;
	}
}
