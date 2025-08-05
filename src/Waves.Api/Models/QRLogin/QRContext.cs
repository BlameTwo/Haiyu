using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.QRLogin
{
    [JsonSerializable(typeof(ScanScreenModel))]
    [JsonSerializable(typeof(QRLoginResult))]
    [JsonSerializable(typeof(SMSModel))]
    [JsonSerializable(typeof(Data))]
    public partial class QRContext:JsonSerializerContext
    {
    }
}
