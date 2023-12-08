using ProtoBuf;

namespace WatchDog
{
    [ProtoContract]
    public class MicroserviceInfo
    {
        [ProtoMember(1)]
        public Guid Guid { get; set; } = Guid.Empty;

        [ProtoMember(2)]
        public string? Name { get; set; } = string.Empty;

        [ProtoMember(3)]
        public string? Type { get; set; } = string.Empty;

        [ProtoMember(4)]
        public LastUpDate LastUpDate { get; set; } = LastUpDate.None;

        public MicroserviceInfo() { }
        public MicroserviceInfo(string name, String type, LastUpDate lastUpDate)
        {
            Name = name;
            Type = type;
            LastUpDate = lastUpDate;
        }

        public void GenerateGuid()
        {
            Guid = Guid.NewGuid();
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            MicroserviceInfo? info = obj as MicroserviceInfo;
            if (info == null) return false;
            return info.Guid == Guid;
        }
    }
}
