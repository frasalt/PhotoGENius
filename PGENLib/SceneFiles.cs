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

namespace PGENLib
{

    /// <summary>
    /// The class points out a specific location in a source file.
    /// The fields are:
    /// <list type="table">
    /// <item>
    ///     <term>FileName</term>
    ///     <description> name of the file or empty string;</description>
    /// </item>
    /// <item>
    ///     <term>LineNum</term>
    ///     <description> number of line;</description>
    /// </item>
    /// <item>
    ///     <term>ColNum</term>
    ///     <description> number of column;</description>
    /// </item>
    /// </list>
    /// </summary>
    public class SourceLocation
    {
        public string FileName;
        public int LineNum;
        public int ColNum;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lineNum"></param>
        /// <param name="colNum"></param>
        public SourceLocation(string fileName, int lineNum = 0, int colNum = 0)
        {
            FileName = fileName;
            LineNum = lineNum;
            ColNum = colNum;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lineNum"></param>
        /// <param name="colNum"></param>
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
            return (SourceLocation) MemberwiseClone();
        }

        /// <summary>
        /// Print a source location.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({LineNum}, {ColNum})";
        }

    }
    
    /// <summary>
    /// A lexical token, used when parsing a scene file.
    /// </summary>
    public class Token
    {
        public SourceLocation Location;

        /// <summary>
        /// Constructor, with a `SourceLocation` object as input parameter.
        /// </summary>
        /// <param name="location"></param>
        public Token(SourceLocation location)
        {
            Location = location;
        }
    }
    
    /// <summary>
    /// A token representing the end of a file.
    /// </summary>
    public class StopToken : Token
    {
        public StopToken(SourceLocation location) : base(location)
        {
        }
    }

    /// <summary>
    /// Enumeration of all possible keywords encountered in a Scene File.
    /// </summary>
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
        Float = 19,
        Cylinder = 20,
        PointLight = 21
    }

    /// <summary>
    /// A token containing a keyword.
    /// Parameters:
    /// <list type="table">
    /// <item>
    ///     <term>Location</term>
    ///     <description> a `SourceLocation` object holding informations about token location in
    ///     the file (filename, number of column, number of rows)</description>
    /// </item>
    /// <item>
    ///     <term>Keyword</term>
    ///     <description> a `KeywordList` object holding the possible Keyword options</description>
    /// </item>
    /// </list>
    /// </summary>
    public class KeywordToken : Token
    {
        public KeywordList Keyword;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="keyword"></param>
        public KeywordToken(SourceLocation location, KeywordList keyword) : base(location)
        {
            Keyword = keyword;
        }

        /// <summary>
        ///  Dictionary linking the keyword to its string identifier.
        /// </summary>
        public static Dictionary<string, KeywordList> Dictionary = new Dictionary<string, KeywordList>
        {

            { "new", KeywordList.New },
            { "material", KeywordList.Material },
            { "plane", KeywordList.Plane },
            { "sphere", KeywordList.Sphere },
            { "cylinder" , KeywordList.Cylinder},
            { "diffuse", KeywordList.Diffuse },
            { "specular", KeywordList.Specular },
            { "uniform", KeywordList.Uniform },
            { "checkered", KeywordList.Checkered },
            { "image", KeywordList.Image },
            { "identity", KeywordList.Identity },
            { "translation", KeywordList.Translation },
            { "rotation_x", KeywordList.RotationX },
            { "rotation_y", KeywordList.RotationY },
            { "rotation_z", KeywordList.RotationZ },
            { "scaling", KeywordList.Scaling },
            { "camera", KeywordList.Camera },
            { "orthogonal", KeywordList.Orthogonal },
            { "perspective", KeywordList.Perspective },
            { "float", KeywordList.Float },
            { "pointlight", KeywordList.PointLight }

        };

        public override string ToString()
        {
            return $"{Keyword}";
        }

    }

    /// <summary>
    /// A token containing an identifier.
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
    /// A scene read from a scene file.
    /// </summary>
    public class Scene
    {
        public Dictionary<string, Material> Materials = new Dictionary<string, Material>();

        public World World = new World();

        public ICamera Camera; 

        public Dictionary<string, float> FloatVariables = new Dictionary<string, float>();

        public Dictionary<string, float>.KeyCollection OverriddenVariables = 
            new Dictionary<string, float>.KeyCollection(new Dictionary<string, float>()); // come faccio a indicare che devono essere stringhe?
    }


    /// <summary>
    /// This class implements a wrapper around a stream, with the following additional capabilities:
    /// <list type="number" >
    /// <item>
    ///     <description> It tracks the line number and column number;</description>
    /// </item>
    /// <item>
    ///     <description> It permits to "un-read" characters and tokens.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// Parameters:
    /// <list type="table">
    /// <item>
    ///     <term>Stream</term>
    ///     <description> a `Stream` object, used to read a file;</description>
    /// </item>
    /// <item>
    ///     <term>Location</term>
    ///     <description> a `SourceLocation` object;</description>
    /// </item>
    /// <item>
    ///     <term>SavedChar</term>
    ///     <description> a `char?` object, holding the un-read character or being null, if no char has been un-read;</description>
    /// </item>
    /// <item>
    ///     <term>SavedLocation</term>
    ///     <description> a `SourceLocation` object with the location of the un-read character or token;</description>
    /// </item>
    /// <item>
    ///     <term>Tabulation</term>
    ///     <description> a `Ray` object that hit the surface;</description>
    /// </item>
    /// <item>
    ///     <term>SavedToken</term>
    ///     <description> a `Token` object, holding the un-read token or being null, if no token has been un-read. </description>
    /// </item>
    /// </list>
    public class InputStream
    {
        public Stream Stream;

        public SourceLocation Location;

        public char SavedChar;

        public SourceLocation SavedLocation;

        public int Tabulations;

        public Token? SavedToken;

        /// <summary>
        /// Basic constructor for the class.
        /// </summary>
        public InputStream(Stream stream, string fileName = "", int tabulations = 4)
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
        /// <param name="ch"></param>
        private void UpdatePosition(char ch)
        {
            if (ch == '\0')
                return;
            if (ch == '\n')
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
        /// Read a new character from the stream.
        /// </summary>
        public char ReadChar()
        {
            char ch;
            if (SavedChar != '\0')
            {
                //Recover the un-read character and return it
                ch = SavedChar;
                SavedChar = '\0';
            }
            else
            {
                //Read a new character from the stream
                int byteRead = Stream.ReadByte();
                // if i read the end-of-file byte, set ch to \0
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
            Debug.Assert(SavedChar == '\0');
            SavedChar = ch;
            Location = SavedLocation;
        }

        /// <summary>
        /// Keep reading characters until a non-whitespace/non-comment character is found.
        /// </summary>
        public void SkipWhitespacesAndComments()
        {
            char ch = ReadChar();

            while (ch is ' ' or '\t' or '\n' or '\r' or '#' or '\0')

            {
                if (ch == '#')
                {
                    while (ch is not ('\r' or '\n' or '\0'))
                    {
                        ch = ReadChar();
                    }
                }

                ch = ReadChar();
                if (ch == '\0')
                {
                    return;
                }
            }

            //Put the non-whitespace character back
            UnreadChar(ch);
        }

        /// <summary>
        /// Read a StringToken in a given location.
        /// </summary>
        /// <param name="tokenLocation"> The location in the file</param>
        /// <returns> The StringToken we just read.</returns>
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

        /// <summary>
        /// Read a LiteralNumberToken in a given location.
        /// </summary>
        /// <param name="firstChar"></param>
        /// <param name="tokenLocation"></param>
        /// <returns></returns>
        /// <exception cref="GrammarErrorException"></exception>
        public LiteralNumberToken ParseFloatToken(string firstChar, SourceLocation tokenLocation)
        {
            var token = firstChar;
            float value;
            while (true)
            {
                var ch = ReadChar();
                char[] e = { 'e', 'E' };
                if (!(Char.IsDigit(ch) || ch == '.' || e.Contains(ch)))
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

        /// <summary>
        /// Read either a KeywordTokeN or a IdentifierToken in a given location.
        /// </summary>
        /// <param name="firstChar"></param>
        /// <param name="tokenLocation"></param>
        /// <returns></returns>
        private Token ParseKeywordOrIdentifierToken(char firstChar, SourceLocation tokenLocation)
        {
            string token = firstChar.ToString();
            while (true)
            {
                char ch = ReadChar();

                if (!(Char.IsLetterOrDigit(ch) || ch == '_'))
                {
                    UnreadChar(ch);
                    break;
                }

                token += ch;
            }

            try
            {
                return new KeywordToken(tokenLocation, KeywordToken.Dictionary[token]);
            }
            catch (Exception)
            {
                return new IdentifierToken(tokenLocation, token);
            }

        }

        /// <summary>
        /// Reads a token from the stream 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="GrammarErrorException"> a lexical error is found</exception>
        public Token ReadToken()
        {
            // If already saved, return it, and clean the savedToken member ...
            if (SavedToken != null)
            {
                Token result = SavedToken;
                SavedToken = null;
                return result;
            }

            // ... else go on and look for another token
            SkipWhitespacesAndComments();

            // Now ch does NOT contain a whitespace character.
            char ch = ReadChar();

            if (ch == '\0')
            {
                // No more characters in the file, so return a StopToken
                return new StopToken(Location);
            }

            // At this point we must check what kind of token begins
            // with the "ch" character (which has been
            // put back in the stream with self.unread_char).
            
            //We first save the position in the stream.
            SourceLocation tokenLocation = Location.ShallowCopy();

            //Now, which token begins with the "ch" character?
            char[] symb = {'(', ')', '<', '>', '[', ']',',', '*'};
            char[] op = {'+', '-', '.'};
            if (symb.Contains(ch))
            {
                return new SymbolToken(tokenLocation, ch.ToString());
            }

            if (ch == '"')
            {
                // A literal string (used for file names)
                return ParseStringToken(tokenLocation);
            }

            if (Char.IsDigit(ch) || op.Contains(ch))
            {
                // A floating-point number
                return ParseFloatToken(ch.ToString(), tokenLocation);
            }

            if (Char.IsLetter(ch) || ch == '_')
            {
                // Since it begins with an alphabetic character, it must either be a keyword or a identifier
                return ParseKeywordOrIdentifierToken(ch, tokenLocation);
            }

            // We got some weird character, like '@` or `&`
            throw new GrammarErrorException($"Invalid character {ch}", Location);
        }

        /// <summary>
        /// Make as if `token` were never read from `inputFile`
        /// </summary>
        /// <param name="token"></param>
        public void UnreadToken(Token token)
        {
            // <<<< NON CI PIAE CHE QUESTO ERRORE NON SIA SOTTO CONTROLLO
            Debug.Assert(SavedToken == null);
            SavedToken = token;
        }

    }

    
    public class ExpectParse
    {
        //=============== EXPECT ============================

        /// <summary>
        /// Read a token from `inputFile` and check that it matches `symbol`.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="symbol"></param>
        public static void expect_symbol(InputStream inputFile, string symbol)
        {
            Token token = inputFile.ReadToken();

            // if token is NOT a symble or the symble-token does NOT match the symble expected, throw error
            if (token is not SymbolToken || ((SymbolToken)token).Symbol != symbol)
            {
                throw new GrammarErrorException($"Got '{token}' instead of '{symbol}'", token.Location);
            }
        }

        /// <summary>
        /// Read a token from `inputFile` and check that it is one of the keywords in `keywords`.
        /// Return the keyword as a class :class:'.KeywordList` object.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static KeywordList expect_keywords(InputStream inputFile, List<KeywordList> keywords)
        {
            Token token = inputFile.ReadToken();

            // if token is NOT of type keyword, throw error (else go on ...)
            if (token is not KeywordToken)
            {
                throw new GrammarErrorException($"Expected a keyword instead of '{token}'", token.Location);
            }

            // (... go on) if the keyword associated to token is NOT in the list, throw error (else go on ...)
            if (!keywords.Contains(((KeywordToken)token).Keyword))
            {
                string str = "";
                foreach (string i in Enum.GetValues(typeof(KeywordList))) // <<<< o va fatto con List?
                {
                    str += i;
                }

                throw new GrammarErrorException(

                    $"Expected one of the keywords '{str}' instead of '{token}'", token.Location);
            }

            // (... go on) return the keyword!
            return ((KeywordToken)token).Keyword;
        }

        /// <summary>
        /// Read a token from `inputFile` and check that it is either a literal number or a variable in `scene`.
        /// Return the number as a ``float``.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static float expect_number(InputStream inputFile, Scene scene)
        {
            Token token = inputFile.ReadToken();

            // if token is a number, return it
            if (token is LiteralNumberToken)
            {
                return ((LiteralNumberToken)token).Value;
            }
            // if there is an identifier instead of a number, look for it and return it if it exists
            else if (token is IdentifierToken)
            {
                string variableName = ((IdentifierToken)token).Identifier;
                if (!scene.FloatVariables.ContainsKey(variableName))
                {
                    throw new GrammarErrorException($"Unknown variable '{token}'", token.Location);
                }

                return scene.FloatVariables[variableName];
            }

            // else throw error
            throw new GrammarErrorException($"Got '{token}' instead of a number", token.Location);
        }

        /// <summary>
        /// Read a token from `inputFile` and check that it is a literal string.
        /// Return the value of the string(a ``str``).
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns></returns>
        public static string expect_string(InputStream inputFile)
        {
            Token token = inputFile.ReadToken();

            if (token is not StringToken)
            {
                throw new GrammarErrorException($"Got '{token}' instead of a string", token.Location);
            }

            return ((StringToken)token).String;
        }

        /// <summary>
        /// Read a token from `inputFile` and check that it is an identifier.
        /// Return the name of the identifier.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns></returns>
        public static string expect_identifier(InputStream inputFile)
        {
            Token token = inputFile.ReadToken();

            if (token is not IdentifierToken)
            {
                throw new GrammarErrorException($"Got '{token}' instead of an identifier", token.Location);
            }

            return ((IdentifierToken)token).Identifier;
        }

        //=============== PARSE ============================

        /// <summary>
        /// Parse symbles and numbers to return a vector of floats
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static Vec parse_vector(InputStream inputFile, Scene scene)
        {
            expect_symbol(inputFile, "[");
            float x = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float y = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float z = expect_number(inputFile, scene);
            expect_symbol(inputFile, "]");

            return new Vec(x, y, z);
        }
        
        /// <summary>
        /// Parse symbles and numbers to return a point of floats
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static Point parse_point(InputStream inputFile, Scene scene)
        {
            expect_symbol(inputFile, "[");
            float x = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float y = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float z = expect_number(inputFile, scene);
            expect_symbol(inputFile, "]");

            return new Point(x, y, z);
        }

        /// <summary>
        /// Parse a <see cref="Color"/> object from a file and store it into memory.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static Color parse_color(InputStream inputFile, Scene scene)
        {
            expect_symbol(inputFile, "<");
            float red = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float green = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float blue = expect_number(inputFile, scene);
            expect_symbol(inputFile, ">");

            return new Color(red, green, blue);
        }

        /// <summary>
        /// Parse a <see cref="Pigment"/> object from a file and store it into memory.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Pigment parse_pigment(InputStream inputFile, Scene scene)
        {

            Pigment result;

            List<KeywordList> mylist = new List<KeywordList>
                { KeywordList.Uniform, KeywordList.Checkered, KeywordList.Image };
            KeywordList keyword = expect_keywords(inputFile, mylist);

            expect_symbol(inputFile, "(");
            if (keyword == KeywordList.Uniform)
            {
                Color color = parse_color(inputFile, scene);
                result = new UniformPigment(color);
            }
            else if (keyword == KeywordList.Checkered)
            {
                Color color1 = parse_color(inputFile, scene);
                expect_symbol(inputFile, ",");
                Color color2 = parse_color(inputFile, scene);
                expect_symbol(inputFile, ",");
                int numOfSteps = (int)expect_number(inputFile, scene);
                result = new CheckeredPigment(color1, color2, numOfSteps);
            }
            else if (keyword == KeywordList.Image)
            {
                string fileName = expect_string(inputFile);
                // don't worry about width & height: they're gonna be overwritten
                HdrImage image = new HdrImage(1, 1);
                using (Stream imageFile = File.OpenRead(fileName))
                {
                    image = image.ReadPFMFile(imageFile);
                    
                }

                result = new ImagePigment(image);
            }
            else
            {
                //assert False, 
                throw new Exception("This line should be unreachable");
            }

            expect_symbol(inputFile, ")");

            return result;
        }

        /// <summary>
        /// Parse a <see cref="BRDF"/> object from a file and store it into memory.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static BRDF parse_brdf(InputStream inputFile, Scene scene)
        {

            List<KeywordList> mylist = new List<KeywordList> { KeywordList.Diffuse, KeywordList.Specular };
            KeywordList brdfKeyword = expect_keywords(inputFile, mylist);

            expect_symbol(inputFile, "(");
            Pigment pigment = parse_pigment(inputFile, scene);
            expect_symbol(inputFile, ")");

            if (brdfKeyword == KeywordList.Diffuse)
                return new DiffuseBRDF(pigment);
            if (brdfKeyword == KeywordList.Specular)
                return new SpecularBRDF(pigment);
            else
            {
                throw new Exception("This line should be unreachable");
            }
        }

        /// <summary>
        /// Parse a <see cref="Material"/> object and its name from a file and store it into memory.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static Tuple<string, Material> parse_material(InputStream inputFile, Scene scene)
        {
            string name = expect_identifier(inputFile);

            expect_symbol(inputFile, "(");
            BRDF brdf = parse_brdf(inputFile, scene);
            expect_symbol(inputFile, ",");
            Pigment emittedRadiance = parse_pigment(inputFile, scene);
            expect_symbol(inputFile, ")");

            return new Tuple<string, Material>(name, new Material(emittedRadiance, brdf));
        }

        /// <summary>
        /// Parse a <see cref="Transformation"/> object from a file and store it into memory.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static Transformation parse_transformation(InputStream inputFile, Scene scene)
        {
            Transformation result = new Transformation();

            while (true)
            {
                List<KeywordList> mylist = new List<KeywordList>
                {
                    KeywordList.Identity,
                    KeywordList.Translation,
                    KeywordList.RotationX,
                    KeywordList.RotationY,
                    KeywordList.RotationZ,
                    KeywordList.Scaling
                };
                KeywordList transformationKw = expect_keywords(inputFile, mylist);

                if (transformationKw == KeywordList.Identity)
                {
                }
                else if (transformationKw == KeywordList.Translation)
                {
                    expect_symbol(inputFile, "(");
                    result *= Transformation.Translation(parse_vector(inputFile, scene));
                    expect_symbol(inputFile, ")");
                }
                else if (transformationKw == KeywordList.RotationX)
                {
                    expect_symbol(inputFile, "(");
                    result *= Transformation.RotationX(expect_number(inputFile, scene));
                    expect_symbol(inputFile, ")");
                }
                else if (transformationKw == KeywordList.RotationY)
                {
                    expect_symbol(inputFile, "(");
                    result *= Transformation.RotationY(expect_number(inputFile, scene));
                    expect_symbol(inputFile, ")");
                }
                else if (transformationKw == KeywordList.RotationZ)
                {
                    expect_symbol(inputFile, "(");
                    result *= Transformation.RotationZ(expect_number(inputFile, scene));
                    expect_symbol(inputFile, ")");
                }
                else if (transformationKw == KeywordList.Scaling)
                {
                    expect_symbol(inputFile, "(");
                    result *= Transformation.Scaling(parse_vector(inputFile, scene));
                    expect_symbol(inputFile, ")");
                }

                // We must peek the next token to check if there is another transformation that is being
                // chained or if the sequence ends.
                // That's why this is a LL(1) parser!
                Token nextKw = inputFile.ReadToken();
                if ((nextKw is not SymbolToken) || ((SymbolToken)nextKw).Symbol != "*")
                {
                    // Pretend you never read this token and put it back!
                    inputFile.UnreadToken(nextKw);
                    break;
                }
                
            }

            return result;
        }

        /// <summary>
        /// Parse a <see cref="Sphere"/> object from a file and store it into memory.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        /// <exception cref="GrammarErrorException"></exception>
        public static Sphere parse_sphere(InputStream inputFile, Scene scene)
        {
            expect_symbol(inputFile, "(");

            string materialName = expect_identifier(inputFile);
            if(! scene.Materials.ContainsKey(materialName))
            {
                // We raise the exception here because inputFile is pointing to the end of the wrong identifier
                throw new GrammarErrorException($"Unknown material {materialName}", inputFile.Location);
            }

            expect_symbol(inputFile, ",");
            Transformation transformation = parse_transformation(inputFile, scene);
            expect_symbol(inputFile, ")");

            return new Sphere(transformation, scene.Materials[materialName]);
        }

        /// <summary>
        /// Parse a <see cref="Cylinder"/> object from a file and store it into memory.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        /// <exception cref="GrammarErrorException"></exception>
        public static Cylinder parse_cylinder(InputStream inputFile, Scene scene)
        {
            expect_symbol(inputFile, "(");
            string materialName = expect_identifier(inputFile);
            if (!scene.Materials.ContainsKey(materialName))
            {
                // We raise the exception here because inputFile is pointing to the end of the wrong identifier
                throw new GrammarErrorException($"Unknown material {materialName}", inputFile.Location);
            }
            expect_symbol(inputFile, ",");
            Transformation transformation = parse_transformation(inputFile, scene);
            expect_symbol(inputFile, ",");
            float zmin = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float zmax = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float r = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float phiMax = expect_number(inputFile, scene);
            expect_symbol(inputFile, ")");
            
            return new Cylinder(transformation, scene.Materials[materialName], zmin, zmax,phiMax, r );
        }

        /// <summary>
        /// Parse a <see cref="Plane"/> object from a file and store it into memory.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        /// <exception cref="GrammarErrorException"></exception>
        public static XyPlane parse_plane(InputStream inputFile, Scene scene)
        {
            XyPlane plane;
            
            expect_symbol(inputFile, "(");
            // material
            string materialName = expect_identifier(inputFile);
            if(! scene.Materials.ContainsKey(materialName))
            {
                // We raise the exception here because inputFile is pointing to the end of the wrong identifier
                throw new GrammarErrorException($"Unknown material {materialName}", inputFile.Location);
            }
            expect_symbol(inputFile, ",");
            // transformation
            Transformation transformation = parse_transformation(inputFile, scene);
            expect_symbol(inputFile, ")");
            
            plane = new XyPlane(transformation, scene.Materials[materialName]);
            return plane;
        }

        public static PointLight parse_pointlight(InputStream inputFile, Scene scene)
        {
            expect_symbol(inputFile, "(");
            Point position = parse_point(inputFile, scene); 
            
            expect_symbol(inputFile, ",");
            Color color = parse_color(inputFile, scene);
            
            expect_symbol(inputFile, ",");
            float linearRadius = expect_number(inputFile, scene);
            
            expect_symbol(inputFile, ")");
            
            return new PointLight(position, color, linearRadius);
        }
        
        /// <summary>
        /// Parse a <see cref="Camera"/> object from a file and store it into memory.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static ICamera parse_camera(InputStream inputFile, Scene scene)
        {
            ICamera result;
            
            expect_symbol(inputFile, "(");
            KeywordList typeKw = expect_keywords(inputFile, 
                new List<KeywordList>{KeywordList.Perspective, KeywordList.Orthogonal});
            expect_symbol(inputFile, ",");
            Transformation transformation = parse_transformation(inputFile, scene);
            expect_symbol(inputFile, ",");
            float aspectRatio = expect_number(inputFile, scene);
            expect_symbol(inputFile, ",");
            float distance = expect_number(inputFile, scene);
            expect_symbol(inputFile, ")");

            if (typeKw == KeywordList.Perspective)
            {
                result = new PerspectiveCamera(distance, aspectRatio, transformation);
            }
            else {
                result = new OrthogonalCamera(aspectRatio, transformation);
            }

            return result;
        }
        
        /// <summary>
        /// Read a scene description from a stream and return a :class:`.Scene` object
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static Scene parse_scene(InputStream inputFile, Dictionary<string, float> variables)
        {
            Scene scene = new Scene();

            try
            {
                scene.FloatVariables = variables;
                scene.OverriddenVariables = variables.Keys;

                while (true)
                {
                    Token what = inputFile.ReadToken();
                    if (what is StopToken)
                        break;
                    if (what is not KeywordToken)
                        throw new GrammarErrorException(msg: $"Parsing, expected a keyword instead of '{what}'", sourceLocation: what.Location);


                    if (((KeywordToken)what).Keyword == KeywordList.Float)
                    {
                        string variableName = expect_identifier(inputFile: inputFile);

                        // Save this for the error message
                        SourceLocation variableLoc = inputFile.Location;

                        expect_symbol(inputFile: inputFile, symbol: "(");
                        float variableValue = expect_number(inputFile: inputFile, scene: scene);
                        expect_symbol(inputFile: inputFile, symbol: ")");

                        if (scene.FloatVariables.ContainsKey(key: variableName) &&
                            !(scene.OverriddenVariables.Contains(value: variableName)))
                        {
                            throw new GrammarErrorException(msg: $"Variable «{variableName}» cannot be redefined",
                                sourceLocation: variableLoc);
                        }

                        if (!scene.OverriddenVariables.Contains(value: variableName))
                        {
                            // Only define the variable if it was not defined by the user OUTSIDE the scene file
                            // (e.g., from the command line)
                            scene.FloatVariables[key: variableName] = variableValue;
                        }
                    }
                    else if (((KeywordToken)what).Keyword == KeywordList.Sphere)
                        scene.World.AddShape(sh: parse_sphere(inputFile: inputFile, scene: scene));
                    else if (((KeywordToken)what).Keyword == KeywordList.Plane)
                        scene.World.AddShape(sh: parse_plane(inputFile: inputFile, scene: scene));
                    else if (((KeywordToken)what).Keyword == KeywordList.Cylinder)
                        scene.World.AddShape(sh: parse_cylinder(inputFile: inputFile, scene: scene));
                    else if (((KeywordToken)what).Keyword == KeywordList.PointLight)
                        scene.World.AddLight(light: parse_pointlight(inputFile: inputFile, scene: scene));
                    else if (((KeywordToken)what).Keyword == KeywordList.Camera)
                    {
                        if (scene.Camera != null)
                            throw new GrammarErrorException(msg: "You cannot define more than one camera", sourceLocation: what.Location);
                        scene.Camera = parse_camera(inputFile: inputFile, scene: scene);
                    }
                    else if (((KeywordToken)what).Keyword == KeywordList.Material)
                    {
                        (string name, Material material) = parse_material(inputFile: inputFile, scene: scene);
                        scene.Materials[key: name] = material;
                    }

                }

            }
            catch (GrammarErrorException grex)
            {
                Console.WriteLine(value: $"Grammar error: {grex.Message} \n   >>> In inputfile (line, row):{grex.SourceLocation}");
            }

            return scene;
            
        }
    }
}






