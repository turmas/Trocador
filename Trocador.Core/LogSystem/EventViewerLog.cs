﻿using Dlp.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trocador.Core.LogSystem {
	class EventViewerLog : AbstractLog {
		public override void Save(string logCategory, string methodName, object objectToLog) {
			string sSource;
			string sLog;
			string sEvent;
			string serializedData = Serializer.JsonSerialize(objectToLog);

			sSource = methodName;
			sLog = "Application";
			sEvent = string.Format("{0}|{1}|{2}|{3}", DateTime.UtcNow, logCategory, methodName, serializedData);

			if (EventLog.SourceExists(sSource) == false) { 
				EventLog.CreateEventSource(sSource, sLog);
			}

			EventLogEntryType eventLogEntryType = new EventLogEntryType();

			switch (logCategory) {
				case "Response":
				case "Request":
					eventLogEntryType = EventLogEntryType.Information;
					break;
				case "Exception":
					eventLogEntryType = EventLogEntryType.Error;
					break;
				default:
					eventLogEntryType = EventLogEntryType.Warning;
					break;
			}

			EventLog.WriteEntry(sSource, sEvent, eventLogEntryType, 666);
		}
	}
}
