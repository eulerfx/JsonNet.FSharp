#I @"..\lib"
#r "Newtonsoft.Json.dll"

#load "OptionConverter.fs"
open Newtonsoft.Json.FSharp

#load "TupleArrayConverter.fs"
open Newtonsoft.Json.FSharp

#load "ListConverter.fs"
open Newtonsoft.Json.FSharp

open System
open Newtonsoft.Json

type HaveAList = {
    Name : string
    Roles : string list
    Prefs : string * Int32
}

let entity = { 
    Name = "Leo G"; 
    Roles = ["admin"];
    Prefs = ("hello",1985)
}

let converters : JsonConverter[] = [| new ListConverter(); new TupleArrayConverter() |]

let json = JsonConvert.SerializeObject(value=entity, converters=converters)

assert (json = """{"Name":"Leo G","Roles":["admin"],"Prefs":["hello",1985]}""")

let back = JsonConvert.DeserializeObject<HaveAList>(value=json, converters=converters)

assert (back = entity)