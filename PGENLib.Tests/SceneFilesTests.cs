/*
PhotoGENius : photorealistic images generation.
Copyright (C) 2022  Lamorte Teresa, Salteri Francesca, Zanetti Martino

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Xunit;
using System;
using System.IO;
using System.Text;

namespace PGENLib.Tests
{

    public class SceneFileTest
    {
        [Fact]
        public void TestInputFile()
        {
            //Create the stream starting from bytes
            byte[] array = Encoding.ASCII.GetBytes("abc   \nd\nef");
            MemoryStream memorystream = new MemoryStream(array);
            InputStream stream = new InputStream(memorystream);

            //Check if all functions work
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
        
        //Define some Assert funcions.
        public void AssertIsKeyword(Token token, KeywordList keyword)
        {
            Console.WriteLine("1");
            Assert.True(token is KeywordToken);
            Console.WriteLine("2");
            Assert.True(((KeywordToken)token).Keyword == keyword);
            Console.WriteLine("3");

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
            Assert.True(Math.Abs(((LiteralNumberToken)token).Value - number) < 10E-5);
        }
        
        private void AssertIsString(Token token, string frase)
        {
            Assert.True(token is StringToken);
            Assert.True(((StringToken)token).String == frase);
        }

        [Fact]
        public void TestLexer()
        {
            string test = @"
                # This is a comment 
                # This is another comment
                new material sky_material(
                 diffuse(image(""my file.pfm"")),
                 < 5.0, 500.0, 300.0 >
                ) # Comment at the end of the line";
            byte[] array = Encoding.ASCII.GetBytes(test);
            MemoryStream memoryStream = new MemoryStream(array);
            InputStream stream = new InputStream(memoryStream);
            
            AssertIsKeyword(stream.ReadToken(), KeywordList.New);
            AssertIsKeyword(stream.ReadToken(), KeywordList.Material);
            AssertIsIdentifier(stream.ReadToken(), "sky_material");
            
            memoryStream.Close();
        }
    }
}