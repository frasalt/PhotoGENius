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
        public void TestInputFile()
        {
            //creo lo stream partendo dai byte
            byte[] Array = Encoding.ASCII.GetBytes("abc   \nd\nef");
            MemoryStream memorystream = new MemoryStream(Array);
            InputStream stream = new InputStream(memorystream);

            Assert.True(stream.Location.LineNum == 1);
            Assert.True(stream.Location.ColNum == 1);

            Assert.True(stream.ReadChar() == 'a');
            Assert.True(stream.Location.LineNum == 1);
            Assert.True(stream.Location.ColNum == 2);
            
            stream.UnreadChar('A');
            Assert.True(stream.Location.LineNum == 1);
            Assert.True(stream.Location.ColNum == 1);
            /*
            Assert.True(stream.ReadChar() == 'A');
            Assert.True(stream.Location.LineNum==1);
            Assert.True(stream.Location.LineNum==2);
*/
           
        }
        
    }
}