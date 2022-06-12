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


using System.Data;
using System.Diagnostics;

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
            return (SourceLocation) MemberwiseClone();
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
        public static Dictionary<string, KeywordList> Dictionary = new Dictionary<string, KeywordList>
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

        public OrthogonalCamera Camera; // ma che tipo di camera? come uso l'equivalente di Union?
        //public PerspectiveCamera camera; // ?

        public Dictionary<string, float> FloatVariables = new Dictionary<string, float>();

        //public DataSet<string> OverriddenVariables = new DataSet<string>(); // come faccio a indicare che devono essere stringhe?
        public DataSet OverriddenVariables = new DataSet(); // come faccio a indicare che devono essere stringhe?
        //public DataTable overridden_variables = new DataTable();
        //overridden_variables.DataType = string;
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
            while (ch is ' ' or '\t' or '\n' or '\r' or '#' or '\0')
            {
                if (ch == '#')
                {
                    //It's a comment! Keep reading until the end of the line (include the case "", the end-of-file)
                    //while (ch != '\r' || ch != '\n' || ch != ' ') // <<<< ma così non è la fine del file!
                    // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                    while (ch is not ('\r' or '\n' or '\0'))
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

        public Token ReadToken()
        {
            if (SavedToken != null)
            {
                Token result = SavedToken;
                SavedToken = null;
                return result;
            }

            SkipWhitespacesAndComments();

            // Now ch does *not* contain a whitespace character.
            var ch = ReadChar();
            if (ch == '\0')
            {
                // No more characters in the file, so return a StopToken
                return new StopToken(Location);
            }
            
            //We first save the position in the stream.
            SourceLocation tokenLocation = Location.ShallowCopy();
            
            //Now, which token begins with the "ch" character?
            char[] SYMB = { '(', ')', '<', '>', '[', ']', '*' };
            char[] op = { '+', '-', '.' };
            if (SYMB.Contains(ch))
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
            throw new GrammarErrorException($"Invalid character {ch}", tokenLocation);
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
        void expect_symbol(InputStream inputFile, string symbol)
        {
            Token token = inputFile.ReadToken();
            
            // if token is NOT a symble or the symble-token does NOT match the symble expected, throw error
            if (token is not SymbolToken || ((SymbolToken)token).Symbol != symbol)
            {
                throw new GrammarErrorException($"Got '{token}' instead of '{symbol}'", token.Location);
            }
        }

        /// <summary>
        /// Read a token from `input_file` and check that it is one of the keywords in `keywords`.
        /// Return the keyword as a class :class:'.KeywordList` object.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        //public
        KeywordList expect_keywords(InputStream inputFile, List<KeywordList> keywords)
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
        /// Read a token from `input_file` and check that it is either a literal number or a variable in `scene`.
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
        /// Read a token from `input_file` and check that it is a literal string.
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
        /// Read a token from `input_file` and check that it is an identifier.
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


        public Color parse_color(InputStream input_file, Scene scene)
        {
            expect_symbol(input_file, "<");
            float red = expect_number(input_file, scene);
            expect_symbol(input_file, ",");
            float green = expect_number(input_file, scene);
            expect_symbol(input_file, ",");
            float blue = expect_number(input_file, scene);
            expect_symbol(input_file, ">");

            return new Color(red, green, blue);
        }
/*
        public Pigment parse_pigment(InputStream input_file, Scene scene)
        {

            Pigment result; //= new Pigment();
            
            List<KeywordList> mylist = new List<KeywordList> {KeywordList.Uniform, KeywordList.Checkered, KeywordList.Image};
            KeywordList keyword = expect_keywords(input_file, mylist);

            expect_symbol(input_file, "(");
            if (keyword == KeywordList.Uniform)
            {
                Color color = parse_color(input_file, scene);
                result = new UniformPigment(color);
            }
            else if (keyword == KeywordList.Checkered)
            {
                Color color1 = parse_color(input_file, scene);
                expect_symbol(input_file, ",");
                Color color2 = parse_color(input_file, scene);
                expect_symbol(input_file, ",");
                int numOfSteps = (int)expect_number(input_file, scene);
                result = new CheckeredPigment(color1, color2, numOfSteps);
            }
            else if (keyword == KeywordList.Image)
            {
                string file_name = expect_string(input_file);
                with open(file_name, "rb") as image_file
                {
                    image = read_pfm_image(image_file);
                }
                result = ImagePigment(image = image);
            }
            else
            {
                assert False, "This line should be unreachable";
            }

            expect_symbol(input_file, ")");
            
            return result;
        }


/*
def parse_brdf(input_file: InputStream, scene: Scene) -> BRDF:
    brdf_keyword = expect_keywords(input_file, [KeywordEnum.DIFFUSE, KeywordEnum.SPECULAR])
    expect_symbol(input_file, "(")
    pigment = parse_pigment(input_file, scene)
    expect_symbol(input_file, ")")

    if brdf_keyword == KeywordEnum.DIFFUSE:
        return DiffuseBRDF(pigment=pigment)
    elif brdf_keyword == KeywordEnum.SPECULAR:
        return SpecularBRDF(pigment=pigment)

    assert False, "This line should be unreachable"


def parse_material(input_file: InputStream, scene: Scene) -> Tuple[str, Material]:
    name = expect_identifier(input_file)

    expect_symbol(input_file, "(")
    brdf = parse_brdf(input_file, scene)
    expect_symbol(input_file, ",")
    emitted_radiance = parse_pigment(input_file, scene)
    expect_symbol(input_file, ")")

    return name, Material(brdf=brdf, emitted_radiance=emitted_radiance)


def parse_transformation(input_file, scene: Scene):
    result = Transformation()

    while True:
        transformation_kw = expect_keywords(input_file, [
            KeywordEnum.IDENTITY,
            KeywordEnum.TRANSLATION,
            KeywordEnum.ROTATION_X,
            KeywordEnum.ROTATION_Y,
            KeywordEnum.ROTATION_Z,
            KeywordEnum.SCALING,
        ])

        if transformation_kw == KeywordEnum.IDENTITY:
            pass  # Do nothing (this is a primitive form of optimization!)
        elif transformation_kw == KeywordEnum.TRANSLATION:
            expect_symbol(input_file, "(")
            result *= translation(parse_vector(input_file, scene))
            expect_symbol(input_file, ")")
        elif transformation_kw == KeywordEnum.ROTATION_X:
            expect_symbol(input_file, "(")
            result *= rotation_x(expect_number(input_file, scene))
            expect_symbol(input_file, ")")
        elif transformation_kw == KeywordEnum.ROTATION_Y:
            expect_symbol(input_file, "(")
            result *= rotation_y(expect_number(input_file, scene))
            expect_symbol(input_file, ")")
        elif transformation_kw == KeywordEnum.ROTATION_Z:
            expect_symbol(input_file, "(")
            result *= rotation_z(expect_number(input_file, scene))
            expect_symbol(input_file, ")")
        elif transformation_kw == KeywordEnum.SCALING:
            expect_symbol(input_file, "(")
            result *= scaling(parse_vector(input_file, scene))
            expect_symbol(input_file, ")")

        # We must peek the next token to check if there is another transformation that is being
        # chained or if the sequence ends. Thus, this is a LL(1) parser.
        next_kw = input_file.read_token()
        if (not isinstance(next_kw, SymbolToken)) or (next_kw.symbol != "*"):
            # Pretend you never read this token and put it back!
            input_file.unread_token(next_kw)
            break

    return result


def parse_sphere(input_file: InputStream, scene: Scene) -> Sphere:
    expect_symbol(input_file, "(")

    material_name = expect_identifier(input_file)
    if material_name not in scene.materials.keys():
        # We raise the exception here because input_file is pointing to the end of the wrong identifier
        raise GrammarError(input_file.location, f"unknown material {material_name}")

    expect_symbol(input_file, ",")
    transformation = parse_transformation(input_file, scene)
    expect_symbol(input_file, ")")

    return Sphere(transformation=transformation, material=scene.materials[material_name])


def parse_plane(input_file: InputStream, scene: Scene) -> Plane:
    expect_symbol(input_file, "(")

    material_name = expect_identifier(input_file)
    if material_name not in scene.materials.keys():
        # We raise the exception here because input_file is pointing to the end of the wrong identifier
        raise GrammarError(input_file.location, f"unknown material {material_name}")

    expect_symbol(input_file, ",")
    transformation = parse_transformation(input_file, scene)
    expect_symbol(input_file, ")")

    return Plane(transformation=transformation, material=scene.materials[material_name])


def parse_camera(input_file: InputStream, scene) -> Camera:
    expect_symbol(input_file, "(")
    type_kw = expect_keywords(input_file, [KeywordEnum.PERSPECTIVE, KeywordEnum.ORTHOGONAL])
    expect_symbol(input_file, ",")
    transformation = parse_transformation(input_file, scene)
    expect_symbol(input_file, ",")
    aspect_ratio = expect_number(input_file, scene)
    expect_symbol(input_file, ",")
    distance = expect_number(input_file, scene)
    expect_symbol(input_file, ")")

    if type_kw == KeywordEnum.PERSPECTIVE:
        result = PerspectiveCamera(screen_distance=distance, aspect_ratio=aspect_ratio, transformation=transformation)
    elif type_kw == KeywordEnum.ORTHOGONAL:
        result = OrthogonalCamera(aspect_ratio=aspect_ratio, transformation=transformation)

    return result


def parse_scene(input_file: InputStream, variables: Dict[str, float] = {}) -> Scene:
    """Read a scene description from a stream and return a :class:`.Scene` object"""
    scene = Scene()
    scene.float_variables = copy(variables)
    scene.overridden_variables = set(variables.keys())

    while True:
        what = input_file.read_token()
        if isinstance(what, StopToken):
            break

        if not isinstance(what, KeywordToken):
            raise GrammarError(what.location, f"expected a keyword instead of '{what}'")

        if what.keyword == KeywordEnum.FLOAT:
            variable_name = expect_identifier(input_file)

            # Save this for the error message
            variable_loc = input_file.location

            expect_symbol(input_file, "(")
            variable_value = expect_number(input_file, scene)
            expect_symbol(input_file, ")")

            if (variable_name in scene.float_variables) and not (variable_name in scene.overridden_variables):
                raise GrammarError(location=variable_loc, message=f"variable «{variable_name}» cannot be redefined")

            if variable_name not in scene.overridden_variables:
                # Only define the variable if it was not defined by the user *outside* the scene file
                # (e.g., from the command line)
                scene.float_variables[variable_name] = variable_value

        elif what.keyword == KeywordEnum.SPHERE:
            scene.world.add_shape(parse_sphere(input_file, scene))
        elif what.keyword == KeywordEnum.PLANE:
            scene.world.add_shape(parse_plane(input_file, scene))
        elif what.keyword == KeywordEnum.CAMERA:
            if scene.camera:
                raise GrammarError(what.location, "You cannot define more than one camera")

            scene.camera = parse_camera(input_file, scene)
        elif what.keyword == KeywordEnum.MATERIAL:
            name, material = parse_material(input_file, scene)
            scene.materials[name] = material

    return scene
         */
    }
}






