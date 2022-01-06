using System;

namespace MidiTools.Core
{
	public class MidiNoteEventEventArgs : EventArgs
	{
		public MidiNoteEventType Type { get; set; }

		public int NoteNumber { get; set; }

		public long Timestamp { get; set; }

		public MidiNoteEventEventArgs(MidiNoteEventType type, int noteNumber, long timestamp)
		{
			this.Type = type;
			this.NoteNumber = noteNumber;
			this.Timestamp = timestamp;
		}
	}
}