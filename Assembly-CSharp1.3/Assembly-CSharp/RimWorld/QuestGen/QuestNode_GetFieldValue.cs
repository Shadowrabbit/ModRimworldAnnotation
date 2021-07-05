using System;
using System.Reflection;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001671 RID: 5745
	public class QuestNode_GetFieldValue : QuestNode
	{
		// Token: 0x060085CA RID: 34250 RVA: 0x002FFACA File Offset: 0x002FDCCA
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060085CB RID: 34251 RVA: 0x002FFAD4 File Offset: 0x002FDCD4
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060085CC RID: 34252 RVA: 0x002FFAE4 File Offset: 0x002FDCE4
		private void SetVars(Slate slate)
		{
			object obj = (this.type.GetValue(slate) != null) ? ConvertHelper.Convert(this.obj.GetValue(slate), this.type.GetValue(slate)) : this.obj.GetValue(slate);
			FieldInfo fieldInfo = obj.GetType().GetField(this.field.GetValue(slate), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fieldInfo == null)
			{
				Log.Error("QuestNode error: " + obj.GetType().Name + " doesn't have a field named " + this.field.GetValue(slate));
				return;
			}
			slate.Set<object>(this.storeAs.GetValue(slate), fieldInfo.GetValue(obj), false);
		}

		// Token: 0x0400539F RID: 21407
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053A0 RID: 21408
		[NoTranslate]
		public SlateRef<object> obj;

		// Token: 0x040053A1 RID: 21409
		[NoTranslate]
		public SlateRef<string> field;

		// Token: 0x040053A2 RID: 21410
		public SlateRef<Type> type;
	}
}
