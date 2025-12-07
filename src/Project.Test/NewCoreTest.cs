using Microsoft.Extensions.DependencyInjection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Waves.Api.Helper;
using Waves.Api.Models.Wrappers;
using Waves.Core;
using Waves.Core.Common;
using Waves.Core.GameContext;
using Waves.Core.Services;

namespace Project.Test;

[TestClass()]
public class NewCoreTest
{
    

    public void InitService() { }

    [TestMethod]
    public async Task Test1()
    {
        //var str = KrKeyHelper.Xor(Convert.FromBase64String(File.ReadAllText(@"C:\Users\30140\Desktop\KRApp.conf")), 99);

        //var text2 = Encoding.UTF8.GetString(str);
        var result =  RecordHelper.GetGuaranteedRange(new List<Tuple<RecordCardItemWrapper, int, bool?>>()
        {
            new Tuple<RecordCardItemWrapper, int, bool?>(null,60,true),
            new Tuple<RecordCardItemWrapper, int, bool?>(null,60,false),
            new Tuple<RecordCardItemWrapper, int, bool?>(null,60,true),
            new Tuple<RecordCardItemWrapper, int, bool?>(null,60,false),
            new Tuple<RecordCardItemWrapper, int, bool?>(null,60,false)
        });
    }

}
