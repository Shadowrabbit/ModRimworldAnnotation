using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDD RID: 7133
	public abstract class Lesson : IExposable
	{
		// Token: 0x170018A2 RID: 6306
		// (get) Token: 0x06009CFD RID: 40189 RVA: 0x00068797 File Offset: 0x00066997
		protected float AgeSeconds
		{
			get
			{
				if (this.startRealTime < 0f)
				{
					this.startRealTime = Time.realtimeSinceStartup;
				}
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x170018A3 RID: 6307
		// (get) Token: 0x06009CFE RID: 40190 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170018A4 RID: 6308
		// (get) Token: 0x06009CFF RID: 40191 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170018A5 RID: 6309
		// (get) Token: 0x06009D00 RID: 40192 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06009D01 RID: 40193 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}

		// Token: 0x06009D02 RID: 40194 RVA: 0x000687BD File Offset: 0x000669BD
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06009D03 RID: 40195 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDeactivated()
		{
		}

		// Token: 0x06009D04 RID: 40196
		public abstract void LessonOnGUI();

		// Token: 0x06009D05 RID: 40197 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void LessonUpdate()
		{
		}

		// Token: 0x06009D06 RID: 40198 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		// Token: 0x06009D07 RID: 40199 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_Event(EventPack ep)
		{
		}

		// Token: 0x06009D08 RID: 40200 RVA: 0x000687CA File Offset: 0x000669CA
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		// Token: 0x170018A6 RID: 6310
		// (get) Token: 0x06009D09 RID: 40201 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040063E9 RID: 25577
		public float startRealTime = -999f;

		// Token: 0x040063EA RID: 25578
		public const float KnowledgeForAutoVanish = 0.2f;
	}
}
