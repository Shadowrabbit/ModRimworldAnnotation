using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x0200032D RID: 813
	public class ScribeSaver
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x00088E6D File Offset: 0x0008706D
		public string CurPath
		{
			get
			{
				return this.curPath;
			}
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x00088E78 File Offset: 0x00087078
		public void InitSaving(string filePath, string documentElementName)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitSaving() but current mode is " + Scribe.mode);
				Scribe.ForceStop();
			}
			if (this.curPath != null)
			{
				Log.Error("Current path is not null in InitSaving");
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
			}
			try
			{
				Scribe.mode = LoadSaveMode.Saving;
				this.saveStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = "\t";
				this.writer = XmlWriter.Create(this.saveStream, xmlWriterSettings);
				this.writer.WriteStartDocument();
				this.EnterNode(documentElementName);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init saving file: ",
					filePath,
					"\n",
					ex
				}));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x00088F70 File Offset: 0x00087170
		public void FinalizeSaving()
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Called FinalizeSaving() but current mode is " + Scribe.mode);
				return;
			}
			if (this.anyInternalException)
			{
				this.ForceStop();
				throw new Exception("Can't finalize saving due to internal exception. The whole file would be most likely corrupted anyway.");
			}
			try
			{
				if (this.writer != null)
				{
					this.ExitNode();
					this.writer.WriteEndDocument();
					this.writer.Flush();
					this.writer.Close();
					this.writer = null;
				}
				if (this.saveStream != null)
				{
					this.saveStream.Flush();
					this.saveStream.Close();
					this.saveStream = null;
				}
				Scribe.mode = LoadSaveMode.Inactive;
				this.savingForDebug = false;
				this.loadIDsErrorsChecker.CheckForErrorsAndClear();
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
				this.anyInternalException = false;
			}
			catch (Exception arg)
			{
				Log.Error("Exception in FinalizeLoading(): " + arg);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x00089078 File Offset: 0x00087278
		public void WriteElement(string elementName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteElemenet(), but writer is null.");
				return;
			}
			try
			{
				this.writer.WriteElementString(elementName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x000890C4 File Offset: 0x000872C4
		public void WriteAttribute(string attributeName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteAttribute(), but writer is null.");
				return;
			}
			try
			{
				this.writer.WriteAttributeString(attributeName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x00089110 File Offset: 0x00087310
		public string DebugOutputFor(IExposable saveable)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("DebugOutput needs current mode to be Inactive");
				return "";
			}
			string result;
			try
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
					xmlWriterSettings.Indent = true;
					xmlWriterSettings.IndentChars = "  ";
					xmlWriterSettings.OmitXmlDeclaration = true;
					try
					{
						using (this.writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
						{
							Scribe.mode = LoadSaveMode.Saving;
							this.savingForDebug = true;
							Scribe_Deep.Look<IExposable>(ref saveable, "saveable", Array.Empty<object>());
						}
						result = stringWriter.ToString();
					}
					finally
					{
						this.ForceStop();
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception while getting debug output: " + arg);
				this.ForceStop();
				result = "";
			}
			return result;
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00089208 File Offset: 0x00087408
		public bool EnterNode(string nodeName)
		{
			if (this.writer == null)
			{
				return false;
			}
			try
			{
				this.writer.WriteStartElement(nodeName);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
			return true;
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0008924C File Offset: 0x0008744C
		public void ExitNode()
		{
			if (this.writer == null)
			{
				return;
			}
			try
			{
				this.writer.WriteEndElement();
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0008928C File Offset: 0x0008748C
		public void ForceStop()
		{
			if (this.writer != null)
			{
				this.writer.Close();
				this.writer = null;
			}
			if (this.saveStream != null)
			{
				this.saveStream.Close();
				this.saveStream = null;
			}
			this.savingForDebug = false;
			this.loadIDsErrorsChecker.Clear();
			this.curPath = null;
			this.savedNodes.Clear();
			this.nextListElementTemporaryId = 0;
			this.anyInternalException = false;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x04001010 RID: 4112
		public DebugLoadIDsSavingErrorsChecker loadIDsErrorsChecker = new DebugLoadIDsSavingErrorsChecker();

		// Token: 0x04001011 RID: 4113
		public bool savingForDebug;

		// Token: 0x04001012 RID: 4114
		private Stream saveStream;

		// Token: 0x04001013 RID: 4115
		private XmlWriter writer;

		// Token: 0x04001014 RID: 4116
		private string curPath;

		// Token: 0x04001015 RID: 4117
		private HashSet<string> savedNodes = new HashSet<string>();

		// Token: 0x04001016 RID: 4118
		private int nextListElementTemporaryId;

		// Token: 0x04001017 RID: 4119
		private bool anyInternalException;
	}
}
