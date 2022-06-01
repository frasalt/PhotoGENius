using System.Diagnostics;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Xunit;
using System;
using System.IO;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using PGENLib;

namespace PGENLib.Tests
{

    public class SceneFileTest
    {
        [Fact]
        public void TestImputFile()
        {
            //creo lo stream partendo dai byte
            byte[] Array = Encoding.ASCII.GetBytes("abc   \nd\nef");
            MemoryStream memorystream = new MemoryStream(Array);
            InputStream stream = new InputStream(memorystream);
            
            //Assert.True(stream.Location.LineNum == 1);
            Assert.True(stream.Location.ColNum == 1);
        }
        
        /*
        assert stream.location.line_num == 1
        assert stream.location.col_num == 1
        */
    }
}