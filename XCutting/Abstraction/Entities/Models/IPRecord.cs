using System.Collections;

namespace Bfar.XCutting.Abstractions.Entities.Models
{
    public sealed record IPRecord
    {
        public int HostBits { get; set; }
        public BitArray CurrentAddress { get; set; }
        public string FirstAddress { get; set; }
        public string LastAddress { get; set; }
        public byte[] Result { get; set; }
    }
}
