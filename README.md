# AOMR XS script to RM script converter
This has been made with Age of Mythology Retold in mind, have not tested it for any other Age games.

This tool converts any XS trigger code to code that can be used within random maps

### Why?
Random map scripts can not use any trigger logic, hence you need to wrap them within "rmTriggerAddScriptLine" calls to make them work, which is tedious, especially when also including other files. This tool includes all includes and converts all code to a class that can be put anywhere.

### Usage
```cs
//           [input]     [output] [class name]
./XStoRM.exe MyMod_tr.xs MyMod.xs MyMod
```
Will create a file with following content:
```cpp
#if (defined(MyMod_INCLUDE) == false)
#define MyMod_INCLUDE
class MyMod {
void _c(string l = ""){rmTriggerAddScriptLine(l);}
void RegisterTriggers()
{
_c("rule SomeRole");
_c("inactive");
_c("highFrequency");
_c("priority 10");
_c("group curse");
_c("{");
// ...
}
};
#endif
```
### Applying to all random maps
If you wish to apply it to all random maps, you can override the core file used random maps, such as `lib2/rm_core.xs` file with your own mod and add the include on top to your generated file, then call `RegisterTriggers()` in main:
```cpp
include "lib2/MyMod.xs"; // copied it to lib dir

// ...

mutable void main()
{
   // ...

   // I add the following at the end:
   // "MyMod" is the name of our exported class
   MyMod mymod;               
   mymod.RegisterTriggers();
}
```
