# HEAD

## Version 2.1.0
- Add verbosity functionality [#45](https://github.com/frasalt/PhotoGENius/pull/45)

## Version 2.0.1

### 21-06-2022
- Documentation improved [#44](https://github.com/frasalt/PhotoGENius/pull/44).
- [Bug](https://github.com/frasalt/PhotoGENius/pull/43/commits/e0aeb3f513ea07ee5ba61d2aef0d4bf6802e1440) fixed in Pointlight Render 

## Version 2.0.0
- **Breaking change**: possibility to save also jpeg output file [PhotoGENius-2.0.0](https://github.com/frasalt/PhotoGENius/releases/tag/v2.0.0).

## Version 1.0.0
- First official release of PhotoGENius: [PhotoGENius-1.0.0](https://github.com/frasalt/PhotoGENius/releases/tag/v1.0.0).

### 18-06-2022
- Better handling of exeptions [#42](https://github.com/frasalt/PhotoGENius/pull/42)

### 17-06-2022
- Fix bug in computation of sphere (u,v) coordinates in issue [#41](https://github.com/frasalt/PhotoGENius/issues/41)
- Update readme [#26](https://github.com/frasalt/PhotoGENius/pull/26)
- Add documentation [#39](https://github.com/frasalt/PhotoGENius/issues/39)
- Fix ImagePigment in issue[#37](https://github.com/frasalt/PhotoGENius/issues/37)

### 16-06-2022
- Test for class `Plane` completed, fixing issue [#31](https://github.com/frasalt/PhotoGENius/issues/31)
- Test for class `World` added, fixing issue [#29](https://github.com/frasalt/PhotoGENius/issues/29)
- Fixed isseus [#33](https://github.com/frasalt/PhotoGENius/issues/33) and [#35](https://github.com/frasalt/PhotoGENius/issues/35) about the class `Cylinder`: the pointlight renderer option is available for it now.

### 14-06-2022
- Pointlight renderer implemented [#23](https://github.com/frasalt/PhotoGENius/pull/23)
- Fixed issue [#22](https://github.com/frasalt/PhotoGENius/commit/c9909fc0577c56fc37eadeb1edb3d4cfbcd37d36), regarding lexer and parser

### 13-06-2022
- **Breaking change**: implemented lexer and parser to read input files containing the scene description [#18](https://github.com/frasalt/PhotoGENius/pull/18)

### 07-06-2022
- Implemented [antialiasing](https://github.com/frasalt/PhotoGENius/pull/19/commits/12c1eceeb410fae05f66a57f0a612d88e4cc119d) to improve images quality.

### 25-05-2022
- **Breaking change**: implement *OnOffRenderer* *FlatRenderer* and *PathTracer* [#11](https://github.com/frasalt/PhotoGENius/pull/11).


## Version 0.2.0
- **Breaking change**: implement `demo` and `pfm2png` commands and implement easy CL interfece to use them.


### 27-04-2022
- Fixed an issue with the correct orientation of the image [#7](https://github.com/frasalt/PhotoGENius/issues/7)

## Version 0.1.0
- First release of the code
