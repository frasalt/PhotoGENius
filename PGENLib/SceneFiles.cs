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

/*
using System.Net.Mail;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using static System.Data.DataSet;
*/
using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Numerics;


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

        /// <summary>
        /// Constructor
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
        /// Constructor
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
            return (SourceLocation)this.MemberwiseClone();
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

        /// <summary>
        /// Constructor, with a `SourceLocation` object as input parameter
        /// </summary>
        /// <param name="location"></param>
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
        Float = 19,
        Pointlight = 20,
        //Cilinder = 21
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
        /// Constructor
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
        public static Dictionary<string, KeywordList> Dictionary = new Dictionary<string, KeywordList>()
        {

            { "new", KeywordList.New },
            { "material", KeywordList.Material },
            { "plane", KeywordList.Plane },
            { "sphere", KeywordList.Sphere },
            //{  "cylinder" , Keyword.Cylinder},
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
            { "pointlight", KeywordList.Pointlight }

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
    /// A scene read from a scene file
    /// </summary>
    public class Scene
    {
        public Dictionary<string, Material> Materials = new Dictionary<string, Material>();

        public World World;

        public ICamera Camera; 

        public Dictionary<string, float> FloatVariables = new Dictionary<string, float>();

        //public DataSet<string> OverriddenVariables = new DataSet<string>(); // come faccio a indicare che devono essere stringhe?
        public Dictionary<string, float>.KeyCollection OverriddenVariables = 
            new Dictionary<string, float>.KeyCollection(new Dictionary<string, float>()); // come faccio a indicare che devono essere stringhe?
    }


    /// <summary>
    /// A high-level wrapper around a stream, used to parse scene files.
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
        /// Read a new character from the stream
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
            while (ch is ' ' or '\t' or '\n' or '\r' or '#')
            {
                if (ch == '#')
                {
                    //It's a comment! Keep reading until the end of the line (include the case "", the end-of-file)
                    //while (ch != '\r' || ch != '\n' || ch != ' ') // <<<< ma così non è la fine del file!
                    // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                    while (ch != '\r' || ch != '\n' || ch != '\0')
                    {
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
                char[] e = { 'e', 'E' };
                if (Char.IsDigit(ch) || ch == '.' || e.Contains(ch))
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
                return new KeywordToken(tokenLocation, KeywordToken.Dictionary[token]);
            }
            catch (System.Exception)
            {
                return new IdentifierToken(tokenLocation, token);
            }

        }

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

            //At this point we must check what kind of token begins with the "ch" character (which has been
            //put back in the stream with self.unread_char). First, we save the position in the stream.
            //SourceLocation tokenLocation = Location.ShallowCopy();
            char[] SYMB = { '(', ')', '<', '>', '[', ']', '*' };
            char[] op = { '+', '-', '.' };
            if (SYMB.Contains(ch))
            {
                return new SymbolToken(Location, ch.ToString());
            }
            if (ch == '"')
            {
                // A literal string (used for file names)
                return ParseStringToken(Location);
            }
            if (Char.IsDigit(ch) || op.Contains(ch))
            {
                // A floating-point number
                return ParseFloatToken(ch.ToString(), Location);
            }
            if (Char.IsLetter(ch) || ch == '_')
            {
                // Since it begins with an alphabetic character, it must either be a keyword or an identifier
                return ParseKeywordOrIdentifierToken(ch, Location);
            }
            else
            {
                // We got some weird character, like '@` or `&`
                throw new GrammarErrorException($"Invalid character {ch}", Location);
            }

        }
        
        /// <summary>
        /// Make as if `token` were never read from `inputFile`
        /// </summary>
        /// <param name="token"></param>
        public void UnreadToken(Token token)
        {
            // <<<< NON CI PIAE CHE QUESTO ERRORE NON SIA SOTTO CONTROLLO
            Debug.Assert(SavedToken != null);
            SavedToken = token;
        }

    }

    public class ExpectParse
    {
        // << perchè siamo obbligati a wrappare in una classe le expect-functions per farle funzionare?

        //=============== EXPECT ============================

        /// <summary>
        /// Read a token from `inputFile` and check that it matches `symbol`.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="symbol"></param>
        public void expect_symbol(InputStream inputFile, string symbol)
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
        public KeywordList expect_keywords(InputStream inputFile, List<KeywordList> keywords)
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
        public float expect_number(InputStream inputFile, Scene scene)
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
        public string expect_string(InputStream inputFile)
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
        public string expect_identifier(InputStream inputFile)
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
        public Vec parse_vector(InputStream inputFile, Scene scene)
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

        public Color parse_color(InputStream inputFile, Scene scene)
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

        public Pigment parse_pigment(InputStream inputFile, Scene scene)
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
                // non capisco bene cosa intendesse tomasi in queste righe di codice
                throw new Exception("This line should be unreachable");
            }

            expect_symbol(inputFile, ")");

            return result;
        }

        public BRDF parse_brdf(InputStream inputFile, Scene scene)
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
                //assert False, 
                // come prima non capisco bene cosa intendesse tomasi in queste righe di codice
                throw new Exception("This line should be unreachable");
            }
        }

        public Tuple<string, Material> parse_material(InputStream inputFile, Scene scene)
        {
            string name = expect_identifier(inputFile);

            expect_symbol(inputFile, "(");
            BRDF brdf = parse_brdf(inputFile, scene);
            expect_symbol(inputFile, ",");
            Pigment emittedRadiance = parse_pigment(inputFile, scene);
            expect_symbol(inputFile, ")");

            return new Tuple<string, Material>(name, new Material(emittedRadiance, brdf));
        }

        public Transformation parse_transformation(InputStream inputFile, Scene scene)
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
                    continue;// Do nothing (a primitive form of optimization!)
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

        public Sphere parse_sphere(InputStream inputFile, Scene scene)
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

        public XyPlane parse_plane(InputStream inputFile, Scene scene)
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

            return new XyPlane(transformation, scene.Materials[materialName]);
        }
        
        public ICamera parse_camera(InputStream inputFile, Scene scene)
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
            else // (typeKw == KeywordList.Orthogonal)
                 // SIAMO TRANQUILLI: NON È POSSIBILE ARRIVARE FIN QUI CON TYPEKW DIVERSO DA PERSPECTIVE O ORTHOGONAL
            {
                result = new OrthogonalCamera(aspectRatio, transformation);
            }

            return result;
        }

        // public Cylinder parse_cylinder { }

        /// <summary>
        /// Read a scene description from a stream and return a :class:`.Scene` object
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public Scene parse_scene(InputStream inputFile, Dictionary<string, float> variables)
        {
            Scene scene = new Scene();
            scene.FloatVariables = variables;
            scene.OverriddenVariables = variables.Keys;

            while (true)
            {
                Token what = inputFile.ReadToken();
                if (what is StopToken)
                    break;
                if (what is not KeywordToken)
                    throw new GrammarErrorException($"Expected a keyword instead of '{what}'", what.Location);
                if (((KeywordToken)what).Keyword == KeywordList.Float)
                {
                    string variableName = expect_identifier(inputFile);

                    // Save this for the error message
                    SourceLocation variableLoc = inputFile.Location;

                    expect_symbol(inputFile, "(");
                    float variableValue = expect_number(inputFile, scene);
                    expect_symbol(inputFile, ")");

                    if (scene.FloatVariables.ContainsKey(variableName) &&
                        !(scene.OverriddenVariables.Contains(variableName)))
                    {
                        throw new GrammarErrorException($"variable «{variableName}» cannot be redefined", variableLoc);
                    }

                    if (!scene.OverriddenVariables.Contains(variableName))
                    {
                        // Only define the variable if it was not defined by the user OUTSIDE the scene file
                        // (e.g., from the command line)
                        scene.FloatVariables[variableName] = variableValue;
                    }
                }
                else if (((KeywordToken)what).Keyword == KeywordList.Sphere)
                    scene.World.AddShape(parse_sphere(inputFile, scene));
                else if (((KeywordToken)what).Keyword == KeywordList.Plane)
                    scene.World.AddShape(parse_plane(inputFile, scene));
                else if (((KeywordToken)what).Keyword == KeywordList.Camera)
                {
                    if (scene.Camera != null)
                        throw new GrammarErrorException("You cannot define more than one camera", what.Location);
                    scene.Camera = parse_camera(inputFile, scene);
                }

                else if (((KeywordToken)what).Keyword == KeywordList.Material)
                {
                    (string name, Material material) = parse_material(inputFile, scene);
                    scene.Materials[name] = material;
                }
            }

            return scene;
        }
    }
}






