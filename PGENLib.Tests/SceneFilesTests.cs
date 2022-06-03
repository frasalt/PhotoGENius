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
            
            Assert.True(stream.ReadChar() == 'A');
            Assert.True(stream.Location.LineNum == 1);
            Assert.True(stream.Location.ColNum == 2);
            
            Assert.True(stream.ReadChar()== 'b');
            Assert.True(stream.Location.LineNum == 1);
            Assert.True(stream.Location.ColNum == 3);
            
            Assert.True(stream.ReadChar()== 'c');
            Assert.True(stream.Location.LineNum == 1);
            Assert.True(stream.Location.ColNum == 4);

            stream.SkipWhitespacesAndComments();
            
            Assert.True(stream.ReadChar() == 'd');
            Assert.True(stream.Location.LineNum == 2);
            Assert.True(stream.Location.ColNum == 2);

            Assert.True(stream.ReadChar()== '\n');
            Assert.True(stream.Location.LineNum == 3);
            Assert.True(stream.Location.ColNum == 1);

            Assert.True(stream.ReadChar()== 'e');
            Assert.True(stream.Location.LineNum == 3);
            Assert.True(stream.Location.ColNum == 2);

            Assert.True(stream.ReadChar()== 'f');
            Assert.True(stream.Location.LineNum == 3);
            Assert.True(stream.Location.ColNum == 3);
            
            Assert.True(stream.ReadChar() == '\0');

        }
        
    }
}

/*
           assert stream.read_char() == "b"
        assert stream.location.line_num == 1
        assert stream.location.col_num == 3        

*/