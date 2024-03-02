namespace System.Throw
{
    internal class EventLine : IEquatable<EventLine>, IComparable<EventLine>
    {
        public EventLine(int severity, string message)
        {
            Severity = severity;
            Message = message;
        }

        public int Severity { get; }

        public string Message { get; }

        public int CompareTo(EventLine other)
        {
            if (other is null || other.Severity < Severity) return 1;
            if (other.Severity > Severity) return -1;
            return 0;
        }

        public bool Equals(EventLine other)
        {
            if (other is null) return false;
            return other.Severity == Severity;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EventLine);
        }

        public override string ToString()
        {
            return $"{Severity}: {Message}";
        }

        public override int GetHashCode()
        {
            return Severity ^ Message.GetHashCode();
        }
    }
}
