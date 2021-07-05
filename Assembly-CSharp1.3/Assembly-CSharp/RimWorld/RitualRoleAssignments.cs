using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8C RID: 3980
	public class RitualRoleAssignments : IExposable
	{
		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x06005E27 RID: 24103 RVA: 0x00204E0C File Offset: 0x0020300C
		public List<Pawn> SpectatorsForReading
		{
			get
			{
				return this.spectators;
			}
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06005E28 RID: 24104 RVA: 0x00204E14 File Offset: 0x00203014
		public Precept_Ritual Ritual
		{
			get
			{
				return this.ritual;
			}
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06005E29 RID: 24105 RVA: 0x00204E1C File Offset: 0x0020301C
		public List<Pawn> Participants
		{
			get
			{
				this.tmpParticipants.Clear();
				foreach (Pawn pawn in this.allPawns)
				{
					if (this.PawnParticipating(pawn))
					{
						this.tmpParticipants.Add(pawn);
					}
				}
				return this.tmpParticipants;
			}
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06005E2A RID: 24106 RVA: 0x00204E90 File Offset: 0x00203090
		public List<Pawn> AllPawns
		{
			get
			{
				return this.allPawns;
			}
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06005E2B RID: 24107 RVA: 0x00204E98 File Offset: 0x00203098
		public List<RitualRole> AllRolesForReading
		{
			get
			{
				return this.allRoles;
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06005E2C RID: 24108 RVA: 0x00204EA0 File Offset: 0x002030A0
		public List<Pawn> ExtraRequiredPawnsForReading
		{
			get
			{
				return this.requiredPawns;
			}
		}

		// Token: 0x06005E2D RID: 24109 RVA: 0x00204EA8 File Offset: 0x002030A8
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.allPawns, "allPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.spectators, "spectators", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.requiredPawns, "requiredPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<string, Pawn>(ref this.forcedRoles, "forcedRoles", LookMode.Value, LookMode.Reference, ref this.tmpForcedRoleIds, ref this.tmpForcedRolePawns);
			Scribe_Collections.Look<string, SerializablePawnList>(ref this.assignedRoles, "assignedRoles", LookMode.Value, LookMode.Deep, ref this.tmpAssignedRoleIds, ref this.tmpAssignedRolePawns);
			Scribe_References.Look<Precept_Ritual>(ref this.ritual, "ritual", false);
			Scribe_References.Look<Pawn>(ref this.selectedPawn, "selectedPawn", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.allRoles = ((this.ritual != null) ? new List<RitualRole>(this.ritual.behavior.def.roles) : new List<RitualRole>());
			}
		}

		// Token: 0x06005E2E RID: 24110 RVA: 0x00204F8C File Offset: 0x0020318C
		public RitualRoleAssignments()
		{
		}

		// Token: 0x06005E2F RID: 24111 RVA: 0x00204FB5 File Offset: 0x002031B5
		public RitualRoleAssignments(Precept_Ritual ritual)
		{
			this.ritual = ritual;
		}

		// Token: 0x06005E30 RID: 24112 RVA: 0x00204FE8 File Offset: 0x002031E8
		public void Setup(List<Pawn> allPawns, Dictionary<string, Pawn> forcedRoles = null, List<Pawn> requiredPawns = null, Pawn selectedPawn = null)
		{
			this.allPawns = allPawns;
			this.forcedRoles = forcedRoles;
			this.allRoles = ((this.ritual != null) ? new List<RitualRole>(this.ritual.behavior.def.roles) : new List<RitualRole>());
			this.selectedPawn = selectedPawn;
			this.requiredPawns = new List<Pawn>();
			if (forcedRoles != null)
			{
				this.requiredPawns.AddRange(forcedRoles.Values);
			}
			if (requiredPawns != null)
			{
				this.requiredPawns.AddRange(requiredPawns);
			}
			allPawns.SortBy((Pawn p) => p.Faction == null || !p.Faction.IsPlayer, (Pawn p) => !Faction.OfPlayer.ideos.Has(p.Ideo), (Pawn p) => !p.IsFreeNonSlaveColonist);
		}

		// Token: 0x06005E31 RID: 24113 RVA: 0x002050CB File Offset: 0x002032CB
		public bool Forced(Pawn pawn)
		{
			return this.forcedRoles != null && this.forcedRoles.ContainsValue(pawn);
		}

		// Token: 0x06005E32 RID: 24114 RVA: 0x002050E4 File Offset: 0x002032E4
		public string ForcedRole(Pawn pawn)
		{
			if (this.forcedRoles == null)
			{
				return null;
			}
			foreach (KeyValuePair<string, Pawn> keyValuePair in this.forcedRoles)
			{
				if (keyValuePair.Value == pawn)
				{
					return keyValuePair.Key;
				}
			}
			return null;
		}

		// Token: 0x06005E33 RID: 24115 RVA: 0x00205154 File Offset: 0x00203354
		public void RemoveParticipant(Pawn pawn)
		{
			this.TryUnassignAnyRole(pawn);
			this.spectators.Remove(pawn);
			this.allPawns.Remove(pawn);
			this.allPawns.Add(pawn);
		}

		// Token: 0x06005E34 RID: 24116 RVA: 0x00205184 File Offset: 0x00203384
		public bool TryUnassignAnyRole(Pawn pawn)
		{
			foreach (KeyValuePair<string, SerializablePawnList> keyValuePair in this.assignedRoles)
			{
				if (keyValuePair.Value.Pawns.Remove(pawn))
				{
					if (this.CanEverSpectate(pawn))
					{
						this.spectators.Add(pawn);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005E35 RID: 24117 RVA: 0x00205200 File Offset: 0x00203400
		public bool TryAssign(Pawn pawn, RitualRole role, out RitualRoleAssignments.FailReason failReason, Pawn insertAfter = null, Pawn insertBefore = null)
		{
			failReason = RitualRoleAssignments.FailReason.Undefined;
			if (this.forcedRoles != null && this.forcedRoles.ContainsValue(pawn))
			{
				return false;
			}
			if (this.PawnUnavailableReason(pawn, role) != null)
			{
				return false;
			}
			if (role == null)
			{
				return this.TryAssignSpectate(pawn, null);
			}
			string text;
			if (role.AppliesToPawn(pawn, out text, null, this, null))
			{
				if (role.maxCount <= 0 || this.AssignedPawns(role).Count<Pawn>() < role.maxCount)
				{
					this.TryUnassignAnyRole(pawn);
					this.spectators.Remove(pawn);
					SerializablePawnList serializablePawnList;
					if (!this.assignedRoles.TryGetValue(role.id, out serializablePawnList))
					{
						serializablePawnList = new SerializablePawnList(new List<Pawn>());
						this.assignedRoles.Add(role.id, serializablePawnList);
					}
					if (insertAfter != null && serializablePawnList.Pawns.Contains(insertAfter))
					{
						serializablePawnList.Pawns.Insert(serializablePawnList.Pawns.IndexOf(insertAfter) + 1, pawn);
					}
					else if (insertBefore != null && serializablePawnList.Pawns.Contains(insertBefore))
					{
						serializablePawnList.Pawns.Insert(serializablePawnList.Pawns.IndexOf(insertBefore), pawn);
					}
					else
					{
						serializablePawnList.Pawns.Add(pawn);
					}
					return true;
				}
				failReason = RitualRoleAssignments.FailReason.MaxPawnsAlreadyAssigned;
			}
			return false;
		}

		// Token: 0x06005E36 RID: 24118 RVA: 0x00205328 File Offset: 0x00203528
		public bool TryAssign(Pawn pawn, string roleId, out RitualRoleAssignments.FailReason failReason, Pawn insertAfter = null, Pawn insertBefore = null)
		{
			return this.TryAssign(pawn, this.GetRole(roleId), out failReason, insertAfter, insertBefore);
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x00205340 File Offset: 0x00203540
		public bool TryAssignSpectate(Pawn pawn, Pawn insertBefore = null)
		{
			if (this.spectators.Contains(pawn) || !this.CanEverSpectate(pawn) || this.PawnUnavailableReason(pawn, null) != null)
			{
				return false;
			}
			this.TryUnassignAnyRole(pawn);
			if (!this.spectators.Contains(pawn))
			{
				if (insertBefore != null && this.spectators.Contains(insertBefore))
				{
					this.spectators.Insert(this.spectators.IndexOf(insertBefore), pawn);
				}
				else
				{
					this.spectators.Add(pawn);
				}
			}
			return this.RoleForPawn(pawn, true) == null;
		}

		// Token: 0x06005E38 RID: 24120 RVA: 0x002053C8 File Offset: 0x002035C8
		public RitualRole GetRole(string roleId)
		{
			if (!this.AllRolesForReading.NullOrEmpty<RitualRole>())
			{
				foreach (RitualRole ritualRole in this.AllRolesForReading)
				{
					if (ritualRole.id == roleId)
					{
						return ritualRole;
					}
				}
			}
			return null;
		}

		// Token: 0x06005E39 RID: 24121 RVA: 0x00205438 File Offset: 0x00203638
		public Pawn FirstAssignedPawn(RitualRole role)
		{
			return this.FirstAssignedPawn(role.id);
		}

		// Token: 0x06005E3A RID: 24122 RVA: 0x00205448 File Offset: 0x00203648
		public Pawn FirstAssignedPawn(string roleId)
		{
			Pawn result;
			if (this.forcedRoles != null && this.forcedRoles.TryGetValue(roleId, out result))
			{
				return result;
			}
			SerializablePawnList serializablePawnList;
			if (this.assignedRoles.TryGetValue(roleId, out serializablePawnList) && serializablePawnList.Pawns.Count > 0)
			{
				return serializablePawnList.Pawns[0];
			}
			return null;
		}

		// Token: 0x06005E3B RID: 24123 RVA: 0x0020549C File Offset: 0x0020369C
		public bool CanEverSpectate(Pawn pawn)
		{
			return (this.ritual == null || !this.ritual.ritualOnlyForIdeoMembers || pawn.Ideo == this.ritual.ideo) && (this.ritual == null || this.ritual.behavior.def.spectatorFilter == null || this.ritual.behavior.def.spectatorFilter.Allowed(pawn)) && (pawn.RaceProps.Humanlike && !pawn.IsPrisoner) && GatheringsUtility.ShouldPawnKeepAttendingRitual(pawn, this.ritual, false);
		}

		// Token: 0x06005E3C RID: 24124 RVA: 0x00205535 File Offset: 0x00203735
		public IEnumerable<Pawn> SpectatorCandidates()
		{
			foreach (Pawn pawn in this.allPawns)
			{
				if (this.CanEverSpectate(pawn) && this.RoleForPawn(pawn, true) == null)
				{
					yield return pawn;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005E3D RID: 24125 RVA: 0x00205545 File Offset: 0x00203745
		public IEnumerable<Pawn> CandidatesForRole(string roleId, bool includeAssigned = false, bool includeAssignedForSameRole = false, bool includeForced = true)
		{
			return this.CandidatesForRole(this.GetRole(roleId), includeAssigned, includeAssignedForSameRole, includeForced);
		}

		// Token: 0x06005E3E RID: 24126 RVA: 0x00205558 File Offset: 0x00203758
		public IEnumerable<Pawn> CandidatesForRole(RitualRole role, bool includeAssigned = false, bool includeAssignedForSameRole = false, bool includeForced = true)
		{
			RitualRoleAssignments.<>c__DisplayClass43_0 CS$<>8__locals1;
			CS$<>8__locals1.includeAssigned = includeAssigned;
			CS$<>8__locals1.includeAssignedForSameRole = includeAssignedForSameRole;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.role = role;
			Pawn pawn;
			if (this.forcedRoles != null && this.forcedRoles.TryGetValue(CS$<>8__locals1.role.id, out pawn))
			{
				yield return pawn;
				yield break;
			}
			foreach (Pawn pawn2 in this.allPawns)
			{
				string text;
				if (CS$<>8__locals1.role.AppliesToPawn(pawn2, out text, null, this, null) && this.<CandidatesForRole>g__ShouldIncludePawn|43_0(pawn2, ref CS$<>8__locals1))
				{
					yield return pawn2;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005E3F RID: 24127 RVA: 0x0020557D File Offset: 0x0020377D
		public IEnumerable<Pawn> AssignedPawns(RitualRole role)
		{
			if (this.forcedRoles != null)
			{
				foreach (KeyValuePair<string, Pawn> keyValuePair in this.forcedRoles)
				{
					if (keyValuePair.Key == role.id)
					{
						yield return keyValuePair.Value;
					}
				}
				Dictionary<string, Pawn>.Enumerator enumerator = default(Dictionary<string, Pawn>.Enumerator);
			}
			SerializablePawnList serializablePawnList;
			if (this.assignedRoles.TryGetValue(role.id, out serializablePawnList))
			{
				foreach (Pawn pawn in serializablePawnList.Pawns)
				{
					yield return pawn;
				}
				List<Pawn>.Enumerator enumerator2 = default(List<Pawn>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06005E40 RID: 24128 RVA: 0x00205594 File Offset: 0x00203794
		public bool AnyPawnAssigned(string roleId)
		{
			return (this.forcedRoles != null && this.forcedRoles.ContainsKey(roleId)) || this.assignedRoles.ContainsKey(roleId);
		}

		// Token: 0x06005E41 RID: 24129 RVA: 0x002055BA File Offset: 0x002037BA
		public bool AnyPawnAssigned(RitualRole role)
		{
			return this.AnyPawnAssigned(role.id);
		}

		// Token: 0x06005E42 RID: 24130 RVA: 0x002055C8 File Offset: 0x002037C8
		public IEnumerable<Pawn> AssignedPawns(string roleId)
		{
			return this.AssignedPawns(this.GetRole(roleId));
		}

		// Token: 0x06005E43 RID: 24131 RVA: 0x002055D8 File Offset: 0x002037D8
		public RitualRole RoleForPawn(Pawn pawn, bool includeForced = true)
		{
			if (this.spectators.Contains(pawn))
			{
				return null;
			}
			if (includeForced && this.forcedRoles != null)
			{
				foreach (KeyValuePair<string, Pawn> keyValuePair in this.forcedRoles)
				{
					if (keyValuePair.Value == pawn)
					{
						return this.GetRole(keyValuePair.Key);
					}
				}
			}
			foreach (KeyValuePair<string, SerializablePawnList> keyValuePair2 in this.assignedRoles)
			{
				if (!keyValuePair2.Value.Pawns.NullOrEmpty<Pawn>() && keyValuePair2.Value.Pawns.Contains(pawn))
				{
					return this.GetRole(keyValuePair2.Key);
				}
			}
			return null;
		}

		// Token: 0x06005E44 RID: 24132 RVA: 0x002056D4 File Offset: 0x002038D4
		public bool PawnParticipating(Pawn pawn)
		{
			return this.RoleForPawn(pawn, true) != null || this.PawnSpectating(pawn);
		}

		// Token: 0x06005E45 RID: 24133 RVA: 0x002056E9 File Offset: 0x002038E9
		public bool PawnSpectating(Pawn pawn)
		{
			return this.spectators.Contains(pawn);
		}

		// Token: 0x06005E46 RID: 24134 RVA: 0x002056F8 File Offset: 0x002038F8
		public void FillPawns(Func<Pawn, bool, bool, bool> filter)
		{
			if (!this.requiredPawns.NullOrEmpty<Pawn>())
			{
				foreach (Pawn pawn in this.requiredPawns)
				{
					if (this.forcedRoles == null || !this.forcedRoles.ContainsValue(pawn))
					{
						this.TryAssignSpectate(pawn, null);
					}
				}
			}
			if (this.selectedPawn != null && this.RoleForPawn(this.selectedPawn, true) == null)
			{
				foreach (RitualRole ritualRole in this.AllRolesForReading)
				{
					string text;
					if (ritualRole.defaultForSelectedColonist && ritualRole.AppliesToPawn(this.selectedPawn, out text, null, this, null))
					{
						RitualRoleAssignments.FailReason failReason;
						this.TryAssign(this.selectedPawn, ritualRole, out failReason, null, null);
						break;
					}
				}
			}
			using (List<RitualRole>.Enumerator enumerator2 = this.AllRolesForReading.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RitualRole role = enumerator2.Current;
					IEnumerable<Pawn> enumerable = from p in this.allPawns
					where filter == null || filter(p, !(role is RitualRoleForced), role.allowOtherIdeos)
					select p;
					if (role.precept != null)
					{
						enumerable = enumerable.OrderBy(delegate(Pawn p)
						{
							Ideo ideo = p.Ideo;
							Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
							if (precept_Role == null || precept_Role.def != role.precept)
							{
								return 1;
							}
							return -1;
						});
					}
					foreach (Pawn pawn2 in enumerable)
					{
						if (this.RoleForPawn(pawn2, true) == null)
						{
							if (role.maxCount > 0 && this.AssignedPawns(role).Count<Pawn>() >= role.maxCount)
							{
								break;
							}
							string text;
							if (role.AppliesToPawn(pawn2, out text, null, this, null))
							{
								RitualRoleAssignments.FailReason failReason;
								this.TryAssign(pawn2, role, out failReason, null, null);
							}
						}
					}
				}
			}
			foreach (Pawn pawn3 in this.SpectatorCandidates())
			{
				this.TryAssignSpectate(pawn3, null);
			}
			List<Pawn> pawnsToRemove = new List<Pawn>();
			foreach (Pawn pawn4 in this.allPawns)
			{
				RitualRole ritualRole2 = this.RoleForPawn(pawn4, true);
				if (ritualRole2 != null && ritualRole2.required && ritualRole2.substitutable && this.PawnUnavailableReason(pawn4, ritualRole2) != null)
				{
					this.RemoveParticipant(pawn4);
					pawnsToRemove.Add(pawn4);
				}
			}
			this.allPawns.RemoveAll((Pawn p) => pawnsToRemove.Contains(p));
		}

		// Token: 0x06005E47 RID: 24135 RVA: 0x00205A18 File Offset: 0x00203C18
		public bool Required(Pawn pawn)
		{
			return !this.requiredPawns.NullOrEmpty<Pawn>() && this.requiredPawns.Contains(pawn);
		}

		// Token: 0x06005E48 RID: 24136 RVA: 0x00205A38 File Offset: 0x00203C38
		public bool RoleSubstituted(string roleId)
		{
			if (this.ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
			{
				return false;
			}
			RitualRole role = this.ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == roleId);
			if (role == null)
			{
				return false;
			}
			if (!role.substitutable || role.precept == null)
			{
				return false;
			}
			Precept precept = this.ritual.ideo.PreceptsListForReading.First((Precept p) => p.def == role.precept);
			bool result = false;
			foreach (Pawn pawn in this.AssignedPawns(roleId))
			{
				Ideo ideo = pawn.Ideo;
				if (((ideo != null) ? ideo.GetRole(pawn) : null) != precept)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06005E49 RID: 24137 RVA: 0x00205B44 File Offset: 0x00203D44
		public string PawnUnavailableReason(Pawn p, RitualRole role)
		{
			bool flag;
			return RitualRoleAssignments.PawnUnavailableReason(p, role, this.ritual, this, out flag);
		}

		// Token: 0x06005E4A RID: 24138 RVA: 0x00205B64 File Offset: 0x00203D64
		public static string PawnUnavailableReason(Pawn p, RitualRole role, Precept_Ritual ritual, RitualRoleAssignments assignments, out bool stillAddToPawnList)
		{
			stillAddToPawnList = false;
			if (p.Downed)
			{
				return "MessageRitualPawnDowned".Translate(p);
			}
			if (p.health.hediffSet.BleedRateTotal > 0f && (role == null || !role.ignoreBleeding))
			{
				return "MessageRitualPawnInjured".Translate(p);
			}
			if (p.InAggroMentalState || (role != null && !role.allowNonAggroMentalState && p.InMentalState))
			{
				return "MessageRitualPawnMentalState".Translate(p);
			}
			if (p.IsPrisoner && role == null)
			{
				string key = "MessageRitualRoleMustNotBePrisonerToSpectate";
				string text;
				if (ritual == null)
				{
					text = null;
				}
				else
				{
					RitualBehaviorWorker behavior = ritual.behavior;
					text = ((behavior != null) ? behavior.def.spectatorGerund : null);
				}
				return key.Translate(text ?? "Spectate".Translate());
			}
			if (p.IsPrisoner)
			{
				if (p.guest.Released)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnReleased".Translate(p);
				}
				if (!p.guest.PrisonerIsSecure)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnPrisonerNotSecured".Translate(p);
				}
			}
			if (p.IsSlave)
			{
				if (p.guest.Released)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnReleased".Translate(p);
				}
				if (!p.guest.SlaveIsSecure)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnSlaveNotSecured".Translate(p);
				}
			}
			if (role == null && !p.RaceProps.Humanlike)
			{
				return "MessageRitualRoleMustBeHumanlike".Translate("Spectators".Translate());
			}
			if (role == null && ritual != null && ritual.ritualOnlyForIdeoMembers && p.Ideo != ritual.ideo)
			{
				string key2 = "MessageRitualRoleMustHaveIdeoToSpectate";
				NamedArgument arg = ritual.ideo.MemberNamePlural;
				string text2;
				if (ritual == null)
				{
					text2 = null;
				}
				else
				{
					RitualBehaviorWorker behavior2 = ritual.behavior;
					text2 = ((behavior2 != null) ? behavior2.def.spectatorGerund : null);
				}
				return key2.Translate(arg, text2 ?? "Spectate".Translate());
			}
			string result;
			if (role != null && !role.AppliesToPawn(p, out result, null, assignments, null))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06005E4B RID: 24139 RVA: 0x00205DAC File Offset: 0x00203FAC
		[CompilerGenerated]
		private bool <CandidatesForRole>g__ShouldIncludePawn|43_0(Pawn pawn, ref RitualRoleAssignments.<>c__DisplayClass43_0 A_2)
		{
			return (A_2.includeAssigned || (A_2.includeAssignedForSameRole && this.RoleForPawn(pawn, true) == A_2.role) || this.RoleForPawn(pawn, true) == null) && GatheringsUtility.ShouldPawnKeepAttendingRitual(pawn, this.ritual, A_2.role != null && A_2.role.ignoreBleeding);
		}

		// Token: 0x04003662 RID: 13922
		private List<Pawn> allPawns;

		// Token: 0x04003663 RID: 13923
		private List<RitualRole> allRoles;

		// Token: 0x04003664 RID: 13924
		private Dictionary<string, Pawn> forcedRoles;

		// Token: 0x04003665 RID: 13925
		private Dictionary<string, SerializablePawnList> assignedRoles = new Dictionary<string, SerializablePawnList>();

		// Token: 0x04003666 RID: 13926
		private List<Pawn> spectators = new List<Pawn>();

		// Token: 0x04003667 RID: 13927
		private List<Pawn> requiredPawns;

		// Token: 0x04003668 RID: 13928
		private Precept_Ritual ritual;

		// Token: 0x04003669 RID: 13929
		private Pawn selectedPawn;

		// Token: 0x0400366A RID: 13930
		private List<Pawn> tmpParticipants = new List<Pawn>();

		// Token: 0x0400366B RID: 13931
		private List<Pawn> tmpForcedRolePawns;

		// Token: 0x0400366C RID: 13932
		private List<string> tmpForcedRoleIds;

		// Token: 0x0400366D RID: 13933
		private List<string> tmpAssignedRoleIds;

		// Token: 0x0400366E RID: 13934
		private List<SerializablePawnList> tmpAssignedRolePawns;

		// Token: 0x02002400 RID: 9216
		public enum FailReason
		{
			// Token: 0x04008914 RID: 35092
			MaxPawnsAlreadyAssigned,
			// Token: 0x04008915 RID: 35093
			Undefined
		}
	}
}
