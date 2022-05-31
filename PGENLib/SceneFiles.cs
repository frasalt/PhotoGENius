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

using System.Net.Mail;

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
        public SourceLocation shallowCopy()
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




    //A lexical token, used when parsing a scene file
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
    /// An error found by the lexer/parser while reading a scene file.
    /// The fields of this type are the following:
    /// <list type="table">
    /// <item>
    ///     <term>Message</term>
    ///     <description> a user-frendly error message</description>
    /// </item>
    /// <item>
    ///     <term>sourceLocation</term>
    ///     <description> a sourceLocation object pointing out the name of the file, the line number and the column
    ///     number where the error occured</description>
    /// </item>
    /// </list>
    /// </summary>
    class GrammarErrorException : Exception
    {
        public SourceLocation SourceLocation;

        public GrammarErrorException() : base()
        {
            SourceLocation = new SourceLocation();
        }

        public GrammarErrorException(string msg, SourceLocation sourceLocation) : base(msg)
        {
            SourceLocation = sourceLocation;
        }

        /// <summary>
        /// A high-level wrapper around a stream, used to parse scene files.
        /// This class implements a wrapper around a stream, with the following additional capabilities:
        /// - It tracks the line number and column number;
        /// - It permits to "un-read" characters and tokens.
        /// </summary>


        public class InputStream
        {
            public Stream stream;

            public SourceLocation location;

            public char savedChar;

            public SourceLocation savedLocation;

            public int tabulations;

            public Token? savedToken;

            /// <summary>
            /// Basic contructor for the class.
            /// </summary>
            public InputStream(Stream stream, SourceLocation location, char savedChar, SourceLocation savedLocation,
                int tabulations, Token? savedToken)
            {
                this.stream = stream;
                this.location = location;
                this.savedChar = savedChar;
                this.savedLocation = savedLocation;
                this.tabulations = tabulations;
                this.savedToken = savedToken;
            }

            /// <summary>
            /// Shift the cursor one position ahead.
            /// </summary>
            private void _updatePosition(char ch)
            {
                if (ch == '\0')
                    return;
                else if (ch == '\n')
                {
                    this.location.LineNum += 1;
                    this.location.ColNum = 1;
                }
                else if (ch == '\t')
                    this.location.ColNum += this.tabulations;
                else
                    this.location.ColNum += 1;
            }

            /// <summary>
            /// Read a new character from the stream
            /// </summary>
            public char readChar()
            {
                char ch;
                if (this.savedChar != '\0')
                {
                    ch = this.savedChar;
                    this.savedChar = '\0';
                }
                else
                {
                    int byteRead = this.stream.ReadByte();
                    if (byteRead == -1)
                        ch = '\0';
                    else
                        ch = Convert.ToChar(byteRead);
                }

                this.savedLocation = this.location.shallowCopy();
                this._updatePosition(ch);
                return ch;
            }

            /// <summary>
            /// Push a character back to the stream
            /// </summary>
            public char unreadChar(char ch)
            {
                this.savedChar = ch;
                this.location = this.savedLocation;
            }

            /// <summary>
            /// Push a character back to the stream
            /// </summary>
            public void SkipWhitespacesAndComments()
            {
                char ch = this.readChar();
                while (ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r' || ch == '#')
                {
                    if (ch == '#')
                    {
                        while (ch != '\r' || ch != '\n' || ch != ' ')
                        {
                            continue;
                        }

                    }

                    ch = this.readChar();
                    if (ch == ' ')
                    {
                        return;
                    }
                }

                unreadChar(ch);
            }
            

        }

    }
}





