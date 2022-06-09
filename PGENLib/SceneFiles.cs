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
using System.Diagnostics;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;


namespace PGENLib
{

    /// <summary>
    /// A specific position in a source file.
    /// </summary>
    public class SourceLocation
    {
        public string FileName;
        public int LineNum;
        public int ColNum;

        public SourceLocation(string fileName, int lineNum = 0, int colNum = 0)
        {
            FileName = fileName;
            LineNum = lineNum;
            ColNum = colNum;
        }

        public SourceLocation(int lineNum = 0, int colNum = 0)
        {
            FileName = new string("");
            LineNum = lineNum;
            ColNum = colNum;
        }

        /// <summary>
        /// Copy constructor for the class.
        /// </summary>
        /// <returns></returns>
        public SourceLocation ShallowCopy()
        {
            return (SourceLocation) this.MemberwiseClone();
        }

        /// <summary>
        /// Print a source location
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({LineNum}, {ColNum})";
        }

    }



    /// <summary>
    /// A lexical token, used when parsing a scene file
    /// </summary>
    public class Token
    {
        public SourceLocation Location;

        public Token(SourceLocation location)
        {
            Location = location;
        }
    }

    public class StopToken : Token
    {
        public StopToken(SourceLocation location) : base(location)
        {
        }
    }

    public enum KeywordList
    {
        New = 1,
        Material = 2,
        Plane = 3,
        Sphere = 4,
        Diffuse = 5,
        Specular = 6,
        Uniform = 7,
        Checkered = 8,
        Image = 9,
        Identity = 10,
        Translation = 11,
        RotationX = 12,
        RotationY = 13,
        RotationZ = 14,
        Scaling = 15,
        Camera = 16,
        Orthogonal = 17,
        Perspective = 18,

        //Cylinder
        Float = 19,
        Pointlight = 20
    }

    /// <summary>
    /// A token containing a keyword
    /// </summary>
    public class KeywordToken : Token
    {
        public KeywordList Keyword;

        public KeywordToken(SourceLocation location, KeywordList keyword) : base(location)
        {
            Keyword = keyword;
        }

        /// <summary>
        ///  Dictionary linking the keyword to its string identifier.
        /// </summary>
        public static Dictionary<string, KeywordList> dictionary = new Dictionary<string, KeywordList>()
        {

            {"new", KeywordList.New},
            {"material", KeywordList.Material},
            {"plane", KeywordList.Plane},
            {"sphere", KeywordList.Sphere},
            //{  "cylinder" , Keyword.Cylinder},
            {"diffuse", KeywordList.Diffuse},
            {"specular", KeywordList.Specular},
            {"uniform", KeywordList.Uniform},
            {"checkered", KeywordList.Checkered},
            {"image", KeywordList.Image},
            {"identity", KeywordList.Identity},
            {"translation", KeywordList.Translation},
            {"rotation_x", KeywordList.RotationX},
            {"rotation_y", KeywordList.RotationY},
            {"rotation_z", KeywordList.RotationZ},
            {"scaling", KeywordList.Scaling},
            {"camera", KeywordList.Camera},
            {"orthogonal", KeywordList.Orthogonal},
            {"perspective", KeywordList.Perspective},
            {"float", KeywordList.Float},
            {"pointlight", KeywordList.Pointlight}
        };

        public override string ToString()
        {
            return $"{Keyword}";
        }

    }

    /// <summary>
    /// A token containing an identifier
    /// </summary>
    public class IdentifierToken : Token
    {
        public string Identifier;


        public IdentifierToken(SourceLocation location, string identifier) : base(location)
        {
            Identifier = identifier;
        }

        public override string ToString()
        {
            return Identifier;
        }
    }

    /// <summary>
    /// A token containing a literal string.
    /// </summary>
    public class StringToken : Token
    {
        public string String;

        public StringToken(SourceLocation location, string s) : base(location)
        {
            String = s;
        }

        public override string ToString()
        {
            return String;
        }

    }

    /// <summary>
    /// A token containing a literal number.
    /// </summary>
    public class LiteralNumberToken : Token
    {
        public float Value;

        public LiteralNumberToken(SourceLocation location, float value) : base(location)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }

    /// <summary>
    /// A token containing a symbol (i.e., a variable name).
    /// </summary>
    public class SymbolToken : Token
    {
        public string Symbol;

        public SymbolToken(SourceLocation location, string symbol) : base(location)
        {
            Symbol = symbol;
        }

        public override string ToString()
        {
            return Symbol;
        }
    }
    

    /// <summary>
    /// A high-level wrapper around a stream, used to parse scene files.
    /// This class implements a wrapper around a stream, with the following additional capabilities:
    /// - It tracks the line number and column number;
    /// - It permits to "un-read" characters and tokens.
    /// </summary>
    public class InputStream
    {
        public Stream Stream;

        public SourceLocation Location;

        public char SavedChar;

        public SourceLocation SavedLocation;

        public int Tabulations;

        public Token? SavedToken;

        /// <summary>
        /// Basic contructor for the class.
        /// </summary>
        public InputStream(Stream stream, string fileName = " ", int tabulations = 4)
        {
            Stream = stream;
            Location = new SourceLocation(fileName: fileName, lineNum: 1, colNum: 1);
            SavedChar = '\0';
            SavedLocation = Location;
            Tabulations = tabulations;
            SavedToken = null;
        }

    

        /// <summary>
        /// Shift the cursor one position ahead.
        /// </summary>
        private void UpdatePosition(char ch)
        { 
            if (ch == '\0')
                return;
            else if (ch == '\n')
            { 
                Location.LineNum += 1;
                Location.ColNum = 1;
            }
            else if (ch == '\t')
                Location.ColNum += Tabulations;
            else
                Location.ColNum += 1;
        }

        /// <summary>
        /// Read a new character from the stream
        /// </summary>
        public char ReadChar()
        {
            char ch;
            if (SavedChar != '\0')
            { 
                ch = SavedChar; 
                SavedChar = '\0';
            }
            else
            {
                int byteRead = Stream.ReadByte();
                if (byteRead == -1)
                    ch = '\0';
                else
                    ch = Convert.ToChar(byteRead);
            }

            SavedLocation = Location.ShallowCopy();
            UpdatePosition(ch);
            return ch;
        }

        /// <summary>
        /// Push a character back to the stream.
        /// </summary>
        public void UnreadChar(char ch)
        {
            //Debug.Assert(SavedChar == ' ');
            SavedChar = ch;
            Location = SavedLocation;
        }

        /// <summary>
        /// Keep reading characters until a non-whitespace/non-comment character is found.
        /// </summary>
        public void SkipWhitespacesAndComments()
        {
            char ch = this.ReadChar();
            while (ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r' || ch == '#')
            {
                if (ch == '#')
                {
                    //It's a comment! Keep reading until the end of the line (include the case "", the end-of-file)
                    while (ch != '\r' || ch != '\n' || ch != ' ')
                    {
                        continue;
                    }
                }
                ch = this.ReadChar();
                if (ch == '\0')
                {
                    return;
                }
            }
            //Put the non-whitespace character back
            UnreadChar(ch);
            return;
        }

        public StringToken ParseStringToken(SourceLocation tokenLocation)
        {
            var token = "";
            while (true)
            {
                var ch = ReadChar();

                if (ch == '"') break;
                if (ch == ' ')
                {
                    throw new GrammarErrorException("Unterminated string", tokenLocation);
                }
            
                token += ch;
            }
            
            return new StringToken(tokenLocation, token);
        }

        public LiteralNumberToken ParseFloatToken(string firstChar, SourceLocation tokenLocation)
        {
            var token = firstChar;
            float value;
            while (true)
            {
                var ch = ReadChar();
                char[] E = { 'e', 'E' };
                if (Char.IsDigit(ch) || ch == '.' || E.Contains(ch))
                {
                    UnreadChar(ch);
                    break;
                }
                token += ch;
            }
            
            try
            {
                value = float.Parse(token);
            }
            catch
            {
                throw new GrammarErrorException($"'{token}' is an invalid floating-point number", tokenLocation);
            }

            return new LiteralNumberToken(tokenLocation, value);
        }
        
        private Token ParseKeywordOrIdentifierToken(char firstChar, SourceLocation tokenLocation)
        {
            string token = firstChar.ToString();
            while (true)
            {
                char ch = this.ReadChar();

                if (!(Char.IsLetterOrDigit(ch) || ch == '_'))
                {
                    this.UnreadChar(ch);
                    break;
                }
                token += ch;
            }

            try
            {
                return new KeywordToken(tokenLocation, KeywordToken.dictionary[token]);
            }
            catch (System.Exception)
            {
                return new IdentifierToken(tokenLocation, token);
            }

        }

        public Token ReadToken()
        {
            if (SavedToken!= null)
            {
                Token result = SavedToken;
                SavedToken = null;
                return result;
            }
            SkipWhitespacesAndComments();
            
            // Now ch does *not* contain a whitespace character.
            char ch = ReadChar();
            if (ch == '\0')
            {
                // No more characters in the file, so return a StopToken
                return new StopToken(Location);
            }
            //At this point we must check what kind of token begins with the "ch" character (which has been
            //put back in the stream with self.unread_char). First, we save the position in the stream.
            //SourceLocation tokenLocation = Location.ShallowCopy();
            char[] SYMB = { '(',')','<','>','[',']', '*' };
            char[] OP = {'+', '-', '.'};
            if (SYMB.Contains(ch))
            {
                return new SymbolToken(Location, ch.ToString());
            }
            else if (ch == '"')
            {
                // A literal string (used for file names)
                return ParseStringToken(Location);
            }
            else if (Char.IsDigit(ch)|| OP.Contains(ch))
            {
                // A floating-point number
                return ParseFloatToken(ch.ToString(), Location);
            }
            else if (Char.IsLetter(ch) || ch == '_')
            {
                // Since it begins with an alphabetic character, it must either be a keyword or a identifier
                return ParseKeywordOrIdentifierToken(ch, Location);
            }
            else
            {
                // We got some weird character, like '@` or `&`
                throw new GrammarErrorException("Invalid character {ch}", Location);
            }

        } 
        
    }

}





