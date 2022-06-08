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
        
        private void AssertIsKeyword(Token token, KeywordList keyword)
        {
            Assert.True(token is KeywordToken);
            Assert.True(((KeywordToken)token).Keyword == keyword);
        }
        private void AssertIsIdentifier(Token token, string identifier)
        {
            Assert.True(token is IdentifierToken);
            Assert.True(((IdentifierToken)token).Identifier == identifier);
        }
        private void AssertIsSymbol(Token token, string symbol)
        {
            Assert.True(token is SymbolToken);
            Assert.True(((SymbolToken)token).Symbol == symbol);
        }
        
        private void AssertIsNumber(Token token, float number)
        {
            Assert.True(token is LiteralNumberToken);
            Assert.True(((LiteralNumberToken)token).Value == number);
        }
        
        private void AssertIsString(Token token, string frase)
        {
            Assert.True(token is StringToken);
            Assert.True(((StringToken)token).String == frase);
        }

        [Fact]
        public void TestLexer()
        {
            //la @ serve per una stringa letterale complessa
            
            string test = @" 
                # This is a comment
                # This is another comment
                new material sky_material(
                 diffuse(image(""my file.pfm"")),
                 <5.0, 500.0, 300.0 >
                ) # Comment at the end of the line";
            byte[] Array = Encoding.ASCII.GetBytes(test);
            MemoryStream memorystream = new MemoryStream(Array);
            InputStream stream = new InputStream(memorystream);
            AssertIsKeyword(memorystream.ReadToken(), KeywordList.New);
            AssertIsKeyword(InputStream.ReadToken(), KeywordList.Material);
            AssertIsIdentifier(InputStream.ReadToken(), "sky_material");

        }
        
    }
}